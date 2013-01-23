using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Server_Client.Forms
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
            txtSavePath.Text = Properties.Settings.Default.SavePath;
            txtServerIP.Text = Properties.Settings.Default.ServerIP;
        }

        private void btnChangeSavePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog BrowserDialog = new FolderBrowserDialog();

            if (BrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSavePath.Text = BrowserDialog.SelectedPath;
             }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SavePath = txtSavePath.Text;
            Properties.Settings.Default.ServerIP = txtServerIP.Text;
            Properties.Settings.Default.Save();
        }

    }
}
