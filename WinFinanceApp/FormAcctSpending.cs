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
        public FormAcctSpending()
        {
            InitializeComponent();
            this._logger = Logger.Instance;
            inif = new Ini(_logger.SetupPath);
            this.MyFinance = CMyFinance.Instance;

        }

        private void FormAcctSpending_Load(object sender, EventArgs e)
        {
            this._logger = Logger.Instance;
            inif = new Ini(_logger.SetupPath);
            this.MyFinance = CMyFinance.Instance;
            //this.LoadSpendingCSV();
            // Initial layout adjustment
            AdjustLayout();

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
                    var columns = line.Split(',');
                    string[] formats = { "MM/dd/yyyy", "M/d/yyyy", "MM/d/yyyy", "M/dd/yyyy" };
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
                for(int i = 1; i < SpendingList.Count; i++)
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
                                            .Select(r => (double)r.MonthlySpending)
                                            .DefaultIfEmpty(0).Average();

                double avgPrev = SpendingList.Where(r => r.Date.Year == prevYear)
                                             .Select(r => (double)r.MonthlySpending)
                                             .DefaultIfEmpty(0).Average();

                double avgPrevPrev = SpendingList.Where(r => r.Date.Year == prevPrevYear)
                                                 .Select(r => (double)r.MonthlySpending)
                                                 .DefaultIfEmpty(0).Average();

                // --- 2. PREPARE PLOT ---
                fPlot.Plot.Clear();

                double[] dates = SpendingList.Select(r => r.Date.ToOADate()).ToArray();
                double[] monthlySpending = SpendingList.Select(r => (double)r.MonthlySpending).ToArray();

                // Add Scatter
                scatter = fPlot.Plot.Add.Scatter(dates, monthlySpending);
                scatter.LineWidth = 3;
                scatter.Color = ScottPlot.Colors.Blue;
                scatter.MarkerSize = 12; // Adjusted dot size
                scatter.MarkerShape = ScottPlot.MarkerShape.FilledCircle;
                scatter.MarkerFillColor = ScottPlot.Colors.Red;
                scatter.MarkerLineColor = ScottPlot.Colors.Blue;
                scatter.MarkerLineWidth = 2;
                scatter.LegendText = "Monthly Spending";

                // --- 3. ADD DYNAMIC HORIZONTAL LINES ---
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

                // Performance & Aesthetics
                fPlot.Plot.FigureBackground.Color = ScottPlot.Colors.White;
                fPlot.Plot.Axes.ContinuouslyAutoscale = false;

                var yGen = new ScottPlot.TickGenerators.NumericAutomatic();
                yGen.LabelFormatter = (double val) => val.ToString("C0");
                fPlot.Plot.Axes.Left.TickGenerator = yGen;

                // --- 5. INTERACTION & FONT STYLING ---
                MyCrosshair = fPlot.Plot.Add.Crosshair(0, 0);
                MyCrosshair.LineColor = ScottPlot.Colors.Red;

                // NEW: Styled Hover Text for High Visibility
                MyHoverText = fPlot.Plot.Add.Text("", dates[0], monthlySpending[0]);
                MyHoverText.LabelFontSize = 14;              // Larger text
                MyHoverText.LabelBold = true;                // Bold for visibility
                MyHoverText.LabelFontName = "Verdana";       // Clear font
                MyHoverText.LabelFontColor = ScottPlot.Colors.Black;
                MyHoverText.LabelBackgroundColor = ScottPlot.Colors.Yellow; // High contrast
                MyHoverText.LabelBorderColor = ScottPlot.Colors.Black;
                MyHoverText.LabelBorderWidth = 2;
                MyHoverText.LabelPadding = 8;

                // Offset it so it doesn't cover the dot
                MyHoverText.LabelAlignment = ScottPlot.Alignment.LowerCenter;

                MyHoverText.IsVisible = false;

                // --- 6. FINALIZE ---
                fPlot.Plot.Axes.AutoScale();

                // Increase top margin to 15% so the large Hover Box doesn't get cut off
                var limits = fPlot.Plot.Axes.GetLimits();
                fPlot.Plot.Axes.SetLimitsY(0, limits.Top * 1.15);

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
            if (MyCrosshair == null || scatter == null || MyHoverText == null) return;

            // 1. Get coordinates from the last successful render
            ScottPlot.Pixel mousePixel = new ScottPlot.Pixel(e.X, e.Y);
            ScottPlot.Coordinates mouseCoords = fPlot.Plot.GetCoordinates(mousePixel);

            // 2. Find the nearest point
            var nearest = scatter.Data.GetNearest(mouseCoords, fPlot.Plot.LastRender);

            if (nearest.IsReal)
            {
                // 3. ONLY proceed if the mouse has moved to a DIFFERENT data point
                if (nearest.Coordinates.X != LastSnappedCoord.X)
                {
                    LastSnappedCoord = nearest.Coordinates;

                    // Update positions without refreshing yet
                    MyCrosshair.Position = nearest.Coordinates;
                    MyHoverText.Location = nearest.Coordinates;

                    DateTime pointDate = DateTime.FromOADate(nearest.Coordinates.X);
                    MyHoverText.LabelText = $"{pointDate:MMM, yyyy}{Environment.NewLine}{nearest.Coordinates.Y:C2}";
                    MyHoverText.IsVisible = true;

                    // 4. USE INVALIDATE INSTEAD OF REFRESH
                    // Refresh() forces an instant redraw (slow). 
                    // Invalidate() tells Windows to redraw when it has a free millisecond (smooth).
                    fPlot.Invalidate();
                }
            }
            else if (MyHoverText.IsVisible)
            {
                MyHoverText.IsVisible = false;
                fPlot.Invalidate();
            }
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
    }
}
