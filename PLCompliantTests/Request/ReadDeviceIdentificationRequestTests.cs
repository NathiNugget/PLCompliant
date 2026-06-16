using PLCompliant.Modbus;
using PLCompliant.RequestModels;
using System.Diagnostics.CodeAnalysis;

namespace PLCompliantTests.Request
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class ReadDeviceIdentificationRequestTests
    {
        [DataRow(ushort.MinValue, byte.MaxValue, byte.MinValue, byte.MinValue, byte.MaxValue)]
        [DataRow(ushort.MinValue, byte.MaxValue, byte.MinValue, byte.MaxValue, byte.MinValue)]
        [DataRow(ushort.MaxValue, byte.MinValue, byte.MaxValue, (byte)52, (byte)95)]
        [DataTestMethod]
        [TestMethod]
        public void ConvertTest(ushort transactionIdent, byte functionCode, byte subfunctionCode, byte productId, byte objectIdent)
        {
            ReadDeviceIdentificationRequest request = new ReadDeviceIdentificationRequest() { FunctionCode = functionCode, SubfunctionCode = subfunctionCode, ProductId = productId, ObjectIdentifier = objectIdent };
            ModBusMessage expected = new ModBusMessage(new(transactionIdent, 0, 0xff), new());
            expected.AddData(request);

            ModBusMessage actual = request.Convert(transactionIdent);
            Assert.AreEqual(expected, actual);
        }
    }
}
