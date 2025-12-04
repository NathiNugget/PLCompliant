using PLCompliant.EventArguments;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    public class StartViableIPScanBeginCallback : UIRaisedEvent
    {

        public StartViableIPScanBeginCallback(RaisedEventArgs args) : base(args)
        {

        }

        public override void ExecuteEvent(Form context)
        {
            PLCompliantUI form = EventUtilities.ValidateContext<PLCompliantUI, Form>(context);
            form.NotifyScanToggle();


        }
    }
}
