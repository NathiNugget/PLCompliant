using PLCompliant.CSV;
using PLCompliant.Response;
using PLCompliant.Utilities;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;

namespace PLCompliantTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class ModBusCSVWriterTests
{
    [TestMethod]
    public void GenerateCSVStringTest()
    {
        ConcurrentBag<ResponseData> responses = new ConcurrentBag<ResponseData>();
        var data1 = new ReadDeviceInformationData();
        data1.noOfObjects = 3;
        data1.Objects.Add(0, "Schneider");
        data1.Objects.Add(1, "123956");
        data1.Objects.Add(2, "V2.30");
        data1.IPAddr = IPAddress.Parse("192.168.123.100");
        var data2 = new ReadDeviceInformationData();
        data2.noOfObjects = 3;
        data2.Objects.Add(0, "Siemens");
        data2.Objects.Add(1, "121456");
        data2.Objects.Add(2, "V3.3");
        data2.IPAddr = IPAddress.Parse("192.168.124.100");
        var data3 = new ReadDeviceInformationData();
        data3.noOfObjects = 3;
        data3.Objects.Add(0, "Modicon");
        data3.Objects.Add(1, "123425");
        data3.Objects.Add(2, "V1.30");
        data3.IPAddr = IPAddress.Parse("192.168.123.99");
        responses.Add(data1);
        responses.Add(data2);
        responses.Add(data3);

        ModBusCSVWriter writer = new ModBusCSVWriter();
        string csv = writer.GenerateCSVString(responses);
        StringBuilder expected = new StringBuilder();
        string headers = string.Join(GlobalVars.CSV_SEPARATOR, ModBusCSVWriter.HeaderNames);
        expected.AppendLine(headers);
        expected.AppendLine("192.168.123.99;Modicon;123425;V1.30");
        expected.AppendLine("192.168.124.100;Siemens;121456;V3.3");
        expected.AppendLine("192.168.123.100;Schneider;123956;V2.30");
        Assert.AreEqual(expected.ToString(), csv);




    }
}
