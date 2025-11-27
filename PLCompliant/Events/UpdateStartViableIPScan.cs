
using PLCompliant.EventArguments;
using PLCompliant.Utilities;

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
            var validatedTypes = EventUtilities.ValidateContextAndArgs<UpdateThreadContext, StartViableIPsScanArgs, UpdateThreadContext, RaisedEventArgs>(context, Argument);
            StartViableIPsScanArgs? args = validatedTypes.Item2;
            context.scanner.SetIPRange(args.AddressRange);
            if (context.scanner.ScanInProgress)
            {
                return;
            }

            Thread scanThread = ThreadUtilities.CreateBackgroundThread(() =>
            {
                context.scanner.FindIPs(args.Protocol); //TODO: Update this to use parameters instead
                UIEventQueue.Instance.Push(new StartScanFinishCallback(new StartScanFinishCallbackArgs(context.scanner.Responses, args.Protocol))); //This is null on purpose, don't touch. 
            });
            scanThread.Start();
        }
    }
}
