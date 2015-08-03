using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;
using UrbanAirSharp.Dto;
using UrbanAirSharp.Type;

namespace UrbanAirSharp.Tests.Dto
{
    [TestFixture]
    public class FieldSerializationTests
    {
        static readonly JsonSerializerSettings _jss;
        
        static FieldSerializationTests()
        {
            _jss = ServiceModelConfig.Instance.SerializerSettings;
        }

		[TestCaseSource("TagOperationTestValues")]
		[TestCaseSource("ITagAudienceTestValues")]
		[TestCaseSource("AudienceBuilderTestValues")]
		[TestCaseSource("AudienceTestValues")]
        [TestCaseSource("PushTestValues")]
        public void FieldValue(object o, string field, object expectedValue)
        {
            string json = JsonConvert.SerializeObject(o, _jss);
            Assert.That(!string.IsNullOrWhiteSpace(json));

            JObject jo = JObject.Parse(json);
            Assert.IsNotNull(jo);

            JToken tk = jo.SelectToken("$." + field);
			if (expectedValue == null)
				Assert.IsNull(tk);
			else
			{
				Assert.IsNotNull(tk);

				string jsonVal = tk.ToString(Formatting.None, _jss.Converters.ToArray());
				string expected = JsonConvert.SerializeObject(expectedValue, _jss);
				Assert.AreEqual(expected, jsonVal);
			}
        }

		#region test data generator

		public static IEnumerable<TestCaseData> PushTestValues
        {
            get
            {
                yield return new TestCaseData(new Push("testing"), "audience", "all");
                yield return new TestCaseData(new Push("w00t"), "device_types", "all");
                yield return new TestCaseData(new Push("abc"), "notification.alert", "abc");

                var dts = new[] { DeviceType.Ios, DeviceType.Android };
                yield return new TestCaseData(new Push("both") { DeviceTypes = dts }, "device_types", dts);

                yield return new TestCaseData(new Push("ios") { DeviceTypes = new[] { DeviceType.Ios } }, "device_types", "ios");

				var au = new Audience(AudienceType.Tag, "test");
				yield return new TestCaseData(new Push("blah", au), "audience", au);
            }
        }

		public static IEnumerable<TestCaseData> AudienceTestValues
		{
			get
			{
				var ios1 = new Audience(AudienceType.Ios, "ioskey");
                yield return new TestCaseData(ios1, "device_token", "ioskey");
				yield return new TestCaseData(ios1, "ios_channel", null);

				var ios2 = new Audience(AudienceType.Ios, "ioschannel1", true);
                yield return new TestCaseData(ios2, "ios_channel", "ioschannel1");
				yield return new TestCaseData(ios2, "device_token", null);

				var android1 = new Audience(AudienceType.Android, "blah");
                yield return new TestCaseData(android1, "apid", "blah");
				yield return new TestCaseData(android1, "android_channel", null);

				var android2 = new Audience(AudienceType.Android, "blah2", true);
                yield return new TestCaseData(android2, "android_channel", "blah2");
				yield return new TestCaseData(android2, "apid", null);

				yield return new TestCaseData(new Audience(AudienceType.Windows, "zzz"), "wns", "zzz");
				yield return new TestCaseData(new Audience(AudienceType.Blackberry, "1234"), "device_pin", "1234");
				yield return new TestCaseData(new Audience(AudienceType.Segment, "!!!"), "segment", "!!!");
				yield return new TestCaseData(new Audience(AudienceType.Alias, "$$$"), "alias", "$$$");
				yield return new TestCaseData(new Audience(AudienceType.Tag, "-?-"), "tag", "-?-");
			}
		}

		public static IEnumerable<TestCaseData> AudienceBuilderTestValues
		{
			get
			{
				var ios = new Audience(AudienceType.Ios, "ioskey");
				var android = new Audience(AudienceType.Android, "blah");
				var windoze = new Audience(AudienceType.Windows, "zzz");
				var bb = new Audience(AudienceType.Blackberry, "1234");
				var seg = new Audience(AudienceType.Segment, "!!!");
				var alias = new Audience(AudienceType.Alias, "$$$");
				var tag = new Audience(AudienceType.Tag, "-?-");

				yield return new TestCaseData(android.And(ios), "AND", new[] { android, ios });
				yield return new TestCaseData(android & ios, "AND", new[] { android, ios });

				yield return new TestCaseData(android.Or(ios), "OR", new[] { android, ios });
				yield return new TestCaseData(android | ios, "OR", new[] { ios, android });

				yield return new TestCaseData(android.Not(), "NOT", android);
				yield return new TestCaseData(!ios, "NOT", ios);

				yield return new TestCaseData(android.And(tag).Not(), "NOT", android & tag);
			}
		}

		public static IEnumerable<TestCaseData> ITagAudienceTestValues
		{
			get
			{
				var ta = new TagAudience
				{
					AmazonChannel = Guid.NewGuid().ToString(),
					AndroidChannel = Guid.NewGuid().ToString(),
					IosChannel = Guid.NewGuid().ToString(),
				};
				yield return new TestCaseData(ta, "amazon_channel", ta.AmazonChannel);
				yield return new TestCaseData(ta, "android_channel", ta.AndroidChannel);
				yield return new TestCaseData(ta, "ios_channel", ta.IosChannel);

				var tab = new TagAudienceBatch
				{
					AmazonChannels = new[] { Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() },
					AndroidChannels = new[] { Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() },
					IosChannels = new[] { Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() },
				};
				yield return new TestCaseData(tab, "amazon_channel", tab.AmazonChannels);
				yield return new TestCaseData(tab, "android_channel", tab.AndroidChannels);
				yield return new TestCaseData(tab, "ios_channel", tab.IosChannels);
			}
		}

		public static IEnumerable<TestCaseData> TagOperationTestValues
		{
			get
			{
				var ta = new TagAudience
				{
					AmazonChannel = Guid.NewGuid().ToString(),
					AndroidChannel = Guid.NewGuid().ToString(),
					IosChannel = Guid.NewGuid().ToString(),
				};
				yield return new TestCaseData(new TagSet(ta), "audience", ta);
				yield return new TestCaseData(new TagAddRemove(ta), "audience", ta);


				var tab = new TagAudienceBatch
				{
					AmazonChannels = new[] { Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() },
					AndroidChannels = new[] { Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() },
					IosChannels = new[] { Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() },
				};
				var op = new TagAddRemove(tab) { Add = new Dictionary<string, ICollection<string>> { { "blah", new[] { "test" } } } };
                yield return new TestCaseData(op, "audience", tab);
				yield return new TestCaseData(op, "set", null);
				//yield return new TestCaseData(op, "remove", null);
				
				yield return new TestCaseData(op, "add.blah[0]", "test");
			}
		}

		#endregion

	}
}