using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Interface
{
    /// <summary>
    /// Interface implementing network deserialization by individually deserializing header and data segments
    /// </summary>
    public interface INetworkMessageDeserializable
    {

        // NOTE
        // It should be the responsibillity of whoever has access to the "length" field to resize the internal storage accordingly

        /// <summary>
        /// Deserialization of header segment from network to be able to extract information
        /// </summary>
        /// <param name="inputBuffer">The serialized data from the network, to be turned into </param>
        public void DeserializeHeader(ReadOnlySpan<byte> inputBuffer);

        /// <summary>
        /// Deserialization of data segment from network to be able to extract information
        /// </summary>
        /// <param name="inputBuffer">The serialized data from the network, to be turned into </param>
        public void DeserializeData(ReadOnlySpan<byte> inputBuffer);

    }
}

