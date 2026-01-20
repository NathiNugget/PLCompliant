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
    public class STEP7HeaderTests
    {
        [TestMethod()]
        [DataRow(byte.MaxValue, byte.MaxValue, ushort.MaxValue)]
        [DataRow((byte)0x54, byte.MaxValue, ushort.MinValue)]
        [DataRow(byte.MinValue, byte.MinValue, ushort.MinValue)]
        [DataRow((byte)0x1, byte.MaxValue, ushort.MaxValue)]

        public void SerializeDeserializeTest(byte protocolId, byte messageType, ushort pduReference)
        {
            STEP7Header msg = new(protocolId, messageType, pduReference);

            byte[] returnBytes = new byte[msg.Size];
            msg.Serialize(returnBytes);

            STEP7Header response = new();
            ReadOnlySpan<byte> returnBytesSpan = new ReadOnlySpan<byte>(returnBytes);
            response.Deserialize(returnBytesSpan);
            Assert.AreEqual(msg, response);
        }

        [TestMethod]
        public void STEP7HeaderSizeNoErrorFields()
        {
            STEP7Header msg = new(0,0x0, 0); // No error fields as messageType isn't 0x3
            int expectedsize = Marshal.SizeOf(msg) - 2; // - 2 because that is the length of the error fields which arent included
            Assert.AreEqual(expectedsize, msg.Size);

        }
        [TestMethod]
        public void STEP7HeaderSizeWithErrorFields()
        {
            STEP7Header msg = new(0, 0x3, 0); // With error fields as messagetype is 0x3
            int expectedsize = Marshal.SizeOf(msg);
            Assert.AreEqual(expectedsize, msg.Size);

        }
    }
}
