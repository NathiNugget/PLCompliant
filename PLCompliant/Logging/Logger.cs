using PLCompliant.Interface;
using System.Diagnostics;

namespace PLCompliant.Logging
{
    public class Logger : ILogger
    {
        private static Logger _instance = new Logger();



        public static Logger Instance { get { return _instance; } }
        private TraceSource _source = new("PLCompliant");

        public void SetLogLevel(SourceLevels level)
        {
            _source.Switch.Level = level;
        }
        public void AddListener(TraceListener listener)
        {
            _source.Listeners.Add(listener);
        }
        public void RemoveListener(TraceListener listener)
        {
            _source.Listeners.Remove(listener);
        }
        public void LogMessage(string message, TraceEventType type, int id)
        {
            _source.TraceEvent(type, id, message);
        }

    }
}
