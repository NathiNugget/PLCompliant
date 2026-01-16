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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public void Serialize(Span<byte> serializedObj)
        {
            serializedObj[0] = _length;
        }
        /// <inheritdoc/>
        public void Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            _length = inputBuffer[0];
        }
    }
}
