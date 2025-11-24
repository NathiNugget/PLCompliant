using PLCompliant.Interface;

namespace PLCompliant.Events
{
    /// <summary>
    /// Class used to update a raised event
    /// </summary>
    public abstract class UpdateRaisedEvent : IRaisedEvent<UpdateThreadContext, UpdateThreadArgs>
    {
        UpdateThreadArgs _args;
        /// <summary>
        /// Constructor to initilize the arguments
        /// </summary>
        /// <param name="args"></param>
        protected UpdateRaisedEvent(UpdateThreadArgs args)
        {
            _args = args;
        }
        /// <inheritdoc/>
        public UpdateThreadArgs Argument { get { return _args; } }

        /// <inheritdoc/>
        public abstract void ExecuteEvent(UpdateThreadContext context);
    }
}
