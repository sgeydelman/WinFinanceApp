using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SLGAutomationLib;
using System.Globalization;
using System.IO;
using ScottPlot;
using Color = ScottPlot.Color;


namespace WinFinanceApp
{
    public partial class FormAcctSpending : Form
    {
        protected Logger _logger;
        Ini inif;
        protected CMyFinance MyFinance;
        List<SpendingRecord> SpendingList = new List<SpendingRecord>();
        ScottPlot.Plottables.Crosshair MyCrosshair;
        ScottPlot.Plottables.Scatter scatter;
        ScottPlot.Plottables.Text MyHoverText;
        string spendingFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "FinanceData", "spending.csv");
        ContextMenuStrip customMenu = new ContextMenuStrip();
        public FormAcctSpending()
        {
            InitializeComponent();

            // --- ADD THESE LINES TO YOUR CONSTRUCTOR ---

            // This disables the default ScottPlot menu (zoom, pan, etc)
            fPlot.Menu.Clear();

            // Create the menu items
            customMenu.Items.Add("Autoscale", null, (s, e) => {
                fPlot.Plot.Axes.AutoScale();
                fPlot.Refresh();
            });

            customMenu.Items.Add("Financial Summary", null, MenuGetStatistic_Click);

            // This attaches your menu to the plot control
            fPlot.ContextMenuStrip = customMenu;

            // --- END OF ADDED MENU CODE ---

            this._logger = Logger.Instance;
            inif = new Ini(_logger.SetupPath);
            this.MyFinance = CMyFinance.Instance;

        }

