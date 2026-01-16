using PLCompliant.Logging;
using System.Diagnostics;
using System.Xml;

namespace PLCompliant.Config
{
    /// <summary>
    /// This class is used to load the config.xml in which the user can specify options for the logger of the program
    /// </summary>
    public static class ConfigLoader
    {
        #region static methods
        /// <summary>
        /// Set the source level for the logger
        /// </summary>
        /// <param name="text">The level set from logging_level</param>
        private static void SetSourceLevelFromNodeText(string text)
        {
            switch (text.ToLower())
            {
                case "verbose":
                    Logger.Instance.SetLogLevel(SourceLevels.Verbose); break;
                case "warning":
                    Logger.Instance.SetLogLevel(SourceLevels.Warning); break;
                case "error":
                    Logger.Instance.SetLogLevel(SourceLevels.Error); break;
                case "all":
                    Logger.Instance.SetLogLevel(SourceLevels.All); break;
                case "critical":
                    Logger.Instance.SetLogLevel(SourceLevels.Critical); break;
                case "off":
                    Logger.Instance.SetLogLevel(SourceLevels.Off); break;
            }
        }

        /// <summary>
        /// Read the nodes from config.xml
        /// </summary>
        /// <param name="node">Node to be handled</param>
        private static void ProcessNode(XmlNode node)
        {

            switch (node.Name.ToLower())
            {
                case "logging_level":
                    SetSourceLevelFromNodeText(node.InnerText);
                    break;
                case "log_path":
                    // Create new stream first to check if the file can be written to or created
                    FileStream stream = new FileStream(node.InnerText, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    Logger.Instance.RemoveListener(Logger.FILE_LOGGER_NAME);
                    Logger.Instance.AddListener(new TextWriterTraceListener(stream, Logger.FILE_LOGGER_NAME));
                    break;
            }
        }

        /// <summary>
        /// Load the config_file for the application
        /// </summary>
        /// <param name="filepath">Path to config.xml</param>
        public static void LoadConfigFile(string filepath)
        {
            XmlDocument config = new XmlDocument();
            try
            {
                config.Load(filepath);
                foreach (var node in config.DocumentElement.ChildNodes)
                {
                    if (node == null)
                    {
                        return;
                    }
                    ProcessNode((XmlNode)node);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage($"Error loading config file: {ex.Message}", TraceEventType.Error);
            }
        }
        #endregion

    }
}
