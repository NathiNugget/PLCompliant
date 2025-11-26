using PLCompliant.EventArguments;
using PLCompliant.Interface;

namespace PLCompliant.Events
{
    public class SavedFileEvent : UIRaisedEvent
    {
        public SavedFileEvent(SavedFileArgs argument) : base(argument)
        {

        }

        public override void ExecuteEvent(Form context)
        {

            var validatedtypes = ValidateArgs<Form1,  SavedFileArgs>(context);
            Form1 form = validatedtypes.Item1; 
            SavedFileArgs args = validatedtypes.Item2;
            form.label1.Text = $"Resultat gemt i {args.Path}, fil navngivet {args.Filename}";

        }
           
    }
}
