

namespace PLCompliant.Enums
{
    static public class EnumToString
    {
        public static string ProtocolType(PLCProtocolType protocolType)
        {
            switch (protocolType)
            {
                case PLCProtocolType.Modbus:
                    return "Modbus";
                case PLCProtocolType.Step_7:
                    return "STEP-7";
                default:
                    return "??";
            }
        }
        public static string ModBusErrorCode(byte code)
        {
            switch (code)
            {
                case 0x1:
                    return "Illegal function";
                case 0x2:
                    return "Illegal data address";
                case 0x3:
                    return "Illegal data value";
                case 0x4:
                    return "Slave device failure";
                case 0x5:
                    return "Acknowledge";
                case 0x6:
                    return "Slave device busy";
                case 0x7:
                    return "Negative acknowledge";
                case 0x8:
                    return "Memory parity error";
                case 0xA:
                    return "Gateway path unavailable";
                case 0xB:
                    return "Gateway target device failed to respond";
                default:
                    return "Unknown error";
            }
        }
    }
}
