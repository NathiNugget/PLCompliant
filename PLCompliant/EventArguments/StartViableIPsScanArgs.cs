using PLCompliant.Scanning;

namespace PLCompliant.EventArguments
{
    public class StartViableIPsScanArgs : RaisedEventArgs
    {
        public IPAddressRange AddressRange { get; set; }

        public StartViableIPsScanArgs(IPAddressRange addr)
        {
            AddressRange = addr;
        }
    }
}
