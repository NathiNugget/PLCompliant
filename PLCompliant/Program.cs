using PLCompliant.Config;
using PLCompliant.Events;
using PLCompliant.Utilities;

using System.Diagnostics.CodeAnalysis;
using System.Text;


namespace PLCompliant
{
    [ExcludeFromCodeCoverage]
    internal static class Programx
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            ConfigLoader.LoadConfigFile("./config.xml");

            foreach (var file in Directory.GetFiles("./Locales/"))
            {
                using (var reader = new StreamReader(file))
                {
                    StringBuilder sb = new StringBuilder(4000);
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        sb.AppendLine(reader.ReadLine());
                    }
                    string s = sb.ToString();

                }
            }

            UpdateThreadContext context = new UpdateThreadContext();

            Thread updateThread = ThreadUtilities.CreateBackgroundThread(() =>
            {
                while (!GlobalVars.ABORT)
                {
                    while (!UpdateEventQueue.Instance.Empty)
                    {
                        if (UpdateEventQueue.Instance.TryPop(out var evt))
                        {
                            evt.ExecuteEvent(context);
                        }
                    }
                    Thread.Sleep(100);
                }

            });
            updateThread.Start();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            ApplicationConfiguration.Initialize();
            Application.Run(new PLCompliantUI());
        }
    }
}