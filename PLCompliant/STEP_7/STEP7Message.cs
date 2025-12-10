using PLCompliant.Interface;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    public class STEP7Message : IProtocolMessage
    {
        private STEP7Header _step7Header;
        private STEP7ParameterData? _step7ParamData;
        private STEP7Data? _step7Data;

        public STEP7Message(STEP7Header step7Header, STEP7ParameterData? step7ParamData, STEP7Data? step7Data)
        {
            _step7Header = step7Header;
            _step7Data = step7Data;
            _step7ParamData = step7ParamData;

        }

        public STEP7Data STEP7Data
        {
            get { return _step7Data; }
            set { _step7Data = value; }
        }

        public STEP7ParameterData STEP7ParamData
        {
            get { return _step7ParamData; }
            set { _step7ParamData = value; }
        }


        public STEP7Header STEP7Header
        {
            get { return _step7Header; }
            set { _step7Header = value; }
        }




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

        public int DataSize => throw new NotImplementedException();

        public void AddData(ushort inputData)
        {
            if (_step7Data == null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }
            _step7Data.AddData(inputData);
            _step7Header.DataLength += (ushort)Marshal.SizeOf(inputData);
        }

        public void AddData(byte inputData)
        {
            if (_step7Data == null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }
            _step7Data.AddData(inputData);
            _step7Header.DataLength += (ushort)Marshal.SizeOf(inputData);
        }

        public void AddData(byte[] stringData)
        {
            if (_step7Data == null)
            {
                throw new ArgumentNullException(nameof(stringData));
            }
            _step7Data.AddData(stringData);
            _step7Header.DataLength += (ushort)stringData.Length;
        }



        public void AddParameterData(ushort inputData)
        {
            if (_step7ParamData == null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }
            _step7ParamData.AddData(inputData);
            _step7Header.ParameterLength += (ushort)Marshal.SizeOf(inputData);
        }

        public void AddParameterData(byte inputData)
        {
            if (_step7ParamData == null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }
            _step7ParamData.AddData(inputData);
            _step7Header.ParameterLength += (ushort)Marshal.SizeOf(inputData);
        }

        public void AddParameterData(byte[] stringData)
        {
            if (_step7ParamData == null)
            {
                throw new ArgumentNullException(nameof(stringData));
            }
            _step7ParamData.AddData(stringData);
            _step7Header.ParameterLength += (ushort)stringData.Length;
        }

        public void DeserializeData(byte[] inputBuffer, int startIndex)
        {
            if (_step7ParamData != null)
            {
                _step7ParamData.Deserialize(inputBuffer, startIndex);
                startIndex += _step7ParamData.Size;
            }
            if (_step7Data != null)
            {
                _step7Data.Deserialize(inputBuffer, startIndex);
                startIndex += _step7Data.Size;
            }
        }

        public void DeserializeHeader(byte[] inputBuffer, int startIndex)
        {
            _step7Header.Deserialize(inputBuffer, startIndex);

        }

        public byte[] Serialize()
        {
            byte[] outputData = new byte[Size];
            int startIndex = 0;
            Array.Copy(_step7Header.Serialize(), 0, outputData, startIndex, _step7Header.Size);
            startIndex += _step7Header.Size;

            if (_step7ParamData != null)
            {
                Array.Copy(_step7ParamData.Serialize(), 0, outputData, startIndex, _step7ParamData.Size);
                startIndex += _step7ParamData.Size;
            }
            if (_step7Data != null)
            {
                Array.Copy(_step7Data.Serialize(), 0, outputData, startIndex, _step7Data.Size);
                startIndex += _step7Data.Size;
            }
            return outputData;


        }
    }
}
