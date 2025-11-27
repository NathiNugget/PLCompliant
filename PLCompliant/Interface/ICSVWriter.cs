using PLCompliant.Response;
namespace PLCompliant.Interface
{
    /// <summary>
    /// Interface for writing CSV's for diffrent protocols
    /// </summary>
    public interface ICSVWriter
    {
        /// <summary>
        /// Generates a CSV file with the given response data in the given directory, and auto-generating a filename
        /// </summary>
        /// <param name="dirPath">Path to the directory</param>
        /// <param name="responses">Enumerable of responses to write</param>
        /// <returns>The name of the auto-generated file</returns>
        public string GenerateCSVFile(string dirPath, IEnumerable<ResponseData> responses);
    }
}
