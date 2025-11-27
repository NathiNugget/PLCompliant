using PLCompliant.Enums;
using PLCompliant.Scanning;

namespace PLCompliant.EventArguments
{
    public class StartViableIPsScanArgs : RaisedEventArgs
    {
        public IPAddressRange AddressRange { get; private set; }
        public PLCProtocolType Protocol { get; private set; }

        public StartViableIPsScanArgs(IPAddressRange addr, PLCProtocolType protocol)
        {
            AddressRange = addr;
            Protocol = protocol;
        }
    }
}
