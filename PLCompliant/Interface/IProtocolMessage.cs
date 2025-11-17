using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PLCompliant.Interface
{
    public interface IProtocolMessage
    {
        public void AddData(UInt16 inputData);

        public byte[] Serialize();
        public void DeserializeHeader(byte[] inputBuffer);
        public void DeserializeData(byte[] inputBuffer);
        public int DataSize { get; }
    }
}
