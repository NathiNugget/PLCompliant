namespace PLCompliant.EventArguments
{
    /// <summary>
    /// This class contains the arguments for where to save a file
    /// </summary>
    public class SavedFileArgs : RaisedEventArgs
    {
        #region properties
        /// <summary>
        /// The path in which the file should be saved
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// The name of the file itself
        /// </summary>
        public string Filename { get; set; }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor for the argument
        /// </summary>
        /// <param name="path">The path for the file</param>
        /// <param name="filename">The filename for the file</param>

        public SavedFileArgs(string path, string filename)
        {
            Path = path;
            Filename = filename;
        }
        #endregion

    }
}