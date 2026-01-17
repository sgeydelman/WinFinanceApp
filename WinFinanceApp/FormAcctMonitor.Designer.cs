namespace WinFinanceApp
{
    partial class FormAcctMonitor
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
            this.grpBox = new System.Windows.Forms.GroupBox();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.tableLP = new System.Windows.Forms.TableLayoutPanel();
            this.lblCurValue = new System.Windows.Forms.Label();
            this.lblCostBasic = new System.Windows.Forms.Label();
            this.lblCurPrecent = new System.Windows.Forms.Label();
            this.lblGainPercent = new System.Windows.Forms.Label();
            this.lblGainDoll = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpReturn = new System.Windows.Forms.GroupBox();
            this.BtnReturn = new System.Windows.Forms.Button();
            this.combo2 = new System.Windows.Forms.ComboBox();
            this.combo1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblVal2 = new System.Windows.Forms.Label();
            this.lblVal1 = new System.Windows.Forms.Label();
            this.lblReturn = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.grpFilter = new System.Windows.Forms.GroupBox();
            this.radioInter = new System.Windows.Forms.RadioButton();
            this.dataGr = new System.Windows.Forms.DataGridView();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radioValue = new System.Windows.Forms.RadioButton();
            this.radioGrowth = new System.Windows.Forms.RadioButton();
            this.radioUS = new System.Windows.Forms.RadioButton();
            this.radioAll = new System.Windows.Forms.RadioButton();
            this.radioCental = new System.Windows.Forms.RadioButton();
            this.lblPosAmount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnLoadFile = new System.Windows.Forms.Button();
            this.openFileD = new System.Windows.Forms.OpenFileDialog();
            this.grpBox.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.tableLP.SuspendLayout();
            this.grpReturn.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // grpBox
            // 
            this.grpBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBox.Controls.Add(this.panelFooter);
            this.grpBox.Controls.Add(this.groupBox2);
            this.grpBox.Controls.Add(this.dataGrid);
            this.grpBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBox.Location = new System.Drawing.Point(9, 3);
            this.grpBox.Name = "grpBox";
            this.grpBox.Padding = new System.Windows.Forms.Padding(0);
            this.grpBox.Size = new System.Drawing.Size(1146, 623);
            this.grpBox.TabIndex = 0;
            this.grpBox.TabStop = false;
            this.grpBox.Text = "Account Monitor";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.tableLP);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 586);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1146, 37);
            this.panelFooter.TabIndex = 19;
            // 
            // tableLP
            // 
            this.tableLP.BackColor = System.Drawing.SystemColors.Control;
            this.tableLP.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLP.ColumnCount = 8;
            this.tableLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.tableLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 252F));
            this.tableLP.Controls.Add(this.lblCurValue, 2, 0);
            this.tableLP.Controls.Add(this.lblCostBasic, 6, 0);
            this.tableLP.Controls.Add(this.lblCurPrecent, 5, 0);
            this.tableLP.Controls.Add(this.lblGainPercent, 4, 0);
            this.tableLP.Controls.Add(this.lblGainDoll, 3, 0);
            this.tableLP.Controls.Add(this.label1, 0, 0);
            this.tableLP.Location = new System.Drawing.Point(0, 0);
            this.tableLP.Margin = new System.Windows.Forms.Padding(0);
            this.tableLP.Name = "tableLP";
            this.tableLP.RowCount = 1;
            this.tableLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLP.Size = new System.Drawing.Size(641, 29);
            this.tableLP.TabIndex = 18;
            // 
            // lblCurValue
            // 
            this.lblCurValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurValue.ForeColor = System.Drawing.Color.GreenYellow;
            this.lblCurValue.Location = new System.Drawing.Point(99, 1);
            this.lblCurValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblCurValue.Name = "lblCurValue";
            this.lblCurValue.Size = new System.Drawing.Size(28, 27);
            this.lblCurValue.TabIndex = 15;
            this.lblCurValue.Text = "?";
            this.lblCurValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCostBasic
            // 
            this.lblCostBasic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCostBasic.ForeColor = System.Drawing.Color.GreenYellow;
            this.lblCostBasic.Location = new System.Drawing.Point(262, 1);
            this.lblCostBasic.Margin = new System.Windows.Forms.Padding(0);
            this.lblCostBasic.Name = "lblCostBasic";
            this.lblCostBasic.Size = new System.Drawing.Size(126, 27);
            this.lblCostBasic.TabIndex = 15;
            this.lblCostBasic.Text = "?";
            this.lblCostBasic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurPrecent
            // 
            this.lblCurPrecent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurPrecent.ForeColor = System.Drawing.Color.GreenYellow;
            this.lblCurPrecent.Location = new System.Drawing.Point(192, 1);
            this.lblCurPrecent.Margin = new System.Windows.Forms.Padding(0);
            this.lblCurPrecent.Name = "lblCurPrecent";
            this.lblCurPrecent.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblCurPrecent.Size = new System.Drawing.Size(69, 27);
            this.lblCurPrecent.TabIndex = 15;
            this.lblCurPrecent.Text = "?";
            this.lblCurPrecent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGainPercent
            // 
            this.lblGainPercent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGainPercent.ForeColor = System.Drawing.Color.GreenYellow;
            this.lblGainPercent.Location = new System.Drawing.Point(163, 1);
            this.lblGainPercent.Margin = new System.Windows.Forms.Padding(0);
            this.lblGainPercent.Name = "lblGainPercent";
            this.lblGainPercent.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblGainPercent.Size = new System.Drawing.Size(28, 27);
            this.lblGainPercent.TabIndex = 15;
            this.lblGainPercent.Text = "?";
            this.lblGainPercent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGainDoll
            // 
            this.lblGainDoll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGainDoll.ForeColor = System.Drawing.Color.GreenYellow;
            this.lblGainDoll.Location = new System.Drawing.Point(128, 1);
            this.lblGainDoll.Margin = new System.Windows.Forms.Padding(0);
            this.lblGainDoll.Name = "lblGainDoll";
            this.lblGainDoll.Size = new System.Drawing.Size(34, 27);
            this.lblGainDoll.TabIndex = 15;
            this.lblGainDoll.Text = "?";
            this.lblGainDoll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "TOTALS:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpReturn
            // 
            this.grpReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpReturn.Controls.Add(this.BtnReturn);
            this.grpReturn.Controls.Add(this.combo2);
            this.grpReturn.Controls.Add(this.combo1);
            this.grpReturn.Controls.Add(this.label4);
            this.grpReturn.Controls.Add(this.label3);
            this.grpReturn.Controls.Add(this.lblVal2);
            this.grpReturn.Controls.Add(this.lblVal1);
            this.grpReturn.Controls.Add(this.lblReturn);
            this.grpReturn.Location = new System.Drawing.Point(1170, 389);
            this.grpReturn.Name = "grpReturn";
            this.grpReturn.Size = new System.Drawing.Size(302, 128);
            this.grpReturn.TabIndex = 17;
            this.grpReturn.TabStop = false;
            this.grpReturn.Text = "Calculate simple  return %";
            // 
            // BtnReturn
            // 
            this.BtnReturn.Location = new System.Drawing.Point(55, 92);
            this.BtnReturn.Name = "BtnReturn";
            this.BtnReturn.Size = new System.Drawing.Size(75, 23);
            this.BtnReturn.TabIndex = 1;
            this.BtnReturn.Text = "Calculate";
            this.BtnReturn.UseVisualStyleBackColor = true;
            this.BtnReturn.Click += new System.EventHandler(this.BtnReturn_Click);
            // 
            // combo2
            // 
            this.combo2.FormattingEnabled = true;
            this.combo2.Location = new System.Drawing.Point(134, 39);
            this.combo2.Name = "combo2";
            this.combo2.Size = new System.Drawing.Size(121, 21);
            this.combo2.TabIndex = 0;
            // 
            // combo1
            // 
            this.combo1.FormattingEnabled = true;
            this.combo1.Location = new System.Drawing.Point(7, 39);
            this.combo1.Name = "combo1";
            this.combo1.Size = new System.Drawing.Size(121, 21);
            this.combo1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(142, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Select End Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Select Start date";
            // 
            // lblVal2
            // 
            this.lblVal2.AutoSize = true;
            this.lblVal2.Location = new System.Drawing.Point(164, 63);
            this.lblVal2.Name = "lblVal2";
            this.lblVal2.Size = new System.Drawing.Size(13, 13);
            this.lblVal2.TabIndex = 15;
            this.lblVal2.Text = "?";
            // 
            // lblVal1
            // 
            this.lblVal1.AutoSize = true;
            this.lblVal1.Location = new System.Drawing.Point(18, 63);
            this.lblVal1.Name = "lblVal1";
            this.lblVal1.Size = new System.Drawing.Size(13, 13);
            this.lblVal1.TabIndex = 15;
            this.lblVal1.Text = "?";
            // 
            // lblReturn
            // 
            this.lblReturn.AutoSize = true;
            this.lblReturn.Location = new System.Drawing.Point(136, 97);
            this.lblReturn.Name = "lblReturn";
            this.lblReturn.Size = new System.Drawing.Size(13, 13);
            this.lblReturn.TabIndex = 15;
            this.lblReturn.Text = "?";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton4);
            this.groupBox2.Controls.Add(this.radioButton5);
            this.groupBox2.Location = new System.Drawing.Point(2333, -196);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(173, 205);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter by SMA (Sleeve)";
            this.groupBox2.Visible = false;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(17, 120);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(139, 17);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Equity Value Stocks";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(17, 97);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(147, 17);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Equity Growth Stocks";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(17, 74);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(147, 17);
            this.radioButton3.TabIndex = 5;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "US Large Cap Stocks";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(17, 28);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(111, 17);
            this.radioButton4.TabIndex = 4;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "All Investments";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(17, 51);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(127, 17);
            this.radioButton5.TabIndex = 4;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Cental Investment";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // grpFilter
            // 
            this.grpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFilter.Controls.Add(this.radioInter);
            this.grpFilter.Controls.Add(this.dataGr);
            this.grpFilter.Controls.Add(this.radioValue);
            this.grpFilter.Controls.Add(this.radioGrowth);
            this.grpFilter.Controls.Add(this.radioUS);
            this.grpFilter.Controls.Add(this.radioAll);
            this.grpFilter.Controls.Add(this.radioCental);
            this.grpFilter.Location = new System.Drawing.Point(1167, 12);
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.Size = new System.Drawing.Size(302, 371);
            this.grpFilter.TabIndex = 16;
            this.grpFilter.TabStop = false;
            this.grpFilter.Text = "Filter by SMA (Sleeve)";
            // 
            // radioInter
            // 
            this.radioInter.AutoSize = true;
            this.radioInter.Location = new System.Drawing.Point(17, 136);
            this.radioInter.Name = "radioInter";
            this.radioInter.Size = new System.Drawing.Size(115, 17);
            this.radioInter.TabIndex = 18;
            this.radioInter.TabStop = true;
            this.radioInter.Text = "Equity International";
            this.radioInter.UseVisualStyleBackColor = true;
            this.radioInter.CheckedChanged += new System.EventHandler(this.radioInter_CheckedChanged);
            // 
            // dataGr
            // 
            this.dataGr.AllowUserToAddRows = false;
            this.dataGr.AllowUserToDeleteRows = false;
            this.dataGr.AllowUserToOrderColumns = true;
            this.dataGr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGr.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12,
            this.Column13});
            this.dataGr.Location = new System.Drawing.Point(9, 168);
            this.dataGr.Name = "dataGr";
            this.dataGr.ReadOnly = true;
            this.dataGr.Size = new System.Drawing.Size(287, 183);
            this.dataGr.TabIndex = 17;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Sleeve";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Width = 70;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "# pos";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Width = 30;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "value";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            this.Column11.Width = 70;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "% of accnt";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            this.Column12.Width = 40;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "gain %";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            this.Column13.Width = 45;
            // 
            // radioValue
            // 
            this.radioValue.AutoSize = true;
            this.radioValue.Location = new System.Drawing.Point(17, 113);
            this.radioValue.Name = "radioValue";
            this.radioValue.Size = new System.Drawing.Size(120, 17);
            this.radioValue.TabIndex = 7;
            this.radioValue.TabStop = true;
            this.radioValue.Text = "Equity Value Stocks";
            this.radioValue.UseVisualStyleBackColor = true;
            this.radioValue.CheckedChanged += new System.EventHandler(this.radioValue_CheckedChanged);
            // 
            // radioGrowth
            // 
            this.radioGrowth.AutoSize = true;
            this.radioGrowth.Location = new System.Drawing.Point(17, 90);
            this.radioGrowth.Name = "radioGrowth";
            this.radioGrowth.Size = new System.Drawing.Size(127, 17);
            this.radioGrowth.TabIndex = 6;
            this.radioGrowth.TabStop = true;
            this.radioGrowth.Text = "Equity Growth Stocks";
            this.radioGrowth.UseVisualStyleBackColor = true;
            this.radioGrowth.CheckedChanged += new System.EventHandler(this.radioGrowth_CheckedChanged);
            // 
            // radioUS
            // 
            this.radioUS.AutoSize = true;
            this.radioUS.Location = new System.Drawing.Point(17, 67);
            this.radioUS.Name = "radioUS";
            this.radioUS.Size = new System.Drawing.Size(128, 17);
            this.radioUS.TabIndex = 5;
            this.radioUS.TabStop = true;
            this.radioUS.Text = "US Large Cap Stocks";
            this.radioUS.UseVisualStyleBackColor = true;
            this.radioUS.CheckedChanged += new System.EventHandler(this.radioUS_CheckedChanged);
            // 
            // radioAll
            // 
            this.radioAll.AutoSize = true;
            this.radioAll.Location = new System.Drawing.Point(17, 21);
            this.radioAll.Name = "radioAll";
            this.radioAll.Size = new System.Drawing.Size(96, 17);
            this.radioAll.TabIndex = 4;
            this.radioAll.TabStop = true;
            this.radioAll.Text = "All Investments";
            this.radioAll.UseVisualStyleBackColor = true;
            this.radioAll.CheckedChanged += new System.EventHandler(this.radioAll_CheckedChanged);
            // 
            // radioCental
            // 
            this.radioCental.AutoSize = true;
            this.radioCental.Location = new System.Drawing.Point(17, 44);
            this.radioCental.Name = "radioCental";
            this.radioCental.Size = new System.Drawing.Size(110, 17);
            this.radioCental.TabIndex = 4;
            this.radioCental.TabStop = true;
            this.radioCental.Text = "Cental Investment";
            this.radioCental.UseVisualStyleBackColor = true;
            this.radioCental.CheckedChanged += new System.EventHandler(this.radioCental_CheckedChanged);
            // 
            // lblPosAmount
            // 
            this.lblPosAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPosAmount.AutoSize = true;
            this.lblPosAmount.Location = new System.Drawing.Point(1433, 539);
            this.lblPosAmount.Name = "lblPosAmount";
            this.lblPosAmount.Size = new System.Drawing.Size(13, 13);
            this.lblPosAmount.TabIndex = 15;
            this.lblPosAmount.Text = "?";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1309, 539);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Total Positions Amount:";
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            this.dataGrid.AllowUserToOrderColumns = true;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column7,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column8});
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Location = new System.Drawing.Point(0, 13);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.Size = new System.Drawing.Size(1146, 610);
            this.dataGrid.TabIndex = 13;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Description";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 400;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Symbol";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 70;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Current Value";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 90;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Total Gain $";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 90;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Total Gain %";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 60;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "% of Account";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 60;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Cost Basic Total";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 90;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Sleeve Name";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 270;
            // 
            // BtnLoadFile
            // 
            this.BtnLoadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnLoadFile.Location = new System.Drawing.Point(1344, 575);
            this.BtnLoadFile.Name = "BtnLoadFile";
            this.BtnLoadFile.Size = new System.Drawing.Size(123, 35);
            this.BtnLoadFile.TabIndex = 12;
            this.BtnLoadFile.Text = "Load Current\r\nAccount from File";
            this.BtnLoadFile.UseVisualStyleBackColor = true;
            this.BtnLoadFile.Click += new System.EventHandler(this.BtnLoadFile_Click);
            // 
            // openFileD
            // 
            this.openFileD.FileName = "openFileD";
            // 
            // FormAcctMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1479, 627);
            this.Controls.Add(this.grpBox);
            this.Controls.Add(this.grpReturn);
            this.Controls.Add(this.lblPosAmount);
            this.Controls.Add(this.grpFilter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnLoadFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormAcctMonitor";
            this.Text = "FormAcctMonitor";
            this.Load += new System.EventHandler(this.FormAcctMonitor_Load);
            this.grpBox.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.tableLP.ResumeLayout(false);
            this.tableLP.PerformLayout();
            this.grpReturn.ResumeLayout(false);
            this.grpReturn.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpFilter.ResumeLayout(false);
            this.grpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBox;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.Button BtnLoadFile;
        private System.Windows.Forms.OpenFileDialog openFileD;
        private System.Windows.Forms.Label lblGainDoll;
        private System.Windows.Forms.Label lblCurValue;
        private System.Windows.Forms.Label lblCostBasic;
        private System.Windows.Forms.Label lblCurPrecent;
        private System.Windows.Forms.Label lblGainPercent;
        private System.Windows.Forms.Label lblPosAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpFilter;
        private System.Windows.Forms.RadioButton radioValue;
        private System.Windows.Forms.RadioButton radioGrowth;
        private System.Windows.Forms.RadioButton radioUS;
        private System.Windows.Forms.RadioButton radioCental;
        private System.Windows.Forms.RadioButton radioAll;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.DataGridView dataGr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.GroupBox grpReturn;
        private System.Windows.Forms.ComboBox combo2;
        private System.Windows.Forms.ComboBox combo1;
        private System.Windows.Forms.Button BtnReturn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblReturn;
        private System.Windows.Forms.Label lblVal2;
        private System.Windows.Forms.Label lblVal1;
        private System.Windows.Forms.RadioButton radioInter;
        private System.Windows.Forms.TableLayoutPanel tableLP;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.Label label1;
    }
}