using PLCompliant.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Modbus
{
    public class ModBusData : IProtocolData
    {
        public byte _functionCode;
        public byte[] _payload = [];

        public ModBusData()
        {
        }

        public ModBusData(byte functionCode, byte[] payload)
        {
            _functionCode = functionCode;
            _payload = payload;

        }


        public byte[] Serialize()
        {
            byte[] buffer = new byte[Marshal.SizeOf(_functionCode) + _payload.Length];
            buffer[0] = _functionCode;
            Array.Copy(_payload, 0, buffer, 1, _payload.Length);
            return buffer;
        }
        public void Deserialize(byte[] inputBuffer)
        {
            var index = 0;
            _functionCode = inputBuffer[index];
            index += sizeof(byte);
            Array.Resize(ref _payload, inputBuffer.Length - index);
            Array.Copy(inputBuffer, index, _payload, 0, inputBuffer.Length - index);
        }
        public int Size { get { return PayloadSize + Marshal.SizeOf(_functionCode); } }
        public ushort PayloadSize { get => (ushort)_payload.Length;  }

        public override bool Equals(object? other)
        {
            if (other is null || other is not ModBusData) return false;
            ModBusData other_data = (ModBusData)other;
            return (Size == other_data.Size && _functionCode == other_data._functionCode && _payload.SequenceEqual(other_data._payload));
        }

    }

    
}
