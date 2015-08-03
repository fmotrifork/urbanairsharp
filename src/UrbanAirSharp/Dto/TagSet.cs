using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UrbanAirSharp.Dto
{
	public class TagSet : TagOperation
	{
		public TagSet(TagAudience audience) : base(audience) { }
		public TagSet(TagAudienceBatch audience) : base(audience) { }

		readonly Dictionary<string, ICollection<string>> _set = new Dictionary<string, ICollection<string>>();
		[JsonProperty("set")]
		public IDictionary<string, ICollection<string>> Set
		{
			get { return _set; }
			set
			{
				if (value != null && object.ReferenceEquals(value, _set))
					return;

				_set.Clear();
				if (value != null && value.Count > 0)
				{
					foreach (var p in value)
					{
						_set.Add(p.Key, p.Value);
					}
				}
			}
		}
	}
}
