using PLCompliant.Interface;
using PLCompliant.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.STEP_7
{
    public class STEP7Data : IProtocolData
    {
		private byte _returnCode;
		private byte _transportType;
		private UInt16 _length;
		private byte[] _data;

		public byte[] Data
		{
			get { return _data; }
			set { _data = value; }
		}

		public UInt16 Length
		{
			get { return _length; }
			set { _length = value; }
		}



		public byte TransportType
		{
			get { return _transportType; }
			set { _transportType = value; }	
		}

		public byte ReturnCode
		{
			get { return _returnCode; }
			set { _returnCode = value; }
		}

        public int Size
		{
			get
			{
				return Marshal.SizeOf(_returnCode) + Marshal.SizeOf(_transportType) + Marshal.SizeOf(_length) + _data.Length;
			}
		}

        public void Deserialize(byte[] inputBuffer, int startIndex)
        {
			_returnCode = inputBuffer[startIndex];
            _transportType = inputBuffer[startIndex + 1];
			_length = EndianConverter.FromNetworkToHost( BitConverter.ToUInt16(inputBuffer, startIndex + 2));
			Array.Resize(ref _data, _length);
			Array.Copy(inputBuffer, startIndex + 4, _data, 0, _length);
        }

		public byte[] Serialize()
		{
			byte[] outData = new byte[Size];
			outData[0] = _returnCode;
			outData[1] = _transportType;
			var lengthAsBytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork( _length));
			outData[2] = lengthAsBytes[0];
			outData[3] = lengthAsBytes[1];
			Array.Copy(_data, 0, outData, 4, _data.Length);
			return outData;

        }
    }
}
