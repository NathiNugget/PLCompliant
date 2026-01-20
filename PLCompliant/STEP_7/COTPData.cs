using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This class represents the data portion of a COTP-segment
    /// </summary>
    public class COTPData : IProtocolData, IEquatable<COTPData>
    {

        private byte[] _data;

        public COTPData()
        {
            _data = [];
        }

        #region properties
        /// <summary>
        /// Property for the PDU-type
        /// </summary>
        public byte PduType
        {
            get { return Data[0]; }
            set { Data[0] = value; }
        }
        /// <summary>
        /// Property for the Data-segment of the class
        /// </summary>
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
        #endregion

        /// <inheritdoc/>
        public int AddData<T>(T inputData, byte type) where T : unmanaged, IEndianConvertable
        {
            var dataAdded = Marshal.SizeOf<T>();
            var oldSize = _data.Length;
            var newSize = oldSize + dataAdded;
            Array.Resize(ref _data, newSize);
            inputData.FromHostToNetwork();
            ReadOnlySpan<T> inputSpan = [inputData];
            ReadOnlySpan<byte> outSpan = MemoryMarshal.AsBytes(inputSpan);
            outSpan.CopyTo(_data.AsSpan(oldSize));
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
        public int Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            inputBuffer = inputBuffer.Slice(0, _data.Length);
            inputBuffer.CopyTo(_data);
            return _data.Length;
        }
        /// <inheritdoc/>
        public T GetData<T>(int index, byte type) where T : unmanaged, IEndianConvertable
        {
            T outVar = MemoryMarshal.AsRef<T>(_data.AsSpan(index));
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

        public override bool Equals(object? obj)
        {
            return Equals(obj as COTPData);
        }
        public bool Equals(COTPData? obj)
        {
            if (obj is null) return false;
            
            return _data.SequenceEqual(obj._data);

        }

        public static bool operator ==(COTPData left, COTPData right)
        {
            return object.Equals(left, right);

        }
        public static bool operator !=(COTPData left, COTPData right) { return !(left == right); }









    }
}
