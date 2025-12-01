using System.Diagnostics.CodeAnalysis;

namespace PLCompliant.Utilities
{
    /// <summary>
    /// Variables used throughout the application that did not make sense to include in their respective places
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class GlobalVars
    {
        public const char CSV_SEPARATOR = ';';
        public static bool ABORT = false;
        public static string CustomFormat = "HH.mm_dd-MM-yyyy";
    }
}
