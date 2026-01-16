namespace PLCompliant.Enums
{
    /// <summary>
    /// This enum contains a range of return codes in a STEP7-response from a PLC
    /// </summary>
    public enum STEP7ReturnCode : byte
    {
        Reserved = 0x0,
        HardwareFault = 0x1,
        ObjectAccessingNotAllowed = 0x3,
        AddressOutOfRange = 0x5,
        DataTypeNotSupported = 0x6,
        DataTypeInconsistent = 0x7,
        ObjectDoesNotExist = 0xA,
        ParameterDoesNotExist = 0x20,
        ParameterIsReadOnly = 0x21,
        ParameterValueOutOfRange = 0x22,
        ParameterIndexIsWrong = 0x23,
        ParameterHasNoIndex = 0x24,
        ParameterImperssibile = 0x34,
        DriveESError = 0x39,
        /// <summary>
        /// Default, expected value for when a response is valid
        /// </summary>
        Sucess = 0xff,
    }
}
