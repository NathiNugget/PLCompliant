
using System.Diagnostics;

namespace PLCompliant.Interface
{
    public interface ILogger
    {
        public void LogMessage(string message, TraceEventType type);
        public void RemoveListener(TraceListener listener);

        public void RemoveListener(string name);

        public void AddListener(TraceListener listener);

        public void SetLogLevel(SourceLevels level);

    }
}
