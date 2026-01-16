namespace PLCompliant.Enums
{
    /// <summary>
    /// This enum represents errors in a response from a STEP7-PLC
    /// </summary>
    public enum STEP7ErrorType : byte
    {
        /// <summary>
        /// Default value - returned when responses are good
        /// </summary>
        NoError,
        /// <summary>
        /// Returned when there in is an error in the headers sent from "master"
        /// </summary>
        HeaderError,
        /// <summary>
        /// Returned when there is an error in the data sent from "master"
        /// </summary>
        DataError
    }
}
