using PLCompliant.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Modbus
{
    public static class ModBusResponseParsing
    {
        public static bool HandleReponseError(ModBusMessage msg, out byte errCode)
        {
            byte functionCode = msg.Data._functionCode;
            bool err = (functionCode & 0b1000_0000) != 0;
            
            if(!err)
            {
                errCode = 0;
                return true;
            }
            errCode = msg.Data._payload[0];
            // write it into log or something TODO
            return false;
        }
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
