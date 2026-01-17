using SLGAutomationLib;

using System;

using System.Collections.Generic;

using System.Collections.Specialized;

using System.ComponentModel;

using System.Data;

using System.Drawing;

using System.Linq;

using System.Security.Principal;

using System.Text;

using System.Threading.Tasks;

using System.Windows.Forms;

using System.IO;

using System.Globalization;



public struct AccountStruct

{

    public string Symbol { get; set; }

    public string Description { get; set; }

    public string Sleeve { get; set; }

    public double CurrentValue { get; set; }

    public double TotalGainD { get; set; }

    public double TotalGainPer { get; set; }

    public double CurrentValuePercent { get; set; }

    public double CostBasic { get; set; }

}



namespace WinFinanceApp

{

    public partial class FormAcctMonitor : Form

    {

        protected Logger _logger;

        Ini inif;

        protected CMyFinance MyFinance;



        List<AccountStruct> dataList = new List<AccountStruct>();

        List<AccountStruct> dataCental = new List<AccountStruct>();

        List<AccountStruct> dataUS = new List<AccountStruct>();

        List<AccountStruct> dataGrowth = new List<AccountStruct>();

        List<AccountStruct> dataValue = new List<AccountStruct>();

        List<AccountStruct> dataInter = new List<AccountStruct>();

        string account = "Account Monitor";

        string AccDirectory = null;



        public FormAcctMonitor()

        {

            InitializeComponent();



            // Wire these up once here. 

            // If these names are correct in the designer, they will work.

            dataGrid.ColumnWidthChanged += (s, e) => AdjustLayout();

            dataGrid.Scroll += (s, e) => AdjustLayout();



            // Also trigger on ColumnDisplayIndexChanged in case user reorders columns

            dataGrid.ColumnDisplayIndexChanged += (s, e) => AdjustLayout();

        }



        private void FormAcctMonitor_Load(object sender, EventArgs e)

        {

            // 1. Initialize Backend Objects

            this._logger = Logger.Instance;

            inif = new Ini(_logger.SetupPath);

            this.MyFinance = CMyFinance.Instance;



            // 2. Set Initial UI State

            this.grpReturn.Visible = false;

            this.radioAll.Checked = true;

            tableLP.Parent = panelFooter;



            // 3. Wire up Grid Events for Footer Alignment

            // Trigger alignment when the user scrolls

            dataGrid.Scroll += (s, se) =>

            {

                AdjustLayout();

            };



            // Trigger alignment when columns are resized manually

            dataGrid.ColumnWidthChanged += (s, ce) =>

            {

                AdjustLayout();

            };



            // Trigger alignment if columns are reordered or visibility changes

            dataGrid.ColumnDisplayIndexChanged += (s, ce) =>

            {

                AdjustLayout();

            };



            // 4. Force an initial layout calculation

            // We use BeginInvoke to let the form finish its internal layout 

            // before we try to calculate pixel positions.

            this.BeginInvoke(new MethodInvoker(() => {

                AdjustLayout();

            }));

        }



        // Helper method to properly parse CSV line respecting quoted values

        private string[] SplitCsvLine(string line)

        {

            List<string> result = new List<string>();

            bool inQuotes = false;

            StringBuilder currentValue = new StringBuilder();



            for (int i = 0; i < line.Length; i++)

            {

                char c = line[i];



                if (c == '"')

                {

                    inQuotes = !inQuotes;

                }

                else if (c == ',' && !inQuotes)

                {

                    result.Add(currentValue.ToString());

                    currentValue.Clear();

                }

                else

                {

                    currentValue.Append(c);

                }

            }



            result.Add(currentValue.ToString());

            return result.ToArray();

        }



        // Helper method to parse CSV values with $, %, commas, parentheses

        private double ParseCsvValue(string value)

        {

            if (string.IsNullOrWhiteSpace(value) || value.Trim() == "--")

                return 0.0;



            value = value.Trim();

            bool isNegative = value.StartsWith("(") && value.EndsWith(")");

            if (isNegative)

            {

                value = value.Substring(1, value.Length - 2);

            }



            value = value.Replace("$", "").Replace("%", "").Replace(",", "").Trim();



            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))

