using PLCompliant.EventArguments;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    /// <summary>
    /// Used for showing a popup window to the end user
    /// </summary>
    public class PopupWindowEvent : UIRaisedEvent
    {
        #region constructor
        /// <summary>
        /// Constructor containing the args
        /// </summary>
        /// <param name="argument">Args for the window, type of window, content</param>
        public PopupWindowEvent(PopupWindowArgs argument) : base(argument)
        {
        }
        #endregion


        #region methods
        /// <summary>
        /// Show popup on the passed form and validate args 
        /// </summary>
        /// <param name="context">The GUI window</param>
        public override void ExecuteEvent(Form context)
        {
            var validatedItems = EventUtilities.ValidateContextAndArgs<PLCompliantUI, PopupWindowArgs, Form, RaisedEventArgs>(context, Argument);
            var args = validatedItems.Item2;
            var form = validatedItems.Item1;
            form.ShowPopup(args.Message, args.Type, MessageBoxButtons.OK);
        }
        #endregion
    }
}
