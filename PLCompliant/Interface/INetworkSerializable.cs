using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Interface
{
    /// <summary>
    /// Interface implementing network serialization
    /// </summary>
    public interface INetworkSerializable
    {

        // NOTE
        // It is the callers responsibillity to pass in a suitable buffer size to be serialized into, which can be acquired with the "Size" property


        /// <summary>
        /// Serialize the contained data to a byte array ready for network transmission
        /// </summary>
        /// <param name="serializedObj">A Span of bytes for the serialized object to be written into. Use the "Size" property to determine the Span size needed for the whole object</param>
        public void Serialize(Span<byte> serializedObj);
      

        /// <summary>
        /// The size of the object in bytes. Can be used together to get buffer size required to serialize
        /// </summary>
        public int Size { get; } 
    }
}
