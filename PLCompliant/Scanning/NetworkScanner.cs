using PLCompliant.Enums;
using PLCompliant.EventArguments;
using PLCompliant.Events;
using PLCompliant.Interface;
using PLCompliant.Logging;
using PLCompliant.Modbus;
using PLCompliant.Response;
using PLCompliant.STEP_7;
using PLCompliant.Utilities;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace PLCompliant.Scanning
{
    public class SocketAndIP
    {
        public Socket Socket { get; set; }
        public IPAddress Address { get; set; }
    }
    public class SocketReadWriteWrapper
    {
        public SocketReadWriteWrapper(Socket socket, IPAddress ip, IProtocolTopMessage messageBuffer)
        {
            Socket = socket;
            IP = ip;
            ReceiveMessage = messageBuffer;
        }

        public Socket Socket { get; set; }
        public bool ShouldSend { get; set; } = true;
        public bool ShouldReceive { get; set; } = false;
        public int CurrentMsgBytesSend { get; set; } = 0;
        public int CurrentMsgBytesReceived { get; set; } = 0;
        public bool ReceivingHeader { get; set; } = true;
        public IProtocolTopMessage ReceiveMessage { get; set; }
        public byte[] ReceiveBuffer { get; set; } = new byte[16384];
        public byte[] SendBuffer { get; set; } = new byte[16384];
        public IPAddress IP { get; set; }

    }
    /// <summary>
    /// Class responsible for scanning IPs and checking if those IPs are open to the specified protocol
    /// </summary>
    public class NetworkScanner
    {
        #region fields
        const int PINGTIMEOUT = 500;
        bool _abortScan = false;

        object scanMutex = new object();
        bool _scanInProgress = false;

        ConcurrentBag<IPAddress> _responsivePLCs = new ConcurrentBag<IPAddress>();
        ConcurrentBag<ResponseData> _responses = new ConcurrentBag<ResponseData>();
        IPAddressRange _scanRange;
        #endregion

        #region constructors
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
        #endregion

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

        #region methods
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
                    _responses.Clear();
                    List<Thread> threads = new List<Thread>();
                    int ipspinged = 1;

                    List<SocketAndIP> connectingSockets = new();
                    List<SocketReadWriteWrapper> connectedSockets = new();


                    foreach (IPAddress ip in _scanRange)
                    {
                        Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                        socket.Blocking = false;
                        try
                        {
                            socket.Connect(ip, 502);
                        }
                        catch (SocketException e)
                        {
                            // 10035 == WSAEWOULDBLOCK
                            if (e.NativeErrorCode.Equals(10035))
                            {
                                connectingSockets.Add(new() { Socket = socket, Address = ip });
                            }
                            continue;
                        }
                        connectingSockets.Add(new() { Socket = socket, Address = ip });
                    }
                    DateTime connectionTimeout = DateTime.Now + TimeSpan.FromSeconds(2);
                    while (true)
                    {
                        DateTime timeOut = DateTime.Now;
                        // Timeout of 2 seconds to connect
                        if (connectionTimeout > DateTime.Now)
                        {
                            for (int i = 0; i < connectingSockets.Count; i++)
                            {
                                if (connectingSockets[i] is null)
                                {
                                    continue;
                                }
                                bool connected = connectingSockets[i].Socket.Poll(1, SelectMode.SelectWrite);
                                if (connected)
                                {
                                    connectedSockets.Add(new(connectingSockets[i].Socket, connectingSockets[i].Address, new ModBusMessage()));
                                    connectingSockets[i] = null;
                                }
                            }
                        }
                        // start doing stuff with connected sockets    
                        for (int i = 0; i < connectedSockets.Count; i++)
                        {
                            var output = StartModbusIdentification(connectedSockets[i]);
                            if(output is not null)
                            {
                                _responses.Add(output);
                            }
                                
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
        private ReadDeviceInformationData? StartModbusIdentification(SocketReadWriteWrapper socket)
        {
            try
            {



                ModBusMessageFactory factory = new ModBusMessageFactory();
                ModBusMessage msg = factory.CreateReadDeviceInformation(new(), 0x2); //"Product ID" for some reason in the specification has implications as to how many fields are read about the device information
                ReadDeviceInformationData? output = null;
                // new try catch cause there isn't supposed to be a socketexception here. Log it.
                try
                {
                    ModBusMessage.SendReceive(msg, socket, (response) =>
                    {
                        bool noError = ModBusResponseParsing.TryHandleReponseError(response, out byte errCode);
                        if (!noError)
                        {
                            Logger.Instance.LogMessage($"Fejl ved forbindelse til Modbus PLC på IP: {socket.IP}, fejlkode {errCode}: {EnumToString.ModBusErrorCode(errCode)}", TraceEventType.Error);
                            return;
                        }
                        else
                        {
                            if (response.Data.FunctionCode == (byte)ModBusCommandType.read_device_information)
                            {
                                output = ModBusResponseParsing.ParseReadDeviceInformationResponse(response, socket.IP);
                                return;
                            }
                            else
                            {
                                Logger.Instance.LogMessage($"Fejl ved forbindelse til Modbus PLC på IP: {socket.IP}, PLC returnerede et ukendt funktionskode: {response.Data.FunctionCode}", TraceEventType.Error);
                                output = null;
                                return;
                            }
                        }
                    });
                    return output;
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogMessage($"Netværksfejl til Modbus PLC med IP-Addresse {socket.IP}", TraceEventType.Error);
                    return null;
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




        private void TryAddPLC(IPAddress ip, ref bool added)
        {
            if (!added)
            {
                _responsivePLCs.Add(ip);
                added = true;
            }
        }

        private IsoTcpMessage TryCOTPConnect(IsoTcpMessage connectionMsg, IPAddress ip, NetworkStream stream)
        {

            var COTPResponse = IsoTcpMessage.SendReceive(connectionMsg, stream);
            return COTPResponse;


        }
        private ReadSZLResponseData? StartSTEP7Identification(IPAddress ip)
        {
            List<IsoTcpMessage> messages = new List<IsoTcpMessage>();

            IsoTcpMessageFactory factory = new IsoTcpMessageFactory();
            messages.Add(factory.CreateCRConnectRequestOne());
            messages.Add(factory.CreateCRConnectRequestTwo());
            TcpClient client = null;
            NetworkStream stream = null;
            bool connected = false;
            foreach (var connectionMsg in messages)
            {
                try
                {
                    client = new TcpClient(ip.ToString(), STEP7Message.STEP7_TCP_PORT);
                    stream = client.GetStream();
                    TryAddPLC(ip, ref connected);
                }
                catch
                {
                    stream?.Dispose();
                    client?.Close();
                    return null;
                }
                try
                {
                    var responseMsg = TryCOTPConnect(connectionMsg, ip, stream);
                    if (responseMsg != null)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogMessage($"Fejl ved COTP-forbindelse til STEP7-PLC: {ex.Message} på IP: {ip}", TraceEventType.Warning);
                    stream?.Dispose();
                    client?.Close();
                }
            }
            // If no COTP connections was accepted
            if (client == null || stream == null)
            {
                Logger.Instance.LogMessage($"Ingen COTP forbindelser virkede på IP {ip}, skipping", TraceEventType.Error);
                return null;
            }
            using (client)
            using (stream)
            {
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
                        Logger.Instance.LogMessage($"Fejl ved i svar fra Setup Communication. Fejlklasse: {err.errClass}, Fejlkode: {err.errValue}", TraceEventType.Error);
                        return null;
                    }

                }
                catch (Exception ex)
                {

                    Logger.Instance.LogMessage($"Netværksfejl ved Setup Communication i forbindelse til STEP7-PLC: {ex.Message} på IP: {ip}", TraceEventType.Error);
                    return null;
                }
                try
                {
                    IsoTcpMessage ReadSZLDataMsg = factory.CreateReadSZL();
                    ReadSZLResponse = IsoTcpMessage.SendReceive(ReadSZLDataMsg, stream);
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogMessage($"Fejl ved aflæsning af SZL data i forbindelse til STEP7-PLC: {ex.Message} på IP: {ip}", TraceEventType.Error);
                    return null;
                }
                return STEP7ResponseParsing.ParseReadSZLResponse(ReadSZLResponse, ip);
            }


        }
        #endregion

    }
}


