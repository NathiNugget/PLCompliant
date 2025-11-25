namespace PLCompliant.EventArguments
{
    public class ViableIPsScanCompletedArgs : RaisedEventArgs
    {
        public int To { get; set; }
        public int Current { get; set; }

        public ViableIPsScanCompletedArgs(int to, int current)
        {
            To = to;
            Current = current;
        }
    }
}
