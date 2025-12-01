using PLCompliant.Enums;
using PLCompliant.Response;

namespace PLCompliant.EventArguments
{
    public class StartScanFinishCallbackArgs : RaisedEventArgs
    {
        public StartScanFinishCallbackArgs(IEnumerable<ResponseData> responses, PLCProtocolType scannedWith)
        {
            Responses = responses;
            ScannedWith = scannedWith;
        }
        public IEnumerable<ResponseData> Responses { get; private set; }
        public PLCProtocolType ScannedWith { get; private set; }
    }
}
