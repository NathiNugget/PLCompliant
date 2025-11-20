using PLCompliant.Exceptions;
using PLCompliant.Scanning;
using System.Diagnostics.CodeAnalysis;

namespace PLCompliantTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class IPAddressRangeTests
{
    [TestMethod]
    public void IPAddressCTORSucess()
    {
        var addrRange = new IPAddressRange(0, 1234);
        Assert.IsNotNull(addrRange);
        var addrRange2 = new IPAddressRange(1234, 1234);
        Assert.IsNotNull(addrRange2);
    }
    [TestMethod]
    public void IPAddressCTORFailStartIsHigherThanEnd()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new IPAddressRange(1234, 1233));

    }
    [TestMethod]
    public void IPAddressCTORFailIPv6AddressNotSupported()
    {
        Assert.ThrowsException<InvalidIPVersionException>(() => new IPAddressRange(1234, (long)uint.MaxValue + 1));

    }
}
