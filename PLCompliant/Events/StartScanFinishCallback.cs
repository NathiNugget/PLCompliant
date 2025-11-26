using PLCompliant.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Events
{
    public class StartScanFinishCallback : UIRaisedEvent
    {
        public StartScanFinishCallback(RaisedEventArgs argument) : base(argument)
        {

        }

        public override void ExecuteEvent(Form context)
        {
            Form1 form = context as Form1;
            UpdateEventQueue.Instance.Push(new GenerateCSVEvent(new GenerateCSVArgs(form.textBox1.Text, form.Protocol))); 
            
        }
    }
}
