// Copyright (c) 2014-2015 Jeff Gosling (jeffery.gosling@gmail.com)
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
		[JsonProperty("apid")]
		public String AndroidDeviceId { get; private set; }

		[JsonProperty("device_token")]
		public String IosDeviceId { get; private set; }

		[JsonProperty("wns")]
		public String WindowsId { get; private set; }

		[JsonProperty("mpns")]
		public String WindowsPhoneId { get; private set; }

		[JsonProperty("device_pin")]
		public String BlackberryId { get; private set; }

		[JsonProperty("segment")]
		public String SegmentId { get; private set; }

		[JsonProperty("alias")]
		public String Alias { get; private set; }

		[JsonProperty("tag")]
		public String Tag { get; private set; }

		public Audience(AudienceType type, String value)
		{
			switch (type)
			{
				case AudienceType.Android:
					AndroidDeviceId = value;
					break;
				case AudienceType.Ios:
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
