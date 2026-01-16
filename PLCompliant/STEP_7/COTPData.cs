using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This class represents the data portion of a COTP-segment
    /// </summary>
    public class COTPData : IProtocolData
    {
        #region fields
        private byte _pduType;
        private byte[] _data;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pduType">Value of the PDU-type</param>
        public COTPData(byte pduType)
        {
            _pduType = pduType;
            _data = [];
        }
        #endregion

        #region properties
        /// <summary>
        /// Property for the PDU-type
        /// </summary>
        public byte PduType
        {
            get { return _pduType; }
            set { _pduType = value; }
        }
        /// <summary>
        /// Property for the Data-segment of the class
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
            private set { _data = value; }
        }

        /// <summary>
        /// Size of the instance to make network serialization and deserialization easier
        /// </summary>
        public int Size
        {
            get
            {
                return Marshal.SizeOf(_pduType) + _data.Length;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Serialize some inputdata to bytes in the _data-field. It is endian-converted
        /// </summary>
        /// <param name="inputData">Data to be serialized</param>
        public void AddData(ushort inputData)
        {
            var oldSize = _data.Length;
            var newSize = _data.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref _data, newSize);
            byte[] bytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(inputData));
            Array.Copy(bytes, 0, _data, oldSize, bytes.Length);
        }

        /// <summary>
        /// Serialize some inputdata to bytes in the _data-field
        /// </summary>
        /// <param name="inputData">Data to be serialized</param>
        public void AddData(byte inputData)
        {
            var newSize = _data.Length + Marshal.SizeOf<byte>();
            Array.Resize(ref _data, newSize);
            _data[newSize - 1] = inputData;
        }

        /// <summary>
        /// Serialize some inputdata to bytes in the _data-field
        /// </summary>
        /// <param name="stringData">Data to be serialized from a string converted to UTF-8 bytes</param>
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
        }

        /// <summary>
        /// Deserialize data from the network
        /// </summary>
        /// <param name="inputBuffer">The buffer from which data should be read</param>
        /// <param name="startIndex">Index to start reading from</param>
        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
            _pduType = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_pduType);
            Array.Resize(ref _data, inputBuffer.Length - startIndex);
            Array.Copy(inputBuffer, startIndex, _data, 0, inputBuffer.Length - startIndex);
        }

        /// <summary>
        /// Serialize this class to a byte-array
        /// </summary>
        /// <returns></returns>
        public byte[] Serialize()
        {
            int startIndex = 0;
            byte[] outData = new byte[Size];
            outData[startIndex] = _pduType;
            startIndex += Marshal.SizeOf(_pduType);
            Array.Copy(_data, 0, outData, startIndex, _data.Length);
            return outData;
        }
    }
    #endregion






}
