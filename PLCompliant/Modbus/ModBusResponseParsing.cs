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
        public static bool HandleReponseError(ModBusMessage msg, IPAddress address)
        {
            byte functionCode = msg.Data.functionCode;
            bool err = (functionCode & 0b1000000) != 0;
            if(!err)
            {
                return true;
            }
            byte errCode = msg.Data.payload[0];
            // write it into log or something TODO
            return false;
        }
        public static ReadDeviceInformationData ParseReadDeviceInformationResponse(ModBusMessage msg, IPAddress address)
        {
            var result = new ReadDeviceInformationData();
            result.IPAddr = address;
            byte subfunction_code = msg.Data.payload[0];
            byte productID = msg.Data.payload[1];
            byte conformity_level = msg.Data.payload[2];
            byte reserved_1 = msg.Data.payload[3];
            byte reserved_2 = msg.Data.payload[4];
            result.noOfObjects = msg.Data.payload[5];
            int index = 6;
            for (int i = 0; i < result.noOfObjects; i++)
            {
                byte id = msg.Data.payload[index];
                index++;
                byte length = msg.Data.payload[index];
                index++;
                string content = Encoding.UTF8.GetString(msg.Data.payload, index, length);
                result.Objects.Add(id, content);
                index += length;

            }
            return result;
        }
    }
}
