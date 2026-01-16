using PLCompliant.Interface;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        /// <inheritdoc/>
        public void Serialize(Span<byte> serializedObj)
        {
            int index = 0;
            _header.Serialize(serializedObj.Slice(index));
            index += _header.Size;
            _data.Serialize(serializedObj.Slice(index));
        }
        /// <inheritdoc/>
        public void DeserializeHeader(ReadOnlySpan<byte> inputBuffer)
        {
            _header.Deserialize(inputBuffer);
        }
        /// <inheritdoc/>
        public void DeserializeData(ReadOnlySpan<byte> inputBuffer)
        {
            _data.ResizeStorage(Header.Length);
            _data.Deserialize(inputBuffer);
        }
        /// <inheritdoc/>
        public void AddData<T>(T inputData, byte type) where T : unmanaged, IEndianConvertable
        {
            _data.AddData(inputData, type);
            _header.Length += (byte)Marshal.SizeOf<T>();
        }
        /// <inheritdoc/>
        public void AddData(ushort inputData, byte type)
        {
            _data.AddData(inputData, type);
            _header.Length += (byte)Marshal.SizeOf(inputData);
        }
        /// <inheritdoc/>
        public void AddData(byte inputData, byte type)
        {
            _data.AddData(inputData, type);
            _header.Length += (byte)Marshal.SizeOf(inputData);
        }
        /// <inheritdoc/>
        public void AddData(ReadOnlySpan<byte> binaryData, byte type)
        {
            _data.AddData(binaryData, type);
            _header.Length += (byte)binaryData.Length;
        }
        /// <inheritdoc/>
        public T GetData<T>(int index, byte type) where T : unmanaged, IEndianConvertable
        {
            return _data.GetData<T>(index, type);
        }
    }
}
