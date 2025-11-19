using PLCompliant.Exceptions;
using PLCompliant.Uilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Scanning
{
    public static class IPAddressUtilities
    {
        public static uint GetRangeCountIPv4(IPAddress fromIp, IPAddress toIp)
        {
            byte[] fromBytes = fromIp.GetAddressBytes();
            byte[] toBytes = toIp.GetAddressBytes();
            if(fromBytes.Length != 4 || toBytes.Length != 4)
            {
                throw new InvalidIPVersionException("IP version must be 4");
            }
            uint fromAddr = EndianConverter.FromNetworkToHost( BitConverter.ToUInt32(fromBytes));
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

    }
}
