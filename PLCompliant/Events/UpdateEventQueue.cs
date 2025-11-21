using PLCompliant.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Events
{
    public class UpdateEventQueue : IEventQueue<UpdateThreadContext, UpdateThreadArgs>
    {
        ConcurrentQueue<IRaisedEvent<UpdateThreadContext, UpdateThreadArgs>> _queue = new ConcurrentQueue<IRaisedEvent<UpdateThreadContext, UpdateThreadArgs>>();

        private static UpdateEventQueue _instance = new UpdateEventQueue();
        /// <summary>
        /// Gets the global instance
        /// </summary>
        public static UpdateEventQueue Instance { get { return _instance; } }


        /// <inheritdoc/>
        public bool Empty { get {  return _queue.IsEmpty; } }
        /// <inheritdoc/>
        public bool Pop(out IRaisedEvent<UpdateThreadContext, UpdateThreadArgs> item)
        {
            return _queue.TryDequeue(out item);
        }
        /// <inheritdoc/>
        public void Push(IRaisedEvent<UpdateThreadContext, UpdateThreadArgs> item)
        {
            _queue.Enqueue(item);
        }
    }
}
