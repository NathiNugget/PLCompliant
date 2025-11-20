using PLCompliant.Modbus;
using System.Diagnostics.CodeAnalysis;

namespace PLCompliantTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class ModBusHeaderTests
{

    [TestMethod]
    [DataRow(ushort.MinValue, ushort.MinValue, byte.MinValue)]
    [DataRow(ushort.MaxValue, ushort.MaxValue, byte.MaxValue)]
    [DataRow((ushort)(ushort.MaxValue / 2), (ushort)(ushort.MaxValue / 2), (byte)(byte.MaxValue / 2))]

    public void ModBusHeaderAreEqual(ushort transmodifier, ushort protidentifier, byte unitid)
    {
        ModBusHeader first = new ModBusHeader(transmodifier, protidentifier, unitid);
        ModBusHeader other = new ModBusHeader(transmodifier, protidentifier, unitid);
        Assert.AreEqual(first, other);

    }

    [TestMethod]
    public void ModBudHeaderAreNotEqual()
    {
        ModBusHeader header = new ModBusHeader(0, 0, 0);
        Assert.IsFalse(header.Equals(null));

    }

    [TestMethod]
    public void ModBusHeaderSizeShouldBeSeven()
    {
        ModBusHeader header = new ModBusHeader(0, 0, 0);
        int expected = 7; //Because the struct should be 7 bytes according to spec
        Assert.AreEqual(expected, header.Size);
    }



}
