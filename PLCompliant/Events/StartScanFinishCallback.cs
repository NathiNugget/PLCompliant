using PLCompliant.EventArguments;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    public class StartScanFinishCallback : UIRaisedEvent
    {
        public StartScanFinishCallback(RaisedEventArgs argument) : base(argument)
        {

        }

        public override void ExecuteEvent(Form context)
        {
            Form1 form = EventUtilities.ValidateContext<Form1, Form>(context);
            UpdateEventQueue.Instance.Push(new GenerateCSVEvent(new GenerateCSVArgs(form.textBox1.Text, form.Protocol)));

        }
    }
}
