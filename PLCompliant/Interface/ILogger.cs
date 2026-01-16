
using System.Diagnostics;

namespace PLCompliant.Interface
{
    /// <summary>
    /// This interface contains methods to implemented in the Logger implementation
    /// </summary>
    public interface ILogger
    {
        #region methods
        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="type">Event type to trace</param>
        public void LogMessage(string message, TraceEventType type);
        /// <summary>
        /// Remove a specified tracelistener
        /// </summary>
        /// <param name="listener">The listener to remove</param>
        public void RemoveListener(TraceListener listener);
        /// <summary>
        /// Remove a listener by name
        /// </summary>
        /// <param name="name">The name of the listener</param>
        public void RemoveListener(string name);
        /// <summary>
        /// Add a new tracelistener
        /// </summary>
        /// <param name="listener">Tracelistener instance</param>
        public void AddListener(TraceListener listener);
        /// <summary>
        /// Set at which level the logger should listen
        /// </summary>
        /// <param name="level">The specified level</param>
        public void SetLogLevel(SourceLevels level);
        #endregion
    }
}
