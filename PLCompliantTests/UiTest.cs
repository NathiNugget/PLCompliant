using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using PLCompliant.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace PLCompliantTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UiTest
    {
        static WindowsDriver<WindowsElement> _driver;
        [TestInitialize]
        public void Setup()
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string AppPath = Path.Combine(userPath, "source\\repos\\PLCompliant\\PLCompliant\\bin\\Debug\\net9.0-windows\\PLCompliant.exe");

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

            foreach (var handle in _driver.WindowHandles)
            {
                var wd = _driver.SwitchTo().Window(handle);

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
            var modbus_button = _driver.FindElementByAccessibilityId("ModbusButton");
            from_box.SendKeys("255.255.255.256");
            modbus_button.Click();
            Thread.Sleep(1000);



            //var tooltipbox = _driver.FindElementByName("Ugyldig IP-addresse");
            //string actual = tooltipbox.Text;    
            //Assert.IsTrue(actual != null); 
        }

        [TestMethod]
        [DataRow("192.168.123.100", "192.168.123.100")]
        [DataRow("192.168.123.100", "192.168.126.254")]

        public void MakeValidScan(string from, string to)
        {
            var from_box = _driver.FindElementByAccessibilityId("FromTextBox");
            var to_box = _driver.FindElementByAccessibilityId("ToTextBox");
            var modbus_button = _driver.FindElementByAccessibilityId("ModbusButton");
            var startstop_button = _driver.FindElementByAccessibilityId("StartStopButton");
            var current_state_label = _driver.FindElementByAccessibilityId("CurrentStateLabel");
            var chosen_path = _driver.FindElementByAccessibilityId("SavePath");
            var browse_button = _driver.FindElementByAccessibilityId("BrowseButton");


            modbus_button.Click();
            from_box.SendKeys(from);
            to_box.SendKeys(to);
            browse_button.Click();

            foreach (var handle in _driver.WindowHandles)
            {
                var wd = _driver.SwitchTo().Window(handle);

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

            startstop_button.Click();
            Thread.Sleep(2000);

            DateTime timestamp = DateTime.Now;
            timestamp.AddSeconds(-2);

            string expected = current_state_label.Text;
            string actual = $"Resultat gemt i {chosen_path.Text}, fil navngivet ModbusResultat{timestamp.ToString(GlobalVars.CustomFormat)}.csv";
            int index_of_resulatat = expected.IndexOf("ModbusResultat") + "ModbusResultat".Length;
            string expected_substring = expected.Substring(0, index_of_resulatat);
            string actual_substring = actual.Substring(0, index_of_resulatat);


            Assert.AreEqual(expected_substring, actual_substring);



        }

        [TestMethod]

        [DataRow("192.168.123.100", "192.168.127.78")]
        [DataRow("192.168.123.100", "192.168.255.100")]


        public void MakeValidScanWithBigRange(string from, string to)
        {
            var from_box = _driver.FindElementByAccessibilityId("FromTextBox");
            var to_box = _driver.FindElementByAccessibilityId("ToTextBox");
            var modbus_button = _driver.FindElementByAccessibilityId("ModbusButton");
            var startstop_button = _driver.FindElementByAccessibilityId("StartStopButton");
            var current_state_label = _driver.FindElementByAccessibilityId("CurrentStateLabel");
            var chosen_path = _driver.FindElementByAccessibilityId("SavePath");
            var browse_button = _driver.FindElementByAccessibilityId("BrowseButton");


            modbus_button.Click();
            from_box.SendKeys(from);
            to_box.SendKeys(to);
            browse_button.Click();

            foreach (var handle in _driver.WindowHandles)
            {
                var wd = _driver.SwitchTo().Window(handle);

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

            startstop_button.Click();

            var ok_button = _driver.FindElementByName("OK");
            ok_button.Click();


            Thread.Sleep(15000);

            DateTime timestamp = DateTime.Now;
            timestamp.AddSeconds(-15);

            string expected = current_state_label.Text;
            string actual = $"Resultat gemt i {chosen_path.Text}, fil navngivet ModbusResultat{timestamp.ToString(GlobalVars.CustomFormat)}.csv";
            int index_of_resulatat = expected.IndexOf("ModbusResultat") + "ModbusResultat".Length;
            string expected_substring = expected.Substring(0, index_of_resulatat);
            string actual_substring = actual.Substring(0, index_of_resulatat);


            Assert.AreEqual(expected_substring, actual_substring);



        }


        [TestMethod]
        [DataRow("192.168.124.100", "192.168.130.78")]
        [DataRow("192.168.130.78", "192.168.140.1")]


        public void MakeScanWithNoPLCsFound(string from, string to)
        {
            var from_box = _driver.FindElementByAccessibilityId("FromTextBox");
            var to_box = _driver.FindElementByAccessibilityId("ToTextBox");
            var modbus_button = _driver.FindElementByAccessibilityId("ModbusButton");
            var startstop_button = _driver.FindElementByAccessibilityId("StartStopButton");
            var current_state_label = _driver.FindElementByAccessibilityId("CurrentStateLabel");
            var chosen_path = _driver.FindElementByAccessibilityId("SavePath");
            var browse_button = _driver.FindElementByAccessibilityId("BrowseButton");


            modbus_button.Click();
            from_box.SendKeys(from);
            to_box.SendKeys(to);
            browse_button.Click();

            foreach (var handle in _driver.WindowHandles)
            {
                var wd = _driver.SwitchTo().Window(handle);

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

            startstop_button.Click();

            var ok_button = _driver.FindElementByName("OK");
            ok_button.Click();


            Thread.Sleep(4000); // Wait for the program to do its thing
            var warning = _driver.FindElementByName("Ingen PLC Addresser fundet på Modbus protokol!");
            Assert.IsNotNull(warning);
            ok_button = _driver.FindElementByName("OK");
            ok_button.Click();





        }

        [TestMethod]
        [DataRow("192.168.124.100", "192.168.130.78")]
        public void CancelBigScan(string from, string to)
        {
            var from_box = _driver.FindElementByAccessibilityId("FromTextBox");
            var to_box = _driver.FindElementByAccessibilityId("ToTextBox");
            var modbus_button = _driver.FindElementByAccessibilityId("ModbusButton");
            var startstop_button = _driver.FindElementByAccessibilityId("StartStopButton");
            var current_state_label = _driver.FindElementByAccessibilityId("CurrentStateLabel");
            var chosen_path = _driver.FindElementByAccessibilityId("SavePath");
            var browse_button = _driver.FindElementByAccessibilityId("BrowseButton");


            modbus_button.Click();
            from_box.SendKeys(from);
            to_box.SendKeys(to);
            browse_button.Click();

            foreach (var handle in _driver.WindowHandles)
            {
                var wd = _driver.SwitchTo().Window(handle);

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

            startstop_button.Click();

            var cancel_button = _driver.FindElementByName("Annuller") ?? _driver.FindElementByName("Cancel");

            cancel_button.Click();
            string expected = "Afventer brugerens instruks";
            string actual = current_state_label.Text;
            Assert.AreEqual(expected, actual);


        }

        [TestCleanup]
        public void TearDown()
        {

            _driver.Quit();

            _driver = null;

        }
    }
}
