using PLCompliant.Enums;
using PLCompliant.Interface;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    public class STEP7Message : IProtocolMessage
    {
        public const ushort STEP7_TCP_PORT = 102;
        private STEP7Header _step7Header;
        private STEP7ParameterData? _step7ParamData;
        private STEP7DataMessage? _step7Data;

        public STEP7Message(STEP7Header step7Header, STEP7ParameterData? step7ParamData, STEP7DataMessage? step7Data)
        {
            _step7Header = step7Header;
            _step7Data = step7Data;
            _step7ParamData = step7ParamData;

        }
        public STEP7Message()
        {
            _step7Header = new();
            _step7ParamData = null;
            _step7ParamData = null;
        }

        public STEP7DataMessage? STEP7Data
        {
            get { return _step7Data; }
            set { _step7Data = value; }
        }

        public STEP7ParameterData? STEP7ParamData
        {
            get { return _step7ParamData; }
            set { _step7ParamData = value; }
        }


        public STEP7Header STEP7Header
        {
            get { return _step7Header; }
            set { _step7Header = value; }
        }

        /// <inheritdoc/>
        public int Size
        {
            get
            {
                int size = _step7Header.Size;
                if (_step7ParamData != null)
                {
                    size += _step7ParamData.Size;
                }
                if (_step7Data != null)
                {
                    size += _step7Data.Size;
                }
                return size;

            }
        }
        /// <inheritdoc/>
        public void Serialize(Span<byte> serializedObj)
        {

            int index = 0;
            _step7Header.Serialize(serializedObj.Slice(index, _step7Header.Size));
            // If the messagetype is ACK-DATA, then we expect to receive the two errorcodes aswell. Otherwise we only use NON_ERROR_LENGTH size instead
            if(_step7Header.MessageType == 0x3)
            {
                index += STEP7Header.NON_ERROR_LENGTH;
            }
            else
            {
                index += _step7Header.Size;
            }      
            if( _step7ParamData != null)
            {
                _step7ParamData.Serialize(serializedObj.Slice(index, _step7ParamData.Size));
                index += _step7ParamData.Size;
            }
            if( _step7Data != null)
            {
                _step7Data.Serialize(serializedObj.Slice(index, _step7Data.Size));
                index += _step7Data.Size;
            }

        }
        public void Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            _step7Header.Deserialize(inputBuffer.Slice(index, Marshal.SizeOf<STEP7Header>())); // Marshal.sizeof here because header may not have some fields depending on the messagetype
            // Only read NON_ERROR_LENGTH bytes if the messagetype is ACK-Data, since the error fields wont be present
            if(_step7Header.MessageType == 0x3)
            {
                index += _step7Header.Size;
            }
            else
            {
                index += STEP7Header.NON_ERROR_LENGTH;
            }

            if (_step7Header.ParameterLength > 0)
            {
                _step7ParamData = new();
                _step7ParamData.ResizeStorage(_step7Header.ParameterLength);
                _step7ParamData.Deserialize(inputBuffer.Slice(index, _step7Header.ParameterLength));
                index += _step7Header.ParameterLength;
            }
            if (_step7Header.DataLength > 0)
            {
                _step7Data = new();
                _step7Data.Deserialize(inputBuffer.Slice(index, _step7Data.Size));
                index += _step7Data.Size;
            }

        }
        /// <inheritdoc/>
        public void AddData<T>(T inputData, byte type) where T : unmanaged, IEndianConvertable
        {
            var flags = (IsoTcpDataType)type;
            if(flags.HasFlag(IsoTcpDataType.STEP7ParamData))
            {
                _step7ParamData.AddData<T>(inputData, type);
                _step7Header.ParameterLength += (ushort)Marshal.SizeOf<T>();
            }
            else
            {
                _step7Data.AddData<T>(inputData, type);
                _step7Header.DataLength += (ushort)Marshal.SizeOf<T>();
            }
        }
        /// <inheritdoc/>
        public void AddData(ushort inputData, byte type)
        {
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.STEP7ParamData))
            {
                _step7ParamData.AddData(inputData, type);
                _step7Header.ParameterLength += (ushort)Marshal.SizeOf(inputData);
            }
            else
            {
                _step7Data.AddData(inputData, type);
                _step7Header.DataLength += (ushort)Marshal.SizeOf(inputData);
            }
        }
        /// <inheritdoc/>
        public void AddData(byte inputData, byte type)
        {
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.STEP7ParamData))
            {
                _step7ParamData.AddData(inputData, type);
                _step7Header.ParameterLength += (ushort)Marshal.SizeOf(inputData);
            }
            else
            {
                _step7Data.AddData(inputData, type);
                _step7Header.DataLength += (ushort)Marshal.SizeOf(inputData);
            }
        }
        /// <inheritdoc/>
        public void AddData(ReadOnlySpan<byte> binaryData, byte type)
        {
            if (binaryData.Length > byte.MaxValue)
            {
                throw new ArgumentException("Input length was greater than allowed in a byte");
            }
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.STEP7ParamData))
            {
                _step7ParamData.AddData(binaryData, type);
                _step7Header.ParameterLength += (ushort)binaryData.Length;
            }
            else
            {
                _step7Data.AddData(binaryData, type);
                _step7Header.DataLength += (ushort)binaryData.Length;
            }
        }
        /// <inheritdoc/>
        public T GetData<T>(int index, byte type) where T : unmanaged, IEndianConvertable
        {
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.STEP7ParamData))
            {
                return _step7ParamData.GetData<T>(index, type);
            }
            else
            {
                return _step7Data.GetData<T>(index, type);
            }
        }

       
    }
}
