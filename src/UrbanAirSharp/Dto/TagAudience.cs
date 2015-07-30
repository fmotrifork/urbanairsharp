using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UrbanAirSharp.Dto
{
	public class TagAudience : ITagAudience
	{
		[JsonProperty("amazon_channel")]
		public string AmazonChannel { get; set; }

		[JsonProperty("android_channel")]
		public string AndroidChannel { get; set; }

		[JsonProperty("ios_channel")]
		public string IosChannel { get; set; }
	}
}
