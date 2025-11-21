using PLCompliant.Interface;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace PLCompliant.Events
{
    /// <summary>
    /// Class used for updating the event queue. Could be for stopping a thread if a user has pushed the stop button
    /// </summary>
    public class UpdateEventQueue : IEventQueue<UpdateThreadContext, UpdateThreadArgs>
    {
        ConcurrentQueue<IRaisedEvent<UpdateThreadContext, UpdateThreadArgs>> _queue = new ConcurrentQueue<IRaisedEvent<UpdateThreadContext, UpdateThreadArgs>>();

        private static UpdateEventQueue _instance = new UpdateEventQueue();
        /// <summary>
        /// Gets the global instance
        /// </summary>
        public static UpdateEventQueue Instance { get { return _instance; } }


        /// <inheritdoc/>
        public bool Empty { get { return _queue.IsEmpty; } }
        /// <inheritdoc/>
        public bool TryPop([NotNullWhen(true)] out IRaisedEvent<UpdateThreadContext, UpdateThreadArgs> item)
        {
            return _queue.TryDequeue(out item!);
        }
        /// <inheritdoc/>
        public void Push(IRaisedEvent<UpdateThreadContext, UpdateThreadArgs> item)
        {
            _queue.Enqueue(item);
        }
    }
}
