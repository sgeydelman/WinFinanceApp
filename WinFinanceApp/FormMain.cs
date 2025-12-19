using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Reflection.Emit;
using SLGAutomationLib;

namespace WinFinanceApp
{
    public partial class FormMain : Form
    {
        Form formIRA1, formSetup, formMonitor, formReturn;
        protected Logger _logger;
        Ini iniF;
        protected CMyFinance MyFinance;
        private static readonly object AssemblyCopyright;
        private Form currentForm; // Track the currently displayed form

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Visible = true;
            this.MyFinance = CMyFinance.Instance;
            this.PANPAN.Controls.Clear();
            this._logger = Logger.Instance;
            this._logger.ev_Send_Event += new Logger.dq_Send_Event(this.addItemLV);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;

            this.listView.Items.Clear();
            this.listView.View = View.Details;
            this.listView.Columns.Add("");
            this.listView.Columns[0].Width = this.listView.Width * 2;
            this.listView.HeaderStyle = ColumnHeaderStyle.None;
            string assemblyCopyright = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
            this.Text = String.Format("WinFinance    Version: {0}           {1} ", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(), assemblyCopyright);
            this.SetClickables();
            this._logger.SentEvent(string.Format("WinFinance Version: {0} {1}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, assemblyCopyright), Logger.EnumLogLevel.INFO_LEVEL);

            if (Properties.Settings.Default.FirstRun)
            {
                // Perform first-run actions 
                // ...
                Location = new Point(100, 100); // Set your desired initial location
                Size = new Size(1600, 900); // Set your desired initial size
                // Set FirstRun to false
                Properties.Settings.Default.FirstRun = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                // Restore saved window size and location
                if (Properties.Settings.Default.Maximized)
                {
                    Location = Properties.Settings.Default.WindowLocation;
                    WindowState = FormWindowState.Maximized;
                    Size = Properties.Settings.Default.WindowSize;
                }
                else if (Properties.Settings.Default.Minimized)
                {
                    Location = Properties.Settings.Default.WindowLocation;
                    WindowState = FormWindowState.Minimized;
                    Size = Properties.Settings.Default.WindowSize;
                }
                else
                {
                    Location = Properties.Settings.Default.WindowLocation;
                    Size = Properties.Settings.Default.WindowSize;
                }
            }

            // Subscribe to panel resize event
            this.PANPAN.Resize += PANPAN_Resize;

            //this.ShowForm(formIRA1);
            this.ShowForm(formMonitor);
        }

        private void PANPAN_Resize(object sender, EventArgs e)
        {
            // When the panel resizes, adjust the child form size
            if (currentForm != null && PANPAN.Controls.Contains(currentForm))
            {
                currentForm.Size = PANPAN.ClientSize;
                currentForm.PerformLayout();
            }
        }

        private void addItemLV(string str, Logger.EnumLogLevel level)
        {
            try
            {
                Color color = Color.Red;
                switch (level)
                {
                    case Logger.EnumLogLevel.INFO_LEVEL:
                        color = Color.White;
                        break;
                    case Logger.EnumLogLevel.WARNING_LEVEL:
                        color = Color.Yellow;
                        break;
                    case Logger.EnumLogLevel.EXCEPTION_LEVEL:
                        color = Color.Red;
                        break;
                    case Logger.EnumLogLevel.ERROR_LEVEL:
                        color = Color.Red;
                        break;
                }

                DateTime dt = DateTime.Now;
                ListViewItem lvitem = new ListViewItem();
                string time = dt.ToString("dd MMM HH:mm:ss") + " ";
                lvitem.Text = time + str;
                lvitem.BackColor = color;
                // listView.Items.Insert(0, lvitem);
                //  this.Invoke((MethodInvoker)delegate { listView.Items.Insert(0, lvitem); });
                this.Invoke((MethodInvoker)delegate
                {
                    listView.Items.Insert(listView.Items.Count, lvitem);
                    listView.Items[listView.Items.Count - 1].EnsureVisible();
                });
                if (listView.Items.Count > 100)
                    //   listView.Items[listView.Items.Count - 1].Remove();
                    listView.Items[0].Remove();
            }
            catch { }
        }

        private void BtnAlarm_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.WindowLocation = RestoreBounds.Location;
                Properties.Settings.Default.WindowSize = RestoreBounds.Size;
                Properties.Settings.Default.Maximized = true;
                Properties.Settings.Default.Minimized = false;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.WindowLocation = Location;
                Properties.Settings.Default.WindowSize = Size;
                Properties.Settings.Default.Maximized = false;
                Properties.Settings.Default.Minimized = false;
            }
            else
            {
                Properties.Settings.Default.WindowLocation = RestoreBounds.Location;
                Properties.Settings.Default.WindowSize = RestoreBounds.Size;
                Properties.Settings.Default.Maximized = false;
                Properties.Settings.Default.Minimized = true;
            }
            Properties.Settings.Default.Save();
        }

