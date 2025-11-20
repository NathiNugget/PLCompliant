using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;

namespace PLCompliant.Scanning
{
    public class NetworkScanner
    {
        const int TIMEOUT = 500;
        ConcurrentBag<IPAddress> _viableIPs = new ConcurrentBag<IPAddress>();
        IPAddressRange _scanRange;
        public NetworkScanner(IPAddressRange scanRange)
        {
            _scanRange = scanRange;
        }

        public void Reset()
        {
            _viableIPs.Clear();
            _scanRange.Reset();
        }

        public async Task FindIPsAsync()
        {
            List<Task> tasks = new List<Task>();

            foreach (IPAddress ip in _scanRange)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        using (Ping ping = new Ping())
                        {
                            PingReply reply = await ping.SendPingAsync(ip, TIMEOUT);
                            if (reply.Status == IPStatus.Success)
                            {

                                _viableIPs.Add(ip);

                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine(ip);
                    }
                }));
            }

            await Task.WhenAll(tasks);

            Console.WriteLine();
        }

    }
}
