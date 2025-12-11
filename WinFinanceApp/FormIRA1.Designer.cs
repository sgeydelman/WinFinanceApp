namespace WinFinanceApp
{
    partial class FormIRA1
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
            this.openFileD = new System.Windows.Forms.OpenFileDialog();
            this.BtnLoadFile = new System.Windows.Forms.Button();
            this.grpBox = new System.Windows.Forms.GroupBox();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RebalanceValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Deviation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTotalRebalance = new System.Windows.Forms.Label();
            this.lblTotalTarget = new System.Windows.Forms.Label();
            this.lblTotalCur = new System.Windows.Forms.Label();
            this.lblTotalVal = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.timer_ui = new System.Windows.Forms.Timer(this.components);
            this.grpBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileD
            // 
            this.openFileD.FileName = "openFileDialogD";
            // 
            // BtnLoadFile
            // 
            this.BtnLoadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnLoadFile.Location = new System.Drawing.Point(189, 545);
            this.BtnLoadFile.Name = "BtnLoadFile";
            this.BtnLoadFile.Size = new System.Drawing.Size(114, 79);
            this.BtnLoadFile.TabIndex = 11;
            this.BtnLoadFile.Text = "Load Current \r\nAccount from File\r\nand Rebalance all\r\npositions";
            this.BtnLoadFile.UseVisualStyleBackColor = true;
            this.BtnLoadFile.Click += new System.EventHandler(this.BtnLoadFile_Click);
            // 
            // grpBox
            // 
            this.grpBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBox.Controls.Add(this.dataGrid);
            this.grpBox.Controls.Add(this.lblTotalRebalance);
            this.grpBox.Controls.Add(this.lblTotalTarget);
            this.grpBox.Controls.Add(this.lblTotalCur);
            this.grpBox.Controls.Add(this.lblTotalVal);
            this.grpBox.Controls.Add(this.label7);
            this.grpBox.Controls.Add(this.BtnLoadFile);
            this.grpBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBox.Location = new System.Drawing.Point(45, 12);
            this.grpBox.Name = "grpBox";
            this.grpBox.Size = new System.Drawing.Size(1333, 631);
            this.grpBox.TabIndex = 12;
            this.grpBox.TabStop = false;
            this.grpBox.Text = "Account Rebalance";
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            this.dataGrid.AllowUserToOrderColumns = true;
            this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Description,
            this.Key,
            this.Value,
            this.CurPercent,
            this.TargetPercent,
            this.RebalanceValue,
            this.Deviation});
            this.dataGrid.Location = new System.Drawing.Point(111, 19);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.Size = new System.Drawing.Size(1149, 385);
            this.dataGrid.TabIndex = 20;
            this.dataGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGrid_CellFormatting);
            // 
            // Description
            // 
            this.Description.DataPropertyName = "Description";
            this.Description.FillWeight = 200F;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 500;
            // 
            // Key
            // 
            this.Key.DataPropertyName = "Key";
            this.Key.HeaderText = "Symbol";
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            // 
            // Value
            // 
            this.Value.DataPropertyName = "Value";
            this.Value.HeaderText = "Carrent $ Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            // 
            // CurPercent
            // 
            this.CurPercent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CurPercent.DataPropertyName = "CurPercent";
            this.CurPercent.HeaderText = "Current % of total";
            this.CurPercent.Name = "CurPercent";
            this.CurPercent.ReadOnly = true;
            // 
            // TargetPercent
            // 
            this.TargetPercent.DataPropertyName = "TargetPercent";
            this.TargetPercent.HeaderText = "Target % of Total";
            this.TargetPercent.Name = "TargetPercent";
            this.TargetPercent.ReadOnly = true;
            // 
            // RebalanceValue
            // 
            this.RebalanceValue.DataPropertyName = "RebalanceValue";
            this.RebalanceValue.HeaderText = "$ Value to Rebalance";
            this.RebalanceValue.Name = "RebalanceValue";
            this.RebalanceValue.ReadOnly = true;
            // 
            // Deviation
            // 
            this.Deviation.HeaderText = "Target Deviation %";
            this.Deviation.Name = "Deviation";
            this.Deviation.ReadOnly = true;
            // 
            // lblTotalRebalance
            // 
            this.lblTotalRebalance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalRebalance.AutoSize = true;
            this.lblTotalRebalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalRebalance.Location = new System.Drawing.Point(1064, 418);
            this.lblTotalRebalance.Name = "lblTotalRebalance";
            this.lblTotalRebalance.Size = new System.Drawing.Size(19, 20);
            this.lblTotalRebalance.TabIndex = 12;
            this.lblTotalRebalance.Text = "?";
            // 
            // lblTotalTarget
            // 
            this.lblTotalTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalTarget.AutoSize = true;
            this.lblTotalTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalTarget.Location = new System.Drawing.Point(966, 418);
            this.lblTotalTarget.Name = "lblTotalTarget";
            this.lblTotalTarget.Size = new System.Drawing.Size(19, 20);
            this.lblTotalTarget.TabIndex = 12;
            this.lblTotalTarget.Text = "?";
            // 
            // lblTotalCur
            // 
            this.lblTotalCur.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalCur.AutoSize = true;
            this.lblTotalCur.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCur.Location = new System.Drawing.Point(868, 418);
            this.lblTotalCur.Name = "lblTotalCur";
            this.lblTotalCur.Size = new System.Drawing.Size(19, 20);
            this.lblTotalCur.TabIndex = 12;
            this.lblTotalCur.Text = "?";
            // 
            // lblTotalVal
            // 
            this.lblTotalVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalVal.AutoSize = true;
            this.lblTotalVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalVal.Location = new System.Drawing.Point(753, 418);
            this.lblTotalVal.Name = "lblTotalVal";
            this.lblTotalVal.Size = new System.Drawing.Size(19, 20);
            this.lblTotalVal.TabIndex = 12;
            this.lblTotalVal.Text = "?";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(420, 418);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 20);
            this.label7.TabIndex = 12;
            this.label7.Text = "Total:";
            // 
            // timer_ui
            // 
            this.timer_ui.Enabled = true;
            this.timer_ui.Tick += new System.EventHandler(this.timer_ui_Tick);
            // 
            // FormIRA1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1495, 666);
            this.Controls.Add(this.grpBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormIRA1";
            this.Text = "FormIRA1";
            this.Load += new System.EventHandler(this.FormIRA1_Load);
            this.Resize += new System.EventHandler(this.FormIRA1_Resize);
            this.grpBox.ResumeLayout(false);
            this.grpBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileD;
        private System.Windows.Forms.Button BtnLoadFile;
        private System.Windows.Forms.GroupBox grpBox;
        private System.Windows.Forms.Label lblTotalTarget;
        private System.Windows.Forms.Label lblTotalCur;
        private System.Windows.Forms.Label lblTotalVal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTotalRebalance;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.Timer timer_ui;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargetPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn RebalanceValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Deviation;
    }
}
