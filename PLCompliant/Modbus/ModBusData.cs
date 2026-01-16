using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.Modbus
{
    /// <summary>
    /// This class represents the data field for the Modbus protocol
    /// </summary>
    public class ModBusData : IProtocolData
    {
        #region instance fields
        /// <summary>
        /// The function code to run on the PLC
        /// </summary>
        public byte _functionCode;
        /// <summary>
        /// The data to send converted to bytes
        /// </summary>
        public byte[] _payload = [];
        #endregion

        #region constructors
        /// <summary>
        /// Empty constructor for easing construction for either unit tests or other cases where an empty constructor should be used
        /// </summary>
        public ModBusData()
        {
        }

        /// <summary>
        /// The normal constructor for the class
        /// </summary>
        /// <param name="functionCode">The function code to be run</param>
        /// <param name="payload">The data to be followed by the function code</param>
        public ModBusData(byte functionCode, byte[] payload)
        {
            _functionCode = functionCode;
            _payload = payload;

        }

        #endregion

        #region methods

        /// <summary>
        /// Override equals to compare to another data-packet
        /// </summary>
        /// <param name="other">Other ModBusData to compare to</param>
        /// <returns>If the objects are equal or not</returns>
        public override bool Equals(object? other)
        {
            if (other is null || other is not ModBusData) return false;
            ModBusData other_data = (ModBusData)other;
            return (Size == other_data.Size && _functionCode == other_data._functionCode && _payload.SequenceEqual(other_data._payload));
        }
        /// <inheritdoc/>
        public void Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            _functionCode = inputBuffer[index];
            index += sizeof(byte);
            inputBuffer = inputBuffer.Slice(index, _payload.Length);
            inputBuffer.CopyTo(_payload);
        }
        /// <inheritdoc/>
        public void Serialize(Span<byte> serializedObj)
        {
            int index = 0;
            serializedObj[index] = _functionCode;
            index += sizeof(byte);
            serializedObj = serializedObj.Slice(index, _payload.Length);

            _payload.CopyTo(serializedObj);
        }
        /// <inheritdoc/>
        public void AddData<T>(T inputData, byte type) where T : unmanaged, IEndianConvertable
        {
            var newSize = _payload.Length + Marshal.SizeOf<T>();
            Array.Resize(ref _payload, newSize);
            inputData.FromHostToNetwork();
            ReadOnlySpan<T> inputSpan = [inputData];
            ReadOnlySpan<byte> outSpan = MemoryMarshal.AsBytes(inputSpan);
            outSpan.CopyTo(_payload);
        }
        /// <inheritdoc/>
        public void AddData(ushort inputData, byte type)
        {
            var oldSize = _payload.Length;
            var newSize = _payload.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref _payload, newSize);
            ReadOnlySpan<ushort> inputSpan = [EndianConverter.FromHostToNetwork(inputData)];
            ReadOnlySpan<byte> inputSpanAsBytes = MemoryMarshal.AsBytes(inputSpan);
            Span<byte> payloadSpan = _payload.AsSpan(oldSize);
            inputSpanAsBytes.CopyTo(payloadSpan);
           
        }
        /// <inheritdoc/>
        public void AddData(byte inputData, byte type)
        {
            var newSize = _payload.Length + Marshal.SizeOf<byte>();
            Array.Resize(ref _payload, newSize);
            _payload[newSize - 1] = inputData;
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
            var oldSize = _payload.Length;
            var newSize = _payload.Length + binarySize;
            Array.Resize(ref _payload, newSize);
            Span<byte> payloadSpan = _payload.AsSpan(oldSize);
            binaryData.CopyTo(payloadSpan);
        }
        /// <inheritdoc/>
        public T GetData<T>(int index, byte type) where T : unmanaged, IEndianConvertable
        {
            T outVar = MemoryMarshal.AsRef<T>(_payload);
            outVar.FromNetworkToHost();
            return outVar;
        }
        /// <inheritdoc/>
        public void ResizeStorage(int newSize)
        {
            Array.Resize(ref _payload, newSize);
        }

        #endregion

        #region properties
        /// <summary>
        /// Property to get the Size of the data + function code in bytes
        /// </summary>
        public int Size { get { return _payload.Length + Marshal.SizeOf(_functionCode); } }
        #endregion
    }


}
