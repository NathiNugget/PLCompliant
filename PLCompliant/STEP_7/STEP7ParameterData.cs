using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;


namespace PLCompliant.STEP_7
{
    public class STEP7ParameterData : IProtocolData
    {
        private byte[] _data;
        public STEP7ParameterData()
        {
            _data = [];
        }

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }


        public byte FunctionCode
        {
            get { return _data[0]; }
        }
        /// <inheritdoc/>
        public int Size
        {
            get
            {
                return _data.Length;
            }
        }
        /// <inheritdoc/>
        public void AddData(ushort inputData)
        {
            var oldSize = _data.Length;
            var newSize = _data.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref _data, newSize);
            byte[] bytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(inputData));
            Array.Copy(bytes, 0, _data, oldSize, bytes.Length);
        }
        /// <inheritdoc/>
        public void AddData(byte inputData)
        {
            var newSize = _data.Length + Marshal.SizeOf<byte>();
            Array.Resize(ref _data, newSize);
            _data[newSize - 1] = inputData;
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public void Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            inputBuffer = inputBuffer.Slice(index, _data.Length);
            inputBuffer.CopyTo(_data);
        }
        /// <inheritdoc/>
        public void Serialize(Span<byte> serializedObj)
        {
            int index = 0;
            serializedObj = serializedObj.Slice(index, _data.Length);

            _data.CopyTo(serializedObj);
        }
        /// <inheritdoc/>
        public void AddData<T>(T inputData, byte type) where T : unmanaged, IEndianConvertable
        {
            var newSize = _data.Length + Marshal.SizeOf<T>();
            Array.Resize(ref _data, newSize);
            inputData.FromHostToNetwork();
            ReadOnlySpan<T> inputSpan = [inputData];
            ReadOnlySpan<byte> outSpan = MemoryMarshal.AsBytes(inputSpan);
            outSpan.CopyTo(_data);
        }
        /// <inheritdoc/>
        public void AddData(ushort inputData, byte type)
        {
            var oldSize = _data.Length;
            var newSize = _data.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref _data, newSize);
            ReadOnlySpan<ushort> inputSpan = [EndianConverter.FromHostToNetwork(inputData)];
            ReadOnlySpan<byte> inputSpanAsBytes = MemoryMarshal.AsBytes(inputSpan);
            Span<byte> payloadSpan = _data.AsSpan(oldSize);
            inputSpanAsBytes.CopyTo(payloadSpan);
        }
        /// <inheritdoc/>
        public void AddData(byte inputData, byte type)
        {
            var newSize = _data.Length + Marshal.SizeOf<byte>();
            Array.Resize(ref _data, newSize);
            _data[newSize - 1] = inputData;
        }
        /// <inheritdoc/>
        public void AddData(ReadOnlySpan<byte> binaryData, byte type)
        {
            if (binaryData.Length > byte.MaxValue)
            {
                throw new ArgumentException("Input length was greater than allowed in a byte");
            }
            byte binarySize = (byte)binaryData.Length;
            if (binarySize == 0) { return; }
            var oldSize = _data.Length;
            var newSize = _data.Length + binarySize;
            Array.Resize(ref _data, newSize);
            Span<byte> payloadSpan = _data.AsSpan(oldSize);
            binaryData.CopyTo(payloadSpan);
        }
        /// <inheritdoc/>
        public T GetData<T>(int index, byte type) where T : unmanaged, IEndianConvertable
        {
            T outVar = MemoryMarshal.AsRef<T>(_data);
            outVar.FromNetworkToHost();
            return outVar;
        }
        /// <inheritdoc/>
        public void ResizeStorage(int newSize)
        {
            Array.Resize(ref _data, newSize);
        }
    }
}
