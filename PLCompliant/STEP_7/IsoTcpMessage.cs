using PLCompliant.Enums;
using PLCompliant.Interface;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PLCompliant.STEP_7
{
    public class IsoTcpMessage : IProtocolMessage, INetworkMessageDeserializable
    {

        const int SOCKETTIMEOUT = 3000;

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

        public STEP7Message STEP7
        {
            get { return _step7; }
            set { _step7 = value; }
        }


        public COTPMessage COTP
        {
            get { return _cotp; }
            set { _cotp = value; }
        }



        public TPKTHeader TPKT
        {
            get { return _tpkt; }
            set { _tpkt = value; }
        }


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

        public void DeserializeHeader(ReadOnlySpan<byte> inputBuffer)
        {
            _tpkt.Deserialize(inputBuffer.Slice(0, _tpkt.Size));
        }

        public void DeserializeData(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            _cotp.Deserialize(inputBuffer.Slice(index, _cotp.Size));
            index += _cotp.Size;
            int effectiveStep7Size = _tpkt.Length - (_tpkt.Size + _cotp.Size); // Size of step7 segment is equal to the total length from tpkt header, minus the non-step7 stuff
            if (effectiveStep7Size > 0 )
            {
                // Initialize a step7 instance since there is clearly data that needs a place to be deserialized
                _step7 = new();
                _step7.Deserialize(inputBuffer.Slice(index, effectiveStep7Size));
            }
        }
        public void Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            DeserializeHeader(inputBuffer.Slice(index, _tpkt.Size));
            index += _tpkt.Size;
            int restOfData = this.Size - _tpkt.Size;
            DeserializeData(inputBuffer.Slice(index, restOfData));
        }

        public void AddData<T>(T inputData, byte type) where T : unmanaged, IEndianConvertable
        {
            var flags = (IsoTcpDataType)type;
            if(flags.HasFlag(IsoTcpDataType.COTPData))
            {
                _cotp.AddData<T>(inputData, type);
                _tpkt.Length += (ushort)Marshal.SizeOf<T>();
            }
            else
            {
                _step7.AddData<T>(inputData, type);
                _tpkt.Length += (ushort)Marshal.SizeOf<T>();
            }
        }

        public void AddData(ushort inputData, byte type)
        {
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.COTPData))
            {
                _cotp.AddData(inputData, type);
                _tpkt.Length += (ushort)Marshal.SizeOf(inputData);
            }
            else
            {
                _step7.AddData(inputData, type);
                _tpkt.Length += (ushort)Marshal.SizeOf(inputData);
            }
        }

        public void AddData(byte inputData, byte type)
        {
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.COTPData))
            {
                _cotp.AddData(inputData, type);
                _tpkt.Length += (ushort)Marshal.SizeOf(inputData);
            }
            else
            {
                _step7.AddData(inputData, type);
                _tpkt.Length += (ushort)Marshal.SizeOf(inputData);
            }
        }

        public void AddData(ReadOnlySpan<byte> binaryData, byte type)
        {
            if (binaryData.Length > byte.MaxValue)
            {
                throw new ArgumentException("Input length was greater than allowed in a byte");
            }
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.COTPData))
            {
                _cotp.AddData(binaryData, type);
                _tpkt.Length += (ushort)binaryData.Length;
            }
            else
            {
                _step7.AddData(binaryData, type);
                _tpkt.Length += (ushort)binaryData.Length;
            }
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

       
    }
}
