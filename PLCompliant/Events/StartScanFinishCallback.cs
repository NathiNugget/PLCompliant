using PLCompliant.Enums;
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
            var form = validatedVals.Item1;
            if (args.Result == ScanResult.LockTaken)
            {
                form.CurrentStateLabel.Text = "Starter ikke scanning pga. en scanning er allerede igang";
            }
            else if(!args.Responses.Any())
            {
                form.CurrentStateLabel.Text = $"{args.ResponsivePLCs.Count()} PLC'er fundet, men 0 returnerede brugbar data. Check log for detaljer";       
            }
            else
            {
                 
                UpdateEventQueue.Instance.Push(new GenerateCSVEvent(new GenerateCSVArgs(form.SavePath.Text, args.Responses, args.ScannedWith)));
            }
            form.NotifyScanToggle();

        }
    }
}
