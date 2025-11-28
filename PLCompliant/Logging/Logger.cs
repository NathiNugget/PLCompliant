using PLCompliant.Interface;
using System.Diagnostics;

namespace PLCompliant.Logging
{
    public class Logger : ILogger
    {
        public const string FILE_LOGGER_NAME = "FileLogger";
        private static Logger _instance = new Logger();


        private Logger()
        {
            _source = new("PLCompliant", SourceLevels.Information);
            _source.Listeners.Add(new ConsoleTraceListener());
            _source.Listeners.Add(new TextWriterTraceListener("./Log.txt", FILE_LOGGER_NAME));
        }

        private int NEXT_LOG_MSG_ID = 0;


        public static Logger Instance { get { return _instance; } }
        private TraceSource _source;

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
        public void RemoveListener(string name)
        {
            _source.Listeners.Remove(name);
        }
        public void LogMessage(string message, TraceEventType type)
        {
            _source.TraceEvent(type, NEXT_LOG_MSG_ID, message);
            _source.Flush();
            Interlocked.Increment(ref NEXT_LOG_MSG_ID);
        }

    }
}
