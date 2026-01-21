using PLCompliant.Interface;
using PLCompliant.Modbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.RequestModels
{
    public struct ReadDeviceIdentificationRequest : IConvertible<ModBusMessage, ushort> , IEndianConvertable
    {
        private byte _functionCode;
        private byte _subfunctionCode;
        private byte _productId;
        private byte _objectIdentifier;


        /// <summary>
        /// Function code, should always be 0x2B for this type
        /// </summary>
        public byte FunctionCode { get { return _functionCode; } set { _functionCode = value; } }
        /// <summary>
        /// Subfunction code. Should be 0x0E for this type
        /// </summary>
        public byte SubfunctionCode { get { return _subfunctionCode; } set { _subfunctionCode = value; } }

        /// <summary>
        /// Defines how much data is requested. 0x1 = basic information. 0x2 = extended information. The actual amount of data returned in each category depends on the PLC model
        /// </summary>
        public byte ProductId { get { return _productId; } set { _productId = value; } }
        /// <summary>
        /// Not used, as far as i can tell
        /// </summary>
        public byte ObjectIdentifier { get { return _objectIdentifier; } set { _objectIdentifier = value; } }

        /// <summary>
        /// Converts the object to a ModBusMessage
        /// </summary>
        /// <param name="transactionIdentifier">The transaction identifier for the Modbus message</param>
        /// <returns>A ModBusMessage</returns>
        public ModBusMessage Convert(ushort transactionIdentifier)
        {
            ModBusMessage result = new(new(transactionIdentifier, 0, 0xff), new());
            result.AddData(this);
            return result;
        }
        // We need Endian convertible functions to pass it to AddData, but no need to endian convert anything so it does nothing
        /// <inheritdoc/>
        public void FromHostToNetwork()
        {
            
        }
        /// <inheritdoc/>
        public void FromNetworkToHost()
        {
            
        }
    }
}
