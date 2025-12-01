

namespace PLCompliant.Enums
{
    static public class EnumToString
    {
        public static string ProtocolType(PLCProtocolType protocolType)
        {
            switch(protocolType)
            {
                case PLCProtocolType.Modbus:
                    return "Modbus";
                case PLCProtocolType.Step_7:
                    return "STEP-7";
                default:
                    return "??";
            }
        }
    }
}
