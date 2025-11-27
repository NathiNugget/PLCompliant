using PLCompliant.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.EventArguments
{
    public class StartScanFinishCallbackArgs : RaisedEventArgs
    {
        public StartScanFinishCallbackArgs(IEnumerable<ResponseData> responses)
        {
            Responses = responses;
        }
        public IEnumerable<ResponseData> Responses { get; private set; }
    }
}
