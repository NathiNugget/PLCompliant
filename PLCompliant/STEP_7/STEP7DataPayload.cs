using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    public class STEP7DataPayload : IProtocolData
    {
        private byte[] _data;

        public STEP7DataPayload()
        {
            _data = [];
        }

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
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
        public void Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            inputBuffer = inputBuffer.Slice(index, _data.Length); // Optionally, do a size check here?
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
            var newSize = _data.Length + inputData;
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
