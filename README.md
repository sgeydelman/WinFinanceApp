# WinFinanceApp# WinFinanceApp

A Windows Forms application for analyzing and monitoring Fidelity investment brokerage accounts. This tool helps you track portfolio performance, manage account rebalancing, and analyze spending patterns.

## Overview

WinFinanceApp processes and analyzes investment data by importing files from your Fidelity investment accounts. It provides comprehensive portfolio monitoring, performance analysis, and account management capabilities.

## Features

- **Portfolio Monitoring & Analysis** - Track your investment positions and asset allocation
- **Account Rebalancing Calculations** - Determine needed adjustments to maintain target allocations using a relative devition method.
- **Historical Return Analysis** - Calculate and visualize Time-Weighted Returns (TWR) for accurate performance measurement
- **Annualized Performance Tracking** - View period-based performance metrics with comparative charting
- **Account Spending Analysis** - Monitor and analyze your monthly account spending patterns
- **Interactive Charts** - Visual analysis using ScottPlot for TWR and return data visualization

## Requirements

- **Windows OS** (Windows 11 or later)
- **.NET Framework** (4.8)
- **Visual Studio** (for development/building from source)

## Getting Started

### Installation

1. Download the latest release from the Releases section
2. Extract the files to your desired location
3. Run `WinFinanceApp.exe`

### Building from Source

1. Clone the repository
2. Open `WinFinanceApp.sln` in Visual Studio
3. Build the solution (Ctrl+Shift+B)
4. Run the application (F5)

## Usage

### Data Import

### Requirements

Files are downloaded directly from **Fidelity Active Trader Pro+** as CSV format. No conversion is needed.

**To download CSV files from Fidelity Active Trader Pro+:**
1. Log in to your Fidelity account via Active Trader Pro+
2. Navigate to the appropriate section (Positions, Returns, or Spending)
3. Select "Export" or "Download" option
4. Choose "CSV" format
5. Save the file to your computer
6. Import into WinFinanceApp

## Core Components

### FormReturn (Historical & Annualized Returns Analysis)

The FormReturn component provides comprehensive return analysis with visual charting capabilities:

**Time-Weighted Return (TWR) Analysis:**
- Load monthly return history files from Fidelity
- Select starting month and number of months to analyze
- Calculate and visualize TWR trends over time using ScottPlot
- Supports both regular and annualized return calculations

**Annualized Return Analysis:**
- Import annualized returns data from Fidelity
- Compare performance across multiple periods (1 Month, 3 Month, YTD, 1 Year, Life, etc.)
- Analyze returns by account with color-coded bar charts
- Track account life start dates and performance history

**Charting:**
- Uses **ScottPlot 5.1.57** for interactive data visualization
- Real-time plot updates with account performance bars
- Responsive charts that scale with window resizing
- Clear visual representation of return trends and comparisons

**Usage:**
1. Click "Load Fidelity return history File" to import CSV
2. Application auto-detects file format (Monthly or Annualized)
3. Select your analysis parameters:
   - Start month (or select Annualized return checkbox)
   - Number of months to analyze
4. Click "Get TWR" to generate chart
5. Analyze trends and performance metrics across accounts

### Account Monitor (FormAcctMonitor)

Real-time portfolio monitoring and analysis:
- Monitor current positions and allocation percentages
- View position symbols, quantities, and values
- Track investment strategy targets and rebalancing thresholds

### Account Rebalance (FormAcctMonitor)

Portfolio rebalancing analysis:
- Calculate required adjustments to match target allocations
- Define rebalancing thresholds
- Visualize target vs. current allocations

### SETUP Configuration (INI Technology)

The SETUP form provides comprehensive configuration management using INI file technology:

**Investment Strategy Configuration:**
- Define target allocation percentages for different asset classes (Stocks, Bonds, Cash)
- Set rebalancing thresholds (e.g., 15% threshold for rebalancing triggers)
- Configure account-specific investment profiles
- Select which account to monitor (e.g., "Sam IRA")
- Store all settings in INI configuration files

**Features:**
- User-friendly interface for setting investment parameters
- Persistent storage using INI files
- Support for multiple account profiles
- Strategy templates for different investment approaches

**Configuration Stored:**
- Target allocation percentages
- Account selection and profiles
- Rebalancing thresholds
- Investment strategy rules

### Workflow Example

1. **Setup Phase** (SETUP button):
   - Define your investment strategy and target allocations
   - Set rebalancing thresholds (e.g., 15%)
   - Select which account to monitor
   - Save configuration to INI files

