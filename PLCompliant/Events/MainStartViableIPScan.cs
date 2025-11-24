using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Events
{
    public class MainStartViableIPScan : MainRaisedEvent
    {
        public MainStartViableIPScan(MainThreadArgs args) : base(args)
        {
        }

        public override void ExecuteEvent(MainThreadContext context)
        {
            context.scanner.SetIPRange(Argument.addressRange);
            if(context.scanner.ScanInProgress)
            {
                return;
            }
            Thread scanThread = new Thread(() =>
            {
                context.scanner.FindIPs();
                context.scanner.FindPLCs(Enums.PLCProtocolType.Modbus); // TODO: CHANGE TO BE BASED ON ARGUMENT
            });
            scanThread.Start();
        }
    }
}
