using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;
using UrbanAirSharp.Dto;
using UrbanAirSharp.Type;

namespace UrbanAirSharp.Tests.Dto
{
    [TestFixture]
    public class SerializationTests
    {
        static readonly JsonSerializerSettings _jss;
        
        static SerializationTests()
        {
            _jss = ServiceModelConfig.Instance.SerializerSettings;
        }

		[TestCaseSource("AudienceBuilderTestValues")]
		[TestCaseSource("AudienceTestValues")]
        [TestCaseSource("PushTestValues")]
        public void FieldValue(object o, string field, object expectedValue)
        {
            string json = JsonConvert.SerializeObject(o, _jss);
            Assert.That(!string.IsNullOrWhiteSpace(json));

            JObject jo = JObject.Parse(json);
            Assert.IsNotNull(jo);

            JToken tk = jo.SelectToken("$." + field);
            Assert.IsNotNull(tk);

            string jsonVal = tk.ToString(Formatting.None, _jss.Converters.ToArray());
            string expected = JsonConvert.SerializeObject(expectedValue, _jss);
            Assert.AreEqual(expected, jsonVal);
        }

        public static IEnumerable<TestCaseData> PushTestValues
        {
            get
            {
                yield return new TestCaseData(new Push("testing"), "audience", "all");
                yield return new TestCaseData(new Push("w00t"), "device_types", "all");
                yield return new TestCaseData(new Push("abc"), "notification.alert", "abc");

                var dts = new[] { DeviceType.Ios, DeviceType.Android };
                yield return new TestCaseData(new Push("both") { DeviceTypes = dts }, "device_types", dts);

                yield return new TestCaseData(new Push("ios") { DeviceTypes = new[] { DeviceType.Ios } }, "device_types", "ios");

				var au = new Audience(AudienceType.Tag, "test");
				yield return new TestCaseData(new Push("blah", au), "audience", au);
            }
        }

		public static IEnumerable<TestCaseData> AudienceTestValues
		{
			get
			{
				yield return new TestCaseData(new Audience(AudienceType.Ios, "ioskey"), "device_token", "ioskey");
				yield return new TestCaseData(new Audience(AudienceType.Android, "blah"), "apid", "blah");
				yield return new TestCaseData(new Audience(AudienceType.Windows, "zzz"), "wns", "zzz");
				yield return new TestCaseData(new Audience(AudienceType.Blackberry, "1234"), "device_pin", "1234");
				yield return new TestCaseData(new Audience(AudienceType.Segment, "!!!"), "segment", "!!!");
				yield return new TestCaseData(new Audience(AudienceType.Alias, "$$$"), "alias", "$$$");
				yield return new TestCaseData(new Audience(AudienceType.Tag, "-?-"), "tag", "-?-");
			}
		}

		public static IEnumerable<TestCaseData> AudienceBuilderTestValues
		{
			get
			{
				var ios = new Audience(AudienceType.Ios, "ioskey");
				var android = new Audience(AudienceType.Android, "blah");
				var windoze = new Audience(AudienceType.Windows, "zzz");
				var bb = new Audience(AudienceType.Blackberry, "1234");
				var seg = new Audience(AudienceType.Segment, "!!!");
				var alias = new Audience(AudienceType.Alias, "$$$");
				var tag = new Audience(AudienceType.Tag, "-?-");

				yield return new TestCaseData(android.And(ios), "AND", new[] { android, ios });
				yield return new TestCaseData(android & ios, "AND", new[] { android, ios });

				yield return new TestCaseData(android.Or(ios), "OR", new[] { android, ios });
				yield return new TestCaseData(android | ios, "OR", new[] { ios, android });

				yield return new TestCaseData(android.Not(), "NOT", android);
				yield return new TestCaseData(!ios, "NOT", ios);

				yield return new TestCaseData(android.And(tag).Not(), "NOT", android & tag);
			}
		}

	}
}