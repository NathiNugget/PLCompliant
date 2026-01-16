using PLCompliant.EventArguments;

namespace PLCompliant.Events
{
    /// <summary>
    /// Stop scan-event either due to something going wrong or user-input
    /// </summary>
    public class StopScanEvent : UpdateRaisedEvent
    {
        #region constrcutor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="args">Void args, not used</param>
        public StopScanEvent(RaisedEventArgs args) : base(args) { }
        #endregion

        #region methods
        /// <summary>
        /// Stop the scan on the NetworkScanner instance contained in the context
        /// </summary>
        /// <param name="context">Backend thread</param>
        public override void ExecuteEvent(UpdateThreadContext context)
        {
            context.scanner.StopScan();
        }
        #endregion
    }
}
