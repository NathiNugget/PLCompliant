using PLCompliant.CSV;
using PLCompliant.Enums;
using PLCompliant.EventArguments;
using PLCompliant.Interface;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    public class GenerateCSVEvent : UpdateRaisedEvent
    {
        public GenerateCSVEvent(GenerateCSVArgs args) : base(args)
        {

        }

        public override void ExecuteEvent(UpdateThreadContext context)
        {
            var validatedTypes = EventUtilities.ValidateContextAndArgs<UpdateThreadContext, GenerateCSVArgs, UpdateThreadContext, RaisedEventArgs>(context, Argument);

            ICSVWriter writer = new ModBusCSVWriter();
            string savedAs = writer.GenerateCSVFile(validatedTypes.Item2.Path, validatedTypes.Item2.Responses);

            UIEventQueue.Instance.Push(new SavedFileEvent(new SavedFileArgs(validatedTypes.Item2.Path, savedAs)));
        }
    }
}
