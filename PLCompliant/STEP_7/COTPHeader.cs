using PLCompliant.Interface;
using System.Runtime.InteropServices;
namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This struct represents the header of a COTP-packet 
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 1, CharSet = CharSet.Ansi)]
    public struct COTPHeader : IProtocolHeader
    {
        [FieldOffset(0)] private byte _length;

        #region properties
        /// <summary>
        /// Property for length in bytes
        /// </summary>
        public byte Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// Size of this struct in bytes
        /// </summary>
        public int Size
        {
            get
            {
                return Marshal.SizeOf(this);
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public COTPHeader()
        {
            _length = 0;
        }
        #endregion

        #region methods
        /// <summary>
        /// Deserialize data inside the buffer at the specified index
        /// </summary>
        /// <param name="inputBuffer">Buffer to read from</param>
        /// <param name="startIndex">Index to read from</param>
        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
            _length = inputBuffer[startIndex];
        }

        /// <summary>
        /// Serialize data for network transmission
        /// </summary>
        /// <returns>A byte array of the data in this struct</returns>
        public byte[] Serialize()
        {
            byte[] outData = new byte[Size];
            outData[0] = _length;
            return outData;
        }
        #endregion
    }
}
