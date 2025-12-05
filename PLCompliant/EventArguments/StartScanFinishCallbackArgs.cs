using PLCompliant.Enums;
using PLCompliant.Response;
using System.Net;

namespace PLCompliant.EventArguments
{
    public class StartScanFinishCallbackArgs : RaisedEventArgs
    {
        public StartScanFinishCallbackArgs(IEnumerable<ResponseData> responses, PLCProtocolType scannedWith, ScanResult result, IEnumerable<IPAddress> plcs)
        {
            Responses = responses;
            ScannedWith = scannedWith;
            Result = result;
            ResponsivePLCs = plcs;
        }
        public IEnumerable<ResponseData> Responses { get; private set; }
        public IEnumerable<IPAddress> ResponsivePLCs { get; set; }
        public PLCProtocolType ScannedWith { get; private set; }
        public ScanResult Result { get; private set; }
    }
}
