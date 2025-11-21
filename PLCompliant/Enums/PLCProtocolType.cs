namespace PLCompliant.Enums
{
    /// <summary>
    /// The possible protocols to scan
    /// </summary>
    public enum PLCProtocolType
    {
        /// <summary>
        /// The Modbus protocol, used for Modicon M340 in the current application
        /// </summary>
        Modbus = 0,
        /// <summary>
        /// The protocol for Siemens devices. Currently not used since the protocol is proprietary
        /// </summary>
        Step_7 = 1,

    }
}
