using PLCompliant.Interface;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    [StructLayout(LayoutKind.Explicit, Size = 12, CharSet = CharSet.Ansi)]
    public struct STEP7Header : IProtocolHeader
    {
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
                // if messagetype is not 0x3 (ack_data), don't include the error codes in size
                int size = Marshal.SizeOf(this);
                if(_messageType != 0x3)
                {
                    size -= (Marshal.SizeOf(_errorClass) + Marshal.SizeOf(_errorCode));
                }
                return size;
            }
        }

        public void Deserialize(byte[] inputBuffer)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
