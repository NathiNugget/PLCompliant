using PLCompliant.Enums;
using PLCompliant.Interface;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This class contains the header and maybe parameter data, maybe data
    /// </summary>
    public class STEP7Message : IProtocolMessage, IEquatable<STEP7Message>
    {
        /// <summary>
        /// Port number for STEP7-communication via ISO-over-TCP
        /// </summary>
        public const ushort STEP7_TCP_PORT = 102;
        private STEP7Header _step7Header;
        private STEP7ParameterData? _step7ParamData;
        private STEP7DataMessage? _step7Data;

        public STEP7Message(STEP7Header step7Header, STEP7ParameterData? step7ParamData, STEP7DataMessage? step7Data)
        {
            _step7Header = step7Header;
            _step7Data = step7Data;
            _step7ParamData = step7ParamData;
            if(_step7ParamData is not null )
            {
                _step7Header.ParameterLength += (ushort)_step7ParamData.Size;
            }
            if(_step7Data is not null)
            {
                _step7Header.DataLength += (ushort)_step7Data.Size;
            }

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

        /// <summary>
        /// Get and set STEP7-header
        /// </summary>
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
            if( _step7ParamData is not null)
            {
                _step7ParamData.Serialize(serializedObj.Slice(index, _step7ParamData.Size));
                index += _step7ParamData.Size;
            }
            if( _step7Data is not null)
            {
                _step7Data.Serialize(serializedObj.Slice(index, _step7Data.Size));
                index += _step7Data.Size;
            }

        }
        public int Deserialize(ReadOnlySpan<byte> inputBuffer)
        {
            int index = 0;
            index += _step7Header.Deserialize(inputBuffer.Slice(index, Marshal.SizeOf<STEP7Header>())); // Marshal.sizeof here because header may not have some fields depending on the messagetype
            

            if (_step7Header.ParameterLength > 0)
            {
                _step7ParamData = new();
                index += _step7ParamData.Deserialize(inputBuffer.Slice(index, _step7Header.ParameterLength));
            }
            if (_step7Header.DataLength > 0)
            {
                _step7Data = new();
                index += _step7Data.Deserialize(inputBuffer.Slice(index, _step7Header.DataLength));
            }
            return index;

        }
        /// <inheritdoc/>
        public int AddData<T>(T inputData, byte type) where T : unmanaged, IEndianConvertable
        {
            int dataAdded = 0;
            var flags = (IsoTcpDataType)type;
            if(flags.HasFlag(IsoTcpDataType.STEP7ParamData))
            {
                if(_step7ParamData is null)
                {
                    _step7ParamData = new(); // No need to increment dataAdded, as the step7 param data is initialized with only a empty array
                }
                dataAdded = _step7ParamData.AddData<T>(inputData, type);
                _step7Header.ParameterLength += (ushort)dataAdded;
            }
            else
            {
                if (_step7Data is null)
                {
                    _step7Data = new(); // This one does need to be incremented since step7data has its own header which is initialized with it
                    dataAdded += _step7Data.Size;
                }
                dataAdded += _step7Data.AddData<T>(inputData, type);
                _step7Header.DataLength += (ushort)dataAdded;
            }
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(ushort inputData, byte type)
        {
            int dataAdded = 0;
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.STEP7ParamData))
            {
                if (_step7ParamData is null)
                {
                    _step7ParamData = new(); // No need to increment dataAdded, as the step7 param data is initialized with only a empty array
                }
                dataAdded = _step7ParamData.AddData(inputData, type);
                _step7Header.ParameterLength += (ushort)dataAdded;
            }
            else
            {
                if (_step7Data is null)
                {
                    _step7Data = new(); // This one does need to be incremented since step7data has its own header which is initialized with it
                    dataAdded += _step7Data.Size;
                }
                dataAdded += _step7Data.AddData(inputData, type);
                _step7Header.DataLength += (ushort)dataAdded;
        
            }
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(byte inputData, byte type)
        {
            int dataAdded = 0;
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.STEP7ParamData))
            {
                if (_step7ParamData is null)
                {
                    _step7ParamData = new(); // No need to increment dataAdded, as the step7 param data is initialized with only a empty array
                }
                dataAdded = _step7ParamData.AddData(inputData, type);
                _step7Header.ParameterLength += (ushort)dataAdded;
            }
            else
            {
                if (_step7Data is null)
                {
                    _step7Data = new(); // This one does need to be incremented since step7data has its own header which is initialized with it
                    dataAdded += _step7Data.Size;
                }
                dataAdded += _step7Data.AddData(inputData, type);
                _step7Header.DataLength += (ushort)dataAdded;
            }
            return dataAdded;
        }
        /// <inheritdoc/>
        public int AddData(ReadOnlySpan<byte> binaryData, byte type)
        {
            int dataAdded = 0;
            var flags = (IsoTcpDataType)type;
            if (flags.HasFlag(IsoTcpDataType.STEP7ParamData))
            {
                if (_step7ParamData is null)
                {
                    _step7ParamData = new(); // No need to increment dataAdded, as the step7 param data is initialized with only a empty array
                }
                dataAdded = _step7ParamData.AddData(binaryData, type);
                _step7Header.ParameterLength += (ushort)dataAdded;
            }
            else
            {
                if (_step7Data is null)
                {
                    _step7Data = new(); // This one does need to be incremented since step7data has its own header which is initialized with it
                    dataAdded += _step7Data.Size;
                }
                dataAdded += _step7Data.AddData(binaryData, type);
                _step7Header.DataLength += (ushort)dataAdded;
            }
            return dataAdded;
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
        public override bool Equals(object? obj)
        {
            return Equals(obj as STEP7Message);
        }
        public bool Equals(STEP7Message? obj)
        {
            bool bothDataNull = false;
            bool bothParamNull = false;
            if (obj is null) return false;
            if(STEP7Data is null || obj.STEP7Data is null)
            {
                if(STEP7Data is null && obj.STEP7Data is null)
                {
                    bothDataNull = true;
                }
                else
                {
                    return false;
                }
                
            }
            if (STEP7ParamData is null || obj.STEP7ParamData is null)
            {
                if (STEP7ParamData is null && obj.STEP7ParamData is null)
                {
                    bothParamNull = true;
                }
                else
                {
                    return false;
                }

            }
            return STEP7Header.Equals(obj.STEP7Header) && (bothDataNull || STEP7Data.Equals(obj.STEP7Data)) && (bothParamNull || STEP7ParamData.Equals(obj.STEP7ParamData));

        }

        public static bool operator ==(STEP7Message left, STEP7Message right)
        {
            return object.Equals(left, right);

        }
        public static bool operator !=(STEP7Message left, STEP7Message right) { return !(left == right); }


    }
}
