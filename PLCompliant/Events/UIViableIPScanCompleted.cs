using PLCompliant.EventArguments;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    /// <summary>
    /// Used when the NetworkScanner is finished scanning for viable IPs on the specified range
    /// </summary>
    public class UIViableIPScanCompleted : UIRaisedEvent
    {
        /// <summary>
        /// Constructor for the finished scan.
        /// </summary>
        /// <param name="argument">Should maybe be discarded</param>
        public UIViableIPScanCompleted(ViableIPsScanCompletedArgs argument) : base(argument) { }

        /// <summary>
        /// Execution of event
        /// </summary>
        /// <param name="context">Form containing the label in which the label should be updated to orient the user</param>
        public override void ExecuteEvent(Form context)
        {
            var validatedTypes = EventUtilities.ValidateContextAndArgs<Form1, ViableIPsScanCompletedArgs, Form, RaisedEventArgs>(context, Argument);
            Form1 form = validatedTypes.Item1;

            ViableIPsScanCompletedArgs args = validatedTypes.Item2;
            int ipsleft = args.To - args.Current;
            if (ipsleft != 0)
            {
                form.label1.Text = $"Scanner {ipsleft} IP-addresser";
            }
            else
            {
                form.label1.Text = $"Scanning er færdig";
            }



        }
    }
}
