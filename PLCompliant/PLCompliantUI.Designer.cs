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
            logTextBox = new TextBox();
            logLabel = new Label();
            SuspendLayout();
            // 
            // StartStopButton
            // 
            StartStopButton.Font = new Font("Segoe UI", 12F);
            StartStopButton.Location = new Point(132, 624);
            StartStopButton.Margin = new Padding(4);
            StartStopButton.Name = "StartStopButton";
            StartStopButton.Size = new Size(94, 52);
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
            CurrentStateLabel.Location = new Point(748, 656);
            CurrentStateLabel.Margin = new Padding(4, 0, 4, 0);
            CurrentStateLabel.MaximumSize = new Size(375, 500);
            CurrentStateLabel.Name = "CurrentStateLabel";
            CurrentStateLabel.Size = new Size(115, 20);
            CurrentStateLabel.TabIndex = 1;
            CurrentStateLabel.Text = "After brugerens instruks";
            CurrentStateLabel.TextChanged += CurrentStateLabel_TextChanged;
            // 
            // BrowseButton
            // 
            BrowseButton.Location = new Point(769, 526);
            BrowseButton.Margin = new Padding(4, 4, 4, 0);
            BrowseButton.Name = "BrowseButton";
            BrowseButton.Size = new Size(94, 29);
            BrowseButton.TabIndex = 3;
            BrowseButton.Text = "Browse";
            BrowseButton.UseVisualStyleBackColor = true;
            BrowseButton.Click += ChooseSaveFilePath;
            // 
            // SavePath
            // 
            SavePath.Font = new Font("Segoe UI", 9F);
            SavePath.Location = new Point(105, 526);
            SavePath.Margin = new Padding(4);
            SavePath.Name = "SavePath";
            SavePath.Size = new Size(655, 27);
            SavePath.TabIndex = 2;
            // 
            // SaveFileInstruction
            // 
            SaveFileInstruction.AutoSize = true;
            SaveFileInstruction.Location = new Point(105, 504);
            SaveFileInstruction.Margin = new Padding(4, 0, 4, 0);
            SaveFileInstruction.Name = "SaveFileInstruction";
            SaveFileInstruction.Size = new Size(195, 20);
            SaveFileInstruction.TabIndex = 4;
            SaveFileInstruction.Text = "Vælg hvor CSV skal gemmes";
            // 
            // ModbusButton
            // 
            ModbusButton.AutoSize = true;
            ModbusButton.Checked = true;
            ModbusButton.Location = new Point(182, 191);
            ModbusButton.Margin = new Padding(4);
            ModbusButton.Name = "ModbusButton";
            ModbusButton.Size = new Size(170, 24);
            ModbusButton.TabIndex = 0;
            ModbusButton.TabStop = true;
            ModbusButton.Text = "Modbus TCP Port 502";
            ModbusButton.UseVisualStyleBackColor = true;
            ModbusButton.CheckedChanged += ModbusButtonCheck;
            // 
            // Step7Button
            // 
            Step7Button.AutoSize = true;
            Step7Button.Location = new Point(182, 300);
            Step7Button.Margin = new Padding(4);
            Step7Button.Name = "Step7Button";
            Step7Button.Size = new Size(185, 24);
            Step7Button.TabIndex = 1;
            Step7Button.Text = "Siemens Step7 Port 102";
            Step7Button.UseVisualStyleBackColor = true;
            Step7Button.CheckedChanged += Step7ButtonCheck;
            // 
            // IPInstructionLabel
            // 
            IPInstructionLabel.AutoSize = true;
            IPInstructionLabel.Location = new Point(769, 139);
            IPInstructionLabel.Margin = new Padding(4, 0, 4, 0);
            IPInstructionLabel.Name = "IPInstructionLabel";
            IPInstructionLabel.Size = new Size(152, 20);
            IPInstructionLabel.TabIndex = 7;
            IPInstructionLabel.Text = "Indtast IP-rækkevidde";
            // 
            // FromTextBox
            // 
            FromTextBox.Culture = new System.Globalization.CultureInfo("en-US");
            FromTextBox.Location = new Point(769, 191);
            FromTextBox.Margin = new Padding(4);
            FromTextBox.Mask = "099.099.099.099";
            FromTextBox.Name = "FromTextBox";
            FromTextBox.PromptChar = ' ';
            FromTextBox.Size = new Size(124, 27);
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
            ToTextBox.Location = new Point(769, 300);
            ToTextBox.Margin = new Padding(4);
            ToTextBox.Mask = "099.099.099.099";
            ToTextBox.Name = "ToTextBox";
            ToTextBox.PromptChar = ' ';
            ToTextBox.Size = new Size(124, 27);
            ToTextBox.TabIndex = 6;
            // 
            // FromLabel
            // 
            FromLabel.AutoSize = true;
            FromLabel.Location = new Point(769, 169);
            FromLabel.Margin = new Padding(4, 0, 4, 0);
            FromLabel.Name = "FromLabel";
            FromLabel.Size = new Size(29, 20);
            FromLabel.TabIndex = 12;
            FromLabel.Text = "Fra";
            // 
            // ToLabel
            // 
            ToLabel.AutoSize = true;
            ToLabel.Location = new Point(769, 277);
            ToLabel.Margin = new Padding(4, 0, 4, 0);
            ToLabel.Name = "ToLabel";
            ToLabel.Size = new Size(25, 20);
            ToLabel.TabIndex = 13;
            ToLabel.Text = "Til";
            // 
            // ProtocolInstruction
            // 
            ProtocolInstruction.AutoSize = true;
            ProtocolInstruction.Location = new Point(182, 138);
            ProtocolInstruction.Margin = new Padding(4, 0, 4, 0);
            ProtocolInstruction.Name = "ProtocolInstruction";
            ProtocolInstruction.Size = new Size(103, 20);
            ProtocolInstruction.TabIndex = 14;
            ProtocolInstruction.Text = "Vælg protokol";
            // 
            // logTextBox
            // 
            logTextBox.Location = new Point(1172, 169);
            logTextBox.Multiline = true;
            logTextBox.Name = "logTextBox";
            logTextBox.ReadOnly = true;
            logTextBox.ScrollBars = ScrollBars.Vertical;
            logTextBox.Size = new Size(323, 507);
            logTextBox.TabIndex = 15;
            // 
            // logLabel
            // 
            logLabel.AutoSize = true;
            logLabel.Font = new Font("Segoe UI", 25.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            logLabel.Location = new Point(1172, 98);
            logLabel.Name = "logLabel";
            logLabel.Size = new Size(98, 60);
            logLabel.TabIndex = 16;
            logLabel.Text = "Log";
            
            // 
            // PLCompliantUI
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1580, 851);
            Controls.Add(logLabel);
            Controls.Add(logTextBox);
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
            Margin = new Padding(4);
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
        public TextBox logTextBox;
        private Label logLabel;
    }
}
