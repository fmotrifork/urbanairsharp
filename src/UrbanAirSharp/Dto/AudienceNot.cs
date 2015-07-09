using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UrbanAirSharp.Dto
{
	public class AudienceNot : AudienceBase, IAudience
	{
		[JsonProperty("NOT", Required = Required.Always)]
		public IAudience Audience { get; set; }
	}
}
