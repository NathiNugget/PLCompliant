
using PLCompliant.Enums;
using PLCompliant.Interface;
using PLCompliant.STEP_7;

namespace PLCompliant.RequestModels
{
	/// <summary>
	/// Represents a CR connection request sent to a PLC from master in COTP.
	/// </summary>
    public struct CRConnectRequest : IConvertible<IsoTcpMessage, ushort>
    {
		private byte _pduType;
		private UInt16 _destinationReference;
		private UInt16 _sourceReference;
		private byte _classBits;
		private UInt16 _sourceTSAP;
		private UInt16 __destinationTSAP;
		private byte _tpduSize;


		/// <summary>
		/// Byte flag representing which TPDU size should be used
		/// </summary>
		public byte TPDUSize
		{
			get { return _tpduSize; }
			set { _tpduSize = value; }
		}


		/// <summary>
		/// The destination slot and rack number. This one matters as if it targets a invalid slot and rack number, the PLC will refuse the request
		/// </summary>
		public UInt16 DestinationTSAP
		{
			get { return __destinationTSAP; }
			set { __destinationTSAP = value; }
		}


		/// <summary>
		/// The rack and slot number of the source sending the message. Does not seem to matter much
		/// </summary>
		public UInt16 SourceTSAP
		{
			get { return _sourceTSAP; }
			set { _sourceTSAP = value; }
		}


		/// <summary>
		/// Bitmask which sets various options
		/// </summary>
		public byte ClassBits
		{
			get { return _classBits; }
			set { _classBits = value; }
		}


		public UInt16 SourceReference
		{
			get { return _sourceReference; }
			set { _sourceReference = value; }
		}

		/// <summary>
		/// Seems to always be 0 in all the packets monitored
		/// </summary>
		public UInt16 DestinationReference
		{
			get { return _destinationReference; }
			set { _destinationReference = value; }
		}

		/// <summary>
		/// PDU type, effectively another function code for the COTP payload
		/// </summary>
		public byte PDUType
		{
			get { return _pduType; }
			set { _pduType = value; }
		}

        public IsoTcpMessage Convert(ushort arg)
        {
            var msg = new IsoTcpMessage(
                new TPKTHeader(0x3),
                new COTPMessage(
                    new COTPHeader(),
                    new COTPData()),
                null);
            msg.AddData(_pduType, (byte)IsoTcpDataType.COTPData);
            msg.AddData(_destinationReference, (byte)IsoTcpDataType.COTPData);
            msg.AddData(_sourceReference, (byte)IsoTcpDataType.COTPData);
            msg.AddData(_classBits, (byte)IsoTcpDataType.COTPData);

            msg.AddData((byte)0xc1, (byte)IsoTcpDataType.COTPData); // parameter code 1 (src tsap)
            msg.AddData((byte)0x2, (byte)IsoTcpDataType.COTPData); // param length 1
            msg.AddData(_sourceTSAP, (byte)IsoTcpDataType.COTPData); // param data 1

            msg.AddData((byte)0xc2, (byte)IsoTcpDataType.COTPData); // parameter code 2 (dest tsap)
            msg.AddData((byte)0x2, (byte)IsoTcpDataType.COTPData); // param length 2
            msg.AddData(__destinationTSAP, (byte)IsoTcpDataType.COTPData); // param data 2

            msg.AddData((byte)0xc0, (byte)IsoTcpDataType.COTPData); // parameter code 3 (tpdu size)
            msg.AddData((byte)0x1, (byte)IsoTcpDataType.COTPData); // param length 2
            msg.AddData(_tpduSize, (byte)IsoTcpDataType.COTPData);

			return msg;




        }
    }
}
