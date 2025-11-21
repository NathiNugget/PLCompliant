using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Events
{
    public class UIViableIPScanCompleted : UIRaisedEvent
    {
        public UIViableIPScanCompleted(Tuple<int, int> argument) : base(argument)
        {

        }

        public override void ExecuteEvent(Form context)
        {
            Form1? form = context as Form1;
            if (form == null) {
                Debug.Assert(false, "Event failed due to context not being the expected runtime type"); 
                return; 
            }
            // TODO: Maybe change depending on threads in network scanning
            int to = Argument.Item1; 
            int current = Argument.Item2;
            int ipsleft = to - current;

            form.label1.Text = $"Scanner {ipsleft} IP-addresser"; 

        }
    }
}
