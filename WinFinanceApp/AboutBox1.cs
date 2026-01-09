using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFinanceApp
{
    partial class AboutBox1 : Form
    {
        public AboutBox1()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            
            // Build the description with application purpose
            string appDescription = BuildApplicationDescription();
            this.textBoxDescription.Text = appDescription;
        }

        /// <summary>
        /// Builds the complete application description including purpose and usage information
        /// </summary>
        private string BuildApplicationDescription()
        {
            StringBuilder description = new StringBuilder();
            
            // Add assembly description if available
            string assemblyDesc = AssemblyDescription;
            if (!string.IsNullOrEmpty(assemblyDesc))
            {
                description.AppendLine(assemblyDesc);
                description.AppendLine();
            }
            
            // Add application purpose and usage information
            description.AppendLine("Application Purpose:");
            description.AppendLine();
            description.AppendLine("This application processes and analyzes investment data by parsing files from Fidelity investment brokerage accounts.");
            description.AppendLine();
            description.AppendLine("Key Features:");
            description.AppendLine("• Portfolio monitoring and analysis");
            description.AppendLine("• Account rebalancing calculations");
            description.AppendLine("• Historical return analysis (Time-Weighted Return)");
            description.AppendLine("• Annualized performance tracking");
            description.AppendLine("• Account Spending analyzing ");
            description.AppendLine();
            description.AppendLine("Data Import Requirements:");
            description.AppendLine("Files (except account spending) must be downloaded from the Fidelity website and converted to CSV format before loading into the application.");
            description.AppendLine();
            description.AppendLine("Supported File Types:");
            description.AppendLine("• Portfolio Positions CSV (for account monitoring and rebalancing)");
            description.AppendLine("• Periodic Returns CSV (for monthly time-weighted returns)");
            description.AppendLine("• Annualized Returns CSV (for period-based performance analysis)");
            description.AppendLine("• Monthly Account Spending CSV (for monthly account spending analysis)");

            return description.ToString();
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
