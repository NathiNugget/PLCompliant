using PLCompliant.Interface;
using System.Runtime.InteropServices;
namespace PLCompliant.STEP_7
{
    [StructLayout(LayoutKind.Explicit, Size = 1, CharSet = CharSet.Ansi)]
    public struct COTPHeader : IProtocolHeader
    {
        [FieldOffset(0)] private byte _length;

        public byte Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public int Size
        {
            get
            {
                return Marshal.SizeOf(this);
            }
        }

        public COTPHeader()
        {
            _length = 0;
        }

        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
            _length = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_length);
        }

        public byte[] Serialize()
        {
            byte[] outData = new byte[Size];
            outData[0] = _length;
            return outData;
        }
    }
}
