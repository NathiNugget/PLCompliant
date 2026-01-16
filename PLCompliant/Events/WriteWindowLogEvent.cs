using PLCompliant.EventArguments;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    /// <summary>
    /// Append text to the GUI log
    /// </summary>
    public class WriteWindowLogEvent : UIRaisedEvent
    {
        #region constructor
        /// <summary>Constructor containing text to append</summary>
        public WriteWindowLogEvent(WriteWindowLogArgs argument) : base(argument)
        {
        }
        #endregion

        #region methods
        /// <summary>
        /// Append text to the log in the specified form
        /// </summary>
        /// <param name="context">The GUI context containing the log</param>
        public override void ExecuteEvent(Form context)
        {
            var validatedItems = EventUtilities.ValidateContextAndArgs<PLCompliantUI, WriteWindowLogArgs, Form, RaisedEventArgs>(context, Argument);
            var form = validatedItems.Item1;
            var args = validatedItems.Item2;
            form.logTextBox.AppendText(args.Message);
            form.logTextBox.AppendText(Environment.NewLine);

        }
        #endregion
    }
}
