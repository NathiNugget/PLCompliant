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
    [DataRow("Schneider", "123956", "V2.30", "192.168.123.100", "Siemens", "121456", "V3.3", "192.168.124.100", "Modicon", "123425", "V1.30", "192.168.123.99")]
    public void GenerateCSVStringTest(string obj1_1, string obj1_2, string obj1_3, string ip_1, string obj2_1, string obj2_2, string obj2_3, string ip_2, string obj3_1, string obj3_2, string obj3_3, string ip_3)
    {
        ConcurrentBag<ResponseData> responses = new ConcurrentBag<ResponseData>();
        var data1 = new ReadDeviceInformationData();
        data1.noOfObjects = 3;
        data1.Objects.Add(0, obj1_1);
        data1.Objects.Add(1, obj1_2);
        data1.Objects.Add(2, obj1_3);
        data1.IPAddr = IPAddress.Parse(ip_1);
        var data2 = new ReadDeviceInformationData();
        data2.noOfObjects = 3;
        data2.Objects.Add(0, obj2_1);
        data2.Objects.Add(1, obj2_2);
        data2.Objects.Add(2, obj2_3);
        data2.IPAddr = IPAddress.Parse(ip_2);
        var data3 = new ReadDeviceInformationData();
        data3.noOfObjects = 3;
        data3.Objects.Add(0, obj3_1);
        data3.Objects.Add(1, obj3_2);
        data3.Objects.Add(2, obj3_3);
        data3.IPAddr = IPAddress.Parse(ip_3);
        responses.Add(data1);
        responses.Add(data2);
        responses.Add(data3);

        ModBusCSVWriter writer = new ModBusCSVWriter();
        string csv = writer.GenerateCSVString(responses);
        StringBuilder expected = new StringBuilder();
        string headers = string.Join(GlobalVars.CSV_SEPARATOR, ModBusCSVWriter.HeaderNames);
        expected.AppendLine(headers);
        expected.AppendLine($"{ip_3};{obj3_1};{obj3_2};{obj3_3}");
        expected.AppendLine($"{ip_2};{obj2_1};{obj2_2};{obj2_3}");
        expected.AppendLine($"{ip_1};{obj1_1};{obj1_2};{obj1_3}");
        Assert.AreEqual(expected.ToString(), csv);
    }
}
