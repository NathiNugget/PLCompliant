namespace PLCompliant
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            button1 = new Button();
            label1 = new Label();
            button2 = new Button();
            textBox1 = new TextBox();
            label2 = new Label();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            label3 = new Label();
            maskedTextBox1 = new MaskedTextBox();
            toolTip1 = new ToolTip(components);
            maskedTextBox2 = new MaskedTextBox();
            toolTip2 = new ToolTip(components);
            label4 = new Label();
            label5 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 12F);
            button1.Location = new Point(106, 499);
            button1.Name = "button1";
            button1.Size = new Size(75, 42);
            button1.TabIndex = 4;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += StartStopButtonClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(948, 515);
            label1.Name = "label1";
            label1.Size = new Size(102, 15);
            label1.TabIndex = 1;
            label1.Text = "xx enheder tilbage";
            label1.Visible = false;
            label1.Click += label1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(615, 383);
            button2.Margin = new Padding(3, 3, 3, 0);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 3;
            button2.Text = "Browse";
            button2.UseVisualStyleBackColor = true;
            button2.Click += ChooseSafeFilePath;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 9F);
            textBox1.Location = new Point(84, 384);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(525, 23);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged_1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(84, 366);
            label2.Name = "label2";
            label2.Size = new Size(156, 15);
            label2.TabIndex = 4;
            label2.Text = "Vælg hvor CSV skal gemmes";
            label2.Click += label2_Click;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new Point(146, 153);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(129, 19);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "Modbus TCP/IP 502";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(146, 240);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(101, 19);
            radioButton2.TabIndex = 1;
            radioButton2.TabStop = true;
            radioButton2.Text = "Siemens Step7";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(928, 209);
            label3.Name = "label3";
            label3.Size = new Size(122, 15);
            label3.TabIndex = 7;
            label3.Text = "Indtast IP-rækkevidde";
            label3.Click += label3_Click;
            // 
            // maskedTextBox1
            // 
            maskedTextBox1.Culture = new System.Globalization.CultureInfo("en-US");
            maskedTextBox1.Location = new Point(928, 252);
            maskedTextBox1.Mask = "099.099.099.099";
            maskedTextBox1.Name = "maskedTextBox1";
            maskedTextBox1.PromptChar = ' ';
            maskedTextBox1.Size = new Size(100, 23);
            maskedTextBox1.TabIndex = 5;
            maskedTextBox1.MaskInputRejected += maskedTextBox1_MaskInputRejected;
            // 
            // toolTip1
            // 
            toolTip1.Popup += toolTip1_Popup;
            // 
            // maskedTextBox2
            // 
            maskedTextBox2.Culture = new System.Globalization.CultureInfo("en-US");
            maskedTextBox2.Location = new Point(928, 336);
            maskedTextBox2.Mask = "099.099.099.099";
            maskedTextBox2.Name = "maskedTextBox2";
            maskedTextBox2.PromptChar = ' ';
            maskedTextBox2.Size = new Size(100, 23);
            maskedTextBox2.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(928, 234);
            label4.Name = "label4";
            label4.Size = new Size(23, 15);
            label4.TabIndex = 12;
            label4.Text = "Fra";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(928, 318);
            label5.Name = "label5";
            label5.Size = new Size(20, 15);
            label5.TabIndex = 13;
            label5.Text = "Til";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1264, 681);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(maskedTextBox2);
            Controls.Add(maskedTextBox1);
            Controls.Add(label3);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private TextBox textBox1;
        private Label label2;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label label3;
        private MaskedTextBox maskedTextBox1;
        private ToolTip toolTip1;
        private MaskedTextBox maskedTextBox2;
        private ToolTip toolTip2;
        private Label label4;
        private Label label5;
        public Label label1;
    }
}
