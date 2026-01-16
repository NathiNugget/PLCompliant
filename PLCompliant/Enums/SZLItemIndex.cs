namespace PLCompliant.Enums
{
    /// <summary>
    /// This represents the index for which the command is to be sent/read from
    /// </summary>
    public enum SZLItemIndex : UInt16
    {
        Module = 0x1,
        BasicHardware = 0x6,
        /// <summary>
        /// Default command/value
        /// </summary>
        BasicFirmware = 0x7,

    }
}
