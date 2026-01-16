using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Interface
{
    /// <summary>
    /// Interface implementing network deserialization
    /// </summary>
    public interface INetworkDeserializable
    {
        // NOTE
        // It should be the responsibillity of whoever has access to the "length" field to resize the internal storage accordingly

        /// <summary>
        /// Deserialization of data from network to be able to extract information
        /// </summary>
        /// <param name="inputBuffer">The serialized data from the network, to be turned into </param>
        public void Deserialize(ReadOnlySpan<byte> inputBuffer);
    }
}
