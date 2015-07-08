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

		internal Push(String alert) //shouldbe locked down as this will not generate a push.  The only public CTORs exposed are the one with valid use-case.
		{
			Notification = new Notification { DefaultAlert = alert };
		}

		public Push(String alert, Audience audience, IEnumerable<BaseAlert> deviceAlerts = null) 
			: this(alert, new[] { audience }, deviceAlerts) { }

		public Push(String alert, IEnumerable<Audience> audiences, IEnumerable<BaseAlert> deviceAlerts = null) 
			: this(alert)
		{
			if (audiences == null || audiences.Count() == 0)
				throw new ArgumentException("audiences can not be null or empty");

			IList<Audience> arr = (from a in audiences where a != null select a).ToArray();
			if (arr.Count == 0)
				throw new ArgumentException("devices contains no valid type that can be mapped to proper audiences");

			if (arr.Count == 1)
				Audience = arr.First();
			else
			{
				var au = new Audience();
				au.OrAudience(arr);
				Audience = au;
			}

			if (deviceAlerts != null)
			{
				Notification.AndroidAlert = deviceAlerts.FirstOrDefault(x => x is AndroidAlert) as AndroidAlert;
				Notification.IosAlert = deviceAlerts.FirstOrDefault(x => x is IosAlert) as IosAlert;
				Notification.WindowsAlert = deviceAlerts.FirstOrDefault(x => x is WindowsAlert) as WindowsAlert;
				Notification.WindowsPhoneAlert = deviceAlerts.FirstOrDefault(x => x is WindowsPhoneAlert) as WindowsPhoneAlert;
				Notification.BlackberryAlert = deviceAlerts.FirstOrDefault(x => x is BlackberryAlert) as BlackberryAlert;
			}
		}

		public Push(String alert, IEnumerable<Device> devices, IEnumerable<BaseAlert> deviceAlerts = null)
			: this(alert, CreteAudience(devices), deviceAlerts) { }

		static IEnumerable<Audience> CreteAudience(IEnumerable<Device> devices)
		{
			if (devices == null || devices.Count() == 0)
				throw new ArgumentException("deviceAlerts can not be null or empty");

			var audiences = new List<Audience>();
			foreach (Device d in devices)
			{
				if (d == null)
					continue;

				audiences.AddRange(CreteAudience(d.Id, d.Type));
			}
			return audiences;
		}

		static IEnumerable<Audience> CreteAudience(String id, DeviceType dt)
		{
			var audiences = new List<Audience>();

			if (dt.HasFlag(DeviceType.Android))
				audiences.Add(new Audience(AudienceType.Android, id));
			if (dt.HasFlag(DeviceType.Ios))
				audiences.Add(new Audience(AudienceType.Ios, id));
			if (dt.HasFlag(DeviceType.Wns))
				audiences.Add(new Audience(AudienceType.Windows, id));
			if (dt.HasFlag(DeviceType.Mpns))
				audiences.Add(new Audience(AudienceType.WindowsPhone, id));
			if (dt.HasFlag(DeviceType.Blackberry))
				audiences.Add(new Audience(AudienceType.Blackberry, id));

			return audiences;
		}

		#endregion

		Notification _notify;

		[JsonProperty("notification", Required = Required.Always)]
		public Notification Notification
		{
			get { return _notify; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("Notification");

				_notify = value;
            }
		}

		Audience _audience;

		[JsonProperty("audience", Required = Required.Always)]
		public object Audience
		{
			get
			{
				if (_audience == null)
				{
					return "all";
				}

				return _audience;
			}
			set
			{
				var audience = value as Audience;
				_audience = audience;
			}
		}

		IList<DeviceType> _deviceTypes;

		[JsonProperty("device_types", Required = Required.Always)]
		public object DeviceTypes {
			get 
			{
				if (_deviceTypes == null)
				{
					return "all";
				}
					
				return _deviceTypes;
			}
			set
			{
				var list = value as IList<DeviceType>;
				_deviceTypes = list;
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

		//public void SetAudience(AudienceType audienceType, String value)
		//{
		//	Audience = new Audience(audienceType, value);
		//}
	}
}
