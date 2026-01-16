namespace PLCompliant.Enums
{
    /// <summary>
    /// This enum several error codes in the params provided in a STEP7-message
    /// </summary>
    public enum STEP7ParamErrorCode : UInt16
    {

        NoError = 0x0,
        InvalidBlockTypeNumber = 0x0110,
        InvalidParameter = 0x0112,
        PGRessourceError = 0x011A,
    }
}
