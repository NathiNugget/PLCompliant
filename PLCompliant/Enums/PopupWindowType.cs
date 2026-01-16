namespace PLCompliant.Enums
{
    /// <summary>
    /// This enum represents the different window types possible to be shown to the user. 
    /// </summary>
    public enum PopupWindowType : byte
    {
        /// <summary>
        /// Show an error winodw
        /// </summary>
        ErrorWindow = 0,
        /// <summary>
        /// Show a warning window
        /// </summary>
        WarningWindow = 1,
        /// <summary>
        /// Show an information window
        /// </summary>
        InformationWindow = 2
    }
}
