

namespace PLCompliant.Enums
{
    /// <summary>
    /// This class converts various enums and values to strings to be represented to the end-user either in UI or in a log
    /// </summary>
    static public class EnumToString
    {

        #region static methods
        /// <summary>
        /// Represent a STEP7ReturnCode as a string
        /// </summary>
        /// <param name="code">Return code</param>
        /// <returns>Either the code as a string-representation or as an unknown error from the PLC</returns>
        public static string STEP7ReturnCode(STEP7ReturnCode code)
        {
            if (Enum.IsDefined(typeof(STEP7ReturnCode), code))
            {
                return code.ToString();
            }
            else
            {
                return "Ukendt Fejl";
            }

        }

        /// <summary>
        /// Convert a PLCProtocolType to a string
        /// </summary>
        /// <param name="protocolType">Runtime instance of a certain protocol</param>
        /// <returns>A string representation of the protocol</returns>
        public static string ProtocolType(PLCProtocolType protocolType)
        {
            switch (protocolType)
            {
                case PLCProtocolType.Modbus:
                    return "Modbus";
                case PLCProtocolType.Step_7:
                    return "STEP-7";
                default:
                    return "??"; // TODO: Replace with unknown_word
            }
        }

        /// <summary>
        /// Convert Modbus error to human readable text
        /// </summary>
        /// <param name="code">The byte value of an error</param>
        /// <returns>A string representation of the PLC</returns>
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


        #endregion

    }
}
