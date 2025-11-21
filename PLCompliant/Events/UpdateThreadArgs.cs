using PLCompliant.Scanning;

namespace PLCompliant.Events
{
    public readonly struct UpdateThreadArgs
    {
        public UpdateThreadArgs(IPAddressRange range)
        {
            addressRange = range;
        }
        public readonly IPAddressRange addressRange;
    }
}
