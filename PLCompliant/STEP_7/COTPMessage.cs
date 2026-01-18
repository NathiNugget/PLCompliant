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
        public COTPMessage()
        {
            _header = new();
            _data = new();
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
        public int Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            _header.Deserialize(inputBuffer.Slice(index, _header.Size));
            index += _header.Size;
            _data.ResizeStorage(_header.Length);
            _data.Deserialize(inputBuffer.Slice(index, _data.Size));
            index += _data.Size;
            return index;
        }
        /// <inheritdoc/>
        public int AddData<T>(T inputData, byte type) where T : unmanaged, IEndianConvertable
        {
            int dataAdded = _data.AddData(inputData, type);
            _header.Length += (byte)dataAdded;
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(ushort inputData, byte type)
        {
            int dataAdded = _data.AddData(inputData, type);
            _header.Length += (byte)dataAdded;
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(byte inputData, byte type)
        {
            int dataAdded = _data.AddData(inputData, type);
            _header.Length += (byte)dataAdded;
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(ReadOnlySpan<byte> binaryData, byte type)
        {
            int dataAdded = _data.AddData(binaryData, type);
            _header.Length += (byte)dataAdded;
            return dataAdded;
        }
        /// <inheritdoc/>
        public T GetData<T>(int index, byte type) where T : unmanaged, IEndianConvertable
        {
            return _data.GetData<T>(index, type);
        }

        
    }
}
