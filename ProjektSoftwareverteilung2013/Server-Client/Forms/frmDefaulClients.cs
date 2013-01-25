using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SVApi.Models;
using SVApi;

namespace Server_Client.Forms
{
    public partial class frmDefaulClients : Form
    {
        List<ClientInfoModel> ClientInfoModelArray;
        public frmDefaulClients()
        {
            InitializeComponent();
            
            GroupInfoModel Group = new GroupInfoModel();
            Group.ID = 0;

            List<ClientInfoModel> ClientInfoModelArrayLoading;
            ClientInfoModelArrayLoading = frmSoftwareverteilung.request.getGroupClients(frmSoftwareverteilung.client, Group);

            foreach (ClientInfoModel Client in ClientInfoModelArrayLoading)
            {
                ListViewItem.ListViewSubItem CurrentItem = new ListViewItem.ListViewSubItem();
                CurrentItem.Tag = Client.ID;
                CurrentItem.Text = Client.pcName;
                listView1.Items[0].SubItems.Add(CurrentItem);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            frmSoftwareverteilung.ClientInfoModelArray = null;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            foreach (TreeNode Node in listView1.SelectedItems)
            {
                ClientInfoModel CurrentModel = new ClientInfoModel();
                CurrentModel.ID = Convert.ToInt32( Node.Tag);
                CurrentModel.pcName = Node.Text;
                ClientInfoModelArray.Add(CurrentModel);
            }
         frmSoftwareverteilung.ClientInfoModelArray = ClientInfoModelArray;
        }
    }
}
