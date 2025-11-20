using PLCompliant.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace PLCompliant
{
    [ExcludeFromCodeCoverage]
    public partial class Form1 : Form
    {
        bool running;
        System.Windows.Forms.Timer _timer;
        Queue<int> _queue;
        public Form1()
        {
            running = false;
            InitializeComponent();
            maskedTextBox1.LostFocus += new EventHandler(IPAddressValidationHandling!);
            maskedTextBox1.MouseClick += new MouseEventHandler(IPAddressOnClick);
            maskedTextBox1.KeyDown += new KeyEventHandler(ControlField);

            maskedTextBox2.LostFocus += new EventHandler(IPAddressValidationHandling!);
            maskedTextBox2.MouseClick += new MouseEventHandler(IPAddressOnClick);
            maskedTextBox2.KeyDown += new KeyEventHandler(ControlField);


            _timer = new System.Windows.Forms.Timer();
            _timer.Tick += new EventHandler(HandleScannerEvent!); //TODO: Make actual eventhandler for ticks when queue is added; 
            _timer.Interval = 100;
            _timer.Start();
            _queue = new Queue<int>(Enumerable.Range(0, 100));
        }

        private void Form1_Load(object sender, EventArgs e)
        {



        }

        private void HandleScannerEvent(object? sender, EventArgs args)
        {
            label1.Text = _queue.Count.ToString();
            if (_queue.Count > 0) _queue.Dequeue();
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
                    int previousindex = text.PreviousIndexOf('.', index);

                    if (previousindex != -1)
                    {
                        textbox.Select(previousindex, 0);
                    }
                    else textbox.Select(0, 0);
                }
            }
        }

        private void IPAddressOnClick(object? sender, EventArgs e)
        {
            MaskedTextBox textbox = (MaskedTextBox)sender!;
            if (textbox.Text == "   .   .   .")
            {
                textbox.Select(0, 0);
            }



        }

        private void IPAddressValidationHandling(object sender, EventArgs e)
        {
            MaskedTextBox maskedTextBox = (MaskedTextBox)sender!;
            string input = maskedTextBox.Text.Replace(" ", "");

            


            if (!IPAddress.TryParse(input, out IPAddress? _))
            {
                toolTip1.ToolTipTitle = "Dårlig IP";
                toolTip1.Show("Du har indtastet en ikke-valid IP-addresse. Tal må ikke over 255, og der skal være et før og efter hvert punktum", maskedTextBox, 0, -40, 5000);
                
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.Text = running ? "Start" : "Stop";
            label1.Visible = !label1.Visible;
            running = !running;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int size = -1;
            FolderBrowserDialog openFolderDialog1 = new FolderBrowserDialog();
            DialogResult result = openFolderDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string folderpath = openFolderDialog1.SelectedPath;


                textBox1.Text = folderpath;

            }


        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

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
    }
}
