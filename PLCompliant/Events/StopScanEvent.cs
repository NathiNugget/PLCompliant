using PLCompliant.EventArguments;
using PLCompliant.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Events
{
    public class StopScanEvent : UpdateRaisedEvent
    {
        public StopScanEvent(RaisedEventArgs args) : base(args) { }

        public override void ExecuteEvent(UpdateThreadContext context)
        {
            context.scanner.StopScan(); 
        }
    }
}
