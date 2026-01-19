using PLCompliant.Enums;
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


            var msg = new IsoTcpMessage(
                new TPKTHeader(0x3),
                new COTPMessage(
                    new COTPHeader(),
                    new COTPData()),
                null);
            msg.AddData(0xe0, (byte)IsoTcpDataType.COTPData); // pdu type
            msg.AddData((UInt16)0x0000, (byte)IsoTcpDataType.COTPData); // destination reference
            msg.AddData((UInt16)0x0005, (byte)IsoTcpDataType.COTPData); // source reference
            msg.AddData((byte)0x0, (byte)IsoTcpDataType.COTPData); // class bits
            msg.AddData((byte)0xc1, (byte)IsoTcpDataType.COTPData); // parameter code 1
            msg.AddData((byte)0x2, (byte)IsoTcpDataType.COTPData); // param length 1
            msg.AddData((UInt16)0x100, (byte)IsoTcpDataType.COTPData); // param data 1

            msg.AddData((byte)0xc2, (byte)IsoTcpDataType.COTPData); // parameter code 2
            msg.AddData((byte)0x2, (byte)IsoTcpDataType.COTPData); // param length 2
            msg.AddData((UInt16)0x200, (byte)IsoTcpDataType.COTPData); // param data 2

            msg.AddData((byte)0xc0, (byte)IsoTcpDataType.COTPData); // parameter code 2
            msg.AddData((byte)0x1, (byte)IsoTcpDataType.COTPData); // param length 2
            msg.AddData((byte)CotpTpduSize.Octets1024, (byte)IsoTcpDataType.COTPData); // param data 2 we choose 1024 cos we observed it would work in wireshark with our test device

            return msg;
        }

        /// <summary>
        /// Message to initiate connection request on second rack slot
        /// </summary>
        /// <returns>IsoTCPMessage</returns>
        public IsoTcpMessage CreateCRConnectRequestTwo()
        {
            var msg = new IsoTcpMessage(
                new TPKTHeader(0x3),
                new COTPMessage(
                    new COTPHeader(),
                    new COTPData()),
                null);
            msg.AddData(0xe0, (byte)IsoTcpDataType.COTPData); //pdu type
            msg.AddData((UInt16)0x0000, (byte)IsoTcpDataType.COTPData); // destination reference
            msg.AddData((UInt16)0x00014, (byte)IsoTcpDataType.COTPData); // source reference
            msg.AddData((byte)0x0, (byte)IsoTcpDataType.COTPData); // class bits
            msg.AddData((byte)0xc1, (byte)IsoTcpDataType.COTPData); // parameter code 1
            msg.AddData((byte)0x2, (byte)IsoTcpDataType.COTPData); // param length 1
            msg.AddData((UInt16)0x100, (byte)IsoTcpDataType.COTPData); // param data 1

            msg.AddData((byte)0xc2, (byte)IsoTcpDataType.COTPData); // parameter code 2
            msg.AddData((byte)0x2, (byte)IsoTcpDataType.COTPData); // param length 2
            msg.AddData((UInt16)0x102, (byte)IsoTcpDataType.COTPData); // param data 2

            msg.AddData((byte)0xc0, (byte)IsoTcpDataType.COTPData); // parameter code 3
            msg.AddData((byte)0x1, (byte)IsoTcpDataType.COTPData); // param length 3
            msg.AddData((byte)CotpTpduSize.Octets1024, (byte)IsoTcpDataType.COTPData); // param data 3 we choose 1024 cos we observed it would work in wireshark with our test device



            return msg;



        }

        /// <summary>
        /// Create a message to initilize setup the communication protocol
        /// </summary>
        /// <returns>IsoTcpMessage ready to send</returns>
        public IsoTcpMessage CreateSetupCommunication()
        {
            var msg = new IsoTcpMessage(
                new TPKTHeader(0x3),
                new COTPMessage(
                    new COTPHeader(),
                    new COTPData()),
                new STEP7Message(
                    new STEP7Header(0x32, 0x1, 0),
                    new STEP7ParameterData(),
                    null));
            msg.AddData(0xf0, (byte)IsoTcpDataType.COTPData); // pdu type
            msg.AddData((byte)0x80, (byte)IsoTcpDataType.COTPData); 
            msg.AddData(0xf0, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // function code
            msg.AddData((byte)0x0, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // add reserved field
            msg.AddData((UInt16)0x1, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // Max AMQ (parallel jobs with ack) calling
            msg.AddData((UInt16)0x1, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // Max AMQ (parallel jobs with ack) called
            msg.AddData((UInt16)0x1e0, (byte)(IsoTcpDataType.STEP7Data | IsoTcpDataType.STEP7ParamData)); // PDU length

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
