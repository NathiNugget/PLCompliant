using PLCompliant.Logging;
using PLCompliant.Response;
using PLCompliant.Utilities;
using System.Net;
using System.Runtime.InteropServices;

namespace PLCompliant.STEP_7
{
    /// <summary>
    /// This class is used for handling and parsing responses from STEP7
    /// </summary>
    public static class STEP7ResponseParsing
    {
        #region static methods
        /// <summary>
        /// Try handling a responseerror
        /// </summary>
        /// <param name="msg">The message read from a PLC</param>
        /// <param name="errInfo">Info about the error occured or didn't</param>
        /// <returns></returns>
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


        /// <summary>
        /// Parse a full IsoTcpMessage
        /// </summary>
        /// <param name="msg">The full message received from the network</param>
        /// <param name="address">IP-address of the PLC this response was received from</param>
        /// <returns>Response data to be written to CSV</returns>
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
        #endregion
    }
}
