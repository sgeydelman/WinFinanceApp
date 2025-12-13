using SLGAutomationLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinFinanceApp
{
    public class CMyFinance
    {
        public Logger _logger;
        public bool isRothIra = false;
        public int Account;
        private static CMyFinance _Instance;
        public enum AccountType
        {
            LenaIRA,
            LenaRothIRA,
            SamIRA,
            SamRothIRA
        }
        public CMyFinance()
        {
            this._logger = Logger.Instance;
            Account = (int)AccountType.SamIRA;
        }

        public static CMyFinance Instance
        {
            get { 
            if (_Instance == null)
                    _Instance = new CMyFinance();
                return _Instance;
            }
        }
    }

    public class MonthlyReturn
    {
        public DateTime Date { get; set; }
        public double? Return { get; set; }

        public MonthlyReturn(DateTime date, double? returnValue)
        {
            Date = date;
            Return = returnValue;
        }
    }

    public class AccountReturns
    {
        public string AccountName { get; set; }
        public List<MonthlyReturn> MonthlyReturns { get; set; }

        public AccountReturns(string accountName)
        {
            AccountName = accountName;
            MonthlyReturns = new List<MonthlyReturn>();
        }

        // Calculate TWR for a period starting from a specific date
        public double? CalculateTWR(DateTime startDate, int numberOfMonths)
        {
            var relevantReturns = MonthlyReturns
             .Where(r => r.Date <= startDate && r.Return.HasValue)
             .OrderByDescending(r => r.Date)
             .Take(numberOfMonths)
             .OrderBy(r => r.Date)  // Re-order chronologically for calculation
             .ToList();

            if (relevantReturns.Count < numberOfMonths)
                return null; // Not enough data

            // TWR calculation: (1 + r1) * (1 + r2) * ... * (1 + rn) - 1
            double twr = 1.0;
            foreach (var ret in relevantReturns)
            {
                twr *= (1.0 + ret.Return.Value);
            }

            return twr - 1.0;
        }
    }
    public class TWRCalculationResult
    {
        public string  StartMonth { get; set; }
        public int NumberOfMonths { get; set; }
        public Dictionary<string, double?> AccountTWRs { get; set; }

        public TWRCalculationResult()
        {
            AccountTWRs = new Dictionary<string, double?>();
        }

        // Get all TWRs as percentage values (multiplied by 100)
        public Dictionary<string, double?> GetTWRPercentages()
        {
            var percentages = new Dictionary<string, double?>();
            foreach (var account in AccountTWRs)
            {
                percentages[account.Key] = account.Value.HasValue ? account.Value.Value * 100 : (double?)null;
            }

            return percentages;
            
        }
    }

    public class FidelityReturnsParser
    {
        public  Dictionary<string, AccountReturns> accounts;
        private List<DateTime> allDates; // All available dates from the CSV

        public FidelityReturnsParser()
        {
            accounts = new Dictionary<string, AccountReturns>();
            allDates = new List<DateTime>();
        }

        // Convert Excel date serial number to DateTime
        private DateTime ExcelSerialToDate(int serialNumber)
        {
            DateTime excelEpoch = new DateTime(1899, 12, 30);
            return excelEpoch.AddDays(serialNumber);
        }

        public void ParseTWRCSV( string[] lines)
        {
            //var lines = File.ReadAllLines(filePath);

            // Find the header row
            int headerRowIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Time-weighted rate of return (pre-tax)"))
                {
                    headerRowIndex = i;
                    break;
                }
            }

            if (headerRowIndex == -1)
                throw new Exception("Could not find header row");

            // Parse date headers
            var dateHeaders = lines[headerRowIndex].Split(',');

            for (int i = 1; i < dateHeaders.Length; i++)
            {
                string header = dateHeaders[i].Trim();
                if (string.IsNullOrEmpty(header))
                    continue;

                // Try parsing as Excel serial number first
                if (int.TryParse(header, out int serialDate))
                {
                    allDates.Add(ExcelSerialToDate(serialDate));
                }
                // Try parsing as "MMM-yy" format (e.g., "Oct-25")
                else if (DateTime.TryParseExact(header, "MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    allDates.Add(parsedDate);
                }
            }

            // Parse account data
            for (int i = headerRowIndex + 1; i < lines.Length; i++)
            {
                var columns = lines[i].Split(',');
                if (columns.Length < 2)
                    continue;

                string accountName = columns[0].Trim();
                // Stop parsing when we hit empty lines or disclaimer text
                if (string.IsNullOrEmpty(accountName))
                    break;
              //  if (accountName.Contains("Total"))
              //      continue;
                // Skip lines that start with quotes (disclaimer text)
                if (accountName.StartsWith("\""))
                    break;
              //  if (lines[i].Contains("Total"))
              //      continue;
                var accountReturns = new AccountReturns(accountName);

                for (int j = 1; j < columns.Length && j <= allDates.Count; j++)
                {
                    string returnValue = columns[j].Trim();
                    double? parsedReturn = null;

                    if (!string.IsNullOrEmpty(returnValue) && returnValue != "--")
                    {
                        // Remove % sign if present and parse
                        returnValue = returnValue.Replace("%", "").Trim();
                        if (double.TryParse(returnValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                        {
                            // If the value looks like a percentage (e.g., 2.52 meaning 2.52%), convert to decimal
                            if (returnValue.Contains(".") || Math.Abs(value) > 1)
                            {
                                parsedReturn = value / 100.0; // Convert percentage to decimal
                            }
                            else
                            {
                                parsedReturn = value;
                            }
                        }
                    }

                    accountReturns.MonthlyReturns.Add(new MonthlyReturn(allDates[j - 1], parsedReturn));
                }

                accounts[accountName] = accountReturns;
            }
        }

        // Get list of available months for user selection
        public List<string> GetAvailableMonths()
        {
            return allDates
           .Select(d => d.ToString("yyyy-MMM", CultureInfo.InvariantCulture))
           .Distinct()
           .OrderByDescending(m => DateTime.ParseExact(m, "yyyy-MMM", CultureInfo.InvariantCulture))
           .ToList();
        }

        // Calculate TWR for all accounts based on user selection
        public TWRCalculationResult CalculateTWR(string selectedMonth, int numberOfMonths)
        {
            var result = new TWRCalculationResult
            {
                StartMonth = selectedMonth,
                NumberOfMonths = numberOfMonths
            };

            // Parse the selected month back to DateTime
            DateTime startDate = DateTime.ParseExact(selectedMonth, "yyyy-MMM", CultureInfo.InvariantCulture);

            foreach (var account in accounts)
            {
                result.AccountTWRs[account.Key] = account.Value.CalculateTWR(startDate, numberOfMonths);
            }

            return result;
        }

        public List<string> GetAccountNames()
        {
            return accounts.Keys.ToList();
        }
    }
}
