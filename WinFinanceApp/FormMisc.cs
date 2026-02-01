using SLGAutomationLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WinFinanceApp
{
    public partial class FormMisc : Form
    {
        protected Logger _logger;
        Ini inif;
        protected CMyFinance MyFinance;

        public FormMisc()
        {
            InitializeComponent();
        }

        private void FormMisc_Load(object sender, EventArgs e)
        {
            this._logger = Logger.Instance;
            inif = new Ini(_logger.SetupPath);
            this.MyFinance = CMyFinance.Instance;

            // Set the first column (Account Name) to Read-Only
            if (dataGrid.Columns.Count > 0)
                dataGrid.Columns[0].ReadOnly = true;

            // Preload rows with default "--" values as you suggested
            this.dataGrid.Rows.Add("WF Joint", "--", 0, 0, 0, 0, 0, 0, "--", 0, "Jan-15-2019");
            this.dataGrid.Rows.Add("WF IRA", "--", 0, 0, 0, "--", "--", "--", "--", 0, "Apr-24-2025");
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            this.openFileD.Filter = "CSV files (*.csv)|*.csv";
            this.openFileD.Title = "Process Fidelity Annualized Returns";

            if (this.openFileD.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] lines = File.ReadAllLines(this.openFileD.FileName);
                    List<string> outputLines = new List<string>();
                    bool inTwrSection = false;
                    bool extraRowsAdded = false;

                    for (int i = 0; i < lines.Length; i++)
                    {
                        // Strip quotes to handle the new Fidelity format (Jan_26 style)
                        string line = lines[i].Replace("\"", "").Trim();

                        if (line.Contains("Time-weighted rate of return"))
                        {
                            inTwrSection = true;
                            var headers = line.Split(',').Select(h => h.Trim()).ToList();
                            //if headers count > 12 throw an exception
                            if (headers.Count > 12)
                                throw new Exception("The file is in correct format for Periodical return");
                            //find if "6 Month" exists
                            if (headers.Any(h => h.Equals("6 Month", StringComparison.OrdinalIgnoreCase)))
                                throw new Exception("The file already contains '6 Month' column.");
                            // Insert "6 Month" column between "3 Month" and "YTD"
                            int ytdIndex = headers.FindIndex(h => h.Equals("YTD", StringComparison.OrdinalIgnoreCase));
                            if (ytdIndex != -1) headers.Insert(ytdIndex, "6 Month");
                            else headers.Insert(3, "6 Month");

                            outputLines.Add(string.Join(",", headers));
                            continue;
                        }

                        if (inTwrSection)
                        {
                            // Detect "Total" to stop and inject DataGrid rows
                            if (line.StartsWith("Total", StringComparison.OrdinalIgnoreCase) ||
                                line.Contains("Money-weighted rate of return"))
                            {
                                if (!extraRowsAdded)
                                {
                                    AddDataGridRowsToOutput(outputLines);
                                    extraRowsAdded = true;
                                }
                                break; // This skips the Total row and deletes everything below it
                            }

                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                var cells = line.Split(',').ToList();
                                // Add placeholder for the new 6 Month column in existing Fidelity accounts
                                if (cells.Count > 3)
                                {
                                    cells.Insert(3, "--");
                                    outputLines.Add(string.Join(",", cells));
                                }
                            }
                        }
                        else
                        {
                            // Keep report headers (dates, etc) without quotes
                            if (!string.IsNullOrWhiteSpace(line))
                                outputLines.Add(line);
                        }
                    }

                    SaveFileDialog saveD = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv", FileName = "Fidelity_Modified.csv" };
                    if (saveD.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllLines(saveD.FileName, outputLines);
                        MessageBox.Show("Success! The file is now ready for FormReturn.", "Process Complete");
                    }
                }
                catch (Exception ex)
                {
                   // MessageBox.Show("Error: " + ex.Message);
                    this._logger.SentEvent(ex.Message, Logger.EnumLogLevel.EXCEPTION_LEVEL);
                }
            }
        }

        private void AddDataGridRowsToOutput(List<string> outputList)
        {
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (row.IsNewRow) continue;

                List<string> rowCells = new List<string>();
                for (int i = 0; i < dataGrid.ColumnCount; i++)
                {
                    string val = row.Cells[i].Value?.ToString()?.Trim() ?? "--";

                    // For Percentage Columns (Indices 1 through 9)
                    if (i >= 1 && i <= 9)
                    {
                        // Check if it's a valid number and NOT zero
                        if (double.TryParse(val, out double num) && Math.Abs(num) > 0.0001)
                        {
                            // Format with +/- sign and % suffix
                            val = (num > 0 ? "+" : "") + num.ToString("F2") + "%";
                        }
                        else
                        {
                            // If it's 0, 0.00, empty, or already "--", output "--"
                            val = "--";
                        }
                    }
                    rowCells.Add(val);
                }
                outputList.Add(string.Join(",", rowCells));
            }
        }
    }
}