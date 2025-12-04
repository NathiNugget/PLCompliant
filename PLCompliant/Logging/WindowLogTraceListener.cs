

using PLCompliant.EventArguments;
using PLCompliant.Events;
using System.Diagnostics;

namespace PLCompliant.Logging
{
    public class WindowLogTraceListener : TraceListener
    {
        public override void Write(string? message)
        {
            if(message == null)
            {
                throw new ArgumentNullException("message");
            }
            UIEventQueue.Instance.Push(new WriteWindowLogEvent(new WriteWindowLogArgs(message)));
        }

        public override void WriteLine(string? message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            UIEventQueue.Instance.Push(new WriteWindowLogEvent(new WriteWindowLogArgs(message)));
        }
    }
}
