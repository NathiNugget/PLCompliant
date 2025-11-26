using PLCompliant.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.EventArguments
{
    public class GenerateCSVArgs : RaisedEventArgs
    {
        public string Path { get; private set; }
        public PLCProtocolType Protocol { get; private set; }

        public GenerateCSVArgs(string path, PLCProtocolType protocol)
        {
            Path = path;
            Protocol = protocol; 
        }
    }
}
