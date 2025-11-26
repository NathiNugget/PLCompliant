using PLCompliant.Enums;
using PLCompliant.EventArguments;
using PLCompliant.Events;
using PLCompliant.Modbus;
using PLCompliant.Response;
using PLCompliant.Utilities;
using System.Collections.Concurrent;
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
        const int PINGTIMEOUT = 500;
        const int SOCKETTIMEOUT = 60000;
        bool _abortIPScan = false;
        bool _abortPLCScan = false;

        object scanMutex = new object();
        bool _scanInProgress = false;

        ConcurrentBag<IPAddress> _responsivePLCs = new ConcurrentBag<IPAddress>();
        ConcurrentBag<ResponseData> _responses = new ConcurrentBag<ResponseData>();
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
        /// <summary>
        /// Contains the responses from a scan
        /// </summary>
        public ConcurrentBag<ResponseData> Responses { get { return _responses; } private set { _responses = value; }}

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
            bool _aquiredLock = false;
            try
            {

                Monitor.TryEnter(scanMutex, ref _aquiredLock);
                if (_aquiredLock)
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
                                        PingReply reply = ping.Send(ip, PINGTIMEOUT);
                                        if (reply.Status == IPStatus.Success)
                                        {

                                            switch (protocol)
                                            {
                                                case PLCProtocolType.Modbus:
                                                    {

                                                        ReadDeviceInformationData? response = StartModbusIdentification(ip);
                                                        if (response != null)
                                                        {
                                                            response.IPAddr = ip;
                                                            _responses.Add(response);

                                                        }
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        break; //TODO: IMPLEMENT when we get to this perhaps maybe necessarily
                                                    }
                                            }






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
                }






                DateTime prefix = new DateTime();


                

                prefix = DateTime.Now;
                string filenameprefix = prefix.ToString(GlobalVars.customformat);
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
            finally
            {
                if (_aquiredLock)
                {
                    Monitor.Exit(scanMutex);
                }
                _scanInProgress = false;

            }
        }
        private ReadDeviceInformationData? StartModbusIdentification(IPAddress ip)
        {
            try
            {

                using (TcpClient client = new TcpClient(ip.ToString(), 502))
                using (NetworkStream clientStream = client.GetStream())
                {
                    clientStream.ReadTimeout = SOCKETTIMEOUT; // Written as milliseconds
                    if (client.Connected)
                    {
                        _responsivePLCs.Add(ip);
                        int identifier = 0;
                        ModBusMessageFactory factory = new ModBusMessageFactory();
                        ModBusMessage msg = factory.CreateReadDeviceInformation(new(), 0x2); //"Product ID" for some reason in the specification has implications as to how many fields are read about the device information
                        byte[] buffer = msg.Serialize();
                        clientStream.Write(buffer, 0, buffer.Length);
                        byte[] databuffer = new byte[1024]; //Default size, actual size is decided by header. 
                        int readbytes = 0;
                        byte[] headerbuffer = new byte[msg.Header.Size];
                        bool readingHeader = true;
                        ModBusMessage response = new(new ModBusHeader(), new ModBusData());

                        while (true)
                        {
                            if (readingHeader)
                            {
                                int dataleft = msg.Header.Size - readbytes;
                                int index = msg.Header.Size - dataleft;
                                readbytes += clientStream.Read(headerbuffer, index, dataleft);
                                if (readbytes == 7)
                                {
                                    response.DeserializeHeader(headerbuffer);
                                    readingHeader = false;
                                    readbytes = 0;
                                    Array.Resize(ref databuffer, response.Header.length - 1); //Minus 1 because unit id is included. Standard Modbus stuff :/
                                }

                            }
                            else
                            {

                                int dataleft = (response.Header.length - 1) - readbytes;
                                int index = (response.Header.length - 1) - dataleft;
                                readbytes += clientStream.Read(databuffer, index, dataleft);
                                if (readbytes == response.Header.length - 1)
                                {
                                    response.DeserializeData(databuffer);
                                    break;
                                }
                            }


                        }
                        bool noError = ModBusResponseParsing.TryHandleReponseError(response, out byte errCode);
                        if (!noError)
                        {
                            //TODO: Implement actual filewriter/logger 
                            string path = "./errors.log";
                            if (!File.Exists(path))
                            {
                                File.WriteAllText(path, $"Fejl ved forbindelse til Modbus PLC på IP: {client.Client.RemoteEndPoint?.ToString() ?? "IP ikke fundet"}, fejlkode: {errCode}");
                            }
                            else
                            {
                                File.AppendAllText(path, $"Fejl ved forbindelse til Modbus PLC på IP: {client.Client.RemoteEndPoint?.ToString() ?? "IP ikke fundet"}, fejlkode: {errCode}");
                            }
                            return null;
                        }
                        else
                        {

                            ReadDeviceInformationData output = ModBusResponseParsing.ParseReadDeviceInformationResponse(response, (client.Client.RemoteEndPoint as IPEndPoint)?.Address);
                            return output;

                        }
                    }
                    else return null;
                }
            }
            catch (SocketException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
        }

        public string GenerateCSV(string path, PLCProtocolType protocol)
        {
            StringBuilder sb = new StringBuilder(1000);
            DateTime currentTime = DateTime.Now;
            string suffix = currentTime.ToString(GlobalVars.customformat);
            string filename = string.Empty; 


            switch (protocol)
            {
                case PLCProtocolType.Modbus:
                    string headers = string.Join(GlobalVars.CSV_SEPARATOR, ResponseData.HeaderNames);
                    sb.AppendLine(headers); 
                    foreach (ReadDeviceInformationData data in Responses)
                    {
                        sb.AppendLine(data.ToCSV()); 
                    }
                    filename = $"ModbusResultat{suffix}.csv";
                    File.WriteAllText($"{path}\\{filename}", sb.ToString());

                    break;
                default: break;
            }

            return filename; 

        }
    }
}
