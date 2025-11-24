using PLCompliant.Enums;
using PLCompliant.Events;
using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace PLCompliant.Scanning
{
    /// <summary>
    /// Class responsible for scanning IPs and checking if those IPs are open to the specified protocol
    /// </summary>
    public class NetworkScanner
    {
        const int TIMEOUT = 500;
        bool _abortIPScan = false;
        bool _abortPLCScan = false;
        bool _scanInProgress = false;
        ConcurrentBag<IPAddress> _viableIPs = new ConcurrentBag<IPAddress>();
        ConcurrentBag<IPAddress> _responsivePLCs = new ConcurrentBag<IPAddress>();
        IPAddressRange _scanRange;

        /// <summary>
        /// Constructor to specify the range to scan
        /// </summary>
        /// <param name="scanRange">Range of IP addresses</param>
        public NetworkScanner(IPAddressRange scanRange)
        {
            _scanRange = scanRange;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public NetworkScanner() : this(new IPAddressRange(1, 1)) { }



        #region properties
        /// <summary>
        /// Check if a Scan is already in progress
        /// </summary>
        public bool ScanInProgress { get { return _scanInProgress; } }
        /// <summary>
        /// Check if the IP scan is aborting
        /// </summary>
        public bool AbortingIPScan { get { return _abortIPScan; } }
        /// <summary>
        /// Check if the PLC scan is aborting
        /// </summary>
        public bool AbortingPLCScan { get { return _abortPLCScan; } }
        #endregion

        /// <summary>
        /// Reset ranges and data
        /// </summary>
        public void Reset()
        {
            _viableIPs.Clear();
            _scanRange.Reset();
        }

        /// <summary>
        /// Set the IP range of the scanner
        /// </summary>
        /// <param name="range">Range to scan</param>
        public void SetIPRange(IPAddressRange range)
        {
            _scanRange = range;
        }

        /// <summary>
        /// Stop the IP scanning
        /// </summary>
        public void StopIPScan()
        {
            _abortIPScan = true;
        }

        /// <summary>
        /// Stop the PLC scanning
        /// </summary>
        public void StopPLCScan()
        {
            _abortPLCScan = true;
        }

        /// <summary>
        /// Scan the IPs in the range specified
        /// </summary>
        /// 

        //TODO: Find out if it has a value for the end user for how many threads should preferably be used. First time setup/test? 
        public void FindIPs()
        {
            _scanInProgress = true;
            List<Thread> threads = new List<Thread>();
            int ipspinged = 1;
            foreach (var chunk in _scanRange.Chunk(1000)) // 1000 seems best
            {
                if (_abortIPScan)
                {
                    break;
                }
                foreach (IPAddress ip in chunk)
                {
                    threads.Add(new Thread(() =>
                    {
                        if (_abortIPScan)
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
                            //TODO: Implement logging here, maybe
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






            //TODO: Remove before release/hand in
            File.WriteAllText("./hello.txt", _viableIPs.Count.ToString());
        }

        /// <summary>
        /// Find PLCs capable of the specified protocol
        /// </summary>
        /// <param name="protocol"></param>
        /// 

        //TODO: Discuss how protocol should be into the class/method
        public void FindPLCs(PLCProtocolType protocol)
        {
            List<Thread> threads = new();
            foreach (IPAddress ip in _viableIPs)
            {
                threads.Add(new Thread(() =>
                {
                    try
                    {
                        TcpClient client;
                        switch (protocol)
                        {
                            case PLCProtocolType.Modbus:
                                {
                                    client = new TcpClient(ip.ToString(), 502); break;
                                }
                            default:
                                {
                                    client = new TcpClient(); break; //TODO: IMPLEMENT when we get to this perhaps maybe necessarily
                                }
                        }


                        if (client.Connected)
                        {
                            _responsivePLCs.Add(ip);
                        }
                        client.Close();
                    }
                    catch (SocketException ex)
                    {
                        //TODO: Implement logging here eventually
                    }
                }));


            }

            foreach (Thread t in threads)
            {
                t.Start();
            }
            foreach (Thread t in threads)
            {
                t.Join();
            }
        }



    }
}
