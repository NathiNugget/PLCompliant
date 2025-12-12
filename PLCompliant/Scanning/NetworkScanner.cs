using PLCompliant.Enums;
using PLCompliant.EventArguments;
using PLCompliant.Events;
using PLCompliant.Logging;
using PLCompliant.Modbus;
using PLCompliant.Response;
using PLCompliant.STEP_7;
using PLCompliant.Utilities;
using System.Collections.Concurrent;
using System.Diagnostics;
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
        const int PINGTIMEOUT = 500;
        bool _abortScan = false;

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
        public bool AbortingScan { get { return _abortScan; } }
        /// <summary>
        /// Check if the PLC scan is aborting
        /// </summary>
        /// <summary>
        /// Contains the responses from a scan
        /// </summary>
        public ConcurrentBag<ResponseData> Responses { get { return _responses; } private set { _responses = value; } }

        public ConcurrentBag<IPAddress> ResponsivePLCs { get { return _responsivePLCs; } }

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
        /// Stop the scanning
        /// </summary>
        public void StopScan()
        {
            _abortScan = true;

        }

        /// <summary>
        /// Scan the IPs in the range specified
        /// </summary>
        /// 

        //TODO: Find out if it has a value for the end user for how many threads should preferably be used. First time setup/test? 
        public ScanResult FindIPs(PLCProtocolType protocol)
        {
            bool _aquiredLock = false;
            try
            {

                Monitor.TryEnter(scanMutex, ref _aquiredLock);
                if (_aquiredLock)
                {
                    _abortScan = false;
                    _scanInProgress = true;
                    _responsivePLCs.Clear();
                    List<Thread> threads = new List<Thread>();
                    int ipspinged = 1;

                    foreach (var chunk in _scanRange.Chunk(1000)) // 1000 seems best
                    {
                        if (_abortScan)
                        {
                            return ScanResult.Aborted;
                        }
                        foreach (IPAddress ip in chunk)
                        {
                            threads.Add(ThreadUtilities.CreateBackgroundThread(() =>
                            {
                                if (_abortScan)
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
                                                    ReadDeviceInformationData? response = StartModbusIdentification(ip);
                                                    if (response != null)
                                                    {
                                                        response.IPAddr = ip;
                                                        _responses.Add(response);

                                                    }
                                                    break;
                                                case PLCProtocolType.Step_7:
                                                    ReadSZLResponseData? step7Response = StartSTEP7Identification(ip);
                                                    if (step7Response != null)
                                                    {
                                                        step7Response.IPAddr = ip;
                                                        _responses.Add(step7Response);
                                                    }
                                                    break;

                                                default:
                                                    break;

                                            }
                                        }
                                    }
                                }
                                catch (PingException) { }
                                catch (IOException) { }
                                Interlocked.Increment(ref ipspinged);
                                if (!_abortScan) // To prevent erraneous update of state label in UI. 
                                {
                                    UIEventQueue.Instance.Push(new UIViableIPScanCompleted(new ViableIPsScanCompletedArgs((int)_scanRange.Count, ipspinged)));

                                }
                            }));

                        }
                        foreach (Thread t in threads)
                        {
                            t.Start();
                        }
                        threads.ForEach(t => t.Join());
                        threads.Clear();
                        if (_abortScan)
                        {
                            return ScanResult.Aborted;
                        }
                    }
                }
                else
                {
                    Logger.Instance.LogMessage("Et scan prøvede at blive startet imens et scan allerede var i gang", TraceEventType.Warning);
                    return ScanResult.LockTaken;
                }

            }
            finally
            {
                if (_aquiredLock)
                {
                    if (_responsivePLCs.IsEmpty)
                    {
                        UIEventQueue.Instance.Push(new PopupWindowEvent(new PopupWindowArgs($"Ingen PLC Addresser fundet på {EnumToString.ProtocolType(protocol)} protokol!", PopupWindowType.WarningWindow)));
                        Logger.Instance.LogMessage($"Ingen PLC IP-Addresser fundet i scan på protocol: {EnumToString.ProtocolType(protocol)}", TraceEventType.Warning);
                    }
                    else
                    {
                        foreach (IPAddress ip in _responsivePLCs)
                        {
                            Logger.Instance.LogMessage($"PLC IP-Addresse fundet i scan: {ip.ToString()} til protocol: {EnumToString.ProtocolType(protocol)}", TraceEventType.Information);
                        }
                    }
                    _scanInProgress = false;
                    Monitor.Exit(scanMutex);
                }
            }
            return ScanResult.Finished;
        }
        private ReadDeviceInformationData? StartModbusIdentification(IPAddress ip)
        {
            try
            {

                using (TcpClient client = new TcpClient(ip.ToString(), ModBusMessage.MODBUS_TCP_PORT))
                using (NetworkStream stream = client.GetStream())
                {
                    if (client.Connected)
                    {
                        _responsivePLCs.Add(ip);
                        ModBusMessageFactory factory = new ModBusMessageFactory();
                        ModBusMessage msg = factory.CreateReadDeviceInformation(new(), 0x2); //"Product ID" for some reason in the specification has implications as to how many fields are read about the device information
                        ModBusMessage response = null;
                        // new try catch cause there isn't supposed to be a socketexception here. Log it.
                        try
                        {
                            response = ModBusMessage.SendReceive(msg, stream);
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.LogMessage($"Netværksfejl til Modbus PLC med IP-Addresse {client.Client.RemoteEndPoint?.ToString()}: {ex.Message}", TraceEventType.Error);
                            return null;
                        }
                        bool noError = ModBusResponseParsing.TryHandleReponseError(response, out byte errCode);
                        if (!noError)
                        {
                            Logger.Instance.LogMessage($"Fejl ved forbindelse til Modbus PLC på IP: {client.Client.RemoteEndPoint?.ToString() ?? "IP ikke fundet"}, fejlkode {errCode}: {EnumToString.ModBusErrorCode(errCode)}", TraceEventType.Error);
                            return null;
                        }
                        else
                        {
                            if (response.Data._functionCode == (byte)ModBusCommandType.read_device_information)
                            {
                                ReadDeviceInformationData output = ModBusResponseParsing.ParseReadDeviceInformationResponse(response, (client.Client.RemoteEndPoint as IPEndPoint)?.Address);
                                return output;
                            }
                            else
                            {
                                Logger.Instance.LogMessage($"Fejl ved forbindelse til Modbus PLC på IP: {client.Client.RemoteEndPoint?.ToString() ?? "IP ikke fundet"}, PLC returnerede et ukendt funktionskode: {response.Data._functionCode}", TraceEventType.Error);
                                return null;
                            }
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







        private ReadSZLResponseData? StartSTEP7Identification(IPAddress ip)
        {

            IsoTcpMessage COTPResponse = null;
            IsoTcpMessageFactory factory = new IsoTcpMessageFactory();
            IsoTcpMessage connectMsgOne = factory.CreateCRConnectRequestOne();
            IsoTcpMessage connectMsgTwo = factory.CreateCRConnectRequestTwo();
            TcpClient client = null;
            NetworkStream stream = null;
            bool connected = false;

            // try first message
            try
            {
                client = new TcpClient(ip.ToString(), STEP7Message.STEP7_TCP_PORT);
                stream = client.GetStream();
                COTPResponse = IsoTcpMessage.SendReceive(connectMsgOne, stream);
                connected = true;
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage($"Fejl ved 1. forbindelse til STEP7-PLC: {ex.Message} på IP: {(client.Client.RemoteEndPoint as IPEndPoint)?.Address}", TraceEventType.Warning);
                stream.Dispose();
                client.Close();
            }

            if (!connected)
            {
                // and then the 2nd
                try
                {
                    client = new TcpClient(ip.ToString(), STEP7Message.STEP7_TCP_PORT);
                    stream = client.GetStream();
                    COTPResponse = IsoTcpMessage.SendReceive(connectMsgTwo, stream);
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogMessage($"Netværksfejl ved 2. forbindelse til STEP7-PLC: {ex.Message} på IP: {(client.Client.RemoteEndPoint as IPEndPoint)?.Address}", TraceEventType.Error);
                    stream.Dispose();
                    client.Close();
                    return null;
                }
            }

            IsoTcpMessage setupCommMsg = factory.CreateSetupCommunication();
            IsoTcpMessage setupCommResponse = null;
            IsoTcpMessage ReadSZLResponse = null;
            try
            {

                setupCommResponse = IsoTcpMessage.SendReceive(setupCommMsg, stream);
                STEP7ErrorInfo err = new STEP7ErrorInfo();
                bool isError = STEP7ResponseParsing.TryHandleReponseError(setupCommResponse.STEP7, out err);
                if (isError)
                {
                    //Logger.Instance.LogMessage($"Fejl ved i svar fra Setup Communication. Fejlklasse: {err.errClass}")
                }

            }
            catch (Exception ex)
            {

                Logger.Instance.LogMessage($"Netværksfejl ved Setup Communication i forbindelse til STEP7-PLC: {ex.Message} på IP: {(client.Client.RemoteEndPoint as IPEndPoint)?.Address}", TraceEventType.Error);
                stream.Dispose();
                client.Close();
                return null;
            }
            try
            {
                IsoTcpMessage ReadSZLDataMsg = factory.CreateReadSZL();
                ReadSZLResponse = IsoTcpMessage.SendReceive(ReadSZLDataMsg, stream);
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage($"Fejl ved aflæsning af SZL data i forbindelse til STEP7-PLC: {ex.Message} på IP: {(client.Client.RemoteEndPoint as IPEndPoint)?.Address}", TraceEventType.Error);
                stream.Dispose();
                client.Close();
                return null;
            }
            return STEP7ResponseParsing.ParseReadSZLResponse(ReadSZLResponse, (client.Client.RemoteEndPoint as IPEndPoint)?.Address);
        }

    }
}

