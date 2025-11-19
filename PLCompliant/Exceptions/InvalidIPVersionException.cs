using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Exceptions
{
    public class InvalidIPVersionException : Exception
    {
        public InvalidIPVersionException(string msg) : base(msg) { }
    }
}
