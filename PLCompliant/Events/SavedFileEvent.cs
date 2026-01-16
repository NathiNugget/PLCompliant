using PLCompliant.EventArguments;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    /// <summary>
    /// Raise event to communicate with the user that a CSV-file has been saved
    /// </summary>
    public class SavedFileEvent : UIRaisedEvent
    {
        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="argument">Save-path and filename</param>
        public SavedFileEvent(SavedFileArgs argument) : base(argument)
        {

        }
        #endregion

        #region methods
        /// <summary>
        /// Show saved file path and name on the target form
        /// </summary>
        /// <param name="context">The GUI window</param>
        public override void ExecuteEvent(Form context)
        {
            //This is so ugly, C++ and Rust could never 
            var validatedTypes = EventUtilities.ValidateContextAndArgs<PLCompliantUI, SavedFileArgs, Form, RaisedEventArgs>(context, Argument);
            PLCompliantUI form = validatedTypes.Item1;
            SavedFileArgs args = validatedTypes.Item2;
            form.CurrentStateLabel.Text = $"Resultat gemt i {args.Path}, fil navngivet {args.Filename}";

        }
        #endregion
    }
}
