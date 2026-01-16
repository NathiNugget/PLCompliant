namespace PLCompliant.EventArguments
{
    /// <summary>
    /// This class contains the args for a single valid IP-scan completed
    /// </summary>
    public class ViableIPsScanCompletedArgs : RaisedEventArgs
    {
        #region constructor
        public ViableIPsScanCompletedArgs(int to, int current)
        {
            To = to;
            Current = current;
        }
        #endregion

        #region properties
        /// <summary>
        /// The count of IPs in the range
        /// </summary>
        public int To { get; set; }

        /// <summary>
        /// The current number of scanned IPs
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// Constructor for the class
        /// </summary>
        /// <param name="to">The total count of IPs to scan</param>
        /// <param name="current">The current count of scanned IPs</param>
        #endregion
    }
}
