using PLCompliant.Interface;

namespace PLCompliant.Events
{
    public abstract class MainRaisedEvent : IRaisedEvent<MainThreadContext, MainThreadArgs>
    {
        MainThreadArgs _args;
        protected MainRaisedEvent(MainThreadArgs args)
        {
            _args = args;
        }
        /// <inheritdoc/>
        public MainThreadArgs Argument { get { return _args; } }

        /// <inheritdoc/>
        public abstract void ExecuteEvent(MainThreadContext context);
    }
}
