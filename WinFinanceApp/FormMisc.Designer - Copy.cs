namespace WinFinanceApp
{
    partial class FormMisc
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
            this.openFileD = new System.Windows.Forms.OpenFileDialog();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.TWR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C1M = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C3M = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C6M = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CYTD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C1Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C3Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C5Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C10Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CLife = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileD
            // 
            this.openFileD.FileName = "openFileD";
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox.Controls.Add(this.BtnLoad);
            this.groupBox.Controls.Add(this.dataGrid);
            this.groupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox.Location = new System.Drawing.Point(5, 3);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(1455, 632);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Modify CVS file";
            // 
            // dataGrid
            // 
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TWR,
            this.C1M,
            this.C3M,
            this.C6M,
            this.CYTD,
            this.C1Y,
            this.C3Y,
            this.C5Y,
            this.C10Y,
            this.CLife,
            this.CStart});
            this.dataGrid.Location = new System.Drawing.Point(21, 38);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.Size = new System.Drawing.Size(1203, 150);
            this.dataGrid.TabIndex = 0;
            // 
            // BtnLoad
            // 
            this.BtnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnLoad.Location = new System.Drawing.Point(1275, 79);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(139, 92);
            this.BtnLoad.TabIndex = 1;
            this.BtnLoad.Text = "Load and Modify CVS file";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // TWR
            // 
            this.TWR.HeaderText = "TWR (pre-tax)";
            this.TWR.Name = "TWR";
            // 
            // C1M
            // 
            this.C1M.HeaderText = "1 Month";
            this.C1M.Name = "C1M";
            // 
            // C3M
            // 
            this.C3M.HeaderText = "3 Month";
            this.C3M.Name = "C3M";
            // 
            // C6M
            // 
            this.C6M.HeaderText = "6 Month";
            this.C6M.Name = "C6M";
            // 
            // CYTD
            // 
            this.CYTD.HeaderText = "YTD";
            this.CYTD.Name = "CYTD";
            // 
            // C1Y
            // 
            this.C1Y.HeaderText = "1 Year";
            this.C1Y.Name = "C1Y";
            // 
            // C3Y
            // 
            this.C3Y.HeaderText = "3 Year";
            this.C3Y.Name = "C3Y";
            // 
            // C5Y
            // 
            this.C5Y.HeaderText = "5 Year";
            this.C5Y.Name = "C5Y";
            // 
            // C10Y
            // 
            this.C10Y.HeaderText = "10 Year";
            this.C10Y.Name = "C10Y";
            // 
            // CLife
            // 
            this.CLife.HeaderText = "Life of data";
            this.CLife.Name = "CLife";
            // 
            // CStart
            // 
            this.CStart.HeaderText = "Start Date";
            this.CStart.Name = "CStart";
            this.CStart.Width = 159;
            // 
            // FormMisc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1463, 638);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMisc";
            this.Text = "FormMisc";
            this.Load += new System.EventHandler(this.FormMisc_Load);
            this.groupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileD;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn TWR;
        private System.Windows.Forms.DataGridViewTextBoxColumn C1M;
        private System.Windows.Forms.DataGridViewTextBoxColumn C3M;
        private System.Windows.Forms.DataGridViewTextBoxColumn C6M;
        private System.Windows.Forms.DataGridViewTextBoxColumn CYTD;
        private System.Windows.Forms.DataGridViewTextBoxColumn C1Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn C3Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn C5Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn C10Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn CLife;
        private System.Windows.Forms.DataGridViewTextBoxColumn CStart;
    }
}