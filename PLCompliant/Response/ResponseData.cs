using System.Net;

namespace PLCompliant.Response
{
    /// <summary>
    /// Abstact class to up the base of how to parse a response object. 
    /// </summary>
    public abstract class ResponseData
    {
        /// <summary>
        /// Unused for now, but meant to be used to make up headers
        /// </summary>
        /// 
        //TODO: Find out if this should be moved to GLOBALS!
        public readonly static string[] HeaderNames =
        {
            "VendorName" ,
            "ProductCode" ,
            "FirmwareVersion"
        };
        /// <summary>
        /// The PLC from which a response was read. <br></br>It is on purpose left as uninitialized
        /// </summary>
        /// 
        public IPAddress IPAddr { get; set; }

        /// <summary>
        /// Represent the response in CSV-format
        /// </summary>
        /// <returns>A string containing all the information of the response in a CSV-format</returns>
        public abstract string ToCSV();
    }
}
