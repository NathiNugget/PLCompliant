using PLCompliant.Scanning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Events
{
    public readonly struct MainThreadArgs
    {
        public MainThreadArgs(IPAddressRange range)
        {
            addressRange = range;   
        }
        public readonly IPAddressRange addressRange;
    }
}
