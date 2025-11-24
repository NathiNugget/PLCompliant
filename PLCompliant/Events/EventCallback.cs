using PLCompliant.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Events
{
    public class UIEventCallback<A> : IRaisedEvent<Form, Tuple<int, int>>
    {
        private Action<Tuple<int, int>> _callback;
        public Tuple<int, int> Argument { get {  return Argument; } }

        public void ExecuteEvent(Form context)
        {
            _callback?.Invoke(Argument);
        }
    }
}
