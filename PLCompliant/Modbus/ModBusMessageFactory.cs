using PLCompliant.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Modbus
{
    public class ModBusMessageFactory
    {
        public ModBusMessage CreateReadDeviceInformation(ModBusHeader header, byte productID )
        {
            var data = new ModBusData { functionCode = (byte)ModBusCommandType.read_device_information, payload = [] };
            var msg = new ModBusMessage(header,  data);
            msg.AddData(0x0E);
            msg.AddData(productID);
            msg.AddData(0x0);
            return msg;

        }
        public ModBusMessage CreateGetSlaveID(ModBusHeader header, byte productID)
        {
            var data = new ModBusData { functionCode = (byte)ModBusCommandType.get_slave_id, payload = [] };
            var msg = new ModBusMessage(header, data);
            return msg;

        }
    }
}
