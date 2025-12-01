using PLCompliant.EventArguments;
using PLCompliant.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
