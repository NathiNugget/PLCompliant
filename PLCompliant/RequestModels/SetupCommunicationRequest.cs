

using PLCompliant.Enums;
using PLCompliant.Interface;
using PLCompliant.STEP_7;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.RequestModels
{
    /// <summary>
    /// Represents a STEP7 Setup Communication command which should be sent to slave from master.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 6, CharSet = CharSet.Ansi)]
    public struct SetupCommunicationRequest : IConvertible<IsoTcpMessage, ushort>, IEndianConvertable
    {
        [FieldOffset(0)] private UInt16 _maxAMQCalling;
        [FieldOffset(2)] private UInt16 _maxAMQCalled;
        [FieldOffset(4)] private UInt16 _pduLength;

        /// <summary>
        /// Negotiated max length of PDU
        /// </summary>
		public UInt16 PDULength
		{
			get { return _pduLength; }
			set { _pduLength = value; }
		}


		public UInt16 MaxAMQCalled
		{
			get { return _maxAMQCalled; }
			set { _maxAMQCalled = value; }
		}


		public UInt16 MaxAMQCalling
		{
			get { return _maxAMQCalling; }
			set { _maxAMQCalling = value; }
		}
        /// <summary>
        /// Converts this object to a IsoTcpMessage 
        /// </summary>
        /// <param name="pduReference">The pdu reference to be used in the message</param>
        /// <returns>An IsoTcpMessage representing this Setup Communication request</returns>
        public IsoTcpMessage Convert(ushort pduReference)
        {
            IsoTcpMessage msg = new(new(3), new(), new(new(protocolId: 0x32, messageType: 0x1, pduReference: pduReference), new(), null));
            msg.AddData((byte)0xf0, (byte)IsoTcpDataType.COTPData); // add PDU type in COTP
            msg.AddData((byte)0x80, (byte)IsoTcpDataType.COTPData); // TPDU number 0x0, with last data unit set to 1

            msg.AddData((byte)0xf0, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // function code for parameters = Setup Communication
            msg.AddData(this, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // add the rest of the payload, which this struct contains directly
            return msg;


        }

        public void FromHostToNetwork()
        {
            _maxAMQCalling = EndianConverter.FromHostToNetwork(_maxAMQCalling);
            _maxAMQCalled = EndianConverter.FromHostToNetwork(_maxAMQCalled);
            _pduLength = EndianConverter.FromHostToNetwork(_pduLength);
        }

        public void FromNetworkToHost()
        {
            _maxAMQCalling = EndianConverter.FromNetworkToHost(_maxAMQCalling);
            _maxAMQCalled = EndianConverter.FromNetworkToHost(_maxAMQCalled);
            _pduLength = EndianConverter.FromNetworkToHost(_pduLength);
        }
    }
}
