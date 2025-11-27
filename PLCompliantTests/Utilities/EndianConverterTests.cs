using System.Diagnostics.CodeAnalysis;

namespace PLCompliant.Utilities.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class EndianConverterTests
    {
        [TestMethod()]
        [DataRow((uint)(uint.MinValue + (uint)1), (uint)1)]
        [DataRow(uint.MaxValue / 2, uint.MaxValue / 2)]

        public void FromHostToNetworkTest(uint val, uint originalbytes)
        {

            byte[] bytes = BitConverter.GetBytes(val);
            byte[] expectedbytes = BitConverter.GetBytes(originalbytes);
            val = EndianConverter.FromHostToNetwork(val);
            bytes = BitConverter.GetBytes(val);
            if (BitConverter.IsLittleEndian)
            {
                Assert.IsFalse(Enumerable.SequenceEqual(expectedbytes, bytes));


            }
            else
            {
                Assert.IsTrue(Enumerable.SequenceEqual(expectedbytes, bytes));

            }








        }

        [TestMethod()]
        [DataRow(int.MinValue, int.MinValue)]
        [DataRow(int.MaxValue, int.MaxValue)]


        public void FromHostToNetworkInt(int actual, int original)
        {


            byte[] actualbytes = BitConverter.GetBytes(actual);
            byte[] expectedbytes = BitConverter.GetBytes(original);
            actual = EndianConverter.FromHostToNetwork(actual);
            actualbytes = BitConverter.GetBytes(actual);
            if (BitConverter.IsLittleEndian)
            {
                Assert.IsFalse(Enumerable.SequenceEqual(expectedbytes, actualbytes));


            }
            else
            {
                Assert.IsTrue(Enumerable.SequenceEqual(expectedbytes, actualbytes));

            }
        }

        [TestMethod()]
        [DataRow((ushort)(ushort.MinValue + (ushort)1), (ushort)(ushort.MinValue + (ushort)1))]
        [DataRow((ushort)(ushort.MaxValue - (ushort)1), (ushort)(ushort.MaxValue - (ushort)1))]

        public void FromHostToNetworkTestUshort(ushort actual, ushort original)
        {
            byte[] actualbytes = BitConverter.GetBytes(actual);
            byte[] expectedbytes = BitConverter.GetBytes(original);
            actual = EndianConverter.FromHostToNetwork(actual);
            actualbytes = BitConverter.GetBytes(actual);
            if (BitConverter.IsLittleEndian)
            {
                Assert.IsFalse(Enumerable.SequenceEqual(expectedbytes, actualbytes));

            }
            else
            {
                Assert.IsTrue(Enumerable.SequenceEqual(expectedbytes, actualbytes));
            }


        }

        [TestMethod()]
        [DataRow(short.MinValue, short.MinValue)]
        [DataRow(short.MaxValue, short.MaxValue)]
        public void FromHostToNetworkTestShort(short actual, short original)
        {
            byte[] actualbytes = BitConverter.GetBytes(actual);
            byte[] expectedbytes = BitConverter.GetBytes(original);
            actual = EndianConverter.FromHostToNetwork(actual);
            actualbytes = BitConverter.GetBytes(actual);
            if (BitConverter.IsLittleEndian)
            {
                Assert.IsFalse(Enumerable.SequenceEqual(expectedbytes, actualbytes));


            }
            else
            {
                Assert.IsTrue(Enumerable.SequenceEqual(expectedbytes, actualbytes));

            }


        }

        [TestMethod()]
        [DataRow((long)(long.MaxValue - 1), (long)(long.MaxValue - 1))]
        [DataRow((long)(long.MinValue + 1), (long)(long.MinValue + 1))]
        public void FromHostToNetworkTestLong(long actual, long original)
        {

            byte[] actualbytes = BitConverter.GetBytes(actual);
            byte[] expectedbytes = BitConverter.GetBytes(original);
            actual = EndianConverter.FromHostToNetwork(actual);
            actualbytes = BitConverter.GetBytes(actual);
            if (BitConverter.IsLittleEndian)
            {
                Assert.IsFalse(Enumerable.SequenceEqual(expectedbytes, actualbytes));


            }
            else
            {
                Assert.IsFalse(Enumerable.SequenceEqual(expectedbytes, actualbytes));

            }
        }

        [TestMethod()]
        [DataRow((uint.MaxValue - 1), (uint)(uint.MaxValue - 1))]
        [DataRow((uint)(uint.MinValue + 1), (uint)(uint.MinValue + 1))]
        public void FromNetworkToHostTestUint(uint actual, uint original)
        {
            byte[] actualbytes = BitConverter.GetBytes(actual);
            byte[] expectedbytes = BitConverter.GetBytes(original);
            actual = EndianConverter.FromNetworkToHost(actual);
            actualbytes = BitConverter.GetBytes(actual);
            if (BitConverter.IsLittleEndian)
            {
                Assert.IsFalse(Enumerable.SequenceEqual(expectedbytes, actualbytes));


            }
            else
            {
                Assert.IsTrue(Enumerable.SequenceEqual(expectedbytes, actualbytes));

            }
        }

        [TestMethod()]
        [DataRow((int.MaxValue - 1), (int)(int.MaxValue - 1))]
        [DataRow((int)(int.MinValue + 1), (int)(int.MinValue + 1))]
        public void FromNetworkToHostTestInt(int actual, int original)
        {
            byte[] actualbytes = BitConverter.GetBytes(actual);
            byte[] expectedbytes = BitConverter.GetBytes(original);
            actual = EndianConverter.FromNetworkToHost(actual);
            actualbytes = BitConverter.GetBytes(actual);
            if (BitConverter.IsLittleEndian)
            {
                Assert.IsFalse(Enumerable.SequenceEqual(expectedbytes, actualbytes));


            }
            else
            {
                Assert.IsTrue(Enumerable.SequenceEqual(expectedbytes, actualbytes));

            }
        }

        [TestMethod()]
        [DataRow((ushort)(ushort.MaxValue - 1), (ushort)(ushort.MaxValue - 1))]
        [DataRow((ushort)(ushort.MinValue + 1), (ushort)(ushort.MinValue + 1))]
        public void FromNetworkToHostTestUshort(ushort actual, ushort original)
        {
            byte[] actualbytes = BitConverter.GetBytes(actual);
            byte[] expectedbytes = BitConverter.GetBytes(original);
            actual = EndianConverter.FromNetworkToHost(actual);
            actualbytes = BitConverter.GetBytes(actual);
            if (BitConverter.IsLittleEndian)
            {
                Assert.IsFalse(Enumerable.SequenceEqual(expectedbytes, actualbytes));


            }
            else
            {
                Assert.IsTrue(Enumerable.SequenceEqual(expectedbytes, actualbytes));

            }
        }

        [TestMethod()]
        [DataRow((short)(short.MaxValue - 1), (short)(short.MaxValue - 1))]
        [DataRow((short)(short.MinValue + 1), (short)(short.MinValue + 1))]
        public void FromNetworkToHostTestShort(short actual, short original)
        {
            byte[] actualbytes = BitConverter.GetBytes(actual);
            byte[] expectedbytes = BitConverter.GetBytes(original);
            actual = EndianConverter.FromNetworkToHost(actual);
            actualbytes = BitConverter.GetBytes(actual);
            if (BitConverter.IsLittleEndian)
            {
                Assert.IsFalse(Enumerable.SequenceEqual(expectedbytes, actualbytes));


            }
            else
            {
                Assert.IsTrue(Enumerable.SequenceEqual(expectedbytes, actualbytes));

            }
        }

        [TestMethod()]
        [DataRow((long)(long.MaxValue - 1), (long)(long.MaxValue - 1))]
        [DataRow((long)(long.MinValue + 1), (long)(long.MinValue + 1))]
        public void FromNetworkToHostTestLong(long actual, long original)
        {
            byte[] actualbytes = BitConverter.GetBytes(actual);
            byte[] expectedbytes = BitConverter.GetBytes(original);
            actual = EndianConverter.FromNetworkToHost(actual);
            actualbytes = BitConverter.GetBytes(actual);
            if (BitConverter.IsLittleEndian)
            {
                Assert.IsFalse(Enumerable.SequenceEqual(expectedbytes, actualbytes));


            }
            else
            {
                Assert.IsTrue(Enumerable.SequenceEqual(expectedbytes, actualbytes));

            }
        }

        

        
    }
}