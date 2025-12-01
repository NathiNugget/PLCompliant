using PLCompliant.Enums;
using PLCompliant.Response;

namespace PLCompliant.EventArguments
{
    /// <summary>
    /// Arguments for CSV generation
    /// </summary>
    public class GenerateCSVArgs : RaisedEventArgs
    {
        /// <summary>
        /// Path of where to save the file
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// Collection of the responses read from the network
        /// </summary>
        public IEnumerable<ResponseData> Responses { get; private set; }
        /// <summary>
        /// The protocol used for scanning
        /// </summary>
        public PLCProtocolType WithProtocol { get; private set; }
        /// <summary>
        /// Set the arguments
        /// </summary>
        /// <param name="path">Where to generate the CSV</param>
        /// <param name="responses">Responses read from PLCs</param>
        /// <param name="withProtocol">Protocol used for scanningn</param>
        public GenerateCSVArgs(string path, IEnumerable<ResponseData> responses, PLCProtocolType withProtocol)
        {
            Path = path;
            Responses = responses;
            WithProtocol = withProtocol;
        }
    }
}
