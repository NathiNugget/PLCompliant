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
        public int Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            inputBuffer = inputBuffer.Slice(0, _data.Length);
            inputBuffer.CopyTo(_data);
            return _data.Length;
        }
        /// <inheritdoc/>
        public void Serialize(Span<byte> serializedObj)
        {
            int index = 0;
            serializedObj = serializedObj.Slice(index, _data.Length);

            _data.CopyTo(serializedObj);
        }
        /// <inheritdoc/>
        public int AddData<T>(T inputData, byte type) where T : unmanaged, IEndianConvertable
        {
            var dataAdded = Marshal.SizeOf<T>();
            var newSize = _data.Length + dataAdded;
            Array.Resize(ref _data, newSize);
            inputData.FromHostToNetwork();
            ReadOnlySpan<T> inputSpan = [inputData];
            ReadOnlySpan<byte> outSpan = MemoryMarshal.AsBytes(inputSpan);
            outSpan.CopyTo(_data);
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(ushort inputData, byte type)
        {
            var dataAdded = Marshal.SizeOf(inputData);
            var oldSize = _data.Length;
            var newSize = _data.Length + dataAdded;
            Array.Resize(ref _data, newSize);
            ReadOnlySpan<ushort> inputSpan = [EndianConverter.FromHostToNetwork(inputData)];
            ReadOnlySpan<byte> inputSpanAsBytes = MemoryMarshal.AsBytes(inputSpan);
            Span<byte> payloadSpan = _data.AsSpan(oldSize);
            inputSpanAsBytes.CopyTo(payloadSpan);
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(byte inputData, byte type)
        {
            var dataAdded = Marshal.SizeOf(inputData);
            var newSize = _data.Length + dataAdded;
            Array.Resize(ref _data, newSize);
            _data[newSize - 1] = inputData;
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(ReadOnlySpan<byte> binaryData, byte type)
        {
            if (binaryData.Length > byte.MaxValue)
            {
                throw new ArgumentException("Input length was greater than allowed in a byte");
            }
            byte binarySize = (byte)binaryData.Length;
            if (binarySize == 0) { return 0; }
            var oldSize = _data.Length;
            var newSize = _data.Length + binarySize;
            Array.Resize(ref _data, newSize);
            Span<byte> payloadSpan = _data.AsSpan(oldSize);
            binaryData.CopyTo(payloadSpan);
            return binaryData.Length;
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
