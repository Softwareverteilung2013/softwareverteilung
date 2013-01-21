using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

    public partial class frmNewName : Form
    {
        public frmNewName()
        {
            InitializeComponent();
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewName.Text))
            {
                frmSoftwareverteilung.NewGroupName = txtNewName.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Bitte geben Sie einen Namen für die Gruppe an");
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
               frmSoftwareverteilung.NewGroupName = "";
               this.Close();
        }

    }

