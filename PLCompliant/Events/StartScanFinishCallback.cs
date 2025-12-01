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
            // Push the callback event back to the backend event queue
            var validatedVals = EventUtilities.ValidateContextAndArgs<PLCompliantUI, StartScanFinishCallbackArgs, Form, RaisedEventArgs>(context, Argument);
            var args = validatedVals.Item2;
            UpdateEventQueue.Instance.Push(new GenerateCSVEvent(new GenerateCSVArgs(validatedVals.Item1.SavePath.Text, args.Responses, args.ScannedWith)));

        }
    }
}
