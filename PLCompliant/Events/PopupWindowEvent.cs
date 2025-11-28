using PLCompliant.Enums;
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
            var validatedItems = EventUtilities.ValidateContextAndArgs<Form1, PopupWindowArgs, Form, RaisedEventArgs>(context, Argument);
            var args = validatedItems.Item2;
            var form = validatedItems.Item1;
            switch(args.Type)
            {
                case PopupWindowType.ErrorWindow:
                    MessageBox.Show(args.Message, "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case PopupWindowType.WarningWindow:
                    MessageBox.Show(args.Message, "Advarsel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case PopupWindowType.InformationWindow:
                    MessageBox.Show(args.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                default:
                    MessageBox.Show(args.Message, "Ukendt Popup Type", MessageBoxButtons.OK, MessageBoxIcon.None);
                    break;

            }
        }
    }
}
