using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    public class COTPData : IProtocolData
    {

        private byte _pduType;
        private byte[] _data;

        public COTPData(byte pduType)
        {
            _pduType = pduType;
            _data = [];
        }

        public byte PduType
        {
            get { return _pduType; }
            set { _pduType = value; }
        }

        public byte[] Data
        {
            get { return _data; }
            private set { _data = value; }
        }




        public int Size
        {
            get
            {
                return Marshal.SizeOf(_pduType) + _data.Length;
            }
        }



        public void AddData(ushort inputData)
        {
            var oldSize = _data.Length;
            var newSize = _data.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref _data, newSize);
            byte[] bytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(inputData));
            Array.Copy(bytes, 0, _data, oldSize, bytes.Length);
        }

        public void AddData(byte inputData)
        {
            var newSize = _data.Length + Marshal.SizeOf<byte>();
            Array.Resize(ref _data, newSize);
            _data[newSize - 1] = inputData;
        }

        public void AddData(byte[] stringData)
        {
            if (stringData.Length > byte.MaxValue)
            {
                throw new ArgumentException("Input length was greater than allowed in a byte");
            }
            byte stringSize = (byte)stringData.Length;
            if (stringSize == 0) { return; }
            var oldSize = Data.Length;
            var newSize = _data.Length + stringSize;
            Array.Resize(ref _data, newSize);
            Array.Copy(stringData, 0, _data, oldSize, stringSize);
        }





        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
            _pduType = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_pduType);
            Array.Resize(ref _data, inputBuffer.Length - startIndex);
            Array.Copy(inputBuffer, startIndex, _data, 0, inputBuffer.Length - startIndex);
        }

        public byte[] Serialize()
        {
            int startIndex = 0;
            byte[] outData = new byte[Size];
            outData[startIndex] = _pduType;
            startIndex += Marshal.SizeOf(_pduType);
            Array.Copy(_data, 0, outData, startIndex, _data.Length);
            return outData;
        }
    }
}