            {

                return isNegative ? -result : result;

            }

            return 0.0;

        }



        // Helper method to detect end of data (disclaimer text)

        private bool IsEndOfDataLine(string line)

        {

            if (string.IsNullOrWhiteSpace(line))

                return true;



            string[] columns = line.Split(',');

            if (columns.Length == 0)

                return true;



            string firstColumn = columns[0].Trim();



            if (string.IsNullOrEmpty(firstColumn))

                return true;



            if (firstColumn.StartsWith("\""))

                return true;



            if (firstColumn.StartsWith("The data and information") ||

                firstColumn.StartsWith("Brokerage services") ||

                firstColumn.StartsWith("Date downloaded"))

                return true;



            return false;

        }



        private void BtnLoadFile_Click(object sender, EventArgs e)

        {

            LoadDataFromCSV(sender, e);

        }



        private void LoadData()

        {

            try

            {

                string[] substrs = { "Central", "US Large", "Equity Growth", "Equity Value", "International" };

                dataGrid.Rows.Clear();

                dataGr.Rows.Clear();

                dataCental = dataList.Where(account => account.Sleeve.Contains(substrs[0])).ToList();

                dataUS = dataList.Where(account => account.Sleeve.Contains(substrs[1])).ToList();

                dataGrowth = dataList.Where(account => account.Sleeve.Contains(substrs[2])).ToList();

                dataValue = dataList.Where(account => account.Sleeve.Contains(substrs[3])).ToList();

                dataInter = dataList.Where(account => account.Sleeve.Contains(substrs[4])).ToList();



                List<AccountStruct> tempData = new List<AccountStruct>();

                if (radioAll.Checked || !grpFilter.Visible)

                {

                    tempData = dataList;

                }

                else if (radioUS.Checked)

                {

                    tempData = dataUS;

                }

                else if (radioCental.Checked)

                {

                    tempData = dataCental;

                }

                else if (radioGrowth.Checked)

                {

                    tempData = dataGrowth;

                }

                else if (radioInter.Checked)

                {

                    tempData = dataInter;

                }

                else

                {

                    tempData = dataValue;

                }



                foreach (AccountStruct account in tempData)

                {

                    this.dataGrid.Rows.Add(account.Description, account.Symbol, account.CurrentValue, account.TotalGainD, account.TotalGainPer, account.CurrentValuePercent, account.CostBasic, account.Sleeve);

                }



                // --- Calculate Footer Totals ---

                double val1 = tempData.Sum(account => account.CurrentValue);

                lblCurValue.Text = val1.ToString("C1");



                double val2 = tempData.Sum(account => account.TotalGainD);

                lblGainDoll.Text = val2.ToString("C1");



                lblGainPercent.Text = string.Format("{0:0.0}%", (val1 - val2 != 0) ? (val2 / (val1 - val2) * 100) : 0);



                val1 = tempData.Sum(account => account.CostBasic);

                lblCostBasic.Text = val1.ToString("C1");



                val1 = tempData.Sum(account => account.CurrentValuePercent);

                lblCurPrecent.Text = string.Format("{0:0.0}%", val1);



                lblPosAmount.Text = tempData.Count.ToString();



                // --- FIX: Ensure Column7 (CurValue) is wide enough for the Footer Sum ---

                using (Graphics g = lblCurValue.CreateGraphics())

                {

                    // Measure the pixel width of the sum string + small buffer

                    int requiredWidth = (int)g.MeasureString(lblCurValue.Text, lblCurValue.Font).Width + 15;



                    // If the grid column is narrower than our footer sum, expand the grid column

                    if (dataGrid.Columns["Column7"].Width < requiredWidth)

                    {

                        dataGrid.Columns["Column7"].Width = requiredWidth;

                    }

                }



                // --- Summary Grid (dataGr) Logic ---

                double totalVal = dataList.Sum(account => account.CurrentValue);

                double v1 = 0;

                double v2 = dataList.Sum(account => account.CostBasic);



                v1 = dataCental.Sum(account => account.CurrentValue);

                this.dataGr.Rows.Add(substrs[0], dataCental.Count, v1, Math.Round(v1 / totalVal * 100, 1), Math.Round(dataCental.Sum(account => account.TotalGainD) / (v2 != 0 ? v2 : 1) * 100, 1));



                v1 = dataUS.Sum(account => account.CurrentValue);

                this.dataGr.Rows.Add(substrs[1], dataUS.Count, v1, Math.Round(v1 / totalVal * 100, 1), Math.Round(dataUS.Sum(account => account.TotalGainD) / (v2 != 0 ? v2 : 1) * 100, 1));



                v1 = dataGrowth.Sum(account => account.CurrentValue);

                this.dataGr.Rows.Add(substrs[2], dataGrowth.Count, v1, Math.Round(v1 / totalVal * 100, 1), Math.Round(dataGrowth.Sum(account => account.TotalGainD) / (v2 != 0 ? v2 : 1) * 100, 1));



                v1 = dataValue.Sum(account => account.CurrentValue);

                this.dataGr.Rows.Add(substrs[3], dataValue.Count, v1, Math.Round(v1 / totalVal * 100, 1), Math.Round(dataValue.Sum(account => account.TotalGainD) / (v2 != 0 ? v2 : 1) * 100, 1));

                v1 = dataInter.Sum(account => account.CurrentValue);

                this.dataGr.Rows.Add(substrs[4], dataInter.Count, v1, Math.Round(v1 / totalVal * 100, 1),

                    Math.Round(dataInter.Sum(account => account.TotalGainD) / (v2 != 0 ? v2 : 1) * 100, 1));

                //v1 = dataValue.Sum(account => account.CurrentValue);

                //this.dataGr.Rows.Add(substrs[3], dataValue.Count, v1, Math.Round(v1 / totalVal * 100, 2), Math.Round(dataValue.Sum(account => account.TotalGainD) / (v2 != 0 ? v2 : 1) * 100, 2));

                dataGrid.PerformLayout(); // Forces the grid to recalculate scrollbars

                // --- Final Layout Sync ---

                AdjustLayout();

            }

            catch (Exception ex)

            {

                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);

            }

        }





        private void LoadDataFromCSV(object sender, EventArgs e)

        {

            try

            {

                AccountStruct data = new AccountStruct();

                data.Description = "N/A";

                data.Sleeve = "N/A";

                dataGrid.Rows.Clear();

                dataGr.Rows.Clear();

                dataList.Clear();

                this.combo1.Items.Clear();

                this.combo2.Items.Clear();

                this.combo1.SelectedIndex = -1;

                this.combo2.SelectedIndex = -1;

                this.combo2.Text = String.Empty;

                this.combo1.Text = String.Empty;

                lblVal1.Text = String.Empty;

                lblVal2.Text = String.Empty;

                lblReturn.Text = String.Empty;

                this.openFileD.Title = "Load Data from CSV File";



                this.openFileD.CheckFileExists = true;

                this.openFileD.CheckPathExists = true;

                this.openFileD.DefaultExt = "csv";

                this.openFileD.Filter = "CSV files (*.csv)|*.csv";

                this.openFileD.FilterIndex = 1;

                this.openFileD.RestoreDirectory = false;

                this.openFileD.ReadOnlyChecked = true;

                this.openFileD.ShowReadOnly = true;



                if (this.openFileD.ShowDialog() == DialogResult.OK)

                {

                    string[] strs = System.IO.File.ReadAllLines(this.openFileD.FileName);



                    double gain = 0, gainPercent = 0, curPercent = 0, costBasic = 0, curValue = 0;

                    bool foundHeader = false;



                    foreach (string s in strs)

                    {

                        // Skip until we find the header row

                        if (!foundHeader)

                        {

                            if (s.Contains("Account Number") || s.Contains("Account Name") || s.Contains("Symbol"))

                            {

                                foundHeader = true;

                            }

                            continue;

                        }



                        // Stop parsing if we hit disclaimer text

                        if (IsEndOfDataLine(s))

                            break;



                        string[] str = SplitCsvLine(s);



                        if (str.Length < 8)

                            continue;



                        curPercent = gain = gainPercent = curValue = costBasic = 0;

                        account = string.Format("{0} #{1}", str[1], str[0]);



                        if (str.Any(ss => ss.Equals("Pending Activity", StringComparison.OrdinalIgnoreCase)))

                        {

                            curValue = ParseCsvValue(str[7]);

                            data.CurrentValue = curValue;

                            dataList.Add(data);

                            continue;

                        }



                        if (!s.Contains("Joint"))

                        {

                            curPercent = ParseCsvValue(str[12]);

                            curValue = ParseCsvValue(str[7]);



                            if (str.Length > 10 && !string.IsNullOrEmpty(str[10]) && str[10] != "--")

                            {

                                gain = ParseCsvValue(str[10]);

                                gainPercent = ParseCsvValue(str[11]);

                                costBasic = ParseCsvValue(str[13]);

                            }

                            else

                            {

                                gain = 0;

                                gainPercent = 0;

                                costBasic = 0;

                            }



                            data.Description = str[3];

                            data.Symbol = str[2];

                            data.CurrentValue = curValue;

                            data.TotalGainD = gain;

                            data.TotalGainPer = gainPercent;

                            data.CurrentValuePercent = curPercent;

                            data.CostBasic = costBasic;

                            dataList.Add(data);

                        }

                        else

                        {

                            if (str.Length == 17)

                            {

                                curPercent = ParseCsvValue(str[13]);

                                curValue = ParseCsvValue(str[8]);



                                if (str.Length > 10 && !string.IsNullOrEmpty(str[10]) && str[10] != "--")

                                {

                                    gain = ParseCsvValue(str[11]);

                                    gainPercent = ParseCsvValue(str[12]);

                                    costBasic = ParseCsvValue(str[14]);

                                }

                                else

                                {

                                    gain = 0;

                                    gainPercent = 0;

                                    costBasic = 0;

                                }



                                data.Description = str[4];

                                data.Symbol = str[3];

                                data.CurrentValue = curValue;

                                data.TotalGainD = gain;

                                data.TotalGainPer = gainPercent;

                                data.CurrentValuePercent = curPercent;

                                data.CostBasic = costBasic;

                                data.Sleeve = str[2];

                                dataList.Add(data);

                            }

                            else

                            {

                                curPercent = ParseCsvValue(str[14]);

                                curValue = ParseCsvValue(str[9]);



                                if (str.Length > 11 && !string.IsNullOrEmpty(str[11]) && str[11] != "--")

                                {

                                    gain = ParseCsvValue(str[12]);

                                    gainPercent = ParseCsvValue(str[13]);

                                    costBasic = ParseCsvValue(str[15]);

                                }

                                else

                                {

                                    gain = 0;

                                    gainPercent = 0;

                                    costBasic = 0;

                                }



                                data.Description = str[4];

                                data.Symbol = str[3];

                                data.CurrentValue = curValue;

                                data.TotalGainD = gain;

                                data.TotalGainPer = gainPercent;

                                data.CurrentValuePercent = curPercent;

                                data.CostBasic = costBasic;

                                data.Sleeve = str[2];

                                dataList.Add(data);

                            }

                        }

                    }



                    this.grpFilter.Visible = !dataList[0].Sleeve.Contains("N/A") ? true : false;

                    this.LoadData();

                    grpBox.Text = string.Format("Account Monitor: {0}                   {1}", account, Path.GetFileName(this.openFileD.FileName));

                    AccDirectory = Path.GetDirectoryName(this.openFileD.FileName);

                    string filen = Path.GetFileName(this.openFileD.FileName);



                    this.grpReturn.Visible = true;



                    var sortedFiles = Directory.GetFiles(AccDirectory)

                    .Select(path => new FileInfo(path))

                    .OrderBy(f => f.LastWriteTime)

                    .ToList();

                    foreach (var file in sortedFiles)

                    {

                        string[] str = file.Name.Split('_');

                        combo1.Items.Add(str[2]);

                    }

                    sortedFiles = Directory.GetFiles(AccDirectory)

                   .Select(path => new FileInfo(path))

                   .OrderByDescending(f => f.LastWriteTime)

                   .ToList();

                    foreach (var file in sortedFiles)

                    {

                        string[] str = file.Name.Split('_');

                        combo2.Items.Add(str[2]);

                    }

                }

                else

                {

                    grpBox.Text = string.Format("Account Monitor: ?                   ?");

                    AccDirectory = null;

                    this.grpReturn.Visible = false;

                    this.combo1.Items.Clear();

                    this.combo2.Items.Clear();

                }

            }

            catch (Exception ex)

            {

                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);

            }

        }



        private void radioAll_CheckedChanged(object sender, EventArgs e)

        {

            if (radioAll.Checked)

            {

                LoadData();

            }

        }



        private void radioCental_CheckedChanged(object sender, EventArgs e)

        {
            if (radioCental.Checked)

            {

                LoadData();

            }



        }



        private void radioUS_CheckedChanged(object sender, EventArgs e)

        {

            if (radioUS.Checked)

            {

                LoadData();

            }

        }



        private void radioGrowth_CheckedChanged(object sender, EventArgs e)

        {

            if (radioGrowth.Checked)

            {

                LoadData();

            }

        }



        private void radioValue_CheckedChanged(object sender, EventArgs e)

        {

            if (radioValue.Checked)

            {

                LoadData();

            }

        }



        private void BtnReturn_Click(object sender, EventArgs e)

        {

            try

            {

                string s1 = this.combo1.SelectedItem.ToString();

                string s2 = this.combo2.SelectedItem.ToString();



                if (s1 == "" || s2 == "")

                {

                    throw new Exception("Invalid item selection");

                }

                double val1 = this.GetTotal("Portfolio_Positions_" + s1);

                double val2 = this.GetTotal("Portfolio_Positions_" + s2);

                lblVal1.Text = "$" + val1.ToString();

                lblVal2.Text = "$" + val2.ToString();

                double ret = Math.Round((val2 - val1) / val1 * 100, 2);



                lblReturn.Text = "Simple Return =" + ret.ToString() + "%";

                string sub = Path.GetFileName(AccDirectory);

                this._logger.SentEvent(string.Format("Simple return from {0}={1}%", sub, ret), Logger.EnumLogLevel.INFO_LEVEL);

            }

            catch (Exception ex)

            {

                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);

            }

        }



        private double GetTotal(string name)

        {

            try

            {

                double r_val = 0;

                string fullName = Path.Combine(AccDirectory, name);

                string[] strs = System.IO.File.ReadAllLines(fullName);



                double curValue = 0;

                bool foundHeader = false;



                foreach (string s in strs)

                {

                    // Skip until we find the header row

                    if (!foundHeader)

                    {

                        if (s.Contains("Account Number") || s.Contains("Account Name") || s.Contains("Symbol"))

                        {

                            foundHeader = true;

                        }

                        continue;

                    }



                    // Stop parsing if we hit disclaimer text

                    if (IsEndOfDataLine(s))

                        break;



                    string[] str = SplitCsvLine(s);



                    if (str.Length < 8)

                        continue;



                    curValue = 0;



                    if (str.Any(ss => ss.Equals("Pending Activity", StringComparison.OrdinalIgnoreCase)))

                    {

                        curValue = ParseCsvValue(str[7]);

                        r_val += curValue;

                        continue;

                    }



                    if (!s.Contains("Joint"))

                    {

                        curValue = ParseCsvValue(str[7]);

                    }

                    else

                    {

                        if (str.Length == 17)

                        {

                            curValue = ParseCsvValue(str[8]);

                        }

                        else

                        {

                            curValue = ParseCsvValue(str[9]);

                        }

                    }

                    r_val += curValue;

                }



                return Math.Round(r_val, 2);

            }

            catch (Exception ex)

            {

                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);

                return 0;

            }

        }



        private void radioInter_CheckedChanged(object sender, EventArgs e)

        {

            if (radioInter.Checked)

            {

                LoadData();

            }

        }

        private void FormAcctMonitor_Resize(object sender, EventArgs e)

        {

            AdjustLayout();

        }



        private void AdjustLayout()

        {

            if (dataGrid == null || tableLP == null || panelFooter == null) return;



            tableLP.SuspendLayout();



            // 1. Position

            int rowHeaderWidth = dataGrid.RowHeadersVisible ? dataGrid.RowHeadersWidth : 0;

            tableLP.Left = rowHeaderWidth - dataGrid.HorizontalScrollingOffset;

            tableLP.Top = 0;

            tableLP.Height = panelFooter.Height;



            // 2. Styling the Table

            tableLP.BackColor = Color.WhiteSmoke; // Softer than pure white

            tableLP.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;



            // 3. Sync Columns

            for (int i = 0; i < dataGrid.Columns.Count; i++)

            {

                if (i >= tableLP.ColumnStyles.Count) break;



                var gridCol = dataGrid.Columns[i];

                tableLP.ColumnStyles[i].SizeType = SizeType.Absolute;

                tableLP.ColumnStyles[i].Width = gridCol.Width;



                Control ctrl = tableLP.GetControlFromPosition(i, 0);

                if (ctrl != null)

                {

                    ctrl.Visible = gridCol.Visible;



                    if (ctrl is Label lbl)

                    {

                        lbl.AutoSize = false;

                        lbl.Dock = DockStyle.Fill;

                        lbl.TextAlign = ContentAlignment.MiddleRight;



                        // Color logic: Make it look like a real spreadsheet footer

                        lbl.ForeColor = Color.MidnightBlue;

                        lbl.Font = new Font("Segoe UI", 9f, FontStyle.Bold);



                        // If this is the 'Totals' label in column 0

                        if (i == 0)

                        {

                            lbl.Text = "TOTALS:";

                            lbl.TextAlign = ContentAlignment.MiddleLeft;

                        }

                    }

                }

            }



            int totalContentWidth = dataGrid.Columns.GetColumnsWidth(DataGridViewElementStates.Visible);

            tableLP.Width = totalContentWidth + 100;



            tableLP.ResumeLayout(false);

        }



        private void SyncColumn(int tableIndex, string gridColName)

        {

            if (tableLP.ColumnStyles.Count <= tableIndex || !dataGrid.Columns.Contains(gridColName)) return;



            var col = dataGrid.Columns[gridColName];

            Control ctrl = tableLP.GetControlFromPosition(tableIndex, 0);



            // Get the rectangle of the column header to see how much of it is actually visible

            Rectangle rect = dataGrid.GetColumnDisplayRectangle(col.Index, true);



            if (col.Visible && rect.Width > 0)

            {

                // Set the TLP column width to match the DISPLAYED width of the grid column

                // This handles horizontal scrolling automatically

                tableLP.ColumnStyles[tableIndex].SizeType = SizeType.Absolute;

                tableLP.ColumnStyles[tableIndex].Width = rect.Width;



                if (ctrl is Label lbl)

                {

                    lbl.Visible = true;

                    lbl.AutoSize = false;

                    lbl.TextAlign = ContentAlignment.MiddleRight;

                    lbl.Dock = DockStyle.Fill;

                }

            }

            else

            {

                // Hide the column if it's scrolled out of view or set to invisible

                tableLP.ColumnStyles[tableIndex].Width = 0;

                if (ctrl != null) ctrl.Visible = false;

            }

        }

        private string GetColumnNameByIndex(int index)

        {

            switch (index)

            {

                case 0: return "Column1"; // Description

                case 1: return "Column2"; // Symbol

                case 2: return "Column7"; // Current Value

                case 3: return "Column3"; // Gain $

                case 4: return "Column4"; // Gain %

                case 5: return "Column5"; // Value %

                case 6: return "Column6"; // Cost Basis

                case 7: return "Column8"; // Sleeve

                default: return "";

            }

        }



    }

}