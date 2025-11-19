using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCompliant.Modbus;
using PLCompliant.Response;
using PLCompliantTests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Modbus.Tests
{
    [TestClass()]
    [ExcludeFromCodeCoverage]
    public class ModBusResponseParsingTests
    {

        [TestMethod()]
        [DataRow((byte)0x1)]
        [DataRow((byte)0b0111_1111)]
        [DataRow((byte)0x0)]
        public void HandleResponseFailTest(byte param_1)
        {
            ModBusMessage exceptionresponse = new(new(), new(((byte)ModBusCommandType.read_device_information | 0b1000_0000), []));
            exceptionresponse.AddData(param_1);
            Assert.IsFalse(ModBusResponseParsing.HandleReponseError(exceptionresponse, out byte err)); 
            Assert.AreEqual(param_1, err);
        }


        [TestMethod()]
        [DataRow((byte)0x0)]
        public void HandleResponseSucceedTest(byte param_1)
        {
            int expected = 0; // We expect error code to be 0 if no expection is thrown within handling method.
            ModBusMessage exceptionresponse = new(new(), new(((byte)ModBusCommandType.read_device_information), []));
            exceptionresponse.AddData(param_1);
            Assert.IsTrue(ModBusResponseParsing.HandleReponseError(exceptionresponse, out byte err));
            Assert.AreEqual(expected, err);
        }

        [TestMethod()]
        public void ParseReadDeviceInformationResponseTest()
        {
            ModBusMessage msg = UtilityMethodTests.CreateExampleReadDeviceInformationResponse();
            int expectedobjectcount = 3; //We will be inserting 3 objects; 
            string expected1 = "Schneider Electric";
            string expected2 = "BMX NOE 0100";
            string expected3 = "V2.30";

            ReadDeviceInformationData response = ModBusResponseParsing.ParseReadDeviceInformationResponse(msg, System.Net.IPAddress.Parse("192.168.123.100"));
            Assert.AreEqual(response.noOfObjects, expectedobjectcount);
            Assert.AreEqual(response.Objects[0], expected1); 
            Assert.AreEqual(response.Objects[1], expected2);
            Assert.AreEqual(response.Objects[2], expected3); 


        }
    }
}