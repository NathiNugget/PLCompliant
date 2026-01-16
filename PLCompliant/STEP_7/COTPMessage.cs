using PLCompliant.Interface;

namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This class contains header and data for the COTP-segment
    /// </summary>
    public class COTPMessage : IProtocolMessage
    {
        #region fields
        private COTPHeader _header;
        private COTPData _data;
        #endregion

        #region properties
        /// <summary>
        /// Property to get the data segment of this class
        /// </summary>
        public COTPData Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// Property to get the header segment of this class
        /// </summary>
        public COTPHeader Header
        {
            get { return _header; }
            set { _header = value; }
        }

        /// <summary>
        /// Get the size of header and data in bytes
        /// </summary>
        public int Size
        {
            get
            {
                return _header.Size + _data.Size;
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public COTPMessage(COTPHeader header, COTPData data)
        {
            _header = header;
            _data = data;
        }
        #endregion

        /// <summary>
        /// Unused property
        /// </summary>
        [Obsolete]
        public int DataSize => throw new NotImplementedException();

        /// <summary>
        /// Add data to the data-portion of the class
        /// </summary>
        /// <param name="inputData">Data to be added</param>
        public void AddData(ushort inputData)
        {
            _data.AddData(inputData);
            _header.Length = (byte)Data.Size;
        }
        /// <summary>
        /// Add data to the data-portion of the class
        /// </summary>
        /// <param name="inputData">Data to be added</param>
        public void AddData(byte inputData)
        {
            _data.AddData(inputData);
            _header.Length = (byte)Data.Size;
        }
        /// <summary>
        /// Add data to the data-portion of the class
        /// </summary>
        /// <param name="stringData">Data to be added from a UTF-8 string</param>
        public void AddData(byte[] stringData)
        {
            _data.AddData(stringData);
            _header.Length = (byte)Data.Size;
        }

        /// <summary>
        /// Deserialize data after receiving from the network
        /// </summary>
        /// <param name="inputBuffer">The buffer to deserialize from</param>
        /// <param name="startIndex">The start-index to read from</param>
        public void DeserializeData(byte[] inputBuffer, int startIndex)
        {
            _data.Deserialize(inputBuffer, startIndex);
        }

        /// <summary>
        /// Deserialize the header received from the network
        /// </summary>
        /// <param name="inputBuffer">The buffer to read from</param>
        /// <param name="startIndex">The index to read from</param>
        public void DeserializeHeader(byte[] inputBuffer, int startIndex)
        {
            _header.Deserialize(inputBuffer, startIndex);

        }

        /// <summary>
        /// Serialize data and header for network transmission
        /// </summary>
        /// <returns>Byte array to send</returns>
        public byte[] Serialize()
        {
            byte[] outputData = new byte[Size];
            int startIndex = 0;
            Array.Copy(_header.Serialize(), 0, outputData, startIndex, _header.Size);
            startIndex += _header.Size;
            Array.Copy(_data.Serialize(), 0, outputData, startIndex, _data.Size);


            return outputData;


        }
    }
}
