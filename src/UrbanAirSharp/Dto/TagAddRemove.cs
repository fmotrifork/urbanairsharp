using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UrbanAirSharp.Dto
{
	public class TagAddRemove : TagOperation
	{
		public TagAddRemove(TagAudience audience) : base(audience)
		{
		}
		public TagAddRemove(TagAudienceBatch audience) : base(audience)
		{
		}

		readonly Dictionary<string, ICollection<string>> _add = new Dictionary<string, ICollection<string>>();
		[JsonProperty("add")]
		public IDictionary<string, ICollection<string>> Add
		{
			get { return _add; }
			set
			{
				if (value != null && object.ReferenceEquals(value, _add))
					return;

				_add.Clear();
				if (value != null && value.Count > 0)
				{
					foreach (var p in value)
					{
						_add.Add(p.Key, p.Value);
					}
				}
			}
		}

		readonly Dictionary<string, ICollection<string>> _remove = new Dictionary<string, ICollection<string>>();
		[JsonProperty("remove")]
		public IDictionary<string, ICollection<string>> Remove
		{
			get { return _remove; }
			set
			{
				if (value != null && object.ReferenceEquals(value, _remove))
					return;

				_remove.Clear();
				if (value != null && value.Count > 0)
				{
					foreach (var p in value)
					{
						_remove.Add(p.Key, p.Value);
					}
				}
			}
		}
	}
}
