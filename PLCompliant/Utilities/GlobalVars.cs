using System.Diagnostics.CodeAnalysis;

namespace PLCompliant.Utilities
{
    /// <summary>
    /// Variables used throughout the application that did not make sense to include in their respective places as they are used repeatedly throughout different poritions of the application
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class GlobalVars
    {
        public const char CSV_SEPARATOR = ';';
        public static bool ABORT = false;
        public static string CustomFormat = "HH.mm.ss_dd-MM-yyyy";
        public static string WRITEWARNINGTEXT = "Du har valgt en en mappe hvor programmet ikke kan skrive til. Vælg venligst en anden mappe";
        public static string WRITEWARNINGTITLE = "Ugyldig skrive rettighed";

    }
}
