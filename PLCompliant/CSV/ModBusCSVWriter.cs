using PLCompliant.Enums;
using PLCompliant.Interface;
using PLCompliant.Response;
using PLCompliant.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.CSV
{
    /// <summary>
    /// CSV Writer for ModBus protocol
    /// </summary>
    public class ModBusCSVWriter : ICSVWriter
    {
        private readonly static string[] HeaderNames =
        {
            "IP-Address",
            "VendorName" ,
            "ProductCode" ,
            "FirmwareVersion"
        };
        /// <inheritdoc/>
        public string GenerateCSVFile(string path, IEnumerable<ResponseData> responses)
        {
            StringBuilder sb = new StringBuilder(1000);
            DateTime currentTime = DateTime.Now;
            string suffix = currentTime.ToString(GlobalVars.CustomFormat);
            string filename = string.Empty;

            string headers = string.Join(GlobalVars.CSV_SEPARATOR, HeaderNames);
            sb.AppendLine(headers);
            foreach (ReadDeviceInformationData data in responses)
            {
                sb.AppendLine(data.ToCSV());
            }
            filename = $"ModbusResultat{suffix}.csv";
            File.WriteAllText($"{path}\\{filename}", sb.ToString());

            return filename;

        }
    }
}
