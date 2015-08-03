using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UrbanAirSharp.Dto
{
	public abstract class TagOperation
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

	}
}
