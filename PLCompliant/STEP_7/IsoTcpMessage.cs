using PLCompliant.Enums;
using PLCompliant.Interface;
using System.Net.Sockets;
using System.Runtime.InteropServices;

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
            STEP7Data STEP7Data = null;
            byte[] headerbuffer = new byte[TPKTheader.Size];

            int TotalMsgSize = 0;

            RecieveState recvState = RecieveState.ReadingTpktHeader;

            int dataleft = 0;
            int index = 0;

            int dataBufferLeft = 0;

            while (recvState != RecieveState.Finished)
            {
                switch (recvState)
                {
                    case RecieveState.ReadingTpktHeader:
                        dataleft = TPKTheader.Size - readbytes;
                        index = TPKTheader.Size - dataleft;
                        readbytes += stream.Read(headerbuffer, index, dataleft);
                        if (readbytes == TPKTheader.Size)
                        {
                            TPKTheader.Deserialize(headerbuffer, 0);
                            recvState = RecieveState.ReadingCotpHeader;
                            readbytes = 0;
                            /*"length" is the field for the lenght of the entire packet, including itself.
                            Therefore, to find the length of the rest we must subtract its own size
                              */
                            TotalMsgSize = TPKTheader.Length;
                            // set size to be correct for the next header
                            Array.Resize(ref headerbuffer, COTPheader.Size);

                        }
                        break;
                    case RecieveState.ReadingCotpHeader:
                        dataleft = COTPheader.Size - readbytes;
                        index = COTPheader.Size - dataleft;
                        readbytes += stream.Read(headerbuffer, index, dataleft);
                        if (readbytes == COTPheader.Size)
                        {
                            COTPheader.Deserialize(headerbuffer, 0);
                            recvState = RecieveState.ReadingCotpData;
                            readbytes = 0;

                            Array.Resize(ref databuffer, COTPheader.Length);

                        }
                        break;
                    case RecieveState.ReadingCotpData:
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
                                recvState = RecieveState.ReadingSTEP7HeaderPrelude;
                                Array.Resize(ref headerbuffer, STEP7Header.PRELUDE_LEN);
                            }
                            else
                            {
                                recvState = RecieveState.Finished;
                            }

                        }
                        break;

                    case RecieveState.ReadingSTEP7HeaderPrelude:
                        dataleft = STEP7Header.PRELUDE_LEN - readbytes;
                        index = STEP7Header.PRELUDE_LEN - dataleft;
                        readbytes += stream.Read(headerbuffer, index, dataleft);
                        if (readbytes == STEP7Header.PRELUDE_LEN)
                        {
                            STEP7Header.DeserializePrelude(headerbuffer, 0);
                            readbytes = 0;
                            /*"length" is the field for the lenght of the entire packet, including itself.
                            Therefore, to find the length of the rest we must subtract its own size
                              */
                            recvState = RecieveState.ReadingSTEP7Header;
                            Array.Resize(ref headerbuffer, STEP7Header.Size - STEP7Header.PRELUDE_LEN);

                        }
                        break;
                    case RecieveState.ReadingSTEP7Header:
                        dataleft = (STEP7Header.Size - STEP7Header.PRELUDE_LEN) - readbytes;
                        index = (STEP7Header.Size - STEP7Header.PRELUDE_LEN) - dataleft;
                        readbytes += stream.Read(headerbuffer, index, dataleft);
                        if (readbytes == (STEP7Header.Size - STEP7Header.PRELUDE_LEN))
                        {
                            STEP7Header.Deserialize(headerbuffer, 0);
                            readbytes = 0;
                            if (STEP7Header.ParameterLength != 0)
                            {
                                recvState = RecieveState.ReadingSTEP7Parameters;
                                Array.Resize(ref databuffer, STEP7Header.ParameterLength);
                            }
                            else if (STEP7Header.DataLength != 0)
                            {
                                recvState = RecieveState.ReadingSTEP7Data;
                                Array.Resize(ref databuffer, STEP7Header.DataLength);
                            }
                            else
                            {
                                recvState = RecieveState.Finished;
                            }

                        }
                        break;
                    case RecieveState.ReadingSTEP7Parameters:
                        dataleft = databuffer.Length - readbytes;
                        index = databuffer.Length - dataleft;
                        readbytes += stream.Read(databuffer, index, dataleft);
                        if (readbytes == databuffer.Length)
                        {
                            STEP7ParamData = new(0);
                            STEP7ParamData.Deserialize(databuffer, 0);
                            readbytes = 0;
                            /*"length" is the field for the lenght of the entire packet, including itself.
                            Therefore, to find the length of the rest we must subtract its own size
                              */
                            if (STEP7Header.DataLength != 0)
                            {
                                recvState = RecieveState.ReadingSTEP7Data;
                                Array.Resize(ref databuffer, STEP7Header.DataLength);
                            }
                            else
                            {
                                recvState = RecieveState.Finished;
                            }

                        }
                        break;
                    case RecieveState.ReadingSTEP7Data:
                        dataleft = databuffer.Length - readbytes;
                        index = databuffer.Length - dataleft;
                        readbytes += stream.Read(databuffer, index, dataleft);
                        if (readbytes == databuffer.Length)
                        {
                            STEP7Data = new(0, 0);
                            STEP7Data.Deserialize(databuffer, 0);
                            readbytes = 0;
                            recvState = RecieveState.Finished;

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
        public void AddParameterData(ushort inputData)
        {
            _tpkt.Length += (ushort)Marshal.SizeOf(inputData);
            _step7.AddParameterData(inputData);
        }

        public void AddParameterData(byte inputData)
        {
            _tpkt.Length += (ushort)Marshal.SizeOf(inputData);
            _step7.AddParameterData(inputData);
        }

        public void AddParameterData(byte[] stringData)
        {
            _tpkt.Length += (ushort)stringData.Length;
            _step7.AddParameterData(stringData);
        }

        public void AddData(ushort inputData)
        {
            _tpkt.Length += (ushort)Marshal.SizeOf(inputData);
            _step7.AddData(inputData);
        }

        public void AddData(byte inputData)
        {
            _tpkt.Length += (ushort)Marshal.SizeOf(inputData);
            _step7.AddData(inputData);
        }

        public void AddData(byte[] stringData)
        {
            _tpkt.Length += (ushort)stringData.Length;
            _step7.AddData(stringData);
        }


        public void AddCOTPData(ushort inputData)
        {
            _tpkt.Length += (ushort)Marshal.SizeOf(inputData);
            _cotp.AddData(inputData);
        }

        public void AddCOTPData(byte inputData)
        {
            _tpkt.Length += (ushort)Marshal.SizeOf(inputData);
            _cotp.AddData(inputData);
        }

        public void AddCOTPData(byte[] stringData)
        {
            _tpkt.Length += (ushort)stringData.Length;
            _cotp.AddData(stringData);
        }

        public void DeserializeData(byte[] inputBuffer, int startIndex)
        {
            _cotp.DeserializeHeader(inputBuffer, startIndex);
            _cotp.DeserializeData(inputBuffer, startIndex);
            if (_step7 != null)
            {
                _step7.DeserializeHeader(inputBuffer, startIndex);
                _step7.DeserializeData(inputBuffer, startIndex);
            }
        }

        public void DeserializeHeader(byte[] inputBuffer, int startIndex)
        {
            _tpkt.Deserialize(inputBuffer, startIndex);

        }

        public byte[] Serialize()
        {
            byte[] outputData = new byte[Size];
            int startIndex = 0;
            Array.Copy(_tpkt.Serialize(), 0, outputData, startIndex, _tpkt.Size);
            startIndex += _tpkt.Size;

            Array.Copy(_cotp.Serialize(), 0, outputData, startIndex, _cotp.Size);
            startIndex += _cotp.Size;

            if (_step7 != null)
            {
                Array.Copy(_step7.Serialize(), 0, outputData, startIndex, _step7.Size);
                startIndex += _step7.Size;
            }


            return outputData;


        }
    }
}
