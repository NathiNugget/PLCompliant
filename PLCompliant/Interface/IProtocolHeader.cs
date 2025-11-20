using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Interface
{
    /// <summary>
    /// This interface is to be used for protocols using a header before the eventual data comes. As such, this interface is very bare
    /// </summary>
    public interface IProtocolHeader
    {
        /// <summary>
        /// The size of the header in bytes
        /// </summary>
        public int Size { get; }
        /// <summary>
        /// Serialize the header to bytes. Also converts to network endianness if needed
        /// </summary>
        /// <returns>The header in bytes for network transmission</returns>
        public byte[] Serialize();
        /// <summary>
        /// Deserialization of network bytes to be a human readable header
        /// </summary>
        /// <param name="inputBuffer">The bytes from the network</param>
        public void Deserialize(byte[] inputBuffer);
    }
}
