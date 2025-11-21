using PLCompliant.Scanning;

namespace PLCompliant.Events
{
    /// <summary>
    /// Class used to provice a NetworkScanner, which is expected to be a singleton
    /// </summary>
    public class UpdateThreadContext
    {
        /// <summary>
        /// The scanner
        /// </summary>
        public NetworkScanner scanner = new NetworkScanner();

    }
}
