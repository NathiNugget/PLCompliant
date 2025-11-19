using PLCompliant.Modbus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliantTests
{
    [ExcludeFromCodeCoverage]
    public static class UtilityMethodTests
    {

        //This method instantiates a ModBusMessage akin to an response from PLC. 
        public static ModBusMessage CreateExampleReadDeviceInformationResponse()
        {
            ModBusMessage msg = new(new(0, 0, 255), new((byte)ModBusCommandType.read_device_information, []));
            string obj1 = "Schneider Electric";
            string obj2 = "BMX NOE 0100";
            string obj3 = "V2.30";
            msg.AddData(0xe);
            msg.AddData(0x2);
            msg.AddData(0x81);
            msg.AddData(0x00);
            msg.AddData(0x00);
            msg.AddData(0x03);

            msg.AddData(0x0);
            msg.AddData(0x12);
            msg.AddData(Encoding.UTF8.GetBytes(obj1));

            msg.AddData(0x1);
            msg.AddData(0xc);
            msg.AddData(Encoding.UTF8.GetBytes(obj2));

            msg.AddData(0x2);
            msg.AddData(0x5);
            msg.AddData(Encoding.UTF8.GetBytes(obj3));

            return msg; 
        }
    }
}
