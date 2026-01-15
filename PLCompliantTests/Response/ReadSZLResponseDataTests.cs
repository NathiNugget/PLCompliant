using PLCompliant.Response;
using System.Diagnostics.CodeAnalysis;

namespace PLCompliantTests.Response
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class ReadSZLResponseDataTests
    {
        static string correctCSVOutput = "192.168.123.99;6ES7 211-1BE40-0XB0 ;V4.5.1";
        [TestMethod()]
        public void ToCSVTestSucess()
        {
            ReadSZLResponseData response = TestHelper.CreateExampleReadSZLResponse();
            string expected = correctCSVOutput;
            string actual = response.ToCSV();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void ToCSVTestFailureNoMatch()
        {
            ReadSZLResponseData response = TestHelper.CreateExampleReadSZLResponse();
            string expected = correctCSVOutput;
            string actual = response.ToCSV();
            Assert.AreNotEqual(expected, actual);
        }
    }
}
