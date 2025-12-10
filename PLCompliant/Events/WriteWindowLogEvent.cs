using PLCompliant.EventArguments;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    public class WriteWindowLogEvent : UIRaisedEvent
    {
        public WriteWindowLogEvent(WriteWindowLogArgs argument) : base(argument)
        {
        }

        public override void ExecuteEvent(Form context)
        {
            var validatedItems = EventUtilities.ValidateContextAndArgs<PLCompliantUI, WriteWindowLogArgs, Form, RaisedEventArgs>(context, Argument);
            var form = validatedItems.Item1;
            var args = validatedItems.Item2;
            form.logTextBox.AppendText(args.Message);
            form.logTextBox.AppendText(Environment.NewLine);

        }
    }
}
