using PLCompliant.Enums;
using PLCompliant.Interface;
using PLCompliant.Logging;
using PLCompliant.Response;
using PLCompliant.Utilities;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace PLCompliant.Modbus
{
    /// <summary>
    /// This class represents a full Modbus packet wrapped in TCP, so it has to contains all the header fields as well as the data that follows
    /// </summary>
    public class ModBusMessage : IProtocolMessage
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
            byte[] buffer = messageToSend.Serialize();
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
                    if (readbytes == 7)
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
        /// Property to get the payload size in bytes
        /// </summary>
        public ushort PayloadSize { get => (ushort)_data.PayloadSize; }
        /// <summary>
        /// Property to get the total size of the Data and Header in bytes
        /// </summary>
        public ushort TotalSize { get => (ushort)(Data.Size + Header.Size); }

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

        #region methods
        /// <summary>
        /// Addition of a ushort/UInt16, which is then converted to bytes and endianness is made into network order if host-machine is little endian
        /// </summary>
        /// <param name="inputData">The ushort to add</param>
        public void AddData(UInt16 inputData)
        {
            var oldSize = _data.PayloadSize;
            var newSize = _data._payload.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref _data._payload, newSize);
            byte[] bytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(inputData));
            Array.Copy(bytes, 0, _data._payload, oldSize, bytes.Length);
            _header.length += sizeof(UInt16);
        }


        /// <inheritdoc/>
        public void AddData(byte inputData)
        {
            var newSize = _data._payload.Length + Marshal.SizeOf<byte>();
            Array.Resize(ref _data._payload, newSize);
            _data._payload[newSize - 1] = inputData;
            _header.length += sizeof(byte);
        }

        /// <inheritdoc/>
        public void AddData(byte[] stringData)
        {
            if (stringData.Length > byte.MaxValue)
            {
                throw new ArgumentException("Input length was greater than allowed in a byte");
            }
            byte stringSize = (byte)stringData.Length;
            if (stringSize == 0) { return; }
            var oldSize = Data._payload.Length;
            var newSize = _data._payload.Length + stringSize;
            Array.Resize(ref _data._payload, newSize);
            Array.Copy(stringData, 0, _data._payload, oldSize, stringSize);
            _header.length += (ushort)stringSize;
        }

        /// <inheritdoc/>

        public byte[] Serialize()
        {
            byte[] headerData = _header.Serialize();
            byte[] payloadData = _data.Serialize();
            byte[] result = new byte[headerData.Length + payloadData.Length];
            Array.Copy(headerData, result, headerData.Length);
            Array.Copy(payloadData, 0, result, headerData.Length, payloadData.Length);
            return result;

        }
        /// <inheritdoc/>

        public void DeserializeHeader(byte[] inputBuffer)
        {
            _header.Deserialize(inputBuffer);
        }
        /// <inheritdoc/>

        public void DeserializeData(byte[] inputBuffer)
        {
            _data.Deserialize(inputBuffer);
        }

        /// <inheritdoc/>
        public int DataSize { get { return Data.Size; } }

        /// <inheritdoc/>
        public override bool Equals(object? other)
        {
            if (other == null) return false;
            if (other is not ModBusMessage) return false;
            ModBusMessage other_msg = (ModBusMessage)other;
            return (Data.Equals(other_msg.Data) && Header.Equals(other_msg.Header));

        }
        #endregion



    }
}
