using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;
using UrbanAirSharp.Dto;
using UrbanAirSharp.Response;
using TT = System.Type;

namespace UrbanAirSharp.Tests.Dto
{
	[TestFixture]
	public class ObjectDeserializationTests
	{
		static readonly JsonSerializerSettings _jss;

		static ObjectDeserializationTests()
		{
			_jss = ServiceModelConfig.Instance.SerializerSettings;
		}

		[TestCaseSource("ChannelTestObjects")]
		public void ObjectValue(TD d)
		{
			CollectionAssert.IsNotEmpty(d.JSON);
			object o = JsonConvert.DeserializeObject(d.JSON, d.Type, _jss);
			Assert.IsNotNull(o);

			foreach(string k in d.Expected.Keys)
			{
				PropertyInfo p = p = d.Type.GetProperty(k, BindingFlags.Public | BindingFlags.Instance);
				Assert.IsNotNull(p);
				Assert.IsTrue(p.CanRead);

				object v = p.GetValue(o);
				Assert.AreEqual(d.Expected[k], v);
			}
		}

		//NOTE: dummy test data class to pass in data by ref. Not sure why these items does not get matched by Nunit 2.6.4
		public class TD
		{
			public TD(string json, TT t, Dictionary<string, object> kv)
			{
				JSON = json;
				Type = t;
				Expected = kv;
			}

			public string JSON;
			public TT Type;
			public Dictionary<string, object> Expected;
		}

		#region test data generator

		public static IEnumerable<TestCaseData> ChannelTestObjects
		{
			get
			{
				const string CHANNEL_JSON = @"{
    ""channel_id"": ""01234567-890a-bcde-f012-3456789abc0"",
    ""device_type"": ""ios"",
    ""installed"": true,
    ""opt_in"": false,
    ""push_address"": ""FE66489F304DC75B8D6E8200DFF8A456E8DAEACEC428B427E9518741C92C6660"",
    ""created"": ""2013-08-08T20:41:06"",
    ""last_registration"": ""2014-05-01T18:00:27"",

    ""alias"": ""your_user_id"",
    ""tags"": [
        ""tag1"",
        ""tag2""
    ],

    ""tag_groups"": {
        ""tag_group_1"": [""tag1"", ""tag2""],
        ""tag_group_2"": [""tag1"", ""tag2""]
    },

    ""ios"": {
        ""badge"": 0,
        ""quiettime"": {
        ""start"": null,
        ""end"": null
        },
        ""tz"": ""America/Los_Angeles""
    }
}";
				yield return new TestCaseData(new TD(CHANNEL_JSON, typeof(Channel), new Dictionary<string, object>
				{
					{ "ChannelId", "01234567-890a-bcde-f012-3456789abc0" },
					{ "DeviceType", Type.DeviceType.Ios },
					{ "IsInstalled", true },
				}));

				const string CHANNEL_RESPOSNE = @"{
   ""ok"": true,
   ""channel"": " + CHANNEL_JSON + @"
}";
				yield return new TestCaseData(new TD(CHANNEL_RESPOSNE, typeof(ChannelResponse), new Dictionary<string, object>
				{
					{ "Ok", true },
				}));
			}
		}

		#endregion
	}
}
