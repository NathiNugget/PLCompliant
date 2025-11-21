using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Events
{
    public class UpdateStartViableIPScan : UpdateRaisedEvent
    {
        public UpdateStartViableIPScan(UpdateThreadArgs args) : base(args)
        {
        }

        public override void ExecuteEvent(UpdateThreadContext context)
        {
            if(context.scanner.ScanInProgress)
            {
                return;
            }
            Thread scanThread = new Thread(() =>
            {
                context.scanner.FindIPs();
            });
        }
    }
}
