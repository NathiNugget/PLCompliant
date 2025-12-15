namespace PLCompliant.EventArguments
{
    public class WriteWindowLogArgs : RaisedEventArgs
    {
        public WriteWindowLogArgs(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
