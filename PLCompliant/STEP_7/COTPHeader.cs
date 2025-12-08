using PLCompliant.Interface;
using System;
using System.Net;
using System.Runtime.InteropServices;
namespace PLCompliant.STEP_7
{
    [StructLayout(LayoutKind.Explicit, Size = 1, CharSet = CharSet.Ansi)]
    public struct COTPHeader : IProtocolHeader
    {
        [FieldOffset(0)] private byte _length;
        [FieldOffset(1)] private byte _pduType;

        public byte Length
        {
            get { return _length; }
            set { _length = value; }
        }
        public byte PduType
        {
            get { return _pduType; }
            set { _pduType = value; }

        }

        public int Size
        {
            get
            {
                return Marshal.SizeOf(this);
            }
        }

        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
            _length = inputBuffer[startIndex];
            _pduType = inputBuffer[startIndex + 1];
        }

        public byte[] Serialize()
        {
            byte[] outData = new byte[Size];
            outData[0] = _length;
            outData[1] = _pduType;
            return outData;
        }
    }
}
