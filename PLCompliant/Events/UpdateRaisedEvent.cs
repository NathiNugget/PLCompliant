using PLCompliant.EventArguments;
using PLCompliant.Interface;

namespace PLCompliant.Events
{
    /// <summary>
    /// Class used to update a raised event
    /// </summary>
    public abstract class UpdateRaisedEvent : IRaisedEvent<UpdateThreadContext, RaisedEventArgs>
    {
        RaisedEventArgs _args;
        /// <summary>
        /// Constructor to initilize the arguments
        /// </summary>
        /// <param name="args"></param>
        protected UpdateRaisedEvent(RaisedEventArgs args)
        {
            _args = args;
        }
        /// <inheritdoc/>
        public RaisedEventArgs Argument { get { return _args; } }

        /// <inheritdoc/>
        public abstract void ExecuteEvent(UpdateThreadContext context);
    }
}
