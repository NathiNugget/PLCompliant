using PLCompliant.EventArguments;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    public class StartScanFinishCallback : UIRaisedEvent
    {
        public StartScanFinishCallback(StartScanFinishCallbackArgs argument) : base(argument)
        {

        }

        public override void ExecuteEvent(Form context)
        {
            var validatedVals = EventUtilities.ValidateContextAndArgs<Form1, StartScanFinishCallbackArgs, Form, RaisedEventArgs>(context, Argument);
            UpdateEventQueue.Instance.Push(new GenerateCSVEvent(new GenerateCSVArgs(validatedVals.Item1.textBox1.Text, validatedVals.Item2.Responses)));

        }
    }
}
