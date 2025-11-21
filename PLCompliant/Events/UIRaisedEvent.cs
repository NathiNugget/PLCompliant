using PLCompliant.Interface;

namespace PLCompliant.Events
{
    public abstract class UIRaisedEvent : IRaisedEvent<Form, Tuple<int, int>>
    {
        Tuple<int, int> _argument;
        public Tuple<int, int> Argument { get { return _argument; } }
        public abstract void ExecuteEvent(Form context);
        public UIRaisedEvent(Tuple<int, int> argument)
        {
            _argument = argument;
        }
    }
}
