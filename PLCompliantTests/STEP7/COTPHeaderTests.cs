using PLCompliant.Enums;
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
    public class COTPHeaderTests
    {
        
        [TestMethod()]
        [DataRow(byte.MaxValue)]
        [DataRow((byte)0x54)]
        [DataRow(byte.MinValue)]
        [DataRow((byte)0x1)]

        public void SerializeDeserializeTest(byte lengthField)
        {
            COTPHeader msg = new();
            msg.Length = lengthField;

            byte[] returnBytes = new byte[msg.Size];
            msg.Serialize(returnBytes);

            COTPHeader response = new();
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
