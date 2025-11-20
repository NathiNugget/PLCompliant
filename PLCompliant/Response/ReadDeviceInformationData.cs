using PLCompliant.Utilities;
using System.Text;

namespace PLCompliant.Response
{
    /// <summary>
    /// Class which contains the parsed information. It does not contain constructors, as the Factory takes care of it!!!! WOOOOOO :D
    /// </summary>
    public class ReadDeviceInformationData : ResponseData
    {
        /// <summary>
        /// Property to get the number of objects
        /// </summary>
        public byte noOfObjects { get; set; }
        /// <summary>
        /// Contains the response strings indexed by object index
        /// </summary>
        public Dictionary<int, string> Objects { get; set; } = new Dictionary<int, string>();

        /// <summary>
        /// Method to convert this class into a CSV-string based on number of items and by the order in which they are indexed. It does check if values are skipped
        /// </summary>
        /// <returns>A string to insert into the CSV output</returns>
        public override string ToCSV()
        {
            StringBuilder sb = new StringBuilder(64);
            sb.Append(IPAddr.ToString());
            sb.Append(GlobalVars.CSV_SEPARATOR);
            if (Objects.ContainsKey(0))
            {
                sb.Append(Objects[0]);
            }
            sb.Append(GlobalVars.CSV_SEPARATOR);
            if (Objects.ContainsKey(1))
            {
                sb.Append(Objects[1]);
            }
            sb.Append(GlobalVars.CSV_SEPARATOR);
            if (Objects.ContainsKey(2))
            {
                sb.Append(Objects[2]);
            }
            return sb.ToString();
        }
    }
}
