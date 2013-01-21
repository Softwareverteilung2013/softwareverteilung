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

    public partial class frmSoftwareverteilung : Form
    {

        public static string NewGroupName;
        public frmSoftwareverteilung()
        {
            InitializeComponent();
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
                    if (System.IO.Directory.Exists(Application.StartupPath + "\\Groups") == false)
                    {
                        System.IO.Directory.CreateDirectory(Application.StartupPath + "\\Groups");
                        System.IO.Directory.CreateDirectory(Application.StartupPath + "\\Groups\\" + NewGroupName);
                    }
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
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "\\Groups\\" + TreeView1.SelectedNode.Text);
                    TreeView1.SelectedNode.Parent.Nodes.Remove(TreeView1.SelectedNode);
                    //Alles Verweise aus DB entfernen
                }
            }

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


        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (TreeView1.SelectedNode == null | object.ReferenceEquals(TreeView1.SelectedNode, TreeView1.Nodes[0]))
            {
                MessageBox.Show("Benutzer können nur Gruppen hinzugefügt werden. Bitte wählen Sie eine Gruppe aus.");
                return;
            }
            //Auflistung Aller User aus der Default Gruppe
            //Benuter in DB und Treeview anlegen
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
            }
             //Abfrage dann User in Gruppe Default
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
            FileDialog.FilterIndex = 2;
            FileDialog.RestoreDirectory = true;

            //geben sie einen namen für paket an falls kein paket markiert ist!
            if (FileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string File in FileDialog.SafeFileNames)
                {
                    TreeNode NewListItem = new TreeNode(File);
                    TreeView1.Nodes[0].Nodes.Add(NewListItem);
                    Assembly test = Assembly.LoadFile(FileDialog.FileName);
                    MessageBox.Show(test.GetType().GUID.ToString());

                    //In Datenbank für diese Gruppe anlegen falls nicht exitstiert
                    //Pfad auf ServerClient dazu speichern
                    //msi gui
                }
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
                //ID's aus Gruppen entfernen & Programme aus Ordnern löschen
                //Alles Verweise aus DB entfernen
            }

            //Delete Software from Package & DB 
            //2. Schritt automatische Deinstallation auf Clients
        }


        private void btnTransmit_Click(object sender, EventArgs e)
        {
            //Prüfe wem welches Packet bei welchem Client fehlt ´fehlende senden
        }
                   
    } 

