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
        protected Logger _logger = Logger.Instance;
        protected CMyFinance MyFinance = CMyFinance.Instance;
        Ini inif;

        List<AccountStruct> dataList = new List<AccountStruct>();
        string AccDirectory = null;

        public FormAcctMonitor()
        {
            InitializeComponent();

            // Force these to stay visible on top of the Dock.Fill area
            this.grpFilter?.BringToFront();
            this.grpReturn?.BringToFront();
        }

        private void FormAcctMonitor_Load(object sender, EventArgs e)
        {
            inif = new Ini(_logger.SetupPath);

            // Ensure they are attached to the Form, not nested inside other boxes
            if (this.grpFilter != null)
            {
                this.grpFilter.Parent = this;
                this.grpFilter.BringToFront();
            }
            if (this.grpReturn != null)
            {
                this.grpReturn.Parent = this;
               // this.grpReturn.Visible = false;
            }

            this.radioAll.Checked = true;

            if (panelFooter != null && tableLP != null)
            {
                tableLP.Parent = panelFooter;
                panelFooter.BackColor = Color.WhiteSmoke;
            }

            this.BeginInvoke(new MethodInvoker(delegate
            {
                AdjustLayout();
            }));
        }

        public void AdjustLayout()
        {
            if (dataGrid == null || tableLP == null || panelFooter == null) return;

            tableLP.SuspendLayout();

            int rowHeaderWidth = dataGrid.RowHeadersVisible ? dataGrid.RowHeadersWidth : 0;
            tableLP.Left = dataGrid.Left + rowHeaderWidth - dataGrid.HorizontalScrollingOffset;
            tableLP.Top = 0;
            tableLP.Height = panelFooter.Height;

            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                if (i >= tableLP.ColumnStyles.Count) break;

                DataGridViewColumn gridColumn = dataGrid.Columns[i];
                tableLP.ColumnStyles[i].SizeType = SizeType.Absolute;
                tableLP.ColumnStyles[i].Width = gridColumn.Width;

                Control control = tableLP.GetControlFromPosition(i, 0);
                if (control is Label)
                {
                    Label lbl = (Label)control;
                    lbl.Visible = gridColumn.Visible;
                    lbl.AutoSize = false;
                    lbl.Dock = DockStyle.Fill;

                    if (i == 0)
                    {
                        lbl.Text = "TOTALS:";
                        lbl.ForeColor = Color.MidnightBlue;
                        lbl.TextAlign = ContentAlignment.MiddleLeft;
                    }
                    else
                    {
                        lbl.ForeColor = Color.Black;
                        lbl.TextAlign = ContentAlignment.MiddleRight;
                    }
                    lbl.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
            }

            tableLP.Width = dataGrid.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + 250;
            tableLP.ResumeLayout(false);
        }

        private void LoadData()
        {
            try
            {
                dataGrid?.Rows.Clear();
                dataGr?.Rows.Clear();

                var filteredData = dataList.Where(a =>
                    radioAll.Checked ||
                    (radioCental.Checked && a.Sleeve.Contains("Central")) ||
                    (radioUS.Checked && a.Sleeve.Contains("US Large")) ||
                    (radioGrowth.Checked && a.Sleeve.Contains("Equity Growth")) ||
                    (radioInter.Checked && a.Sleeve.Contains("International")) ||
                    (radioValue.Checked && a.Sleeve.Contains("Equity Value"))
                ).ToList();

                foreach (var a in filteredData)
                {
                    this.dataGrid.Rows.Add(a.Description, a.Symbol, a.CurrentValue, a.TotalGainD, a.TotalGainPer, a.CurrentValuePercent, a.CostBasic, a.Sleeve);
                }

                double totalCurrentValue = filteredData.Sum(a => a.CurrentValue);
                double totalGainD = filteredData.Sum(a => a.TotalGainD);
                double totalCostBasic = filteredData.Sum(a => a.CostBasic);
                double totalCurrentValuePercent = filteredData.Sum(a => a.CurrentValuePercent);

                lblCurValue.Text = totalCurrentValue.ToString("C1");
                lblGainDoll.Text = totalGainD.ToString("C1");
                lblCostBasic.Text = totalCostBasic.ToString("C1");
                lblCurPrecent.Text = string.Format("{0:0.0}%", totalCurrentValuePercent);
                lblPosAmount.Text = filteredData.Count.ToString();

                double denominator = totalCurrentValue - totalGainD;
                if (denominator != 0)
                    lblGainPercent.Text = string.Format("{0:0.0}%", (totalGainD / denominator) * 100);
                else
                    lblGainPercent.Text = "0.0%";

                string[] sleeveGroups = { "Central", "US Large", "Equity Growth", "Equity Value", "International" };
                foreach (string groupName in sleeveGroups)
                {
                    var groupData = dataList.Where(x => x.Sleeve.Contains(groupName)).ToList();
                    if (groupData.Count > 0)
                    {
                        double gVal = groupData.Sum(x => x.CurrentValue);
                        double gGain = groupData.Sum(x => x.TotalGainD);
                        double gWeight = (totalCurrentValue != 0) ? (gVal / totalCurrentValue) * 100 : 0;
                        double gReturn = (totalCostBasic != 0) ? (gGain / totalCostBasic) * 100 : 0;

                        this.dataGr.Rows.Add(groupName, groupData.Count, gVal, Math.Round(gWeight, 2), Math.Round(gReturn, 2));
                    }
                }

                AdjustLayout();
            }
            catch (Exception ex)
            {
                _logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);
            }
        }

        private void LoadDataFromCSV(object sender, EventArgs e)
        {
            if (this.openFileD.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dataList.Clear();
                    string[] lines = File.ReadAllLines(this.openFileD.FileName);
                    bool headerFound = false;

                    foreach (string line in lines)
                    {
                        if (!headerFound)
                        {
                            if (line.Contains("Symbol")) headerFound = true;
                            continue;
                        }

                        if (IsEndOfDataLine(line)) break;
                        //if last character in line is coma, add a space to prevent split issues
                        string item = line;
                        if (item.EndsWith(",")& !item.Contains("Pending activity"))
                        {
                            item = item.TrimEnd(',') + " ";
                        }
                        string[] parts = SplitCsvLine(item);
                        if (parts.Length < 8) continue;

                        AccountStruct data = new AccountStruct();
                        bool isJoint = line.Contains("Joint");

                        int valIdx = isJoint ? (parts.Length == 17 ? 8 : 9) : 7;
                        int gainIdx = isJoint ? (parts.Length == 17 ? 11 : 12) : 10;
                        int percIdx = isJoint ? (parts.Length == 17 ? 12 : 13) : 11;
                        int curPercIdx = isJoint ? (parts.Length == 17 ? 13 : 14) : 12;
                        int costIdx = isJoint ? (parts.Length == 17 ? 14 : 15) : 13;

                        data.Symbol = isJoint ? parts[3] : parts[2];
                        data.Description = isJoint ? parts[4] : parts[3];
                        data.Sleeve = isJoint ? parts[2] : "N/A";
                        data.CurrentValue = ParseCsvValue(parts[valIdx]);

                        if (parts.Length > gainIdx && parts[gainIdx] != "--")
                        {
                            data.TotalGainD = ParseCsvValue(parts[gainIdx]);
                            data.TotalGainPer = ParseCsvValue(parts[percIdx]);
                            data.CurrentValuePercent = ParseCsvValue(parts[curPercIdx]);
                            data.CostBasic = ParseCsvValue(parts[costIdx]);
                        }

                        dataList.Add(data);
                    }

                    this.AccDirectory = Path.GetDirectoryName(this.openFileD.FileName);
                    UpdateDateCombos();
                    this.LoadData();
                }
                catch (Exception ex)
                {
                    _logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);
                }
            }
        }

        private void UpdateDateCombos()
        {
            if (string.IsNullOrEmpty(AccDirectory)) return;
            combo1.Items.Clear();
            combo2.Items.Clear();

            DirectoryInfo d = new DirectoryInfo(AccDirectory);
            FileInfo[] files = d.GetFiles("Portfolio_Positions_*.csv").OrderBy(f => f.LastWriteTime).ToArray();

            foreach (FileInfo f in files)
            {
                string name = Path.GetFileNameWithoutExtension(f.Name).Replace("Portfolio_Positions_", "");
                combo1.Items.Add(name);
                combo2.Items.Insert(0, name);
            }
        }

        private double ParseCsvValue(string val)
        {
            if (string.IsNullOrWhiteSpace(val) || val.Trim() == "--") return 0;
            string clean = val.Replace("$", "").Replace("%", "").Replace(",", "").Trim();
            bool isNegative = clean.StartsWith("(") && clean.EndsWith(")");
            if (isNegative) clean = clean.Substring(1, clean.Length - 2);

            double result;
            if (double.TryParse(clean, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return isNegative ? -result : result;
            return 0;
        }

        private string[] SplitCsvLine(string line)
        {
            List<string> parts = new List<string>();
            bool inQuotes = false;
            StringBuilder currentPart = new StringBuilder();

            foreach (char c in line)
            {
                if (c == '\"') inQuotes = !inQuotes;
                else if (c == ',' && !inQuotes)
                {
                    parts.Add(currentPart.ToString());
                    currentPart.Clear();
                }
                else currentPart.Append(c);
            }
            parts.Add(currentPart.ToString());
            return parts.ToArray();
        }

        private bool IsEndOfDataLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return true;
            string firstPart = line.Split(',')[0].Trim();
            return firstPart.StartsWith("\"") || firstPart.Contains("Date downloaded");
        }

        private double GetTotal(string fileName)
        {
            string path = Path.Combine(AccDirectory, fileName);
            if (!File.Exists(path)) return 0;

            double total = 0;
            string[] lines = File.ReadAllLines(path);
            bool headerFound = false;

            foreach (string line in lines)
            {
                if (!headerFound)
                {
                    if (line.Contains("Symbol")) headerFound = true;
                    continue;
                }
                if (IsEndOfDataLine(line)) break;

                string item = line;
                if (item.EndsWith(",") & !item.Contains("Pending activity"))
                {
                    item = item.TrimEnd(',') + " ";
                }
                string[] parts = SplitCsvLine(item);
                //string[] parts = SplitCsvLine(line);
                if (parts.Length < 8) continue;

                bool isJoint = line.Contains("Joint");
                int valIdx = isJoint ? (parts.Length == 17 ? 8 : 9) : 7;
                total += ParseCsvValue(parts[valIdx]);
            }
            return total;
        }

        private void BtnLoadFile_Click(object sender, EventArgs e) => LoadDataFromCSV(sender, e);

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                if (combo1.SelectedItem == null || combo2.SelectedItem == null)
                {
                    if (this.grpReturn != null)
                    {
                        this.grpReturn.Visible = !this.grpReturn.Visible;
                        if (this.grpReturn.Visible) this.grpReturn.BringToFront();
                    }
                    return;
                }

                double val1 = GetTotal("Portfolio_Positions_" + combo1.SelectedItem.ToString() + ".csv");
                double val2 = GetTotal("Portfolio_Positions_" + combo2.SelectedItem.ToString() + ".csv");

                lblVal1.Text = val1.ToString("C2");
                lblVal2.Text = val2.ToString("C2");

                if (val1 != 0)
                    lblReturn.Text = "Return =" + Math.Round((val2 - val1) / val1 * 100, 2) + "%";

                if (this.grpReturn != null)
                {
                    this.grpReturn.Visible = true;
                    this.grpReturn.BringToFront();
                }
            }
            catch (Exception ex)
            {
                _logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);
            }
        }

        private void radioAll_CheckedChanged(object sender, EventArgs e) { if (radioAll.Checked) LoadData(); }
        private void radioCental_CheckedChanged(object sender, EventArgs e) { if (radioCental.Checked) LoadData(); }
        private void radioUS_CheckedChanged(object sender, EventArgs e) { if (radioUS.Checked) LoadData(); }
        private void radioGrowth_CheckedChanged(object sender, EventArgs e) { if (radioGrowth.Checked) LoadData(); }
        private void radioValue_CheckedChanged(object sender, EventArgs e) { if (radioValue.Checked) LoadData(); }
        private void radioInter_CheckedChanged(object sender, EventArgs e) { if (radioInter.Checked) LoadData(); }
    }
}