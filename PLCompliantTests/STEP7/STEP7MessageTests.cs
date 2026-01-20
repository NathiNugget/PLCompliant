using PLCompliant.Enums;
using PLCompliant.Interface;
using PLCompliant.Modbus;
using PLCompliant.STEP_7;
using PLCompliant.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;

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
        public void STEP7MessageDefaultCTOR()
        {
            STEP7Message msg = CreateSTEP7Msg();
            Assert.IsNull(msg.STEP7Data);
            Assert.IsNull(msg.STEP7ParamData);
            Assert.IsNotNull(msg);

        }
        [TestMethod()]
        public void STEP7MessageOtherCTOR()
        {
            STEP7Message msg = new(new(), new(), new());
            Assert.IsNotNull(msg.STEP7Data);
            Assert.IsNotNull(msg.STEP7ParamData);
            Assert.IsNotNull(msg);

            Assert.AreEqual(msg.STEP7ParamData.Size, msg.STEP7Header.ParameterLength);
            Assert.AreEqual(msg.STEP7Data.Size, msg.STEP7Header.DataLength);


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
            // Regular data
            STEP7Message msg = CreateSTEP7Msg();
            var expectedLength = msg.AddData(param1, (byte)IsoTcpDataType.STEP7RegularData);
            Assert.AreEqual(expectedLength, msg.STEP7Header.DataLength);


            expectedLength += msg.AddData(param2, (byte)IsoTcpDataType.STEP7RegularData);
            Assert.AreEqual(expectedLength, msg.STEP7Header.DataLength);
            expectedLength += msg.AddData(param3, (byte)IsoTcpDataType.STEP7RegularData);
            Assert.AreEqual(expectedLength, msg.STEP7Header.DataLength);
            expectedLength += msg.AddData(param4, (byte)IsoTcpDataType.STEP7RegularData);
            Assert.AreEqual(expectedLength, msg.STEP7Header.DataLength);
            Assert.AreEqual(param4, msg.STEP7Data.Data.Data[msg.STEP7Data.Data.Data.Length - 1]); // check that the last item added was infact the value we added
            Assert.AreEqual(expectedLength, msg.STEP7Data.Size);


            // Parameter data

            var expectedLengthParams = msg.AddData(param2, (byte)IsoTcpDataType.STEP7ParamData);
            Assert.AreEqual(expectedLengthParams, msg.STEP7Header.ParameterLength);
            expectedLengthParams += msg.AddData(param3, (byte)IsoTcpDataType.STEP7ParamData);
            Assert.AreEqual(expectedLengthParams, msg.STEP7Header.ParameterLength);
            expectedLengthParams += msg.AddData(param4, (byte)IsoTcpDataType.STEP7ParamData);
            Assert.AreEqual(expectedLengthParams, msg.STEP7Header.ParameterLength);
            Assert.AreEqual(param4, msg.STEP7ParamData.Data[msg.STEP7ParamData.Data.Length - 1]); // check that the last item added was infact the value we added
            Assert.AreEqual(expectedLengthParams, msg.STEP7ParamData.Size);



        }

        [TestMethod]
        [DataRow((uint)0)]
        [DataRow((uint)255)]
        public void AddDataByteArrayTest(uint size)
        {
            // regular data
            STEP7Message msg = CreateSTEP7Msg();
            byte[] arr = new byte[size];
            if(size > 0)
            {
                arr[arr.Length - 1] = 123;
                arr[0] = 45;
            }
            
            int expectedlength = (int)size;
            msg.AddData(arr, (byte)IsoTcpDataType.STEP7RegularData);
            expectedlength += msg.STEP7Data.Header.Size; // add header since it is initialized in AddData
            Assert.AreEqual(msg.STEP7Header.DataLength, expectedlength);
            Assert.AreEqual(msg.STEP7Data.Size, (int)expectedlength);
            if(size > 0)
            {
                Assert.AreEqual(arr[arr.Length - 1], msg.STEP7Data.Data.Data[arr.Length - 1]);
                Assert.AreEqual(arr[0], msg.STEP7Data.Data.Data[0]);
            }
            

            //Repeat
            expectedlength += (int)size;
            msg.AddData(arr, (byte)IsoTcpDataType.STEP7RegularData);
            Assert.AreEqual(expectedlength, msg.STEP7Header.DataLength);
            Assert.AreEqual((int)expectedlength, msg.STEP7Data.Size);
            if(size > 0)
            {
                Assert.AreEqual(arr[arr.Length - 1], msg.STEP7Data.Data.Data[msg.STEP7Data.Data.Data.Length - 1]);
                Assert.AreEqual(arr[0], msg.STEP7Data.Data.Data[arr.Length]);
            }
            

            // param data

            int expectedlengthParams = (int)size;
            msg.AddData(arr, (byte)IsoTcpDataType.STEP7ParamData);
            Assert.AreEqual(expectedlengthParams, msg.STEP7Header.ParameterLength);
            Assert.AreEqual((int)expectedlengthParams, msg.STEP7ParamData.Size);
            if(size > 0)
            {
                Assert.AreEqual(arr[arr.Length - 1], msg.STEP7ParamData.Data[arr.Length - 1]);
                Assert.AreEqual(arr[0], msg.STEP7ParamData.Data[0]);
            }
            

            //Repeat
            expectedlengthParams += (int)size;
            msg.AddData(arr, (byte)IsoTcpDataType.STEP7ParamData);
            Assert.AreEqual(expectedlengthParams, msg.STEP7Header.ParameterLength);
            Assert.AreEqual((int)expectedlengthParams, msg.STEP7ParamData.Size);
            if(size > 0)
            {
                Assert.AreEqual(arr[arr.Length - 1], msg.STEP7ParamData.Data[msg.STEP7ParamData.Data.Length - 1]);
                Assert.AreEqual(arr[0], msg.STEP7ParamData.Data[arr.Length]);
            }
            

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
        [TestMethod]
        public void AddDataStructTest()
        {
            // regular data
            TestStruct s1 = new TestStruct();
            STEP7Message msg = CreateSTEP7Msg();
            var expectedLength = msg.AddData(s1, (byte)IsoTcpDataType.STEP7RegularData);
            Assert.AreEqual(expectedLength, msg.STEP7Header.DataLength);
            Assert.AreEqual(expectedLength, msg.STEP7Data.Size);

            int addedData = msg.AddData(s1, (byte)IsoTcpDataType.STEP7RegularData);

            Assert.AreEqual(addedData, Marshal.SizeOf(s1));

            expectedLength += addedData;
            
            Assert.AreEqual(expectedLength, msg.STEP7Header.DataLength);
            Assert.AreEqual(expectedLength, msg.STEP7Data.Size);

            // param data

            var expectedLengthParams = msg.AddData(s1, (byte)IsoTcpDataType.STEP7ParamData);
            Assert.AreEqual(expectedLengthParams, msg.STEP7Header.ParameterLength);
            Assert.AreEqual(expectedLengthParams, msg.STEP7ParamData.Size);

            int addedDataParams = msg.AddData(s1, (byte)IsoTcpDataType.STEP7ParamData);

            Assert.AreEqual(addedDataParams, Marshal.SizeOf(s1));

            expectedLengthParams += addedDataParams;

            Assert.AreEqual(expectedLengthParams, msg.STEP7Header.ParameterLength);
            Assert.AreEqual(expectedLengthParams, msg.STEP7ParamData.Size);

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
            STEP7Message msg = CreateSTEP7Msg();

            // Regular data
            var expectedLength = msg.AddData(s1, (byte)IsoTcpDataType.STEP7RegularData);
            expectedLength += msg.AddData(s2, (byte)IsoTcpDataType.STEP7RegularData);
            TestStruct output1 = msg.GetData<TestStruct>(0, (byte)IsoTcpDataType.STEP7RegularData);
            TestStruct output2 = msg.GetData<TestStruct>(Marshal.SizeOf<TestStruct>(), (byte)IsoTcpDataType.STEP7RegularData); 


            Assert.AreEqual(s1, output1);
            Assert.AreEqual(s2, output2);


            var expectedLengthParams = msg.AddData(s1, (byte)IsoTcpDataType.STEP7ParamData);
            expectedLengthParams += msg.AddData(s2, (byte)IsoTcpDataType.STEP7ParamData);
            TestStruct output1Param = msg.GetData<TestStruct>(0, (byte)IsoTcpDataType.STEP7ParamData);
            TestStruct output2Param = msg.GetData<TestStruct>(Marshal.SizeOf<TestStruct>(), (byte)IsoTcpDataType.STEP7ParamData);


            Assert.AreEqual(s1, output1Param);
            Assert.AreEqual(s2, output2Param);

        }
        [TestMethod]
        public void GetDataStructTestFailNotEnoughData()
        {
            STEP7Message msg = CreateSTEP7Msg();
            // Null reference if the data and param segments havent been initialized
            Assert.ThrowsException<NullReferenceException>(() => msg.GetData<TestStruct>(0, (byte)IsoTcpDataType.STEP7RegularData));
            Assert.ThrowsException<NullReferenceException>(() => msg.GetData<TestStruct>(0, (byte)IsoTcpDataType.STEP7ParamData));
            // initialize it
            msg.AddData(12345, (byte)IsoTcpDataType.STEP7RegularData);
            msg.AddData(12345, (byte)IsoTcpDataType.STEP7ParamData);
            // If there is data, but there isnt enough, throw out of range exception
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => msg.GetData<TestStruct>(0, (byte)IsoTcpDataType.STEP7RegularData));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => msg.GetData<TestStruct>(0, (byte)IsoTcpDataType.STEP7ParamData));
        }
        [TestMethod()]
        [DataRow((byte)0xFF, (byte)0xA, (byte)0xA,     (byte)5, (byte)6, (byte)198, ushort.MinValue, byte.MinValue, (ushort)12456)]
        [DataRow((byte)0xFF, (byte)0xA, byte.MaxValue,  byte.MaxValue, byte.MaxValue, (byte)10, ushort.MaxValue, byte.MinValue, (ushort)12456)]
        [DataRow((byte)0xFF, (byte)0xA, byte.MinValue, (byte)5, byte.MinValue, (byte)124, ushort.MinValue, byte.MaxValue, (ushort)0)]
        [DataRow(byte.MinValue, (byte)0xD, (byte)0xF,    byte.MinValue, (byte)6, (byte)0, (ushort)19999, (byte)0, (ushort)10094)]
        [DataRow(byte.MaxValue, (byte)0xF, (byte)0x2A,  (byte)5, (byte)6, byte.MaxValue, (ushort)0, (byte)10, (ushort)5)]
        [DataRow((byte)0xFF, byte.MinValue, (byte)0xBA, (byte)5, (byte)6, byte.MinValue, ushort.MaxValue, byte.MinValue, ushort.MaxValue)]
        [DataRow((byte)0xFF, byte.MaxValue, (byte)0xA9, (byte)5, (byte)6, (byte)5, ushort.MinValue, byte.MaxValue, ushort.MinValue)]

        public void SerializeDeserializeTest(byte protocolId, byte messageType, byte pduReference, byte returnCode, byte transportType, byte dataParam1, ushort dataParam2, byte parameterParam1, ushort parameterParam2)
        {
            STEP7Header header = new(protocolId, messageType, pduReference);
            STEP7ParameterData paramData = new();
            STEP7DataMessage dataSegment = new(returnCode, transportType);
            STEP7Message msg = new(header, paramData, dataSegment);
            msg.AddData(dataParam1, (byte)IsoTcpDataType.STEP7RegularData);
            msg.AddData(dataParam2, (byte)IsoTcpDataType.STEP7RegularData);
            msg.AddData(parameterParam1, (byte)IsoTcpDataType.STEP7ParamData);
            msg.AddData(parameterParam2, (byte)IsoTcpDataType.STEP7ParamData);

            byte[] returnBytes = new byte[msg.Size];
            msg.Serialize(returnBytes);

            STEP7Message response = CreateSTEP7Msg();
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

            STEP7Message msg = CreateSTEP7Msg();
            int expectedsize = msg.STEP7Header.Size; // Only header is default-initialized
            Assert.AreEqual(expectedsize, msg.Size);

            expectedsize += msg.AddData(param_1, (byte)IsoTcpDataType.STEP7RegularData);

            Assert.AreEqual(expectedsize, msg.Size);

            expectedsize += msg.AddData(param_1, (byte)IsoTcpDataType.STEP7ParamData);

            Assert.AreEqual(expectedsize, msg.Size);


        }


    }
}

