using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.EventArguments
{
    public class WriteWindowLogArgs : RaisedEventArgs
    {
        public WriteWindowLogArgs(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
