namespace PLCompliant.Modbus
{
    /// <summary>
    /// An enum for the implemented function codes for PLCs using Modbus TCP/IP 502
    /// </summary>
    public enum ModBusCommandType : byte
    {
        /// <summary>
        /// Function code for reading about the PLC id
        /// </summary>
        get_slave_id = 17,
        /// <summary>
        /// Function code for reading about the PLC device information
        /// </summary>
        read_device_information = 43,
    }
}
