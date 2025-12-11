namespace WinFinanceApp
{
    partial class FormReturn
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.grpMonths = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numMonths = new System.Windows.Forms.NumericUpDown();
            this.lblTotalM = new System.Windows.Forms.Label();
            this.chkAnnualized = new System.Windows.Forms.CheckBox();
            this.fPlot = new ScottPlot.WinForms.FormsPlot();
            this.grpSelect = new System.Windows.Forms.GroupBox();
            this.BtnCalculate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboMonths = new System.Windows.Forms.ComboBox();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.openFileD = new System.Windows.Forms.OpenFileDialog();
            this.timerGUI = new System.Windows.Forms.Timer(this.components);
            this.groupBox.SuspendLayout();
            this.grpMonths.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonths)).BeginInit();
            this.grpSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox.Controls.Add(this.grpMonths);
            this.groupBox.Controls.Add(this.chkAnnualized);
            this.groupBox.Controls.Add(this.fPlot);
            this.groupBox.Controls.Add(this.grpSelect);
            this.groupBox.Controls.Add(this.BtnLoad);
            this.groupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox.Location = new System.Drawing.Point(12, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(1455, 653);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Historic time weighted return (TWR)";
            // 
            // grpMonths
            // 
            this.grpMonths.Controls.Add(this.label2);
            this.grpMonths.Controls.Add(this.label3);
            this.grpMonths.Controls.Add(this.numMonths);
            this.grpMonths.Controls.Add(this.lblTotalM);
            this.grpMonths.Location = new System.Drawing.Point(6, 229);
            this.grpMonths.Name = "grpMonths";
            this.grpMonths.Size = new System.Drawing.Size(106, 133);
            this.grpMonths.TabIndex = 5;
            this.grpMonths.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "# of following \r\n    months";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "total months\r\n   avaialble";
            // 
            // numMonths
            // 
            this.numMonths.Location = new System.Drawing.Point(31, 54);
            this.numMonths.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMonths.Name = "numMonths";
            this.numMonths.Size = new System.Drawing.Size(47, 20);
            this.numMonths.TabIndex = 3;
            this.numMonths.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMonths.ValueChanged += new System.EventHandler(this.numMonths_ValueChanged);
            // 
            // lblTotalM
            // 
            this.lblTotalM.AutoSize = true;
            this.lblTotalM.Location = new System.Drawing.Point(48, 108);
            this.lblTotalM.Name = "lblTotalM";
            this.lblTotalM.Size = new System.Drawing.Size(14, 13);
            this.lblTotalM.TabIndex = 2;
            this.lblTotalM.Text = "?";
            // 
            // chkAnnualized
            // 
            this.chkAnnualized.AutoSize = true;
            this.chkAnnualized.Location = new System.Drawing.Point(20, 87);
            this.chkAnnualized.Name = "chkAnnualized";
            this.chkAnnualized.Size = new System.Drawing.Size(88, 30);
            this.chkAnnualized.TabIndex = 4;
            this.chkAnnualized.Text = "Annualized\r\n return";
            this.chkAnnualized.UseVisualStyleBackColor = true;
            // 
            // fPlot
            // 
            this.fPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fPlot.DisplayScale = 0F;
            this.fPlot.Location = new System.Drawing.Point(124, 19);
            this.fPlot.Name = "fPlot";
            this.fPlot.Size = new System.Drawing.Size(1325, 611);
            this.fPlot.TabIndex = 3;
            // 
            // grpSelect
            // 
            this.grpSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpSelect.Controls.Add(this.BtnCalculate);
            this.grpSelect.Controls.Add(this.label1);
            this.grpSelect.Controls.Add(this.comboMonths);
            this.grpSelect.Location = new System.Drawing.Point(6, 123);
            this.grpSelect.Name = "grpSelect";
            this.grpSelect.Size = new System.Drawing.Size(112, 318);
            this.grpSelect.TabIndex = 2;
            this.grpSelect.TabStop = false;
            this.grpSelect.Text = "Select months  and click Get TWR";
            // 
            // BtnCalculate
            // 
            this.BtnCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnCalculate.Location = new System.Drawing.Point(19, 259);
            this.BtnCalculate.Name = "BtnCalculate";
            this.BtnCalculate.Size = new System.Drawing.Size(75, 44);
            this.BtnCalculate.TabIndex = 4;
            this.BtnCalculate.Text = "Get TWR";
            this.BtnCalculate.UseVisualStyleBackColor = true;
            this.BtnCalculate.Click += new System.EventHandler(this.BtnCalculate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 26);
            this.label1.TabIndex = 2;
            this.label1.Text = "start  month\r\n(or Annualized)";
            // 
            // comboMonths
            // 
            this.comboMonths.FormattingEnabled = true;
            this.comboMonths.Location = new System.Drawing.Point(1, 79);
            this.comboMonths.Name = "comboMonths";
            this.comboMonths.Size = new System.Drawing.Size(84, 21);
            this.comboMonths.TabIndex = 1;
            this.comboMonths.SelectedIndexChanged += new System.EventHandler(this.comboMonths_SelectedIndexChanged);
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(6, 19);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(112, 52);
            this.BtnLoad.TabIndex = 0;
            this.BtnLoad.Text = "Load \r\nFidelity return \r\nhistory File";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // openFileD
            // 
            this.openFileD.FileName = "openFileD";
            // 
            // timerGUI
            // 
            this.timerGUI.Enabled = true;
            this.timerGUI.Tick += new System.EventHandler(this.timerGUI_Tick);
            // 
            // FormReturn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1479, 677);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormReturn";
            this.Text = "FormReturn";
            this.Load += new System.EventHandler(this.FormReturn_Load);
            this.Resize += new System.EventHandler(this.FormReturn_Resize);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.grpMonths.ResumeLayout(false);
            this.grpMonths.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonths)).EndInit();
            this.grpSelect.ResumeLayout(false);
            this.grpSelect.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.OpenFileDialog openFileD;
        private System.Windows.Forms.GroupBox grpSelect;
        private System.Windows.Forms.NumericUpDown numMonths;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboMonths;
        private System.Windows.Forms.Label lblTotalM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnCalculate;
        private ScottPlot.WinForms.FormsPlot fPlot;
        private System.Windows.Forms.Timer timerGUI;
        private System.Windows.Forms.CheckBox chkAnnualized;
        private System.Windows.Forms.GroupBox grpMonths;
    }
}