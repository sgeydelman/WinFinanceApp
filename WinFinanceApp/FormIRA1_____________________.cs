using SLGAutomationLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using WinFinanceApp;
using System.IO;
using System.Globalization;

namespace WinFinanceApp
{
    public struct AcountRecord
    {
        public string desc;
        public double value;
        public double curPercent;
        public double valRebalance;
        public double targetPercent;
        public double deviation;
        public bool isCashPosition;  // NEW: Flag to identify cash positions

        public AcountRecord(string desc, double value, double curPercent, double targetPercent, double valRebalance, double deviation, bool isCashPosition = false) : this()
        {
            this.desc = desc;
            this.value = value;
            this.curPercent = curPercent;
            this.targetPercent = targetPercent;
            this.valRebalance = valRebalance;
            this.deviation = deviation;
            this.isCashPosition = isCashPosition;
        }
    }

    public partial class FormIRA1 : Form
    {
        protected Logger _logger;
        Ini inif;
        protected CMyFinance MyFinance;

        int account = (int)CMyFinance.AccountType.SamIRA;
        private double TotalCurValue = 0, TotalCurPercent = 0, TotalTargetPercent = 0;

        // Define cash/money market symbols
        private readonly HashSet<string> CashPositions = new HashSet<string> { "FMPXX", "FDRXX", "SPAXX", "VMFXX", "SPRXX" };

        Dictionary<string, AcountRecord> accountDictionary = new Dictionary<string, AcountRecord>();

        public FormIRA1()
        {
            InitializeComponent();
        }

        private void FormIRA1_Load(object sender, EventArgs e)
        {
            this._logger = Logger.Instance;
            inif = new Ini(_logger.SetupPath);
            this.MyFinance = CMyFinance.Instance;

            // Initial layout adjustment
            AdjustLayout();
        }

        // IMPROVED: Added Resize handler for dynamic layout adjustment
        private void FormIRA1_Resize(object sender, EventArgs e)
        {
            AdjustLayout();
        }

        // IMPROVED: Centralized layout adjustment method
        private void AdjustLayout()
        {
            // With proper Anchor properties set in Designer, minimal adjustment needed
            // The anchors will handle the resizing automatically
            if (this.Width > 0 && this.Height > 0)
            {
                // Force layout refresh
                this.PerformLayout();
            }
        }

