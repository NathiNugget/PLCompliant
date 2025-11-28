using PLCompliant.EventArguments;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    public class SavedFileEvent : UIRaisedEvent
    {
        public SavedFileEvent(SavedFileArgs argument) : base(argument)
        {

        }

        public override void ExecuteEvent(Form context)
        {
            //This is so ugly, C++ and Rust could never 
            var validatedTypes = EventUtilities.ValidateContextAndArgs<PLCompliantUI, SavedFileArgs, Form, RaisedEventArgs>(context, Argument);
            PLCompliantUI form = validatedTypes.Item1;
            SavedFileArgs args = validatedTypes.Item2;
            form.CurrentStateLabel.Text = $"Resultat gemt i {args.Path}, fil navngivet {args.Filename}";

        }

    }
}
