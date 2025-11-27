using System.Buffers.Binary;


namespace PLCompliant.Utilities
{
    /// <summary>
    /// This class is a helper-class made for possibly converting endianness of values where byte order could be different between host and network
    /// </summary>
    static public class EndianConverter
    {
        /// <summary>
        /// Convert byte order from host to network
        /// </summary>
        /// <param name="val">A uint</param>
        /// <returns>Value with possibly reversed endianness</returns>
        public static uint FromHostToNetwork(uint val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }

        /// <summary>
        /// Convert byte order from host to network
        /// </summary>
        /// <param name="val">An int</param>
        /// <returns>Value with possibly reversed endianness</returns>
        public static int FromHostToNetwork(int val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }

        /// <summary>
        /// Convert from byte order host to network
        /// </summary>
        /// <param name="val">A ushort</param>
        /// <returns>Value with possibly reversed endianness</returns>
        public static ushort FromHostToNetwork(ushort val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }

        /// <summary>
        /// Convert byte order from host to network
        /// </summary>
        /// <param name="val">A short</param>
        /// <returns>Value with possibly reversed endianness</returns>
        public static short FromHostToNetwork(short val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;

        }

        /// <summary>
        /// Convert byte order from host to network
        /// </summary>
        /// <param name="val">A long</param>
        /// <returns>Value with possibly reversed endianness</returns>
        public static long FromHostToNetwork(long val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;

        }

        /// <summary>
        /// Convert byte order from network to host
        /// </summary>
        /// <param name="val">A uint</param>
        /// <returns>Value with possibly reversed endianness</returns>
        public static uint FromNetworkToHost(uint val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }

        /// <summary>
        /// Convert byte order from network to host
        /// </summary>
        /// <param name="val">A val</param>
        /// <returns>Value with possibly reversed endianness</returns>
        public static int FromNetworkToHost(int val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }

        /// <summary>
        /// Convert byte order from network to host
        /// </summary>
        /// <param name="val">A ushort</param>
        /// <returns>Value with possibly reversed endianness</returns>
        public static ushort FromNetworkToHost(ushort val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }

        /// <summary>
        /// Convert byte order from network to host
        /// </summary>
        /// <param name="val">A short</param>
        /// <returns>Value with possibly reversed endianness</returns>
        public static short FromNetworkToHost(short val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }

        /// <summary>
        /// Convert byte order from network to host
        /// </summary>
        /// <param name="val">A long</param>
        /// <returns>Value with possibly reversed endianness</returns>
        public static long FromNetworkToHost(long val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }


    }
}
