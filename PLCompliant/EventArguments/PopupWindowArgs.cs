using PLCompliant.Enums;

namespace PLCompliant.EventArguments
{
    /// <summary>
    /// This class represents arguments for a popupwindow
    /// </summary>
    public class PopupWindowArgs : RaisedEventArgs
    {
        #region constructor
        /// <inheritdoc/>
        public PopupWindowArgs(string message, PopupWindowType type)
        {
            Message = message;
            Type = type;
        }
        #endregion

        #region properties
        /// <inheritdoc/>
        public string Message { get; set; }

        /// <inheritdoc/>
        public PopupWindowType Type { get; set; }
        #endregion




    }
}
