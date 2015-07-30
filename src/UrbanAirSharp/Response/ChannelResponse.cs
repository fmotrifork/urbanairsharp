using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UrbanAirSharp.Dto;

namespace UrbanAirSharp.Response
{
	public class ChannelResponse : BaseResponse
	{
		[JsonProperty("channel")]
		public Channel Channel { get; set; }
	}
}
