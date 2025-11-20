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
    /// <summary>
    /// This struct is the header of a Modbus message sent over TCP. Because of the protcol, we have to specify order of the bytes as well
    /// as specifying the size to be 7 bytes explicitely
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 7, CharSet = CharSet.Ansi)]
    public struct ModBusHeader : IProtocolHeader
    {
        
        [FieldOffset(0)] public UInt16 transactionIdentifier;
        [FieldOffset(2)] public UInt16 protocolIdentifier;
        [FieldOffset(4)] public UInt16 length;
        [FieldOffset(6)] public byte unitID;
        /// <summary>
        /// The normal constructor of a header
        /// </summary>
        /// <param name="transmodifier">The transaction modifier/counter</param>
        /// <param name="protidentifier">Protocol identifier which always has to be 0x0</param>
        /// <param name="unitid">x</param>
        public ModBusHeader(ushort transmodifier, ushort protidentifier, byte unitid)
        {
            unitID = unitid;
            transactionIdentifier = transmodifier;
            protocolIdentifier = protidentifier;
            length = 2;
        }

        public ModBusHeader()
        {
            length = 2; 
        }


        public int Size { get { return Marshal.SizeOf<ModBusHeader>(); } }

        public void Deserialize(byte[] inputBuffer)
        {
            var index = 0;
            transactionIdentifier = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, index)) ;
            index += sizeof(UInt16);
            protocolIdentifier = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, index));
            index += sizeof(UInt16);
            length = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, index));
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
