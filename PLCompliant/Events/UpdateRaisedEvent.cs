using PLCompliant.Interface;

namespace PLCompliant.Events
{
    public abstract class UpdateRaisedEvent : IRaisedEvent<UpdateThreadContext, UpdateThreadArgs>
    {
        UpdateThreadArgs _args;
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
