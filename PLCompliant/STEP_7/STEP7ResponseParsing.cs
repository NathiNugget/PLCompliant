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
            else if (msg.STEP7Data != null && msg.STEP7Data.Header.ReturnCode != 0xff)
            {
                errInfo.errorType = Enums.STEP7ErrorType.DataError;
                errInfo.errClass = 0;
                errInfo.errValue = msg.STEP7Data.Header.ReturnCode;
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

            int startIndex = 0;
            ReadOnlySpan<byte> dataSpan = new(msg.STEP7.STEP7Data.Data.Data);
            var responseHeader = MemoryMarshal.Read<ReadSZLResponseHeader>(dataSpan.Slice(startIndex, Marshal.SizeOf<ReadSZLResponseHeader>()));
            startIndex += Marshal.SizeOf<ReadSZLResponseHeader>();
            responseHeader.FromNetworkToHost();
            var result = new ReadSZLResponseData(responseHeader);


            for (int i = 0; i < result.Header.ListCount; i++)
            {
                ReadSZLDataItem item = MemoryMarshal.Read<ReadSZLDataItem>(dataSpan.Slice(startIndex, Marshal.SizeOf<ReadSZLDataItem>()));
                item.FromNetworkToHost();
                result.Objects.Add(item);
                startIndex += Marshal.SizeOf<ReadSZLDataItem>();
            }
            return result;
        }
    }
}
