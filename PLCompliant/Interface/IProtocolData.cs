using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Interface
{

    /// <summary>
    /// This is an interface for the data for a protocol implementing this interface
    /// </summary>
    public interface IProtocolData
    {
        /// <summary>
        /// The size for of the data in bytes
        /// </summary>
        public int Size { get; }
        /// <summary>
        /// Serialize the contained data to a byte array ready for network transmission
        /// </summary>
        /// <returns>The data serialized in bytes</returns>
        public byte[] Serialize();
        /// <summary>
        /// Deserialization of data from network to be able to extract information
        /// </summary>
        /// <param name="inputBuffer">The serialized data from the network</param>
        public void Deserialize(byte[] inputBuffer);
    }
}
