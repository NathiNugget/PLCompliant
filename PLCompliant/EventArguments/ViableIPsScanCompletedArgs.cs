using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.EventArguments
{
    public class ViableIPsScanCompletedArgs : RaisedEventArgs
    {
        public int To { get; set; }
        public int Current { get; set; }

        public ViableIPsScanCompletedArgs(int to, int current)
        {
            To = to;
            Current = current;
        }
    }
}
