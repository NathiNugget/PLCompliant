using Accessibility;
using PLCompliant.Enums;
using PLCompliant.Response;
using PLCompliant.Scanning;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace PLCompliantTests;
[ExcludeFromCodeCoverage]
[TestClass]
public class NetworkScannerTests
{
    // These tests use the network and expect PLC's to be connected. Will likely fail otherwise
    // Expects there to be a ModBus PLC on 192.168.123.100
    [TestMethod]
    [DataRow("192.168.100.1", "192.168.130.100")]
    [DataRow("192.168.123.100", "192.168.123.100")]
    [DataRow("192.168.123.100", "192.168.123.101")]
    public void ScanIPsTestModBusPLCFound(string startIp, string endIp)
    {

        var range = new IPAddressRange(IPAddress.Parse(startIp), IPAddress.Parse(endIp));
        NetworkScanner scanner = new NetworkScanner(range);
        scanner.FindIPs(PLCProtocolType.Modbus);
        Assert.IsFalse(scanner.Responses.IsEmpty);
        foreach (var response in scanner.Responses)
        {
            ReadDeviceInformationData castedResponse = (ReadDeviceInformationData)response;
            Assert.IsTrue(castedResponse.Objects.Count == 3);
            foreach (var obj in castedResponse.Objects)
            {
                Assert.IsTrue(obj.Value.Length != 0);
            }
            Assert.IsNotNull(castedResponse.IPAddr);
        }
    }
    // Expects there to be a ModBus PLC on 192.168.123.100, and tries to avoid it
    [TestMethod]
    [DataRow("192.168.123.99", "192.168.123.99")]
    [DataRow("192.168.123.101", "192.168.123.101")]
    [DataRow("192.168.100.101", "192.168.110.101")]
    public void ScanIPsTestModBusNoneFound(string startIp, string endIp)
    {

        var range = new IPAddressRange(IPAddress.Parse(startIp), IPAddress.Parse(endIp));
        NetworkScanner scanner = new NetworkScanner(range);
        scanner.FindIPs(PLCProtocolType.Modbus);
        Assert.IsTrue(scanner.Responses.IsEmpty);
    }


    // These tests use the network and expect PLC's to be connected. Will likely fail otherwise
    // Expects there to be a STEP7 PLC on 192.168.123.99
    [TestMethod]
    [DataRow("192.168.100.1", "192.168.130.99")]
    [DataRow("192.168.123.99", "192.168.123.99")]
    [DataRow("192.168.123.99", "192.168.123.100")]
    public void ScanIPsTestSTEP7PLCFound(string startIp, string endIp)
    {

        var range = new IPAddressRange(IPAddress.Parse(startIp), IPAddress.Parse(endIp));
        NetworkScanner scanner = new NetworkScanner(range);
        scanner.FindIPs(PLCProtocolType.Step_7);
        Assert.IsFalse(scanner.Responses.IsEmpty);
        foreach (var response in scanner.Responses)
        {
            ReadSZLResponseData castedResponse = (ReadSZLResponseData)response;
            Assert.IsTrue(castedResponse.Objects.Count == 3);
            foreach (var obj in castedResponse.Objects)
            {
                // atleast one SZL object must have index 0x7 (firmware data), and order number must not have nulls
                if(obj.Index == 0x0007)
                {
                    for(int i = 0;i < OrderNumBuffer.SIZE;i++)
                    {
                        Assert.IsTrue(obj.OrderNum[i] != '\0');
                    }
                    // The test passes
                    return;

                }
            }
            Assert.Fail();
        }
    }


    // Expects there to be a STEP7 PLC on 192.168.123.99, and tries to avoid it
    [TestMethod]
    [DataRow("192.168.123.100", "192.168.123.100")]
    [DataRow("192.168.123.101", "192.168.123.101")]
    [DataRow("192.168.100.98", "192.168.110.98")]
    public void ScanIPsTestSTEP7NoneFound(string startIp, string endIp)
    {

        var range = new IPAddressRange(IPAddress.Parse(startIp), IPAddress.Parse(endIp));
        NetworkScanner scanner = new NetworkScanner(range);
        scanner.FindIPs(PLCProtocolType.Step_7);
        Assert.IsTrue(scanner.Responses.IsEmpty);
    }






}
