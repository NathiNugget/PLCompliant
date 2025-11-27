using PLCompliant.Exceptions;
using PLCompliant.Scanning;
using PLCompliant.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Net;

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
    [DataRow("0.0.0.1", "0.0.0.0")]
    [DataRow("255.255.255.255", "0.0.0.0")]
    public void IPAddressWithAddressCTORFailStartIsHigherThanEnd(string start, string end)
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new IPAddressRange(IPAddress.Parse(start), IPAddress.Parse(end)));
    }

    [TestMethod]
    public void IPAddressCTORFailIPv6AddressNotSupported()
    {
        Assert.ThrowsException<InvalidIPVersionException>(() => new IPAddressRange(1234, (long)uint.MaxValue + 1));
    }

    [TestMethod]
    [DataRow("192.168.1.1", "192.168.1.2")]
    [DataRow("0.0.0.0", "255.255.255.255")]
    [DataRow("1.1.1.1", "100.100.100.100")]

    public void CreateRangeUsingIPAddresses(string add1, string add2)
    {
        IPAddress address1 = IPAddress.Parse(add1);
        IPAddress address2 = IPAddress.Parse(add2);
        long addressaslong1 = address1.GetIPv4Addr();
        long addressaslong2 = address2.GetIPv4Addr();

        IPAddressRange expected = new IPAddressRange(EndianConverter.FromNetworkToHost((uint)addressaslong1), EndianConverter.FromNetworkToHost((uint)addressaslong2));

        IPAddressRange actual = new IPAddressRange(address1, address2);
        Assert.IsTrue(expected.Equals(actual));


    }
}
