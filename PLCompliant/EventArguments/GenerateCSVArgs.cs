using PLCompliant.Enums;
using PLCompliant.Response;

namespace PLCompliant.EventArguments
{
    public class GenerateCSVArgs : RaisedEventArgs
    {
        public string Path { get; private set; }
        public IEnumerable<ResponseData> Responses { get; private set; }

        public GenerateCSVArgs(string path, IEnumerable<ResponseData> responses)
        {
            Path = path;
            Responses = responses;
        }
    }
}
