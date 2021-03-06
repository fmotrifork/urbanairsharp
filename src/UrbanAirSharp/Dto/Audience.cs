﻿// Copyright (c) 2014-2015 Jeff Gosling (jeffery.gosling@gmail.com)
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UrbanAirSharp.Type;

namespace UrbanAirSharp.Dto
{
	/// <summary>
	/// Atomic audience selector
	/// </summary>
	public class Audience : AudienceBase, IAudience
	{
		/// <summary>
		/// apid
		/// </summary>
		[JsonProperty("apid", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public String AndroidDeviceId { get; private set; }

		[JsonProperty("android_channel", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string AndroidChannel { get; private set; }

		/// <summary>
		/// Device token
		/// </summary>
		[JsonProperty("device_token", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public String IosDeviceId { get; private set; }

		[JsonProperty("ios_channel", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string IosChannel { get; private set; }

		[JsonProperty("wns", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public String WindowsId { get; private set; }

		[JsonProperty("mpns", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public String WindowsPhoneId { get; private set; }

		[JsonProperty("device_pin", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public String BlackberryId { get; private set; }

		[JsonProperty("segment", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public String SegmentId { get; private set; }

		[JsonProperty("alias", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public String Alias { get; private set; }

		[JsonProperty("tag", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public String Tag { get; private set; }

		public Audience(AudienceType type, string value, bool isChannel = false)
		{
			switch (type)
			{
				case AudienceType.Android:
					if (isChannel)
						AndroidChannel = value;
					else
						AndroidDeviceId = value;
					break;
				case AudienceType.Ios:
					if (isChannel)
						IosChannel = value;
					else
						IosDeviceId = value;
					break;
				case AudienceType.Windows:
					WindowsId = value;
					break;
				case AudienceType.WindowsPhone:
					WindowsPhoneId = value;
					break;
				case AudienceType.Blackberry:
					BlackberryId = value;
					break;
				case AudienceType.Segment:
					SegmentId = value;
					break;
				case AudienceType.Alias:
					Alias = value;
					break;
				case AudienceType.Tag:
					Tag = value;
					break;
			}
		}

		public static IAudience operator &(Audience a, IAudience b)
		{
			if (a != null)
				return a.And(b);

			return b;
		}
	}
}
