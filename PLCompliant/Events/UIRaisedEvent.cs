using PLCompliant.Interface;

namespace PLCompliant.Events
{
    public abstract class UIRaisedEvent : IRaisedEvent<Form, EventArgs>
    {
        EventArgs _argument;
        public EventArgs Argument { get { return _argument; } }
        public abstract void ExecuteEvent(Form context);
    }
}
