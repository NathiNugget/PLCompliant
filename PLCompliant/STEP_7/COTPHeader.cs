using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;
namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This struct represents the header of a COTP-packet 
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 1, CharSet = CharSet.Ansi)]
    public struct COTPHeader : IProtocolHeader, IEquatable<COTPHeader>
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
        public int Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            _length = inputBuffer[0];
            return Size;
        }



        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj is not COTPHeader) return false;
            COTPHeader castedObj = (COTPHeader)obj;
            return Equals(this, castedObj);

        }

        public bool Equals(COTPHeader other)
        {
            return MemoryUtilities.CompareMemory(ref this, ref other);
        }

        public static bool operator ==(COTPHeader left, COTPHeader right)
        {
            return left.Equals(right);

        }
        public static bool operator !=(COTPHeader left, COTPHeader right) { return !(left == right); }




    }
}
