using System.Net;

namespace PLCompliant.Response
{
    public abstract class ResponseData
    {
        readonly static Dictionary<int, string> HeaderNames = new Dictionary<int, string>
        {
            {0, "VendorName" },
            {1, "ProductCode" },
            {2, "FirmwareVersion" }
        };
        public IPAddress IPAddr { get; set; }

        public abstract string ToCSV();
    }
}
