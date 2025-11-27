using PLCompliant.Modbus;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace PLCompliantTests
{
    [ExcludeFromCodeCoverage]
    public static class TestHelper
    {

        public static string getDeviceInfoObject1 = "Schneider Electric";
        public static string getDeviceInfoObject2 = "BMX NOE 0100";
        public static string getDeviceInfoObject3 = "V2.30";

        //This method instantiates a ModBusMessage akin to an response from PLC. 
        public static ModBusMessage CreateExampleReadDeviceInformationResponse()
        {
            ModBusMessage msg = new(new(0, 0, 255), new((byte)ModBusCommandType.read_device_information, []));
            msg.AddData(0xe);
            msg.AddData(0x2);
            msg.AddData(0x81);
            msg.AddData(0x00);
            msg.AddData(0x00);
            msg.AddData(0x03);

            msg.AddData(0x0);
            msg.AddData(0x12);
            msg.AddData(Encoding.UTF8.GetBytes(getDeviceInfoObject1));

            msg.AddData(0x1);
            msg.AddData(0xc);
            msg.AddData(Encoding.UTF8.GetBytes(getDeviceInfoObject2));

            msg.AddData(0x2);
            msg.AddData(0x5);
            msg.AddData(Encoding.UTF8.GetBytes(getDeviceInfoObject3));

            return msg;
        }
    }
}
