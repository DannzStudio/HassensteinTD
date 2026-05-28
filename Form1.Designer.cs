namespace HassensteinTD
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
            mainDisplay = new PictureBox();
            waveLabel = new Label();
            waveCounterL = new Label();
            moneyLabel = new Label();
            goldCounterL = new Label();
            mainUpdate = new System.Windows.Forms.Timer(components);
            enemyUpdate = new System.Windows.Forms.Timer(components);
            archerUpdate = new System.Windows.Forms.Timer(components);
            arrowUpdate = new System.Windows.Forms.Timer(components);
            hedgehogUpdate = new System.Windows.Forms.Timer(components);
            trapperUpdate = new System.Windows.Forms.Timer(components);
            bombUpdate = new System.Windows.Forms.Timer(components);
            bomberUpdate = new System.Windows.Forms.Timer(components);
            livesLabel = new Label();
            livesCounterL = new Label();
            menuDisplay = new PictureBox();
            startWaveBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)mainDisplay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)menuDisplay).BeginInit();
            SuspendLayout();
            // 
            // mainDisplay
            // 
            mainDisplay.BackColor = Color.FromArgb(64, 64, 64);
            mainDisplay.Location = new Point(12, 56);
            mainDisplay.Name = "mainDisplay";
            mainDisplay.Size = new Size(950, 750);
            mainDisplay.TabIndex = 0;
            mainDisplay.TabStop = false;
            mainDisplay.Paint += mainDisplay_Paint;
            mainDisplay.MouseDown += mainDisplay_MouseDown;
            mainDisplay.MouseMove += mainDisplay_MouseMove;
            mainDisplay.MouseUp += mainDisplay_MouseUp;
            // 
            // waveLabel
            // 
            waveLabel.AutoSize = true;
            waveLabel.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            waveLabel.ForeColor = Color.White;
            waveLabel.Location = new Point(12, 18);
            waveLabel.Name = "waveLabel";
            waveLabel.Size = new Size(65, 25);
            waveLabel.TabIndex = 1;
            waveLabel.Text = "Wave:";
            // 
            // waveCounterL
            // 
            waveCounterL.AutoSize = true;
            waveCounterL.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            waveCounterL.ForeColor = Color.White;
            waveCounterL.Location = new Point(80, 18);
            waveCounterL.Name = "waveCounterL";
            waveCounterL.Size = new Size(23, 25);
            waveCounterL.TabIndex = 2;
            waveCounterL.Text = "1";
            // 
            // moneyLabel
            // 
            moneyLabel.AutoSize = true;
            moneyLabel.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            moneyLabel.ForeColor = Color.White;
            moneyLabel.Location = new Point(734, 18);
            moneyLabel.Name = "moneyLabel";
            moneyLabel.Size = new Size(60, 25);
            moneyLabel.TabIndex = 3;
            moneyLabel.Text = "Gold:";
            // 
            // goldCounterL
            // 
            goldCounterL.AutoSize = true;
            goldCounterL.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            goldCounterL.ForeColor = Color.White;
            goldCounterL.Location = new Point(796, 18);
            goldCounterL.Name = "goldCounterL";
            goldCounterL.Size = new Size(122, 25);
            goldCounterL.TabIndex = 4;
            goldCounterL.Text = "9999999999";
            // 
            // mainUpdate
            // 
            mainUpdate.Enabled = true;
            mainUpdate.Interval = 50;
            mainUpdate.Tick += mainUpdate_Tick;
            // 
            // enemyUpdate
            // 
            enemyUpdate.Enabled = true;
            enemyUpdate.Interval = 1000;
            enemyUpdate.Tick += enemyUpdate_Tick;
            // 
            // archerUpdate
            // 
            archerUpdate.Enabled = true;
            archerUpdate.Interval = 1000;
            archerUpdate.Tick += archerUpdate_Tick;
            // 
            // arrowUpdate
            // 
            arrowUpdate.Enabled = true;
            arrowUpdate.Interval = 25;
            arrowUpdate.Tick += arrowUpdate_Tick;
            // 
            // hedgehogUpdate
            // 
            hedgehogUpdate.Enabled = true;
            hedgehogUpdate.Interval = 500;
            hedgehogUpdate.Tick += hedgehogUpdate_Tick;
            // 
            // trapperUpdate
            // 
            trapperUpdate.Enabled = true;
            trapperUpdate.Interval = 5000;
            trapperUpdate.Tick += trapperUpdate_Tick;
            // 
            // bombUpdate
            // 
            bombUpdate.Enabled = true;
            bombUpdate.Interval = 25;
            bombUpdate.Tick += bombUpdate_Tick;
            // 
            // bomberUpdate
            // 
            bomberUpdate.Enabled = true;
            bomberUpdate.Interval = 1500;
            bomberUpdate.Tick += bomberUpdate_Tick;
            // 
            // livesLabel
            // 
            livesLabel.AutoSize = true;
            livesLabel.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            livesLabel.ForeColor = Color.White;
            livesLabel.Location = new Point(370, 18);
            livesLabel.Name = "livesLabel";
            livesLabel.Size = new Size(60, 25);
            livesLabel.TabIndex = 5;
            livesLabel.Text = "Lives:";
            // 
            // livesCounterL
            // 
            livesCounterL.AutoSize = true;
            livesCounterL.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            livesCounterL.ForeColor = Color.White;
            livesCounterL.Location = new Point(433, 18);
            livesCounterL.Name = "livesCounterL";
            livesCounterL.Size = new Size(67, 25);
            livesCounterL.TabIndex = 6;
            livesCounterL.Text = "99999";
            // 
            // menuDisplay
            // 
            menuDisplay.BackColor = SystemColors.ControlDarkDark;
            menuDisplay.Location = new Point(1000, 1000);
            menuDisplay.Name = "menuDisplay";
            menuDisplay.Size = new Size(970, 817);
            menuDisplay.TabIndex = 7;
            menuDisplay.TabStop = false;
            menuDisplay.Visible = false;
            menuDisplay.Paint += menuDisplay_Paint;
            menuDisplay.MouseDown += menuDisplay_MouseDown;
            menuDisplay.MouseMove += menuDisplay_MouseMove;
            // 
            // startWaveBtn
            // 
            startWaveBtn.BackColor = Color.PaleGreen;
            startWaveBtn.Enabled = false;
            startWaveBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            startWaveBtn.Location = new Point(134, 8);
            startWaveBtn.Name = "startWaveBtn";
            startWaveBtn.Size = new Size(127, 44);
            startWaveBtn.TabIndex = 8;
            startWaveBtn.Text = "Start Wave";
            startWaveBtn.UseVisualStyleBackColor = false;
            startWaveBtn.Click += startWaveBtn_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(970, 817);
            Controls.Add(startWaveBtn);
            Controls.Add(menuDisplay);
            Controls.Add(livesCounterL);
            Controls.Add(livesLabel);
            Controls.Add(goldCounterL);
            Controls.Add(moneyLabel);
            Controls.Add(waveCounterL);
            Controls.Add(waveLabel);
            Controls.Add(mainDisplay);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HassensteinTD";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)mainDisplay).EndInit();
            ((System.ComponentModel.ISupportInitialize)menuDisplay).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox mainDisplay;
        private Label waveLabel;
        private Label waveCounterL;
        private Label moneyLabel;
        private Label goldCounterL;
        private System.Windows.Forms.Timer mainUpdate;
        private System.Windows.Forms.Timer enemyUpdate;
        private System.Windows.Forms.Timer archerUpdate;
        private System.Windows.Forms.Timer arrowUpdate;
        private System.Windows.Forms.Timer hedgehogUpdate;
        private System.Windows.Forms.Timer trapperUpdate;
        private System.Windows.Forms.Timer bombUpdate;
        private System.Windows.Forms.Timer bomberUpdate;
        private Label livesLabel;
        private Label livesCounterL;
        private PictureBox menuDisplay;
        private Button startWaveBtn;
    }
}
