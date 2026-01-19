using PLCompliant.Response;
using System.Net;
using System.Text;

namespace PLCompliant.Modbus
{
    /// <summary>
    /// This is a static class with capability to read the response from the network
    /// </summary>
    public static class ModBusResponseParsing
    {
        #region static methods
        /// <summary>
        /// Method to try and read the incoming packet.
        /// </summary>
        /// <param name="msg">The response</param>
        /// <param name="errCode">0 if nothing most significant bit is 0, otherwise has the number specified in the payload</param>
        /// <returns>False if there is an exception, otherwise true</returns>
        public static bool TryHandleReponseError(ModBusMessage msg, out byte errCode)
        {
            byte functionCode = msg.Data.FunctionCode;
            bool err = (functionCode & 0b1000_0000) != 0;

            if (!err)
            {
                errCode = 0;
                return true;
            }
            errCode = msg.Data.Payload[1];
            //TODO: Write into log perhaps or send an event to UI
            return false;
        }

        /// <summary>
        /// Parse the response
        /// </summary>
        /// <param name="msg">The response from PLC</param>
        /// <param name="address">IP address of the device the originates from</param>
        /// <returns>Object containing the response device information</returns>
        public static ReadDeviceInformationData ParseReadDeviceInformationResponse(ModBusMessage msg, IPAddress address)
        {
            var result = new ReadDeviceInformationData();
            result.IPAddr = address;
            byte subfunction_code = msg.Data.Payload[1];
            byte productID = msg.Data.Payload[2];
            byte conformity_level = msg.Data.Payload[3];
            byte reserved_1 = msg.Data.Payload[4];
            byte reserved_2 = msg.Data.Payload[5];
            result.noOfObjects = msg.Data.Payload[6];
            int index = 7;
            for (int i = 0; i < result.noOfObjects; i++)
            {
                byte id = msg.Data.Payload[index];
                index++;
                byte length = msg.Data.Payload[index];
                index++;
                string content = Encoding.UTF8.GetString(msg.Data.Payload, index, length);
                result.Objects.Add(id, content);
                index += length;

            }
            return result;
        }
        #endregion
    }
}
