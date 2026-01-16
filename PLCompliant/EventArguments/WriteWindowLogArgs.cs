namespace PLCompliant.EventArguments
{
    /// <summary>
    /// This class contains the args for writing to the log contained in the GUI
    /// </summary>
    public class WriteWindowLogArgs : RaisedEventArgs
    {
        #region constructor
        /// <summary>
        /// Constructor for the arg
        /// </summary>
        /// <param name="message">Message to be appended to the GUI log</param>
        public WriteWindowLogArgs(string message)
        {
            Message = message;
        }
        #endregion

        #region properties
        /// <summary>
        /// Message to be appended to the GUI log
        /// </summary>
        public string Message { get; set; }
        #endregion
    }
}
