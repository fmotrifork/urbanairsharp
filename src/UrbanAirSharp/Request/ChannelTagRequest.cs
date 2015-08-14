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
	public class ChannelTagRequest : PostRequest<BaseResponse, TagOperation>
	{
		public ChannelTagRequest(TagOperation content, ServiceModelConfig cfg) : base(content, cfg)
		{
			if (content == null)
				throw new ArgumentNullException("content");

			RequestUrl = "api/channels/tags/";
        }
	}
}
