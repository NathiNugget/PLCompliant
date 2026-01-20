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
    public class IsoTcpMessageTests
    {
        [TestMethod()]
        public void STEP7MessageDefaultCTOR()
        {
            IsoTcpMessage msg = new();
            Assert.IsNotNull(msg.COTP);
            Assert.IsNull(msg.STEP7);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msg.Size, msg.TPKT.Length);

        }
        [TestMethod()]
        public void STEP7MessageOtherCTOR()
        {
            IsoTcpMessage msg = new(new(), new(), new());
            Assert.IsNotNull(msg.COTP);
            Assert.IsNotNull(msg.STEP7);
            Assert.IsNotNull(msg);

            Assert.AreEqual(msg.Size, msg.TPKT.Length);


        }
        [TestMethod()]
        public void STEP7MessageOtherCTOR2()
        {
            IsoTcpMessage msg = new(new(), new(), new(new(), new(), new()));
            Assert.IsNotNull(msg.COTP);
            Assert.IsNotNull(msg.STEP7);
            Assert.IsNotNull(msg);

            Assert.AreEqual(msg.Size, msg.TPKT.Length);
            Assert.AreEqual(msg.STEP7.STEP7Header.DataLength, msg.STEP7.STEP7Data.Size);
            Assert.AreEqual(msg.STEP7.STEP7Header.ParameterLength, msg.STEP7.STEP7ParamData.Size);


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
            // Regular step7 data
            IsoTcpMessage msg = new();
            var expectedLength = msg.Size;
            expectedLength += msg.AddData(param1, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);


            expectedLength += msg.AddData(param2, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);
            expectedLength += msg.AddData(param3, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);
            expectedLength += msg.AddData(param4, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);
            Assert.AreEqual(param4, msg.STEP7.STEP7Data.Data.Data[msg.STEP7.STEP7Data.Data.Data.Length - 1]); // check that the last item added was infact the value we added


            //step7 parameter data

            expectedLength += msg.AddData(param1, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);


            expectedLength += msg.AddData(param2, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);
            expectedLength += msg.AddData(param3, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);
            expectedLength += msg.AddData(param4, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);
            Assert.AreEqual(param4, msg.STEP7.STEP7ParamData.Data[msg.STEP7.STEP7ParamData.Data.Length - 1]); // check that the last item added was infact the value we added



            //cotp data

            expectedLength += msg.AddData(param1, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);

            expectedLength += msg.AddData(param2, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);
            expectedLength += msg.AddData(param3, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);
            expectedLength += msg.AddData(param4, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);
            Assert.AreEqual(param4, msg.COTP.Data.Data[msg.COTP.Data.Data.Length - 1]); // check that the last item added was infact the value we added



        }

        [TestMethod]
        [DataRow((UInt16)1245)]
        [DataRow(UInt16.MinValue)]
        [DataRow((UInt16)(UInt16.MaxValue - 100))]
        public void AddDataByteArrayTestSTEP7Data(UInt16 size)
        {
            // regular data, can go up to UInt16 max value in size. However the TPKT header can also only go up to 16 bits, and it will include header information. So the most bytes allowed is actually abit lower than UInt16 max
            IsoTcpMessage msg = new();
            byte[] arr = new byte[size];
            if (size > 0)
            {
                arr[arr.Length - 1] = 123;
                arr[0] = 45;
            }

            int expectedlength = msg.Size;
            expectedlength += msg.AddData(arr, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));

            Assert.AreEqual(expectedlength, msg.TPKT.Length);
            Assert.AreEqual(expectedlength, msg.Size);
            if (size > 0)
            {
                Assert.AreEqual(arr[arr.Length - 1], msg.STEP7.STEP7Data.Data.Data[arr.Length - 1]);
                Assert.AreEqual(arr[0], msg.STEP7.STEP7Data.Data.Data[0]);
            }
        }


        [TestMethod]
        [DataRow((UInt16)1245)]
        [DataRow(UInt16.MinValue)]
        [DataRow((UInt16)(UInt16.MaxValue - 100))]
        public void AddDataByteArrayTestSTEP7ParamData(UInt16 size)
        {
            // parameter data, can go up to UInt16 max value in size. However the TPKT header can also only go up to 16 bits, and it will include header information. So the most bytes allowed is actually abit lower than UInt16 max
            IsoTcpMessage msg = new();
            byte[] arr = new byte[size];
            if (size > 0)
            {
                arr[arr.Length - 1] = 123;
                arr[0] = 45;
            }

            int expectedlength = msg.Size;
            expectedlength += msg.AddData(arr, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));

            Assert.AreEqual(expectedlength, msg.TPKT.Length);
            Assert.AreEqual(expectedlength, msg.Size);
            if (size > 0)
            {
                Assert.AreEqual(arr[arr.Length - 1], msg.STEP7.STEP7ParamData.Data[arr.Length - 1]);
                Assert.AreEqual(arr[0], msg.STEP7.STEP7ParamData.Data[0]);
            }
        }


        [TestMethod]
        [DataRow((byte)123)]
        [DataRow(byte.MinValue)]
        [DataRow(byte.MaxValue)]
        public void AddDataByteArrayTestCOTPData(byte size)
        {
            // COTP data, can go up to a byte max value in size
            IsoTcpMessage msg = new();
            byte[] arr = new byte[size];
            if (size > 0)
            {
                arr[arr.Length - 1] = 123;
                arr[0] = 45;
            }

            int expectedlength = msg.Size;
            expectedlength += msg.AddData(arr, (byte)IsoTcpDataType.COTPData);

            Assert.AreEqual(expectedlength, msg.TPKT.Length);
            Assert.AreEqual(expectedlength, msg.Size);
            if (size > 0)
            {
                Assert.AreEqual(arr[arr.Length - 1], msg.COTP.Data.Data[arr.Length - 1]);
                Assert.AreEqual(arr[0], msg.COTP.Data.Data[0]);
            }
        }

        [TestMethod]
        [DataRow((uint)UInt16.MaxValue + 1)]
        [DataRow((uint)500000)]

        public void AddDataByteArrayTooLargeTestSTEP7Data(uint size)
        {
            uint expectedlength = size;
            IsoTcpMessage msg = new();
            byte[] arr = new byte[size];

            Assert.ThrowsException<ArgumentException>(() => msg.AddData(arr, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data)));

        }
        [TestMethod]
        [DataRow((uint)UInt16.MaxValue + 1)]
        [DataRow((uint)500000)]

        public void AddDataByteArrayTooLargeTestSTEP7ParamData(uint size)
        {
            uint expectedlength = size;
            IsoTcpMessage msg = new();
            byte[] arr = new byte[size];

            Assert.ThrowsException<ArgumentException>(() => msg.AddData(arr, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data)));

        }
        [TestMethod]
        [DataRow((uint)byte.MaxValue + 1)]
        [DataRow((uint)500000)]

        public void AddDataByteArrayTooLargeTestCOTPData(uint size)
        {
            uint expectedlength = size;
            IsoTcpMessage msg = new();
            byte[] arr = new byte[size];

            Assert.ThrowsException<ArgumentException>(() => msg.AddData(arr, (byte)IsoTcpDataType.COTPData));

        }
        [TestMethod]
        public void AddDataStructTest()
        {
            // regular step data
            TestStruct s1 = new TestStruct();
            IsoTcpMessage msg = new();
            var expectedLength = msg.Size;
            expectedLength += msg.AddData(s1, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);

            int addedData = msg.AddData(s1, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));

            Assert.AreEqual(addedData, Marshal.SizeOf(s1));

            expectedLength += addedData;

            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);

            // step7 param data

            expectedLength += msg.AddData(s1, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);

            int addedDataParams = msg.AddData(s1, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));

            Assert.AreEqual(addedDataParams, Marshal.SizeOf(s1));

            expectedLength += addedDataParams;

            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);


            // COTP data

            expectedLength += msg.AddData(s1, (byte)IsoTcpDataType.COTPData);
            Assert.AreEqual(expectedLength, msg.TPKT.Length);
            Assert.AreEqual(expectedLength, msg.Size);

            int addedCotpData = msg.AddData(s1, (byte)IsoTcpDataType.COTPData);

            Assert.AreEqual(addedCotpData, Marshal.SizeOf(s1));

            expectedLength += addedCotpData;

            Assert.AreEqual(expectedLength, msg.TPKT.Length);
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
            IsoTcpMessage msg = new();

            // step7 regular data
            msg.AddData(s1, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));
            msg.AddData(s2, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));
            TestStruct output1 = msg.GetData<TestStruct>(0, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));
            TestStruct output2 = msg.GetData<TestStruct>(Marshal.SizeOf<TestStruct>(), (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));


            Assert.AreEqual(s1, output1);
            Assert.AreEqual(s2, output2);

            //step7 param data

            msg.AddData(s1, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));
            msg.AddData(s2, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));
            TestStruct output1Param = msg.GetData<TestStruct>(0, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));
            TestStruct output2Param = msg.GetData<TestStruct>(Marshal.SizeOf<TestStruct>(), (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));


            Assert.AreEqual(s1, output1Param);
            Assert.AreEqual(s2, output2Param);

            //cotp data

            msg.AddData(s1, (byte)IsoTcpDataType.COTPData);
            msg.AddData(s2, (byte)IsoTcpDataType.COTPData);
            TestStruct output1cotp = msg.GetData<TestStruct>(0, (byte)IsoTcpDataType.COTPData);
            TestStruct output2cotp = msg.GetData<TestStruct>(Marshal.SizeOf<TestStruct>(), (byte)IsoTcpDataType.COTPData);


            Assert.AreEqual(s1, output1cotp);
            Assert.AreEqual(s2, output2cotp);


        }
        [TestMethod]
        public void GetDataStructTestFailNotEnoughData()
        {
            IsoTcpMessage msg = new();
            // Null reference if the data and param segments havent been initialized
            Assert.ThrowsException<NullReferenceException>(() => msg.GetData<TestStruct>(0, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data)));
            Assert.ThrowsException<NullReferenceException>(() => msg.GetData<TestStruct>(0, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data)));

            // cotp is however initialized by default
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => msg.GetData<TestStruct>(0, (byte)IsoTcpDataType.COTPData));
            // initialize it
            msg.AddData(12345, (byte)IsoTcpDataType.STEP7RegularData);
            msg.AddData(12345, (byte)IsoTcpDataType.STEP7ParamData);
            // If there is data, but there isnt enough, throw out of range exception on all three
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => msg.GetData<TestStruct>(0, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => msg.GetData<TestStruct>(0, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => msg.GetData<TestStruct>(0, (byte)IsoTcpDataType.COTPData));
        }
        [TestMethod()]
        [DataRow((byte)198, ushort.MinValue, byte.MinValue, (ushort)12456, (byte)243, (ushort)1)]
        [DataRow((byte)10, ushort.MaxValue, byte.MinValue, (ushort)12456, (byte)234, (ushort)14567)]
        [DataRow((byte)124, ushort.MinValue, byte.MaxValue, (ushort)0, (byte)1, (ushort)24567)]
        [DataRow((byte)0, (ushort)19999, (byte)0, (ushort)10094, byte.MinValue, ushort.MinValue)]
        [DataRow(byte.MaxValue, (ushort)0, (byte)10, (ushort)5, byte.MaxValue, ushort.MaxValue)]
        [DataRow(byte.MinValue, ushort.MaxValue, byte.MinValue, ushort.MaxValue, byte.MinValue, ushort.MaxValue)]
        [DataRow((byte)5, ushort.MinValue, byte.MaxValue, ushort.MinValue, byte.MaxValue, ushort.MinValue)]

        public void SerializeDeserializeTest(byte dataParam1, ushort dataParam2, byte parameterParam1, ushort parameterParam2, byte cotpParam1, ushort cotpParam2)
        {
            TestStruct s1 = new TestStruct { b1 = 1, b2 = 123, b3 = 102, b4 = 0 };
            IsoTcpMessage msg = new();
            msg.AddData(dataParam1, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));
            msg.AddData(dataParam2, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));
            msg.AddData(parameterParam1, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));
            msg.AddData(parameterParam2, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));
            msg.AddData(cotpParam1, (byte)IsoTcpDataType.COTPData);
            msg.AddData(cotpParam2, (byte)IsoTcpDataType.COTPData);

            msg.AddData(s1, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));
            msg.AddData(s1, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));
            msg.AddData(s1, (byte)IsoTcpDataType.COTPData);

            byte[] returnBytes = new byte[msg.Size];
            msg.Serialize(returnBytes);

            IsoTcpMessage response = new();
            ReadOnlySpan<byte> returnBytesSpan = new ReadOnlySpan<byte>(returnBytes);
            response.Deserialize(returnBytesSpan);
            Assert.AreEqual(msg, response);
        }

        [TestMethod]
        [DataRow(byte.MaxValue)]
        [DataRow((byte)(byte.MaxValue / 2))]
        [DataRow(byte.MinValue)]
        public void STEP7MessageSize(byte param_1)
        {

            IsoTcpMessage msg = new();
            int expectedsize = msg.TPKT.Size + msg.COTP.Size; // Only COTP-header and TPKT-header are default-initialized
            Assert.AreEqual(expectedsize, msg.Size);

            expectedsize += msg.AddData(param_1, (byte)(IsoTcpDataType.STEP7RegularData | IsoTcpDataType.STEP7Data));

            Assert.AreEqual(expectedsize, msg.Size);

            expectedsize += msg.AddData(param_1, (byte)(IsoTcpDataType.STEP7ParamData | IsoTcpDataType.STEP7Data));

            Assert.AreEqual(expectedsize, msg.Size);

            expectedsize += msg.AddData(param_1, (byte)IsoTcpDataType.COTPData);

            Assert.AreEqual(expectedsize, msg.Size);


        }
    }
}
