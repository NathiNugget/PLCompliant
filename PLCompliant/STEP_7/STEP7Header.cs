using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This struct contains the fields in a STEP7-header
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 12, CharSet = CharSet.Ansi)]
    public struct STEP7Header : IProtocolHeader, IEndianConvertable, IEquatable<STEP7Header>
    {
        public const byte PRELUDE_LEN = 2;
        public const byte NON_ERROR_LENGTH = 10;
        [FieldOffset(0)] private byte _protocolId;
        [FieldOffset(1)] private byte _messageType;
        [FieldOffset(2)] private UInt16 _reserved;
        [FieldOffset(4)] private UInt16 _pduReference; // this one might be little endian? Should be incremented by master each transmission
        [FieldOffset(6)] private UInt16 _parameterLength;
        [FieldOffset(8)] private UInt16 _dataLength;
        // These two fields are ony present in Ack-Data replies. They should be ignored otherwise and not serialized, only deserialized when needed
        [FieldOffset(10)] private byte _errorClass;
        [FieldOffset(11)] private byte _errorCode;

        public byte ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }


        public byte ErrorClass
        {
            get { return _errorClass; }
            set { _errorClass = value; }
        }
        public byte MyProperty
        {
            get { return _errorClass; }
            set { _errorClass = value; }
        }


        public UInt16 DataLength
        {
            get { return _dataLength; }
            set { _dataLength = value; }
        }

        public UInt16 ParameterLength
        {
            get { return _parameterLength; }
            set { _parameterLength = value; }
        }
        public UInt16 PduReference
        {
            get { return _pduReference; }
            set { _pduReference = value; }
        }

        public UInt16 Reserved
        {
            get { return _reserved; }
            set { _reserved = value; }
        }


        public byte MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }


        public byte ProtocolId
        {
            get { return _protocolId; }
            set { _protocolId = value; }
        }



        public int Size
        {
            get
            {
                if (_messageType == 0x3)
                {
                    return Marshal.SizeOf(this);
                }
                else
                {
                    return NON_ERROR_LENGTH;
                }
            }
        }
        public STEP7Header(byte protocolId, byte messageType, UInt16 pduReference)
        {
            _protocolId = protocolId;
            _messageType = messageType;
            _pduReference = pduReference;
            _dataLength = 0;
            _parameterLength = 0;
            _errorClass = 0;
            _errorCode = 0;
            _reserved = 0;
        }
        /// <inheritdoc/>
        public void Serialize(Span<byte> serializedObj)
        {
            STEP7Header headerCpy = this;
            headerCpy.FromHostToNetwork();
            ReadOnlySpan<STEP7Header> headerSpan = [headerCpy];
            ReadOnlySpan<byte> headerBytes = MemoryMarshal.AsBytes(headerSpan);

            if(_messageType == 0x3)
            {
                headerBytes.CopyTo(serializedObj);
            }
            else
            {
                headerBytes.Slice(0, NON_ERROR_LENGTH).CopyTo(serializedObj);
            }
        }
        /// <inheritdoc/>
        public int Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            // Deserialize prelude first, as we need to know the messagetype
            int index = 0;
            _protocolId = inputBuffer[index];
            index += sizeof(byte);
            _messageType = inputBuffer[index];
            index += sizeof(byte);
            Span<STEP7Header> temp = new(ref this);
            Span<byte> headerSpan = MemoryMarshal.AsBytes(temp);

            if(_messageType == 0x3)
            {
                inputBuffer.Slice(index, Size - index).CopyTo(headerSpan.Slice(index, Size - index));
                index += (Size - index);
            }
            else
            {
                inputBuffer.Slice(index, NON_ERROR_LENGTH - index).CopyTo(headerSpan.Slice(index, NON_ERROR_LENGTH - index));
                index += (NON_ERROR_LENGTH - index);
            }
            
            this.FromNetworkToHost();
            return index;
        }
        /// <inheritdoc/>
        public void FromHostToNetwork()
        {
            _reserved = EndianConverter.FromHostToNetwork(_reserved);
            _pduReference = EndianConverter.FromHostToNetwork(_pduReference);
            _parameterLength = EndianConverter.FromHostToNetwork(_parameterLength);
            _dataLength = EndianConverter.FromHostToNetwork(_dataLength);
        }
        /// <inheritdoc/>
        public void FromNetworkToHost()
        {
            _reserved = EndianConverter.FromNetworkToHost(_reserved);
            _pduReference = EndianConverter.FromNetworkToHost(_pduReference);
            _parameterLength = EndianConverter.FromNetworkToHost(_parameterLength);
            _dataLength = EndianConverter.FromNetworkToHost(_dataLength);
        }




        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj is not STEP7Header) return false;
            STEP7Header castedObj = (STEP7Header)obj;
            return Equals(this, castedObj);

        }

        public bool Equals(STEP7Header other)
        {
            return MemoryUtilities.CompareMemory(ref this, ref other);
        }

        public static bool operator ==(STEP7Header left, STEP7Header right)
        {
            return left.Equals(right);

        }
        public static bool operator !=(STEP7Header left, STEP7Header right) { return !(left == right); }

    }
}
