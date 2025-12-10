using PLCompliant.Interface;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    public class COTPMessage : IProtocolMessage
    {
        private COTPHeader _header;
        private COTPData _data;

        public COTPData Data
        {
            get { return _data; }
            set { _data = value; }
        }


        public COTPHeader Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public int Size
        {
            get
            {
                return _header.Size + _data.Size;
            }
        }

        public COTPMessage(COTPHeader header, COTPData data)
        {
            _header = header;
            _data = data;
        }

        public int DataSize => throw new NotImplementedException();

        public void AddData(ushort inputData)
        {
            _data.AddData(inputData);
            _header.Length += (byte)Marshal.SizeOf(inputData);
        }

        public void AddData(byte inputData)
        {
            _data.AddData(inputData);
            _header.Length += (byte)Marshal.SizeOf(inputData);
        }

        public void AddData(byte[] stringData)
        {
            _data.AddData(stringData);
            _header.Length += (byte)stringData.Length;
        }

        public void DeserializeData(byte[] inputBuffer, int startIndex)
        {
            _data.Deserialize(inputBuffer, startIndex);
        }

        public void DeserializeHeader(byte[] inputBuffer, int startIndex)
        {
            _header.Deserialize(inputBuffer, startIndex);

        }

        public byte[] Serialize()
        {
            byte[] outputData = new byte[Size];
            int startIndex = 0;
            Array.Copy(_header.Serialize(), 0, outputData, startIndex, _header.Size);
            startIndex += _header.Size;

            Array.Copy(_data.Serialize(), 0, outputData, startIndex, _data.Size);
            startIndex += _data.Size;


            return outputData;


        }
    }
}
