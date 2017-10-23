namespace TankBattle
{
    partial class SkirmishForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkirmishForm));
            this.displayPanel = new System.Windows.Forms.Panel();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.playerLabel = new System.Windows.Forms.Label();
            this.windTitle = new System.Windows.Forms.Label();
            this.windLabel = new System.Windows.Forms.Label();
            this.weaponTitle = new System.Windows.Forms.Label();
            this.weaponSelector = new System.Windows.Forms.ComboBox();
            this.angleTitle = new System.Windows.Forms.Label();
            this.angleSelector = new System.Windows.Forms.NumericUpDown();
            this.powerTitle = new System.Windows.Forms.Label();
            this.powerSelector = new System.Windows.Forms.TrackBar();
            this.fireButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.currentPower = new System.Windows.Forms.Label();
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.angleSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // displayPanel
            // 
            this.displayPanel.Location = new System.Drawing.Point(0, 32);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(800, 600);
            this.displayPanel.TabIndex = 0;
            this.displayPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.displayPanel_Paint);
            // 
            // controlPanel
            // 
            this.controlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlPanel.BackColor = System.Drawing.Color.OrangeRed;
            this.controlPanel.Controls.Add(this.currentPower);
            this.controlPanel.Controls.Add(this.fireButton);
            this.controlPanel.Controls.Add(this.powerSelector);
            this.controlPanel.Controls.Add(this.powerTitle);
            this.controlPanel.Controls.Add(this.angleSelector);
            this.controlPanel.Controls.Add(this.angleTitle);
            this.controlPanel.Controls.Add(this.weaponSelector);
            this.controlPanel.Controls.Add(this.weaponTitle);
            this.controlPanel.Controls.Add(this.windLabel);
            this.controlPanel.Controls.Add(this.windTitle);
            this.controlPanel.Controls.Add(this.playerLabel);
            this.controlPanel.Enabled = false;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(800, 32);
            this.controlPanel.TabIndex = 1;
            // 
            // playerLabel
            // 
            this.playerLabel.AutoSize = true;
            this.playerLabel.Location = new System.Drawing.Point(12, 9);
            this.playerLabel.Name = "playerLabel";
            this.playerLabel.Size = new System.Drawing.Size(36, 13);
            this.playerLabel.TabIndex = 0;
            this.playerLabel.Text = "Player";
            // 
            // windTitle
            // 
            this.windTitle.AutoSize = true;
            this.windTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windTitle.Location = new System.Drawing.Point(71, 0);
            this.windTitle.Name = "windTitle";
            this.windTitle.Size = new System.Drawing.Size(44, 13);
            this.windTitle.TabIndex = 1;
            this.windTitle.Text = "Wind: ";
            // 
            // windLabel
            // 
            this.windLabel.AutoSize = true;
            this.windLabel.Location = new System.Drawing.Point(74, 13);
            this.windLabel.Name = "windLabel";
            this.windLabel.Size = new System.Drawing.Size(27, 13);
            this.windLabel.TabIndex = 2;
            this.windLabel.Text = "0 W";
            // 
            // weaponTitle
            // 
            this.weaponTitle.AutoSize = true;
            this.weaponTitle.Location = new System.Drawing.Point(160, 9);
            this.weaponTitle.Name = "weaponTitle";
            this.weaponTitle.Size = new System.Drawing.Size(54, 13);
            this.weaponTitle.TabIndex = 3;
            this.weaponTitle.Text = "Weapon: ";
            // 
            // weaponSelector
            // 
            this.weaponSelector.FormattingEnabled = true;
            this.weaponSelector.Location = new System.Drawing.Point(220, 6);
            this.weaponSelector.Name = "weaponSelector";
            this.weaponSelector.Size = new System.Drawing.Size(121, 21);
            this.weaponSelector.TabIndex = 4;
            this.weaponSelector.SelectedValueChanged += new System.EventHandler(this.weaponSelector_SelectedValueChanged);
            // 
            // angleTitle
            // 
            this.angleTitle.AutoSize = true;
            this.angleTitle.Location = new System.Drawing.Point(347, 9);
            this.angleTitle.Name = "angleTitle";
            this.angleTitle.Size = new System.Drawing.Size(40, 13);
            this.angleTitle.TabIndex = 5;
            this.angleTitle.Text = "Angle: ";
            // 
            // angleSelector
            // 
            this.angleSelector.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.angleSelector.Location = new System.Drawing.Point(393, 5);
            this.angleSelector.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.angleSelector.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.angleSelector.Name = "angleSelector";
            this.angleSelector.Size = new System.Drawing.Size(44, 20);
            this.angleSelector.TabIndex = 6;
            this.angleSelector.ValueChanged += new System.EventHandler(this.angleSelector_ValueChanged);
            // 
            // powerTitle
            // 
            this.powerTitle.AutoSize = true;
            this.powerTitle.Location = new System.Drawing.Point(457, 9);
            this.powerTitle.Name = "powerTitle";
            this.powerTitle.Size = new System.Drawing.Size(43, 13);
            this.powerTitle.TabIndex = 7;
            this.powerTitle.Text = "Power: ";
            // 
            // powerSelector
            // 
            this.powerSelector.LargeChange = 10;
            this.powerSelector.Location = new System.Drawing.Point(506, 0);
            this.powerSelector.Maximum = 100;
            this.powerSelector.Minimum = 5;
            this.powerSelector.Name = "powerSelector";
            this.powerSelector.Size = new System.Drawing.Size(152, 45);
            this.powerSelector.TabIndex = 8;
            this.powerSelector.Value = 5;
            this.powerSelector.ValueChanged += new System.EventHandler(this.powerSelector_ValueChanged);
            // 
            // fireButton
            // 
            this.fireButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fireButton.Location = new System.Drawing.Point(713, 4);
            this.fireButton.Name = "fireButton";
            this.fireButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fireButton.Size = new System.Drawing.Size(75, 23);
            this.fireButton.TabIndex = 9;
            this.fireButton.Text = "Fire!";
            this.fireButton.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            this.timer.Interval = 20;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // currentPower
            // 
            this.currentPower.AutoSize = true;
            this.currentPower.Location = new System.Drawing.Point(665, 8);
            this.currentPower.Name = "currentPower";
            this.currentPower.Size = new System.Drawing.Size(13, 13);
            this.currentPower.TabIndex = 10;
            this.currentPower.Text = "0";
            // 
            // SkirmishForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 629);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.displayPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "SkirmishForm";
            this.Text = "Form1";
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.angleSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerSelector)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel displayPanel;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Button fireButton;
        private System.Windows.Forms.TrackBar powerSelector;
        private System.Windows.Forms.Label powerTitle;
        private System.Windows.Forms.NumericUpDown angleSelector;
        private System.Windows.Forms.Label angleTitle;
        private System.Windows.Forms.ComboBox weaponSelector;
        private System.Windows.Forms.Label weaponTitle;
        private System.Windows.Forms.Label windLabel;
        private System.Windows.Forms.Label windTitle;
        private System.Windows.Forms.Label playerLabel;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label currentPower;
    }
}

