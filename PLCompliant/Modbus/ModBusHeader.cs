using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace PLCompliant.Modbus
{
    /// <summary>
    /// This struct is the header of a Modbus message sent over TCP. Because of the protcol, we have to specify order of the bytes as well
    /// as specifying the size to be 7 bytes explicitely
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 7, CharSet = CharSet.Ansi)]
    public struct ModBusHeader : IProtocolHeader, IEndianConvertable
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
        #endregion

        #region Methods
        /// <inheritdoc/>
        public void Serialize(Span<byte> serializedObj)
        {
            ModBusHeader headerCpy = this;
            headerCpy.FromHostToNetwork();
            ReadOnlySpan<ModBusHeader> span = [headerCpy];
            MemoryMarshal.AsBytes(span).CopyTo(serializedObj);
        }
        /// <inheritdoc/>
        public void Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            this = MemoryMarshal.AsRef<ModBusHeader>(inputBuffer.Slice(0, this.Size));
            this.FromNetworkToHost();
        }
        /// <inheritdoc/>
        public void FromHostToNetwork()
        {
            transactionIdentifier = EndianConverter.FromHostToNetwork(transactionIdentifier);
            protocolIdentifier = EndianConverter.FromHostToNetwork(protocolIdentifier);
            length = EndianConverter.FromHostToNetwork(length);
        }
        /// <inheritdoc/>
        public void FromNetworkToHost()
        {
            transactionIdentifier = EndianConverter.FromNetworkToHost(transactionIdentifier);
            protocolIdentifier = EndianConverter.FromNetworkToHost(protocolIdentifier);
            length = EndianConverter.FromNetworkToHost(length);
        }

        #endregion

    }
}
