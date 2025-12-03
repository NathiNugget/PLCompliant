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
            IPInstructionLabel = new Label();
            FromTextBox = new MaskedTextBox();
            Tooltip = new ToolTip(components);
            ToTextBox = new MaskedTextBox();
            FromLabel = new Label();
            ToLabel = new Label();
            ProtocolInstruction = new Label();
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
            CurrentStateLabel.AutoSize = true;
            CurrentStateLabel.Location = new Point(920, 515);
            CurrentStateLabel.MaximumSize = new Size(300, 400);
            CurrentStateLabel.Name = "CurrentStateLabel";
            CurrentStateLabel.Size = new Size(92, 15);
            CurrentStateLabel.TabIndex = 1;
            CurrentStateLabel.Text = "Starter scanning";
            CurrentStateLabel.Visible = false;
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
            SaveFileInstruction.Location = new Point(84, 403);
            SaveFileInstruction.Name = "SaveFileInstruction";
            SaveFileInstruction.Size = new Size(156, 15);
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
            // IPInstructionLabel
            // 
            IPInstructionLabel.AutoSize = true;
            IPInstructionLabel.Location = new Point(920, 110);
            IPInstructionLabel.Name = "IPInstructionLabel";
            IPInstructionLabel.Size = new Size(122, 15);
            IPInstructionLabel.TabIndex = 7;
            IPInstructionLabel.Text = "Indtast IP-rækkevidde";
            // 
            // FromTextBox
            // 
            FromTextBox.Culture = new System.Globalization.CultureInfo("en-US");
            FromTextBox.Location = new Point(920, 152);
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
            ToTextBox.Location = new Point(920, 239);
            ToTextBox.Mask = "099.099.099.099";
            ToTextBox.Name = "ToTextBox";
            ToTextBox.PromptChar = ' ';
            ToTextBox.Size = new Size(100, 23);
            ToTextBox.TabIndex = 6;
            // 
            // FromLabel
            // 
            FromLabel.AutoSize = true;
            FromLabel.Location = new Point(920, 134);
            FromLabel.Name = "FromLabel";
            FromLabel.Size = new Size(23, 15);
            FromLabel.TabIndex = 12;
            FromLabel.Text = "Fra";
            // 
            // ToLabel
            // 
            ToLabel.AutoSize = true;
            ToLabel.Location = new Point(920, 221);
            ToLabel.Name = "ToLabel";
            ToLabel.Size = new Size(20, 15);
            ToLabel.TabIndex = 13;
            ToLabel.Text = "Til";
            // 
            // ProtocolInstruction
            // 
            ProtocolInstruction.AutoSize = true;
            ProtocolInstruction.Location = new Point(146, 110);
            ProtocolInstruction.Name = "ProtocolInstruction";
            ProtocolInstruction.Size = new Size(81, 15);
            ProtocolInstruction.TabIndex = 14;
            ProtocolInstruction.Text = "Vælg protokol";
            // 
            // PLCompliantUI
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1264, 681);
            Controls.Add(ProtocolInstruction);
            Controls.Add(ToLabel);
            Controls.Add(FromLabel);
            Controls.Add(ToTextBox);
            Controls.Add(FromTextBox);
            Controls.Add(IPInstructionLabel);
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
        private Label IPInstructionLabel;
        private MaskedTextBox FromTextBox;
        private MaskedTextBox ToTextBox;
        private Label FromLabel;
        public Label CurrentStateLabel;
        public TextBox SavePath;
        private Label ToLabel;
        private Label ProtocolInstruction;
        public Button BrowseButton;
        public ToolTip Tooltip;
    }
}
