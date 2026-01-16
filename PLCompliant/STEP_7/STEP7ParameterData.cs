using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;


namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This class represents data for the parameters of a STEP7-message
    /// </summary>
    public class STEP7ParameterData : IProtocolData
    {
        #region fields
        private byte _functionCode;
        private byte[] _data;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor for the class
        /// </summary>
        /// <param name="functionCode">Function code for the message</param>
        public STEP7ParameterData(byte functionCode)
        {
            _functionCode = functionCode;
            _data = [];
        }
        #endregion

        #region properties
        /// <summary>
        /// Data property for the params
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// Function code for the params
        /// </summary>
        public byte FunctionCode
        {
            get { return _functionCode; }
            set { _functionCode = value; }
        }

        /// <summary>
        /// Size of function code and data in bytes
        /// </summary>
        public int Size
        {
            get
            {
                return Marshal.SizeOf(_functionCode) + _data.Length;
            }
        }

        /// <summary>
        /// Deserialize data from the network
        /// </summary>
        /// <param name="inputBuffer">Buffer to read from</param>
        /// <param name="startIndex">Index to start reading from</param>
        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
            FunctionCode = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(FunctionCode);
            Array.Resize(ref _data, inputBuffer.Length - startIndex);
            Array.Copy(inputBuffer, startIndex, _data, 0, _data.Length);
        }

        /// <summary>
        /// Serialize data for network transmission
        /// </summary>
        /// <returns>Bytes in an array ready to send</returns>
        public byte[] Serialize()
        {
            byte[] outData = new byte[Size];
            outData[0] = _functionCode;
            Array.Copy(_data, 0, outData, 1, _data.Length);
            return outData;
        }

        /// <summary>
        /// Add data to the params
        /// </summary>
        /// <param name="inputData">Data to be added, possibly endian-converted</param>
        public void AddData(ushort inputData)
        {
            var oldSize = _data.Length;
            var newSize = _data.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref _data, newSize);
            byte[] bytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(inputData));
            Array.Copy(bytes, 0, _data, oldSize, bytes.Length);
        }

        /// <summary>
        /// Add data to the params
        /// </summary>
        /// <param name="inputData">Data to be added</param>
        public void AddData(byte inputData)
        {
            var newSize = _data.Length + Marshal.SizeOf<byte>();
            Array.Resize(ref _data, newSize);
            _data[newSize - 1] = inputData;
        }

        /// <summary>
        /// Add data to the params
        /// </summary>
        /// <param name="stringData">Data to be added from an UTF8-string</param>
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
        #endregion
    }
}
