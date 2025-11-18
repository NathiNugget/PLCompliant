using PLCompliant.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Response
{
    public class ReadDeviceInformationData : ResponseData
    {
  
        public byte noOfObjects { get; set; }

        public Dictionary<int, string> Objects { get; set; } = new Dictionary<int, string>();

        public override string ToCSV()
        {
            StringBuilder sb = new StringBuilder(64);
            sb.Append(IPAddr.ToString());
            sb.Append(GlobalVars.CSV_SEPERATOR);
            if (Objects.ContainsKey(0))
            {
                sb.Append(Objects[0]);
            }
            sb.Append(GlobalVars.CSV_SEPERATOR);
            if (Objects.ContainsKey(1))
            {
                sb.Append(Objects[1]);
            }
            sb.Append(GlobalVars.CSV_SEPERATOR);
            if(Objects.ContainsKey(2))
            {
                sb.Append(Objects[2]);
            }
            return sb.ToString();
        }
    }
}
