using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    public class STEP7Data : IProtocolData
    {
        private byte _returnCode;
        private byte _transportType;
        private UInt16 _length;
        private byte[] _data;

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public UInt16 Length
        {
            get { return _length; }
            set { _length = value; }
        }



        public byte TransportType
        {
            get { return _transportType; }
            set { _transportType = value; }
        }

        public byte ReturnCode
        {
            get { return _returnCode; }
            set { _returnCode = value; }
        }

        public int Size
        {
            get
            {
                return Marshal.SizeOf(_returnCode) + Marshal.SizeOf(_transportType) + Marshal.SizeOf(_length) + _data.Length;
            }
        }
        public STEP7Data(byte returnCode, byte transportType)
        {
            _returnCode = returnCode;
            _transportType = transportType;
            _length = 0;
            _data = [];
        }

        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
            _returnCode = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_returnCode);
            _transportType = inputBuffer[startIndex];
            startIndex += Marshal.SizeOf(_transportType);
            _length = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(inputBuffer, startIndex));
            startIndex += Marshal.SizeOf(_length);
            Array.Resize(ref _data, _length);
            Array.Copy(inputBuffer, startIndex, _data, 0, _length);
        }

        public byte[] Serialize()
        {
            int startIndex = 0;

            byte[] outData = new byte[Size];
            outData[startIndex] = _returnCode;
            startIndex += Marshal.SizeOf(_returnCode);
            outData[startIndex] = _transportType;
            startIndex += Marshal.SizeOf(_transportType);
            var lengthAsBytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(_length));
            outData[startIndex] = lengthAsBytes[0];
            startIndex += 1;
            outData[startIndex] = lengthAsBytes[1];
            startIndex += 1;
            Array.Copy(_data, 0, outData, startIndex, _data.Length);
            return outData;

        }

        public void AddData(ushort inputData)
        {
            var oldSize = _data.Length;
            var newSize = _data.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref _data, newSize);
            byte[] bytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(inputData));
            Array.Copy(bytes, 0, _data, oldSize, bytes.Length);
            _length += (ushort)Marshal.SizeOf(inputData);
        }

        public void AddData(byte inputData)
        {
            var newSize = _data.Length + Marshal.SizeOf<byte>();
            Array.Resize(ref _data, newSize);
            _data[newSize - 1] = inputData;
            _length += (ushort)Marshal.SizeOf(inputData);
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
            _length += (ushort)stringData.Length;
        }

    }
}
