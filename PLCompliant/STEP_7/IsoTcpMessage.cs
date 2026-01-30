using PLCompliant.Enums;
using PLCompliant.Interface;
using System.Net.Sockets;

namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This is the main container class for a whole packet to communicate with STEP7-PLCs
    /// </summary>
    public class IsoTcpMessage : IProtocolMessage, INetworkMessageDeserializable, IEquatable<IsoTcpMessage>
    {

        const int SOCKETTIMEOUT = 3000;

        #region static methods
        /// <summary>
        /// This method sends an IsoTcpMessage by the open stream passed
        /// </summary>
        /// <param name="messageToSend">The read-packet to send</param>
        /// <param name="stream">The network-stream on port 102</param>
        /// <returns>IsoTcpMessage read from the remote PLC</returns>
        public static IsoTcpMessage SendReceive(IsoTcpMessage messageToSend, NetworkStream stream)
        {
            stream.ReadTimeout = SOCKETTIMEOUT;
            byte[] buffer = new byte[messageToSend.Size];
            messageToSend.Serialize(buffer);
            stream.Write(buffer);
            int readbytes = 0;
            IsoTcpMessage response = new();
            byte[] receiveBuffer = new byte[response.TPKT.Size];

            int TotalMsgSize = 0;

            ReceiveState recvState = ReceiveState.ReadingTpktHeader;

            

            int dataleft = 0;
            int index = 0;
            int payloadSize = 0; // Will be set after the header is read

            while (recvState != ReceiveState.Finished)
            {
                if (recvState == ReceiveState.ReadingTpktHeader)
                {
                    dataleft = response.TPKT.Size - readbytes;
                    index = response.TPKT.Size - dataleft;
                    readbytes += stream.Read(receiveBuffer, index, dataleft);
                    if (readbytes >= response.TPKT.Size)
                    {
                        response.DeserializeHeader(receiveBuffer);
                        recvState = ReceiveState.ReadingData;
                        readbytes = 0;
                        /*"length" is the field for the length of the entire message, including itself. Thus we must subtract the header's size from it to get the payload size */
                        payloadSize = response.TPKT.Length - response.TPKT.Size;
                        // set size to be correct for the payload
                        Array.Resize(ref receiveBuffer, payloadSize);

                    }

                }
                else if(recvState == ReceiveState.ReadingData) 
                {

                    dataleft = payloadSize - readbytes;
                    index = payloadSize - dataleft;
                    readbytes += stream.Read(receiveBuffer, index, dataleft);
                    if (readbytes >= payloadSize)
                    {
                        
                        response.DeserializeData(receiveBuffer);
                        readbytes = 0;
                        recvState = ReceiveState.Finished;

                    }
                }
            }
            return response;
        }

        private TPKTHeader _tpkt;
        private COTPMessage _cotp;
        private STEP7Message? _step7;
        #endregion

        /// <summary>
        /// Property for the STEP7-segment
        /// </summary>
        public STEP7Message STEP7
        {
            get { return _step7; }
            set { _step7 = value; }
        }


        /// <summary>
        /// Property to get COTP-segment
        /// </summary>
        public COTPMessage COTP
        {
            get { return _cotp; }
            set { _cotp = value; }
        }


        /// <summary>
        /// Property for the TPKT-segment
        /// </summary>
        public TPKTHeader TPKT
        {
            get { return _tpkt; }
            set { _tpkt = value; }
        }


        /// <summary>
        /// Get the size of the IsoTCPMessage
        /// </summary>
        public int Size
        {
            get
            {
                int size = _tpkt.Size + _cotp.Size;
                if (_step7 != null)
                {
                    size += _step7.Size;
                }
                return size;
            }
        }
        // NOTE
        // Using the contructor which passes in a header will make the length of the message in the TPKT header reflect the messagetype in the STEP7Header
        // However changing it afterwards in a way which changes the STEP7Header size will NOT update the TPKT header size on its own
        // The default constructor defaults to using messagetype 0, which means it will use the whole header. If the messagetype is changed afterwards, the TPKT header size must be manually updated
        public IsoTcpMessage(TPKTHeader tpkt, COTPMessage cotp, STEP7Message? step7)
        {
            _tpkt = tpkt;
            _cotp = cotp;
            _step7 = step7;
            _tpkt.Length = (ushort)Size;
        }
        public IsoTcpMessage()
        {
            _tpkt = new();
            _cotp = new();
            _step7 = null;
            _tpkt.Length = (ushort)Size;
        }
        public void Serialize(Span<byte> serializedObj)
        {
            int index = 0;
            _tpkt.Serialize(serializedObj.Slice(index, _tpkt.Size));
            index += _tpkt.Size;
            _cotp.Serialize(serializedObj.Slice(index, _cotp.Size));
            index += _cotp.Size;
            if( _step7 != null )
            {
                _step7.Serialize(serializedObj.Slice(index, _step7.Size));
                index = _step7.Size;
            }
        }

        public int DeserializeHeader(ReadOnlySpan<byte> inputBuffer)
        {
            return _tpkt.Deserialize(inputBuffer.Slice(0, _tpkt.Size));
        }

        public int DeserializeData(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            index += _cotp.Deserialize(inputBuffer.Slice(index));
            int effectiveStep7Size = _tpkt.Length - (_tpkt.Size + _cotp.Size); // Size of step7 segment is equal to the total length from tpkt header, minus the non-step7 stuff
            if (effectiveStep7Size > 0 )
            {
                // Initialize a step7 instance since there is clearly data that needs a place to be deserialized
                _step7 = new();
                _step7.Deserialize(inputBuffer.Slice(index, effectiveStep7Size));
                index += effectiveStep7Size;
            }
            return index;
        }
        public int Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            index += DeserializeHeader(inputBuffer.Slice(index, _tpkt.Size));
            int restOfData = _tpkt.Length - _tpkt.Size; // Length is the total size of the message including itself. Size is the size of the header only
            index += DeserializeData(inputBuffer.Slice(index, restOfData));
            return index;
        }

        public int AddData<T>(T inputData, byte type) where T : unmanaged, IEndianConvertable
        {
            int dataAdded = 0;
            var flags = (IsoTcpDataType)type;
            if(flags.HasFlag(IsoTcpDataType.COTPData))
            {
                dataAdded = _cotp.AddData<T>(inputData, type);
            }
            else
            {
                if (_step7 is null)
                {
                    _step7 = new();
                    dataAdded += _step7.Size; // Some things like header is auto initialized with the message, so we must include it

                }
                dataAdded += _step7.AddData<T>(inputData, type);                  
            }
            _tpkt.Length += (ushort)dataAdded;
            return dataAdded;
        }

        public int AddData(ushort inputData, byte type)
        {
            int dataAdded = 0;
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.COTPData))
            {
                dataAdded = _cotp.AddData(inputData, type);
                
            }
            else
            {
                
                if (_step7 is null)
                {
                    _step7 = new();
                    dataAdded += _step7.Size; // Some things like header is auto initialized with the message, so we must include it

                }
                dataAdded += _step7.AddData(inputData, type);         
            }
            _tpkt.Length += (ushort)dataAdded;
            return dataAdded;
        }

        public int AddData(byte inputData, byte type)
        {
            int dataAdded = 0;
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.COTPData))
            {
                dataAdded = _cotp.AddData(inputData, type);
            }
            else
            {
                if (_step7 is null)
                {
                    _step7 = new();
                    dataAdded += _step7.Size; // Some things like header is auto initialized with the message, so we must include it

                }
                dataAdded += _step7.AddData(inputData, type);
                
            }
            _tpkt.Length += (ushort)dataAdded;
            return dataAdded;
        }

        public int AddData(ReadOnlySpan<byte> binaryData, byte type)
        {
            int dataAdded = 0;
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.COTPData))
            {
                dataAdded = _cotp.AddData(binaryData, type);
            }
            else
            {
                if (_step7 is null)
                {
                    _step7 = new();
                    dataAdded += _step7.Size; // Some things like header is auto initialized with the message, so we must include it

                }
                dataAdded += _step7.AddData(binaryData, type);               
            }
            _tpkt.Length += (ushort)dataAdded;
            return dataAdded;
        }

        public T GetData<T>(int index, byte type) where T : unmanaged, IEndianConvertable
        {
            var flags = (IsoTcpDataType)type;
            if(flags.HasFlag(IsoTcpDataType.COTPData))
            {
                return _cotp.GetData<T>(index, type);
            }
            else
            {
                return _step7.GetData<T>(index, type);
            }
        }
        /// <summary>
        /// Sets the pdu reference counter to a new value in the STEP7 segment, if such a segment exists
        /// </summary>
        /// <param name="newCount">The new count</param>
        public void SetPduReferenceCounter(ushort newCount)
        {
            _step7?.SetPduReferenceCounter(newCount);
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as IsoTcpMessage);
        }
        public bool Equals(IsoTcpMessage? obj)
        {
            bool bothSTEP7Null = false;
            if (obj is null) return false;
            if (_step7 is null || obj._step7 is null)
            {
                if (_step7 is null && obj._step7 is null)
                {
                    bothSTEP7Null = true;
                }
                else
                {
                    return false;
                }

            }
            return _cotp.Equals(obj._cotp) && (bothSTEP7Null || _step7.Equals(obj._step7));

        }

        public static bool operator ==(IsoTcpMessage left, IsoTcpMessage right)
        {
            return object.Equals(left, right);

        }
        public static bool operator !=(IsoTcpMessage left, IsoTcpMessage right) { return !(left == right); }

















    }
}
