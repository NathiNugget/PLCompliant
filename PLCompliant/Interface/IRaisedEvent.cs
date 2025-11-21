namespace PLCompliant.Interface
{
    /// <summary>
    /// Interface for a raised event, for use in the IEventQueue
    /// </summary>
    /// <typeparam name="T">Type of the context parameter for when the event is executed</typeparam>
    /// <typeparam name="A">Type of the event argument</typeparam>
    public interface IRaisedEvent<T, A>
    {
        /// <summary>
        /// Event argument 
        /// </summary>
        public A Argument { get; }
        /// <summary>
        /// Executes the event with the given context parameter
        /// </summary>
        /// <param name="context"></param>
        public void ExecuteEvent(T context);

    }
}
