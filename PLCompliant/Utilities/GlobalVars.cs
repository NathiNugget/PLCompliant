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
        public static string CustomFormat = "HH.mm.ss_dd-MM-yyyy"; // TODO: Make into string concat from date_format
        public static string WRITEWARNINGTEXT = "Du har valgt en en mappe hvor programmet ikke kan skrive til. Vælg venligst en anden mappe"; // TODO: Replace with illegal_write_access_tooltip_header
        public static string WRITEWARNINGTITLE = "Ugyldig skrive rettighed"; // TODO: Replace with illegal_write_acecss_tooltip_text
        public static Dictionary<string, string> LOCALIZATION = new Dictionary<string, string>();

    }
}
