using PLCompliant.Interface;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace PLCompliant.Modbus
{
    /// <summary>
    /// This class represents a full Modbus packet wrapped in TCP, so it has to contains all the header fields as well as the data that follows
    /// </summary>
    public class ModBusMessage : IProtocolMessage, INetworkMessageDeserializable
    {
        const int SOCKETTIMEOUT = 3000;
        /// <summary>
        /// Sends a ModBus message to the specified socket, and returns the response
        /// </summary>
        /// <param name="messageToSend">The modbus message to send</param>
        /// <param name="stream">The stream to send it to</param>
        /// <returns>The response as a ModBusMessage</returns>
        public static ModBusMessage SendReceive(ModBusMessage messageToSend, NetworkStream stream)
        {
            stream.ReadTimeout = SOCKETTIMEOUT;
            byte[] buffer = new byte[messageToSend.Size];
            messageToSend.Serialize(buffer);
            stream.Write(buffer, 0, buffer.Length);
            byte[] databuffer = new byte[1024]; //Default size, actual size is decided by header. 
            int readbytes = 0;
            byte[] headerbuffer = new byte[messageToSend.Header.Size];
            bool readingHeader = true;
            ModBusMessage response = new(new ModBusHeader(), new ModBusData());

            while (true)
            {
                if (readingHeader)
                {
                    int dataleft = messageToSend.Header.Size - readbytes;
                    int index = messageToSend.Header.Size - dataleft;
                    readbytes += stream.Read(headerbuffer, index, dataleft);
                    if (readbytes == response.Header.Size)
                    {
                        response.DeserializeHeader(headerbuffer);
                        readingHeader = false;
                        readbytes = 0;
                        Array.Resize(ref databuffer, response.Header.length - 1); //Minus 1 because unit id is included. Standard Modbus stuff :/
                    }

                }
                else
                {

                    int dataleft = (response.Header.length - 1) - readbytes;
                    int index = (response.Header.length - 1) - dataleft;
                    readbytes += stream.Read(databuffer, index, dataleft);
                    if (readbytes == response.Header.length - 1)
                    {
                        response.DeserializeData(databuffer);
                        break;
                    }
                }
            }
            return response;

        }

        public static ushort MODBUS_TCP_PORT = 502;
        #region instance fields


        private ModBusHeader _header;
        private ModBusData _data;

        #endregion

        #region properties
        /// <summary>
        /// Property to get the Header member
        /// </summary>
        public ModBusHeader Header { get { return _header; } }
        /// <summary>
        /// Property to get the Data member
        /// </summary>
        public ModBusData Data { get { return _data; } }

        /// <summary>
        /// Normal constructor for the class with header and data passed
        /// </summary>
        /// <param name="header">The header for the packet</param>
        /// <param name="data">The data for the packet</param>
        /// 
        #endregion

        #region constructors
        public ModBusMessage(ModBusHeader header, ModBusData data)
        {
            _header = header;
            _data = data;
            _header.length += (ushort)_data.Size; //increment it as we are initializing ModBusData with potential data in it
        }

        /// <summary>
        /// Empty constructor for easy initialization; 
        /// </summary>
        public ModBusMessage()
        {
            _header = new();
            _data = new();
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public int Size {  get { return _data.Size + _header.Size; } }
        #endregion
        #region Methods
        /// <inheritdoc/>
        public override bool Equals(object? other)
        {
            if (other == null) return false;
            if (other is not ModBusMessage) return false;
            ModBusMessage other_msg = (ModBusMessage)other;
            return (Data.Equals(other_msg.Data) && Header.Equals(other_msg.Header));

        }
        /// <inheritdoc/>
        public int DeserializeHeader(ReadOnlySpan<byte> inputBuffer)
        {
            return _header.Deserialize(inputBuffer.Slice(0, _header.Size));
        }
        /// <inheritdoc/>
        public int DeserializeData(ReadOnlySpan<byte> inputBuffer)
        {
            _data.ResizeStorage(Header.length - 1); // - 1 because it includes unitId, which is in the header
            return _data.Deserialize(inputBuffer.Slice(0, _data.Size));
        }
        public int Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            index += DeserializeHeader(inputBuffer.Slice(index, _header.Size));
            index += DeserializeData(inputBuffer.Slice(index, _data.Size));
            return index;
        }
        /// <inheritdoc/>
        public int AddData<T>(T inputData, byte type = 0) where T : unmanaged, IEndianConvertable
        {
            int dataAdded = _data.AddData<T>(inputData, type);
            _header.length += (ushort)dataAdded;
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(ushort inputData, byte type = 0)
        {
            int dataAdded = _data.AddData(inputData, type);
            _header.length += (ushort)dataAdded;
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(byte inputData, byte type = 0)
        {
            int dataAdded = _data.AddData(inputData, type);
            _header.length += (ushort)dataAdded;
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(ReadOnlySpan<byte> binaryData, byte type = 0)
        {
            if (binaryData.Length > byte.MaxValue)
            {
                throw new ArgumentException("Input length was greater than allowed in a byte");
            }
            int dataAdded = _data.AddData(binaryData, type);
            _header.length += (ushort)dataAdded;
            return dataAdded;
        }
        /// <inheritdoc/>
        public T GetData<T>(int index, byte type = 0) where T : unmanaged, IEndianConvertable
        {
            return _data.GetData<T>(index, type);
        }

        public void Serialize(Span<byte> serializedObj)
        {
            int index = 0;
            _header.Serialize(serializedObj.Slice(index, _header.Size));
            index += _header.Size;
            _data.Serialize(serializedObj.Slice(index, _data.Size));
        }

       
        #endregion



    }
}
