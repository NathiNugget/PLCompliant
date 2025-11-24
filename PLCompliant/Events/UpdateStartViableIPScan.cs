namespace PLCompliant.Events
{
    /// <summary>
    /// Class used for starting IP scanning
    /// </summary>
    public class UpdateStartViableIPScan : UpdateRaisedEvent
    {
        /// <inheritdoc/>
        public UpdateStartViableIPScan(UpdateThreadArgs args) : base(args)
        {
        }

        /// <summary>
        /// Start scan of IPs as well as scanning PLCs
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteEvent(UpdateThreadContext context)
        {
            context.scanner.SetIPRange(Argument.addressRange);
            if (context.scanner.ScanInProgress)
            {
                return;
            }
            // TODO: implement proper locking/atomic mechanism to ensure two scan threads cannot run concurrently
            Thread scanThread = new Thread(() =>
            {
                context.scanner.FindIPs();
                context.scanner.FindPLCs(Enums.PLCProtocolType.Modbus); // TODO: CHANGE TO BE BASED ON ARGUMENT
            });
            scanThread.Start();
        }
    }
}
