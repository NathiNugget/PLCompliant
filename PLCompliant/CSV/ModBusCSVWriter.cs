using PLCompliant.Interface;
using PLCompliant.Response;
using PLCompliant.Utilities;
using System.Text;

namespace PLCompliant.CSV
{
    /// <summary>
    /// CSV Writer for ModBus protocol
    /// </summary>
    public class ModBusCSVWriter : ICSVWriter
    {
        private const int EXPECTED_CHARS_PER_LINE = 80;
        public readonly static string[] HeaderNames =
        {
            "IP-Address",
            "VendorName" ,
            "ProductCode" ,
            "FirmwareVersion"
        };
        /// <inheritdoc/>
        public string GenerateCSVFile(string dirPath, string CSVText)
        {

            DateTime currentTime = DateTime.Now;
            string suffix = currentTime.ToString(GlobalVars.CustomFormat);
            string filename = string.Empty;
            filename = $"ModbusResultat{suffix}.csv";
            File.WriteAllText($"{dirPath}\\{filename}", CSVText);

            return filename;
        }
        /// <inheritdoc/>
        public string GenerateCSVString(IEnumerable<ResponseData> responses)
        {
            StringBuilder sb = new StringBuilder(EXPECTED_CHARS_PER_LINE * (responses.Count() + 1));

            string headers = string.Join(GlobalVars.CSV_SEPARATOR, HeaderNames);
            sb.AppendLine(headers);
            foreach (var data in responses)
            {
                sb.AppendLine(data.ToCSV());
            }
            return sb.ToString();
        }
    }
}
