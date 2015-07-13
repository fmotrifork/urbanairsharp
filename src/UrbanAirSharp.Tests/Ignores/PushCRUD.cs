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
		readonly UrbanAirSharpGateway _ua;

		public PushCRUD()
		{
			_ua = new UrbanAirSharpGateway();
		}

		[TestCase("Hello From UrbanAirship!", "My.DeviceToken")]
		public void Push(string message, string iosDeviceToken)
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
			PushResponse r = _ua.Push(p);

			Assert.IsNotNull(r);
			Assert.That(string.IsNullOrEmpty(r.Error), r.Error);
			Assert.LessOrEqual(r.ErrorCode, 0);
			Assert.IsTrue(r.Ok);
			CollectionAssert.IsNotEmpty(r.PushIds);
        }
	}
}
