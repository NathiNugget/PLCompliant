using PLCompliant.Interface;
using PLCompliant.Modbus;

namespace PLCompliant.STEP_7
{
    public class STEP7Message : IProtocolMessage
    {
        private TPKTHeader _tpkt;
        private COTPHeader _cotpHeader;
        private COTPData _cotpData;
        private STEP7Header _step7Header;
        private STEP7ParameterData? _step7ParamData;
        private STEP7Data? _step7Data;

        public STEP7Message(TPKTHeader tpkt, COTPHeader cotpHeader, COTPData cotpData, STEP7Header step7Header, STEP7ParameterData step7ParamData, STEP7Data step7Data)
        {
            _tpkt = tpkt;
            _cotpHeader = cotpHeader;
            _cotpData = cotpData;
            _step7Header = step7Header;
            _step7Data = step7Data;
            _step7ParamData = step7ParamData;

        }

        public STEP7Data STEP7Data
        {
            get { return _step7Data; }
            set { _step7Data = value; }
        }

        public STEP7ParameterData STEP7ParamData
        {
            get { return _step7ParamData; }
            set { _step7ParamData = value; }
        }


        public STEP7Header STEP7Header
        {
            get { return _step7Header; }
            set { _step7Header = value; }
        }


        public COTPData COTPData
        {
            get { return _cotpData; }
            set { _cotpData = value; }
        }


        public COTPHeader COTPHeader
        {
            get { return _cotpHeader; }
            set { _cotpHeader = value; }
        }


        public TPKTHeader TPKT
        {
            get { return _tpkt; }
            set { _tpkt = value; }
        }




        public int TotalSize
        {
            get
            {
                int size = _tpkt.Size + _cotpHeader.Size + _cotpData.Size + _step7Header.Size;
                if(_step7ParamData != null)
                {
                    size += _step7ParamData.Size;
                }
                if(_step7Data != null)
                {
                    size += _step7Data.Size;
                }
                return size;

            }
        }

        public int DataSize => throw new NotImplementedException();

        public ModBusData Data => throw new NotImplementedException();

        public void AddData(ushort inputData)
        {
            throw new NotImplementedException();
        }

        public void AddData(byte inputData)
        {
            throw new NotImplementedException();
        }

        public void AddData(byte[] stringData)
        {
            throw new NotImplementedException();
        }



        public void AddParameterData(ushort inputData)
        {
            throw new NotImplementedException();
        }

        public void AddParameterData(byte inputData)
        {
            throw new NotImplementedException();
        }

        public void AddParameterData(byte[] stringData)
        {
            throw new NotImplementedException();
        }










        public void DeserializeData(byte[] inputBuffer)
        {
            throw new NotImplementedException();
        }

        public void DeserializeHeader(byte[] inputBuffer)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
