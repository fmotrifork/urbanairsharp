using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UrbanAirSharp.Dto
{
	public class TagOperation
	{
		public TagOperation(ITagAudience audience)
		{
			Audience = audience;
		}

		ITagAudience _audience;
		[JsonProperty("audience")]
		public ITagAudience Audience
		{
			get { return _audience; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("Audience");

				_audience = value;
			}
		}

		readonly Dictionary<string, ICollection<string>> _add = new Dictionary<string, ICollection<string>>();
		[JsonProperty("add")]
		public IDictionary<string, ICollection<string>> Add
		{
			get { return _add; }
		}

		readonly Dictionary<string, ICollection<string>> _remove = new Dictionary<string, ICollection<string>>();
		[JsonProperty("remove")]
		public IDictionary<string, ICollection<string>> Remove
		{
			get { return _remove; }
		}

		readonly Dictionary<string, ICollection<string>> _set = new Dictionary<string, ICollection<string>>();
		[JsonProperty("set")]
		public IDictionary<string, ICollection<string>> Set
		{
			get { return _set; }
		}
	}
}
