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

            this.dataGrid.Rows.Add("WF Joint", 0, 0, 0, 0, 0 ,0, 0, 0, 0,"Jan-15-2019");
            this.dataGrid.Rows.Add("WF IRA",   0, 0, 0, 0, 0, 0, 0, 0, 0, "Apr-24-2025");

        }

        private void BtnLoad_Click(object sender, EventArgs e)
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
            //periodical return
            if (this.openFileD.ShowDialog() == DialogResult.OK)
            {
                string filepath = this.openFileD.FileName;
                try
                {
                    var lines = File.ReadAllLines(filepath);
                 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading file: " + ex.Message);
                }
            }

        }
    }
}
