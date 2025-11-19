using PLCompliant.Scanning;
using System.Net;

namespace PLCompliantTests;

[TestClass]
public class IPAddressUtilitiesTests
{
    [TestMethod]
    [DataRow("192.168.1.1", "192.168.1.6", (uint)6)]
    [DataRow("0.0.0.1", "255.255.255.255", uint.MaxValue)]
    [DataRow("100.243.123.214", "100.243.124.214", (uint)257)]
    [DataRow("100.243.124.1", "100.243.124.1", (uint)1)]
    public void GetRangeCountIPv4TestSucess(string from, string to, uint expectedCount)
    {
        IPAddress ip1 = IPAddress.Parse(from);
        IPAddress ip2 = IPAddress.Parse(to);
        uint actual = IPAddressUtilities.GetRangeCountIPv4(ip1 , ip2);
        Assert.AreEqual(expectedCount, actual);
    }
}
