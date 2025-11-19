using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCompliant.Modbus;
using PLCompliant.Response;
using PLCompliantTests;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Response.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class ReadDeviceInformationDataTests
    {
        [TestMethod()]
        public void ToCSVTest()
        {
            ModBusMessage msg = UtilityMethodTests.CreateExampleReadDeviceInformationResponse();
            string expected = "192.168.123.100;Schneider Electric;BMX NOE 0100;V2.30";
            ReadDeviceInformationData response = ModBusResponseParsing.ParseReadDeviceInformationResponse(msg, System.Net.IPAddress.Parse("192.168.123.100"));
            string actual = response.ToCSV(); 
            Assert.AreEqual(expected, actual);
        }
    }
}