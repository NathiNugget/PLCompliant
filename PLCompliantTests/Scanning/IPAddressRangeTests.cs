using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCompliant.Exceptions;
using PLCompliant.Scanning;
using PLCompliant.Utilities;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;
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

    [TestMethod]
    [DataRow((long)((long)uint.MaxValue + 1), (long)((long)uint.MaxValue + 1))]
    [DataRow(long.MaxValue, long.MaxValue)]
    public void CreateIllegalRange(long start, long end)
    {
        Assert.ThrowsException<InvalidIPVersionException>(() => { new IPAddressRange(start, end); });
    }

    [TestMethod]
    [DataRow(0, 10)]
    [DataRow(uint.MaxValue, uint.MaxValue)]
    public void IEnumeratorTest(long start, long end)
    {
        IPAddressRange range = new IPAddressRange(start, end);
        foreach (var item in range)
        {
            Console.WriteLine(item);
        }


        Assert.IsTrue(range != null); 

    }
    [TestMethod]
    [DataRow(1, 1)]
    [DataRow(uint.MaxValue, uint.MaxValue)]
    [DataRow(0, 0)]
    public void IEnumeratorSameStartEnd(long start, long end)
    {
        IPAddressRange range = new IPAddressRange(start, end);
        int count = 0;
        foreach (var item in range)
        {
            count++;
        }
        Assert.AreEqual(1, count) ;

    }
    [TestMethod]
    [DataRow(1, 2)]
    [DataRow(uint.MaxValue - 1, uint.MaxValue)]
    [DataRow(0, 1)]
    public void IEnumeratorSameStartEndPlus1(long start, long end)
    {
        IPAddressRange range = new IPAddressRange(start, end);
        int count = 0;
        foreach (var item in range)
        {
            count++;
        }
        Assert.AreEqual(2, count);

    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("swag")]
    public void EqualsIllegalValues(object? obj)
    {
        IPAddressRange range = new(1, 10);
        Assert.IsFalse(range.Equals(obj)); 

    }

    [TestMethod]
    [DataRow(1,1)]
    [DataRow(0, 10)]
    [DataRow(0, uint.MaxValue)]


    public void EqualsIllegalValues(long otherstart, long otherend)
    {
        IPAddressRange expected = new(1, 10);
        IPAddressRange actual = new(otherstart, otherend);

        Assert.IsFalse(expected.Equals(actual));

    }


    [TestMethod]
    [DataRow(uint.MinValue, uint.MaxValue)]
    [DataRow(1, 10)]
    public void EqualsOperator(long start, long end)
    {
        IPAddressRange left = new(start, end);
        IPAddressRange right = new(start, end);
        Assert.IsTrue(left == right); 

    }

    [TestMethod]
    [DataRow(uint.MinValue, uint.MaxValue)]
    [DataRow(1, 10)]
    public void NotEqualsOperator(long start, long end)
    {
        IPAddressRange left = new(start, end);
        IPAddressRange right = new(-1, -1);
        Assert.IsTrue(left != right);

    }



    /*
     * 
     * 
     * 
     * 
     * IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this;
        }

        public static bool operator ==(IPAddressRange left, IPAddressRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IPAddressRange left, IPAddressRange right)
        {
            return !(left == right);
        }
     * 
     * 
     * 
     * */
}
