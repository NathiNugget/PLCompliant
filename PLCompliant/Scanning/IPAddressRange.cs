using PLCompliant.Exceptions;
using System.Collections;
using System.Net;

namespace PLCompliant.Scanning
{
    public struct IPAddressRange : IEnumerator<IPAddress>
    {
        long _start;
        long _end;
        long _current;
        public IPAddressRange(long start, long end)
        {
            if (start > uint.MaxValue || end > uint.MaxValue)
            {
                throw new InvalidIPVersionException("IPv6 is not supported");
            }
            if (start > end)
            {
                throw new ArgumentOutOfRangeException("Start IP cannot be greater than end IP");
            }
            _start = start;
            _end = end;
            Reset();
        }

        public long Count { get { return _end - _start + 1; } }

        public IPAddress Current { get { return new IPAddress(_current); } }

        object IEnumerator.Current { get { return new IPAddress(_current); } }

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            _current++;
            return (_current > _end);
        }

        public void Reset()
        {
            _current = _start - 1;
        }
    }
}
