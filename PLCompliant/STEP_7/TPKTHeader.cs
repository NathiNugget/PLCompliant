using PLCompliant.Interface;
using PLCompliant.Modbus;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    [StructLayout(LayoutKind.Explicit, Size = 4, CharSet = CharSet.Ansi)]
    public struct TPKTHeader : IProtocolHeader, IEndianConvertable
    {
        [FieldOffset(0)] private byte _version;
        [FieldOffset(1)] private byte _reserved;
        [FieldOffset(2)] private UInt16 _length;


        public UInt16 Length
        {
            get { return _length; }
            set { _length = value; }


        }
        public byte Reserved
        {
            get { return _reserved; }
            set { _reserved = value; }
        }

        public byte Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <inheritdoc/>
        public int Size
        {
            get
            {
                return Marshal.SizeOf(this);
            }
        }
        public TPKTHeader(byte version)
        {
            _version = version;
            _length = 0;
            _reserved = 0;
        }
        /// <inheritdoc/>
        public void Serialize(Span<byte> serializedObj)
        {
            TPKTHeader headerCpy = this;
            headerCpy.FromHostToNetwork();
            ReadOnlySpan<TPKTHeader> span = [headerCpy];
            MemoryMarshal.AsBytes(span).CopyTo(serializedObj.Slice(0, this.Size));
        }
        /// <inheritdoc/>
        public int Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            this = MemoryMarshal.AsRef<TPKTHeader>(inputBuffer.Slice(0, this.Size));
            this.FromNetworkToHost();
            return this.Size;
        }
        /// <inheritdoc/>
        public void FromHostToNetwork()
        {
            _length = EndianConverter.FromHostToNetwork(_length);
        }
        /// <inheritdoc/>
        public void FromNetworkToHost()
        {
            _length = EndianConverter.FromNetworkToHost(_length);
        }
    }
}
