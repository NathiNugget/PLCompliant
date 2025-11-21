namespace PLCompliant.Events
{
    public class UpdateStartViableIPScan : UpdateRaisedEvent
    {
        public UpdateStartViableIPScan(UpdateThreadArgs args) : base(args)
        {
        }

        public override void ExecuteEvent(UpdateThreadContext context)
        {
            context.scanner.SetIPRange(Argument.addressRange);
            if (context.scanner.ScanInProgress)
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
