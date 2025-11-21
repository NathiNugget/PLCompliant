using PLCompliant.Events;
using System.Collections.Concurrent;
using System.Globalization;
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

        public void FindIPs()
        {
            List<Thread> threads = new List<Thread>();
            int ipspinged = 0; 
            foreach (var chunk in _scanRange.Chunk(1000)) // 1000 seems best
            {
                foreach (IPAddress ip in chunk)
                {
                    threads.Add(new Thread(() =>
                    {
                        try
                        {
                            using (Ping ping = new Ping())
                            {
                                PingReply reply = ping.Send(ip, TIMEOUT);
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
                        Interlocked.Increment(ref ipspinged);
                        UIEventQueue.Instance.Push(new UIViableIPScanCompleted(new Tuple<int, int>((int)_scanRange.Count, ipspinged))); 
                    }));
                    
                }
                foreach (Thread t in threads)
                {
                    t.Start(); 
                }
                threads.ForEach(t => t.Join());
                threads.Clear(); 
            }
            

            
            

            
            File.WriteAllText("./hello.txt", _viableIPs.Count.ToString());
        }

    }
}
