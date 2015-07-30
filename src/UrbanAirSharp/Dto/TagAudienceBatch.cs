using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UrbanAirSharp.Dto
{
	public class TagAudienceBatch : ITagAudience
	{
		readonly ICollection<string> _amazon = new HashSet<string>();
		[JsonProperty("amazon_channel")]
		public ICollection<string> AmazonChannels
		{
			get { return _amazon; }
			set
			{
				if (value != null && object.ReferenceEquals(value, this))
					return;

				_amazon.Clear();
				if(value != null && value.Count > 0)
				{
					foreach(string s in value)
					{
						_amazon.Add(s);
					}
				}
			}
		}

        readonly ICollection<string> _android = new HashSet<string>();
		[JsonProperty("android_channel")]
		public ICollection<string> AndroidChannels
		{
			get { return _android; }
			set
			{ 
				if (value != null && object.ReferenceEquals(value, this))
					return;

				_android.Clear();
				if (value != null && value.Count > 0)
				{
					foreach (string s in value)
					{
						_android.Add(s);
					}
				}
			}
		}

        readonly ICollection<string> _ios = new HashSet<string>();
		[JsonProperty("ios_channel")]
		public ICollection<string> IosChannels
		{
			get { return _ios; }
			set
			{
				if (value != null && object.ReferenceEquals(value, this))
					return;

				_ios.Clear();
				if (value != null && value.Count > 0)
				{
					foreach (string s in value)
					{
						_ios.Add(s);
					}
				}
			}
		}

	}
}
