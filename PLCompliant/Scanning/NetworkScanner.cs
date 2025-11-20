using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

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

        public void FindIPs()
        {
            List<Task> tasks = new List<Task>();
            foreach(IPAddress ip in _scanRange)
            {
                
                    Task task = new Task(() =>
                    {
                        try
                        {
                            Ping ping = new Ping();
                            PingReply reply = ping.Send(ip, TIMEOUT);
                            if (reply.Status == IPStatus.Success)
                            {
                                _viableIPs.Add(ip);
                            }
                        }
                        catch
                        {
                            Console.WriteLine(ip);
                        }
                    });
                    tasks.Add(task);
                    task.Start();
                
              
            }
            Task.WaitAll(tasks);
        }

    }
}
