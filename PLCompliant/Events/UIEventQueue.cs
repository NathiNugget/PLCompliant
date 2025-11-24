using PLCompliant.Interface;
using System.Collections.Concurrent;

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

        public UIEventQueue()
        {
            _queue = new();
        }

        public bool Empty { get { return _queue.IsEmpty; } }

        public void Push(IRaisedEvent<Form, Tuple<int, int>> item)
        {
            _queue.Enqueue(item);
        }

        public bool Pop(out IRaisedEvent<Form, Tuple<int, int>> item)
        {
            return _queue.TryDequeue(out item);
        }
    }
}
