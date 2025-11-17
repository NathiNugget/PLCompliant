using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Interface
{
    public interface IProtocolData
    {
        public int Size { get; }
        public byte[] Serialize();
        public void Deserialize(byte[] inputBuffer);
    }
}
