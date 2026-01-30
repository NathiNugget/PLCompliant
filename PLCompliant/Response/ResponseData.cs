using System.Net;

namespace PLCompliant.Response
{
    /// <summary>
    /// Abstact class to up the base of how to parse a response object. 
    /// </summary>
    public abstract class ResponseData
    {
        #region fields
        /// <summary>
        /// Unused for now, but meant to be used to make up headers
        /// </summary>
        /// 

        public readonly static string[] HeaderNames =
        {
            "IP-Address", // TODO: Replace with modbus_csv_ip_address_header
            "VendorName" , //TODO: Replace with modbus_csv_vendorname_header
            "ProductCode" , //TODO: Replace with modbus_csv_productcode_header
            "FirmwareVersion" //TODO: Replace with modbus_csv_firmwareversion_header
        };

        #endregion

        #region properties
        /// <summary>
        /// The PLC from which a response was read. <br></br>It is on purpose left as uninitialized
        /// </summary>
        /// 
        public IPAddress IPAddr { get; set; }
        #endregion

        #region methods
        /// <summary>
        /// Represent the response in CSV-format
        /// </summary>
        /// <returns>A string containing all the information of the response in a CSV-format</returns>
        public abstract string ToCSV();
        #endregion
    }
}
