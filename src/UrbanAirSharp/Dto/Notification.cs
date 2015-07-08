// Copyright (c) 2014-2015 Jeff Gosling (jeffery.gosling@gmail.com)
using System;
using Newtonsoft.Json;

namespace UrbanAirSharp.Dto
{
	public class Notification
	{
		string _default;
		[JsonProperty("alert", Required = Required.Always)]
		public String DefaultAlert
		{
			get { return _default; }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentException("DefaultAlert can not be null or empty!");

				_default = value;
			}
		}

		[JsonProperty("android")]
		public AndroidAlert AndroidAlert { get; set; }

		[JsonProperty("ios")]
		public IosAlert IosAlert { get; set; }

		[JsonProperty("wns")]
		public WindowsAlert WindowsAlert { get; set; }

		[JsonProperty("mpns")]
		public WindowsPhoneAlert WindowsPhoneAlert { get; set; }

		[JsonProperty("blackberry")]
		public BlackberryAlert BlackberryAlert { get; set; }
	}
}
