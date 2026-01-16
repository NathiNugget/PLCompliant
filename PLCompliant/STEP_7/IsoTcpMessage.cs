using PLCompliant.Enums;
using PLCompliant.Interface;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PLCompliant.STEP_7
{
    public class IsoTcpMessage : IProtocolMessage
    {

        const int SOCKETTIMEOUT = 3000;

        public static IsoTcpMessage SendReceive(IsoTcpMessage messageToSend, NetworkStream stream)
        {
            stream.ReadTimeout = SOCKETTIMEOUT;
            byte[] buffer = messageToSend.Serialize();
            stream.Write(buffer);
            byte[] databuffer = new byte[1024]; //Default size, actual size is decided by header. 
            int readbytes = 0;
            bool Step7Exists = false;

            TPKTHeader TPKTheader = new TPKTHeader();
            COTPHeader COTPheader = new COTPHeader();
            COTPData COTPData = new COTPData(0);
            STEP7Header STEP7Header = new STEP7Header();
            STEP7ParameterData STEP7ParamData = null;
            STEP7DataPayload STEP7Data = null;
            byte[] headerbuffer = new byte[TPKTheader.Size];

            int TotalMsgSize = 0;

            ReceiveState recvState = ReceiveState.ReadingTpktHeader;

            int dataleft = 0;
            int index = 0;

            int dataBufferLeft = 0;

            while (recvState != ReceiveState.Finished)
            {
                switch (recvState)
                {
                    case ReceiveState.ReadingTpktHeader:
                        dataleft = TPKTheader.Size - readbytes;
                        index = TPKTheader.Size - dataleft;
                        readbytes += stream.Read(headerbuffer, index, dataleft);
                        if (readbytes == TPKTheader.Size)
                        {
                            TPKTheader.Deserialize(headerbuffer, 0);
                            recvState = ReceiveState.ReadingCotpHeader;
                            readbytes = 0;
                            /*"length" is the field for the lenght of the entire packet, including itself. */
                            TotalMsgSize = TPKTheader.Length;
                            // set size to be correct for the next header
                            Array.Resize(ref headerbuffer, COTPheader.Size);

                        }
                        break;
                    case ReceiveState.ReadingCotpHeader:
                        dataleft = COTPheader.Size - readbytes;
                        index = COTPheader.Size - dataleft;
                        readbytes += stream.Read(headerbuffer, index, dataleft);
                        if (readbytes == COTPheader.Size)
                        {
                            COTPheader.Deserialize(headerbuffer, 0);
                            recvState = ReceiveState.ReadingCotpData;
                            readbytes = 0;

                            Array.Resize(ref databuffer, COTPheader.Length);

                        }
                        break;
                    case ReceiveState.ReadingCotpData:
                        dataleft = databuffer.Length - readbytes;
                        index = databuffer.Length - dataleft;
                        readbytes += stream.Read(databuffer, index, dataleft);
                        if (readbytes == databuffer.Length)
                        {
                            COTPData.Deserialize(databuffer, 0);
                            readbytes = 0;
                            // check if there are any data left in the message. If there is, attempt to get STEP7 msg
                            if (TotalMsgSize > COTPData.Size + COTPheader.Size + TPKTheader.Size)
                            {
                                Step7Exists = true;
                                recvState = ReceiveState.ReadingSTEP7HeaderPrelude;
                                Array.Resize(ref headerbuffer, STEP7Header.PRELUDE_LEN);
                            }
                            else
                            {
                                recvState = ReceiveState.Finished;
                            }

                        }
                        break;

                    case ReceiveState.ReadingSTEP7HeaderPrelude:
                        dataleft = STEP7Header.PRELUDE_LEN - readbytes;
                        index = STEP7Header.PRELUDE_LEN - dataleft;
                        readbytes += stream.Read(headerbuffer, index, dataleft);
                        if (readbytes == STEP7Header.PRELUDE_LEN)
                        {
                            STEP7Header.DeserializePrelude(headerbuffer, 0);
                            readbytes = 0;
                            //"length" is the field for the lenght of the entire packet, including itself.
                            recvState = ReceiveState.ReadingSTEP7Header;
                            Array.Resize(ref headerbuffer, STEP7Header.Size - STEP7Header.PRELUDE_LEN);

                        }
                        break;
                    case ReceiveState.ReadingSTEP7Header:
                        dataleft = (STEP7Header.Size - STEP7Header.PRELUDE_LEN) - readbytes;
                        index = (STEP7Header.Size - STEP7Header.PRELUDE_LEN) - dataleft;
                        readbytes += stream.Read(headerbuffer, index, dataleft);
                        if (readbytes == (STEP7Header.Size - STEP7Header.PRELUDE_LEN))
                        {
                            STEP7Header.Deserialize(headerbuffer, 0);
                            readbytes = 0;
                            if (STEP7Header.ParameterLength != 0)
                            {
                                recvState = ReceiveState.ReadingSTEP7Parameters;
                                Array.Resize(ref databuffer, STEP7Header.ParameterLength);
                            }
                            else if (STEP7Header.DataLength != 0)
                            {
                                recvState = ReceiveState.ReadingSTEP7Data;
                                Array.Resize(ref databuffer, STEP7Header.DataLength);
                            }
                            else
                            {
                                recvState = ReceiveState.Finished;
                            }

                        }
                        break;
                    case ReceiveState.ReadingSTEP7Parameters:
                        dataleft = databuffer.Length - readbytes;
                        index = databuffer.Length - dataleft;
                        readbytes += stream.Read(databuffer, index, dataleft);
                        if (readbytes == databuffer.Length)
                        {
                            STEP7ParamData = new(0);
                            STEP7ParamData.Deserialize(databuffer, 0);
                            readbytes = 0;
                            //"length" is the field for the lenght of the entire packet, including itself.

                            if (STEP7Header.DataLength != 0)
                            {
                                recvState = ReceiveState.ReadingSTEP7Data;
                                Array.Resize(ref databuffer, STEP7Header.DataLength);
                            }
                            else
                            {
                                recvState = ReceiveState.Finished;
                            }

                        }
                        break;
                    case ReceiveState.ReadingSTEP7Data:
                        dataleft = databuffer.Length - readbytes;
                        index = databuffer.Length - dataleft;
                        readbytes += stream.Read(databuffer, index, dataleft);
                        if (readbytes == databuffer.Length)
                        {
                            STEP7Data = new(0, 0);
                            STEP7Data.Deserialize(databuffer, 0);
                            readbytes = 0;
                            recvState = ReceiveState.Finished;

                        }
                        break;
                }

            }
            if (Step7Exists)
            {
                return new IsoTcpMessage(TPKTheader, new COTPMessage(COTPheader, COTPData), new STEP7Message(STEP7Header, STEP7ParamData, STEP7Data));
            }
            else
            {
                return new IsoTcpMessage(TPKTheader, new COTPMessage(COTPheader, COTPData), null);
            }
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

        public int DataSize => throw new NotImplementedException();

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
            
            _cotp.DeserializeHeader(inputBuffer.Slice(0, _cotp.Header.Size));
            index += _cotp.Header.Size;
            _cotp.DeserializeData(inputBuffer.Slice(index, _cotp.Header.Length));

            if(_step7 != null )
            {
                int effectiveStep7Size = _tpkt.Length - (_tpkt.Size + _cotp.Size); // Size of step7 segment is equal to the total length from tpkt header, minus the non-step7 stuff
                _step7.DeserializeHeader(inputBuffer.Slice(index, _step7.STEP7Header.Size));
                // If the messagetype is ACK-Data, then take we increment by the whole size since it includes every field
                // Otherwise, we take NON_ERROR_LENGTH, since it will exclude the error fields
                if(_step7.STEP7Header.MessageType == 0x3)
                {
                    index += _step7.STEP7Header.Size;
                }
                else
                {
                    index += STEP7Header.NON_ERROR_LENGTH;
                }
                _step7.DeserializeData(inputBuffer.Slice(index, _step7.STEP7Header.ParameterLength + _step7.STEP7Header.DataLength));
            }

            
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
            throw new NotImplementedException();
        }
    }
}
