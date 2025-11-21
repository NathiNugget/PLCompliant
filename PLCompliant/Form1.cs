using PLCompliant.Enums;
using PLCompliant.Events;
using PLCompliant.Scanning;
using PLCompliant.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace PLCompliant
{
    [ExcludeFromCodeCoverage]
    public partial class Form1 : Form
    {
        //TODO: DELETE running field; 
        /// <summary>
        /// instance field used while testing out capabilities of timer
        /// </summary>
        bool running;
        System.Windows.Forms.Timer _timer;
        public PLCProtocolType Protocol { get; private set; }
        public Form1()
        {

            running = false;
            InitializeComponent();
            maskedTextBox1.LostFocus += new EventHandler(IPAddressValidationHandling!);
            maskedTextBox1.MouseClick += new MouseEventHandler(MaskedTextBoxOnClick);
            maskedTextBox1.KeyDown += new KeyEventHandler(ControlField);

            maskedTextBox2.LostFocus += new EventHandler(IPAddressValidationHandling!);
            maskedTextBox2.MouseClick += new MouseEventHandler(MaskedTextBoxOnClick);
            maskedTextBox2.KeyDown += new KeyEventHandler(ControlField);


            radioButton1.MouseClick += new MouseEventHandler(CheckIfButtonIsPressed);
            radioButton2.MouseClick += new MouseEventHandler(CheckIfButtonIsPressed);


            _timer = new System.Windows.Forms.Timer();
            _timer.Tick += new EventHandler(UIOnTick!); //TODO: Make actual eventhandler for ticks when queue is added; 
            _timer.Interval = 100;
            _timer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {



        }

        private void UIOnTick(object? sender, EventArgs args)
        {
            UIEventQueue queue = UIEventQueue.Instance;
            while (!queue.Empty)
            {
                if (queue.TryPop(out var evt))
                {
                    evt.ExecuteEvent(this);

                }
            }


        }




        private void ControlField(object? sender, KeyEventArgs e)
        {
            MaskedTextBox textbox = (MaskedTextBox)sender!;
            int index = textbox.SelectionStart;
            string text = textbox.Text;
            int textlength = textbox.Text.Length;
            char[] chararr = textbox.Text.ToCharArray();
            List<char> charlist = chararr.ToList();

            if (e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Oemcomma || e.KeyCode == Keys.Right)
            {
                var range = charlist.GetRange(index, charlist.Count - index);
                if (range.Count == 0)
                {
                    int nextindex = text.LastIndexOf('.');

                    if (nextindex != -1) textbox.Select(nextindex, 0);
                }

                else if (index < textlength && (charlist.IndexOf('.', index) != -1 &&
                    charlist.GetRange(index, charlist.Count - index).TakeWhile(x => x != ' ').ToList().Count != 1))
                {

                    int nextindex = text.IndexOf('.', index);

                    if (nextindex != -1) textbox.Select(nextindex, 0);
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (index > textlength)
                {
                    int tosubtract = index - textlength;
                    index -= tosubtract;
                }
                if (index != 0 && (chararr[index - 1] == ' ' || chararr[index - 1] == '.'))
                {
                    int previousindex = text.PreviousIndexOfAndFixToSeparator('.', index);

                    if (previousindex != -1)
                    {
                        textbox.Select(previousindex, 0);
                    }
                    else textbox.Select(0, 0);
                }
            }
        }

        private void MaskedTextBoxOnClick(object? sender, EventArgs e)
        {
            MaskedTextBox box = (MaskedTextBox)sender!;
            //Console.WriteLine(box.Location.X);
            //Console.WriteLine(e);
            // TODO: Make logic about where they click maybe

            int index = box.Text.IndexOf("   ");

            if (index == -1) box.Select(box.Text.LastIndexOf(".") + 1, 0); else box.Select(index, 0);




        }

        private void IPAddressValidationHandling(object sender, EventArgs e)
        {
            MaskedTextBox maskedTextBox = (MaskedTextBox)sender!;
            string input = maskedTextBox.Text.Replace(" ", "");




            if (!IPAddress.TryParse(input, out IPAddress? _))
            {
                ShowIPWarning(maskedTextBox);


            }



        }

        private void ShowIPWarning(object sender, string title = "Ugyldig IP-addresse", string msg = "Du har indtastet en ikke-valid IP-addresse. Tal må ikke over 255, og der skal være tal før og efter hvert punktum")
        {




            toolTip1.ToolTipTitle = title;
            toolTip1.Show(msg, (IWin32Window)sender, 0, -40, 5000);
        }

        private void StartStopButtonClick(object sender, EventArgs e)
        {
            if (ValidateRange(maskedTextBox1, maskedTextBox2, out IPAddress from, out IPAddress to))
            {
                var addrRange = new IPAddressRange(from, to);
                UpdateEventQueue.Instance.Push(new UpdateStartViableIPScan(new UpdateThreadArgs(addrRange)));
                label1.Visible = !label1.Visible;
                running = !running;
            }
            else
            {
                ShowIPWarning(button1, "Ugyldig indtastning", "Du skal skrive to gyldige IPv4-addresser");
            }


            //if (ValidateRange(maskedTextBox1, maskedTextBox2, out IPAddress from, out IPAddress to))
            //{
            //    Button button = (Button)sender;
            //    button.Text = running ? "Start" : "Stop";
            //    label1.Visible = !label1.Visible;
            //    Thread t = new Thread(() =>
            //    {
            //        if (!running)
            //        {


            //            IPAddressRange range = new IPAddressRange(from, to);
            //            NetworkScanner scanner = new NetworkScanner(range);
            //            scanner.FindIPs();
            //        }



            //        running = !running;
            //    }); 
            //    t.Start();




            //}


            //else
            //{

            //    ShowIPWarning(button1, "Ugyldig indtastning", "Du skal skrive to gyldige IPv4-addresser");

            //}

        }

        private bool ValidateRange(MaskedTextBox maskedTextBox1, MaskedTextBox maskedTextBox2, out IPAddress? from, out IPAddress? to)
        {

            string addr1 = maskedTextBox1.Text.Replace(" ", "");
            string addr2 = maskedTextBox2.Text.Replace(" ", "");
            if (IPAddress.TryParse(addr1, out IPAddress? left) && IPAddress.TryParse(addr2, out IPAddress? right))
            {
                from = left;
                to = right;
                return true;
            }




            from = null!;
            to = null!;
            return false;
        }





        private void ChooseSafeFilePath(object sender, EventArgs e)
        {
            FolderBrowserDialog openFolderDialog1 = new FolderBrowserDialog();
            DialogResult result = openFolderDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderpath = openFolderDialog1.SelectedPath;


                textBox1.Text = folderpath;

            }


        }



        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {


        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CheckIfButtonIsPressed(object? sender, EventArgs e)
        {
            if (sender == null) return;
            RadioButton button = (RadioButton)sender;
            if (button.Checked)
            {
                Protocol = button.TabIndex == 0 ? PLCProtocolType.Modbus : PLCProtocolType.Step_7;
            }
            Console.WriteLine((int)Protocol);

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
