namespace PLCompliant.Enums
{
    public enum RecieveState : byte
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