namespace PLCompliant.Modbus
{
    /// <summary>
    /// This class is a factory making creation of ModBusMessage easier as you don't have to specify all the different bytes to create a read-message
    /// </summary>
    public class ModBusMessageFactory
    {
        #region methods
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


        #endregion
    }
}
