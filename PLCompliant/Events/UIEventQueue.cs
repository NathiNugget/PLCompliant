using PLCompliant.Interface;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace PLCompliant.Events
{
    /// <summary>
    /// This singleton handles sending events FROM the worker threads, to the UI thread.
    /// </summary>
    public class UIEventQueue : IEventQueue<Form, Tuple<int, int>>
    {

        ConcurrentQueue<IRaisedEvent<Form, Tuple<int, int>>> _queue;


        private static UIEventQueue _instance = new UIEventQueue();
        /// <summary>
        /// Gets the global instance
        /// </summary>
        public static UIEventQueue Instance { get { return _instance; } }

        /// <summary>
        /// Constructor to initialize the queue
        /// </summary>
        public UIEventQueue()
        {
            _queue = new();
        }

        /// <summary>
        /// If the queue is empty, this is true
        /// </summary>
        public bool Empty { get { return _queue.IsEmpty; } }

        /// <summary>
        /// Pushes an event with a form (context)
        /// </summary>
        /// <param name="item">Two integers. The first is the count, second is how many have been scanned</param>
        public void Push(IRaisedEvent<Form, Tuple<int, int>> item)
        {
            _queue.Enqueue(item);
        }

        /// <summary>
        /// Tries to pop an event from the queue
        /// </summary>
        /// <param name="item">Two integers. The first is the count, second is how many have been scanned</param>
        /// <returns>If true, returns an event and an integer tuple</returns>
        public bool TryPop([NotNullWhen(true)] out IRaisedEvent<Form, Tuple<int, int>> item)
        {
            return _queue.TryDequeue(out item!);
        }
    }
}
