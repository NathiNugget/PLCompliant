namespace PLCompliant
{
    partial class PLCompliantUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PLCompliantUI));
            StartStopButton = new Button();
            CurrentStateLabel = new Label();
            BrowseButton = new Button();
            SavePath = new TextBox();
            SaveFileInstruction = new Label();
            ModbusButton = new RadioButton();
            Step7Button = new RadioButton();
            FromTextBox = new MaskedTextBox();
            Tooltip = new ToolTip(components);
            ToTextBox = new MaskedTextBox();
            FromLabel = new Label();
            ToLabel = new Label();
            ProtocolInstruction = new Label();
            logTextBox = new TextBox();
            logLabel = new Label();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // StartStopButton
            // 
            StartStopButton.Font = new Font("Segoe UI", 12F);
            StartStopButton.Location = new Point(106, 499);
            StartStopButton.Name = "StartStopButton";
            StartStopButton.Size = new Size(75, 42);
            StartStopButton.TabIndex = 4;
            StartStopButton.Text = "Start";
            StartStopButton.UseVisualStyleBackColor = true;
            StartStopButton.Click += StartStopButtonClick;
            // 
            // CurrentStateLabel
            // 
            CurrentStateLabel.AccessibleDescription = "";
            CurrentStateLabel.AccessibleName = "";
            CurrentStateLabel.AutoSize = true;
            CurrentStateLabel.Location = new Point(615, 526);
            CurrentStateLabel.MaximumSize = new Size(300, 400);
            CurrentStateLabel.Name = "CurrentStateLabel";
            CurrentStateLabel.Size = new Size(133, 15);
            CurrentStateLabel.TabIndex = 1;
            CurrentStateLabel.Text = "After brugerens instruks";
            CurrentStateLabel.TextChanged += CurrentStateLabel_TextChanged;
            // 
            // BrowseButton
            // 
            BrowseButton.Location = new Point(615, 421);
            BrowseButton.Margin = new Padding(3, 3, 3, 0);
            BrowseButton.Name = "BrowseButton";
            BrowseButton.Size = new Size(75, 23);
            BrowseButton.TabIndex = 3;
            BrowseButton.Text = "Browse";
            BrowseButton.UseVisualStyleBackColor = true;
            BrowseButton.Click += ChooseSaveFilePath;
            // 
            // SavePath
            // 
            SavePath.Font = new Font("Segoe UI", 9F);
            SavePath.Location = new Point(84, 421);
            SavePath.Name = "SavePath";
            SavePath.Size = new Size(525, 23);
            SavePath.TabIndex = 2;
            // 
            // SaveFileInstruction
            // 
            SaveFileInstruction.AutoSize = true;
            SaveFileInstruction.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SaveFileInstruction.Location = new Point(84, 381);
            SaveFileInstruction.Name = "SaveFileInstruction";
            SaveFileInstruction.Size = new Size(274, 30);
            SaveFileInstruction.TabIndex = 4;
            SaveFileInstruction.Text = "Vælg hvor CSV skal gemmes";
            // 
            // ModbusButton
            // 
            ModbusButton.AutoSize = true;
            ModbusButton.Checked = true;
            ModbusButton.Location = new Point(146, 153);
            ModbusButton.Name = "ModbusButton";
            ModbusButton.Size = new Size(139, 19);
            ModbusButton.TabIndex = 0;
            ModbusButton.TabStop = true;
            ModbusButton.Text = "Modbus TCP Port 502";
            ModbusButton.UseVisualStyleBackColor = true;
            ModbusButton.CheckedChanged += ModbusButtonCheck;
            // 
            // Step7Button
            // 
            Step7Button.AutoSize = true;
            Step7Button.Location = new Point(146, 240);
            Step7Button.Name = "Step7Button";
            Step7Button.Size = new Size(147, 19);
            Step7Button.TabIndex = 1;
            Step7Button.Text = "Siemens Step7 Port 102";
            Step7Button.UseVisualStyleBackColor = true;
            Step7Button.CheckedChanged += Step7ButtonCheck;
            // 
            // FromTextBox
            // 
            FromTextBox.Culture = new System.Globalization.CultureInfo("en-US");
            FromTextBox.Location = new Point(615, 153);
            FromTextBox.Mask = "099.099.099.099";
            FromTextBox.Name = "FromTextBox";
            FromTextBox.PromptChar = ' ';
            FromTextBox.Size = new Size(100, 23);
            FromTextBox.TabIndex = 5;
            // 
            // Tooltip
            // 
            Tooltip.AutoPopDelay = 5000;
            Tooltip.InitialDelay = 0;
            Tooltip.ReshowDelay = 100;
            // 
            // ToTextBox
            // 
            ToTextBox.Culture = new System.Globalization.CultureInfo("en-US");
            ToTextBox.Location = new Point(615, 240);
            ToTextBox.Mask = "099.099.099.099";
            ToTextBox.Name = "ToTextBox";
            ToTextBox.PromptChar = ' ';
            ToTextBox.Size = new Size(100, 23);
            ToTextBox.TabIndex = 6;
            // 
            // FromLabel
            // 
            FromLabel.AutoSize = true;
            FromLabel.Location = new Point(615, 135);
            FromLabel.Name = "FromLabel";
            FromLabel.Size = new Size(23, 15);
            FromLabel.TabIndex = 12;
            FromLabel.Text = "Fra";
            // 
            // ToLabel
            // 
            ToLabel.AutoSize = true;
            ToLabel.Location = new Point(615, 222);
            ToLabel.Name = "ToLabel";
            ToLabel.Size = new Size(20, 15);
            ToLabel.TabIndex = 13;
            ToLabel.Text = "Til";
            // 
            // ProtocolInstruction
            // 
            ProtocolInstruction.AutoSize = true;
            ProtocolInstruction.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ProtocolInstruction.Location = new Point(146, 88);
            ProtocolInstruction.Name = "ProtocolInstruction";
            ProtocolInstruction.Size = new Size(141, 30);
            ProtocolInstruction.TabIndex = 14;
            ProtocolInstruction.Text = "Vælg protokol";
            // 
            // logTextBox
            // 
            logTextBox.Location = new Point(938, 135);
            logTextBox.Margin = new Padding(2);
            logTextBox.Multiline = true;
            logTextBox.Name = "logTextBox";
            logTextBox.ReadOnly = true;
            logTextBox.ScrollBars = ScrollBars.Vertical;
            logTextBox.Size = new Size(259, 406);
            logTextBox.TabIndex = 15;
            // 
            // logLabel
            // 
            logLabel.AutoSize = true;
            logLabel.Font = new Font("Segoe UI", 25.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            logLabel.Location = new Point(938, 75);
            logLabel.Margin = new Padding(2, 0, 2, 0);
            logLabel.Name = "logLabel";
            logLabel.Size = new Size(78, 47);
            logLabel.TabIndex = 16;
            logLabel.Text = "Log";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(615, 88);
            label1.Name = "label1";
            label1.Size = new Size(214, 30);
            label1.TabIndex = 7;
            label1.Text = "Indtast IP-rækkevidde";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(615, 496);
            label2.Name = "label2";
            label2.Size = new Size(147, 30);
            label2.TabIndex = 17;
            label2.Text = "Programstatus";
            // 
            // PLCompliantUI
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1264, 681);
            Controls.Add(label2);
            Controls.Add(logLabel);
            Controls.Add(logTextBox);
            Controls.Add(ProtocolInstruction);
            Controls.Add(ToLabel);
            Controls.Add(FromLabel);
            Controls.Add(ToTextBox);
            Controls.Add(FromTextBox);
            Controls.Add(label1);
            Controls.Add(Step7Button);
            Controls.Add(ModbusButton);
            Controls.Add(SaveFileInstruction);
            Controls.Add(SavePath);
            Controls.Add(BrowseButton);
            Controls.Add(CurrentStateLabel);
            Controls.Add(StartStopButton);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "PLCompliantUI";
            Text = "PLCompliant";
            Load += PLCompliantUI_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button StartStopButton;
        private Label SaveFileInstruction;
        private RadioButton ModbusButton;
        private RadioButton Step7Button;
        private MaskedTextBox FromTextBox;
        private MaskedTextBox ToTextBox;
        private Label FromLabel;
        public Label CurrentStateLabel;
        public TextBox SavePath;
        private Label ToLabel;
        private Label ProtocolInstruction;
        public Button BrowseButton;
        public ToolTip Tooltip;
        public TextBox logTextBox;
        private Label logLabel;
        private Label label1;
        private Label label2;
    }
}
