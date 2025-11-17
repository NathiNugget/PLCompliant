using PLCompliant.Interface;
using PLCompliant.Uilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Modbus
{
    public class ModBusMessage : IProtocolMessage
    {
        ModBusHeader header = new();
        ModBusData data = new();


        public void AddData(UInt16 inputData)
        {
            var newSize = data.payload.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref data.payload, newSize);
            byte[] bytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(inputData));
            Array.Copy(bytes, data.payload, bytes.Length);
        }

        public byte[] Serialize()
        {
            byte[] headerData = header.Serialize();
            byte[] payloadData = data.Serialize();
            byte[] result = new byte[headerData.Length + payloadData.Length];
            Array.Copy(headerData, result, headerData.Length);
            Array.Copy(payloadData, 0, result, headerData.Length, payloadData.Length);
            return result;

        }
        public void DeserializeHeader(byte[] inputBuffer)
        {
            header.Deserialize(inputBuffer);
        }
        public void DeserializeData(byte[] inputBuffer)
        {
            data.Deserialize(inputBuffer);
        }
        public int DataSize { get { return data.Size; } }
    }
}
