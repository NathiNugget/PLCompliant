using PLCompliant.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Events
{
    public class MainEventQueue : IEventQueue<MainThreadContext, MainThreadArgs>
    {
        ConcurrentQueue<IRaisedEvent<MainThreadContext, MainThreadArgs>> _queue = new ConcurrentQueue<IRaisedEvent<MainThreadContext, MainThreadArgs>>();

        private static MainEventQueue _instance = new MainEventQueue();
        /// <summary>
        /// Gets the global instance
        /// </summary>
        public static MainEventQueue Instance { get { return _instance; } }


        /// <inheritdoc/>
        public bool Empty { get {  return _queue.IsEmpty; } }
        /// <inheritdoc/>
        public bool Pop(out IRaisedEvent<MainThreadContext, MainThreadArgs> item)
        {
            return _queue.TryDequeue(out item);
        }
        /// <inheritdoc/>
        public void Push(IRaisedEvent<MainThreadContext, MainThreadArgs> item)
        {
            _queue.Enqueue(item);
        }
    }
}
