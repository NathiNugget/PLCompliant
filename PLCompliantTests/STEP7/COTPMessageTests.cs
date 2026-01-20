using PLCompliant.Enums;
using PLCompliant.STEP_7;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliantTests.STEP7
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class COTPMessageTests
    {
        [TestMethod()]
        public void COTPMessageDefaultCTOR()
        {
            // Data segment should be default-initiailized and not be null
            COTPMessage msg = new();
            Assert.IsNotNull(msg);
            Assert.IsNotNull(msg.Data);
            Assert.IsNotNull(msg.Data.Data);

            Assert.AreEqual(msg.Data.Size, msg.Header.Length);
        }
        [TestMethod()]
        public void COTPessageOtherCTOR()
        {

            COTPMessage msg = new(new(), new());
            Assert.IsNotNull(msg);
            Assert.IsNotNull(msg.Data);
            Assert.IsNotNull(msg.Data.Data);

            Assert.AreEqual(msg.Data.Size, msg.Header.Length);


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
            COTPMessage msg = new();
            var expectedLength = msg.AddData(param1, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.Header.Length);


            expectedLength += msg.AddData(param2, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.Header.Length);
            expectedLength += msg.AddData(param3, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.Header.Length);
            expectedLength += msg.AddData(param4, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.Header.Length);
            Assert.AreEqual(expectedLength, msg.Data.Size);
            Assert.AreEqual(param4, msg.Data.Data[msg.Data.Data.Length - 1]); // check that the last item added was infact the value we added
            Assert.AreEqual(expectedLength + msg.Header.Size, msg.Size); // expected length from data + header should equal the whole message size


        }

        [TestMethod]
        [DataRow((uint)0)]
        [DataRow((uint)123)]
        [DataRow((uint)255)]
        public void AddDataByteArrayTest(uint size)
        {
            // The length field in COTPHeader is a 8-bit int, so no more than 255 bytes of data can be in there at any point

            COTPMessage msg = new();
            byte[] arr = new byte[size];
            if (size > 0)
            {
                arr[arr.Length - 1] = 123;
                arr[0] = 45;
            }
            int expectedlength = (int)size;
            msg.AddData(arr, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedlength, msg.Header.Length);
            Assert.AreEqual(expectedlength + msg.Header.Size, msg.Size); // expected length from data + header should equal the whole message size
            if (size > 0)
            {
                Assert.AreEqual(arr[arr.Length - 1], msg.Data.Data[arr.Length - 1]);
                Assert.AreEqual(arr[0], msg.Data.Data[0]);
            }

        }

        [TestMethod]
        [DataRow((uint)256)]
        [DataRow((uint)500000)]

        public void AddDataByteArrayTooLargeTest(uint size)
        {
            uint expectedlength = size;
            COTPMessage msg = new();
            byte[] arr = new byte[size];

            Assert.ThrowsException<ArgumentException>(() => msg.AddData(arr, (byte)IsoTcpDataType.COTPData));

        }
        [TestMethod]
        public void AddDataStructTest()
        {
            TestStruct s1 = new TestStruct();
            COTPMessage msg = new();
            var expectedLength = msg.AddData(s1, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.Header.Length);
            Assert.AreEqual(expectedLength, msg.Data.Size);

            int addedData = msg.AddData(s1, (byte)IsoTcpDataType.COTPData);

            Assert.AreEqual(addedData, Marshal.SizeOf(s1));

            expectedLength += addedData;

            Assert.AreEqual(expectedLength, msg.Header.Length);
            Assert.AreEqual(expectedLength, msg.Data.Size);

        }
        [TestMethod]
        public void GetDataStructTestSucess()
        {
            TestStruct s1 = new TestStruct();
            s1.b4 = 123;
            s1.b3 = 125;
            s1.b2 = 2;
            s1.b1 = 1;
            TestStruct s2 = new TestStruct();
            s2.b4 = 1235;
            s2.b3 = 121;
            s2.b2 = 7;
            s2.b1 = 8;
            COTPMessage msg = new();

            var expectedLength = msg.AddData(s1, (byte)IsoTcpDataType.COTPData);
            expectedLength += msg.AddData(s2, (byte)IsoTcpDataType.COTPData);
            TestStruct output1 = msg.GetData<TestStruct>(0, (byte)IsoTcpDataType.COTPData);
            TestStruct output2 = msg.GetData<TestStruct>(Marshal.SizeOf<TestStruct>(), (byte)IsoTcpDataType.COTPData);


            Assert.AreEqual(s1, output1);
            Assert.AreEqual(s2, output2);

        }

        [TestMethod]
        public void GetDataStructTestFailNotEnoughData()
        {
            COTPMessage msg = new();
            // Data segment is initialized with the message, so it should throw argument out of range exception
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => msg.GetData<TestStruct>(0, (byte)IsoTcpDataType.COTPData));
            // add some data, but not enough to grab TestStruct
            msg.AddData(12345, (byte)IsoTcpDataType.COTPData);
            // If there is data, but there isnt enough, throw out of range exception
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => msg.GetData<TestStruct>(0, (byte)IsoTcpDataType.COTPData));
        }



        [TestMethod()]
        [DataRow((byte)0xFF, (byte)0xA, (byte)0xA, (byte)5, (byte)6, (byte)198, ushort.MinValue, byte.MinValue, (ushort)12456)]
        [DataRow((byte)0xFF, (byte)0xA, byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)10, ushort.MaxValue, byte.MinValue, (ushort)12456)]
        [DataRow((byte)0xFF, (byte)0xA, byte.MinValue, (byte)5, byte.MinValue, (byte)124, ushort.MinValue, byte.MaxValue, (ushort)0)]
        [DataRow(byte.MinValue, (byte)0xD, (byte)0xF, byte.MinValue, (byte)6, (byte)0, (ushort)19999, (byte)0, (ushort)10094)]
        [DataRow(byte.MaxValue, (byte)0xF, (byte)0x2A, (byte)5, (byte)6, byte.MaxValue, (ushort)0, (byte)10, (ushort)5)]
        [DataRow((byte)0xFF, byte.MinValue, (byte)0xBA, (byte)5, (byte)6, byte.MinValue, ushort.MaxValue, byte.MinValue, ushort.MaxValue)]
        [DataRow((byte)0xFF, byte.MaxValue, (byte)0xA9, (byte)5, (byte)6, (byte)5, ushort.MinValue, byte.MaxValue, ushort.MinValue)]

        public void SerializeDeserializeTest(byte protocolId, byte messageType, byte pduReference, byte returnCode, byte transportType, byte dataParam1, ushort dataParam2, byte parameterParam1, ushort parameterParam2)
        {
            TestStruct s1 = new TestStruct { b1 = 1, b2 = 123, b3 = 102, b4 = 0 };
            COTPMessage msg = new();
            msg.AddData(dataParam1, (byte)IsoTcpDataType.COTPData);
            msg.AddData(dataParam2, (byte)IsoTcpDataType.COTPData);
            msg.AddData(parameterParam1, (byte)IsoTcpDataType.COTPData);
            msg.AddData(parameterParam2, (byte)IsoTcpDataType.COTPData);
            msg.AddData(s1, (byte)IsoTcpDataType.COTPData);

            byte[] returnBytes = new byte[msg.Size];
            msg.Serialize(returnBytes);

            COTPMessage response = new();
            ReadOnlySpan<byte> returnBytesSpan = new ReadOnlySpan<byte>(returnBytes);
            response.Deserialize(returnBytesSpan);
            Assert.AreEqual(msg, response);
        }

        [TestMethod]
        [DataRow(byte.MaxValue)]
        [DataRow((byte)(byte.MaxValue / 2))]
        [DataRow(byte.MinValue)]
        public void COTPMessageSize(byte param_1)
        {

            COTPMessage msg = new();
            int expectedsize = msg.Header.Size; // Only header is default-initialized
            Assert.AreEqual(expectedsize, msg.Size);

            expectedsize += msg.AddData(param_1, (byte)IsoTcpDataType.COTPData);

            Assert.AreEqual(expectedsize, msg.Size);

            expectedsize += msg.AddData(param_1, (byte)IsoTcpDataType.COTPData);

            Assert.AreEqual(expectedsize, msg.Size);


        }

    }
}
