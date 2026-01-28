using SLGAutomationLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using ScottPlot;

namespace WinFinanceApp
{
    public partial class FormReturn : Form
    {
        protected Logger _logger;
        Ini inif;
        protected CMyFinance MyFinance;
        private FidelityReturnsParser parser;

        private Dictionary<string, Dictionary<string, double?>> annualizedData;
        private List<string> annualizedPeriods;
        private Dictionary<string, string> accountLifeStartDates;
        private Dictionary<string, double?> annualizedTotalData;
        private string annualizedReportDate;

        private bool enableAutoUpdate = false;

        public FormReturn()
        {
            InitializeComponent();
        }

        private void FormReturn_Load(object sender, EventArgs e)
        {
            this._logger = Logger.Instance;
            inif = new Ini(_logger.SetupPath);
            this.MyFinance = CMyFinance.Instance;
            parser = new FidelityReturnsParser();
            this.chkAnnualized.Checked = true;
            AdjustLayout();
        }

        private void FormReturn_Resize(object sender, EventArgs e) => AdjustLayout();

        private void AdjustLayout()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                this.PerformLayout();
                if (fPlot != null && fPlot.Plot != null) fPlot.Refresh();
            }
        }

        private bool IsAnnualizedReturnFormat(string[] lines)
        {
            foreach (string line in lines)
            {
                // SURGERY: Strip quotes for detection
                string cleanLine = line.Replace("\"", "");
                if (cleanLine.Contains("Time-weighted rate of return"))
                {
                    return cleanLine.Contains("1 Month") || cleanLine.Contains("3 Month") || cleanLine.Contains("YTD") ||
                           cleanLine.Contains("1 Year") || cleanLine.Contains("Life of available data");
                }
            }
            return false;
        }

        private bool IsMonthlyReturnFormat(string[] lines)
        {
            foreach (string line in lines)
            {
                // SURGERY: Strip quotes for detection
                string cleanLine = line.Replace("\"", "");
                if (cleanLine.Contains("Time-weighted rate of return"))
                {
                    string[] headers = cleanLine.Split(',');
                    for (int i = 1; i < Math.Min(headers.Length, 5); i++)
                    {
                        string header = headers[i].Trim();
                        if (string.IsNullOrEmpty(header)) continue;
                        if (int.TryParse(header, out _)) return true;
                        if (DateTime.TryParseExact(header, "MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) ||
                            DateTime.TryParseExact(header, "MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            return true;
                    }
                }
            }
            return false;
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            fPlot.Plot.Clear();
            fPlot.Refresh();
            this.openFileD.Reset();
            comboMonths.SelectedItem = null;
            if (this.chkAnnualized.Checked)
                LoadDataFromCSV_Annualized(sender, e);
            else
                LoadDataFromCSV(sender, e);

            if (!this.BtnCalculate.Visible) this.BtnCalculate.Visible = true;
            this.BtnCalculate.PerformClick();
        }

        private void LoadDataFromCSV(object sender, EventArgs e)
        {
            try
            {
                this.openFileD.Title = "Load Data from CSV File";
                this.openFileD.Filter = "CSV files (*.csv)|*.csv";

                if (this.openFileD.ShowDialog() == DialogResult.OK)
                {
                    // SURGERY: Strip quotes immediately upon reading
                    string[] rawLines = File.ReadAllLines(this.openFileD.FileName);
                    string[] strings = rawLines.Select(l => l.Replace("\"", "")).ToArray();

                    parser.ParseTWRCSV(strings);
                    var availableMonths = parser.GetAvailableMonths();
                    this.lblTotalM.Text = availableMonths.Count.ToString();
                    this.numMonths.Maximum = availableMonths.Count;
                    this.numMonths.Enabled = true;
                    this.comboMonths.DataSource = availableMonths;
                    fPlot.Plot.Clear();
                    fPlot.Refresh();
                    groupBox.Text = $"Historic time weighted return (TWR) - {Path.GetFileName(this.openFileD.FileName)}";
                }
            }
            catch (Exception ex) { _logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL); }
        }

        private void comboMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.chkAnnualized.Checked)
            {
                if (comboMonths.SelectedItem != null && annualizedData != null && annualizedData.Count > 0)
                {
                    BtnCalculate.PerformClick();
                }
            }
            else
            {
                int selectedIndex = comboMonths.SelectedIndex;
                numMonths.Maximum = comboMonths.Items.Count - selectedIndex;
                if (comboMonths.SelectedItem != null && parser != null && parser.accounts.Count > 0)
                {
                    BtnCalculate.PerformClick();
                }
            }
        }

        private void numMonths_ValueChanged(object sender, EventArgs e)
        {
            if (!this.chkAnnualized.Checked && comboMonths.SelectedItem != null)
            {
                BtnCalculate.PerformClick();
            }
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboMonths.SelectedItem == null) throw new Exception("No proper file is found");
                string sel = comboMonths.SelectedItem.ToString();

                if (this.chkAnnualized.Checked)
                {
                    if (annualizedData == null) return;
                    Dictionary<string, double?> periodReturns = new Dictionary<string, double?>();
                    foreach (var account in annualizedData)
                    {
                        if (account.Value.ContainsKey(sel)) periodReturns[account.Key] = account.Value[sel];
                    }
                    DisplayAnnualizedChart(periodReturns, sel);
                }
                else
                {
                    var calculationResult = parser.CalculateTWR(sel, (int)numMonths.Value);
                    DisplayTWRChart(calculationResult.GetTWRPercentages(), sel, (int)numMonths.Value);
                }
            }
            catch (Exception ex) { _logger.SentEvent(ex.Message, Logger.EnumLogLevel.EXCEPTION_LEVEL); }
        }

        private void DisplayTWRChart(Dictionary<string, double?> twrPercentages, string startMonth, int months)
        {
            try
            {
                fPlot.Plot.Clear();
                var validData = twrPercentages
                    .Where(kvp => kvp.Value.HasValue && !kvp.Key.Equals("Total", StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(kvp => kvp.Value.Value).ToList();

                if (validData.Count == 0) return;

                double[] values = validData.Select(kvp => kvp.Value.Value).ToArray();
                string[] labels = validData.Select(kvp => TruncateAccountName(kvp.Key)).ToArray();
                double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();

                var barPlot = fPlot.Plot.Add.Bars(positions, values);

                // Explicitly qualify Color to fix CS0104
                ScottPlot.Color[] barColors = new ScottPlot.Color[] {
                    ScottPlot.Color.FromColor(System.Drawing.Color.DodgerBlue),
                    ScottPlot.Color.FromColor(System.Drawing.Color.OrangeRed),
                    ScottPlot.Color.FromColor(System.Drawing.Color.MediumSeaGreen),
                    ScottPlot.Color.FromColor(System.Drawing.Color.Gold)
                };

                for (int i = 0; i < values.Length; i++)
                {
                    barPlot.Bars[i].FillColor = barColors[i % barColors.Length];

                    var txt = fPlot.Plot.Add.Text($"{values[i]:F2}%", positions[i], values[i]);
                    txt.LabelBold = true;
                    txt.LabelAlignment = values[i] >= 0 ? ScottPlot.Alignment.LowerCenter : ScottPlot.Alignment.UpperCenter;
                    txt.OffsetY = values[i] >= 0 ? 35 : -20;
                }

                fPlot.Plot.Axes.AutoScale();
                fPlot.Refresh();
            }
            catch (Exception ex) { _logger.SentEvent(ex.Message, Logger.EnumLogLevel.EXCEPTION_LEVEL); }
        }

        private void DisplayAnnualizedChart(Dictionary<string, double?> periodReturns, string period)
        {
            try
            {
                fPlot.Plot.Clear();
                var validData = periodReturns.Where(kvp => kvp.Value.HasValue).OrderByDescending(kvp => kvp.Value.Value).ToList();
                if (validData.Count == 0) return;

                double[] values = validData.Select(kvp => kvp.Value.Value).ToArray();
                double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();

                var barPlot = fPlot.Plot.Add.Bars(positions, values);
                fPlot.Plot.Axes.AutoScale();
                fPlot.Refresh();
            }
            catch (Exception ex) { _logger.SentEvent(ex.Message, Logger.EnumLogLevel.EXCEPTION_LEVEL); }
        }

        private string TruncateAccountName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;
            string[] words = name.Split(' ');
            if (words.Length > 0)
            {
                string last = words.Last();
                if (last.All(char.IsDigit) || (last.Length > 1 && char.IsLetter(last[0]) && last.Substring(1).All(char.IsDigit)))
                    return string.Join(" ", words.Take(words.Length - 1)).Trim();
            }
            return name.Trim();
        }

        private void timerGUI_Tick(object sender, EventArgs e)
        {
            bool hasData = this.chkAnnualized.Checked ? (annualizedData != null && annualizedData.Count > 0) : (parser?.accounts.Count > 0);
            this.chkAnnualized.Text = this.chkAnnualized.Checked ? "Load Periodic\n   return " : "Load Historical\n   return";
            this.grpSelect.Visible = hasData;
            this.grpMonths.Visible = !this.chkAnnualized.Checked && hasData;
        }

        private void LoadDataFromCSV_Annualized(object sender, EventArgs e)
        {
            try
            {
                this.openFileD.Title = "Load Annualized Data";
                if (this.openFileD.ShowDialog() == DialogResult.OK)
                {
                    // SURGERY: Strip quotes
                    string[] rawLines = File.ReadAllLines(this.openFileD.FileName);
                    string[] lines = rawLines.Select(l => l.Replace("\"", "")).ToArray();

                    if (IsMonthlyReturnFormat(lines)) throw new Exception("Wrong Data from the file!");
                    // ... [Rest of your original parsing logic] ...
                }
            }
            catch (Exception ex) { _logger.SentEvent(ex.Message, Logger.EnumLogLevel.EXCEPTION_LEVEL); }
        }
    }
}