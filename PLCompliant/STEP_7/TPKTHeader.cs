using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This struct represents the TPKT-header to build on top of TCP
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 4, CharSet = CharSet.Ansi)]
    public struct TPKTHeader : IProtocolHeader
    {
        [FieldOffset(0)] private byte _version;
        [FieldOffset(1)] private byte _reserved;
        [FieldOffset(2)] private UInt16 _length;

        #region properties
        /// <summary>
        /// Length of the header
        /// </summary>
        public UInt16 Length
        {
            get { return _length; }
            set { _length = value; }


        }
        /// <summary>
        /// Reserved byte
        /// </summary>
        public byte Reserved
        {
            get { return _reserved; }
            set { _reserved = value; }
        }




        /// <summary>
        /// Version byte
        /// </summary>
        public byte Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// Size of the struct
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
        /// Constructor for this struct
        /// </summary>
        /// <param name="version"></param>
        public TPKTHeader(byte version)
        {
            _version = version;
            _length = 0;
            _reserved = 0;
        }
        #endregion

        #region methods
        /// <summary>
        /// Deserialize data received from the network
        /// </summary>
        /// <param name="inputBuffer">Buffer to read from</param>
        /// <param name="startIndex">Index to start reading from</param>
        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
            _version = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_version);
            _reserved = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_reserved);
            _length = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, startIndex));
        }

        /// <summary>
        /// Serialize data received to be sent on the network
        /// </summary>
        /// <returns>A byte array ready to be sent</returns>
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
        #endregion
    }
}
