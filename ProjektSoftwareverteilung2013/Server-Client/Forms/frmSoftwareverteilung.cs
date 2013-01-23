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

    public partial class frmSoftwareverteilung : Form
    {

        public static string NewGroupName;
        public frmSoftwareverteilung()
        {
            InitializeComponent();
            TreeView1.Nodes[0].Expand();
        }

        //Vergleich der Software kann nur gemacht werden, wenn sich clients beim laufen der software am server anmelden denn wohin sonst speichern? Für alle anderen die software aus den paketen nehmen 
        //Software wie inoPhone in den Hintergrund mit Icon unten rechts
        //Wann welches treeview leeren (selected index)


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
                    NewGroupName = "";
                    //Safe to DB
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
                if (TreeView1.SelectedNode.Text == "Default")
                {
                    return;
                }
                if (MessageBox.Show("Möchten Sie die ausgewählte Gruppe mit allen enthaltenen Clients löschen?" , "Achtung!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.IO.Directory.Delete(Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text, true);
                    TreeView1.SelectedNode.Parent.Nodes.Remove(TreeView1.SelectedNode);
                    //Alles Verweise aus DB entfernen
                }
            }

        }


        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (TreeView1.SelectedNode == null || TreeView1.SelectedNode == TreeView1.Nodes[0])
            {
                MessageBox.Show("Benutzer können nur Gruppen hinzugefügt werden. Bitte wählen Sie eine Gruppe aus.");
                return;
            }

            TreeView2.Nodes.Clear();
            //Füge Benutzer der Gruppe Default Treeview2 hinzu und füge Abbruch Button hinzu (Andere Treeviews disablen)
            //Benutzer Selektiert und Bestätigt: Benutzer der selektieren Gruppe und den neuen Benutzer hinzufügen
            //Neuen Benutzer in DB schreiben
            //Abbruch: Benutzer der Gruppe laden
            //Treeviews enablen & Abbruch Button raus
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (TreeView2.SelectedNode == null)
            {
                MessageBox.Show("Bitte wählen Sie einen zu löschenden Benutzer aus.");
                return;
            }
            if (MessageBox.Show("Möchten Sie den ausgewählten Benutzer aus dieser Gruppe löschen? (Dieser wird automatisch in die Gruppe 'Default' verschoben)", "Achtung!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                TreeView2.SelectedNode.Parent.Nodes.Remove(TreeView2.SelectedNode);
                //Alles Verweise aus DB entfernen
                //User in Gruppe Default DB
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

                foreach (string File in FileDialog.FileNames)
                {
                    FileArray[counter] = File;
                    counter++;
                }
                Guid PacketGuid = Guid.NewGuid();
               
                if (!Directory.Exists(Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\"))
                {
                    Directory.CreateDirectory(Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\");
                }
                clsZipFile.ZipFiles(Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\" + PacketGuid + ".zip", FileArray, System.IO.Packaging.CompressionOption.Normal);
                NewListMainItem.Expand();    
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
                    System.IO.File.Copy(FileDialog.FileNames[i], Server_Client.Properties.Settings.Default.SavePath + "Groups\\" + TreeView1.SelectedNode.Text + "\\" + PacketGuid);

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
               
                         NewPacketNode.Expand();
			}

               
                TreeView3.Nodes[0].Expand();
            }
        }


        private void btnDeleteSoftware_Click(object sender, EventArgs e)
        {
            if (TreeView3.SelectedNode == null)
            {
                MessageBox.Show("Bitte wählen Sie ein zu löschendes Programm/Paket aus.");
                return;
            }
            if (MessageBox.Show("Möchten Sie das ausgewählte Programm/Paket aus der gewählten Gruppe löschen?", "Achtung!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                TreeView3.SelectedNode.Parent.Nodes.Remove(TreeView3.SelectedNode);
                //ID's aus Gruppen entfernen & Programme/ZpDatei mit Unterprogrammen aus Ordnern löschen
                //Alles Verweise aus DB entfernen
            }
        }


        private void btnTransmit_Click(object sender, EventArgs e)
        {
            //Prüfe wem welches Packet bei welchem Client fehlt ´fehlende senden
        }


        private void TreeView1_BeforeExpand()
        {
            //Lade alle Benutzer und Software der Gruppe
        }

        private void TreeView2_BeforeExpand()
        {
            //Zeige alle zum Client gehörigen Programme an(bei Anmeldung angezeigte Software > fehlt Software dann mit gelben Rufzeigen rechts anzeigen (könnte auch zuviel Software installiert sein)
        }

        private void TreeView3_NodeMouseClick(System.Object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //If Node hat SoftwareCode dann zeige Contextmenü mit sende Programm
                //Ist node ein Paket, dann sende ganzes paket
            }
        }


        private void BeendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    } 

