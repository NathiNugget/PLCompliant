using PLCompliant.Exceptions;
using PLCompliant.Uilities;
using System.Net;

namespace PLCompliant.Scanning
{
    public static class IPAddressUtilities
    {
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
            if(ipBytes.Length != 4)
            {
                throw new InvalidIPVersionException("IP Version 6 is not supported");
            } 
            var ipUint = BitConverter.ToUInt32(ipBytes, 0);
            return (long)ipUint;
        }

    }
}
