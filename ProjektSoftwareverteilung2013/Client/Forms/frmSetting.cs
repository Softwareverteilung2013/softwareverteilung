using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            txtSavePath.Text = Properties.Settings.Default.SavePath;
                     txtUpdateInterval.Text = Properties.Settings.Default.RequestTime;

                     if (Properties.Settings.Default.ServerIP == "0")
                     {
                         txtServerIP.Text = "";
                     }
                     else
                     {
                         txtServerIP.Text = Properties.Settings.Default.ServerIP;
                     }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DateTime CheckTime = new DateTime();
            try
            {
                CheckTime = Convert.ToDateTime(txtUpdateInterval.Text);
            }
            catch (Exception)
            {
                txtUpdateInterval.BackColor = Color.Coral;
                MessageBox.Show("Die von Ihnen angegebene Zeichenreihenfolge kann nicht verwendet werden. Sie muss das Format: 08:00 haben.");
                txtUpdateInterval.BackColor = Color.White;
                txtUpdateInterval.Text = "";
                txtUpdateInterval.Focus();
                return;
            }

            Properties.Settings.Default.SavePath = txtSavePath.Text;
            Properties.Settings.Default.ServerIP = txtServerIP.Text;
            Properties.Settings.Default.RequestTime = txtUpdateInterval.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Ihre Änderungen wurden gespeichert.");
        }

        private void btnChangeSavePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog BrowserDialog = new FolderBrowserDialog();

            if (BrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSavePath.Text = BrowserDialog.SelectedPath;
            }
        }

    }
}
