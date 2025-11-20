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
        /// <summary>
        /// Method to try and read the incoming packet.
        /// </summary>
        /// <param name="msg">The response</param>
        /// <param name="errCode">0 if nothing most significant bit is 0, otherwise has the number specified in the payload</param>
        /// <returns>True if there is an exception in the response, otherwise false</returns>
        public static bool TryHandleReponseError(ModBusMessage msg, out byte errCode)
        {
            byte functionCode = msg.Data._functionCode;
            bool err = (functionCode & 0b1000_0000) != 0;

            if (!err)
            {
                errCode = 0;
                return true;
            }
            errCode = msg.Data._payload[0];
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
            byte subfunction_code = msg.Data._payload[0];
            byte productID = msg.Data._payload[1];
            byte conformity_level = msg.Data._payload[2];
            byte reserved_1 = msg.Data._payload[3];
            byte reserved_2 = msg.Data._payload[4];
            result.noOfObjects = msg.Data._payload[5];
            int index = 6;
            for (int i = 0; i < result.noOfObjects; i++)
            {
                byte id = msg.Data._payload[index];
                index++;
                byte length = msg.Data._payload[index];
                index++;
                string content = Encoding.UTF8.GetString(msg.Data._payload, index, length);
                result.Objects.Add(id, content);
                index += length;

            }
            return result;
        }
    }
}
