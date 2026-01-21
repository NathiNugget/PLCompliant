using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Interface
{
    public interface IProtocolTopMessage : IProtocolMessage, INetworkMessageDeserializable
    {
    }
}
