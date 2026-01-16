namespace PLCompliant.Enums
{
    public enum ReceiveState : byte
    {
        ReadingTpktHeader,
        ReadingData,
        Finished
    }
}