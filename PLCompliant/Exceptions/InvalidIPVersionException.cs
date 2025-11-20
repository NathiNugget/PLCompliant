using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Exceptions
{
    /// <summary>
    /// This Exception is not expected to be used, but the case a long or IPv6 address is received, this is thrown
    /// </summary>
    public class InvalidIPVersionException : Exception
    {
        /// <summary>
        /// The Exception to be thrown
        /// </summary>
        /// <param name="msg">The response containing a message to the person raising the Exception</param>
        public InvalidIPVersionException(string msg) : base(msg) { }
    }
}
