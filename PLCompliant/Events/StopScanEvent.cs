using PLCompliant.EventArguments;

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