        private void timer_ui_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.MyFinance == null)
                    return;
                if (this.account != this.MyFinance.Account)
                {
                    this.account = this.MyFinance.Account;
                    accountDictionary.Clear();
                    dataGrid.Rows.Clear();
                }
            }
            catch { }
        }

        private void dataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dataGrid.Columns[e.ColumnIndex].Name == "RebalanceValue")
                {
                    string st = dataGrid.Rows[e.RowIndex].Cells["RebalanceValue"].Value.ToString();
                    double rebalanceValue = 0.0;
                    double.TryParse(st, out rebalanceValue);
                    if (rebalanceValue < 0)
                        dataGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    else
                        dataGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                }

                // IMPROVED: Highlight cash positions with different background
                if (e.RowIndex >= 0 && dataGrid.Columns[e.ColumnIndex].Name == "Symbol")
                {
                    string symbol = dataGrid.Rows[e.RowIndex].Cells["Symbol"].Value?.ToString();
                    if (CashPositions.Contains(symbol))
                    {
                        dataGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);
            }
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

        // Helper method to parse percentage values
        private double ParsePercentageValue(string value)
        {
            return ParseCsvValue(value);
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

        // IMPROVED: New method for intelligent rebalancing with cash reinvestment
        private void PerformIntelligentRebalancing()
        {
            // Step 1: Calculate total available cash from money market positions
            double totalAvailableCash = 0;
            foreach (var kvp in accountDictionary.Where(x => x.Value.isCashPosition))
            {
                totalAvailableCash += kvp.Value.value;
                this._logger.SentEvent($"Cash position {kvp.Key}: ${kvp.Value.value:F2}", Logger.EnumLogLevel.INFO_LEVEL);
            }

            // Step 2: Calculate initial rebalancing needs for non-cash positions only
            double totalInvestmentValue = TotalCurValue - totalAvailableCash;
            List<string> keysToUpdate = accountDictionary.Keys.ToList();

            // First pass: Calculate rebalancing needs for investment positions
            Dictionary<string, double> rebalanceNeeds = new Dictionary<string, double>();
            double totalPositiveRebalance = 0;
            double totalNegativeRebalance = 0;

            foreach (var key in keysToUpdate)
            {
                AcountRecord record = accountDictionary[key];

                if (record.isCashPosition)
                {
                    // Cash positions: target is typically 0% (or minimal)
                    // All cash is available for reinvestment
                    record.curPercent = record.value / TotalCurValue * 100;
                    record.valRebalance = -record.value; // Negative means sell/reinvest
                    record.deviation = record.curPercent - record.targetPercent;
                }
                else
                {
                    // Investment positions: calculate rebalancing need
                    record.curPercent = record.value / TotalCurValue * 100;
                    double targetValue = (record.targetPercent / 100.0) * TotalCurValue;
                    record.valRebalance = Math.Round(targetValue - record.value, 2);
                    record.deviation = Math.Round(record.curPercent - record.targetPercent, 3);

                    if (record.valRebalance > 0)
                        totalPositiveRebalance += record.valRebalance;
                    else
                        totalNegativeRebalance += Math.Abs(record.valRebalance);
                }

                accountDictionary[key] = record;
                rebalanceNeeds[key] = record.valRebalance;
            }

            // Step 3: Adjust for cash reinvestment
            // The total cash available should be distributed to positions needing investment
            double cashToDistribute = totalAvailableCash;
            double totalRebalanceWithCash = totalPositiveRebalance;

            this._logger.SentEvent($"Total cash available for reinvestment: ${cashToDistribute:F2}", Logger.EnumLogLevel.INFO_LEVEL);
            this._logger.SentEvent($"Total positions needing investment: ${totalPositiveRebalance:F2}", Logger.EnumLogLevel.INFO_LEVEL);

            // If we have excess cash beyond what's needed for rebalancing
            if (cashToDistribute > totalPositiveRebalance)
            {
                // Distribute excess proportionally to all investment positions based on target allocation
                double excessCash = cashToDistribute - totalPositiveRebalance;
                this._logger.SentEvent($"Excess cash after rebalancing: ${excessCash:F2}", Logger.EnumLogLevel.WARNING_LEVEL);

                // Option 1: Keep minimal cash position (e.g., 0.5% - 1%)
                double minimalCashTarget = TotalCurValue * 0.005; // 0.5% cash buffer

                // Option 2: Distribute excess to underweight positions proportionally
                foreach (var key in keysToUpdate.Where(k => !accountDictionary[k].isCashPosition))
                {
                    AcountRecord record = accountDictionary[key];
                    if (record.targetPercent > 0)
                    {
                        // Add proportional share of excess based on target allocation
                        double proportionalShare = (record.targetPercent / TotalTargetPercent) * (excessCash - minimalCashTarget);
                        record.valRebalance = Math.Round(record.valRebalance + proportionalShare, 2);
                        accountDictionary[key] = record;
                    }
                }
            }

            // Step 4: Final adjustment - ensure total rebalancing sums to near zero
            double totalRebalance = accountDictionary.Values.Sum(dp => dp.valRebalance);
            if (Math.Abs(totalRebalance) > 0.01)
            {
                this._logger.SentEvent($"Rebalancing check: Total = ${totalRebalance:F2} (should be near 0)", Logger.EnumLogLevel.WARNING_LEVEL);
            }

            TotalCurPercent = Math.Round(accountDictionary.Values.Sum(dp => dp.curPercent), 1);
            this.lblTotalCur.Text = TotalCurPercent.ToString();
            lblTotalRebalance.Text = Math.Round(totalRebalance, 2).ToString();
        }

        private void LoadDataFromCSV(object sender, EventArgs e)
        {
            try
            {
                List<KeyValuePair<string, double>> positions = new List<KeyValuePair<string, double>>();

                accountDictionary.Clear();
                dataGrid.Rows.Clear();
                TotalCurValue = 0; TotalCurPercent = 0; TotalTargetPercent = 0;
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
                    string[] strings = System.IO.File.ReadAllLines(this.openFileD.FileName);

                    // Load target positions from config
                    for (int i = 0; i < 20; i++)
                    {
                        string symbol = this.inif.GetString(i.ToString(), "symbol", "");
                        if (string.IsNullOrEmpty(symbol) || symbol == "?")
                            continue;
                        double targetPct = this.inif.GetDouble(i.ToString(), "%", 0);
                        positions.Add(new KeyValuePair<string, double>(symbol, targetPct));
                    }

                    string account = "";
                    bool foundHeader = false;

                    foreach (string str in strings)
                    {
                        // Skip until we find the header row
                        if (!foundHeader)
                        {
                            if (str.Contains("Account Number") || str.Contains("Account Name") || str.Contains("Symbol"))
                            {
                                foundHeader = true;
                            }
                            continue;
                        }

                        // Stop parsing if we hit disclaimer text or empty lines
                        if (IsEndOfDataLine(str))
                            break;

                        string[] s = SplitCsvLine(str);  // Use proper CSV splitter instead of simple Split(',')

                        // Skip if not enough columns
                        if (s.Length < 8)
                            continue;

                        double value = 0, curPercent = 0, targetPercent = 0, valRebalance = 0, deviation = 0;
                        account = string.Format("{0} #{1}", s[1], s[0]);
                        string desc = string.Format("{0}", s[3]);
                        string symbol = s[2];
                        bool isCashPosition = CashPositions.Contains(symbol);

                        if (s.Contains("Pending Activity"))
                        {
                            desc = s[2];
                            symbol = "N/A";
                            value = ParseCsvValue(s[7]);
                            accountDictionary.Add(symbol, new AcountRecord(desc, value, curPercent, targetPercent, valRebalance, deviation, false));
                            continue;
                        }

                        // Parse value from column 7 (Current Value)
                        if (s.Length > 7)
                        {
                            value = ParseCsvValue(s[7]);
                        }

                        // Parse percent from column 12 (Percent Of Account) if it exists
                        if (s.Length > 12)
                        {
                            curPercent = ParsePercentageValue(s[12]);
                        }

                        // For cash positions, typically set target to 0% unless specified
                        if (isCashPosition)
                        {
                            targetPercent = 0; // Override to 0% for cash positions
                            this._logger.SentEvent($"Cash position identified: {symbol} with value ${value:F2}", Logger.EnumLogLevel.INFO_LEVEL);
                        }
                        else
                        {
                            KeyValuePair<string, double> foundPair = positions.FirstOrDefault(pair => pair.Key == symbol);
                            if (foundPair.Key == null)
                            {
                                this._logger.SentEvent(string.Format("position {0} not found in configuration", symbol), Logger.EnumLogLevel.EXCEPTION_LEVEL);
                            }
                            else
                            {
                                targetPercent = foundPair.Value;
                            }
                        }

                        deviation = Math.Round((curPercent - targetPercent), 3);
                        if (!isCashPosition) // Only add non-cash positions to target total
                            TotalTargetPercent += targetPercent;

                        accountDictionary.Add(symbol, new AcountRecord(desc, value, curPercent, targetPercent, valRebalance, deviation, isCashPosition));
                    }

                    grpBox.Text = string.Format("Account Monitor: {0}                   {1}", account, Path.GetFileName(this.openFileD.FileName));
                }

                TotalCurValue = Math.Round(accountDictionary.Values.Sum(dp => dp.value), 1);
                TotalTargetPercent = positions.Where(p => !CashPositions.Contains(p.Key)).Sum(pair => pair.Value);
                this.lblTotalVal.Text = TotalCurValue.ToString();
                this.lblTotalTarget.Text = TotalTargetPercent.ToString();

                // IMPROVED: Use intelligent rebalancing that handles cash positions
                PerformIntelligentRebalancing();

                // Populate grid
                foreach (var kvp in accountDictionary.OrderBy(x => x.Value.isCashPosition ? 1 : 0).ThenBy(x => x.Key))
                {
                    DataGridViewRow row = (DataGridViewRow)dataGrid.Rows[0].Clone();
                    row.Cells[0].Value = kvp.Value.desc;
                    row.Cells[1].Value = kvp.Key;
                    row.Cells[2].Value = kvp.Value.value;
                    row.Cells[3].Value = Math.Round(kvp.Value.curPercent, 3);
                    row.Cells[4].Value = kvp.Value.targetPercent;
                    row.Cells[5].Value = kvp.Value.valRebalance;
                    row.Cells[6].Value = kvp.Value.deviation;

                    // Add visual indicator for cash positions
                    if (kvp.Value.isCashPosition)
                    {
                        row.Cells[0].Value = "💰 " + kvp.Value.desc; // Add cash emoji
                    }

                    dataGrid.Rows.Add(row);
                }

                // Add a summary row showing total cash available
                double totalCash = accountDictionary.Where(x => x.Value.isCashPosition).Sum(x => x.Value.value);
                this._logger.SentEvent($"=== REBALANCING SUMMARY ===", Logger.EnumLogLevel.INFO_LEVEL);
                this._logger.SentEvent($"Total Portfolio Value: ${TotalCurValue:F2}", Logger.EnumLogLevel.INFO_LEVEL);
                this._logger.SentEvent($"Total Cash Available: ${totalCash:F2} ({(totalCash / TotalCurValue * 100):F2}%)", Logger.EnumLogLevel.INFO_LEVEL);
                this._logger.SentEvent($"Investment Positions: ${(TotalCurValue - totalCash):F2}", Logger.EnumLogLevel.INFO_LEVEL);
            }
            catch (Exception ex)
            {
                accountDictionary.Clear();
                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);
            }
        }

        private void BtnLoadFile_Click(object sender, EventArgs e)
        {
            switch (this.MyFinance.Account)
            {
                case (int)CMyFinance.AccountType.LenaIRA:
                    inif = new Ini(Properties.Settings.Default.SetupLenaIRA);
                    break;
                case (int)CMyFinance.AccountType.LenaRothIRA:
                    inif = new Ini(Properties.Settings.Default.SetupLenaROTH);
                    break;
                case (int)CMyFinance.AccountType.SamIRA:
                    inif = new Ini(Properties.Settings.Default.SetupPath);
                    break;
                case (int)CMyFinance.AccountType.SamRothIRA:
                    inif = new Ini(Properties.Settings.Default.SetupSamROTH);
                    break;
            }
            LoadDataFromCSV(sender, e);
        }

        // IMPROVED: Add option to show/hide cash positions
        private void ShowRebalancingReport()
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine("=== REBALANCING REPORT ===");
            report.AppendLine($"Portfolio Total: ${TotalCurValue:F2}");

            double totalCash = accountDictionary.Where(x => x.Value.isCashPosition).Sum(x => x.Value.value);
            report.AppendLine($"Cash Available: ${totalCash:F2} ({(totalCash / TotalCurValue * 100):F2}%)");

            report.AppendLine("\nPositions to BUY:");
            foreach (var kvp in accountDictionary.Where(x => !x.Value.isCashPosition && x.Value.valRebalance > 0).OrderByDescending(x => x.Value.valRebalance))
            {
                report.AppendLine($"  {kvp.Key}: ${kvp.Value.valRebalance:F2}");
            }

            report.AppendLine("\nPositions to SELL:");
            foreach (var kvp in accountDictionary.Where(x => !x.Value.isCashPosition && x.Value.valRebalance < 0).OrderBy(x => x.Value.valRebalance))
            {
                report.AppendLine($"  {kvp.Key}: ${Math.Abs(kvp.Value.valRebalance):F2}");
            }

            report.AppendLine("\nCash to Deploy:");
            foreach (var kvp in accountDictionary.Where(x => x.Value.isCashPosition))
            {
                report.AppendLine($"  {kvp.Key}: ${kvp.Value.value:F2}");
            }

            MessageBox.Show(report.ToString(), "Rebalancing Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}