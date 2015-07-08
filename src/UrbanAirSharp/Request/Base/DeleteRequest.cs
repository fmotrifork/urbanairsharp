// Copyright (c) 2014-2015 Jeff Gosling (jeffery.gosling@gmail.com)

using System.Net.Http;
using System.Threading.Tasks;
using UrbanAirSharp.Response;

namespace UrbanAirSharp.Request.Base
{
	public class DeleteRequest<TResponse> : BaseRequest<TResponse> where TResponse : BaseResponse, new()
	{
		public DeleteRequest(ServiceModelConfig cfg)
			: base(cfg)
		{
			RequestMethod = HttpMethod.Delete;
		}

		public override async Task<TResponse> ExecuteAsync()
		{
			Log.Debug(RequestMethod + " - " + Host + RequestUrl);

			var response = await HttpClient.DeleteAsync(Host + RequestUrl);

			return await DeserializeResponseAsync(response);
		}
	}
}
