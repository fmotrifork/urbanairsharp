﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UrbanAirSharp.Dto
{
	public class AudienceOr : AudienceBase, IAudienceCompounder
	{
		IEnumerable<IAudience> _audiences = new IAudience[0];

		[JsonProperty("OR", Required = Required.Always)]
		public IEnumerable<IAudience> Audiences
		{
			get { return _audiences; }
			set { _audiences = value ?? new IAudience[0]; }
		}
	}
}
