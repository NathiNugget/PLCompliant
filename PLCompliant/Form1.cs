using PLCompliant.Utilities;
using System.ComponentModel;
using System.Net;

namespace PLCompliant
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            maskedTextBox1.LostFocus += new EventHandler(IPAddressValidationHandling);
            maskedTextBox1.MouseClick += new MouseEventHandler(IPAddressOnClick);
            maskedTextBox1.KeyDown += new KeyEventHandler(ControlField);

            maskedTextBox2.LostFocus += new EventHandler(IPAddressValidationHandling);
            maskedTextBox2.MouseClick += new MouseEventHandler(IPAddressOnClick);
            maskedTextBox2.KeyDown += new KeyEventHandler(ControlField);




        }

        private void ControlField(object? sender, KeyEventArgs e)
        {
            MaskedTextBox textbox = (MaskedTextBox)sender!;
            int index = textbox.SelectionStart;

            if (e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Oemcomma || e.KeyCode == Keys.Right)
            {

                if (index < textbox.Text.Length && (textbox.Text.ToCharArray().ToList().IndexOf('.', index) != -1 && textbox.Text.ToCharArray()[index+1] == ' '))
                {

                    int nextindex = textbox.Text.IndexOf('.', index);

                    if (nextindex != -1) textbox.Select(nextindex, 0);
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (index > textbox.Text.Length)
                {
                    int tosubtract = index - textbox.Text.Length;
                    index -= tosubtract; 
                }
                if (index != 0 && (textbox.Text.ToCharArray()[index - 1] == ' ' || textbox.Text.ToCharArray()[index - 1] == '.'))
                {
                    int nextindex = textbox.Text.PreviousIndexOf('.', index);

                    if (nextindex != -1)
                    {
                        textbox.Select(nextindex, 0);
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




            if (!IPAddress.TryParse(input, out IPAddress? address))
            {
                toolTip1.ToolTipTitle = "Dårlig IP";
                toolTip1.Show("Du har indtastet en ikke-valid IP-addresse", maskedTextBox1, 0, -40, 5000);
            }
            Console.WriteLine(address);

        }

        private void button1_Click(object sender, EventArgs e)
        {

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
            Console.WriteLine(size); // <-- Shows file size in debugging mode.
            Console.WriteLine(result); // <-- For debugging use.

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
    }
}
