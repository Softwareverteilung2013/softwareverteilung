using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.Windows.Forms;
using System.IO.Compression;
using Server_Client.Classes;
using SVApi;
using SVApi.Models;
using System.Net.NetworkInformation;
using System.Text;
using System.Security.Principal;
using Server_Client.Forms;

    public partial class frmSoftwareverteilung : Form
    {
        public static string NewGroupName;
        public static List<ClientInfoModel> ClientInfoModelArray;
        public static sendRequest request;
        public static ClientInfoModel client = new ClientInfoModel();
        public frmSoftwareverteilung()
        {
            InitializeComponent();
            TreeView1.Nodes[0].Expand();

            if (!Directory.Exists(Server_Client.Properties.Settings.Default.SavePath + "\\Softwareverteilung"))
            {
                Directory.CreateDirectory(Server_Client.Properties.Settings.Default.SavePath + "\\Softwareverteilung");
            }

            if (Server_Client.Properties.Settings.Default.ServerIP == "0")
            {
                MessageBox.Show("Bitte geben Sie die IP des Servers an.");

               frmSettings settings = new frmSettings();
                settings.ShowDialog();
                if (Server_Client.Properties.Settings.Default.ServerIP == "0")
                {
                    MessageBox.Show("Sie haben keine IP für den Servers angegeben. Das Programm wird beendet.");
                    this.Close();
                }
            }

            client.arc = GetArchitecture();
            client.macAddress = GetMacAddress();
            client.admin = GetAdminBool();

            List<GroupInfoModel> clientList = null;

            try
            {
            request = new sendRequest(Server_Client.Properties.Settings.Default.ServerIP);
            clientList = request.getDatabaseGroups(client);
            }
            catch (Exception)
            {
                MessageBox.Show("Probleme beim Herstellen einer Verbindung. Bitte überprüfen Sie Ihre Einstellungen.");

                frmSettings settings = new frmSettings();
                settings.ShowDialog();

                try
                {
                clientList = request.getDatabaseGroups(client);
                }
                catch (Exception)
                {
                    MessageBox.Show("Verbindung kann nicht hergestellt werden. Anwendung wird beendet.");
                    this.Close();
                }

                return;
            }
          
            foreach (GroupInfoModel Group in clientList)
            {
                TreeNode CurrentGroup = new TreeNode(Group.Name);
                CurrentGroup.Tag = Group.ID;
                TreeView1.Nodes[0].Nodes.Add(CurrentGroup);
            }

           
        }


        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            if (AddGroup() == false)
            {
                btnAddGroup_Click(sender, e);
                return;
            }
            
        }

        private bool AddGroup()
        {

            TreeNode NewListItem = new TreeNode("Neuer Eintrag");
            TreeView1.Nodes[0].Nodes.Add(NewListItem);

            frmNewName NewName = new frmNewName();
            NewName.ShowDialog();

            if (!string.IsNullOrEmpty(NewGroupName))
            {
                if (CheckIfEntryExists(ref NewListItem))
                {
                    MessageBox.Show("Gruppenname bereits vorhanden. Bitte geben Sie einen neuen Namen an.");
                    NewGroupName = "";
                    NewListItem.Parent.Nodes.Remove(NewListItem);
                    return false;
                }
                else
                {
                    NewListItem.Text = NewGroupName;
                    if (System.IO.Directory.Exists(Server_Client.Properties.Settings.Default.SavePath + "Groups") == false)
                    {
                        System.IO.Directory.CreateDirectory(Server_Client.Properties.Settings.Default.SavePath + "Groups");
                    }
                    System.IO.Directory.CreateDirectory(Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + NewGroupName);
                    
                    //Safe to DB                  
                    GroupInfoModel group = new GroupInfoModel();
                    group.Name = NewGroupName;
                    group.ID = -1;
                    NewListItem.Tag = request.addGroupInfo(client, group);
                    if (Convert.ToInt32(NewListItem.Tag) == -1)
                    {
                    MessageBox.Show("Es ist ein Fehler beim Hinzufügen aufgetreten.", "Achtung",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                    NewListItem.Parent.Nodes.Remove(NewListItem);
                    }
                    NewGroupName = "";
                }
            }
            else
            {
                NewListItem.Parent.Nodes.Remove(NewListItem);
            }

            return true;
        }

        private bool CheckIfEntryExists(ref TreeNode NewListItem)
        {

            foreach (TreeNode Node in TreeView1.Nodes[0].Nodes)
            {
                if (Node.Text == NewGroupName)
                {
                    return true;
                }
            }

            return false;
        }

        private void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            if ((TreeView1.SelectedNode != null))
            {
                if (TreeView1.SelectedNode.Text == "Standard")
                {
                    return;
                }
                if (MessageBox.Show("Möchten Sie die ausgewählten Gruppen mit allen enthaltenen Clients löschen?" , "Achtung!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                  //  System.IO.Directory.Delete(Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text, true);

                   
                    GroupInfoModel CurrentGroup = new GroupInfoModel();
                    CurrentGroup.ID = Convert.ToInt32(TreeView1.SelectedNode.Tag);
                    CurrentGroup.Name = TreeView1.SelectedNode.Text;
                    request.delGroupInfo(client, CurrentGroup);

                    TreeView1.SelectedNode.Parent.Nodes.Remove(TreeView1.SelectedNode);

                }
            }
            TreeView2.Nodes[0].Nodes.Clear();
            TreeView3.Nodes[0].Nodes.Clear();
        }


        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (TreeView1.SelectedNode == null || TreeView1.SelectedNode == TreeView1.Nodes[0] || TreeView1.SelectedNode.Text == "Standard")
            {
                MessageBox.Show("Benutzer können nur Gruppen hinzugefügt werden. Bitte wählen Sie eine Gruppe aus.");
                return;
            }

            TreeView2.Nodes[0].Nodes.Clear();
            frmDefaulClients DefaultClients = new frmDefaulClients();
            DefaultClients.ShowDialog();

            if (ClientInfoModelArray != null)
            {
                List<ClientInfoModel> InfoModelArray = request.getDatabaseClients(client);
            foreach (ClientInfoModel CurrentClient in ClientInfoModelArray)
            {
                CurrentClient.group =Convert.ToInt32( TreeView1.SelectedNode.Tag);
                
                foreach (ClientInfoModel CurrClient in InfoModelArray)
                {
                    if (CurrClient.ID == CurrentClient.ID)
                    {
                        CurrentClient.arc = CurrClient.arc;
                        CurrentClient.admin = CurrClient.admin;
                        CurrentClient.macAddress = CurrClient.macAddress;
                        break;
                    }
                }
                request.addClientInfo(client, CurrentClient); 
            }
            }
            GroupInfoModel UpdatedGroup = new GroupInfoModel();
            UpdatedGroup.ID =Convert.ToInt32( TreeView1.SelectedNode.Tag);
     
            List<ClientInfoModel> clientList = null;
            clientList = request.getGroupClients(client, UpdatedGroup);

            foreach (ClientInfoModel Client in clientList)
            {
                TreeNode CurrentNode = new TreeNode(Client.pcName);
                CurrentNode.Tag = Client.ID;
                TreeView2.Nodes[0].Nodes.Add(CurrentNode);
            }

            ClientInfoModelArray = null;

            TreeView2.Nodes[0].Expand();
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            TreeNode NodeToDelete = new TreeNode();
            for (int i = 0; i < TreeView2.Nodes[0].Nodes.Count; i++)
            {
                if (TreeView2.Nodes[0].Nodes[i].Checked)
                {
                    NodeToDelete = TreeView2.Nodes[0].Nodes[i];
                }
            }
            if (NodeToDelete.TreeView == null)
            {
                MessageBox.Show("Bitte wählen Sie einen zu löschenden Benutzer aus.");
                return;
            }
            
            if (MessageBox.Show("Möchten Sie den ausgewählten Benutzer aus dieser Gruppe löschen? (Dieser wird automatisch in die Gruppe 'Standard' verschoben)", "Achtung!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                for (int i = 0; i < TreeView2.Nodes[0].Nodes.Count; i++)
                {
                    if (TreeView2.Nodes[0].Nodes[i].Checked)
                    {
                        ClientInfoModel DelClient = new ClientInfoModel();
                        DelClient.ID = Convert.ToInt32(TreeView2.Nodes[0].Nodes[i].Tag);
                        request.delClientInfo(client, DelClient);

                        TreeView2.Nodes[0].Nodes[i].Parent.Nodes.Remove(TreeView2.Nodes[0].Nodes[i]);
                    }
                }
            }
            
        }


        private void btnAddSoftware_Click(object sender, EventArgs e)
        {
            if (TreeView1.SelectedNode == null | object.ReferenceEquals(TreeView1.SelectedNode, TreeView1.Nodes[0]))
            {
             MessageBox.Show("Programme können nur Gruppen hinzugefügt werden. Bitte wählen Sie eine Gruppe aus.");
                return;
            }

            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.Multiselect = true;

            FileDialog.Filter = "msi files (*.msi)|*.msi|exe files (*.exe*)|*.exe*|All files (*.*)|*.*";
            FileDialog.FilterIndex = 1;
            FileDialog.RestoreDirectory = true;

            //geben sie einen namen für paket an falls kein paket markiert ist!
            if (FileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (FileDialog.SafeFileNames.Length == 0)
                {
                    return;
                }
                TreeNode NewListMainItem = new TreeNode("Neuer Eintrag");
                TreeView3.Nodes[0].Nodes.Add(NewListMainItem);

            frmNewName NewName = new frmNewName();
            NewName.ShowDialog();

            if (!string.IsNullOrEmpty(NewGroupName))
            {
                NewListMainItem.Text = NewGroupName;
                NewGroupName = "";
                TreeView3.Nodes[0].Expand();
            }
            else 
            {
                MessageBox.Show("Kein Softwarepaketname angegeben. Vorgang wird abgebrochen.");
                return; 
            }

                foreach (string File in FileDialog.SafeFileNames)
                {
                    TreeNode NewListItem = new TreeNode(File);
                    NewListMainItem.Nodes.Add(NewListItem);
                }
                string[] FileArray = new string[FileDialog.FileNames.Length];
                int counter = 0;
            
                string archit = "32bit";
                foreach (string File in FileDialog.FileNames)
                {
                    //Assembly ExeAssembly = Assembly.LoadFile(File);
                    //System.Reflection.AssemblyName AssemblyAr = new System.Reflection.AssemblyName(ExeAssembly.FullName);
                    //if (archit != "" && archit != AssemblyAr.ProcessorArchitecture.ToString())
                    //{
                    //    MessageBox.Show("Die Datei: " + File + " hat das falsche Format(" + AssemblyAr.ProcessorArchitecture.ToString() + "). Bitte erstellen Sie für 32/64 bit Programme einzelne Softwarepakete.");
                    //    continue;
                    //}
                    //archit = AssemblyAr.ProcessorArchitecture.ToString();

                    FileArray[counter] = File;
                    counter++;
                }
                Guid PacketGuid = Guid.NewGuid();

                string Path = Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\";
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                clsZipFile.ZipFiles(Path + PacketGuid + ".zip", FileArray, System.IO.Packaging.CompressionOption.Normal);
                NewListMainItem.Expand();

                //Assembly assembly = Assembly.LoadFile(Path + PacketGuid + ".zip");
                //System.Reflection.AssemblyName AssemblyName = new System.Reflection.AssemblyName(assembly.FullName);
                FileInfo fileinfo_ = new FileInfo(Path + PacketGuid + ".zip");
                    
                PackageInfoModel CurrentPackage = new PackageInfoModel();
                CurrentPackage.showName = NewListMainItem.Text;
                CurrentPackage.Name =  PacketGuid.ToString();
                CurrentPackage.arc = archit;
                CurrentPackage.size  =Convert.ToInt32( fileinfo_.Length ) ;
                CurrentPackage.group = Convert.ToInt32( TreeView1.SelectedNode.Tag);

                request.addPackageInfo(client, CurrentPackage);

                GroupInfoModel GroupModel = new GroupInfoModel();
                GroupModel.ID = Convert.ToInt32( TreeView1.SelectedNode.Tag);
                GroupModel.Name = TreeView1.SelectedNode.Text;
                request.sendFile(client, GroupModel, CurrentPackage, Path + PacketGuid + ".zip");
            }

           
        //Füge Kopie der Datei hinzu und unten entfernen und client gui trayicon
                
                    //In Datenbank für diese Gruppe anlegen falls nicht exitstiert
                    //Pfad auf ServerClient dazu speichern
                    //msi gui
            }

        private void btnAddExistingPacket_Click(object sender, EventArgs e)
        {
            if (TreeView1.SelectedNode == null | object.ReferenceEquals(TreeView1.SelectedNode, TreeView1.Nodes[0]))
            {
                MessageBox.Show("Programme können nur Gruppen hinzugefügt werden. Bitte wählen Sie eine Gruppe aus.");
                return;
            }

             OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.Multiselect = true;

            FileDialog.Filter = "zip (*.zip)|*.zip";
            FileDialog.FilterIndex = 1;
            FileDialog.InitialDirectory = Server_Client.Properties.Settings.Default.SavePath + "Groups\\";
            FileDialog.RestoreDirectory = true;

            //geben sie einen namen für paket an falls kein paket markiert ist!
            if (FileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (FileDialog.SafeFileNames.Length == 0)
                {
                    return;
                }
               
                for (int i = 0; i < FileDialog.FileNames.Length; i++)
			{
                    Guid PacketGuid = Guid.NewGuid();
                    string Path = Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\" + PacketGuid + ".zip";
                    System.IO.File.Copy(FileDialog.FileNames[i], Path);

                    frmNewName NewName = new frmNewName();
                    NewName.ShowDialog();
                    //Hier Datenbankabfrage der Gruppen auf Gruppe mit ID (Diesen Namen nehmen)

                    TreeNode NewPacketNode = new TreeNode();
                    if (!string.IsNullOrEmpty(NewGroupName))
                    {
                        NewPacketNode.Text = NewGroupName;
                        NewGroupName = "";
                        TreeView3.Nodes[0].Nodes.Add(NewPacketNode);
                        TreeView3.Nodes[0].Expand();
                    }
                    else
                    {
                        MessageBox.Show("Kein Softwarepaketname angegeben. Vorgang wird abgebrochen.");
                        return;
                    }
               
                   clsZipFile.Unzip(FileDialog.FileNames[i].ToString(), Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\Temp\\");

                   string[] FileArr=  System.IO.Directory.GetFiles(Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\Temp\\");

                   for (int d = 0; d < FileArr.Length; d++)
                   {
                       System.IO.DirectoryInfo oDir = new System.IO.DirectoryInfo(FileArr[0]);
                       NewPacketNode.Nodes.Add(oDir.Name);
                       System.IO.File.Delete(oDir.FullName);
                   }

                   System.IO.File.Copy(FileDialog.FileNames[i].ToString(), Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\" + NewPacketNode.Text);
                   string CurrentName = FileDialog.SafeFileName[i].ToString().Replace(".zip", "");

                 List<PackageInfoModel> AllPackages = request.getDatabasePackages(client);
                 PackageInfoModel CurrentPackage = new PackageInfoModel();
                 CurrentPackage.Name = PacketGuid.ToString();
                 CurrentPackage.group = Convert.ToInt32(TreeView1.SelectedNode.Tag);

                    foreach (PackageInfoModel Package in AllPackages)
                 {
                     if (Package.Name == CurrentName)
                     {
                         CurrentPackage.showName = Package.showName;
                         CurrentPackage.arc = Package.arc;
                         CurrentPackage.size = Package.size;
                         break;
                     } 
                 }
                    request.addPackageInfo(client, CurrentPackage);

                    GroupInfoModel GroupModel = new GroupInfoModel();
                    GroupModel.ID = Convert.ToInt32(TreeView1.SelectedNode.Tag);
                    GroupModel.Name = TreeView1.SelectedNode.Text;
                    request.sendFile(client, GroupModel, CurrentPackage, Path + PacketGuid + ".zip");

                    NewPacketNode.Expand();
			}

               
                TreeView3.Nodes[0].Expand();
            }
        }


        private void btnDeleteSoftware_Click(object sender, EventArgs e)
        {
            TreeNode NodeToDelete = new TreeNode();
            for (int i = 0; i < TreeView3.Nodes[0].Nodes.Count; i++)
                {
                    if (TreeView3.Nodes[0].Nodes[i].Checked)
                    {
                        NodeToDelete = TreeView3.Nodes[0].Nodes[i];
                    }
            }
            if (NodeToDelete.TreeView == null)
	{
		 MessageBox.Show("Bitte wählen Sie ein zu löschendes Programm/Paket aus.");
                return;
	}
               
            if (MessageBox.Show("Möchten Sie das ausgewählte Programm/Paket aus der gewählten Gruppe löschen?", "Achtung!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                for (int i = 0; i < TreeView3.Nodes[0].Nodes.Count; i++)
                {
                    if (TreeView3.Nodes[0].Nodes[i].Checked)
                    {
                        PackageInfoModel CurrentPackage = new PackageInfoModel();
                        CurrentPackage.ID = Convert.ToInt32(TreeView3.Nodes[0].Nodes[i].Tag);
                        CurrentPackage.Name = TreeView3.Nodes[0].Nodes[i].Text;
                        request.delPackageInfo(client, CurrentPackage);
                        TreeView3.Nodes[0].Nodes[i].Parent.Nodes.Remove(TreeView3.Nodes[0].Nodes[i]);

                        System.IO.File.Delete(Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\" + CurrentPackage.Name + ".zip");
                    }
                }
            }
        }


        private void BeendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowForm(object sender, MouseEventArgs e)
        {
            this.TopMost = true;
            this.TopMost = false;
        }


        private string GetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //for each j you can get the MAC
            PhysicalAddress address = nics[0].GetPhysicalAddress();
            StringBuilder strb = new StringBuilder();
            for (int e = 0; e < nics.Length; e++)
            {
                byte[] bytes = address.GetAddressBytes();
                for (int i = 0; i < bytes.Length; i++)
                {
                    // Display the physical address in hexadecimal.
                    strb.AppendLine(bytes[i].ToString("X2"));
                    // Insert a hyphen after each byte, unless we are at the end of the
                    // address.
                    if (i != bytes.Length - 1)
                    {
                        strb.AppendLine("-");
                    }
                }
            }

            return strb.ToString();
        }

        private bool GetAdminBool()
        {
            bool isAdmin;
            try
            {
                //get the currently logged in user
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException)
            {
                isAdmin = false;
            }
            catch (Exception)
            {
                isAdmin = false;
            }
            return isAdmin;
        }

        private string GetArchitecture()
        {

            string architecture = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            string archWOW = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432");
            if (archWOW != null && archWOW != "" && archWOW.Contains("64"))
                return "x64";
            if (architecture.Contains("86"))
                return "x86";
            if (architecture.Contains("64"))
                return "x64";

            if (architecture == null)
            {
                architecture = "";
            }
            else if (architecture == "")
            {
                architecture = "Not Defined";
            }
            return architecture;
        }

        private void ServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
         frmSettings settings = new frmSettings();
         settings.ShowDialog();
        }

        private void frmSoftwareverteilung_FormClosed(object sender, FormClosedEventArgs e)
        {
        icoSoftwareverteilung.Visible = false;
        }

  
        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (e.Node == TreeView1.Nodes[0])
            {
                return;
            }

            TreeView2.Nodes[0].Nodes.Clear();
            TreeView3.Nodes[0].Nodes.Clear();

            TreeView3.Nodes[0].Text = "Pakete";
            TreeView3.Nodes[0].ForeColor = System.Drawing.Color.Black;

            GroupInfoModel CurrentGroup = new GroupInfoModel();
            CurrentGroup.ID = Convert.ToInt32(e.Node.Tag);

            List<ClientInfoModel> clientList = null;
            clientList = request.getGroupClients(client, CurrentGroup);

            foreach (ClientInfoModel Client in clientList)
            {
                TreeNode CurrentNode = new TreeNode(Client.pcName);
                CurrentNode.Tag = Client.ID;
                TreeView2.Nodes[0].Nodes.Add(CurrentNode);
            }

            List<PackageInfoModel> softwareList = null;
            softwareList = request.getGroupPackages(client, CurrentGroup);

            foreach (PackageInfoModel Package in softwareList)
            {
                TreeNode CurrentNode = new TreeNode(Package.showName);
                CurrentNode.Tag = Package.ID;
                TreeView3.Nodes[0].Nodes.Add(CurrentNode);


                clsZipFile.Unzip(Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\" + Package.Name + ".zip", Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\Temp\\");

                string[] FileArr = System.IO.Directory.GetFiles(Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\Temp\\");

                for (int d = 0; d < FileArr.Length; d++)
                {
                    System.IO.DirectoryInfo oDir = new System.IO.DirectoryInfo(FileArr[0]);
                    CurrentNode.Nodes.Add(oDir.Name);
                    System.IO.File.Delete(oDir.FullName);
                }
            }
            TreeView2.Nodes[0].Expand();
            TreeView3.Nodes[0].Expand();
        }

        private void TreeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (e.Node == TreeView2.Nodes[0])
            {
                return;
            }
            TreeView3.Nodes[0].Nodes.Clear();

            List<PackageInfoModel> softwareList = null;
            ClientInfoModel CurrentClient = new ClientInfoModel();
            CurrentClient.ID = Convert.ToInt32(e.Node.Tag);
            softwareList = request.getClientPackages(client, CurrentClient);

            if (softwareList == null)
            {
                return;
            }
            foreach (PackageInfoModel Package in softwareList)
            {
                TreeNode CurrentNode = new TreeNode(Package.showName);
                CurrentNode.Tag = Package.ID;
                TreeView3.Nodes[0].Nodes.Add(CurrentNode);
                TreeView3.Nodes[0].Text = TreeView3.Nodes[0].Text + " (Clientsoftware)";
                TreeView3.Nodes[0].ForeColor = System.Drawing.Color.Red;

                clsZipFile.Unzip(Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\" + Package.Name + ".zip", Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\Temp\\");

                string[] FileArr = System.IO.Directory.GetFiles(Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\Temp\\");

                for (int d = 0; d < FileArr.Length; d++)
                {
                    System.IO.DirectoryInfo oDir = new System.IO.DirectoryInfo(FileArr[0]);
                    CurrentNode.Nodes.Add(oDir.Name);
                    System.IO.File.Delete(oDir.FullName);
                }
            }
        }
    } 

