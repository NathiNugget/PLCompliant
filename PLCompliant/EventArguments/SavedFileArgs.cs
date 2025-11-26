namespace PLCompliant.EventArguments
{
    public class SavedFileArgs : RaisedEventArgs
    {
        public string Path { get; set; }
        public string Filename { get; set; }

        public SavedFileArgs(string path, string filename)
        {
            Path = path;
            Filename = filename;
        }
    }
}