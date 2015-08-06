// Copyright (c) 2014-2015 Jeff Gosling (jeffery.gosling@gmail.com)
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UrbanAirSharp.Dto
{
	public class IosAlert : BaseAlert
	{
		static readonly IComparable BADGE_DEFAULT = "auto";
		IComparable _badge = BADGE_DEFAULT;
		[JsonProperty("badge")]
		public IComparable Badge
		{
			get { return _badge; }
			set
			{
				IComparable v = null;
				if(value != null)
				{
					if (value is string)
					{
						string s = value as string;
						if (!string.IsNullOrWhiteSpace(s))
							v = s.Trim();
					}
					else if(value is int || value is long || value is short || value is byte || value is uint || value is ulong || value is ushort || value is sbyte)
						v = value;
					else if(value is double || value is float || value is decimal)
					{
						long d = (long)Math.Floor(Convert.ToDouble(value));
						v = d;
					}
				}
				_badge = v ?? BADGE_DEFAULT;
			}
		}

		[JsonProperty("sound")]
		public String Sound { get; set; }

		[JsonProperty("content_available")]
		public bool ContentAvailable { get; set; }

		[JsonProperty("expiry")]
		public int ApnsTimeToLive { get; set; }

		[JsonProperty("priority")] 
		public int Priority = 10;

		[JsonProperty("extra")]
		public IDictionary<String, String> Extras { get; set; }
	}
}