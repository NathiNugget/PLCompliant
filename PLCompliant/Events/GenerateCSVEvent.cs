using PLCompliant.EventArguments;
using PLCompliant.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Events
{
    public class GenerateCSVEvent : UpdateRaisedEvent
    {
        public GenerateCSVEvent(GenerateCSVArgs args) : base(args)
        {
            
        }

        public override void ExecuteEvent(UpdateThreadContext context)
        {
            var validatedTypes = EventUtilities.ValidateArgs<UpdateThreadContext, GenerateCSVArgs, UpdateThreadContext, RaisedEventArgs>(context, Argument); 
            
            GenerateCSVArgs? args = Argument as GenerateCSVArgs;
            string savedAs = context.scanner.GenerateCSV(args.Path, args.Protocol); 
            UIEventQueue.Instance.Push(new SavedFileEvent(new SavedFileArgs(args.Path, savedAs))); 
        }
    }
}
