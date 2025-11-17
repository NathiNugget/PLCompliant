using PLCompliant.Interface;
using PLCompliant.Uilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PLCompliant.Modbus
{
    public class ModBusMessage : IProtocolMessage
    {
        ModBusHeader _header = new();
        ModBusData _data = new();


        public ModBusHeader Header { get { return _header; } }
        public ModBusData Data { get { return _data; } }


        public ModBusMessage(ModBusHeader header, ModBusData data)
        {
            _header = header;
            _data = data;
        }

        public void AddData(UInt16 inputData)
        {
            var newSize = _data.payload.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref _data.payload, newSize);
            byte[] bytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(inputData));
            Array.Copy(bytes, _data.payload, bytes.Length);
        }

        public void AddData(byte inputData)
        {
            var newSize = _data.payload.Length + Marshal.SizeOf<byte>();
            Array.Resize(ref _data.payload, newSize);
            _data.payload[newSize - 1] = inputData;

        }

        public byte[] Serialize()
        {
            byte[] headerData = _header.Serialize();
            byte[] payloadData = _data.Serialize();
            byte[] result = new byte[headerData.Length + payloadData.Length];
            Array.Copy(headerData, result, headerData.Length);
            Array.Copy(payloadData, 0, result, headerData.Length, payloadData.Length);
            return result;

        }
        public void DeserializeHeader(byte[] inputBuffer)
        {
            _header.Deserialize(inputBuffer);
        }
        public void DeserializeData(byte[] inputBuffer)
        {
            _data.Deserialize(inputBuffer);
        }
        public int DataSize { get { return _data.Size; } }
    }
}
