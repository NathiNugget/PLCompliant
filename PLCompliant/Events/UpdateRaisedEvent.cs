using PLCompliant.EventArguments;
using PLCompliant.Interface;

namespace PLCompliant.Events
{
    /// <summary>
    /// Class used to update a raised event
    /// </summary>
    public abstract class UpdateRaisedEvent : IRaisedEvent<UpdateThreadContext, RaisedEventArgs>
    {
        #region fields
        RaisedEventArgs _args;
        #endregion

        #region methods
        /// <summary>
        /// Constructor to initilize the arguments
        /// </summary>
        /// <param name="args"></param>
        /// <inheritdoc/>
        public abstract void ExecuteEvent(UpdateThreadContext context);
        protected UpdateRaisedEvent(RaisedEventArgs args)
        {
            _args = args;
        }
        #endregion

        #region properties
        /// <inheritdoc/>
        public RaisedEventArgs Argument { get { return _args; } }
        #endregion
    }
}
