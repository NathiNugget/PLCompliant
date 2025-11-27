using PLCompliant.Exceptions;
using PLCompliant.Utilities;
using System.Collections;
using System.Net;

namespace PLCompliant.Scanning
{
    /// <summary>
    /// This class is used for mapping out the beginning and end IP addresses to look for in a scan
    /// </summary>
    public struct IPAddressRange : IEnumerator<IPAddress>, IEnumerable<IPAddress>
    {
        long _start;
        long _end;
        long _current;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <exception cref="InvalidIPVersionException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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


        public IPAddressRange(IPAddress from, IPAddress to) : this(EndianConverter.FromNetworkToHost((uint)from.GetIPv4Addr()), EndianConverter.FromNetworkToHost((uint)to.GetIPv4Addr())) { }

        public long Count { get { return _end - _start + 1; } }

        public IPAddress Current
        {
            get
            {
                if (_current < 0)
                {
                    return new IPAddress(EndianConverter.FromHostToNetwork((uint)(_current + 1)));
                }
                else
                {
                    return new IPAddress(EndianConverter.FromHostToNetwork((uint)_current));
                }
            }
        }

        object IEnumerator.Current
        {
            get
            {
                if (_current < 0)
                {
                    return new IPAddress(EndianConverter.FromHostToNetwork((uint)(_current + 1)));
                }
                else
                {
                    return new IPAddress(EndianConverter.FromHostToNetwork((uint)_current));
                }
            }
        }

        public void Dispose()
        {

        }

        public IEnumerator<IPAddress> GetEnumerator()
        {
            return (IEnumerator<IPAddress>)this;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not IPAddressRange) return false;
            IPAddressRange other = (IPAddressRange)obj;
            if (other.Count == Count && other.Current.ToString() == Current.ToString()) return true;
            return false;
        }

        public bool MoveNext()
        {
            _current++;
            return (_current < _end);
        }

        public void Reset()
        {
            _current = _start - 1;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this;
        }

        public static bool operator ==(IPAddressRange left, IPAddressRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IPAddressRange left, IPAddressRange right)
        {
            return !(left == right);
        }
    }
}
