using PLCompliant.Interface;
using System.Runtime.InteropServices;


namespace PLCompliant.STEP_7
{
    public class STEP7ParameterData : IProtocolData
    {
        private byte _functionCode;
        private byte[] _data;

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }


        public byte FunctionCode
        {
            get { return _functionCode; }
            set { _functionCode = value; }
        }

        public int Size
        {
            get
            {
                return Marshal.SizeOf(_functionCode) + _data.Length;
            }
        }

        public void Deserialize(byte[] inputBuffer)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
