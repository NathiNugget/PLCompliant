using PLCompliant.Interface;
using PLCompliant.Modbus;
using PLCompliant.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.STEP_7
{
    [StructLayout(LayoutKind.Explicit, Size = 4, CharSet = CharSet.Ansi)]
    public struct STEP7DataHeader : IProtocolHeader, IEndianConvertable
    {
        [FieldOffset(0)] private byte _returnCode;
        [FieldOffset(1)] private byte _transportType;
        [FieldOffset(2)] private UInt16 _length;



        public UInt16 Length
        {
            get { return _length; }
            set { _length = value; }
        }



        public byte TransportType
        {
            get { return _transportType; }
            set { _transportType = value; }
        }

        public byte ReturnCode
        {
            get { return _returnCode; }
            set { _returnCode = value; }
        }
        /// <inheritdoc/>
        public int Size { get { return Marshal.SizeOf<STEP7DataHeader>(); } }
        /// <inheritdoc/>
        public int Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            this = MemoryMarshal.AsRef<STEP7DataHeader>(inputBuffer);
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
        /// <inheritdoc/>
        public void Serialize(Span<byte> serializedObj)
        {
            STEP7DataHeader headerCpy = this;
            headerCpy.FromHostToNetwork();
            ReadOnlySpan<STEP7DataHeader> span = [headerCpy];
            MemoryMarshal.AsBytes(span).CopyTo(serializedObj.Slice(0, Size));
        }
    }
}
