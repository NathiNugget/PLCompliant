using PLCompliant.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Events
{
    public abstract class UIRaisedEvent : IRaisedEvent<Form, EventArgs>
    {
        EventArgs _argument;
        public EventArgs Argument { get { return _argument; } }
        public abstract void ExecuteEvent(Form context);
    }
}
