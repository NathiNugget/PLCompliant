using PLCompliant.Exceptions;
using PLCompliant.Utilities;
using System.Net;

namespace PLCompliant.Scanning
{
    /// <summary>
    /// Contains utilities related to IPAddresses
    /// </summary>
    public static class IPAddressUtilities
    {
        /// <summary>
        /// Gets the count of how many IPs are between (both inclusive) from and to IP addresses
        /// </summary>
        /// <param name="fromIp">From IP</param>
        /// <param name="toIp">To IP</param>
        /// <returns>Count of the range</returns>
        /// <exception cref="InvalidIPVersionException">Thrown if an IPv6 address is somehow passed</exception>
        public static uint GetRangeCountIPv4(IPAddress fromIp, IPAddress toIp)
        {
            byte[] fromBytes = fromIp.GetAddressBytes();
            byte[] toBytes = toIp.GetAddressBytes();
            if (fromBytes.Length != 4 || toBytes.Length != 4)
            {
                throw new InvalidIPVersionException("IP version must be 4");
            }
            uint fromAddr = EndianConverter.FromNetworkToHost(BitConverter.ToUInt32(fromBytes));
            uint toAddr = EndianConverter.FromNetworkToHost(BitConverter.ToUInt32(toBytes));
            return (uint)new IPAddressRange(fromAddr, toAddr).Count;

        }

        /// <summary>
        /// Get an IPAdressRange based on form and to IPs (both inclusive)
        /// </summary>
        /// <param name="fromIp">From IP</param>
        /// <param name="toIp">To IP</param>
        /// <returns>Range of IP addresses</returns>
        /// <exception cref="InvalidIPVersionException">Thrown if an IPv6 address is somehow passed</exception>
        public static IPAddressRange GetRangeIPsIPv4(IPAddress fromIp, IPAddress toIp)
        {
            byte[] fromBytes = fromIp.GetAddressBytes();
            byte[] toBytes = toIp.GetAddressBytes();
            if (fromBytes.Length != 4 || toBytes.Length != 4)
            {
                throw new InvalidIPVersionException("IP version must be 4");
            }
            uint fromAddr = EndianConverter.FromNetworkToHost(BitConverter.ToUInt32(fromBytes));
            uint toAddr = EndianConverter.FromNetworkToHost(BitConverter.ToUInt32(toBytes));
            return new IPAddressRange(fromAddr, toAddr);

        }
        /// <summary>
        /// Gets the raw IPv4 address, in network order
        /// </summary>
        /// <param name="ip">The IP to get the raw address from</param>
        /// <returns></returns>
        public static long GetIPv4Addr(this IPAddress ip)
        {
            var ipBytes = ip.GetAddressBytes();
            if (ipBytes.Length != 4)
            {
                throw new InvalidIPVersionException("IP Version 6 is not supported");
            }
            var ipUint = BitConverter.ToUInt32(ipBytes, 0);
            return (long)ipUint;
        }

        /// <summary>
        /// Overload to check IP-address in host order
        /// </summary>
        /// <param name="ip">An IPv4 address</param>
        /// <returns></returns>
        public static long GetIPv4AddrHost(this IPAddress ip)
        {
            return EndianConverter.FromNetworkToHost((uint)ip.GetIPv4Addr());
        }

    }
}
