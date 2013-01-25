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
        List<ClientInfoModel> ClientInfoModelArray = new List<ClientInfoModel>();
        public frmDefaulClients()
        {
            InitializeComponent();
            
            GroupInfoModel Group = new GroupInfoModel();
            Group.ID = 0;

            List<ClientInfoModel> ClientInfoModelArrayLoading;
            ClientInfoModelArrayLoading = frmSoftwareverteilung.request.getGroupClients(frmSoftwareverteilung.client, Group);

            foreach (ClientInfoModel Client in ClientInfoModelArrayLoading)
            {
                TreeNode CurrentItem = new TreeNode();
                CurrentItem.Tag = Client.ID;
                CurrentItem.Text = Client.pcName;

                treeView1.Nodes.Add(CurrentItem);
                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            frmSoftwareverteilung.ClientInfoModelArray = null;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            foreach (TreeNode Node in treeView1.Nodes)
            {
                if (Node.Checked)
                {
                    ClientInfoModel CurrentModel = new ClientInfoModel();
                    CurrentModel.ID = Convert.ToInt32(Node.Tag);
                    CurrentModel.pcName = Node.Text;
                    ClientInfoModelArray.Add(CurrentModel);
                }
            }
         frmSoftwareverteilung.ClientInfoModelArray = ClientInfoModelArray;
         this.Close();
        }
    }
}
