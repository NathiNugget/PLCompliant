

using PLCompliant.EventArguments;
using PLCompliant.Events;
using System.Diagnostics;

namespace PLCompliant.Logging
{
    /// <summary>
    /// Custom tracelistener for events on the WindowLog
    /// </summary>
    public class WindowLogTraceListener : TraceListener
    {
        #region methods
        /// <summary>
        /// Write a message to to the WindowLog
        /// </summary>
        /// <param name="message">Message to write to the log</param>
        /// <exception cref="ArgumentNullException">Thrown if message is null</exception>
        public override void Write(string? message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            UIEventQueue.Instance.Push(new WriteWindowLogEvent(new WriteWindowLogArgs(message)));
        }

        /// <summary>
        /// Write a message and a line to the UI
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <exception cref="ArgumentNullException">Thrown if message is null</exception>
        public override void WriteLine(string? message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            UIEventQueue.Instance.Push(new WriteWindowLogEvent(new WriteWindowLogArgs(message)));
        }
        #endregion
    }
}
