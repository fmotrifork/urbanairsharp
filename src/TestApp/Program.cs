// Copyright (c) 2014-2015 Jeff Gosling (jeffery.gosling@gmail.com)
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrbanAirSharp;
using UrbanAirSharp.Dto;
using UrbanAirSharp.Type;

namespace TestApp
{
    class Program
    {
		private const String AppKey = "YOUR_URBAN_AIRSHIP_APP_KEY";
		private const String AppMasterSecret = "YOUR_URBAN_AIRSHIP_APP_MASTER_SECRET";
	    
		private const String TestDeviceToken = "YOUR_TEST_DEVICE_DEVICE_TOKEN";
		private const String TestDeviceGuid = "YOUR_TEST_DEVICE_DEVICE_GUID"; //Example: "946fdc3d-0284-468f-a2f7-d007ed694908"

	    private static UrbanAirSharpGateway _urbanAirSharpGateway;

        static void Main(String[] args)
        {
            _urbanAirSharpGateway = new UrbanAirSharpGateway(AppKey, AppMasterSecret);

            var t = Task.Run(() =>
            {
                TestValidate();
                TestPush();
                TestRegisterDevice();
                TestSchedules();
                TestTags();

            });
            t.Wait();
            

            Console.ReadLine();
        }

		private static async Task TestValidate()
		{
			Console.WriteLine("================ TESTING VALIDATE ================");
			Console.WriteLine();

			var response = await _urbanAirSharpGateway.Validate(new Push(new Device(TestDeviceGuid, DeviceType.Android), "Validate push"));

			Console.Write(response.HttpResponseCode + " - ");
			Console.WriteLine(response.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();
		}

		private static async Task TestPush()
		{
			Console.WriteLine("================ TESTING PUSH ================");
			Console.WriteLine();

			Console.WriteLine("PUSH Broadcast Alert");
			var response = await _urbanAirSharpGateway.Push(new Push("Broadcast Alert"));
			Console.Write(response.HttpResponseCode + " - ");
			Console.WriteLine(response.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();

			Console.WriteLine("PUSH Broadcast Alert to Androids");
			response = await _urbanAirSharpGateway.Push(new Push("Broadcast Alert to Androids") { DeviceTypes = new[] { DeviceType.Android } });
			Console.Write(response.HttpResponseCode + " - ");
			Console.WriteLine(response.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();

			Console.WriteLine("PUSH Targeted Alert to device");
			response = await _urbanAirSharpGateway.Push(new Push(new Device("android-id-blah-blah", DeviceType.Android), "Targeted Alert to device"));
            Console.Write(response.HttpResponseCode + " - ");
			Console.WriteLine(response.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();

			Console.WriteLine("PUSH Custom Alert per device type");
			response = await _urbanAirSharpGateway.Push(new Push("Custom Alert per device type", new[]
            {
                new AndroidAlert
                {
                    Alert = "Custom Android Alert",
                    CollapseKey = "Collapse_Key",
                    DelayWhileIdle = true,
                    GcmTimeToLive = 5
                }
            }));
			Console.Write(response.HttpResponseCode + " - ");
			Console.WriteLine(response.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();

			//these are just examples of tags
			var rugbyFanAudience = new Audience(AudienceType.Tag, "Rugby Fan");
			var footballFanAudience = new Audience(AudienceType.Tag, "Football Fan");
			var notFootballFanAudience = new AudienceNot { Audience = footballFanAudience };
			var newZealandAudience = new Audience(AudienceType.Alias, "NZ");
			var englishAudience = new Audience(AudienceType.Tag, "language_en");

			var fansAudience = englishAudience.Or(new IAudience[] { rugbyFanAudience, notFootballFanAudience });

			var customAudience = rugbyFanAudience.And(new IAudience[] { fansAudience, !newZealandAudience, !englishAudience });
			var customAudience2 = rugbyFanAudience & fansAudience & !newZealandAudience & !englishAudience;

			Console.WriteLine("PUSH to custom Audience");
			response = await _urbanAirSharpGateway.Push(new Push(customAudience, "Rugby fans that's not English or NewZealanders"));

			Console.Write(response.HttpResponseCode + " - ");
			Console.WriteLine(response.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();
		}

		private static async Task TestRegisterDevice()
	    {
			Console.WriteLine("================ TESTING REGISTERING A DEVICE TOKEN ================");
			Console.WriteLine();
			var response = await _urbanAirSharpGateway.RegisterDeviceToken(TestDeviceToken);
			Console.WriteLine("Register Device Response: Ok?: {0}   Message: {1}  ErrorCode: {2}  ErrorMessage: {3}", 
				response.Ok, 
				response.Message, 
				response.ErrorCode, 
				response.Error);

			Console.Write(response.HttpResponseCode + " - ");
			Console.WriteLine(response.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();
	    }

		private static async Task TestTags()
		{
			Console.WriteLine("================ TESTING TAGS ================");
			Console.WriteLine();

			const string testTag = "some_tag";

			var tag = new Tag
			{
				TagName = testTag,
				AndroidChannels = new AddRemoveList
				{
					Add = new[] { "TEST_ANDROID_CHANNEL" }
				}
			};

			Console.WriteLine("CREATE TAG:");
			var response = await _urbanAirSharpGateway.CreateTag(tag);
			Console.Write(response.HttpResponseCode + " - ");
			Console.WriteLine(response.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();

			Console.WriteLine("LIST TAGS:");
			var listResponse = await _urbanAirSharpGateway.ListTags();
			Console.Write(listResponse.HttpResponseCode + " - ");
			Console.WriteLine(listResponse.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();

			Console.WriteLine("DELETE TAG:");
			response = await _urbanAirSharpGateway.DeleteTag(testTag);
			Console.Write(response.HttpResponseCode + " - ");
			Console.WriteLine(response.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();

			var client = new UrbanAirSharpGateway(AppKey, AppMasterSecret);
			var compoundAudience = new Audience(AudienceType.Android, "") & new Audience(AudienceType.Blackberry, "") & new Audience(AudienceType.Android, "");
			compoundAudience &= new Audience(AudienceType.Android, "");

			var moreAudience = new AudienceAnd { Audiences = new[] { new Audience(AudienceType.Tag, ""), new Audience(AudienceType.Tag, "") } };
			compoundAudience &= moreAudience;

			var message = new Push(compoundAudience, "What's up");

			await client.Push(new Push("Custom Android Alert per device type", new[]
			{
				new AndroidAlert()
				{
					Alert = "Custom Android Alert",
					CollapseKey = "Collapse_Key",
					DelayWhileIdle = true,
					GcmTimeToLive = 5
				}
			}));
		}

		private static async Task TestSchedules()
		{
			Console.WriteLine("================ TESTING SCHEDULES ================");
			Console.WriteLine();

			var schedule = new Schedule
			{
				Name = "TEST_SCHEDULE",
				ScheduleInfo = new ScheduleInfo
				{
					ScheduleTime = DateTime.Now.AddMinutes(5)
				},
				Push = new Push("Scheduled Push"),
			};

			Console.WriteLine("CREATE SCHEDULE:");
			var createResponse = await _urbanAirSharpGateway.CreateSchedule(schedule);
			Console.Write(createResponse.HttpResponseCode + " - ");
			Console.WriteLine(createResponse.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();

			Console.WriteLine("LIST SCHEDULES:");
			var listResponse = await _urbanAirSharpGateway.ListSchedules();
			Console.Write(listResponse.HttpResponseCode + " - ");
			Console.WriteLine(listResponse.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();

			var scheduleId = Guid.NewGuid();

			if (createResponse.Ok && createResponse.Schedules.Count > 0)
			{
				scheduleId = createResponse.Schedules[0].Id;
			}

			Console.WriteLine("GET SCHEDULE:");
			var getResponse = await _urbanAirSharpGateway.GetSchedule(scheduleId);
			Console.Write(getResponse.HttpResponseCode + " - ");
			Console.WriteLine(getResponse.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();

			Console.WriteLine("DELETE SCHEDULE:");
			var deleteResponse = await _urbanAirSharpGateway.DeleteSchedule(scheduleId);
			Console.Write(deleteResponse.HttpResponseCode + " - ");
			Console.WriteLine(deleteResponse.Ok ? "SUCCESS" : "FAILED");
			Console.WriteLine();
		}
    }
}
