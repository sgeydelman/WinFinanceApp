using SLGAutomationLib;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public struct SetingStructure
{
    public string Symbol { get; set; }
    public double TargetPercent { get; set; }
    public string Note { get; set; }

    // Add properties for other columns as needed
}
namespace WinFinanceApp
{
    public partial class FormSetup : Form
    {
        Logger _logger;
        Ini inif;
        protected CMyFinance MyFinance;
        double TotalPercent = 0;
        public FormSetup()
        {
            InitializeComponent();
        }
        private void FormSetup_Load(object sender, EventArgs e)
        {
          //  Logger.Create(Properties.Settings.Default.SetupPath);
            this._logger = Logger.Instance;
            inif = new Ini(_logger.SetupPath);
            this.MyFinance = CMyFinance.Instance;
            this.ReadAll();
            this.comboAccount.SelectedIndex = (int)CMyFinance.AccountType.SamIRA;
            
            // Initial layout adjustment
            AdjustLayout();
        }

        // IMPROVED: Added Resize handler for dynamic layout adjustment
        private void FormSetup_Resize(object sender, EventArgs e)
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
        private void BtnRead_Click(object sender, EventArgs e)
        {
            this.ReadAll();
        }

        private void BtnWrite_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Configuration data will be owerwritten!", "Are you sure?!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.WriteAll();
                this._logger.SentEvent(string.Format("User {0} changed SETUP file", _logger.User), Logger.EnumLogLevel.INFO_LEVEL);

            }
        }

        private void ReadAll()
        {
            try
            {
              //  inif = new Ini(_logger.SetupPath);
                string[] strs = this.inif.GetSectionNames();
                this.dataGrid.Rows.Clear();
                this.dataGrid.Refresh();
               
                foreach (string str in strs)
                {
                    this.dataGrid.Rows.Add(this.inif.GetString(str, "symbol", "?"), this.inif.GetDouble(str, "%", 0), this.inif.GetString(str, "Note", "?"));
                }
                List<SetingStructure> dataList = GetDataFromDataGridView(); // new List<SetingStructure>();
                lblTotal.Text = dataList.Sum(item => item.TargetPercent).ToString();
                
            }
            catch (Exception ex)
            {
                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.ERROR_LEVEL);
            }
        }
        private void WriteAll()
        {
            try
            {
                
                List<SetingStructure> dataList = GetDataFromDataGridView(); // new List<SetingStructure>();
                TotalPercent =  dataList.Sum(item => item.TargetPercent);
                if (TotalPercent != 100)
                    throw new Exception("Recording declined! Total Percent Value shold be  100%");
                int i = 0;
                foreach (SetingStructure data in dataList)
                {
                    this.inif.WriteValue(i.ToString(), "symbol", data.Symbol);
                    this.inif.WriteValue(i.ToString(), "%", data.TargetPercent);
                    if (data.Note == null)
                        this.inif.WriteValue(i.ToString(), "Note", "?");
                    else
                        this.inif.WriteValue(i.ToString(), "Note", data.Note);
                    i++;
                }

               

            }
            catch (Exception ex)
            {
                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.ERROR_LEVEL);
            }
        }

        private void dataGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGrid.CurrentCell.ColumnIndex == 1)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    // Remove any previous event handlers to avoid multiple attachments
                    textBox.KeyPress -= TextBox_KeyPress_DigitsOnly;
                    textBox.KeyPress += TextBox_KeyPress_DigitsOnly;
                }
            }
            else
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.KeyPress -= TextBox_KeyPress_DigitsOnly;
                }
            }

        }
        private void TextBox_KeyPress_DigitsOnly(object sender, KeyPressEventArgs e)
        {
            // Allow digits (0-9), backspace (8), and optionally the decimal point (46) if needed
            //if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 46)
            {
                e.Handled = true; // Prevent the character from being entered
                return;
            }
            // --- Logic for handling the decimal point ---
            // If the character is a decimal point (ASCII 46)
            // and the TextBox already contains a decimal point,
            // then prevent entering another one.
            if (e.KeyChar == 46)
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null && textBox.Text.Contains("."))
                {
                    e.Handled = true; // Prevent entering more than one decimal point
                }
            }
            // If you need to allow a single decimal point (for floating-point numbers)
            // uncomment the following condition:
            //if (e.KeyChar == 46 && (sender as TextBox).Text.Contains('.'))
            //{
            //    e.Handled = true; // Prevent entering more than one decimal point
            //}
        }
        public List<SetingStructure> GetDataFromDataGridView()
        {
            List<SetingStructure> dataList = new List<SetingStructure>();
            double targetStocks = 0, targetBonds = 0, targetCash = 0;
            // Iterate through each row in the DataGridView
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                // Skip the "new row" if it exists and is not filled
                if (row.IsNewRow)
                    continue;
               
                SetingStructure myStruct = new SetingStructure();

                // Access cell values by column index or column name
                if (dataGrid.Columns.Count > 0)
                {
                    // By Column Index (order dependent, less robust if column order changes)
                    if (row.Cells.Count > 0 && row.Cells[0].Value != null)
                    {
                        myStruct.Symbol = row.Cells[0].Value.ToString();
                    }
                    if (row.Cells.Count > 1 && row.Cells[1].Value != null)
                    {
                        if (double.TryParse(row.Cells[1].Value.ToString(), out double Value))
                        {
                            myStruct.TargetPercent = Value;
                        }
                        else
                        {
                            // Handle parsing error (e.g., log, set default value)
                            throw new Exception($"Error parsing integer from cell value: {row.Cells[1].Value}");
                        }
                    }
                    if (row.Cells.Count > 2 && row.Cells[2].Value != null)
                    {
                        myStruct.Note = row.Cells[2].Value.ToString();
                    }
                }
                dataList.Add(myStruct);
                if (myStruct.Note.ToLower().Contains("stock"))
                {
                    // Stocks
                    
                    targetStocks += myStruct.TargetPercent;
                }
                else if (myStruct.Note.ToLower().Contains("bond"))
                {
                    // Bonds
                   
                    targetBonds += myStruct.TargetPercent;
                }
                else if (myStruct.Note.ToLower().Contains("cash"))
                {
                    // Cash
                   
                    targetCash += myStruct.TargetPercent;
                }
            }
            txtStocksT.Text = targetStocks.ToString("F2") + " %";
           
            txtBondsT.Text = targetBonds.ToString("F2") + " %";
           
            txtCashT.Text = targetCash.ToString("F2") + " %";
           
            return dataList;
        }

        private void dataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                TotalPercent = 0;
                foreach (DataGridViewRow row in dataGrid.Rows)
                {

                    if (row.Cells.Count > 1 && row.Cells[1].Value != null)
                    {
                        if (double.TryParse(row.Cells[1].Value.ToString(), out double Value))
                        {
                            TotalPercent += Value;
                        }

                    }
                }
                lblTotal.Text = TotalPercent.ToString();
            }
            catch (Exception ex)
            {                
                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);
            }

        }
        
        private void comboAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.MyFinance.Account = comboAccount.SelectedIndex;
                switch (comboAccount.SelectedIndex)
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
                this.ReadAll();
            }
            catch (Exception ex)
            {
                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);
            }
        }
    }
}
