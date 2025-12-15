using PLCompliant.Enums;

namespace PLCompliant.STEP_7
{
    public class IsoTcpMessageFactory
    {
        public IsoTcpMessage CreateCRConnectRequestOne()
        {


            var msg = new IsoTcpMessage(
                new TPKTHeader(0x3),
                new COTPMessage(
                    new COTPHeader(),
                    new COTPData(0xe0)),
                null);
            msg.AddCOTPData((UInt16)0x0000); // destination reference
            msg.AddCOTPData((UInt16)0x0005); // source reference
            msg.AddCOTPData((byte)0x0); // class bits
            msg.AddCOTPData((byte)0xc1); // parameter code 1
            msg.AddCOTPData((byte)0x2); // param length 1
            msg.AddCOTPData((UInt16)0x100); // param data 1

            msg.AddCOTPData((byte)0xc2); // parameter code 2
            msg.AddCOTPData((byte)0x2); // param length 2
            msg.AddCOTPData((UInt16)0x200); // param data 2

            msg.AddCOTPData((byte)0xc0); // parameter code 2
            msg.AddCOTPData((byte)0x1); // param length 2
            msg.AddCOTPData((byte)CotpTpduSize.Octets1024); // param data 2 we choose 1024 cos we observed it would work in wireshark with our test device

            return msg;
        }
        public IsoTcpMessage CreateCRConnectRequestTwo()
        {
            var msg = new IsoTcpMessage(
                new TPKTHeader(0x3),
                new COTPMessage(
                    new COTPHeader(),
                    new COTPData(0xe0)),
                null);

            msg.AddCOTPData((UInt16)0x0000); // destination reference
            msg.AddCOTPData((UInt16)0x00014); // source reference
            msg.AddCOTPData((byte)0x0); // class bits
            msg.AddCOTPData((byte)0xc1); // parameter code 1
            msg.AddCOTPData((byte)0x2); // param length 1
            msg.AddCOTPData((UInt16)0x100); // param data 1

            msg.AddCOTPData((byte)0xc2); // parameter code 2
            msg.AddCOTPData((byte)0x2); // param length 2
            msg.AddCOTPData((UInt16)0x102); // param data 2

            msg.AddCOTPData((byte)0xc0); // parameter code 3
            msg.AddCOTPData((byte)0x1); // param length 3
            msg.AddCOTPData((byte)CotpTpduSize.Octets1024); // param data 3 we choose 1024 cos we observed it would work in wireshark with our test device



            return msg;



        }
        public IsoTcpMessage CreateSetupCommunication()
        {
            var msg = new IsoTcpMessage(
                new TPKTHeader(0x3),
                new COTPMessage(
                    new COTPHeader(),
                    new COTPData(0xf0)),
                new STEP7Message(
                    new STEP7Header(0x32, 0x1, 0),
                    new STEP7ParameterData(0xf0),
                    null));
            msg.AddCOTPData((byte)0x80); // T
                                         // number
            msg.AddParameterData((byte)0x0); // add reserved field
            msg.AddParameterData((UInt16)0x1); // Max AMQ (parallel jobs with ack) calling
            msg.AddParameterData((UInt16)0x1); // Max AMQ (parallel jobs with ack) called
            msg.AddParameterData((UInt16)0x1e0); // PDU length

            return msg;
        }
        public IsoTcpMessage CreateReadSZL()
        {
            var msg = new IsoTcpMessage(
                new TPKTHeader(0x3),
                new COTPMessage(
                    new COTPHeader(),
                    new COTPData(0xf0)),
                new STEP7Message(
                    new STEP7Header(0x32, 0x7, 0),
                    new STEP7ParameterData(0x00),
                    new STEP7Data(0xff, 0x09)));
            msg.AddCOTPData((byte)0x80); // tpdu number mask

            msg.AddParameterData((byte)0x1); // item count

            msg.AddParameterData((byte)0x12); // Variable specification
            msg.AddParameterData((byte)0x4); // len of var specification
            msg.AddParameterData((byte)0x11); // syntax id
            msg.AddParameterData((byte)0x44); // function group bitmask

            msg.AddParameterData((byte)0x1); // subfunction: read SZL
            msg.AddParameterData((byte)0x0); // sequence num

            msg.AddData((UInt16)0x11); // SLZ-id bitmask
            msg.AddData((UInt16)0x1); // SLZ-index
            return msg;
        }
    }
}
