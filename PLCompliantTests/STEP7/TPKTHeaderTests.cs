using PLCompliant.STEP_7;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace PLCompliantTests.STEP7
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class TPKTHeaderTests
    {
        [TestMethod()]
        [DataRow(byte.MaxValue)]
        [DataRow((byte)0x54)]
        [DataRow(byte.MinValue)]
        [DataRow((byte)0x1)]

        public void SerializeDeserializeTest(byte version)
        {
            TPKTHeader msg = new(version);

            byte[] returnBytes = new byte[msg.Size];
            msg.Serialize(returnBytes);

            TPKTHeader response = new();
            ReadOnlySpan<byte> returnBytesSpan = new ReadOnlySpan<byte>(returnBytes);
            response.Deserialize(returnBytesSpan);
            Assert.AreEqual(msg, response);
        }

        [TestMethod]
        public void STEP7HeaderSize()
        {
            TPKTHeader msg = new();
            int expectedsize = Marshal.SizeOf(msg);
            Assert.AreEqual(expectedsize, msg.Size);

        }
    }
}
