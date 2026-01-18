using PLCompliant.Enums;
using PLCompliant.Modbus;
using PLCompliant.STEP_7;
using PLCompliant.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace PLCompliantTests.STEP7
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class STEP7MessageTests
    {
        public STEP7Message CreateSTEP7Msg()
        {
            STEP7Message msg = new();
            return msg;
        }
        [TestMethod()]
        public void STEP7MessageExists()
        {
            STEP7Message msg = CreateSTEP7Msg();
            Assert.IsNotNull(msg);

        }

        [TestMethod()]
        [DataRow(ushort.MinValue, byte.MinValue, ushort.MinValue, byte.MinValue)]
        [DataRow(ushort.MaxValue, byte.MaxValue, ushort.MaxValue, byte.MaxValue)]
        [DataRow((ushort)(ushort.MaxValue / 2), (byte)(byte.MaxValue / 2), (ushort)(ushort.MaxValue / 2), (byte)(byte.MaxValue / 2))]
        [DataRow((ushort)10, (byte)0x10, (ushort)10, (byte)0x10)]
        [DataRow(ushort.MinValue, byte.MaxValue, ushort.MaxValue, byte.MinValue)]
        [DataRow(ushort.MaxValue, byte.MinValue, ushort.MinValue, byte.MaxValue)]
        [DataRow((ushort)0, (byte)0xF, (ushort)255, (byte)0xFF)]

        public void AddDataTest(ushort param1, byte param2, ushort param3, byte param4)
        {
            STEP7Message msg = CreateSTEP7Msg();
            var expectedLength = msg.AddData(param1, (byte)IsoTcpDataType.STEP7RegularData);
            Assert.AreEqual(expectedLength, msg.STEP7Header.DataLength);


            expectedLength += msg.AddData(param2, (byte)IsoTcpDataType.STEP7RegularData);
            Assert.AreEqual(expectedLength, msg.STEP7Header.DataLength);
            expectedLength += msg.AddData(param3, (byte)IsoTcpDataType.STEP7RegularData);
            Assert.AreEqual(expectedLength, msg.STEP7Header.DataLength);
            expectedLength += msg.AddData(param4, (byte)IsoTcpDataType.STEP7RegularData);
            Assert.AreEqual(expectedLength, msg.STEP7Header.DataLength);
            Assert.AreEqual(expectedLength, msg.STEP7Data.Size);


        }

        [TestMethod]
        [DataRow((uint)0)]
        [DataRow((uint)255)]
        public void AddDataByteArrayTest(uint size)
        {

            STEP7Message msg = CreateSTEP7Msg();
            byte[] arr = new byte[size];
            int expectedlength = (int)size;
            msg.AddData(arr, (byte)IsoTcpDataType.STEP7RegularData);
            expectedlength += msg.STEP7Data.Header.Size; // add header since it is initialized in AddData
            Assert.AreEqual(msg.STEP7Header.DataLength, expectedlength);
            Assert.AreEqual(msg.STEP7Data.Size, (int)expectedlength);

            //Repeat
            expectedlength += (int)size;
            msg.AddData(arr, (byte)IsoTcpDataType.STEP7RegularData);
            Assert.AreEqual(msg.STEP7Header.DataLength, expectedlength);
            Assert.AreEqual(msg.STEP7Data.Size, (int)expectedlength);


        }

        [TestMethod]
        [DataRow((uint)256)]
        [DataRow((uint)500000)]

        public void AddDataByteArrayTooLargeTest(uint size)
        {
            uint expectedlength = size;
            STEP7Message msg = CreateSTEP7Msg();
            byte[] arr = new byte[size];

            Assert.ThrowsException<ArgumentException>(() => msg.AddData(arr, (byte)IsoTcpDataType.STEP7RegularData));

        }


        //[TestMethod()]
        //[DataRow((byte)0xFF,     (byte)0xA,       (byte)0xA,      (byte)3,         (byte)5,        (byte)6,        (byte)198,      ushort.MinValue,     byte.MinValue,    (ushort)12456)]
        //[DataRow((byte)0xFF,     (byte)0xA,       byte.MaxValue,  (byte)3,         byte.MaxValue,   byte.MaxValue,  (byte)10,       ushort.MaxValue,     byte.MinValue,    (ushort)12456)]
        //[DataRow((byte)0xFF,     (byte)0xA,       byte.MinValue,  (byte)3,         (byte)5,         byte.MinValue,  (byte)124,      ushort.MinValue,     byte.MaxValue,    (ushort)0)]
        //[DataRow(byte.MinValue, (byte)0xD,       (byte)0xF,       byte.MinValue,  byte.MinValue,  (byte)6,        (byte)0,        (ushort)19999,      (byte)0,           (ushort)10094)]
        //[DataRow(byte.MaxValue, (byte)0xF,       (byte)0x2A,      byte.MaxValue,  (byte)5,        (byte)6,         byte.MaxValue, (ushort)0,          (byte)10,          (ushort)5)]
        //[DataRow((byte)0xFF,     byte.MinValue,  (byte)0xBA,      (byte)3,        (byte)5,        (byte)6,         byte.MinValue, ushort.MaxValue,     byte.MinValue,     ushort.MaxValue)]
        //[DataRow((byte)0xFF,     byte.MaxValue,  (byte)0xA9,      (byte)3,        (byte)5,        (byte)6,        (byte)5,        ushort.MinValue,     byte.MaxValue,     ushort.MinValue)]

        //public void SerializeDeserializeTest(byte protocolId, byte messageType, byte pduReference, byte functionCode, byte returnCode, byte transportType, byte dataParam1, ushort dataParam2, byte normalParam1, ushort normalParam2)
        //{
        //    STEP7Header header = new(protocolId, messageType, pduReference);
        //    STEP7ParameterData paramData = new(functionCode);
        //    STEP7Data normalData = new(returnCode, transportType);
        //    STEP7Message msg = new(header, paramData, normalData);
        //    msg.AddData(dataParam1);
        //    msg.AddData(dataParam2);
        //    msg.AddParameterData(normalParam1);
        //    msg.AddParameterData(normalParam2);

        //    byte[] returnBytes = msg.Serialize();
        //    //header
        //    STEP7Message response = new(new(), new(), new());
        //    byte[] headerBytes = new byte[header.Size];
        //    Array.Copy(returnBytes, 0, headerBytes, 0, headerBytes.Length);
        //    response.DeserializeHeader(headerBytes, 0);
        //    //data
        //    byte[] payloadData = new byte[msg.STEP7Data.Size + msg.STEP7ParamData.Size];
        //    Array.Copy(returnBytes, header.Size, payloadData, 0, payloadData.Length);
        //    response.DeserializeData(payloadData, 0);
        //    Assert.AreEqual(msg, response);
        //}

        [TestMethod]
        [DataRow(byte.MaxValue)]
        [DataRow((byte)(byte.MaxValue / 2))]
        [DataRow(byte.MinValue)]
        public void ModBusMessageSizeAndTotalSize(byte param_1)
        {

            STEP7Message msg = CreateSTEP7Msg();
            int expectedsize = msg.STEP7Header.Size; // Only header is default-initialized
            Assert.AreEqual(expectedsize, msg.Size);

            expectedsize += msg.AddData(param_1, (byte)IsoTcpDataType.STEP7RegularData);

            Assert.AreEqual(expectedsize, msg.Size);

            expectedsize += msg.AddData(param_1, (byte)IsoTcpDataType.STEP7ParamData);


        }


    }
}

