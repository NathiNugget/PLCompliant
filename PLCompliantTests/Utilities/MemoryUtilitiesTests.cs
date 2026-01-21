
using PLCompliant.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace PLCompliantTests.Utilities
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class MemoryUtilitiesTests
    {
        [TestMethod]
        public void CompareMemoryTestSucess()
        {
            TestStruct s1 = new TestStruct() {b1 = 1, b2 = 133, b3 = 2134, b4 = 135 };
            TestStruct s2 = new TestStruct() {b1 = 1, b2 = 133, b3 = 2134, b4 = 135 };

            Assert.IsTrue(MemoryUtilities.CompareMemory(ref s1, ref s2));
        }
        [TestMethod]
        public void CompareMemoryTestFailure()
        {
            TestStruct s1 = new TestStruct() { b1 = 1, b2 = 133, b3 = 2134, b4 = 135 };
            TestStruct s2 = new TestStruct() { b1 = 2, b2 = 133, b3 = 2134, b4 = 135 };

            Assert.IsFalse(MemoryUtilities.CompareMemory(ref s1, ref s2));
        }
    }
}
