

using PLCompliant.Interface;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PLCompliant.STEP_7
{
    public class STEP7DataMessage : IProtocolMessage
    {
        public STEP7DataMessage(byte returnCode, byte transportType)
        {
            _header = new();
            _data = new();
            _header.ReturnCode = returnCode;
            _header.TransportType = transportType;
            _header.Length = 0;
        }
        public STEP7DataMessage()
        {
            _header = new();
            _data = new();
            _header.ReturnCode = 0;
            _header.TransportType = 0;
            _header.Length = 0;
        }

        private STEP7DataHeader _header = new();
        private STEP7DataPayload _data = new();

        public STEP7DataHeader Header { get { return _header; } }
        public STEP7DataPayload Data { get { return _data; } }
        /// <inheritdoc/>
        public int Size {  get { return _header.Size + _data.Size; } }
        /// <inheritdoc/>
        public void Serialize(Span<byte> serializedObj)
        {
            int index = 0;
            _header.Serialize(serializedObj.Slice(index, _header.Size));
            index += _header.Size;
            _data.Serialize(serializedObj.Slice(index, _data.Size));
            index += _data.Size;
        }
        /// <inheritdoc/>
        public int Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            index += _header.Deserialize(inputBuffer.Slice(index, _header.Size));
            _data.ResizeStorage(_header.Length);
            index += _data.Deserialize(inputBuffer.Slice(index, _header.Length));
            return index;
        }

        /// <inheritdoc/>
        public int AddData<T>(T inputData, byte type) where T : unmanaged, IEndianConvertable
        {
            int dataAdded = _data.AddData<T>(inputData, type);
            _header.Length += (ushort)dataAdded;
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(ushort inputData, byte type)
        {
            int dataAdded = _data.AddData(inputData, type);
            _header.Length += (ushort)dataAdded;
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(byte inputData, byte type)
        {
            int dataAdded =_data.AddData(inputData, type);
            _header.Length += (ushort)dataAdded;
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(ReadOnlySpan<byte> binaryData, byte type)
        {
            if (binaryData.Length > byte.MaxValue)
            {
                throw new ArgumentException("Input length was greater than allowed in a byte");
            }
            int dataAdded = _data.AddData(binaryData, type);
            _header.Length += (ushort)dataAdded;
            return dataAdded;
        }
        /// <inheritdoc/>
        public T GetData<T>(int index, byte type) where T : unmanaged, IEndianConvertable
        {
            return _data.GetData<T>(index, type);
        }

       
    }
}
