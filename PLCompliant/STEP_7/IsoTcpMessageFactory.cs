using PLCompliant.Enums;
using PLCompliant.RequestModels;
using System.Net;

namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This class provides a factory for IsoTCPMessages
    /// </summary>
    public class IsoTcpMessageFactory
    {
        #region methods
        /// <summary>
        /// Message to initiate connection request on first rack slot
        /// </summary>
        /// <returns>IsoTcpMessage with the response</returns>
        public IsoTcpMessage CreateCRConnectRequestOne()
        {

            CRConnectRequest request = new() {PDUType = 0xe0, DestinationReference = 0x0, SourceReference = 0x5, ClassBits = 0x0, SourceTSAP = 0x0100, DestinationTSAP = 0x0200, TPDUSize = (byte)CotpTpduSize.Octets1024 };
            var msg = request.Convert(0);


            return msg;
        }

        /// <summary>
        /// Message to initiate connection request on second rack slot
        /// </summary>
        /// <returns>IsoTCPMessage</returns>
        public IsoTcpMessage CreateCRConnectRequestTwo()
        {

            CRConnectRequest request = new() { PDUType = 0xe0, DestinationReference = 0x0, SourceReference = 0x14, ClassBits = 0x0, SourceTSAP = 0x0100, DestinationTSAP = 0x0102, TPDUSize = (byte)CotpTpduSize.Octets1024 };
            var msg = request.Convert(0);

            return msg;



        }

        /// <summary>
        /// Create a message to initilize setup the communication protocol
        /// </summary>
        /// <returns>IsoTcpMessage ready to send</returns>
        public IsoTcpMessage CreateSetupCommunication()
        {
            SetupCommunicationRequest request = new() { MaxAMQCalling = 1, MaxAMQCalled = 1, PDULength = 480 }; // pdu length HEX: 0x1e0
            var msg = request.Convert(0);
            return msg;
        }

        /// <summary>
        /// Create message to Read SZL (read firmware)
        /// </summary>
        /// <returns>IsoTcpMessage ready to send</returns>
        public IsoTcpMessage CreateReadSZL()
        {
            var msg = new IsoTcpMessage(
                new TPKTHeader(0x3),
                new COTPMessage(
                    new COTPHeader(),
                    new COTPData()),
                new STEP7Message(
                    new STEP7Header(0x32, 0x7, 0),
                    new STEP7ParameterData(),
                    new STEP7DataMessage(0xff, 0x09))); 
            msg.AddData(0xf0, (byte)IsoTcpDataType.COTPData); // pdu type
            msg.AddData((byte)0x80, (byte)IsoTcpDataType.COTPData); // tpdu number mask

            msg.AddData(0x00, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // function code
            msg.AddData((byte)0x1, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // item count

            msg.AddData((byte)0x12, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // Variable specification
            msg.AddData((byte)0x4, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // len of var specification
            msg.AddData((byte)0x11, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // syntax id
            msg.AddData((byte)0x44, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // function group bitmask

            msg.AddData((byte)0x1, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // subfunction: read SZL
            msg.AddData((byte)0x0, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // sequence num

            msg.AddData((UInt16)0x11, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7RegularData)); // SLZ-id bitmask
            msg.AddData((UInt16)0x1, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7RegularData)); // SLZ-index
            return msg;
        }
        #endregion
    }
}
