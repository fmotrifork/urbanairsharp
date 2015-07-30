using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanAirSharp.Dto;
using UrbanAirSharp.Request.Base;
using UrbanAirSharp.Response;

namespace UrbanAirSharp.Request
{
	public class ChannelRequest : GetRequest<ChannelResponse>
	{
		public ChannelRequest(string channelId, ServiceModelConfig cfg) : base(cfg)
		{
			if (string.IsNullOrWhiteSpace(channelId))
				throw new ArgumentException("ChannelId is required");

			RequestUrl = string.Format("api/channels/{0}", channelId);
		}
	}
}
