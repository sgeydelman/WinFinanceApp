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
    //public struct AcountRecord
    //{
    //    public string desc;
    //    public double value;
    //    public double curPercent;
    //    public double valRebalance;
    //    public double targetPercent;
    //  //  public double deviation;
    //    // NEW: Add fields for simulation results
    //    public double simulatedValue;
    //    public double simulatedPercent;

    //    public AcountRecord(string desc, double value, double curPercent, double targetPercent, double valRebalance) : this()
    //    {
    //        this.desc = desc;
    //        this.value = value;
    //        this.curPercent = curPercent;
    //        this.targetPercent = targetPercent;
    //        this.valRebalance = valRebalance;
           
    //    }
    //}

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

            // Ensure numeric sorting is available for the deviation column
            // subscribe to SortCompare so we can compare numeric values stored in cells
            try
            {
                this.dataGrid.SortCompare += DataGrid_SortCompare;
            }
            catch { }
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
                    this.txtBondsC.Text = txtBondsT.Text = txtCashC.Text = txtCashT.Text = txtStocksC.Text = txtStocksT.Text = "?";
                }
            }
            catch { }
        }

        private void dataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                // Only color the RebalanceValue cell (not the entire row) so deviation highlighting remains visible
                if (e.RowIndex >= 0 && dataGrid.Columns[e.ColumnIndex].Name == "RebalanceValue")
                {
                    var cell = dataGrid.Rows[e.RowIndex].Cells["RebalanceValue"];
                    double rebalanceValue;
                    if (double.TryParse(cell.Value?.ToString(), out rebalanceValue))
                    {
                        cell.Style.ForeColor = (rebalanceValue < 0) ? Color.Red : Color.Green;
                    }
                    else
                    {
                        cell.Style.ForeColor = Color.Black;
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

            // Do not treat quoted lines (common in CSV headers) as end-of-data
            // if (firstColumn.StartsWith("\""))
            //     return true;

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
                this.txtBondsC.Text = txtBondsT.Text = txtCashC.Text = txtCashT.Text = txtStocksC.Text = txtStocksT.Text = "?";
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
                if (this.openFileD.ShowDialog() != DialogResult.OK)
                    return;

                string[] strings = System.IO.File.ReadAllLines(this.openFileD.FileName);

                // Load target positions from config
                for (int i = 0; i < 20; i++)
                {
                    string symbol = this.inif.GetString(i.ToString(), "symbol", "");
                    // if symbol contains "FDRXX" make symbol "FDRXX" only to match cash fund entries
                    if (symbol.Contains("FDRXX"))
                        symbol = "FDRXX";


                    if (string.IsNullOrEmpty(symbol) || symbol == "?")
                        continue;
                    double targetPct = this.inif.GetDouble(i.ToString(), "%", 0);
                    positions.Add(new KeyValuePair<string, double>(symbol, targetPct));
                }

                string account = Path.GetFileName(this.openFileD.FileName);

                int idxSymbol = -1, idxDescription = -1, idxCurrentValue = -1, idxPercent = -1;
                int headerIndex = -1;

                // Find header row
                for (int i = 0; i < strings.Length; i++)
                {
                    string line = strings[i];
                    if (IsEndOfDataLine(line))
                        continue;
                    string lower = line.ToLowerInvariant();
                    if (lower.Contains("symbol") || lower.Contains("% of acct") || lower.Contains("% of account") || lower.Contains("value") || lower.Contains("%"))
                    {
                        headerIndex = i;
                        string[] headerCols = SplitCsvLine(line);
                        if ((headerCols == null || headerCols.Length == 1) && line.Contains('\t'))
                            headerCols = line.Split('\t');

                        for (int c = 0; c < headerCols.Length; c++)
                        {
                            string col = headerCols[c].Trim().ToLowerInvariant();
                            if (col.Contains("symbol")) idxSymbol = c;
                            else if (col.Contains("description") || col.Contains("position") || col.Contains("security")) idxDescription = c;
                            else if (col.Contains("current value") || col.Contains("market value") || col.Contains("marketvalue") || col == "value" || col.Contains(" value")) idxCurrentValue = c;
                            else if (col.Contains("% of acct") || col.Contains("% of account") || col.Contains("percent") || col.Contains("% of") || col.Contains("%")) idxPercent = c;
                        }
                        break;
                    }
                }

                // If filename contains 'new' use known mapping (provided by user)
                var fname = Path.GetFileName(this.openFileD.FileName).ToLowerInvariant();
                if (fname.Contains("new"))
                {
                    // According to provided header: Value is column index 14, '% of Acct' is index 15 (0-based)
                    idxSymbol = idxSymbol == -1 ? 0 : idxSymbol;
                    idxCurrentValue = 14;
                    idxPercent = 15;
                    this._logger.SentEvent($"Using preset mapping for '{fname}': idxSymbol={idxSymbol}, idxCurrentValue={idxCurrentValue}, idxPercent={idxPercent}", Logger.EnumLogLevel.INFO_LEVEL);
                }

              //  this._logger.SentEvent($"CSV headerIndex={headerIndex}, idxSymbol={idxSymbol}, idxDescription={idxDescription}, idxCurrentValue={idxCurrentValue}, idxPercent={idxPercent}", Logger.EnumLogLevel.INFO_LEVEL);

                int startRow = (headerIndex >= 0) ? headerIndex + 1 : 0;
                int naCounter = 0;

                // Parse rows
                for (int r = startRow; r < strings.Length; r++)
                {
                    string line = strings[r];
                    try
                    {
                        if (IsEndOfDataLine(line))
                            break;

                        var cols = SplitCsvLine(line);
                        if (cols == null || cols.Length == 0)
                            continue;

                        // Removed per-row info logging to improve performance

                        bool allEmpty = true;
                        foreach (var c in cols) if (!string.IsNullOrWhiteSpace(c)) { allEmpty = false; break; }
                        if (allEmpty) continue;

                        // Pending Activity
                        if (cols.Any(col => col.IndexOf("Pending Activity", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            string desc = cols.Length > 2 ? cols[2].Trim() : "Pending Activity";
                            string key = $"N/A_{naCounter++}";
                            double val = (idxCurrentValue >= 0 && idxCurrentValue < cols.Length) ? ParseCsvValue(cols[idxCurrentValue]) : ParseCsvValue(cols.Last());
                            accountDictionary[key] = new AcountRecord(desc, val, 0.0, 0.0, 0.0);
                            continue;
                        }

                        // Symbol
                        string symbol = null;
                        if (idxSymbol >= 0 && idxSymbol < cols.Length)
                        {
                            symbol = cols[idxSymbol].Trim();
                            //if symbol contains "FDRXX" make symbol "FDRXX" only to match cash fund entries
                            if (symbol.Contains("FDRXX"))
                                symbol = "FDRXX";
                        }
                        else
                        {
                            for (int c = 0; c < cols.Length; c++)
                            {
                                var cand = cols[c].Trim();
                                if (cand.Length >= 1 && cand.Length <= 6 && System.Text.RegularExpressions.Regex.IsMatch(cand, "^[A-Za-z0-9.\\-]+$"))
                                {
                                    symbol = cand; break;
                                }
                            }
                            if (string.IsNullOrEmpty(symbol))
                                symbol = cols.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim() ?? ("N/A_" + (naCounter++).ToString());
                            //if symbol contains "FDRXX" make symbol "FDRXX" only to match cash fund entries
                            if (symbol.Contains("FDRXX"))
                                symbol = "FDRXX";
                        }

                        string descVal = (idxDescription >= 0 && idxDescription < cols.Length) ? cols[idxDescription].Trim() : string.Empty;

                        double valueVal = 0.0;
                        if (idxCurrentValue >= 0 && idxCurrentValue < cols.Length)
                            valueVal = ParseCsvValue(cols[idxCurrentValue]);
                        else
                        {
                            for (int c = cols.Length - 1; c >= 0; c--)
                            {
                                string tok = cols[c];
                                if (string.IsNullOrWhiteSpace(tok)) continue;
                                double tv = ParseCsvValue(tok);
                                if (tv != 0.0 || tok.Contains("$") || tok.Contains(",") || tok.Contains("(") || System.Text.RegularExpressions.Regex.IsMatch(tok, "\\d"))
                                {
                                    valueVal = tv; break;
                                }
                            }
                        }

                        double percentVal = 0.0;
                        if (idxPercent >= 0 && idxPercent < cols.Length)
                            percentVal = ParsePercentageValue(cols[idxPercent]);
                        else
                        {
                            for (int c = 0; c < cols.Length; c++)
                            {
                                if (cols[c].Contains("%")) { percentVal = ParsePercentageValue(cols[c]); break; }
                            }
                        }

                        // Target percent from config
                        KeyValuePair<string, double> foundPair = positions.FirstOrDefault(p => p.Key == symbol);
                        double targetPercent = (foundPair.Key == null) ? 0.0 : foundPair.Value;

                        if (!accountDictionary.ContainsKey(symbol))
                            accountDictionary.Add(symbol, new AcountRecord(descVal, valueVal, percentVal, targetPercent, 0.0));
                        else
                        {
                            var ex = accountDictionary[symbol];
                            ex.value += valueVal;
                            accountDictionary[symbol] = ex;
                        }

                        TotalTargetPercent += targetPercent;
                    }
                    catch (Exception rowEx)
                    {
                        // Keep only important warnings/errors
                        this._logger.SentEvent("Error parsing CSV row: " + rowEx.ToString(), Logger.EnumLogLevel.WARNING_LEVEL);
                        continue;
                    }
                }

                grpBox.Text = string.Format("Account Monitor: {0}                   {1}", account, Path.GetFileName(this.openFileD.FileName));

                TotalCurValue = Math.Round(accountDictionary.Values.Sum(dp => dp.value), 1);
                TotalTargetPercent = positions.Sum(pair => pair.Value);
                this.lblTotalVal.Text = TotalCurValue.ToString();
                this.lblTotalTarget.Text = TotalTargetPercent.ToString();

                // Rebalance computation
                double RawBuysNeeded = 0;
                double RawSellsGenerated = 0;
                Dictionary<string, double> RawAdjustments = new Dictionary<string, double>();

                foreach (var key in accountDictionary.Keys.ToList())
                {
                    var record = accountDictionary[key];
                    double curPct = (TotalCurValue > 1e-9) ? (record.value / TotalCurValue * 100.0) : 0.0;
                    double rawRebalance = (record.targetPercent / 100.0 - curPct / 100.0) * TotalCurValue;
                    RawAdjustments[key] = rawRebalance;
                    if (rawRebalance > 0) RawBuysNeeded += rawRebalance; else RawSellsGenerated += rawRebalance;
                    record.curPercent = curPct;
                    accountDictionary[key] = record;
                }

                double TotalFundsFromSales = Math.Abs(RawSellsGenerated);
                double scaleFactor = 1.0;
                if (RawBuysNeeded > 0 && TotalFundsFromSales < RawBuysNeeded)
                    scaleFactor = TotalFundsFromSales / RawBuysNeeded;

                Dictionary<string, double> FinalRebalanceValues = new Dictionary<string, double>();
                foreach (var kv in RawAdjustments)
                {
                    double raw = kv.Value;
                    double finalVal = raw > 0 ? Math.Round(raw * scaleFactor, 3) : Math.Round(raw, 3);
                    FinalRebalanceValues[kv.Key] = finalVal;
                }

                double newTotalValue = TotalCurValue;
                var keysSnapshot = accountDictionary.Keys.ToList();
                foreach (var key in keysSnapshot)
                {
                    var rec = accountDictionary[key];
                    if (FinalRebalanceValues.ContainsKey(key)) rec.valRebalance = FinalRebalanceValues[key];
                    rec.curPercent = (TotalCurValue > 1e-9) ? (rec.value / TotalCurValue * 100.0) : 0.0;
                    rec.simulatedValue = rec.value + rec.valRebalance;
                    rec.simulatedPercent = (newTotalValue > 1e-9) ? (rec.simulatedValue / newTotalValue * 100.0) : 0.0;
                    accountDictionary[key] = rec;
                }

                double ResidualError = accountDictionary.Values.Sum(r => r.valRebalance);
                string AbsorberSymbol = accountDictionary.Keys.FirstOrDefault(k => k.Contains("FMPXX") || k.Contains("FDRXX"));
                if (!string.IsNullOrEmpty(AbsorberSymbol) && Math.Abs(ResidualError) > 0.001)
                {
                    var absorber = accountDictionary[AbsorberSymbol];
                    absorber.valRebalance = Math.Round(absorber.valRebalance - ResidualError, 3);
                    absorber.simulatedValue = absorber.value + absorber.valRebalance;
                    absorber.simulatedPercent = (newTotalValue > 1e-9) ? (absorber.simulatedValue / newTotalValue * 100.0) : 0.0;
                    accountDictionary[AbsorberSymbol] = absorber;
                }

                TotalCurPercent = Math.Round(accountDictionary.Values.Sum(dp => double.IsNaN(dp.curPercent) ? 0.0 : dp.curPercent), 1);
                this.lblTotalCur.Text = TotalCurPercent.ToString();
                double totalRebalance = Math.Round(accountDictionary.Values.Sum(dp => double.IsNaN(dp.valRebalance) ? 0.0 : dp.valRebalance), 3);
                lblTotalRebalance.Text = totalRebalance.ToString();

                // Populate grid with styling for rebalance and deviation
                foreach (var kvp in accountDictionary)
                {
                    double curPercent = double.IsNaN(kvp.Value.curPercent) ? 0.0 : Math.Round(kvp.Value.curPercent, 3);
                    double targetPercent = kvp.Value.targetPercent;
                    string desc = kvp.Value.desc ?? string.Empty;
                    if (kvp.Key.ToString().Contains("FMPXX") || kvp.Key.ToString().Contains("FDRXX"))
                        desc = "💰 " + desc;

                    string deviationStr;
                    if (Math.Abs(targetPercent) > 1e-9)
                    {
                        double relativeDeviation = ((curPercent - targetPercent) / targetPercent) * 100.0;
                        deviationStr = string.Format("{0:+0.00;-0.00;0.00}%", relativeDeviation);
                    }
                    else
                    {
                        deviationStr = "N/A";
                    }

                    double displayValue = double.IsNaN(kvp.Value.value) ? 0.0 : kvp.Value.value;
                    double displayRebalance = double.IsNaN(kvp.Value.valRebalance) ? 0.0 : kvp.Value.valRebalance;

                    int rowIndex = dataGrid.Rows.Add(
                        desc,
                        kvp.Key,
                        displayValue,
                        curPercent,
                        targetPercent,
                        displayRebalance,
                        deviationStr
                    );

                    try
                    {
                        var row = dataGrid.Rows[rowIndex];

                        // Style rebalance value: green for positive, red for negative
                        var rebCell = row.Cells[5];
                        double rebVal;
                        if (double.TryParse(rebCell.Value?.ToString(), out rebVal))
                        {
                            rebCell.Style.ForeColor = (rebVal < 0) ? Color.Red : Color.Green;
                        }

                        // Deviation cell styling and threshold indicator
                        var devCell = row.Cells[6];
                        devCell.Style.Font = new Font(dataGrid.Font, FontStyle.Regular);
                        devCell.Style.ForeColor = Color.Black;

                        double relativeDeviation = 0.0;
                        bool thresholdExceeded = false;
                        if (Math.Abs(targetPercent) > 1e-9)
                        {
                            relativeDeviation = ((curPercent - targetPercent) / targetPercent) * 100.0;
                            double threshold = this.MyFinance?.thresholdTrigger ?? 0.0;
                            if (threshold > 0 && Math.Abs(relativeDeviation) >= threshold) thresholdExceeded = true;
                        }

                        if (thresholdExceeded)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightSalmon;
                            row.DefaultCellStyle.ForeColor = Color.Black;
                            devCell.Style.Font = new Font(dataGrid.Font, FontStyle.Bold);
                            devCell.Style.ForeColor = Color.DarkRed;
                            devCell.Value = "⚠ " + deviationStr;
                        }
                    }
                    catch { }
                }

                // Load global threshold setting into MyFinance (defensive)
                try
                {
                    var globalIni = new Ini(this._logger.SetupPath);
                    double cfgThreshold = globalIni.GetDouble("Settings", "RebalanceThresholdPercent", this.MyFinance.thresholdTrigger);
                    this.MyFinance.thresholdTrigger = cfgThreshold;
                }
                catch { }

                // Investment strategy summary: compute current vs target for Stocks/Bonds/Cash
                try
                {
                    string[] strs = this.inif.GetSectionNames();
                    double curStocks = 0, targetStocks = 0, curBonds = 0, targetBonds = 0, curCash = 0, targetCash = 0;
                    foreach (string s in strs)
                    {
                        SetingStructure data = new SetingStructure
                        {
                            Symbol = this.inif.GetString(s, "symbol", ""),
                            TargetPercent = this.inif.GetDouble(s, "%", 0),
                            Note = this.inif.GetString(s, "note", "")
                        };

                        if (data.Note != null && data.Note.ToLower().Contains("stock"))
                        {
                            if (accountDictionary.ContainsKey(data.Symbol)) curStocks += double.IsNaN(accountDictionary[data.Symbol].curPercent) ? 0.0 : accountDictionary[data.Symbol].curPercent;
                            targetStocks += data.TargetPercent;
                        }
                        else if (data.Note != null && data.Note.ToLower().Contains("bond"))
                        {
                            if (accountDictionary.ContainsKey(data.Symbol)) curBonds += double.IsNaN(accountDictionary[data.Symbol].curPercent) ? 0.0 : accountDictionary[data.Symbol].curPercent;
                            targetBonds += data.TargetPercent;
                        }
                        else if (data.Note != null && data.Note.ToLower().Contains("cash"))
                        {
                            if (accountDictionary.ContainsKey(data.Symbol)) curCash += double.IsNaN(accountDictionary[data.Symbol].curPercent) ? 0.0 : accountDictionary[data.Symbol].curPercent;
                            targetCash += data.TargetPercent;
                        }
                    }

                    txtStocksT.Text = targetStocks.ToString("F2") + " %";
                    txtStocksC.Text = (double.IsNaN(curStocks) ? 0.0 : curStocks).ToString("F2") + " %";
                    txtBondsT.Text = targetBonds.ToString("F2") + " %";
                    txtBondsC.Text = (double.IsNaN(curBonds) ? 0.0 : curBonds).ToString("F2") + " %";
                    txtCashT.Text = targetCash.ToString("F2") + " %";
                    txtCashC.Text = (double.IsNaN(curCash) ? 0.0 : curCash).ToString("F2") + " %";
                }
                catch { }
            }
            catch (Exception ex)
            {
                accountDictionary.Clear();
                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);
            }
        }

        private void BtnLoadFile_Click(object sender, EventArgs e)
        {
            // Ensure inif corresponds to current account selection
            switch (this.MyFinance.Account)
            {
                case (int)CMyFinance.AccountType.LenaIRA:
                    inif = new Ini(Properties.Settings.Default.SetupLenaIRA);
                    break;
                case (int)CMyFinance.AccountType.LenaRothIRA:
                    inif = new Ini(Properties.Settings.Default.SetupLenaROTH);
                    break;
                case (int)CMyFinance.AccountType.SamIRA:
                    inif = new Ini(Properties.Settings.Default.SetupSamIRA);
                    break;
                case (int)CMyFinance.AccountType.SamRothIRA:
                    inif = new Ini(Properties.Settings.Default.SetupSamROTH);
                    break;
                case (int)CMyFinance.AccountType.Other:
                    inif = new Ini(Properties.Settings.Default.SetupOther);
                    break;
            }

            // Load global threshold setting (defensive)
            try
            {
                var globalIni = new Ini(this._logger.SetupPath);
                double cfgThreshold = globalIni.GetDouble("Settings", "RebalanceThresholdPercent", this.MyFinance.thresholdTrigger);
                this.MyFinance.thresholdTrigger = cfgThreshold;
            }
            catch { }

            LoadDataFromCSV(sender, e);
        }

        // DataGrid SortCompare handler to allow numeric sorting of the deviation column
        private void DataGrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            try
            {
                // deviation column is index 6 in our grid
                if (e.Column != null && e.Column.Index == 6)
                {
                    double v1 = ExtractDeviationNumeric(e.CellValue1);
                    double v2 = ExtractDeviationNumeric(e.CellValue2);

                    // Treat NaN (N/A) as largest to push them to the end
                    if (double.IsNaN(v1)) v1 = double.MaxValue;
                    if (double.IsNaN(v2)) v2 = double.MaxValue;

                    int cmp = v1.CompareTo(v2);
                    if (cmp == 0)
                        cmp = string.Compare(Convert.ToString(e.CellValue1), Convert.ToString(e.CellValue2), StringComparison.InvariantCultureIgnoreCase);

                    e.SortResult = cmp;
                    e.Handled = true;
                }
            }
            catch { }
        }

        // Extract numeric deviation value from the cell display (e.g. "+12.34" or "⚠ +5.67" or "N/A")
        private double ExtractDeviationNumeric(object cellValue)
        {
            if (cellValue == null) return double.NaN;
            string s = cellValue.ToString();
            s = s.Replace("⚠", "").Trim();
            if (string.IsNullOrWhiteSpace(s) || s.Equals("N/A", StringComparison.OrdinalIgnoreCase))
                return double.NaN;

            // remove possible percent sign if present
            if (s.EndsWith("%")) s = s.Substring(0, s.Length - 1).Trim();
            // remove plus sign and spaces
            s = s.Replace("+", "").Replace(" ", "");

            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out double d))
                return d;
            return double.NaN;
        }
    }
}