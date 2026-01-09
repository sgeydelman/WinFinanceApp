namespace WinFinanceApp
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.BtnSetup = new System.Windows.Forms.Button();
            this.BtnIRA_S = new System.Windows.Forms.Button();
            this.PANPAN = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewScreensToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oPMMonitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sETUPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.historicalRetrunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sETUPToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.listView = new System.Windows.Forms.ListView();
            this.btnAlarm = new System.Windows.Forms.Button();
            this.BtnMonitor = new System.Windows.Forms.Button();
            this.lblRothIRA = new System.Windows.Forms.Label();
            this.timer_ui = new System.Windows.Forms.Timer(this.components);
            this.BtnReturn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BtnAcctSpending = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnSetup
            // 
            this.BtnSetup.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSetup.Location = new System.Drawing.Point(4, 392);
            this.BtnSetup.Name = "BtnSetup";
            this.BtnSetup.Size = new System.Drawing.Size(135, 64);
            this.BtnSetup.TabIndex = 182;
            this.BtnSetup.Text = "SETUP";
            this.BtnSetup.UseVisualStyleBackColor = true;
            this.BtnSetup.Click += new System.EventHandler(this.BtnSetup_Click);
            // 
            // BtnIRA_S
            // 
            this.BtnIRA_S.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnIRA_S.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnIRA_S.Location = new System.Drawing.Point(4, 178);
            this.BtnIRA_S.Name = "BtnIRA_S";
            this.BtnIRA_S.Size = new System.Drawing.Size(135, 64);
            this.BtnIRA_S.TabIndex = 181;
            this.BtnIRA_S.Text = "Account Rebalance";
            this.BtnIRA_S.UseVisualStyleBackColor = true;
            this.BtnIRA_S.Click += new System.EventHandler(this.BtnIRA_S_Click);
            // 
            // PANPAN
            // 
            this.PANPAN.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PANPAN.Location = new System.Drawing.Point(145, 32);
            this.PANPAN.Name = "PANPAN";
            this.PANPAN.Size = new System.Drawing.Size(1477, 490);
            this.PANPAN.TabIndex = 183;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.viewScreensToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1624, 30);
            this.menuStrip1.TabIndex = 184;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(65, 24);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // viewScreensToolStripMenuItem
            // 
            this.viewScreensToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oPMMonitorToolStripMenuItem,
            this.sETUPToolStripMenuItem,
            this.historicalRetrunToolStripMenuItem,
            this.sETUPToolStripMenuItem1});
            this.viewScreensToolStripMenuItem.Name = "viewScreensToolStripMenuItem";
            this.viewScreensToolStripMenuItem.Size = new System.Drawing.Size(112, 24);
            this.viewScreensToolStripMenuItem.Text = "View Screens";
            // 
            // oPMMonitorToolStripMenuItem
            // 
            this.oPMMonitorToolStripMenuItem.Name = "oPMMonitorToolStripMenuItem";
            this.oPMMonitorToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.oPMMonitorToolStripMenuItem.Text = "Account Monitor";
            this.oPMMonitorToolStripMenuItem.Click += new System.EventHandler(this.oPMMonitorToolStripMenuItem_Click);
            // 
            // sETUPToolStripMenuItem
            // 
            this.sETUPToolStripMenuItem.Name = "sETUPToolStripMenuItem";
            this.sETUPToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.sETUPToolStripMenuItem.Text = "Account Rebalance";
            this.sETUPToolStripMenuItem.Click += new System.EventHandler(this.sETUPToolStripMenuItem_Click);
            // 
            // historicalRetrunToolStripMenuItem
            // 
            this.historicalRetrunToolStripMenuItem.Name = "historicalRetrunToolStripMenuItem";
            this.historicalRetrunToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.historicalRetrunToolStripMenuItem.Text = "Historical Return";
            this.historicalRetrunToolStripMenuItem.Click += new System.EventHandler(this.historicalRetrunToolStripMenuItem_Click);
            // 
            // sETUPToolStripMenuItem1
            // 
            this.sETUPToolStripMenuItem1.Name = "sETUPToolStripMenuItem1";
            this.sETUPToolStripMenuItem1.Size = new System.Drawing.Size(211, 24);
            this.sETUPToolStripMenuItem1.Text = "SETUP";
            this.sETUPToolStripMenuItem1.Click += new System.EventHandler(this.sETUPToolStripMenuItem1_Click);
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(145, 528);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(1477, 76);
            this.listView.TabIndex = 185;
            this.listView.UseCompatibleStateImageBehavior = false;
            // 
            // btnAlarm
            // 
            this.btnAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAlarm.BackColor = System.Drawing.Color.PaleGreen;
            this.btnAlarm.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlarm.Location = new System.Drawing.Point(12, 556);
            this.btnAlarm.Name = "btnAlarm";
            this.btnAlarm.Size = new System.Drawing.Size(127, 48);
            this.btnAlarm.TabIndex = 186;
            this.btnAlarm.Text = "Clear \r";
            this.btnAlarm.UseVisualStyleBackColor = false;
            this.btnAlarm.Click += new System.EventHandler(this.BtnAlarm_Click);
            // 
            // BtnMonitor
            // 
            this.BtnMonitor.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnMonitor.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMonitor.Location = new System.Drawing.Point(4, 106);
            this.BtnMonitor.Name = "BtnMonitor";
            this.BtnMonitor.Size = new System.Drawing.Size(135, 64);
            this.BtnMonitor.TabIndex = 181;
            this.BtnMonitor.Text = "Account Monitor";
            this.BtnMonitor.UseVisualStyleBackColor = true;
            this.BtnMonitor.Click += new System.EventHandler(this.BtnMonitor_Click);
            // 
            // lblRothIRA
            // 
            this.lblRothIRA.AutoSize = true;
            this.lblRothIRA.Location = new System.Drawing.Point(485, 13);
            this.lblRothIRA.Name = "lblRothIRA";
            this.lblRothIRA.Size = new System.Drawing.Size(234, 13);
            this.lblRothIRA.TabIndex = 187;
            this.lblRothIRA.Text = "Attention! The setup is configured for Roth IRA !";
            // 
            // timer_ui
            // 
            this.timer_ui.Enabled = true;
            this.timer_ui.Tick += new System.EventHandler(this.timer_ui_Tick);
            // 
            // BtnReturn
            // 
            this.BtnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnReturn.Location = new System.Drawing.Point(4, 248);
            this.BtnReturn.Name = "BtnReturn";
            this.BtnReturn.Size = new System.Drawing.Size(135, 64);
            this.BtnReturn.TabIndex = 181;
            this.BtnReturn.Text = "Analyze Return";
            this.BtnReturn.UseVisualStyleBackColor = true;
            this.BtnReturn.Click += new System.EventHandler(this.BtnReturn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WinFinanceApp.Properties.Resources.money;
            this.pictureBox1.Location = new System.Drawing.Point(12, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(69, 56);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // BtnAcctSpending
            // 
            this.BtnAcctSpending.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnAcctSpending.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAcctSpending.Location = new System.Drawing.Point(4, 319);
            this.BtnAcctSpending.Name = "BtnAcctSpending";
            this.BtnAcctSpending.Size = new System.Drawing.Size(135, 64);
            this.BtnAcctSpending.TabIndex = 182;
            this.BtnAcctSpending.Text = "Analize Spending";
            this.BtnAcctSpending.UseVisualStyleBackColor = true;
            this.BtnAcctSpending.Click += new System.EventHandler(this.BtnAcctSpending_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1624, 606);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblRothIRA);
            this.Controls.Add(this.BtnMonitor);
            this.Controls.Add(this.btnAlarm);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.PANPAN);
            this.Controls.Add(this.BtnAcctSpending);
            this.Controls.Add(this.BtnSetup);
            this.Controls.Add(this.BtnReturn);
            this.Controls.Add(this.BtnIRA_S);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WinFinanceApps";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Button BtnSetup;
        public System.Windows.Forms.Button BtnIRA_S;
        protected System.Windows.Forms.Panel PANPAN;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewScreensToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oPMMonitorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sETUPToolStripMenuItem;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.Button btnAlarm;
        public System.Windows.Forms.Button BtnMonitor;
        private System.Windows.Forms.Label lblRothIRA;
        private System.Windows.Forms.Timer timer_ui;
        public System.Windows.Forms.Button BtnReturn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem historicalRetrunToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sETUPToolStripMenuItem1;
        protected System.Windows.Forms.Button BtnAcctSpending;
    }
}
