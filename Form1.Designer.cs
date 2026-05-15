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
            moneyCounterL = new Label();
            mainUpdate = new System.Windows.Forms.Timer(components);
            enemyUpdate = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)mainDisplay).BeginInit();
            SuspendLayout();
            // 
            // mainDisplay
            // 
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
            waveLabel.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            waveLabel.Location = new Point(12, 18);
            waveLabel.Name = "waveLabel";
            waveLabel.Size = new Size(62, 25);
            waveLabel.TabIndex = 1;
            waveLabel.Text = "Wave:";
            // 
            // waveCounterL
            // 
            waveCounterL.AutoSize = true;
            waveCounterL.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            waveCounterL.Location = new Point(80, 18);
            waveCounterL.Name = "waveCounterL";
            waveCounterL.Size = new Size(22, 25);
            waveCounterL.TabIndex = 2;
            waveCounterL.Text = "1";
            // 
            // moneyLabel
            // 
            moneyLabel.AutoSize = true;
            moneyLabel.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            moneyLabel.Location = new Point(734, 18);
            moneyLabel.Name = "moneyLabel";
            moneyLabel.Size = new Size(56, 25);
            moneyLabel.TabIndex = 3;
            moneyLabel.Text = "Gold:";
            // 
            // moneyCounterL
            // 
            moneyCounterL.AutoSize = true;
            moneyCounterL.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            moneyCounterL.Location = new Point(796, 18);
            moneyCounterL.Name = "moneyCounterL";
            moneyCounterL.Size = new Size(112, 25);
            moneyCounterL.TabIndex = 4;
            moneyCounterL.Text = "9999999999";
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(970, 817);
            Controls.Add(moneyCounterL);
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
            KeyDown += Form1_KeyDown;
            ((System.ComponentModel.ISupportInitialize)mainDisplay).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox mainDisplay;
        private Label waveLabel;
        private Label waveCounterL;
        private Label moneyLabel;
        private Label moneyCounterL;
        private System.Windows.Forms.Timer mainUpdate;
        private System.Windows.Forms.Timer enemyUpdate;
    }
}
