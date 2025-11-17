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
        public byte functionCode;
        public byte[] payload = [];

        public ModBusData()
        {
        }

        public byte[] Serialize()
        {
            byte[] buffer = new byte[Marshal.SizeOf(functionCode) + payload.Length];
            buffer[0] = functionCode;
            Array.Copy(payload, 0, buffer, 1, payload.Length);
            return buffer;
        }
        public void Deserialize(byte[] inputBuffer)
        {
            var index = 0;
            functionCode = inputBuffer[index];
            index += sizeof(byte);
            Array.Resize(ref payload, inputBuffer.Length - index);
            Array.Copy(inputBuffer, index, payload, 0, inputBuffer.Length - index);
        }
        public int Size { get { return payload.Length + Marshal.SizeOf(functionCode); } }

    }
}
