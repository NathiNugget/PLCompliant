using PLCompliant.Interface;

namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This class contains the header and maybe parameter data, maybe data
    /// </summary>
    public class STEP7Message : IProtocolMessage
    {
        #region fields
        /// <summary>
        /// Port number for STEP7-communication via ISO-over-TCP
        /// </summary>
        public const ushort STEP7_TCP_PORT = 102;
        private STEP7Header _step7Header;
        private STEP7ParameterData? _step7ParamData;
        private STEP7Data? _step7Data;
        #endregion

        #region constructor
        public STEP7Message(STEP7Header step7Header, STEP7ParameterData? step7ParamData, STEP7Data? step7Data)
        {
            _step7Header = step7Header;
            _step7Data = step7Data;
            _step7ParamData = step7ParamData;

        }
        #endregion

        #region properties
        /// <summary>
        /// Get and set STEP7-data
        /// </summary>
        public STEP7Data STEP7Data
        {
            get { return _step7Data; }
            set { _step7Data = value; }
        }

        /// <summary>
        /// Get and set STEP7-ParamData
        /// </summary>
        public STEP7ParameterData STEP7ParamData
        {
            get { return _step7ParamData; }
            set { _step7ParamData = value; }
        }

        /// <summary>
        /// Get and set STEP7-header
        /// </summary>
        public STEP7Header STEP7Header
        {
            get { return _step7Header; }
            set { _step7Header = value; }
        }

        /// <summary>
        /// Size of the whole header, data and params (if not null)
        /// </summary>
        public int Size
        {
            get
            {
                int size = _step7Header.Size;
                if (_step7ParamData != null)
                {
                    size += _step7ParamData.Size;
                }
                if (_step7Data != null)
                {
                    size += _step7Data.Size;
                }
                return size;

            }
        }

        /// <summary>
        /// Unused method
        /// </summary>
        [Obsolete]
        public int DataSize => throw new NotImplementedException();
        #endregion

        #region methods
        /// <summary>
        /// Add data to the data-segment
        /// </summary>
        /// <param name="inputData">Data to add, will possibly be endian-converted</param>
        /// <exception cref="ArgumentNullException">Thrown if the Data-segment is null</exception>
        public void AddData(ushort inputData)
        {
            if (_step7Data == null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }
            _step7Data.AddData(inputData);
            _step7Header.DataLength = (UInt16)_step7Data.Size;
        }

        /// <summary>
        /// Add data to the data-segment
        /// </summary>
        /// <param name="inputData">Data to add</param>
        /// <exception cref="ArgumentNullException">Thrown if the Data-segment is null</exception>
        public void AddData(byte inputData)
        {
            if (_step7Data == null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }
            _step7Data.AddData(inputData);
            _step7Header.DataLength = (UInt16)_step7Data.Size;
        }

        /// <summary>
        /// Add data to the data-segment
        /// </summary>
        /// <param name="stringData">Data to add from a UTF8-string</param>
        /// <exception cref="ArgumentNullException">Thrown if the Data-segment is null</exception>
        public void AddData(byte[] stringData)
        {
            if (_step7Data == null)
            {
                throw new ArgumentNullException(nameof(stringData));
            }
            _step7Data.AddData(stringData);
            _step7Header.DataLength = (UInt16)_step7Data.Size;
        }


        /// <summary>
        /// Add data to ParamData
        /// </summary>
        /// <param name="inputData">Data to be added, possibly endian-converted</param>
        /// <exception cref="ArgumentNullException">Thrown if Param-segment is null</exception>
        public void AddParameterData(ushort inputData)
        {
            if (_step7ParamData == null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }
            _step7ParamData.AddData(inputData);
            _step7Header.ParameterLength = (UInt16)_step7ParamData.Size;
        }

        /// <summary>
        /// Add data to ParamData
        /// </summary>
        /// <param name="inputData">Data to be added</param>
        /// <exception cref="ArgumentNullException">Thrown if Param-segment is null</exception>
        public void AddParameterData(byte inputData)
        {
            if (_step7ParamData == null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }
            _step7ParamData.AddData(inputData);
            _step7Header.ParameterLength = (UInt16)_step7ParamData.Size;
        }

        /// <summary>
        /// Add data to ParamData
        /// </summary>
        /// <param name="stringData">Data to be added from a UTF8-string</param>
        /// <exception cref="ArgumentNullException">Thrown if Param-segment is null</exception>
        public void AddParameterData(byte[] stringData)
        {
            if (_step7ParamData == null)
            {
                throw new ArgumentNullException(nameof(stringData));
            }
            _step7ParamData.AddData(stringData);
            _step7Header.ParameterLength = (UInt16)_step7ParamData.Size;
        }

        /// <summary>
        /// Deserialize data from the network
        /// </summary>
        /// <param name="inputBuffer">Buffer to read from</param>
        /// <param name="startIndex">Index to start reading from</param>
        public void DeserializeData(byte[] inputBuffer, int startIndex)
        {
            if (_step7ParamData != null)
            {
                _step7ParamData.Deserialize(inputBuffer, startIndex);
                startIndex += _step7ParamData.Size;
            }
            if (_step7Data != null)
            {
                _step7Data.Deserialize(inputBuffer, startIndex);
                startIndex += _step7Data.Size;
            }
        }

        /// <summary>
        /// Deserialize header from network
        /// </summary>
        /// <param name="inputBuffer">Buffer to read from</param>
        /// <param name="startIndex">Index to start reading from</param>
        public void DeserializeHeader(byte[] inputBuffer, int startIndex)
        {
            _step7Header.Deserialize(inputBuffer, startIndex);

        }

        /// <summary>
        /// Serialize data for network transmission
        /// </summary>
        /// <returns>Byte array containing the data in bytes</returns>
        public byte[] Serialize()
        {
            byte[] outputData = new byte[Size];
            int startIndex = 0;
            Array.Copy(_step7Header.Serialize(), 0, outputData, startIndex, _step7Header.Size);
            startIndex += _step7Header.Size;

            if (_step7ParamData != null)
            {
                Array.Copy(_step7ParamData.Serialize(), 0, outputData, startIndex, _step7ParamData.Size);
                startIndex += _step7ParamData.Size;
            }
            if (_step7Data != null)
            {
                Array.Copy(_step7Data.Serialize(), 0, outputData, startIndex, _step7Data.Size);
                startIndex += _step7Data.Size;
            }
            return outputData;
        }
        #endregion
    }
}
