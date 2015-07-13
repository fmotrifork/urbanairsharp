// Copyright (c) 2014-2015 Jeff Gosling (jeffery.gosling@gmail.com)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UrbanAirSharp.Dto;
using UrbanAirSharp.Request;
using UrbanAirSharp.Request.Base;
using UrbanAirSharp.Response;
using UrbanAirSharp.Type;

namespace UrbanAirSharp
{
	/// <summary>
	/// A gateway for pushing notifications to the Urban Airship API V3
	/// http://docs.urbanairship.com/reference/api/v3/
	/// 
	/// Supported:
	/// ---------
	/// api/push
	/// api/push/validate
	/// api/schedule 
	/// api/device_tokens 
	/// api/tags 
	/// 
	/// Not Supported Yet:
	/// -----------------
	/// api/feeds 
	/// api/reports 
	/// api/segments 
	/// api/location 
	/// </summary>

	public class UrbanAirSharpGateway
	{
		private static readonly ILog Log;
		static UrbanAirSharpGateway()
		{
			try
			{
				XmlConfigurator.Configure();
			}
			catch { }
			Log = LogManager.GetLogger(typeof(UrbanAirSharpGateway));
		}

		readonly ServiceModelConfig _cfg;

		public UrbanAirSharpGateway() 
			: this(ServiceModelConfig.Instance) { }
		
		public UrbanAirSharpGateway(String appKey, String appMasterSecret) 
			: this(ServiceModelConfig.Create(appKey, appMasterSecret)) { }

		public UrbanAirSharpGateway(ServiceModelConfig cfg)
		{
			_cfg = cfg;
		}

		/// <summary>
		/// - Broadcast to all devices
		/// - Broadcast to one device type
		/// - Send to a targeted device
		/// - Broadcast to all devices with a different alert for each type
		/// </summary>
		/// <param name="alert">The message to be pushed</param>
		/// <param name="deviceTypes">use null for broadcast</param>
		/// <param name="deviceId">use null for broadcast or deviceTypes must contain 1 element that distinguishes this deviceId</param>
		/// <param name="deviceAlerts">per device alert messages and extras</param>
		/// <param name="customAudience">a more specific way to choose the audience for the push. If this is set, deviceId is ignored</param>
		[Obsolete("Please use the other versions of this method.", false)]
		public PushResponse Push(String alert, IEnumerable<DeviceType> deviceTypes = null, String deviceId = null, IEnumerable<BaseAlert> deviceAlerts = null, Audience customAudience = null)
		{
			Push content = BackwardCompatibilityCreate(alert, deviceTypes, deviceId, deviceAlerts, customAudience);
			return Push(content);
        }

		[Obsolete("Please do not use, it is only here to support old Push(...) and Validate(...) methods!")]
		static Push BackwardCompatibilityCreate(String alert, IEnumerable<DeviceType> deviceTypes = null, String deviceId = null, IEnumerable<BaseAlert> deviceAlerts = null, Audience customAudience = null)
		{
			Push content = null;
			if (customAudience != null)
				content = new Push(alert, customAudience, deviceAlerts);
			else if (deviceTypes != null && !string.IsNullOrWhiteSpace(deviceId))
			{
				IEnumerable<Device> devices = from dt in deviceTypes select new Device(deviceId, dt);
				content = new Push(alert, devices, deviceAlerts);
			}
			return content;
		}

		public PushResponse Push(Push content)
		{
			var request = new PushRequest(content, _cfg);
			return SendRequest(request);
		}

