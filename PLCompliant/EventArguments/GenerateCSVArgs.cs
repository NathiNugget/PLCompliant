using PLCompliant.Enums;
using PLCompliant.Response;

namespace PLCompliant.EventArguments
{
    public class GenerateCSVArgs : RaisedEventArgs
    {
        public string Path { get; private set; }
        public IEnumerable<ResponseData> Responses { get; private set; }
        public PLCProtocolType WithProtocol { get; private set; }

        public GenerateCSVArgs(string path, IEnumerable<ResponseData> responses, PLCProtocolType withProtocol)
        {
            Path = path;
            Responses = responses;
            WithProtocol = withProtocol;
        }
    }
}
