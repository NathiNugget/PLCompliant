using PLCompliant.Scanning;
using System.Runtime.InteropServices;

namespace PLCompliant.EventArguments
{
    /// <summary>
    /// Struct used for passing IP address range to an event
    /// </summary>
    public class UpdateThreadArgs : RaisedEventArgs
    {
        /// <summary>
        /// Constructor for the struct
        /// </summary>
        /// <param name="range">Range of IPv4 addresses</param>
        /// 


        public UpdateThreadArgs(IPAddressRange range) : base() {
            AddressRange = range;
        }

        

        /// <summary>
        /// Adress range
        /// </summary>
        public readonly IPAddressRange AddressRange;
    }
}
