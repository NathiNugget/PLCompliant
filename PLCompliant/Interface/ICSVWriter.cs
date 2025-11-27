using PLCompliant.Response;
namespace PLCompliant.Interface
{
    /// <summary>
    /// Interface for writing CSV's for diffrent protocols
    /// </summary>
    public interface ICSVWriter
    {
        /// <summary>
        /// Generates a CSV file with the given CSV data string, and auto-generating a filename
        /// </summary>
        /// <param name="dirPath">Path to the directory</param>
        /// <param name="CSVText">String of CSV data to write</param>
        /// <returns>The name of the auto-generated file</returns>
        public string GenerateCSVFile(string dirPath, string CSVText);
        /// <summary>
        /// Generates a CSV string of a IEnumerable of ResponseData objects
        /// </summary>
        /// <param name="responses">Enumerable of responses to convert to CSV</param>
        /// <returns>String representing the CSV formatted data</returns>
        public string GenerateCSVString(IEnumerable<ResponseData> responses);
    }
}
