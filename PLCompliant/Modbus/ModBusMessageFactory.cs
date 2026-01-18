namespace PLCompliant.Modbus
{
    public class ModBusMessageFactory
    {
        /// <summary>
        /// Constuction of a ModBusMessage for when reading PLC device information.
        /// </summary>
        /// <param name="header">The header for the command</param>
        /// <param name="productID">Is expected to be 2 because the command respons with 3 indexes for objects</param>
        /// <returns>A ModBusMessage to send the function. It is not yet serialized</returns>
        public ModBusMessage CreateReadDeviceInformation(ModBusHeader header, byte productID = 2)
        {
            var data = new ModBusData { };
            var msg = new ModBusMessage(header, data);
            msg.AddData((byte)ModBusCommandType.read_device_information); // function code
            msg.AddData((byte)0x0E);
            msg.AddData((byte)productID);
            msg.AddData((byte)0x0);
            return msg;

        }

        //TODO: Implement or remove this!
        /// <summary>
        /// Construction of a ModBusMessage with the function code to read the slave ID. This is not currently working 
        /// 
        /// </summary>
        /// <param name="header">The header for the packet</param>
        /// <returns>A ModBusMessage to send the function. It is not yet serialized</returns>
        public ModBusMessage CreateGetSlaveID(ModBusHeader header)
        {
            var data = new ModBusData {  };
            var msg = new ModBusMessage(header, data);
            msg.AddData((byte)ModBusCommandType.get_slave_id);
            return msg;

        }

    }
}
