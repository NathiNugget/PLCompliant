using PLCompliant.Enums;
using PLCompliant.EventArguments;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    /// <summary>
    /// Class to create a callback for when a scan either has to start or finish
    /// </summary>
    public class StartScanFinishCallback : UIRaisedEvent
    {
        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="argument">Reponses, IPs and protocol</param>
        public StartScanFinishCallback(StartScanFinishCallbackArgs argument) : base(argument)
        {

        }
        #endregion

        #region methods
        /// <summary>
        /// Validate args and communicate with the user if the control flow did not go as expected. Otherwise generate push CSV event to the backend queue
        /// </summary>
        /// <param name="context">The form to execute the event on</param>
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
            else if (!args.Responses.Any())
            {
                form.CurrentStateLabel.Text = $"{args.ResponsivePLCs.Count()} PLC'er fundet, men 0 returnerede brugbar data. Check log for detaljer";
            }
            else
            {

                UpdateEventQueue.Instance.Push(new GenerateCSVEvent(new GenerateCSVArgs(form.SavePath.Text, args.Responses, args.ScannedWith)));
            }
            form.NotifyScanToggle();

        }
        #endregion
    }
}
