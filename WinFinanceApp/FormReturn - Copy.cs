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

        // Storage for annualized return data
        private Dictionary<string, Dictionary<string, double?>> annualizedData;
        private List<string> annualizedPeriods;
        private Dictionary<string, string> accountLifeStartDates;
        private Dictionary<string, double?> annualizedTotalData;
        private string annualizedReportDate;

        // Flag to control automatic chart updates
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

            // Initial layout adjustment
            AdjustLayout();
        }

        // IMPROVED: Added Resize handler for dynamic layout adjustment
        private void FormReturn_Resize(object sender, EventArgs e)
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

                // ScottPlot needs explicit refresh after resize
                if (fPlot != null && fPlot.Plot != null)
                {
                    fPlot.Refresh();
                }
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (this.chkAnnualized.Checked)
                LoadDataFromCSV_Annualized(sender, e);
            else
                LoadDataFromCSV(sender, e);

        }

        private void LoadDataFromCSV(object sender, EventArgs e)
        {
            try
            {
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
                    parser.ParseTWRCSV(strings); // Use ParseTWRCSV method

                    // Get available months for user to select
                    var availableMonths = parser.GetAvailableMonths();
                    this.lblTotalM.Text = availableMonths.Count.ToString();
                    this.numMonths.Maximum = availableMonths.Count;
                    this.numMonths.Enabled = true; // Enable for normal mode
                    this.comboMonths.DataSource = availableMonths;

                    // Clear the plot
                    fPlot.Plot.Clear();
                    fPlot.Refresh();

                    groupBox.Text = $"Historic time weighted return (TWR) - {Path.GetFileName(this.openFileD.FileName)}";
                }
            }
            catch (Exception ex)
            {
                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);
              //  MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.chkAnnualized.Checked)
            {
                // Annualized mode: automatically redraw the plot when period selection changes
                if (comboMonths.SelectedItem != null && annualizedData != null && annualizedData.Count > 0)
                {
                    string selectedPeriod = comboMonths.SelectedItem.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(selectedPeriod))
                    {
                        // Extract returns for the selected period
                        Dictionary<string, double?> periodReturns = new Dictionary<string, double?>();
                        foreach (var account in annualizedData)
                        {
                            if (account.Value.ContainsKey(selectedPeriod))
                            {
                                periodReturns[account.Key] = account.Value[selectedPeriod];
                            }
                        }

                        // Display results in plot
                        DisplayAnnualizedChart(periodReturns, selectedPeriod);
                    }
                }
            }
            else
            {
                // Normal TWR mode: limit to number of months available from selected month onwards
                int selectedIndex = comboMonths.SelectedIndex;
                int availableMonthsFromSelection = comboMonths.Items.Count - selectedIndex;
                numMonths.Maximum = availableMonthsFromSelection;

                // Automatically redraw the plot when month selection changes
                if (comboMonths.SelectedItem != null && parser != null && parser.accounts.Count > 0)
                {
                    string selectedMonth = comboMonths.SelectedItem.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(selectedMonth))
                    {
                        int numberOfMonths = (int)numMonths.Value;

                        try
                        {
                            var calculationResult = parser.CalculateTWR(selectedMonth, numberOfMonths);
                            var twrPercentages = calculationResult.GetTWRPercentages();

                            // Display results in plot
                            DisplayTWRChart(twrPercentages, selectedMonth, numberOfMonths);
                        }
                        catch (Exception ex)
                        {
                            this._logger.SentEvent($"Error auto-updating chart: {ex.Message}", Logger.EnumLogLevel.WARNING_LEVEL);
                        }
                    }
                }
            }
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboMonths.SelectedItem == null)
                {
                    this._logger.SentEvent("Please select a start month or period", Logger.EnumLogLevel.WARNING_LEVEL);
                //    MessageBox.Show("Please select a start month or period", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if we're in annualized mode
                if (this.chkAnnualized.Checked)
                {
                    // Annualized mode
                    string selectedPeriod = comboMonths.SelectedItem.ToString() ?? string.Empty;
                    if (string.IsNullOrEmpty(selectedPeriod))
                    {
                        this._logger.SentEvent("Invalid period selection", Logger.EnumLogLevel.WARNING_LEVEL);
                       // MessageBox.Show("Invalid period selection", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (annualizedData == null || annualizedData.Count == 0)
                    {
                        this._logger.SentEvent("No annualized data loaded", Logger.EnumLogLevel.WARNING_LEVEL);
                       // MessageBox.Show("No annualized data loaded", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Extract returns for the selected period
                    Dictionary<string, double?> periodReturns = new Dictionary<string, double?>();
                    foreach (var account in annualizedData)
                    {
                        if (account.Value.ContainsKey(selectedPeriod))
                        {
                            periodReturns[account.Key] = account.Value[selectedPeriod];
                        }
                    }

                    // Display results in plot
                    DisplayAnnualizedChart(periodReturns, selectedPeriod);
                }
                else
                {
                    // Normal TWR mode
                    string selectedMonth = comboMonths.SelectedItem.ToString() ?? string.Empty;
                    if (string.IsNullOrEmpty(selectedMonth))
                    {
                        this._logger.SentEvent("Invalid month selection", Logger.EnumLogLevel.ERROR_LEVEL);
                       // MessageBox.Show("Invalid month selection", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int numberOfMonths = (int)numMonths.Value;

                    var calculationResult = parser.CalculateTWR(selectedMonth, numberOfMonths);
                    var twrPercentages = calculationResult.GetTWRPercentages();

                    // Display results in plot
                    DisplayTWRChart(twrPercentages, selectedMonth, numberOfMonths);

                    //Log results
                    foreach (var accountTWR in twrPercentages)
                    {
                        if (accountTWR.Value.HasValue)
                        {
                            // this._logger.SentEvent($"{accountTWR.Key}: {accountTWR.Value.Value:F2}%", Logger.EnumLogLevel.INFO_LEVEL);
                        }
                        else
                        {
                            this._logger.SentEvent($"{accountTWR.Key}: No data", Logger.EnumLogLevel.WARNING_LEVEL);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.SentEvent($"Error calculating returns: {ex.Message}", Logger.EnumLogLevel.EXCEPTION_LEVEL);
              //  MessageBox.Show($"Error calculating returns: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayTWRChart(Dictionary<string, double?> twrPercentages, string startMonth, int months)
        {
            try
            {
                // Clear previous plot
                fPlot.Plot.Clear();

                // Filter out accounts with no data, exclude "Total", and sort by TWR value
                var validData = twrPercentages
                    .Where(kvp => kvp.Value.HasValue && !kvp.Key.Equals("Total", StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(kvp => kvp.Value.Value)
                    .ToList();

                if (validData.Count == 0)
                {
                    fPlot.Plot.Title("No data available for selected period");
                    fPlot.Refresh();
                    return;
                }

                // Prepare data for plotting
                double[] values = validData.Select(kvp => kvp.Value.Value).ToArray();
                string[] labels = validData.Select(kvp => TruncateAccountName(kvp.Key)).ToArray();
                double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();

                // Create bar plot
                var barPlot = fPlot.Plot.Add.Bars(positions, values);

                // Define color palette for bars
                ScottPlot.Color[] barColors = new ScottPlot.Color[]
                {
                    ScottPlot.Color.FromColor(System.Drawing.Color.DodgerBlue),
                    ScottPlot.Color.FromColor(System.Drawing.Color.OrangeRed),
                    ScottPlot.Color.FromColor(System.Drawing.Color.MediumSeaGreen),
                    ScottPlot.Color.FromColor(System.Drawing.Color.Gold),
                    ScottPlot.Color.FromColor(System.Drawing.Color.MediumPurple),
                    ScottPlot.Color.FromColor(System.Drawing.Color.Coral),
                    ScottPlot.Color.FromColor(System.Drawing.Color.Teal),
                    ScottPlot.Color.FromColor(System.Drawing.Color.Crimson),
                    ScottPlot.Color.FromColor(System.Drawing.Color.DarkOrange),
                    ScottPlot.Color.FromColor(System.Drawing.Color.SlateBlue),
                    ScottPlot.Color.FromColor(System.Drawing.Color.LimeGreen),
                    ScottPlot.Color.FromColor(System.Drawing.Color.DeepPink)
                };

                // --- Assign colors and create legend entries ---
                for (int i = 0; i < values.Length; i++)
                {
                    // Determine the color for the bar
                    ScottPlot.Color barColor = barColors[i % barColors.Length];

                    // Assign the color
                    barPlot.Bars[i].FillColor = barColor;

                    // Add value label on top of each bar
                    var txt = fPlot.Plot.Add.Text($"{values[i]:F2}%", positions[i], values[i]);
                    txt.LabelFontSize = 10;
                    txt.LabelBold = true;
                    txt.LabelFontColor = ScottPlot.Color.FromColor(System.Drawing.Color.Black);
                    txt.LabelBackgroundColor = ScottPlot.Color.FromColor(System.Drawing.Color.FromArgb(180, 255, 255, 255));
                    txt.LabelBorderColor = ScottPlot.Color.FromColor(System.Drawing.Color.Gray);
                    txt.LabelBorderWidth = 1;
                    txt.LabelPadding = 3;

                    // 4. ADD ACCOUNT NAME LABEL (now positioned closer to the bar)
                    var labelTxt = fPlot.Plot.Add.Text(labels[i], positions[i], values[i]);
                    labelTxt.LabelFontSize = 10;
                    labelTxt.LabelBold = false; // Less prominent
                    labelTxt.LabelFontColor = ScottPlot.Color.FromColor(System.Drawing.Color.Black);
                    labelTxt.LabelBackgroundColor = ScottPlot.Color.FromColor(System.Drawing.Color.FromArgb(180, 255, 255, 255));
                    labelTxt.LabelBorderColor = ScottPlot.Color.FromColor(System.Drawing.Color.Gray);
                    labelTxt.LabelBorderWidth = 1;
                    labelTxt.LabelPadding = 3;

                    // Position label above or below bar depending on value
                    if (values[i] >= 0)
                    {
                        //txt.OffsetY = 10; // Above the bar
                        //txt.LabelAlignment = ScottPlot.Alignment.LowerCenter;
                        // Positive bars: Percentage (txt) goes further away, Label (labelTxt) stays closer
                        txt.OffsetY = 35; // FURTHER UP
                        txt.LabelAlignment = ScottPlot.Alignment.LowerCenter;

                        labelTxt.OffsetY = 5; // CLOSER TO THE BAR
                        labelTxt.LabelAlignment = ScottPlot.Alignment.LowerCenter;
                    }
                    else
                    {
                        //txt.OffsetY = -10; // Below the bar
                        //txt.LabelAlignment = ScottPlot.Alignment.UpperCenter;
                        // Negative bars: Percentage (txt) goes further away, Label (labelTxt) stays closer
                        txt.OffsetY = -20; // FURTHER DOWN
                        txt.LabelAlignment = ScottPlot.Alignment.UpperCenter;

                        labelTxt.OffsetY = -5; // CLOSER TO THE BAR
                        labelTxt.LabelAlignment = ScottPlot.Alignment.UpperCenter;
                    }

                    // Create a dummy scatter plot for legend with matching color
                    var dummyScatter = fPlot.Plot.Add.Scatter(new double[] { }, new double[] { });
                    dummyScatter.Color = barColor;
                    dummyScatter.LegendText = $"{labels[i]}: {values[i]:F2}%";
                    dummyScatter.LineWidth = 8;
                    dummyScatter.MarkerSize = 0;
                }

                // Calculate total TWR across all accounts
                double totalTWR = (double)twrPercentages["Total"]; //values.Sum();

                // Add a dummy scatter for the total in the legend
                var totalScatter = fPlot.Plot.Add.Scatter(new double[] { }, new double[] { });
                totalScatter.Color = ScottPlot.Color.FromColor(System.Drawing.Color.Black);
                totalScatter.LegendText = $"Total TWR: {totalTWR:F2}%";
                totalScatter.LineWidth = 8;
                totalScatter.MarkerSize = 0;

                // Customize the plot
                fPlot.Plot.Title($"TWR Performance: {months} months from {startMonth}");
                fPlot.Plot.XLabel("Account");
                fPlot.Plot.YLabel("TWR (%)");

                // Remove X-axis tick labels (no account names on x-axis)
                fPlot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual();

                // Add horizontal line at 0
                fPlot.Plot.Add.HorizontalLine(0, 1, ScottPlot.Color.FromColor(System.Drawing.Color.Black), ScottPlot.LinePattern.Dashed);

                // Show the legend in the upper right
                fPlot.Plot.Legend.IsVisible = true;
                fPlot.Plot.Legend.Alignment = ScottPlot.Alignment.UpperRight;

                // Auto-scale axes
                fPlot.Plot.Axes.AutoScale();

                // Add extra margin to accommodate labels and legend
                fPlot.Plot.Axes.Margins(bottom: 0.1, left: 0.1, right: 0.3, top: 0.15);

                // Refresh the plot
                fPlot.Refresh();
            }
            catch (Exception ex)
            {
                this._logger.SentEvent($"Error displaying chart: {ex.Message}", Logger.EnumLogLevel.EXCEPTION_LEVEL);
             //   MessageBox.Show($"Error displaying chart: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayAnnualizedChart(Dictionary<string, double?> periodReturns, string period)
        {
            try
            {
                // Clear previous plot
                fPlot.Plot.Clear();

                // Filter out accounts with no data and sort by return value
                var validData = periodReturns
                    .Where(kvp => kvp.Value.HasValue)
                    .OrderByDescending(kvp => kvp.Value.Value)
                    .ToList();

                if (validData.Count == 0)
                {
                    fPlot.Plot.Title("No data available for selected period");
                    fPlot.Refresh();
                    return;
                }

                // Prepare data for plotting
                string[] accountNames = validData.Select(kvp => kvp.Key).ToArray();
                string[] labels = new string[accountNames.Length];

                // Create labels with life start dates if available AND period is "Life of available data"
                bool showLifeStartDates = period.IndexOf("Life of available", StringComparison.OrdinalIgnoreCase) >= 0;

                for (int i = 0; i < accountNames.Length; i++)
                {
                    string truncatedName = TruncateAccountName(accountNames[i]);

                    // Add life start date only if "Life of available data" is selected
                    if (showLifeStartDates &&
                        accountLifeStartDates != null &&
                        accountLifeStartDates.ContainsKey(accountNames[i]))
                    {
                        labels[i] = $"{truncatedName} {accountLifeStartDates[accountNames[i]]}";
                    }
                    else
                    {
                        labels[i] = truncatedName;
                    }
                }

                double[] values = validData.Select(kvp => kvp.Value.Value).ToArray();
                double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();

                // Create bar plot
                var barPlot = fPlot.Plot.Add.Bars(positions, values);

                // Define color palette for bars
                ScottPlot.Color[] barColors = new ScottPlot.Color[]
                {
                    ScottPlot.Color.FromColor(System.Drawing.Color.DodgerBlue),
                    ScottPlot.Color.FromColor(System.Drawing.Color.OrangeRed),
                    ScottPlot.Color.FromColor(System.Drawing.Color.MediumSeaGreen),
                    ScottPlot.Color.FromColor(System.Drawing.Color.Gold),
                    ScottPlot.Color.FromColor(System.Drawing.Color.MediumPurple),
                    ScottPlot.Color.FromColor(System.Drawing.Color.Coral),
                    ScottPlot.Color.FromColor(System.Drawing.Color.Teal),
                    ScottPlot.Color.FromColor(System.Drawing.Color.Crimson),
                    ScottPlot.Color.FromColor(System.Drawing.Color.DarkOrange),
                    ScottPlot.Color.FromColor(System.Drawing.Color.SlateBlue),
                    ScottPlot.Color.FromColor(System.Drawing.Color.LimeGreen),
                    ScottPlot.Color.FromColor(System.Drawing.Color.DeepPink)
                };

                // Assign colors and create legend entries
                for (int i = 0; i < values.Length; i++)
                {
                    // Determine the color for the bar
                    ScottPlot.Color barColor = barColors[i % barColors.Length];

                    // Assign the color
                    barPlot.Bars[i].FillColor = barColor;

                    // Add value label on top of each bar
                    var txt = fPlot.Plot.Add.Text($"{values[i]:F2}%", positions[i], values[i]);
                    txt.LabelFontSize = 10;
                    txt.LabelBold = true;
                    txt.LabelFontColor = ScottPlot.Color.FromColor(System.Drawing.Color.Black);
                    txt.LabelBackgroundColor = ScottPlot.Color.FromColor(System.Drawing.Color.FromArgb(180, 255, 255, 255));
                    txt.LabelBorderColor = ScottPlot.Color.FromColor(System.Drawing.Color.Gray);
                    txt.LabelBorderWidth = 1;
                    txt.LabelPadding = 3;

                    // Add account name label
                    var labelTxt = fPlot.Plot.Add.Text(labels[i], positions[i], values[i]);
                    labelTxt.LabelFontSize = 10;
                    labelTxt.LabelBold = false;
                    labelTxt.LabelFontColor = ScottPlot.Color.FromColor(System.Drawing.Color.Black);
                    labelTxt.LabelBackgroundColor = ScottPlot.Color.FromColor(System.Drawing.Color.FromArgb(180, 255, 255, 255));
                    labelTxt.LabelBorderColor = ScottPlot.Color.FromColor(System.Drawing.Color.Gray);
                    labelTxt.LabelBorderWidth = 1;
                    labelTxt.LabelPadding = 3;

                    // Position label above or below bar depending on value
                    if (values[i] >= 0)
                    {
                        txt.OffsetY = 35; // Percentage further up
                        txt.LabelAlignment = ScottPlot.Alignment.LowerCenter;

                        labelTxt.OffsetY = 5; // Account name closer to bar
                        labelTxt.LabelAlignment = ScottPlot.Alignment.LowerCenter;
                    }
                    else
                    {
                        txt.OffsetY = -20; // Percentage further down
                        txt.LabelAlignment = ScottPlot.Alignment.UpperCenter;

                        labelTxt.OffsetY = -5; // Account name closer to bar
                        labelTxt.LabelAlignment = ScottPlot.Alignment.UpperCenter;
                    }

                    // Create a dummy scatter plot for legend with matching color
                    var dummyScatter = fPlot.Plot.Add.Scatter(new double[] { }, new double[] { });
                    dummyScatter.Color = barColor;
                    dummyScatter.LegendText = $"{labels[i]}: {values[i]:F2}%";
                    dummyScatter.LineWidth = 8;
                    dummyScatter.MarkerSize = 0;
                }

                // Add Total for Fidelity to legend if available for this period
                if (annualizedTotalData != null && annualizedTotalData.ContainsKey(period))
                {
                    double? totalValue = annualizedTotalData[period];
                    if (totalValue.HasValue)
                    {
                        var totalScatter = fPlot.Plot.Add.Scatter(new double[] { }, new double[] { });
                        totalScatter.Color = ScottPlot.Color.FromColor(System.Drawing.Color.Black);
                        totalScatter.LegendText = $"Total for Fidelity: {totalValue.Value:F2}%";
                        totalScatter.LineWidth = 8;
                        totalScatter.MarkerSize = 0;
                    }
                }

                // Customize the plot
                string periodLabel = period.Equals("YTD", StringComparison.OrdinalIgnoreCase)
                    ? "YTD"
                    : $"{period}";

                string dateLabel = !string.IsNullOrEmpty(annualizedReportDate) ? annualizedReportDate : "2025-Oct";
                fPlot.Plot.Title($"TWR Performance: {periodLabel} from {dateLabel}");
                fPlot.Plot.XLabel("Account");
                fPlot.Plot.YLabel("TWR (%)");

                // Remove X-axis tick labels
                fPlot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual();

                // Add horizontal line at 0
                fPlot.Plot.Add.HorizontalLine(0, 1, ScottPlot.Color.FromColor(System.Drawing.Color.Black), ScottPlot.LinePattern.Dashed);

                // Show the legend in the upper right
                fPlot.Plot.Legend.IsVisible = true;
                fPlot.Plot.Legend.Alignment = ScottPlot.Alignment.UpperRight;

                // Auto-scale axes
                fPlot.Plot.Axes.AutoScale();

                // Add extra margin to accommodate labels and legend
                fPlot.Plot.Axes.Margins(bottom: 0.1, left: 0.1, right: 0.3, top: 0.15);

                // Refresh the plot
                fPlot.Refresh();
            }
            catch (Exception ex)
            {
                this._logger.SentEvent($"Error displaying chart: {ex.Message}", Logger.EnumLogLevel.EXCEPTION_LEVEL);
               // MessageBox.Show($"Error displaying chart: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to clean account name by removing account numbers
        private string TruncateAccountName(string accountName)
        {
            if (string.IsNullOrWhiteSpace(accountName))
                return accountName;

            string[] words = accountName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Remove the last word if it looks like an account number (starts with letter followed by digits)
            // Examples: "X66823550", "Y81530220", "249908834"
            if (words.Length > 0)
            {
                string lastWord = words[words.Length - 1];

                // Check if last word is all digits OR starts with a letter followed by digits
                bool isAccountNumber = lastWord.All(char.IsDigit) ||
                                      (lastWord.Length > 1 &&
                                       char.IsLetter(lastWord[0]) &&
                                       lastWord.Substring(1).All(char.IsDigit));

                if (isAccountNumber)
                {
                    // Remove the account number
                    return string.Join(" ", words.Take(words.Length - 1)).Trim();
                }
            }

            return accountName.Trim();
        }

        private void timerGUI_Tick(object sender, EventArgs e)
        {
            try
            {
                // Check if we have data in either mode
                bool hasData = false;
                if (this.chkAnnualized.Checked)
                {
                    // Annualized mode: check if annualizedData has any accounts
                    hasData = (annualizedData != null && annualizedData.Count > 0);
                }
                else
                {
                    // Normal TWR mode: check if parser has any accounts
                    hasData = (parser?.accounts.Count > 0);
                }
                this.grpSelect.Visible = hasData;
                this.grpMonths.Visible = !this.chkAnnualized.Checked & hasData;
            }
            catch (Exception ex)
            {
                // Silent catch for timer tick errors
            }
        }

        // Event handler for numMonths ValueChanged - wire this up in the designer
        // to numMonths.ValueChanged event
        private void numMonths_ValueChanged(object sender, EventArgs e)
        {
            // Only auto-update in normal TWR mode
            if (!this.chkAnnualized.Checked &&
                comboMonths.SelectedItem != null &&
                parser != null &&
                parser.accounts.Count > 0)
            {
                string selectedMonth = comboMonths.SelectedItem.ToString() ?? string.Empty;
                if (!string.IsNullOrEmpty(selectedMonth))
                {
                    int numberOfMonths = (int)numMonths.Value;

                    try
                    {
                        var calculationResult = parser.CalculateTWR(selectedMonth, numberOfMonths);
                        var twrPercentages = calculationResult.GetTWRPercentages();

                        // Display results in plot
                        DisplayTWRChart(twrPercentages, selectedMonth, numberOfMonths);
                    }
                    catch (Exception ex)
                    {
                        this._logger.SentEvent($"Error auto-updating chart: {ex.Message}", Logger.EnumLogLevel.WARNING_LEVEL);
                    }
                }
            }
        }



        private void LoadDataFromCSV_Annualized(object sender, EventArgs e)
        {
            try
            {
                this.openFileD.Title = "Load Annualized Return Data from CSV File";
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
                    string[] lines = System.IO.File.ReadAllLines(this.openFileD.FileName);

                    if (lines.Length < 2)
                    {
                        this._logger.SentEvent("Invalid CSV file format", Logger.EnumLogLevel.ERROR_LEVEL);
                    //    MessageBox.Show("Invalid CSV file format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Extract date from first line: "Prior month end performance as of Nov-30-2025"
                    string reportDate = "Unknown";
                    if (lines[0].Contains("as of"))
                    {
                        int asOfIndex = lines[0].IndexOf("as of");
                        if (asOfIndex >= 0)
                        {
                            string datePart = lines[0].Substring(asOfIndex + 5).Trim();
                            // Remove trailing commas
                            reportDate = datePart.TrimEnd(',');
                        }
                    }

                    // Find the Time-weighted rate of return header line
                    int headerLineIndex = -1;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("Time-weighted rate of return"))
                        {
                            headerLineIndex = i;
                            break;
                        }
                    }

                    if (headerLineIndex == -1)
                    {
                        this._logger.SentEvent("Could not find 'Time-weighted rate of return' table in CSV file", Logger.EnumLogLevel.ERROR_LEVEL);
                    //    MessageBox.Show("Could not find 'Time-weighted rate of return' table in CSV file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Parse header row to get periods (1 Month, 3 Month, YTD, etc.)
                    string[] headers = lines[headerLineIndex].Split(',');
                    annualizedPeriods = new List<string>();
                    int lifeStartDateColumnIndex = -1;

                    // Find period columns and life start date column
                    for (int i = 1; i < headers.Length; i++)
                    {
                        string header = headers[i].Trim();
                        if (string.IsNullOrEmpty(header))
                            continue;

                        if (header.Equals("Life start date", StringComparison.OrdinalIgnoreCase))
                        {
                            lifeStartDateColumnIndex = i;
                            break;
                        }
                        else
                        {
                            annualizedPeriods.Add(header);
                        }
                    }

                    // Parse data rows (start from line after header)
                    annualizedData = new Dictionary<string, Dictionary<string, double?>>();
                    Dictionary<string, string> accountLifeStartDates = new Dictionary<string, string>();

                    for (int lineIdx = headerLineIndex + 1; lineIdx < lines.Length; lineIdx++)
                    {
                        string line = lines[lineIdx].Trim();
                        if (string.IsNullOrEmpty(line))
                            continue;

                        // Stop if we hit the Money-weighted rate of return section
                        if (line.Contains("Money-weighted rate of return"))
                            break;

                        string[] values = line.Split(',');
                        if (values.Length < 2)
                            continue;

                        string accountName = values[0].Trim();
                        if (string.IsNullOrEmpty(accountName))
                            continue;

                        Dictionary<string, double?> accountReturns = new Dictionary<string, double?>();

                        // Parse returns for each period
                        for (int i = 0; i < annualizedPeriods.Count && i + 1 < values.Length; i++)
                        {
                            string valueStr = values[i + 1].Trim();
                            double? returnValue = null;

                            // Handle both "NA", "--", and empty values
                            if (!string.IsNullOrEmpty(valueStr) &&
                                !valueStr.Equals("NA", StringComparison.OrdinalIgnoreCase) &&
                                !valueStr.Equals("--", StringComparison.OrdinalIgnoreCase))
                            {
                                // Remove percentage sign if present
                                valueStr = valueStr.Replace("%", "");

                                if (double.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedValue))
                                {
                                    returnValue = parsedValue;
                                }
                            }

                            accountReturns[annualizedPeriods[i]] = returnValue;
                        }

                        // Store Total row separately, don't add to main data
                        if (accountName.Equals("Total", StringComparison.OrdinalIgnoreCase))
                        {
                            annualizedTotalData = accountReturns;
                        }
                        else
                        {
                            annualizedData[accountName] = accountReturns;
                        }

                        // Extract life start date if available (only for non-Total accounts)
                        if (!accountName.Equals("Total", StringComparison.OrdinalIgnoreCase) &&
                            lifeStartDateColumnIndex > 0 && lifeStartDateColumnIndex < values.Length)
                        {
                            string lifeStartDate = values[lifeStartDateColumnIndex].Trim();
                            if (!string.IsNullOrEmpty(lifeStartDate))
                            {
                                accountLifeStartDates[accountName] = lifeStartDate;
                            }
                        }
                    }

                    // Store life start dates for later use in chart display
                    if (accountLifeStartDates.Count > 0)
                    {
                        // Store in a class field for use in DisplayAnnualizedChart
                        this.accountLifeStartDates = accountLifeStartDates;
                    }

                    // Store report date for chart title
                    this.annualizedReportDate = reportDate;

                    // Populate combobox with available periods
                    this.comboMonths.DataSource = new List<string>(annualizedPeriods);

                    // Update UI
                    this.lblTotalM.Text = annualizedPeriods.Count.ToString();
                    this.numMonths.Maximum = 1; // Not used in annualized mode
                    this.numMonths.Value = 1;
                    this.numMonths.Enabled = false; // Disable since we select by period, not count

                    // Clear the plot
                    fPlot.Plot.Clear();
                    fPlot.Refresh();

                    groupBox.Text = $"Annualized Return - {Path.GetFileName(this.openFileD.FileName)}";

                    this._logger.SentEvent($"Loaded annualized data for {annualizedData.Count} accounts with {annualizedPeriods.Count} periods",
                        Logger.EnumLogLevel.INFO_LEVEL);
                }
            }
            catch (Exception ex)
            {
                this._logger.SentEvent($"Error loading annualized data: {ex.Message}", Logger.EnumLogLevel.EXCEPTION_LEVEL);
              //  MessageBox.Show($"Error loading annualized data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}