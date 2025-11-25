using PLCompliant.EventArguments;
using PLCompliant.Interface;

namespace PLCompliant.Events
{
    /// <summary>
    /// The base for a event on the meant to be executed on the form context
    /// </summary>
    public abstract class UIRaisedEvent : IRaisedEvent<Form, RaisedEventArgs>
    {
        RaisedEventArgs _argument;
        /// <summary>
        /// Get the argument for the execution
        /// </summary>
        public RaisedEventArgs Argument { get { return _argument; } }
        /// <summary>
        /// Where to execute an event
        /// </summary>
        /// <param name="context"></param>
        public abstract void ExecuteEvent(Form context);
        /// <summary>
        /// Constructor to initialize the argument
        /// </summary>
        /// <param name="argument"></param>
        public UIRaisedEvent(RaisedEventArgs argument)
        {
            _argument = argument;
        }
    }
}
