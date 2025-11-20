namespace PLCompliant.Interface
{
    /// <summary>
    /// Interface for an Event Queue. 
    /// </summary>
    /// <typeparam name="T">Type of the context parameter required in each RaisedEvent item in the queue</typeparam>
    /// <typeparam name="A">Type of the event argument member in each RaisedEvent item in the queue</typeparam>
    public interface IEventQueue<T, A>
    {
        /// <summary>
        /// Returns true if the queue is empty, false if not
        /// </summary>
        public bool Empty { get; }

        /// <summary>
        /// Returns a raised event to the queue
        /// </summary>
        void Push(IRaisedEvent<T, A> item);

        /// <summary>
        /// Pops a raised event from the queue via the "out" parameter. Returns true if an element exists and was sucessfully popped from the queue
        /// </summary>

        bool Pop(out IRaisedEvent<T, A> item);

    }
}
