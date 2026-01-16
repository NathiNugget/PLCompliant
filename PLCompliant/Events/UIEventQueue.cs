using PLCompliant.EventArguments;
using PLCompliant.Interface;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace PLCompliant.Events
{
    /// <summary>
    /// This singleton handles sending events FROM the worker threads, to the UI thread.
    /// </summary>
    public class UIEventQueue : IEventQueue<Form, RaisedEventArgs>
    {
        #region fields
        /// <summary>
        /// The queue field
        /// </summary>
        ConcurrentQueue<IRaisedEvent<Form, RaisedEventArgs>> _queue;


        private static UIEventQueue _instance = new UIEventQueue();
        /// <summary>
        /// Gets the global instance
        /// </summary>
        public static UIEventQueue Instance { get { return _instance; } }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor to initialize the queue
        /// </summary>
        public UIEventQueue()
        {
            _queue = new();
        }
        #endregion

        #region methods
        /// <summary>
        /// Pushes an event with a form (context)
        /// </summary>
        /// <param name="item">Event to be added</param>
        public void Push(IRaisedEvent<Form, RaisedEventArgs> item)
        {
            _queue.Enqueue(item);
        }

        /// <summary>
        /// Tries to pop an event from the queue
        /// </summary>
        /// <param name="item">The event to be popped</param>
        /// <returns>If true, event was popped</returns>
        public bool TryPop([NotNullWhen(true)] out IRaisedEvent<Form, RaisedEventArgs> item)
        {
            return _queue.TryDequeue(out item!);
        }
        #endregion

        #region properties
        /// <summary>
        /// If the queue is empty, this is true
        /// </summary>
        public bool Empty { get { return _queue.IsEmpty; } }
        #endregion

    }
}
