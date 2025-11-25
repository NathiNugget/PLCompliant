using PLCompliant.EventArguments;
using System.Diagnostics;

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
            Form1? form = context as Form1;
            if (form == null)
            {
                Debug.Assert(false, "Event failed due to context not being the expected runtime type");
                return;
            }
            ViableIPsScanCompletedArgs? args = Argument as ViableIPsScanCompletedArgs;
            if (args == null)
            {
                Debug.Assert(false, "Event failed due to eventargs not being the expected runtime type");
                return;
            }
            // TODO: Maybe change depending on threads in network scanning
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
