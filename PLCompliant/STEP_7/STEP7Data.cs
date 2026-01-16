using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This class contains the data-segment of a STEP7Message
    /// </summary>
    public class STEP7Data : IProtocolData
    {
        #region fields
        private byte _returnCode;
        private byte _transportType;
        private UInt16 _length;
        private byte[] _data;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="transportType"></param>
        public STEP7Data(byte returnCode, byte transportType)
        {
            _returnCode = returnCode;
            _transportType = transportType;
            _length = 0;
            _data = [];
        }
        #endregion

        #region properties
        /// <summary>
        /// Data portion of the data-segment as a byte array
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// Length of the data
        /// </summary>
        public UInt16 Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// A byte for the TransportType
        /// </summary>
        public byte TransportType
        {
            get { return _transportType; }
            set { _transportType = value; }
        }

        /// <summary>
        /// Byte for the ReturnCode
        /// </summary>
        public byte ReturnCode
        {
            get { return _returnCode; }
            set { _returnCode = value; }
        }

        /// <summary>
        /// Size of the fields of the data-segment plus the data itself in bytes
        /// </summary>
        public int Size
        {
            get
            {
                return Marshal.SizeOf(_returnCode) + Marshal.SizeOf(_transportType) + Marshal.SizeOf(_length) + _data.Length;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Deserialize data received from the network
        /// </summary>
        /// <param name="inputBuffer">The buffer to read from</param>
        /// <param name="startIndex">The index to read from</param>
        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
            _returnCode = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_returnCode);
            _transportType = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_transportType);
            _length = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, startIndex));
            startIndex += Marshal.SizeOf(_length);
            Array.Resize(ref _data, _length);
            Array.Copy(inputBuffer, startIndex, _data, 0, _length);
        }

        /// <summary>
        /// Serialize the data for network transmission
        /// </summary>
        /// <returns>The data in a byte array</returns>
        public byte[] Serialize()
        {
            int startIndex = 0;

            byte[] outData = new byte[Size];
            outData[startIndex] = _returnCode;
            startIndex += Marshal.SizeOf(_returnCode);
            outData[startIndex] = _transportType;
            startIndex += Marshal.SizeOf(_transportType);
            var lengthAsBytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(_length));
            outData[startIndex] = lengthAsBytes[0];
            startIndex += 1;
            outData[startIndex] = lengthAsBytes[1];
            startIndex += 1;
            Array.Copy(_data, 0, outData, startIndex, _data.Length);
            return outData;

        }

        /// <summary>
        /// Add data and possibly endian-convert to network-order
        /// </summary>
        /// <param name="inputData">The number to add</param>
        public void AddData(ushort inputData)
        {
            var oldSize = _data.Length;
            var newSize = _data.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref _data, newSize);
            byte[] bytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(inputData));
            Array.Copy(bytes, 0, _data, oldSize, bytes.Length);
            _length += (ushort)Marshal.SizeOf(inputData);
        }

        /// <summary>
        /// Add a byte to data
        /// </summary>
        /// <param name="inputData">The byte to add</param>
        public void AddData(byte inputData)
        {
            var newSize = _data.Length + Marshal.SizeOf<byte>();
            Array.Resize(ref _data, newSize);
            _data[newSize - 1] = inputData;
            _length += (ushort)Marshal.SizeOf(inputData);
        }

        /// <summary>
        /// Add data from a string
        /// </summary>
        /// <param name="stringData">Bytes from a UTF8-string</param>
        public void AddData(byte[] stringData)
        {
            if (stringData.Length > byte.MaxValue)
            {
                throw new ArgumentException("Input length was greater than allowed in a byte");
            }
            byte stringSize = (byte)stringData.Length;
            if (stringSize == 0) { return; }
            var oldSize = Data.Length;
            var newSize = _data.Length + stringSize;
            Array.Resize(ref _data, newSize);
            Array.Copy(stringData, 0, _data, oldSize, stringSize);
            _length += (ushort)stringData.Length;
        }
        #endregion

    }
}