        private void SetClickables()
        {
            formIRA1 = new FormIRA1();
            formSetup = new FormSetup();
            formMonitor = new FormAcctMonitor();
            formReturn = new FormReturn();
        }

        private void BtnIRA_S_Click(object sender, EventArgs e)
        {
            this.ShowForm(formIRA1);
        }

        private void BtnSetup_Click(object sender, EventArgs e)
        {
            this.ShowForm(formSetup);
        }

        private void timer_ui_Tick(object sender, EventArgs e)
        {
            try
            {
                string str = "This Setup is configured for ";
                string s = string.Empty;
                switch (this.MyFinance.Account)
                {
                    case (int)CMyFinance.AccountType.LenaIRA:
                        s = "Lena's IRA account";
                        break;
                    case (int)CMyFinance.AccountType.LenaRothIRA:
                        s = "Lena's ROTH IRA account";
                        break;
                    case (int)CMyFinance.AccountType.SamIRA:
                        s = "Sam's IRA account";
                        break;
                    case (int)CMyFinance.AccountType.SamRothIRA:
                        s = "Sam's ROTH IRA account";
                        break;
                    case (int)CMyFinance.AccountType.Other:
                        s = "Other account";
                        break;
                }
                this.lblRothIRA.Text = str + s;

            }
            catch { }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox1 box = new AboutBox1())
            {
                box.ShowDialog(this);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (AboutBox1 box = new AboutBox1())
            {
                box.ShowDialog(this);
            }
        }

        private void oPMMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowForm(formMonitor);
        }

        private void sETUPToolStripMenuItem_Click(object sender, EventArgs e) //Rebalance
        {
            this.ShowForm(formIRA1);
        }

        private void historicalRetrunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowForm(formReturn);
        }

        private void sETUPToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.ShowForm(formSetup);
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            this.ShowForm(formReturn);
        }

        private void BtnMonitor_Click(object sender, EventArgs e)
        {
            this.ShowForm(formMonitor);
        }

        private void ShowForm(Form form)
        {
            try
            {
                this.Visible = true;
                this.PANPAN.Controls.Clear();

                // Prepare the form for embedding
                form.FormBorderStyle = FormBorderStyle.None;
                form.TopLevel = false;
                form.Location = new Point(0, 0);
                form.Size = PANPAN.ClientSize;
                form.Dock = DockStyle.Fill;
                
                // Set AutoScaleMode to match parent for consistent scaling
                form.AutoScaleMode = AutoScaleMode.Dpi;
                
                // Add form to panel
                this.PANPAN.Controls.Add(form);
                
                // Store reference to current form
                currentForm = form;
                
                // Show and force layout
                form.Show();
                form.PerformLayout();
                
                // Force immediate size adjustment
                form.Size = PANPAN.ClientSize;
            }
            catch (Exception ex)
            {
                this._logger.SentEvent(ex.ToString(), Logger.EnumLogLevel.EXCEPTION_LEVEL);
            }
        }
    }
}
