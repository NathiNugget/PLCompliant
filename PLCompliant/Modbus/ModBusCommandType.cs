using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Modbus
{
    /// <summary>
    /// An enum for the implemented function codes for PLCs using Modbus TCP/IP 502
    /// </summary>
    public enum ModBusCommandType : byte
    {
        get_slave_id = 17,
        read_device_information = 43,
    }
}
