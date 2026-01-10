namespace WinFinanceApp
{
    partial class FormAcctSpending
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fPlot = new ScottPlot.WinForms.FormsPlot();
            this.BtnPlot = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BtnUpdate = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numWithdrawVal = new System.Windows.Forms.NumericUpDown();
            this.numDividentsIn = new System.Windows.Forms.NumericUpDown();
            this.numDividentsOut = new System.Windows.Forms.NumericUpDown();
            this.numFundAdded = new System.Windows.Forms.NumericUpDown();
            this.numAmount = new System.Windows.Forms.NumericUpDown();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWithdrawVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDividentsIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDividentsOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFundAdded)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.fPlot);
            this.groupBox1.Location = new System.Drawing.Point(147, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1304, 570);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Analyze Spending ";
            // 
            // fPlot
            // 
            this.fPlot.DisplayScale = 0F;
            this.fPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fPlot.Location = new System.Drawing.Point(3, 16);
            this.fPlot.Name = "fPlot";
            this.fPlot.Size = new System.Drawing.Size(1298, 551);
            this.fPlot.TabIndex = 0;
            this.fPlot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fPlot_MouseDown);
            this.fPlot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fPlot_MouseUp);
            // 
            // BtnPlot
            // 
            this.BtnPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnPlot.Location = new System.Drawing.Point(14, 532);
            this.BtnPlot.Name = "BtnPlot";
            this.BtnPlot.Size = new System.Drawing.Size(75, 44);
            this.BtnPlot.TabIndex = 5;
            this.BtnPlot.Text = "Refresh Plot";
            this.BtnPlot.UseVisualStyleBackColor = true;
            this.BtnPlot.Click += new System.EventHandler(this.BtnPlot_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BtnUpdate);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numWithdrawVal);
            this.groupBox2.Controls.Add(this.numDividentsIn);
            this.groupBox2.Controls.Add(this.numDividentsOut);
            this.groupBox2.Controls.Add(this.numFundAdded);
            this.groupBox2.Controls.Add(this.numAmount);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(117, 328);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add a new record for the month";
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Location = new System.Drawing.Point(6, 293);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(75, 23);
            this.BtnUpdate.TabIndex = 2;
            this.BtnUpdate.Text = "Record";
            this.BtnUpdate.UseVisualStyleBackColor = true;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 246);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "WithdrawVal";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 26);
            this.label4.TabIndex = 1;
            this.label4.Text = "Dividents\r\nIn $";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 26);
            this.label3.TabIndex = 1;
            this.label3.Text = "Dividents \r\nOut $";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "$ added ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "$ left";
            // 
            // numWithdrawVal
            // 
            this.numWithdrawVal.DecimalPlaces = 1;
            this.numWithdrawVal.Location = new System.Drawing.Point(10, 262);
            this.numWithdrawVal.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numWithdrawVal.Name = "numWithdrawVal";
            this.numWithdrawVal.Size = new System.Drawing.Size(67, 20);
            this.numWithdrawVal.TabIndex = 0;
            this.numWithdrawVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // numDividentsIn
            // 
            this.numDividentsIn.DecimalPlaces = 1;
            this.numDividentsIn.Location = new System.Drawing.Point(10, 205);
            this.numDividentsIn.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numDividentsIn.Name = "numDividentsIn";
            this.numDividentsIn.Size = new System.Drawing.Size(67, 20);
            this.numDividentsIn.TabIndex = 0;
            this.numDividentsIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // numDividentsOut
            // 
            this.numDividentsOut.DecimalPlaces = 1;
            this.numDividentsOut.Location = new System.Drawing.Point(10, 148);
            this.numDividentsOut.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numDividentsOut.Name = "numDividentsOut";
            this.numDividentsOut.Size = new System.Drawing.Size(67, 20);
            this.numDividentsOut.TabIndex = 0;
            this.numDividentsOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // numFundAdded
            // 
            this.numFundAdded.DecimalPlaces = 1;
            this.numFundAdded.Location = new System.Drawing.Point(10, 95);
            this.numFundAdded.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numFundAdded.Name = "numFundAdded";
            this.numFundAdded.Size = new System.Drawing.Size(67, 20);
            this.numFundAdded.TabIndex = 0;
            this.numFundAdded.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // numAmount
            // 
            this.numAmount.DecimalPlaces = 1;
            this.numAmount.Location = new System.Drawing.Point(10, 58);
            this.numAmount.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numAmount.Name = "numAmount";
            this.numAmount.Size = new System.Drawing.Size(67, 20);
            this.numAmount.TabIndex = 0;
            this.numAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::WinFinanceApp.Properties.Resources.trend;
            this.pictureBox1.Location = new System.Drawing.Point(18, 476);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(57, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // FormAcctSpending
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1463, 588);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.BtnPlot);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormAcctSpending";
            this.Text = "FormAcctSpending";
            this.Load += new System.EventHandler(this.FormAcctSpending_Load);
            this.Resize += new System.EventHandler(this.FormAcctSpending_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWithdrawVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDividentsIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDividentsOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFundAdded)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private ScottPlot.WinForms.FormsPlot fPlot;
        private System.Windows.Forms.Button BtnPlot;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numDividentsIn;
        private System.Windows.Forms.NumericUpDown numDividentsOut;
        private System.Windows.Forms.NumericUpDown numFundAdded;
        private System.Windows.Forms.NumericUpDown numAmount;
        private System.Windows.Forms.Button BtnUpdate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numWithdrawVal;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}