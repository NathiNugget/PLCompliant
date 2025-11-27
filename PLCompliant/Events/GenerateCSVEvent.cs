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
            var args = validatedTypes.Item2;
            ICSVWriter writer = null;
            switch (args.WithProtocol)
            {
                case PLCProtocolType.Modbus:
                    writer = new ModBusCSVWriter();
                    break;
                default:
                    break;
            }
            string csv = writer.GenerateCSVString(args.Responses);
            string savedAs = writer.GenerateCSVFile(args.Path, csv);

            UIEventQueue.Instance.Push(new SavedFileEvent(new SavedFileArgs(validatedTypes.Item2.Path, savedAs)));
        }
    }
}
