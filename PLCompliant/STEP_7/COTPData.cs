using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    public class COTPData : IProtocolData
    {

        private byte[] _data;

        public COTPData(byte pduType)
        {
            _data = [];
        }
        public COTPData()
        {
            _data = [];
        }

        public byte PduType
        {
            get { return Data[0]; }
            set { Data[0] = value; }
        }

        public byte[] Data
        {
            get { return _data; }
            private set { _data = value; }
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
        public void Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            inputBuffer = inputBuffer.Slice(index, _data.Length);
            inputBuffer.CopyTo(_data);
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

        /// <inheritdoc/>
        public void Serialize(Span<byte> serializedObj)
        {
            int index = 0;
            serializedObj = serializedObj.Slice(index, _data.Length);

            _data.CopyTo(serializedObj);
        }
    }
}
