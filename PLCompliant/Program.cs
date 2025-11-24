using PLCompliant.Events;
using PLCompliant.Scanning;
using PLCompliant.Utilities;
using System.Net;

namespace PLCompliant
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {







            /*
            TcpClient client = new TcpClient("192.168.123.100", 502);


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

                byte[] payload_data = new byte[response.Header.length-1];
                Array.Copy(returnbytes, Marshal.SizeOf<ModBusHeader>(), payload_data, 0, payload_data.Length);





                response.DeserializeData(payload_data);
                var output = ModBusResponseParsing.ParseReadDeviceInformationResponse(response, System.Net.IPAddress.Parse("192.168.123.100"));
                var CSV_data = output.ToCSV();
                Console.WriteLine(CSV_data);







                address += 64;
                identifier++;
                Thread.Sleep(100);
            }








            client.Close();
            */
            MainThreadContext context = new MainThreadContext();

            Thread updateThread = new Thread( () =>
            {
                while(!GlobalVars.ABORT)
                {
                    while(!MainEventQueue.Instance.Empty)
                    {
                        if(MainEventQueue.Instance.Pop(out var evt))
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
            Application.Run(new Form1());
        }
    }
}