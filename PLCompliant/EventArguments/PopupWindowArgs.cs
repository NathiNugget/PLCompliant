using PLCompliant.Enums;

namespace PLCompliant.EventArguments
{
    public class PopupWindowArgs : RaisedEventArgs
    {
        public PopupWindowArgs(string message, PopupWindowType type)
        {
            Message = message;
            Type = type;
        }
        public string Message { get; set; }
        public PopupWindowType Type { get; set; }


    }
}
