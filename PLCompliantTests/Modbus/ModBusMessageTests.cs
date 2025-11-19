using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCompliant.Modbus;
using PLCompliant.Uilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PLCompliant.Modbus.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class ModBusMessageTests
    {


        //Crete
        [TestMethod()]
        public void ModBusMessageExists()
        {
            ModBusMessage msg = new(new(), new());
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

        public void AddDataTest(ushort param_1, byte param_2, ushort param_3, byte param_4)
        {
            ushort expected_length = 2;
            ModBusMessage msg = new(new(), new());
            msg.AddData(param_1);
            expected_length += (ushort)Marshal.SizeOf(param_1);
            Assert.AreEqual(msg.Header.length, expected_length);


            msg.AddData(param_2);
            expected_length += (ushort)Marshal.SizeOf(param_2);
            Assert.AreEqual(msg.Header.length, expected_length);
            msg.AddData(param_3);
            expected_length += (ushort)Marshal.SizeOf(param_3);
            Assert.AreEqual(msg.Header.length, expected_length);
            msg.AddData(param_4);
            expected_length += (ushort)Marshal.SizeOf(param_4);
            Assert.AreEqual(msg.Header.length, expected_length);
            Assert.AreEqual(msg.Data.Size, expected_length - 1);
            // Converted manually to extract data simulating network extraction because they are wrapped for network when they are added. 
            ushort param_1_actual = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(msg.Data._payload, 0)); 
            byte param_2_acutal = msg.Data._payload[2];
            ushort param_3_actual = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(msg.Data._payload, 3)); 
            byte param_4_acutal = msg.Data._payload[5];
            Assert.AreEqual(param_1, param_1_actual);
            Assert.AreEqual(param_2, param_2_acutal);
            Assert.AreEqual(param_3, param_3_actual);
            Assert.AreEqual(param_4, param_4_acutal);


        }

        [TestMethod]
        [DataRow((uint)0)]
        [DataRow((uint)255)]
        public void AddDataByteArrayTest(uint size)
        {
            uint expectedlength = size + 2; 
            ModBusMessage msg = new(new(), new());
            byte[] arr = new byte[size];
            msg.AddData(arr);
            Assert.AreEqual(msg.Header.length, expectedlength);
            Assert.AreEqual(msg.Data.Size, (int)expectedlength-1);

            //Repeat
            expectedlength += size; 
            msg.AddData(arr);
            Assert.AreEqual(msg.Header.length, expectedlength);
            Assert.AreEqual(msg.Data.Size, (int)expectedlength - 1);


        }

        [TestMethod]
        [DataRow((uint)256)]
        [DataRow((uint)500000)]

        public void AddDataByteArrayTooLargeTest(uint size)
        {
            uint expectedlength = size + 2;
            ModBusMessage msg = new(new(), new());
            byte[] arr = new byte[size];

            Assert.ThrowsException<ArgumentException>(() => msg.AddData(arr)); 

        }



        [TestMethod()]
        public void ModbusFactoryCreateDeviceWhatever()
        {

            ModBusMessageFactory factory = new ModBusMessageFactory();
            ModBusMessage expected = new(new(), new((byte)ModBusCommandType.read_device_information, Array.Empty<byte>()));
            byte productid = 0x2;
            expected.AddData(0x0E);
            expected.AddData(productid);
            expected.AddData(0x0);
            
            ModBusMessage actual = factory.CreateReadDeviceInformation(new(), productid);
            Assert.AreEqual(expected, actual);


        }

        [TestMethod]

        public void ModBusMessageEqualsWithNullAndFactory() {
            var data = new ModBusData { _functionCode = (byte)ModBusCommandType.get_slave_id, _payload = [] };
            var expected = new ModBusMessage(new(1, 2, 3), data);

            ModBusMessageFactory factory = new(); 
            ModBusMessage actual = factory.CreateGetSlaveID(new(1,2,3));


            

            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [DataRow((ushort)1, (ushort)0xFF, (byte)0xA, (byte)0xA, (ushort)3, (byte)5, (ushort)6)]
        [DataRow(ushort.MaxValue, (ushort)0xFF, (byte)0xA, byte.MaxValue, (ushort)3, byte.MaxValue, ushort.MaxValue)]
        [DataRow(ushort.MinValue, (ushort)0xFF, (byte)0xA, byte.MinValue, (ushort)3, (byte)5, ushort.MinValue)]
        [DataRow((ushort)2, ushort.MinValue, (byte)0xD, (byte)0xF, ushort.MinValue, byte.MinValue, (ushort)6)]
        [DataRow((ushort)3, ushort.MaxValue, (byte)0xF, (byte)0x2A, ushort.MaxValue, (byte)5, (ushort)6)]
        [DataRow((ushort)4, (ushort)0xFF, byte.MinValue, (byte)0xBA, (ushort)3, (byte)5, (ushort)6)]
        [DataRow((ushort)5, (ushort)0xFF, byte.MaxValue, (byte)0xA9, (ushort)3, (byte)5, (ushort)6)]

        public void SerializeDeserializeTest(ushort transmodifier, ushort protidentifier, byte unitid, byte functioncode, ushort param_1, byte param_2, ushort param_3)
        {
            ModBusMessage msg = new(new(transmodifier, protidentifier, unitid), new());
            msg.AddData(param_1);
            msg.AddData(param_2);
            msg.AddData(param_3);

            byte[] returnbytes = msg.Serialize();
            //header
            ModBusMessage response = new(new ModBusHeader(), new ModBusData());
            byte[] header_bytes = new byte[Marshal.SizeOf<ModBusHeader>()];
            Array.Copy(returnbytes, 0, header_bytes, 0, header_bytes.Length);
            response.DeserializeHeader(header_bytes);
            //data
            byte[] payload_data = new byte[response.Header.length - 1];
            Array.Copy(returnbytes, Marshal.SizeOf<ModBusHeader>(), payload_data, 0, payload_data.Length);
            response.DeserializeData(payload_data);
            Assert.AreEqual(msg, response);
        }

        [TestMethod]
        public void TestModBusMessageEqualsWithNullAndFactory()
        {
            ModBusMessage msg = new(new(), new());
            Assert.IsFalse(msg.Equals(null));
            Assert.IsFalse(msg.Equals(new ModBusMessageFactory())); 
        }

        [TestMethod]
        [DataRow(byte.MaxValue)]
        [DataRow((byte)(byte.MaxValue/2))]
        [DataRow(byte.MinValue)]
        public void ModBusMessageSizeAndTotalSize(byte param_1)
        {
            ModBusMessage msg = new(new(), new());
            msg.AddData(param_1);
            int expectedsize = 7 + +1 + 1; //Header + function code + data
            Assert.AreEqual(expectedsize, msg.TotalSize); 
        }

   
    }
}