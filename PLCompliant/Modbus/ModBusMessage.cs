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
        public ushort PayloadSize { get => (ushort)_data.PayloadSize; }
        public ushort TotalSize { get => (ushort)(Data.Size + Header.Size); }


        public ModBusMessage(ModBusHeader header, ModBusData data)
        {
            _header = header;
            _data = data;
        }

        public void AddData(UInt16 inputData)
        {
            var oldSize = _data.PayloadSize;
            var newSize = _data._payload.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref _data._payload, newSize);
            byte[] bytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(inputData));
            Array.Copy(bytes,0, _data._payload,oldSize, bytes.Length);
            _header.length += sizeof(UInt16);
        }

        public void AddData(byte inputData)
        {
            var newSize = _data._payload.Length + Marshal.SizeOf<byte>();
            Array.Resize(ref _data._payload, newSize);
            _data._payload[newSize - 1] = inputData;
            _header.length += sizeof(byte); 
        }

        public void AddData(byte[] stringData)
        {
            if (stringData.Length > byte.MaxValue)
            {
                throw new ArgumentException("Input length was greater than allowed in a byte"); 
            }
            byte stringSize = (byte)stringData.Length;
            if(stringSize == 0) { return; }
            var oldSize = Data._payload.Length;
            var newSize = _data._payload.Length + stringSize;
            Array.Resize(ref _data._payload, newSize);
            Array.Copy(stringData,0, _data._payload,oldSize, stringSize);
            _header.length += (ushort)stringSize;
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
        public int DataSize { get { return Data.Size; } }

        public override bool Equals(object? other)
        {
            if (other == null) return false;
            if (other is not ModBusMessage) return false;
            ModBusMessage other_msg = (ModBusMessage)other;
            return (Data.Equals(other_msg.Data) && Header.Equals(other_msg.Header));
            
        }

        

        
    }
}
