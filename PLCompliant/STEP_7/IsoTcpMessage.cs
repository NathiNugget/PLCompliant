using PLCompliant.Interface;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    public class IsoTcpMessage : IProtocolMessage
    {
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
