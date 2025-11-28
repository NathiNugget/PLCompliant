
using System.Diagnostics;

namespace PLCompliant.Interface
{
    public interface ILogger
    {
        public void LogMessage(string message, TraceEventType type, int id);
        public void RemoveListener(TraceListener listener);

        public void AddListener(TraceListener listener);

        public void SetLogLevel(SourceLevels level);

    }
}
