using PLCompliant.EventArguments;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    public class PopupWindowEvent : UIRaisedEvent
    {
        public PopupWindowEvent(PopupWindowArgs argument) : base(argument)
        {
        }

        public override void ExecuteEvent(Form context)
        {
            var validatedItems = EventUtilities.ValidateContextAndArgs<PLCompliantUI, PopupWindowArgs, Form, RaisedEventArgs>(context, Argument);
            var args = validatedItems.Item2;
            var form = validatedItems.Item1;
            form.ShowPopup(args.Message, args.Type, MessageBoxButtons.OK);
        }
    }
}
