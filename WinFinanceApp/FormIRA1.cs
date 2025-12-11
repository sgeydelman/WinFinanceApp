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

        public AcountRecord(string desc, double value, double curPercent, double targetPercent, double valRebalance, double deviation) : this()
        {
            this.desc = desc;
            this.value = value;
            this.curPercent = curPercent;
            this.targetPercent = targetPercent;
            this.valRebalance = valRebalance;
            this.deviation = deviation;
        }
    }

    public partial class FormIRA1 : Form
    {
        protected Logger _logger;
        Ini inif;
        protected CMyFinance MyFinance;

        int account = (int)CMyFinance.AccountType.SamIRA;
        private double TotalCurValue = 0, TotalCurPercent = 0, TotalTargetPercent = 0;

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

                        if (s.Contains("Pending Activity"))
                        {
                            desc = s[2];
                            symbol = "N/A";
                            value = ParseCsvValue(s[7]);
                            accountDictionary.Add(symbol, new AcountRecord(desc, value, curPercent, targetPercent, valRebalance, deviation));
                            continue;
                        }

                        // Parse value from column 7 (Current Value) - note CSV columns are 0-indexed
                        // Column layout: 0=Account#, 1=Name, 2=Symbol, 3=Description, 4=Qty, 5=Price, 6=PriceChange, 7=CurrentValue
                        if (s.Length > 7)
                        {
                            value = ParseCsvValue(s[7]);
                        }

                        // Parse percent from column 12 (Percent Of Account) if it exists
                        if (s.Length > 12)
                        {
                            curPercent = ParsePercentageValue(s[12]);
                            // curPercent is already a percentage number (e.g., 0.23 for 0.23%)
                            // No need to multiply by 100 since the CSV already has it as percentage
                        }

                        // Debug output for first symbol to verify parsing
                        if (symbol == "FMPXX" || symbol == "VWO")
                        {
                       //     this._logger.SentEvent($"Debug {symbol}: Raw s[7]='{s[7]}' Parsed value={value} | Raw s[12]='{s[12]}' Parsed curPercent={curPercent}", Logger.EnumLogLevel.WARNING_LEVEL);
                        }

                        KeyValuePair<string, double> foundPair = positions.FirstOrDefault(pair => pair.Key == symbol);
                        if (foundPair.Key == null)
                        {
                            this._logger.SentEvent(string.Format("position {0} not found in configuration", symbol), Logger.EnumLogLevel.EXCEPTION_LEVEL);
                        }
                        else
                        {
                            targetPercent = foundPair.Value;
                        }

                        deviation = Math.Round((curPercent - targetPercent), 3);
                        TotalTargetPercent += targetPercent;
                        accountDictionary.Add(symbol, new AcountRecord(desc, value, curPercent, targetPercent, valRebalance, deviation));
                    }

                    grpBox.Text = string.Format("Account Monitor: {0}                   {1}", account, Path.GetFileName(this.openFileD.FileName));
                }

                TotalCurValue = Math.Round(accountDictionary.Values.Sum(dp => dp.value),1);
               // TotalCurPercent = Math.Round(accountDictionary.Values.Sum(dp => dp.curPercent),1);
                TotalTargetPercent = positions.Sum(pair => pair.Value);
                this.lblTotalVal.Text = TotalCurValue.ToString();
                this.lblTotalTarget.Text = TotalTargetPercent.ToString();

                // Perform a rebalance
                List<string> keysToUpdate = accountDictionary.Keys.ToList();
                foreach (var key in keysToUpdate)
                {
                    AcountRecord recordToUpdate = accountDictionary[key];
                    recordToUpdate.curPercent = recordToUpdate.value / TotalCurValue * 100;
                    recordToUpdate.valRebalance = Math.Round((recordToUpdate.targetPercent / 100.00 - recordToUpdate.curPercent / 100) * TotalCurValue, 3);

                    accountDictionary[key] = recordToUpdate;
                }
                TotalCurPercent = Math.Round(accountDictionary.Values.Sum(dp => dp.curPercent),1);
                this.lblTotalCur.Text = TotalCurPercent.ToString();
                lblTotalRebalance.Text = Math.Round(accountDictionary.Values.Sum(dp => dp.valRebalance), 3).ToString();

                // Populate grid
                foreach (var kvp in accountDictionary)
                {
                    dataGrid.Rows.Add(kvp.Value.desc, kvp.Key, kvp.Value.value, Math.Round(kvp.Value.curPercent, 3), kvp.Value.targetPercent, kvp.Value.valRebalance, kvp.Value.deviation);
                }
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
    }
}