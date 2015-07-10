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
    public class PushSerialization
    {
        static readonly JsonSerializerSettings _jss;
        
        static PushSerialization()
        {
            _jss = ServiceModelConfig.Instance.SerializerSettings;
        }

        [TestCaseSource("FieldValues")]
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

        public static IEnumerable<TestCaseData> FieldValues
        {
            get
            {
                yield return new TestCaseData(new Push("testing"), "audience", "all");
                yield return new TestCaseData(new Push("w00t"), "device_types", "all");
                yield return new TestCaseData(new Push("abc"), "notification.alert", "abc");

                var dts = new[] { DeviceType.Ios, DeviceType.Android };
                yield return new TestCaseData(new Push("both") { DeviceTypes = dts }, "device_types", dts);

                yield return new TestCaseData(new Push("ios") { DeviceTypes = new[] { DeviceType.Ios } }, "device_types", "ios");
            }
        }

    }
}