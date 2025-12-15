using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    [StructLayout(LayoutKind.Explicit, Size = 4, CharSet = CharSet.Ansi)]
    public struct TPKTHeader : IProtocolHeader
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
        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
            _version = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_version);
            _reserved = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_reserved);
            _length = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, startIndex));
        }

        public byte[] Serialize()
        {
            int startIndex = 0;
            byte[] outData = new byte[Size];
            outData[startIndex] = _version;
            startIndex += Marshal.SizeOf(_version);
            outData[startIndex] = _reserved;
            startIndex += Marshal.SizeOf(_reserved);
            var lengthAsBytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(_length));
            Array.Copy(lengthAsBytes, 0, outData, startIndex, lengthAsBytes.Length);
            return outData;
        }
    }
}
