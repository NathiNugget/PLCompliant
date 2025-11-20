using PLCompliant.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Events
{
    /// <summary>
    /// This singleton handles sending events FROM the worker threads, to the UI thread.
    /// </summary>
    public class UIEventQueue : IEventQueue<Form, EventArgs>
    {

        ConcurrentQueue<IRaisedEvent<Form, EventArgs>> _queue;


        private static UIEventQueue _instance = new UIEventQueue();
        /// <summary>
        /// Gets the global instance
        /// </summary>
        public static UIEventQueue Instance {  get { return _instance; } }

        public UIEventQueue()
        {
            _queue = new();
        }
    
        public bool Empty { get { return _queue.IsEmpty; } }

        public void Push(IRaisedEvent<Form, EventArgs> item)
        {
            _queue.Enqueue(item);
        }

        public bool Pop( out IRaisedEvent<Form, EventArgs> item)
        {
            return _queue.TryDequeue(out item);
        }
    }
}
