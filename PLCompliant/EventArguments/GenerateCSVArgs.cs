using PLCompliant.Enums;

namespace PLCompliant.EventArguments
{
    public class GenerateCSVArgs : RaisedEventArgs
    {
        public string Path { get; private set; }
        public PLCProtocolType Protocol { get; private set; }

        public GenerateCSVArgs(string path, PLCProtocolType protocol)
        {
            Path = path;
            Protocol = protocol;
        }
    }
}
