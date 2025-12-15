using PLCompliant.Interface;
using PLCompliant.Utilities;
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
        public STEP7ParameterData(byte functionCode)
        {
            _functionCode = functionCode;
            _data = [];
        }

        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
            FunctionCode = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(FunctionCode);
            Array.Resize(ref _data, inputBuffer.Length - startIndex);
            Array.Copy(inputBuffer, startIndex, _data, 0, _data.Length);
        }

        public byte[] Serialize()
        {
            byte[] outData = new byte[Size];
            outData[0] = _functionCode;
            Array.Copy(_data, 0, outData, 1, _data.Length);
            return outData;
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
    }
}
