using System.Net.Sockets;
using PLCompliant.Modbus; 

namespace PLCompliant
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

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
                ModBusHeader header = new();
                header.length = 5;
                header.unitID = 0xFF; 
                header.transactionIdentifier = (ushort)identifier;
                ModBusData data = new();
                data.functionCode = 0x2B; 

                ModBusMessage msg = new(header, data);
                msg.AddData((byte)0xE);
                msg.AddData((byte)0x2);
                msg.AddData((byte)0x0);
                








                byte[] buffer = msg.Serialize();

                try
                {
                    var stream = client.GetStream();

                    stream.Write(buffer, 0, buffer.Length);

                    byte[] returnbytes = new byte[1024];
                    int readbytes = 0;
                    while (readbytes == 0)
                    {
                        readbytes = stream.Read(returnbytes, 0, 1);

                    }

                    Console.WriteLine(returnbytes);
                    ModBusMessage response = new(new ModBusHeader(), new ModBusData()); 
                    response.DeserializeData(returnbytes);
                    


                }

                catch
                {

                }
                address += 64;
                identifier++;
                Thread.Sleep(100);
            }








            client.Close();




            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}