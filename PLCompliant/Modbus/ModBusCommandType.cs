using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Modbus
{
    public enum ModBusCommandType : byte
    {
        get_slave_id = 17,
        read_device_information = 43,
    }
}