        private void FormAcctSpending_Load(object sender, EventArgs e)
        {
            this._logger = Logger.Instance;
            inif = new Ini(_logger.SetupPath);
            this.MyFinance = CMyFinance.Instance;

            fPlot.MouseDown += fPlot_MouseDown;
            fPlot.MouseUp += fPlot_MouseUp;

            if (BtnPlot.Image != null)
            {
                // 1. FORCE THE SIZE: Use a small fixed size (24x24).
                // If we use BtnPlot.Width, the icon becomes a background and causes overlap.
                System.Drawing.Image smallIcon = ResizeButtonIcon(BtnPlot.Image, 24, 24);

                // 2. TRANSPARENCY: 0.2f makes it a very subtle watermark.
                BtnPlot.Image = MakeImageTransparent(smallIcon, 0.99f);

                // 3. THE ALIGNMENT COMBO:
                // This is the most stable configuration for WinForms buttons:
                BtnPlot.TextImageRelation = TextImageRelation.ImageBeforeText;
                BtnPlot.ImageAlign = ContentAlignment.MiddleLeft;
                BtnPlot.TextAlign = ContentAlignment.MiddleCenter;

                // 4. THE GAP:
                // Pushing the text 10 pixels to the right to ensure it never touches the icon.
                BtnPlot.Padding = new Padding(10, 0, 0, 0);

                // Ensure the button doesn't shrink to fit the small icon
                BtnPlot.AutoSize = false;
            }

            AdjustLayout();

            this.BeginInvoke(new Action(() => {
                BtnPlot.PerformClick();
            }));
        }
        private void LoadSpendingCSV()
        {
            // Implementation for loading spending CSV
            //filePath is the path to the CSV file in C:\Users\Public\Documents

            SpendingList = new List<SpendingRecord>();
           // string spendingFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "FinanceData", "spending.csv");
            if (!File.Exists(spendingFile))
            {
                this._logger.SentEvent("Spending CSV file not found: " + spendingFile, Logger.EnumLogLevel.ERROR_LEVEL);
                return;
            }
            try
            {
                // Read all lines from the CSV file skippinf first line (header)
                var lines = File.ReadAllLines(spendingFile).Skip(1);
                //split each line in Lines
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines
                    var columns = line.Split(',');
                    string[] formats = { "MM/dd/yyyy", "M/d/yyyy", "MM/d/yyyy", "M/dd/yyyy" };
                    if (columns.Length < 6) continue; // Skip lines that don't have all data points
                    //add to SpendingList a new SpendingRecord with consecutive columns as Date, ammount , fundAdded, dividcentsIn,dividentsOut ,withdrawVal
                    SpendingList.Add(new SpendingRecord
                    {
                        //convert column[0] from string to DateTime Date
                        Date = DateTime.ParseExact(columns[0], formats, CultureInfo.InvariantCulture, DateTimeStyles.None),
                        Amount = double.Parse(columns[1]),
                        FundAdded = double.Parse(columns[2]),
                        DividentsOut = double.Parse(columns[3]),
                        DividentsIn = double.Parse(columns[4]),
                        WithdrawVal = double.Parse(columns[5])
                        
                    });

                }
                SpendingList = SpendingList.OrderBy(r => r.Date).ToList();
                for (int i = 1; i < SpendingList.Count; i++)
                {
                    // Display each record in the logger for verification
                    var record0 = SpendingList[i-1];
                    var record1 = SpendingList[i];
                    record1.MonthlySpending = Math.Round((record0.Amount - record1.Amount + record1.FundAdded + record1.DividentsOut),2);
                    SpendingList[i] = record1;
                    
                }

            }
            catch (Exception ex)
            {
                this._logger.SentEvent("Error reading spending CSV file: " + ex.Message, Logger.EnumLogLevel.ERROR_LEVEL);

            }
        }

        private void FormAcctSpending_Resize(object sender, EventArgs e)
        {
            AdjustLayout();
        }

        private void AdjustLayout()
        {
            // 1. Force the plot to fill the GroupBox
            fPlot.Dock = DockStyle.Fill;

            // 2. Ensure the GroupBox (the plot's parent) is stretching
            // Assuming your GroupBox is named groupBox1
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            if (fPlot != null && fPlot.Plot != null)
            {
                // 3. This is the "magic" command that recalculates the internal 
                // ScottPlot bitmap to match the new screen size.
                fPlot.Refresh();
            }
        }



        //private void BtnPlot_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        LoadSpendingCSV();
        //        if (SpendingList == null || SpendingList.Count == 0) return;

        //        // --- 1. DYNAMIC YEAR CALCULATIONS ---
        //        int curYear = DateTime.Now.Year;
        //        int prevYear = curYear - 1;
        //        int prevPrevYear = curYear - 2;

        //        double avgYTD = SpendingList.Where(r => r.Date.Year == curYear)
        //                                    .Select(r => r.MonthlySpending)
        //                                    .DefaultIfEmpty(0).Average();

        //        double avgPrev = SpendingList.Where(r => r.Date.Year == prevYear)
        //                                     .Select(r => r.MonthlySpending)
        //                                     .DefaultIfEmpty(0).Average();

        //        double avgPrevPrev = SpendingList.Where(r => r.Date.Year == prevPrevYear)
        //                                         .Select(r => r.MonthlySpending)
        //                                         .DefaultIfEmpty(0).Average();

        //        // --- 2. PREPARE PLOT ---
        //        fPlot.Plot.Clear();

        //        double[] dates = SpendingList.Select(r => r.Date.ToOADate()).ToArray();
        //        double[] monthlySpending = SpendingList.Select(r => r.MonthlySpending).ToArray();
        //        double[] totalDividends = SpendingList.Select(r => r.DividentsIn + r.DividentsOut).ToArray();

        //        // Main Spending Line
        //        scatter = fPlot.Plot.Add.Scatter(dates, monthlySpending);
        //        scatter.LineWidth = 3;
        //        scatter.Color = ScottPlot.Colors.Blue;
        //        // scatter.MarkerSize = 12;
        //        // scatter.MarkerShape = ScottPlot.MarkerShape.FilledCircle;
        //        scatter.MarkerSize = 15;
        //        scatter.MarkerShape = ScottPlot.MarkerShape.OpenCircle; // Makes them look like targets
        //        scatter.MarkerLineWidth = 2;
        //        scatter.MarkerFillColor = ScottPlot.Colors.Red;
        //        scatter.MarkerLineColor = ScottPlot.Colors.Blue;
        //        scatter.LegendText = "Monthly Spending";

        //        // New Dividend Line (In + Out)
        //        var divScatter = fPlot.Plot.Add.Scatter(dates, totalDividends);
        //        divScatter.LineWidth = 2;
        //        divScatter.Color = ScottPlot.Colors.Purple;
        //        divScatter.MarkerSize = 0; // Clean line without dots
        //        divScatter.LegendText = "Total Dividends (In+Out)";

        //        // --- 3. DYNAMIC HORIZONTAL AVERAGE LINES ---
        //        var lineYTD = fPlot.Plot.Add.HorizontalLine(avgYTD);
        //        lineYTD.LegendText = $"{curYear} YTD Avg: {avgYTD:C0}";
        //        lineYTD.LineColor = ScottPlot.Colors.SlateGray;
        //        lineYTD.LineWidth = 2;

        //        var linePrev = fPlot.Plot.Add.HorizontalLine(avgPrev);
        //        linePrev.LegendText = $"{prevYear} Avg: {avgPrev:C0}";
        //        linePrev.LineColor = ScottPlot.Colors.Green;
        //        linePrev.LinePattern = ScottPlot.LinePattern.Dashed;

        //        var linePrevPrev = fPlot.Plot.Add.HorizontalLine(avgPrevPrev);
        //        linePrevPrev.LegendText = $"{prevPrevYear} Avg: {avgPrevPrev:C0}";
        //        linePrevPrev.LineColor = ScottPlot.Colors.Orange;
        //        linePrevPrev.LinePattern = ScottPlot.LinePattern.Dotted;

        //        // --- 4. AXIS SETUP ---
        //        var monthUnit = new ScottPlot.TickGenerators.TimeUnits.Month();
        //        var dtGen = new ScottPlot.TickGenerators.DateTimeFixedInterval(monthUnit, 1);
        //        dtGen.LabelFormatter = (DateTime dt) => dt.ToString("MM/yy");
        //        fPlot.Plot.Axes.Bottom.TickGenerator = dtGen;
        //        fPlot.Plot.Axes.Bottom.TickLabelStyle.Rotation = -45;

        //        // Y-Axis Currency Formatting
        //        var yGen = new ScottPlot.TickGenerators.NumericAutomatic();
        //        yGen.LabelFormatter = (double val) => val.ToString("C0");
        //        fPlot.Plot.Axes.Left.TickGenerator = yGen;

        //        // Performance Settings
        //        fPlot.Plot.FigureBackground.Color = ScottPlot.Colors.White;
        //        fPlot.Plot.Axes.ContinuouslyAutoscale = false;

        //        // --- 5. INTERACTION & TOOLTIP SETUP ---
        //        // Note: MyCrosshair is removed to avoid confusion and lag

        //        MyHoverText = fPlot.Plot.Add.Text("", 0, 0);
        //        MyHoverText.LabelFontSize = 14;
        //        MyHoverText.LabelBold = true;
        //        MyHoverText.LabelFontName = "Verdana";
        //        MyHoverText.LabelFontColor = ScottPlot.Colors.Black;
        //        MyHoverText.LabelBackgroundColor = ScottPlot.Colors.Yellow;
        //        MyHoverText.LabelBorderColor = ScottPlot.Colors.Black;
        //        MyHoverText.LabelBorderWidth = 2;
        //        MyHoverText.LabelPadding = 8;
        //        MyHoverText.LabelAlignment = ScottPlot.Alignment.LowerCenter;
        //        MyHoverText.IsVisible = false;

        //        // --- 6. FINALIZE ---
        //        fPlot.Plot.Axes.AutoScale();

        //        // Ensure 25% margin at top so the tall multi-line tooltip has room
        //        var limits = fPlot.Plot.Axes.GetLimits();
        //        fPlot.Plot.Axes.SetLimitsY(0, limits.Top * 1.25);

        //        fPlot.Plot.ShowLegend(ScottPlot.Alignment.UpperRight);
        //        fPlot.Refresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.SentEvent("Plotting Error: " + ex.Message, Logger.EnumLogLevel.ERROR_LEVEL);
        //    }
        //}
        private void BtnPlot_Click(object sender, EventArgs e)
        {
            try
            {
                LoadSpendingCSV();
                if (SpendingList == null || SpendingList.Count == 0) return;

                // --- 1. DYNAMIC YEAR CALCULATIONS ---
                int curYear = DateTime.Now.Year;
                int prevYear = curYear - 1;
                int prevPrevYear = curYear - 2;

                double avgYTD = SpendingList.Where(r => r.Date.Year == curYear)
                                            .Select(r => r.MonthlySpending)
                                            .DefaultIfEmpty(0).Average();

                double avgPrev = SpendingList.Where(r => r.Date.Year == prevYear)
                                             .Select(r => r.MonthlySpending)
                                             .DefaultIfEmpty(0).Average();

                double avgPrevPrev = SpendingList.Where(r => r.Date.Year == prevPrevYear)
                                                 .Select(r => r.MonthlySpending)
                                                 .DefaultIfEmpty(0).Average();

                // --- 2. PREPARE PLOT ---
                fPlot.Plot.Clear();

                double[] dates = SpendingList.Select(r => r.Date.ToOADate()).ToArray();
                double[] monthlySpending = SpendingList.Select(r => r.MonthlySpending).ToArray();
                double[] totalDividends = SpendingList.Select(r => r.DividentsIn + r.DividentsOut).ToArray();

                // Main Spending Line (Standard months are Open Circles)
                scatter = fPlot.Plot.Add.Scatter(dates, monthlySpending);
                scatter.LineWidth = 3;
                scatter.Color = ScottPlot.Colors.Blue;
                scatter.MarkerSize = 15;
                scatter.MarkerShape = ScottPlot.MarkerShape.OpenCircle;
                scatter.LegendText = "Monthly Spending";

                // --- NEW: SPECIAL MARKERS & LABELS FOR FUND ADDED ---
                //foreach (var record in SpendingList)
                //{
                //    if (record.FundAdded > 0)
                //    {
                //        // Add the Solid Red Marker
                //        var m = fPlot.Plot.Add.Marker(record.Date.ToOADate(), record.MonthlySpending);
                //        m.Shape = ScottPlot.MarkerShape.FilledCircle;
                //        m.Size = 12; // Slightly smaller to look nested
                //        m.Color = ScottPlot.Colors.Red;

                //        // Add the Floating Text Label
                //        var lbl = fPlot.Plot.Add.Text($"+{record.FundAdded:C0}", record.Date.ToOADate(), record.MonthlySpending);
                //        lbl.LabelFontSize = 10;
                //        lbl.LabelBold = true;
                //        lbl.LabelFontColor = ScottPlot.Colors.Red;
                //        lbl.LabelBackgroundColor = ScottPlot.Colors.White.WithAlpha(0.8);
                //        lbl.LabelBorderColor = ScottPlot.Colors.Red;
                //        lbl.LabelBorderWidth = 1;
                //        lbl.LabelAlignment = ScottPlot.Alignment.LowerCenter; // Anchors text ABOVE the point
                //        lbl.OffsetY = -12; // Negative moves it UP
                //    }
                //}
                // --- SPECIAL MARKERS & LABELS FOR SPECIAL TRANSACTIONS ---
                //foreach (var record in SpendingList)
                //{
                //    // 1. HANDLE FUNDS ADDED (Money In)
                //    if (record.FundAdded > 0)
                //    {
                //        var m = fPlot.Plot.Add.Marker(record.Date.ToOADate(), record.MonthlySpending);
                //        m.Shape = ScottPlot.MarkerShape.FilledCircle;
                //        m.Size = 12;
                //        m.Color = ScottPlot.Colors.Red;

                //        var lbl = fPlot.Plot.Add.Text($"+{record.FundAdded:C0}", record.Date.ToOADate(), record.MonthlySpending);
                //        lbl.LabelFontSize = 10;
                //        lbl.LabelBold = true;
                //        lbl.LabelFontColor = ScottPlot.Colors.Red;
                //        lbl.LabelBackgroundColor = ScottPlot.Colors.White.WithAlpha(0.8);
                //        lbl.LabelBorderColor = ScottPlot.Colors.Red;
                //        lbl.LabelBorderWidth = 1;
                //        lbl.LabelAlignment = ScottPlot.Alignment.LowerCenter; // Above the point
                //        lbl.OffsetY = -12;
                //    }

                //    // 2. HANDLE WITHDRAWALS (Money Out)
                //    if (record.WithdrawVal > 0)
                //    {
                //        var m = fPlot.Plot.Add.Marker(record.Date.ToOADate(), record.MonthlySpending);
                //        m.Shape = ScottPlot.MarkerShape.FilledCircle;
                //        m.Size = 12;
                //        m.Color = ScottPlot.Colors.DarkOrange; // Different color for withdrawals

                //        var lbl = fPlot.Plot.Add.Text($"-{record.WithdrawVal:C0}", record.Date.ToOADate(), record.MonthlySpending);
                //        lbl.LabelFontSize = 10;
                //        lbl.LabelBold = true;
                //        lbl.LabelFontColor = ScottPlot.Colors.DarkOrange;
                //        lbl.LabelBackgroundColor = ScottPlot.Colors.White.WithAlpha(0.8);
                //        lbl.LabelBorderColor = ScottPlot.Colors.DarkOrange;
                //        lbl.LabelBorderWidth = 1;

                //        // We place withdrawals BELOW the point so they don't clash if a month has both
                //        lbl.LabelAlignment = ScottPlot.Alignment.UpperCenter;
                //        lbl.OffsetY = 12;
                //    }
                //}
                // --- SPECIAL MARKERS & LABELS FOR SPECIAL TRANSACTIONS ---
                // --- SPECIAL MARKERS & LABELS FOR SPECIAL TRANSACTIONS ---
                foreach (var record in SpendingList)
                {
                    double xCoord = record.Date.ToOADate();
                    double yCoord = record.MonthlySpending;

                    // 1. FUNDS ADDED (Checks for positive values)
                    if (record.FundAdded > 0.01)
                    {
                        var m = fPlot.Plot.Add.Marker(xCoord, yCoord);
                        m.Shape = ScottPlot.MarkerShape.FilledCircle;
                        m.Size = 12;
                        m.Color = ScottPlot.Colors.Red;

                        var lbl = fPlot.Plot.Add.Text($"+{record.FundAdded:C0}", xCoord, yCoord);
                        lbl.LabelFontColor = ScottPlot.Colors.Red;
                        lbl.LabelAlignment = ScottPlot.Alignment.LowerCenter;
                        lbl.OffsetY = -12;
                        // ... (Keep your other styling: background, border, etc.)
                    }

                    // 2. WITHDRAWALS (Updated to check for NEGATIVE values)
                    // Your CSV uses -18700, so we check for < -0.01
                    if (record.WithdrawVal < -0.01)
                    {
                        // Draw the Marker
                        var m = fPlot.Plot.Add.Marker(xCoord, yCoord);
                        m.Shape = ScottPlot.MarkerShape.FilledDiamond; // Diamond looks great for withdrawals
                        m.Size = 14;
                        m.Color = ScottPlot.Colors.Magenta;

                        // Draw the Label (using Abs to avoid "-$-18,700")
                        var lbl = fPlot.Plot.Add.Text($"-{Math.Abs(record.WithdrawVal):C0}", xCoord, yCoord);
                        lbl.LabelFontSize = 10;
                        lbl.LabelBold = true;
                        lbl.LabelFontColor = ScottPlot.Colors.Magenta;
                        lbl.LabelBackgroundColor = ScottPlot.Colors.White.WithAlpha(0.9);
                        lbl.LabelBorderColor = ScottPlot.Colors.Magenta;
                        lbl.LabelBorderWidth = 1;

                        // Position BELOW the point so it doesn't fight the FundAdded labels
                        lbl.LabelAlignment = ScottPlot.Alignment.UpperCenter;
                        lbl.OffsetY = 12;
                    }
                }
                // Dividend Line (Purple)
                var divScatter = fPlot.Plot.Add.Scatter(dates, totalDividends);
                divScatter.LineWidth = 2;
                divScatter.Color = ScottPlot.Colors.Purple;
                divScatter.MarkerSize = 0;
                divScatter.LegendText = "Total Dividends (In+Out)";

                // --- 3. DYNAMIC HORIZONTAL AVERAGE LINES ---
                var lineYTD = fPlot.Plot.Add.HorizontalLine(avgYTD);
                lineYTD.LegendText = $"{curYear} YTD Avg: {avgYTD:C0}";
                lineYTD.LineColor = ScottPlot.Colors.SlateGray;
                lineYTD.LineWidth = 2;

                var linePrev = fPlot.Plot.Add.HorizontalLine(avgPrev);
                linePrev.LegendText = $"{prevYear} Avg: {avgPrev:C0}";
                linePrev.LineColor = ScottPlot.Colors.Green;
                linePrev.LinePattern = ScottPlot.LinePattern.Dashed;

                var linePrevPrev = fPlot.Plot.Add.HorizontalLine(avgPrevPrev);
                linePrevPrev.LegendText = $"{prevPrevYear} Avg: {avgPrevPrev:C0}";
                linePrevPrev.LineColor = ScottPlot.Colors.Orange;
                linePrevPrev.LinePattern = ScottPlot.LinePattern.Dotted;

                // --- 4. AXIS SETUP ---
                var monthUnit = new ScottPlot.TickGenerators.TimeUnits.Month();
                var dtGen = new ScottPlot.TickGenerators.DateTimeFixedInterval(monthUnit, 1);
                dtGen.LabelFormatter = (DateTime dt) => dt.ToString("MM/yy");
                fPlot.Plot.Axes.Bottom.TickGenerator = dtGen;
                fPlot.Plot.Axes.Bottom.TickLabelStyle.Rotation = -45;

                var yGen = new ScottPlot.TickGenerators.NumericAutomatic();
                yGen.LabelFormatter = (double val) => val.ToString("C0");
                fPlot.Plot.Axes.Left.TickGenerator = yGen;

                // --- 5. INTERACTION SETUP ---
                MyHoverText = fPlot.Plot.Add.Text("", 0, 0);
                MyHoverText.LabelFontSize = 14;
                MyHoverText.LabelBold = true;
                MyHoverText.LabelFontColor = ScottPlot.Colors.Black;
                MyHoverText.LabelBackgroundColor = ScottPlot.Colors.Yellow;
                MyHoverText.LabelBorderColor = ScottPlot.Colors.Black;
                MyHoverText.LabelBorderWidth = 2;
                MyHoverText.LabelPadding = 8;

                // Use these two lines to control the position:
                MyHoverText.LabelAlignment = ScottPlot.Alignment.UpperCenter; // Anchor is at the TOP of the text
                MyHoverText.OffsetY = 15; // Pushes the text 15 pixels DOWN from the point

                MyHoverText.IsVisible = false;

                // --- 6. FINALIZE ---
                fPlot.Plot.Axes.AutoScale();
                var limits = fPlot.Plot.Axes.GetLimits();
                fPlot.Plot.Axes.SetLimitsY(0, limits.Top * 1.25); // Room for labels at the top

                fPlot.Plot.ShowLegend(ScottPlot.Alignment.UpperRight);
                fPlot.Refresh();
            }
            catch (Exception ex)
            {
                _logger?.SentEvent("Plotting Error: " + ex.Message, Logger.EnumLogLevel.ERROR_LEVEL);
            }
        }
        // At the top of your class
        private ScottPlot.Coordinates LastSnappedCoord;

       
        private void fPlot_MouseMove(object sender, MouseEventArgs e)
        {

        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try 
            {
                // get a current date as string in format MM/dd/yyyy
                string currentDate = DateTime.Now.ToString("MM/dd/yyyy");
                // create string with current date, numAmount,numFundAdded,numDividentsOut,numDividentIn,numWithdrawVal 
                string newRecord = $"{currentDate},{numAmount.Value},{numFundAdded.Value},{numDividentsOut.Value},{numDividentsIn.Value},{numWithdrawVal.Value}";
                //give a warning in meassge box that new records  for this date with those values will written to spendingFile
                var result = MessageBox.Show($"This will append a new record to the spending file:{Environment.NewLine}{newRecord}{Environment.NewLine}Do you want to continue?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //append newRecord to spendingFile
                    File.AppendAllText(spendingFile, newRecord + Environment.NewLine);
                    
                    _logger?.SentEvent("Update Success: " + "New record added successfully.", Logger.EnumLogLevel.INFO_LEVEL);
                }

            }
            catch (Exception ex)
            {
                _logger?.SentEvent("Update Error: " + ex.Message, Logger.EnumLogLevel.ERROR_LEVEL);
            }
            

        }
        private void fPlot_MouseDown(object sender, MouseEventArgs e)
        {
            // 1. Determine where the user clicked
            ScottPlot.Pixel mousePixel = new ScottPlot.Pixel(e.X, e.Y);
            ScottPlot.Coordinates mouseCoords = fPlot.Plot.GetCoordinates(mousePixel);

            // 2. Find the nearest data point on the Spending Line
            var nearest = scatter.Data.GetNearest(mouseCoords, fPlot.Plot.LastRender);

            if (nearest.IsReal)
            {
                // 3. Find the matching record in your list
                var foundRecords = SpendingList.Where(r => Math.Abs(r.Date.ToOADate() - nearest.Coordinates.X) < 0.01);

                if (foundRecords.Any())
                {
                    var record = foundRecords.First();

                    // 4. Build the dynamic tooltip
                    DateTime ptDate = DateTime.FromOADate(nearest.Coordinates.X);
                    string txt = $"{ptDate:MMM, yyyy}{Environment.NewLine}";
                    txt += $"Spent: {record.MonthlySpending:C2}{Environment.NewLine}";

                    double totalDiv = record.DividentsIn + record.DividentsOut;
                    if (totalDiv != 0) txt += $"Total Div: {totalDiv:C2}{Environment.NewLine}";
                    if (record.WithdrawVal != 0) txt += $"Withd: {record.WithdrawVal:C2}";

                    // 5. Dynamic Coloring based on Goal (e.g., $4000)
                    double spendingGoal = 4000;
                    if (record.MonthlySpending > spendingGoal)
                    {
                        MyHoverText.LabelBackgroundColor = ScottPlot.Colors.Salmon;
                        MyHoverText.LabelFontColor = ScottPlot.Colors.White;
                    }
                    else
                    {
                        MyHoverText.LabelBackgroundColor = ScottPlot.Colors.Yellow; // Default visibility color
                        MyHoverText.LabelFontColor = ScottPlot.Colors.Black;
                    }

                    // 6. Update and Show the Label
                    MyHoverText.Location = nearest.Coordinates;
                    MyHoverText.LabelText = txt.TrimEnd();
                    MyHoverText.IsVisible = true;

                    fPlot.Refresh();
                }
            }
        }

        private void fPlot_MouseUp(object sender, MouseEventArgs e)
        {
            if (MyHoverText != null && MyHoverText.IsVisible)
            {
                MyHoverText.IsVisible = false;
                fPlot.Refresh();
            }
        }
        private System.Drawing.Image MakeImageTransparent(System.Drawing.Image image, float opacity)
        {
            // Create a new bitmap with the same dimensions
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(image.Width, image.Height);

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
            {
                // The ColorMatrix is what controls transparency (Matrix33 is the Alpha channel)
                System.Drawing.Imaging.ColorMatrix matrix = new System.Drawing.Imaging.ColorMatrix();
                matrix.Matrix33 = opacity; // 0.0f (invisible) to 1.0f (fully opaque)

                System.Drawing.Imaging.ImageAttributes attributes = new System.Drawing.Imaging.ImageAttributes();
                attributes.SetColorMatrix(matrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

                // Draw the original image onto the new bitmap using the transparency attributes
                g.DrawImage(image,
                    new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, image.Width, image.Height,
                    System.Drawing.GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }
        private System.Drawing.Image ResizeButtonIcon(System.Drawing.Image img, int width, int height)
        {
            // We specify System.Drawing to avoid conflict with ScottPlot
            System.Drawing.Bitmap b = new System.Drawing.Bitmap(width, height);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, width, height);
            }
            return b;
        }
        private void MenuGetStatistic_Click(object sender, EventArgs e)
        {
            if (SpendingList == null || !SpendingList.Any()) return;

            // 1. Get current visible limits from the plot
            var limits = fPlot.Plot.Axes.GetLimits();
            double xMin = limits.Left;
            double xMax = limits.Right;

            // 2. Filter records within the visible date range
            var visibleRecords = SpendingList
                .Where(r => r.Date.ToOADate() >= xMin && r.Date.ToOADate() <= xMax)
                .ToList();

            if (!visibleRecords.Any())
            {
                MessageBox.Show("No data points are currently visible.", "Statistics");
                return;
            }

            // 3. Perform Aggregations
            // We skip the very first record if its MonthlySpending is 0 (uncalculated) 
            // to keep the average accurate.
            var calcRecords = visibleRecords.Where(r => r.MonthlySpending != 0).ToList();

            double totalSpent = calcRecords.Sum(r => r.MonthlySpending);
            double totalAdded = visibleRecords.Sum(r => r.FundAdded);
            double totalWithdrawn = Math.Abs(visibleRecords.Sum(r => r.WithdrawVal));

            double avgMonthly = calcRecords.Any() ? totalSpent / calcRecords.Count : 0;

            // 4. Calculate Duration (Years and Months)
            int totalCount = visibleRecords.Count;
            int years = totalCount / 12;
            int remainingMonths = totalCount % 12;

            string durationStr = "";
            if (years > 0) durationStr += $"{years} year(s) ";
            if (remainingMonths > 0 || years == 0) durationStr += $"{remainingMonths} month(s)";

            // 5. Format the Output
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== ACCOUNT STATISTICS (VISIBLE) ===");
            sb.AppendLine($"Range: {visibleRecords.Min(r => r.Date):MMM yyyy} - {visibleRecords.Max(r => r.Date):MMM yyyy}");
            sb.AppendLine(new string('-', 40));
            sb.AppendLine($"Total Spending:    {totalSpent:C2}");
            sb.AppendLine($"Avg Monthly:       {avgMonthly:C2}");
            sb.AppendLine($"Total Funds Added: {totalAdded:C2}");
            sb.AppendLine($"Extra Withdrawals (Taxes): {totalWithdrawn:C2}");
            sb.AppendLine(new string('-', 40));
            sb.AppendLine($"Duration: {durationStr.Trim()}");
            sb.AppendLine($"Data Points: {totalCount} months");

            MessageBox.Show(sb.ToString(), "Financial Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    } // End of Class
} // End of Namespace
