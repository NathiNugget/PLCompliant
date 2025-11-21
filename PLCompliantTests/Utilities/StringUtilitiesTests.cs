namespace PLCompliant.Utilities.Tests
{
    [TestClass()]
    public class StringUtilitiesTests
    {
        [TestMethod()]
        [DataRow("   .   .   .   ", 3, 0)]
        [DataRow("   .   .   .   ", 7, 7 - 3)]
        [DataRow("   .   .   .   ", 11, 11 - 3)]
        [DataRow("   .   .   .   ", 12, 8)]
        [DataRow("   .   .   .   ", int.MaxValue, 8)]
        [DataRow("   .   .   .   ", int.MinValue, -1)]
        public void PreviousIndexOfTest(string stringtosearch, int index, int expected)
        {
            int actual = stringtosearch.PreviousIndexOfAndFixToSeparator('.', index);
            char[] chars = stringtosearch.ToCharArray();
            Assert.AreEqual(expected, actual);


        }
    }
}