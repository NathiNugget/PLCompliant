using PLCompliant.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.STEP_7
{
    public class COTPData : IProtocolData
    {


        private byte[] _data;

        public byte[] Data
        {
            get { return _data; }
            private set { _data = value; }
        }




        public int Size
        {
            get
            {
                return _data.Length;
            }
        }

        public void Deserialize(ArraySegment<byte> inputBuffer)
        {
            Array.Resize(ref _data, inputBuffer.Count);
            inputBuffer.CopyTo(_data, 0);
        }

        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
