using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using PLCompliant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PLCompliantTests
{
    [TestClass]
    public class UITESTMOCK
    {
        static WindowsDriver<WindowsElement> driver;
        [TestInitialize]
        public void Setup()
        {
            // Replace with your application path
            string AppPath = @"C:\Users\natha\source\repos\PLCompliant\PLCompliant\bin\Debug\net9.0-windows\PLCompliant.exe";

            // 1. Instantiate AppiumOptions
            AppiumOptions opts = new();

            // 2. Set standard required capabilities
            opts.PlatformName = "Windows";


            // 3. Use the strongly-typed .App property (creates 'appium:app')

            // 4. THE WORKAROUND: Force the non-prefixed 'app' capability.
            // This is the key line that should resolve the 400 Bad Request error.
            opts.AddAdditionalCapability("app", AppPath);

            // 5. Initialize the driver
            driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723/"), opts);
        }

        [TestMethod]
        public void WindowsHandlesExist()
        {

            Assert.IsTrue(driver.WindowHandles != null);
            driver.CloseApp();



        }

        [TestMethod]
        public void SelectPath()
        {
            var elem = driver.FindElementByAccessibilityId("BrowseButton");
            elem.Click();
            int i = 0;
            foreach (var handle in driver.WindowHandles)
            {
                var wd = driver.SwitchTo().Window(handle);
                i = driver.WindowHandles.Count;
                WindowsElement desktop_elem = null!;
                desktop_elem = driver.FindElementByName("Start på Hurtig adgang – Skrivebord (fastgjort)") ?? driver.FindElementByName("Start on Quick Access – Desktop (pinned)");
                desktop_elem.Click();
                var submit_elem = driver.FindElementsByName("Select Folder");
                if (submit_elem.Count < 1)
                {
                    submit_elem = driver.FindElementsByName("Vælg mappe");
                }

                submit_elem[1].Click(); 

                

            }

            var chosen_path = driver.FindElementByAccessibilityId("SavePath");
            Assert.IsTrue(chosen_path.Text.Contains("Desktop") || chosen_path.Text.Contains("Skrivebord")); 
        }

        [TestMethod]
        public void ToolTipTest()
        {
            WindowsElement 
        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
            driver = null;

        }
    }
}
