using PLCompliant.Modbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PLCompliant.Interface
{
    /// <summary>
    /// This is an interface for the whole message to send over the protcol implementing this interface
    /// </summary>
    public interface IProtocolMessage
    {
        /// <summary>
        /// Add data to the data field of the protcol. Also converts to network endianness 
        /// </summary>
        /// <param name="inputData">Ushort to add</param>
        public void AddData(UInt16 inputData);
        /// <summary>
        /// Add data to the data field of the protocol
        /// </summary>
        /// <param name="inputData">The byte to add</param>
        public void AddData(byte inputData);
        /// <summary>
        /// Add data to the data field of the protocol.
        /// </summary>
        /// <param name="stringData">Usually a string converted to UTF-8 bytes</param>
        public void AddData(byte[] stringData);
        /// <summary>
        /// Serialize every data field, header and other fields that may be present in the protocol
        /// </summary>
        /// <returns>The final byte array for network transmission</returns>
        public byte[] Serialize();
        /// <summary>
        /// Deserialize header and fields to human readable format
        /// </summary>
        /// <param name="inputBuffer">The bytes received from the network</param>
        public void DeserializeHeader(byte[] inputBuffer);
        /// <summary>
        /// Deserialize data and fields to human readable format
        /// </summary>
        /// <param name="inputBuffer">The bytes received from the network</param>
        public void DeserializeData(byte[] inputBuffer);
     
       
        /// <summary>
        /// Size of the Data in bytes
        /// </summary>
        public int DataSize { get; }
        /// <summary>
        /// The Data-field of the Message
        /// </summary>
        public ModBusData Data { get; }
       
    }
}
