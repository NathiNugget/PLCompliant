using PLCompliant.CSV;
using PLCompliant.Enums;
using PLCompliant.EventArguments;
using PLCompliant.Interface;
using PLCompliant.Utilities;

namespace PLCompliant.Events
{
    /// <summary>
    /// Used for generation of CSV once a scan has been completed
    /// </summary>
    public class GenerateCSVEvent : UpdateRaisedEvent
    {



        /// <summary>
        /// Instantiate CSVEvent
        /// </summary>
        public GenerateCSVEvent(GenerateCSVArgs args) : base(args)
        {

        }


        /// <summary>
        /// Generate a CSV based and raise a SavedFileEvent in order to signal to UI a file has been saved on the chosen path
        /// </summary>
        /// <param name="context">Update thread</param>
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
                case PLCProtocolType.Step_7:
                    writer = new STEP7CSVWriter();
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
