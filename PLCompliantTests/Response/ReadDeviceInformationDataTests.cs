using PLCompliant.Modbus;
using PLCompliantTests;
using System.Diagnostics.CodeAnalysis;

namespace PLCompliant.Response.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class ReadDeviceInformationDataTests
    {
        [TestMethod()]
        public void ToCSVTest()
        {
            ModBusMessage msg = TestHelper.CreateExampleReadDeviceInformationResponse();
            string expected = "192.168.123.100;Schneider Electric;BMX NOE 0100;V2.30";
            ReadDeviceInformationData response = ModBusResponseParsing.ParseReadDeviceInformationResponse(msg, System.Net.IPAddress.Parse("192.168.123.100"));
            string actual = response.ToCSV();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ObjectsInResponse()
        {
            ModBusMessage msg = TestHelper.CreateExampleReadDeviceInformationResponse();
            ReadDeviceInformationData response = ModBusResponseParsing.ParseReadDeviceInformationResponse(msg, System.Net.IPAddress.Parse("192.168.123.100"));
            Dictionary<int, string> objs = response.Objects;
            Assert.IsNotNull(objs);
            objs.Add(4, "SomeObjectValueOn4thIndexBecauseProtocolWasNotStandardized");
            Assert.IsNotNull(objs);
        }
    }
}