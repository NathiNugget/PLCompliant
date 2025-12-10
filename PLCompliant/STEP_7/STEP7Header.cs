using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Net;
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
        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
            _protocolId = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_protocolId);
            _messageType = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_messageType);
            _reserved = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, startIndex));
            startIndex += Marshal.SizeOf(_reserved);
            _pduReference = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, startIndex));
            startIndex += Marshal.SizeOf(_pduReference);

            _parameterLength = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, startIndex));
            startIndex += Marshal.SizeOf(_parameterLength);
            _dataLength = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, startIndex));
            startIndex += Marshal.SizeOf(_dataLength);

            if(_messageType == 0x3)
            {
                _errorClass = inputBuffer[startIndex];
                startIndex += Marshal.SizeOf(_errorClass);
                _errorCode = inputBuffer[startIndex];
                startIndex += Marshal.SizeOf(_errorCode);
            }
        }

        public byte[] Serialize()
        {
            int startIndex = 0;
            byte[] outData = new byte[Size];
            outData[startIndex] = _protocolId;
            startIndex += Marshal.SizeOf(_protocolId);
            outData[startIndex] = _messageType;
            startIndex += Marshal.SizeOf(_messageType);

            var reservedAsBytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(_reserved));
            Array.Copy(reservedAsBytes, 0, outData, startIndex, reservedAsBytes.Length);
            startIndex += reservedAsBytes.Length;

            var pduRefAsBytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(_pduReference));
            Array.Copy(pduRefAsBytes, 0, outData, startIndex, pduRefAsBytes.Length);
            startIndex += pduRefAsBytes.Length;

            var paramLengthAsBytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(_parameterLength));
            Array.Copy(paramLengthAsBytes, 0, outData, startIndex, paramLengthAsBytes.Length);
            startIndex += paramLengthAsBytes.Length;

            var dataLengthAsBytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(_dataLength));
            Array.Copy(dataLengthAsBytes, 0, outData, startIndex, dataLengthAsBytes.Length);
            startIndex += dataLengthAsBytes.Length;

            if (_messageType == 0x3)
            {
                outData[startIndex] = _errorClass;
                startIndex += Marshal.SizeOf(_errorClass);
                outData[startIndex] = _errorCode;
                startIndex += Marshal.SizeOf(_errorCode);
            }


            return outData;

        }
    }
}
