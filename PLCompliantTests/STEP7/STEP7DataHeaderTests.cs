using PLCompliant.STEP_7;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliantTests.STEP7
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class STEP7DataHeaderTests
    {

        [TestMethod()]
        [DataRow(UInt16.MaxValue)]
        [DataRow((UInt16)0x54)]
        [DataRow(UInt16.MinValue, (byte)12, byte.MaxValue)]
        [DataRow((UInt16)0x1, byte.MaxValue, byte.MinValue)]

        public void SerializeDeserializeTest(UInt16 lengthField, byte returnCode, byte transportType)
        {
            STEP7DataHeader msg = new() { Length = lengthField };

            byte[] returnBytes = new byte[msg.Size];
            msg.Serialize(returnBytes);

            STEP7DataHeader response = new();
            ReadOnlySpan<byte> returnBytesSpan = new ReadOnlySpan<byte>(returnBytes);
            response.Deserialize(returnBytesSpan);
            Assert.AreEqual(msg, response);
        }

        [TestMethod]
        public void COTPHeaderSize()
        {
            COTPHeader msg = new();
            int expectedsize = Marshal.SizeOf(msg);
            Assert.AreEqual(expectedsize, msg.Size);

        }
    }
}
