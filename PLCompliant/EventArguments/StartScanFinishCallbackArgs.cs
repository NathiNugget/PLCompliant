using PLCompliant.Enums;
using PLCompliant.Response;
using System.Net;

namespace PLCompliant.EventArguments
{
    /// <summary>
    /// This class represents an event raised when a full scan has finished and responses are to be written to CSV
    /// </summary>
    public class StartScanFinishCallbackArgs : RaisedEventArgs
    {
        #region properties
        /// <summary>
        /// Collection of meaningful response-data 
        /// </summary>
        public IEnumerable<ResponseData> Responses { get; private set; }
        /// <summary>
        /// The protcol that was scanned
        /// </summary>
        public IEnumerable<IPAddress> ResponsivePLCs { get; set; }
        /// <summary>
        /// The range of IPs which were pinged, but could not necessarily be scanned
        /// </summary>
        public PLCProtocolType ScannedWith { get; private set; }
        /// <summary>
        /// State of the lock in the NetworkScanner
        /// </summary>
        public ScanResult Result { get; private set; }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor for the instance
        /// </summary>
        /// <param name="responses">Collection of meaningful response-data</param>
        /// <param name="scannedWith">The protcol that was scanned</param>
        /// <param name="result">State of the lock in the NetworkScanner</param>
        /// <param name="plcs">The range of IPs which were pinged, but could not necessarily be scanned</param>
        public StartScanFinishCallbackArgs(IEnumerable<ResponseData> responses, PLCProtocolType scannedWith, ScanResult result, IEnumerable<IPAddress> plcs)
        {
            Responses = responses;
            ScannedWith = scannedWith;
            Result = result;
            ResponsivePLCs = plcs;
        }
        #endregion


    }
}
