using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using PLCompliant;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PLCompliantTests
{
    [TestClass]
    public class UiTest
    {
        static WindowsDriver<WindowsElement> _driver;
        [TestInitialize]
        public void Setup()
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string AppPath = userPath + "\\source\\repos\\PLCompliant\\PLCompliant\\bin\\Debug\\net9.0-windows\\PLCompliant.exe"; 

            AppiumOptions opts = new();
       
            opts.PlatformName = "Windows";
               
            opts.AddAdditionalCapability("app", AppPath);

            _driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723/"), opts);
        }

        [TestMethod]
        public void WindowsHandlesExist()
        {

            Assert.IsTrue(_driver.WindowHandles != null);



        }

        [TestMethod]
        public void SelectPath()
        {
            var elem = _driver.FindElementByAccessibilityId("BrowseButton");
            elem.Click();
            int i = 0;
            foreach (var handle in _driver.WindowHandles)
            {
                var wd = _driver.SwitchTo().Window(handle);
                i = _driver.WindowHandles.Count;
                WindowsElement desktop_elem = null!;
                desktop_elem = _driver.FindElementByName("Start på Hurtig adgang – Skrivebord (fastgjort)") ?? _driver.FindElementByName("Start on Quick Access – Desktop (pinned)");
                desktop_elem.Click();
                var submit_elem = _driver.FindElementsByName("Select Folder");
                if (submit_elem.Count < 1)
                {
                    submit_elem = _driver.FindElementsByName("Vælg mappe");
                }

                submit_elem[1].Click(); 

                

            }

            var chosen_path = _driver.FindElementByAccessibilityId("SavePath");
            Assert.IsTrue(chosen_path.Text.Contains("Desktop") || chosen_path.Text.Contains("Skrivebord")); 
        }

        [TestMethod]
        public void InsertInvalidIP()
        {
            var from_box = _driver.FindElementByAccessibilityId("FromTextBox");
            var to_box = _driver.FindElementByAccessibilityId("ToTextBox");
            from_box.SendKeys("255.255.255.256");
            from_box.SendKeys("\t");
            Thread.Sleep(1000);
            //to_box.Click(); 
            

            //var tooltipbox = _driver.FindElementByName("Ugyldig IP-addresse");
            //string actual = tooltipbox.Text;    
            //Assert.IsTrue(actual != null); 


        }

        [TestCleanup]
        public void TearDown()
        {

            _driver.Quit();
            
            _driver = null;

        }
    }
}
