
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCompliant.Enums;
using PLCompliant.STEP_7;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace PLCompliantTests.STEP7
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class COTPDataTests
    {
        [TestMethod()]
        public void COTPDataDefaultCTOR()
        {
            // Data segment should be default-initiailized and not be null
            COTPData msg = new();
            Assert.IsNotNull(msg);
            Assert.IsNotNull(msg.Data);

            Assert.AreEqual(msg.Size, msg.Data.Length);
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
            COTPData msg = new();
            var expectedLength = msg.AddData(param1, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.Data.Length);
            Assert.AreEqual(expectedLength, msg.Size);


            expectedLength += msg.AddData(param2, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.Data.Length);
            Assert.AreEqual(expectedLength, msg.Size);
            expectedLength += msg.AddData(param3, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.Data.Length);
            Assert.AreEqual(expectedLength, msg.Size);
            expectedLength += msg.AddData(param4, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.Data.Length);
            Assert.AreEqual(expectedLength, msg.Size);

            Assert.AreEqual(param4, msg.Data[msg.Data.Length - 1]); // check that the last item added was infact the value we added

        }

        [TestMethod]
        [DataRow((uint)0)]
        [DataRow((uint)123)]
        [DataRow((uint)255)]
        public void AddDataByteArrayTest(uint size)
        {

            COTPData msg = new();
            byte[] arr = new byte[size];
            if (size > 0)
            {
                arr[arr.Length - 1] = 123;
                arr[0] = 45;
            }
            int expectedlength = (int)size;
            msg.AddData(arr, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedlength, msg.Data.Length);
            Assert.AreEqual(expectedlength, msg.Size);
            if (size > 0)
            {
                Assert.AreEqual(arr[arr.Length - 1], msg.Data[arr.Length - 1]);
                Assert.AreEqual(arr[0], msg.Data[0]);
            }

        }

        [TestMethod]
        [DataRow((uint)256)]
        [DataRow((uint)500000)]

        public void AddDataByteArrayTooLargeTest(uint size)
        {
            uint expectedlength = size;
            COTPData msg = new();
            byte[] arr = new byte[size];

            Assert.ThrowsException<ArgumentException>(() => msg.AddData(arr, (byte)IsoTcpDataType.COTPData));

        }
        [TestMethod]
        public void AddDataStructTest()
        {
            TestStruct s1 = new TestStruct();
            COTPData msg = new();
            var expectedLength = msg.AddData(s1, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.Data.Length);
            Assert.AreEqual(expectedLength, msg.Size);

            int addedData = msg.AddData(s1, (byte)IsoTcpDataType.COTPData);

            Assert.AreEqual(addedData, Marshal.SizeOf(s1));

            expectedLength += addedData;

            Assert.AreEqual(expectedLength, msg.Data.Length);
            Assert.AreEqual(expectedLength, msg.Size);

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
            COTPData msg = new();

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
            COTPData msg = new();
            // Data segment is initialized, so it should throw argument out of range exception
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
            COTPData msg = new();
            msg.AddData(dataParam1, (byte)IsoTcpDataType.COTPData);
            msg.AddData(dataParam2, (byte)IsoTcpDataType.COTPData);
            msg.AddData(parameterParam1, (byte)IsoTcpDataType.COTPData);
            msg.AddData(parameterParam2, (byte)IsoTcpDataType.COTPData);
            msg.AddData(s1, (byte)IsoTcpDataType.COTPData);

            byte[] returnBytes = new byte[msg.Size];
            msg.Serialize(returnBytes);

            COTPData response = new();
            ReadOnlySpan<byte> returnBytesSpan = new ReadOnlySpan<byte>(returnBytes);
            response.Deserialize(returnBytesSpan);
            Assert.AreEqual(msg, response);
        }

        [TestMethod]
        [DataRow(byte.MaxValue)]
        [DataRow((byte)(byte.MaxValue / 2))]
        [DataRow(byte.MinValue)]
        public void COTPDataSize(byte param_1)
        {

            COTPData msg = new();
            int expectedsize = 0; // should start with 0 length on default init
            Assert.AreEqual(expectedsize, msg.Size);

            expectedsize += msg.AddData(param_1, (byte)IsoTcpDataType.COTPData);

            Assert.AreEqual(expectedsize, msg.Size);

            expectedsize += msg.AddData(param_1, (byte)IsoTcpDataType.COTPData);

            Assert.AreEqual(expectedsize, msg.Size);


        }


    }
}
