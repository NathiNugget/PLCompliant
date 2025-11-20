using PLCompliant.Interface;
using PLCompliant.Uilities;
using System.Runtime.InteropServices;

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


        #region constructors
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
        /// <summary>
        /// Empty constructor mostly used for tests and other standard initialization
        /// </summary>
        public ModBusHeader()
        {
            length = 2;
        }

        #endregion

        #region properties
        /// <summary>
        /// Size of the header struct in bytes
        /// </summary>
        public int Size { get { return Marshal.SizeOf<ModBusHeader>(); } }
        /// <summary>
        /// Deserialize the struct from bytes to human readable header-data
        /// </summary>
        /// <param name="inputBuffer">Header bytes received from the network</param>
        public void Deserialize(byte[] inputBuffer)
        {
            var index = 0;
            transactionIdentifier = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, index));
            index += sizeof(UInt16);
            protocolIdentifier = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, index));
            index += sizeof(UInt16);
            length = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, index));
            index += sizeof(UInt16);
            unitID = inputBuffer[index];


        }
        #endregion

        #region methods

        /// <summary>
        /// Serialize the header data to bytes
        /// </summary>
        /// <returns>The header data as bytes for network transmission, so endianness is set to network order</returns>
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

        #endregion

    }
}
