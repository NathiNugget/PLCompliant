using PLCompliant.Scanning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.EventArguments
{
    public class StartViableIPsScanArgs : RaisedEventArgs
    {
        public IPAddressRange AddressRange {  get; set; }

        public StartViableIPsScanArgs(IPAddressRange addr)
        {
            AddressRange = addr;
        }
    }
}
