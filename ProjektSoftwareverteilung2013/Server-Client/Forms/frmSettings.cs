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

            if (   Properties.Settings.Default.ServerIP == "0")
            {
                  txtServerIP.Text = "";
            }
            else
            {
                txtServerIP.Text = Properties.Settings.Default.ServerIP;
            }
     
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
            Properties.Settings.Default.SavePath = txtSavePath.Text + "\\Softwareverteilung";
            Properties.Settings.Default.ServerIP = txtServerIP.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Ihre Änderungen wurden gespeichert.");

            if (!System.IO.Directory.Exists(txtSavePath.Text + "\\Softwareverteilung"))
            {
                System.IO.Directory.CreateDirectory(txtSavePath.Text + "\\Softwareverteilung");
            }
        }

    }
}
