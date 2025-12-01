using PLCompliant.Enums;
using PLCompliant.Response;

namespace PLCompliant.EventArguments
{
    public class StartScanFinishCallbackArgs : RaisedEventArgs
    {
        public StartScanFinishCallbackArgs(IEnumerable<ResponseData> responses, PLCProtocolType scannedWith, ScanResult result)
        {
            Responses = responses;
            ScannedWith = scannedWith;
            Result = result;
        }
        public IEnumerable<ResponseData> Responses { get; private set; }
        public PLCProtocolType ScannedWith { get; private set; }
        public ScanResult Result { get; private set; }
    }
}
