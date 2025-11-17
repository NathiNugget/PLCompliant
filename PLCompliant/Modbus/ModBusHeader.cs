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
    [StructLayout(LayoutKind.Explicit, Size = 7, CharSet = CharSet.Ansi)]
    public struct ModBusHeader : IProtocolHeader
    {
        [FieldOffset(0)] public UInt16 transactionIdentifier;
        [FieldOffset(2)] public UInt16 protocolIdentifier;
        [FieldOffset(4)] public UInt16 length;
        [FieldOffset(6)] public byte unitID;

        public int Size { get { return 7; } }

        public void Deserialize(byte[] inputBuffer)
        {
            var index = 0;
            transactionIdentifier = BitConverter.ToUInt16(inputBuffer, index);
            index += sizeof(UInt16);
            protocolIdentifier = BitConverter.ToUInt16(inputBuffer, index);
            index += sizeof(UInt16);
            length = BitConverter.ToUInt16(inputBuffer, index);
            index += sizeof(UInt16);
            unitID = inputBuffer[index];


        }

        public byte[] Serialize()
        {
            byte[] buffer = new byte[Marshal.SizeOf(this)];
            var index = 0;
            Array.Copy(BitConverter.GetBytes(EndianConverter.FromHostToNetwork(transactionIdentifier)), 0, buffer, index, Marshal.SizeOf(transactionIdentifier));
            index += Marshal.SizeOf(transactionIdentifier);
            Array.Copy(BitConverter.GetBytes(EndianConverter.FromHostToNetwork(protocolIdentifier)), 0, buffer, index, Marshal.SizeOf(protocolIdentifier));

            index += Marshal.SizeOf(protocolIdentifier);
            Array.Copy(BitConverter.GetBytes(EndianConverter.FromHostToNetwork(length)), 0, buffer, index, Marshal.SizeOf(length));

            index += Marshal.SizeOf(length);

            buffer[index] = unitID;



            return buffer;
        }

    }
}
