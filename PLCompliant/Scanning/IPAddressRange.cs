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
        #region fields
        long _start;
        long _end;
        long _current;
        #endregion

        #region constructors
        /// <summary>
        /// Constructor for a range. Both values specified are inclusive
        /// </summary>
        /// <param name="start">The lower IP</param>
        /// <param name="end">The upper IP</param>
        /// <exception cref="InvalidIPVersionException">Thrown if IP is outside IPv4 range</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if start is smaller than end</exception>
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

        /// <summary>
        /// This method takes IPs directly and converts to call the default constructor. <br></br>Both addresses are included in the range
        /// </summary>
        /// <param name="from">Lower IP</param>
        /// <param name="to">Upper IP</param>
        public IPAddressRange(IPAddress from, IPAddress to) : this(EndianConverter.FromNetworkToHost((uint)from.GetIPv4Addr()), EndianConverter.FromNetworkToHost((uint)to.GetIPv4Addr())) { }

        #endregion

        #region properties
        /// <summary>
        /// Gets the count of IPs in the range
        /// </summary>
        public long Count { get { return _end - _start + 1; } }

        /// <summary>
        /// Current address of the enumerator
        /// </summary>
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

        /// <summary>
        /// Gets the current address of the enumerator
        /// </summary>
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
        #endregion

        #region methods
        /// <summary>
        /// This method is *UNUSED*
        /// </summary>
        [Obsolete]
        readonly void IDisposable.Dispose()
        {

        }

        /// <summary>
        /// Gets the enumerator of the class
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<IPAddress> GetEnumerator()
        {
            return (IEnumerator<IPAddress>)this;
        }


        /// <summary>
        /// Compare two IPAdressRange instances and compare their current address and their total count
        /// </summary>
        /// <param name="obj">Object to compare, perferably an IPAdressRange instance</param>
        /// <returns>True if equal, otherwise false</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not IPAddressRange) return false;
            IPAddressRange other = (IPAddressRange)obj;
            if (other.Count == Count && other.Current.ToString() == Current.ToString()) return true;
            return false;
        }

        /// <summary>
        /// Increment instance field to keep track of how many addresses has been traversed
        /// </summary>
        /// <returns>True as long as current is not greater than end</returns>
        public bool MoveNext()
        {
            _current++;
            return (_current <= _end);
        }

        /// <summary>
        /// Start current at start -1 due to off by 1s
        /// </summary>
        public void Reset()
        {
            _current = _start - 1;
        }

        /// <summary>
        /// Get the enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this;
        }
        #endregion

        #region operator overloads

        public static bool operator ==(IPAddressRange left, IPAddressRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IPAddressRange left, IPAddressRange right)
        {
            return !(left == right);
        }
        #endregion
    }
}
