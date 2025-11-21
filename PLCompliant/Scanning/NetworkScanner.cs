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
        bool _abortIPScan = false;
        bool _abortPLCScan = false;
        bool _scanInProgress = false;
        ConcurrentBag<IPAddress> _viableIPs = new ConcurrentBag<IPAddress>();
        IPAddressRange _scanRange;
        public NetworkScanner(IPAddressRange scanRange)
        {
            _scanRange = scanRange;
        }
        public bool ScanInProgress { get { return _scanInProgress; } }
        public bool AbortIPScan { get { return _abortIPScan; } }
        public bool AbortPLCScan { get { return _abortPLCScan; } }

        public void Reset()
        {
            _viableIPs.Clear();
            _scanRange.Reset();
        }
        public void StopIPScan()
        {
            _abortIPScan = true;
        }
        public void StopPLCScan()
        {
            _abortPLCScan = true;
        }

        public void FindIPs()
        {
            _scanInProgress = true;
            List<Thread> threads = new List<Thread>();
            int ipspinged = 0; 
            foreach (var chunk in _scanRange.Chunk(1000)) // 1000 seems best
            {
                if(_abortIPScan)
                {
                    break;
                }
                foreach (IPAddress ip in chunk)
                {
                    threads.Add(new Thread(() =>
                    {
                        if(_abortIPScan)
                        {
                            return;
                        }
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
            _abortIPScan = false;
            _scanInProgress = false;
            

            
            

            
            File.WriteAllText("./hello.txt", _viableIPs.Count.ToString());
        }

    }
}
