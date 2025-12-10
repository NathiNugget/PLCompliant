using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;

namespace PLCompliant.Modbus
{
    /// <summary>
    /// This class represents the data field for the Modbus protocol
    /// </summary>
    public class ModBusData : IProtocolData
    {
        #region instance fields
        /// <summary>
        /// The function code to run on the PLC
        /// </summary>
        public byte _functionCode;
        /// <summary>
        /// The data to send converted to bytes
        /// </summary>
        public byte[] _payload = [];
        #endregion

        #region constructors
        /// <summary>
        /// Empty constructor for easing construction for either unit tests or other cases where an empty constructor should be used
        /// </summary>
        public ModBusData()
        {
        }

        /// <summary>
        /// The normal constructor for the class
        /// </summary>
        /// <param name="functionCode">The function code to be run</param>
        /// <param name="payload">The data to be followed by the function code</param>
        public ModBusData(byte functionCode, byte[] payload)
        {
            _functionCode = functionCode;
            _payload = payload;

        }

        #endregion

        #region methods



        public void AddData(UInt16 inputData)
        {
            var oldSize = _payload.Length;
            var newSize = _payload.Length + Marshal.SizeOf<UInt16>();
            Array.Resize(ref _payload, newSize);
            byte[] bytes = BitConverter.GetBytes(EndianConverter.FromHostToNetwork(inputData));
            Array.Copy(bytes, 0, _payload, oldSize, bytes.Length);
        }


        /// <inheritdoc/>
        public void AddData(byte inputData)
        {
            var newSize = _payload.Length + Marshal.SizeOf<byte>();
            Array.Resize(ref _payload, newSize);
            _payload[newSize - 1] = inputData;
        }

        /// <inheritdoc/>
        public void AddData(byte[] stringData)
        {
            if (stringData.Length > byte.MaxValue)
            {
                throw new ArgumentException("Input length was greater than allowed in a byte");
            }
            byte stringSize = (byte)stringData.Length;
            if (stringSize == 0) { return; }
            var oldSize = _payload.Length;
            var newSize = _payload.Length + stringSize;
            Array.Resize(ref _payload, newSize);
            Array.Copy(stringData, 0, _payload, oldSize, stringSize);
        }






        /// <summary>
        /// Serialize the function code and data for network transmission
        /// </summary>
        /// <returns>Bytes for network transmission</returns>
        public byte[] Serialize()
        {
            byte[] buffer = new byte[Marshal.SizeOf(_functionCode) + _payload.Length];
            buffer[0] = _functionCode;
            Array.Copy(_payload, 0, buffer, Marshal.SizeOf(_functionCode), _payload.Length);
            return buffer;
        }

        /// <summary>
        /// Deserialize the data to be human readable
        /// </summary>
        /// <param name="inputBuffer">The data in bytes received from the network</param>
        public void Deserialize(byte[] inputBuffer, int startIndex = 0)
        {
            _functionCode = inputBuffer[startIndex];
            startIndex += sizeof(byte);
            Array.Resize(ref _payload, inputBuffer.Length - startIndex);
            Array.Copy(inputBuffer, startIndex, _payload, 0, inputBuffer.Length - startIndex);
        }
        /// <summary>
        /// Override equals to compare to another data-packet
        /// </summary>
        /// <param name="other">Other ModBusData to compare to</param>
        /// <returns>If the objects are equal or not</returns>
        public override bool Equals(object? other)
        {
            if (other is null || other is not ModBusData) return false;
            ModBusData other_data = (ModBusData)other;
            return (Size == other_data.Size && _functionCode == other_data._functionCode && _payload.SequenceEqual(other_data._payload));
        }

        #endregion

        #region properties
        /// <summary>
        /// Property to get the Size of the data + function code in bytes
        /// </summary>
        public int Size { get { return PayloadSize + Marshal.SizeOf(_functionCode); } }
        /// <summary>
        /// 
        /// </summary>
        public ushort PayloadSize { get { return (ushort)_payload.Length; } }
        #endregion
    }


}
