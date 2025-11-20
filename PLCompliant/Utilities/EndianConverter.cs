using System.Buffers.Binary;


namespace PLCompliant.Uilities
{
    static public class EndianConverter
    {
        public static uint FromHostToNetwork(uint val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }
        public static int FromHostToNetwork(int val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }
        public static ushort FromHostToNetwork(ushort val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }
        public static short FromHostToNetwork(short val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;

        }



        public static uint FromNetworkToHost(uint val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }
        public static int FromNetworkToHost(int val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }
        public static ushort FromNetworkToHost(ushort val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }
        public static short FromNetworkToHost(short val)
        {
            if (BitConverter.IsLittleEndian)
            {
                val = BinaryPrimitives.ReverseEndianness(val);
            }
            return val;
        }

    }
}
