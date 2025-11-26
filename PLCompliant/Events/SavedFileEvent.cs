using PLCompliant.EventArguments;
using PLCompliant.Interface;
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
            var validatedTypes = EventUtilities.ValidateArgs<Form1, SavedFileArgs, Form, RaisedEventArgs>(context, Argument); 
            Form1 form = validatedTypes.Item1; 
            SavedFileArgs args = validatedTypes.Item2;
            form.label1.Text = $"Resultat gemt i {args.Path}, fil navngivet {args.Filename}";

        }
           
    }
}