2. **Monitor Phase** (Account Monitor):
   - Import your Portfolio Positions file from Fidelity
   - Review current holdings and allocation percentages
   - Compare against target allocation strategy

3. **Rebalance Phase** (Account Rebalance):
   - Review rebalancing recommendations
   - Identify positions that exceed thresholds
   - Plan necessary adjustments

4. **Analyze Phase** (Analyze Return):
   - Import Periodic Returns CSV for monthly TWR analysis
   - Import Annualized Returns for performance comparison
   - Visualize performance trends with interactive ScottPlot charts

5. **Spending Phase** (Analyze Spending):
   - Track monthly account spending patterns
   - Monitor cash flow and withdrawals

## Project Structure

```
WinFinanceApp/
├── WinFinanceApp.sln              # Solution file
├── README.md                      # This file
├── packages.config                # NuGet packages
└── WinFinanceApp/                 # Main application project
    ├── Properties/
    │   ├── AssemblyInfo.cs
    │   ├── Resources.resx
    │   └── Settings.settings
    ├── Forms/
    │   ├── Form1.cs              # Main application form
    │   ├── FormReturn.cs         # Historical and annualized returns analysis
    │   ├── FormIRA1.cs           # IRA account analysis
    │   ├── FormAcctMonitor.cs    # Account monitoring
    │   ├── FormAcctSpending.cs   # Account spending analysis
    │   ├── FormMainc.cs          # Main control form
    │   ├── FormMisc.cs           # Miscellaneous utilities
    │   ├── AboutBox1.cs          # About dialog
    │   └── OpenTK.dll.config     # OpenTK configuration
    ├── Core/
    │   ├── Program.cs            # Application entry point
    │   ├── App.config            # Application configuration
    │   ├── financial-profit.ico  # Application icon
    │   ├── CMyFinance.cs         # Finance business logic
    │   └── packages.config       # NuGet package configuration
    ├── Resources/
    │   ├── Resources.resx        # Resource file
    │   ├── money.png             # Resource images
    │   ├── trend.png
    │   └── [other resources]
    └── Configuration/
        └── [INI configuration files]
```

## Configuration

Assembly metadata is configured in `Properties/AssemblyInfo.cs`:

```csharp
[assembly: AssemblyTitle("WinFinanceApp")]
[assembly: AssemblyProduct("WinFinanceApp")]
[assembly: AssemblyVersion("1.0.1.12")]
[assembly: AssemblyCopyright("Copyright © Sam Geydelman, 2025")]
```

Update these values to match your project information.

## Data File Format

All CSV files should include appropriate headers. The application expects standard Fidelity export formats with the following general structure:

- **Portfolio Positions**: Symbol, Quantity, Price, Value, etc.
- **Periodic Returns**: Date, Return %, TWR, etc.
- **Annualized Returns**: Period, Return %, etc.
- **Monthly Spending**: Month, Category, Amount, etc.

## Troubleshooting

### File Import Issues
- Ensure files are in CSV format (not Excel .xlsx)
- Verify file headers match expected format
- Check that data contains no special characters or encoding issues

### Performance Analysis Problems
- Verify date ranges in your data files
- Ensure transaction data is complete
- Check for any data gaps or missing values

## Development

### Technologies Used
- **Language**: C# (.NET Framework)
- **UI Framework**: Windows Forms
- **Charting Library**: ScottPlot 5.1.57 (for interactive data visualization)
- **IDE**: Visual Studio (latest)

### NuGet Dependencies
- **ScottPlot** (5.1.57) - Interactive plotting library for TWR and return analysis charts

### Building & Testing
- Build: Open solution in Visual Studio and press Ctrl+Shift+B
- Run: Press F5 or click Start
- Test: Manually verify with sample Fidelity export files

## Contributing

Contributions are welcome! Please feel free to:
- Report bugs
- Suggest new features
- Submit pull requests

## Future Enhancements

Potential features for future versions:
- Support for additional brokerage platforms
- Export analysis results to reports/Excel
- Advanced charting and visualization
- Automated data import scheduling
- Support for multiple account aggregation

## License

Copyright © Sam Geydelman, 2025

## Support

For issues, questions, or suggestions, please:
1. Check existing documentation
2. Review the About dialog (Help → About) for application version and details
3. Open an issue in the repository

## Disclaimer

This application is provided as-is for personal investment analysis. Always verify calculations independently and consult with a financial advisor before making investment decisions. The developers assume no responsibility for any investment decisions made based on this application's analysis.

## Changelog

### Version 1.0.0
- Initial release
- Portfolio monitoring
- Return analysis (TWR)
- Rebalancing calculations
- Spending analysis

---

**Last Updated**: February 2026


