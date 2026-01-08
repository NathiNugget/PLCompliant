using PLCompliant.Config;
using PLCompliant.Events;
using PLCompliant.Scanning;
using PLCompliant.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace PLCompliant
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IPAddressRange range = new IPAddressRange(IPAddress.Parse("192.168.123.0"), IPAddress.Parse("192.168.130.0"));
            List<Socket> sockets = new List<Socket>((int)range.Count);
            foreach (IPAddress ip in range)
            {
                Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                socket.Blocking = false;
                try
                {
                    socket.Connect(ip, 102);
                }
                catch (SocketException e)
                {
                    // 10035 == WSAEWOULDBLOCK
                    if (e.SocketErrorCode == SocketError.WouldBlock)
                    {
                        sockets.Add(socket);
                    }
                    else
                    {
                        socket.Dispose();
                    }
                }

            }
            List<Socket> aliveSockets = new List<Socket>();
            DateTime startTime = DateTime.Now;
            TimeSpan timeout = TimeSpan.FromSeconds(2);
            DateTime endTime = startTime + timeout;
            while (DateTime.Now < endTime)
            {
                for (int i = 0; i < sockets.Count; i++)
                {
                    if (sockets[i] == null)
                    {
                        continue;
                    }
                    if (sockets[i].Poll(1, SelectMode.SelectWrite))
                    {
                        aliveSockets.Add(sockets[i]);
                        sockets[i].Dispose();
                        sockets[i] = null;
                    }
                }
            }
            sockets.ForEach((socket) => { socket.Dispose(); });
            while (aliveSockets.Count > 0)
            {
                foreach (Socket socket in aliveSockets)
                {
                    //do nonblocking socket send/recv
                }
            }

            ConfigLoader.LoadConfigFile("./config.xml");

            UpdateThreadContext context = new UpdateThreadContext();

            Thread updateThread = ThreadUtilities.CreateBackgroundThread(() =>
            {
                while (!GlobalVars.ABORT)
                {
                    while (!UpdateEventQueue.Instance.Empty)
                    {
                        if (UpdateEventQueue.Instance.TryPop(out var evt))
                        {
                            evt.ExecuteEvent(context);
                        }
                    }
                    Thread.Sleep(100);
                }

            });
            updateThread.Start();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            ApplicationConfiguration.Initialize();
            Application.Run(new PLCompliantUI());
        }
    }
}