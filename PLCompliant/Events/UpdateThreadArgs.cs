using PLCompliant.Scanning;

namespace PLCompliant.Events
{
    /// <summary>
    /// Struct used for passing IP address range to an event
    /// </summary>
    public readonly struct UpdateThreadArgs
    {
        /// <summary>
        /// Constructor for the struct
        /// </summary>
        /// <param name="range">Range of IPv4 addresses</param>
        public UpdateThreadArgs(IPAddressRange range)
        {
            addressRange = range;
        }
        /// <summary>
        /// Adress range
        /// </summary>
        public readonly IPAddressRange addressRange;
    }
}