		/// <summary>
		/// Validates a push request. Duplicates Push without actually sending the alert. See Push
		/// </summary>
		/// <param name="alert">The message to be pushed</param>
		/// <param name="deviceTypes">use null for broadcast</param>
		/// <param name="deviceId">use null for broadcast or deviceTypes must contain 1 element that distinguishes this deviceId</param>
		/// <param name="deviceAlerts">per device alert messages and extras</param>
		/// <param name="customAudience">a more specific way to choose the audience for the push. If this is set, deviceId is ignored</param>
		[Obsolete("Please use the other versions of this method.", false)]
		public PushResponse Validate(String alert, IEnumerable<DeviceType> deviceTypes = null, String deviceId = null,
			IEnumerable<BaseAlert> deviceAlerts = null, Audience customAudience = null)
		{
			Push content = BackwardCompatibilityCreate(alert, deviceTypes, deviceId, deviceAlerts, customAudience);
			return Validate(content);
		}

		public PushResponse Validate(Push content)
		{
			var request = new PushValidateRequest(content, _cfg);
			return SendRequest(request);
		}

		public ScheduleCreateResponse CreateSchedule(Schedule schedule)
        {
			var request = new ScheduleCreateRequest(schedule, _cfg);
            return SendRequest(request);
        }

		public ScheduleEditResponse EditSchedule(Guid scheduleId, Schedule schedule)
        {
			var request = new ScheduleEditRequest(scheduleId, schedule, _cfg);
            return SendRequest(request);
        }

		public BaseResponse DeleteSchedule(Guid scheduleId)
        {
			var request = new ScheduleDeleteRequest(scheduleId, _cfg);
            return SendRequest(request);
        }

        public ScheduleGetResponse GetSchedule(Guid scheduleId)
        {
			var request = new ScheduleGetRequest(scheduleId, _cfg);
            return SendRequest(request);
        }

        public ScheduleListResponse ListSchedules()
        {
			var request = new ScheduleListRequest(_cfg);
            return SendRequest(request);
        }

		/// <summary>
		/// Registers a device token only with the Urban Airship site, this can be used for new device tokens and for existing tokens.
		/// The existing settings (badge, tags, alias, quiet times) will be overriden. If a token has become inactive reregistering it
		/// will make it active again.
		/// </summary>
		/// <returns>Response from Urban Airship</returns>
		public BaseResponse RegisterDeviceToken(string deviceToken)
		{
			return RegisterDeviceToken(new DeviceToken() {Token = deviceToken});
		}

		/// <summary>
		/// Registers a device token with extended properties with the Urban Airship site, this can be used for new device 
		/// tokens and for existing tokens. If a token has become inactive reregistering it will make it active again. 
		/// </summary>
		/// <returns>Response from Urban Airship</returns>
		public BaseResponse RegisterDeviceToken(DeviceToken deviceToken)
		{
			if (string.IsNullOrEmpty(deviceToken.Token))
				throw new ArgumentException("A device Tokens Token field is Required", "deviceToken");

			var request = new DeviceTokenRequest(deviceToken, _cfg);
            return SendRequest(request);
		}

		public BaseResponse CreateTag(Tag tag)
		{
			if (string.IsNullOrEmpty(tag.TagName))
				throw new ArgumentException("A tag name is Required", "tag.TagName");

			var request = new TagCreateRequest(tag, _cfg);
            return SendRequest(request);
		}

		public BaseResponse DeleteTag(string tag)
		{
			if (string.IsNullOrEmpty(tag))
				throw new ArgumentException("A tag is Required", "tag");

			var request = new TagDeleteRequest(tag, _cfg);
            return SendRequest(request);
		}

		public TagListResponse ListTags()
		{
			var request = new TagListRequest(_cfg);
            return SendRequest(request);
		}

		static TResponse SendRequest<TResponse>(BaseRequest<TResponse> request) where TResponse : BaseResponse, new()
		{
			try
			{
				var requestTask = request.ExecuteAsync();

				return requestTask.Result;
			}
			catch (Exception e)
			{
				Log.Error(request.GetType().FullName, e);

				return new TResponse()
				{
					Error = e.InnerException != null ? e.InnerException.Message : e.Message,
					Ok = false
				};
			}
		}

	}
}
