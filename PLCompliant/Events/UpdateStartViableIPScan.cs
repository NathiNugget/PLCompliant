
using PLCompliant.EventArguments;
using System.Diagnostics;

namespace PLCompliant.Events
{
    /// <summary>
    /// Class used for starting IP scanning
    /// </summary>
    public class UpdateStartViableIPScan : UpdateRaisedEvent
    {
        /// <inheritdoc/>
        public UpdateStartViableIPScan(StartViableIPsScanArgs args) : base(args)
        {
        }

        /// <summary>
        /// Start scan of IPs as well as scanning PLCs
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteEvent(UpdateThreadContext context)
        {

            StartViableIPsScanArgs? args = Argument as StartViableIPsScanArgs;
            if(args == null)
            {
                Debug.Assert(false, "Event argument was not the expected runtime type ");
                return;
            }
            context.scanner.SetIPRange(args.AddressRange);
            if (context.scanner.ScanInProgress)
            {
                return;
            }
            // TODO: implement proper locking/atomic mechanism to ensure two scan threads cannot run concurrently
            Thread scanThread = new Thread(() =>
            {
                context.scanner.FindIPs(Enums.PLCProtocolType.Modbus); //TODO: Update this to use parameters instead
            });
            scanThread.Start();
        }
    }
}
