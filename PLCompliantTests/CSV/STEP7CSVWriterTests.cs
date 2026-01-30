using PLCompliant.CSV;
using PLCompliant.Response;
using PLCompliant.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliantTests.CSV
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class STEP7CSVWriterTests
    {
        [TestMethod]
        [DataRow((ushort)0x7, "6ES7 211-1BE40-0XB0 ", (ushort)0, (ushort)22020, (ushort)1281, (ushort)0x11, (ushort)0x1, "192.168.123.99")] // These vaues are from step7 messages received and logged
        public void GenerateCSVStringTest(ushort index, string orderNum, ushort moduleTypeId, ushort version, ushort pgDescFile, ushort diagnosticTypeMask, ushort szlIndex, string ip) { 
            ConcurrentBag<ResponseData> responses = new ConcurrentBag<ResponseData>();
            for(int i = 0;i<3;i++)  // create this object 3 times
            {
                var data = new ReadSZLResponseData();
                var orderNumber = new OrderNumBuffer(orderNum);
                data.Objects.Add(new ReadSZLDataItem(index, ref orderNumber, moduleTypeId, version, pgDescFile));
                data.Header = new(diagnosticTypeMask, szlIndex, (ushort)Marshal.SizeOf<ReadSZLDataItem>(), 1);
                data.IPAddr = IPAddress.Parse(ip);
                responses.Add(data);
            }

            STEP7CSVWriter writer = new();
            string csv = writer.GenerateCSVString(responses);
            StringBuilder expected = new StringBuilder();
            string headers = string.Join(GlobalVars.CSV_SEPARATOR, STEP7CSVWriter.HeaderNames);
            expected.AppendLine(headers);
            for (int i = 0; i < 3; i++) 
            {
                expected.AppendLine($"{ip};{orderNum};V4.5.1");
            }
            Assert.AreEqual(expected.ToString(), csv);

            // test file writing
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = writer.GenerateCSVFile(path, csv);

            string actual = File.ReadAllText($"{path}\\{fileName}");
            File.Delete($"{path}\\{fileName}");
            Assert.AreEqual(expected.ToString(), actual);
        }
    }
}
