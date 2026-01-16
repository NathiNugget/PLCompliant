using PLCompliant.EventArguments;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    /// <summary>
    /// This class creates a callback for when a scan has to begin
    /// </summary>
    public class StartViableIPScanBeginCallback : UIRaisedEvent
    {
        #region constructor
        /// <inheritdoc/>
        public StartViableIPScanBeginCallback(RaisedEventArgs args) : base(args)
        {

        }
        #endregion

        #region methods
        /// <summary>
        /// Toggle the scanning-state of the GUI-window
        /// </summary>
        /// <param name="context">The form containing the GUI</param>
        public override void ExecuteEvent(Form context)
        {
            PLCompliantUI form = EventUtilities.ValidateContext<PLCompliantUI, Form>(context);
            form.NotifyScanToggle();
        }
        #endregion
    }
}
