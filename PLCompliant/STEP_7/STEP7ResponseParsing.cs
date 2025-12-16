using PLCompliant.Logging;
using PLCompliant.Response;
using PLCompliant.Utilities;
using System.Net;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    public static class STEP7ResponseParsing
    {


        public static bool TryHandleReponseError(STEP7Message msg, out STEP7ErrorInfo errInfo)
        {
            if (msg.STEP7Header.MessageType == 0x3 && (msg.STEP7Header.ErrorClass != 0 || msg.STEP7Header.ErrorCode != 0))
            {
                errInfo.errorType = Enums.STEP7ErrorType.HeaderError;
                errInfo.errClass = msg.STEP7Header.ErrorClass;
                errInfo.errValue = msg.STEP7Header.ErrorCode;
                return true;
            }
            else if (msg.STEP7Data != null && msg.STEP7Data.ReturnCode != 0xff)
            {
                errInfo.errorType = Enums.STEP7ErrorType.DataError;
                errInfo.errClass = 0;
                errInfo.errValue = msg.STEP7Data.ReturnCode;
                return true;
            }
            else
            {
                errInfo.errorType = Enums.STEP7ErrorType.NoError;
                errInfo.errClass = 0;
                errInfo.errValue = 0;
                return false;
            }
        }



        public static ReadSZLResponseData ParseReadSZLResponse(IsoTcpMessage msg, IPAddress address)
        {
            // TODO map and lay out the parameter (and the entire response) part propertly to be able to detect param errors earlier.
            UInt16 paramErr = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(msg.STEP7.STEP7ParamData.Data, msg.STEP7.STEP7ParamData.Data.Length - 2));
            if (paramErr != 0)
            {
                Logger.Instance.LogMessage($"Parameter fejl i forbindelse til STEP7-PLC: Fejlkode {paramErr}", System.Diagnostics.TraceEventType.Error);
                return new ReadSZLResponseData();
            }

            var result = new ReadSZLResponseData();
            int startIndex = 0;
            result.DiagnosticTypeMask = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(msg.STEP7.STEP7Data.Data, startIndex)); // TODO: Double check endianness of this one if its important
            startIndex += Marshal.SizeOf<UInt16>();

            result.SZLIndex = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(msg.STEP7.STEP7Data.Data, startIndex));
            startIndex += Marshal.SizeOf<UInt16>();

            result.ListLength = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(msg.STEP7.STEP7Data.Data, startIndex));
            startIndex += Marshal.SizeOf<UInt16>();

            result.ListCount = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(msg.STEP7.STEP7Data.Data, startIndex));
            startIndex += Marshal.SizeOf<UInt16>();


            for (int i = 0; i < result.ListCount; i++)
            {
                UInt16 index = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(msg.STEP7.STEP7Data.Data, startIndex));
                startIndex += Marshal.SizeOf<UInt16>();

                OrderNumBuffer orderNum = new OrderNumBuffer();

                // Ugly, one memcpy call could fix all of this, but alas we must stay safe  ):
                for (int j = 0; j < OrderNumBuffer.SIZE; j++)
                {
                    orderNum[j] = (char)msg.STEP7.STEP7Data.Data[startIndex + j];
                }
                startIndex += OrderNumBuffer.SIZE;
                UInt16 moduleTypeId = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(msg.STEP7.STEP7Data.Data, startIndex));
                startIndex += Marshal.SizeOf<UInt16>();

                UInt16 version = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(msg.STEP7.STEP7Data.Data, startIndex));
                startIndex += Marshal.SizeOf<UInt16>();

                UInt16 pgDescription = EndianConverter.FromNetworkToHost(BitConverter.ToUInt16(msg.STEP7.STEP7Data.Data, startIndex));
                startIndex += Marshal.SizeOf<UInt16>();
                result.Objects.Add(new ReadSZLDataItem(index, ref orderNum, moduleTypeId, version, pgDescription));
            }
            return result;
        }
    }
}
