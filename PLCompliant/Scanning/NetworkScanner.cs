using PLCompliant.Enums;
using PLCompliant.EventArguments;
using PLCompliant.Events;
using PLCompliant.Modbus;
using PLCompliant.Utilities;
using System.CodeDom;
using System.Collections.Concurrent;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

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
        
        object scanMutex = new object();
        bool _scanInProgress = false;

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
            _responsivePLCs.Clear();
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
        public void FindIPs(PLCProtocolType protocol)
        {
            try
            {
                Monitor.TryEnter(scanMutex, ref _scanInProgress);
                if (_scanInProgress)
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
                            threads.Add(ThreadUtilities.CreateBackgroundThread(() =>
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
                                                client = new TcpClient("192.168.123.100", 502);


                                                //ModbusHeader header = new ModbusHeader();
                                                //header.transactionmodifier = 0;
                                                //header.protocolidentifier = 0;
                                                //header.length = 0; 
                                                //header.unitidentifier = 0;

                                                //ModbusMessage mbm = new ModbusMessage(header); 

                                                //mbm.data = 






                                                int identifier = 0;

                                                ushort address = 0;


                                                while (address < 64)
                                                {
                                                    ModBusMessageFactory factory = new ModBusMessageFactory();
                                                    ModBusMessage msg = factory.CreateReadDeviceInformation(new(), 0x2); //"Product ID" for some reason in the specification has implications as to how many fields are read about the device information










                                                    byte[] buffer = msg.Serialize();


                                                    var stream = client.GetStream();

                                                    stream.Write(buffer, 0, buffer.Length);

                                                    byte[] returnbytes = new byte[1024];
                                                    int readbytes = 0;
                                                    while (readbytes == 0)
                                                    {

                                                        readbytes = stream.Read(returnbytes);

                                                    }

                                                    Console.WriteLine(returnbytes);


                                                    ModBusMessage response = new(new ModBusHeader(), new ModBusData());
                                                    byte[] header_bytes = new byte[Marshal.SizeOf<ModBusHeader>()];
                                                    Array.Copy(returnbytes, 0, header_bytes, 0, header_bytes.Length);
                                                    response.DeserializeHeader(header_bytes);

                                                    byte[] payload_data = new byte[response.Header.length - 1];
                                                    Array.Copy(returnbytes, Marshal.SizeOf<ModBusHeader>(), payload_data, 0, payload_data.Length);





                                                    response.DeserializeData(payload_data);
                                                    var output = ModBusResponseParsing.ParseReadDeviceInformationResponse(response, System.Net.IPAddress.Parse("192.168.123.100"));
                                                    var CSV_data = output.ToCSV();
                                                    Console.WriteLine(CSV_data);







                                                    address += 64;
                                                    identifier++;
                                                    Thread.Sleep(100);
                                                }
                                            }
                                            client.Close();



                                        }
                                    }
                                }
                                catch
                                {
                                    //TODO: Implement logging here, maybe
                                    Console.WriteLine(ip);
                                }
                                Interlocked.Increment(ref ipspinged);
                                UIEventQueue.Instance.Push(new UIViableIPScanCompleted(new ViableIPsScanCompletedArgs((int)_scanRange.Count, ipspinged)));
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




                    DateTime prefix = new DateTime();


                    string customformat = "dd/MM yyyy";

                    prefix = DateTime.Now;
                    string filenameprefix = prefix.ToString(customformat);
                    string filename = $"IP-skan log - {filenameprefix}";

                    StringBuilder sb = new StringBuilder(1000);
                    foreach (IPAddress ip in _responsivePLCs)
                    {
                        sb.AppendLine(ip.ToString());
                    }
                    string output = sb.ToString();

                    //TODO: Remove before release/hand in
                    if (!File.Exists($"./{filename}.log"))
                    {
                        File.WriteAllText("./" + filename + ".log", $"IP-adresser fundet kl. {prefix.ToString("hh:mm\n")}" + output);
                    }
                    else File.AppendAllText("./" + filename + ".log", $"IP-adresser fundet kl. {prefix.ToString("hh:mm\n")}" + output);

                }
            }
            finally
            {
                if (_scanInProgress)
                {
                    Monitor.Exit(scanMutex);
                }
            }







        }
    }
}
