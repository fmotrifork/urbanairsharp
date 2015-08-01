using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

using UrbanAirSharp.Dto;
using UrbanAirSharp.Response;

namespace UrbanAirSharp.Tests.Ignores
{
	[Explicit]
	[TestFixture]
	public class PushCRUD
	{
		readonly Regex DEV_TK = new Regex(@"^[a-f0-9]{64}$", RegexOptions.IgnoreCase);
		readonly Regex CHL_TK = new Regex(@"^[a-f0-9\-]{32,48}$", RegexOptions.IgnoreCase);
		readonly UrbanAirSharpGateway _ua;

		public PushCRUD()
		{
			_ua = new UrbanAirSharpGateway();
		}

		[TestCase("DeviceToken Push Test from UrbanAirSharp UnitTest", @"F1F0EA5D64313623203333CD38D4D1D08BBA19DF2F18209590267E28A83B5F8E")]
		public void PushToken(string message, string iosDeviceToken)
		{
			Assert.That(!string.IsNullOrWhiteSpace(message));
			Assert.That(!string.IsNullOrWhiteSpace(iosDeviceToken));

			if (!DEV_TK.IsMatch(iosDeviceToken))
			{
				string tk = ServiceModelConfig.GetConfigValue(iosDeviceToken);
				Assert.That(DEV_TK.IsMatch(tk), "Invalid iOS Device Token: {0} => {1}", iosDeviceToken, tk);
				iosDeviceToken = tk;
			}

			var p = new Push(message, new Audience(Type.AudienceType.Ios, iosDeviceToken));
			PushResponse pv = _ua.Validate(p);
			TestResponse(pv);

			PushResponse pr = _ua.Push(p);
			TestResponse(pr);
			CollectionAssert.IsNotEmpty(pr.PushIds);
		}

		void TestResponse(BaseResponse r)
		{
			Assert.IsNotNull(r);
			Assert.That(string.IsNullOrEmpty(r.Error), r.Error);
			Assert.LessOrEqual(r.ErrorCode, 0);
			Assert.IsTrue(r.Ok);
		}

		[TestCase("Channel Push Test from UrbainAirSharp UnitTest", @"a1e4ae13-7595-454c-bf96-e85b38ab265b")]
        public void PushChannel(string message, string channelId)
		{
			Assert.That(!string.IsNullOrWhiteSpace(message));
			Assert.That(!string.IsNullOrWhiteSpace(channelId));

			if (!CHL_TK.IsMatch(channelId))
			{
				string ch = ServiceModelConfig.GetConfigValue(channelId);
				Assert.That(CHL_TK.IsMatch(ch), "Invalid UA Channel Id: {0} => {1}", channelId, ch);
				channelId = ch;
			}

			var p = new Push(message, new Audience(Type.AudienceType.Ios, channelId, true));
			PushResponse pv = _ua.Validate(p);
			TestResponse(pv);

			PushResponse pr = _ua.Push(p);
			TestResponse(pr);
			CollectionAssert.IsNotEmpty(pr.PushIds);
		}
	}
}
