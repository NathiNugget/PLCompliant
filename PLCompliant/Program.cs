using PLCompliant.Config;
using PLCompliant.Events;
using PLCompliant.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace PLCompliant
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            ConfigLoader.LoadConfigFile("./config.xml");

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
            Application.Run(new Form1());
        }
    }
}