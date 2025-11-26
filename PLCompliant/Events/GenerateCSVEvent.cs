using PLCompliant.EventArguments;
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
            GenerateCSVArgs? args = Argument as GenerateCSVArgs;
            StringBuilder sb = new StringBuilder(1000);
            if (args != null) { 
                foreach (Response in context.scanner.re)
            }
        }
    }
}
