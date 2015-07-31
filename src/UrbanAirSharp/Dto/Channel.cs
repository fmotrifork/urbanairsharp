using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UrbanAirSharp.Type;

namespace UrbanAirSharp.Dto
{
	public class Channel
	{
		[JsonProperty("channel_id")]
		public string ChannelId { get; set; }

		[JsonProperty("device_type")]
		public DeviceType DeviceType { get; set; }

		[JsonProperty("installed")]
		public bool IsInstalled { get; set; }

		[JsonProperty("background")]
		public bool IsBackground { get; set; }

		[JsonProperty("opt_in")]
		public bool IsOptIn { get; set; }

		[JsonProperty("push_address")]
		public string PushAddress { get; set; }

		[JsonProperty("created")]
		public DateTime Created { get; set; }

		/// <summary>
		/// Last time this device registered
		/// </summary>
		[JsonProperty("last_registration")]
		public DateTime Registration { get; set; }

		[JsonProperty("alias")]
		public string Alias { get; set; }

		readonly ICollection<string> _tags = new HashSet<string>();
		[JsonProperty("tags", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
		public ICollection<string> Tags
		{
			get { return _tags; }
			set
			{
				if (value != null && object.ReferenceEquals(value, this))
					return;

				_tags.Clear();
				if (value != null && value.Count > 0)
				{
					foreach (string t in value)
					{
						_tags.Add(t);
					}
				}
			}
		}

		[JsonProperty("tag_groups", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
		public IDictionary<string, ICollection<string>> TagGroups { get; set; }

		[JsonProperty("ios")]
		public DeviceToken IOS { get; set; }
	}
}
