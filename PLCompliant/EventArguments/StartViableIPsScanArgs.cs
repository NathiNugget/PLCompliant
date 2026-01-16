using PLCompliant.Enums;
using PLCompliant.Scanning;

namespace PLCompliant.EventArguments
{
    /// <summary>
    /// This class contains the arguments for when an IP-scan event has to be raised
    /// </summary>
    public class StartViableIPsScanArgs : RaisedEventArgs
    {
        #region constructor
        /// <summary>
        /// Constructor for this class
        /// </summary>
        /// <param name="addr">The IP-address range to scan for</param>
        /// <param name="protocol">The protocol to be used for the scan</param>

        public StartViableIPsScanArgs(IPAddressRange addr, PLCProtocolType protocol)
        {
            AddressRange = addr;
            Protocol = protocol;
        }
        #endregion

        #region properties
        /// <summary>
        /// The range of IP-addresses to scan
        /// </summary>
        public IPAddressRange AddressRange { get; private set; }
        /// <summary>
        /// The PLC-protocol to scan
        /// </summary>
        public PLCProtocolType Protocol { get; private set; }
        #endregion



    }
}
