namespace PLCompliant.Enums
{
    /// <summary>
    /// This class represents the state machine created while reading a response from a STEP7-PLC
    /// </summary>
    public enum ReceiveState : byte
    {
        ReadingTpktHeader,
        ReadingCotpHeader,
        ReadingCotpData,
        ReadingSTEP7Header,
        ReadingSTEP7HeaderPrelude,
        ReadingSTEP7Parameters,
        ReadingSTEP7Data,
        Finished
    }
}