using PLCompliant.Interface;
using PLCompliant.Logging;
using PLCompliant.Response;
using PLCompliant.Utilities;
using System.Diagnostics;
using System.Text;

namespace PLCompliant.CSV
{
    public class STEP7CSVWriter : ICSVWriter
    {
        public string GenerateCSVFile(string dirPath, string CSVText)
        {
            DateTime currentTime = DateTime.Now;
            string suffix = currentTime.ToString(GlobalVars.CustomFormat);
            string filename = string.Empty;
            filename = $"STEP7Resultat{suffix}.csv";
            File.WriteAllText($"{dirPath}\\{filename}", CSVText);

            Logger.Instance.LogMessage($"Skrev CSV fil {filename} til sti {dirPath}", TraceEventType.Information);

            return filename;
        }
        static readonly string[] HeaderNames = {
           "IP-Address",
            "OrderNumber" ,
            "FirmwareVersion"
        };
        public string GenerateCSVString(IEnumerable<ResponseData> responses)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(GlobalVars.CSV_SEPARATOR, HeaderNames));
            foreach (ResponseData response in responses)
            {
                sb.AppendLine(response.ToCSV());
            }
            return sb.ToString();

        }
    }
}
