// Copyright (c) 2014-2015 Jeff Gosling (jeffery.gosling@gmail.com)

using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using UrbanAirSharp.Type;

namespace UrbanAirSharp.Dto
{
	/// <summary>
	/// Used to form a PUSH request
	/// Send a push notification to a specified device or list of devices
	/// 
	/// audience - Required
	/// notification - Required
	/// device_types - Required
	/// 
	/// options - optionally specify an expiry date
	/// actions - TODO not supported yet 
	/// message - RICH PUSH message - TODO not supported yet
	/// 
	/// http://docs.urbanairship.com/reference/api/v3/push.html
	/// </summary>
	public class Push
	{
		#region CTOR & helpers

		public Push(String alert, IEnumerable<BaseAlert> deviceAlerts = null)
		{
			Notification = new Notification { DefaultAlert = alert };

			if (deviceAlerts != null && deviceAlerts.Count() > 0)
			{
				Notification.AndroidAlert = deviceAlerts.FirstOrDefault(x => x is AndroidAlert) as AndroidAlert;
				Notification.IosAlert = deviceAlerts.FirstOrDefault(x => x is IosAlert) as IosAlert;
				Notification.WindowsAlert = deviceAlerts.FirstOrDefault(x => x is WindowsAlert) as WindowsAlert;
				Notification.WindowsPhoneAlert = deviceAlerts.FirstOrDefault(x => x is WindowsPhoneAlert) as WindowsPhoneAlert;
				Notification.BlackberryAlert = deviceAlerts.FirstOrDefault(x => x is BlackberryAlert) as BlackberryAlert;
			}
		}

		public Push(String alert, IAudience audience, IEnumerable<BaseAlert> deviceAlerts = null) 
			: this(alert, deviceAlerts)
		{
			Audience = audience;			
		}

		public Push(String alert, Device device, IEnumerable<BaseAlert> deviceAlerts = null) 
			: this(alert, new[] { device }, deviceAlerts) { }

		public Push(String alert, IEnumerable<Device> devices, IEnumerable<BaseAlert> deviceAlerts = null)
			: this(alert, CreteAudience(devices), deviceAlerts) { }

		static IAudience CreteAudience(IEnumerable<Device> devices)
		{
			if (devices == null || devices.Count() == 0)
				throw new ArgumentException("deviceAlerts can not be null or empty");

			var audiences = new List<Audience>();
			foreach (Device d in devices)
			{
				if (d == null)
					continue;

				audiences.Add(MakeAudience(d.Id, d.Type));
			}
			return new AudienceOr { Audiences = audiences };
		}

		static Audience MakeAudience(String id, DeviceType dt)
		{
			switch(dt)
			{
				case DeviceType.Android:
					return new Audience(AudienceType.Android, id);
				case DeviceType.Ios:
					return new Audience(AudienceType.Ios, id);
				case DeviceType.Wns:
					return new Audience(AudienceType.Windows, id);
				case DeviceType.Mpns:
					return new Audience(AudienceType.WindowsPhone, id);
				case DeviceType.Blackberry:
					return new Audience(AudienceType.Blackberry, id);
				default:
					throw new ArgumentException("DeviceType dt " + dt + " is not currently supported");
			}
		}

		#endregion

		Notification _notify;

		[JsonProperty("notification", Required = Required.Always)]
		public virtual Notification Notification
		{
			get { return _notify; }
			protected set
			{
				if (value == null)
					throw new ArgumentNullException("Notification");

				_notify = value;
            }
		}

		//IAudience _audience;

		[JsonProperty("audience", NullValueHandling = NullValueHandling.Ignore)]
		public virtual IAudience Audience
		{
			get; set;
			//get
			//{
			//	if (_audience == null)
			//		return "all";

			//	return _audience;
			//}
			//protected set
			//{
			//	_audience = value as IAudience;
			//}
		}

		[JsonProperty("audience", NullValueHandling = NullValueHandling.Ignore)]
		public virtual string AllAudience
		{
			get { return Audience == null ? "all" : null; }
			//set
			//{
			//	if (!string.IsNullOrEmpty(value) && string.Compare(value, "all", true) == 0)
			//		Audience = null;
			//}
		}

		//IEnumerable<DeviceType> _deviceTypes;

		[JsonIgnore]
		public virtual IEnumerable<DeviceType> DeviceTypes
		{
			get; set;

			//get 
			//{
			//	if (_deviceTypes == null)
			//		return "all";
					
			//	return _deviceTypes;
			//}
			//protected set
			//{
			//	var list = value as IList<DeviceType>;
			//	_deviceTypes = list;
			//}
		}

		[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
		[JsonProperty("device_types", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
		public virtual dynamic DeviceSet
		{
			get
			{
				if (DeviceTypes == null)
					return DeviceType.All;
				else if (DeviceTypes.Count() > 1)
					return DeviceTypes;
				else
					return DeviceTypes.FirstOrDefault();
			}
		}

		[JsonProperty("options")]
		public Options Options { get; set; }

		//TODO: not implemented yet
		[JsonProperty("actions")]
		public Actions Actions { get; private set; }

		//TODO: not implemented yet
		[JsonProperty("message")]
		public RichMessage RichMessage { get; private set; }
	}
}
