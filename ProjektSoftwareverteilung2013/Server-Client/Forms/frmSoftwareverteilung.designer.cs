    partial class frmSoftwareverteilung
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Programme");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Benutzer");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Default");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Gruppen", new System.Windows.Forms.TreeNode[] {
            treeNode3});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSoftwareverteilung));
            this.btnTransmit = new System.Windows.Forms.Button();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDeleteSoftware = new System.Windows.Forms.Button();
            this.BearbeitenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BeendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.btnDeleteGroup = new System.Windows.Forms.Button();
            this.btnAddSoftware = new System.Windows.Forms.Button();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.TreeView3 = new System.Windows.Forms.TreeView();
            this.TreeView2 = new System.Windows.Forms.TreeView();
            this.TreeView1 = new System.Windows.Forms.TreeView();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.btnAddExistingPacket = new System.Windows.Forms.Button();
            this.icoSoftwareverteilung = new System.Windows.Forms.NotifyIcon(this.components);
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTransmit
            // 
            this.btnTransmit.Location = new System.Drawing.Point(580, 437);
            this.btnTransmit.Name = "btnTransmit";
            this.btnTransmit.Size = new System.Drawing.Size(227, 23);
            this.btnTransmit.TabIndex = 22;
            this.btnTransmit.Text = "Verteilen";
            this.btnTransmit.UseVisualStyleBackColor = true;
            this.btnTransmit.Click += new System.EventHandler(this.btnTransmit_Click);
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(124, 6);
            // 
            // btnDeleteSoftware
            // 
            this.btnDeleteSoftware.Location = new System.Drawing.Point(349, 403);
            this.btnDeleteSoftware.Name = "btnDeleteSoftware";
            this.btnDeleteSoftware.Size = new System.Drawing.Size(458, 23);
            this.btnDeleteSoftware.TabIndex = 20;
            this.btnDeleteSoftware.Text = "Löschen";
            this.btnDeleteSoftware.UseVisualStyleBackColor = true;
            this.btnDeleteSoftware.Click += new System.EventHandler(this.btnDeleteSoftware_Click);
            // 
            // BearbeitenToolStripMenuItem
            // 
            this.BearbeitenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ServerToolStripMenuItem,
            this.ToolStripMenuItem1,
            this.BeendenToolStripMenuItem});
            this.BearbeitenToolStripMenuItem.Name = "BearbeitenToolStripMenuItem";
            this.BearbeitenToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.BearbeitenToolStripMenuItem.Text = "Datei";
            // 
            // ServerToolStripMenuItem
            // 
            this.ServerToolStripMenuItem.Name = "ServerToolStripMenuItem";
            this.ServerToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.ServerToolStripMenuItem.Text = "Server";
            // 
            // BeendenToolStripMenuItem
            // 
            this.BeendenToolStripMenuItem.Name = "BeendenToolStripMenuItem";
            this.BeendenToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.BeendenToolStripMenuItem.Text = "Beenden";
            this.BeendenToolStripMenuItem.Click += new System.EventHandler(this.BeendenToolStripMenuItem_Click);
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.Location = new System.Drawing.Point(181, 404);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(162, 23);
            this.btnDeleteUser.TabIndex = 19;
            this.btnDeleteUser.Text = "Löschen";
            this.btnDeleteUser.UseVisualStyleBackColor = true;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
            // 
            // btnDeleteGroup
            // 
            this.btnDeleteGroup.Location = new System.Drawing.Point(13, 403);
            this.btnDeleteGroup.Name = "btnDeleteGroup";
            this.btnDeleteGroup.Size = new System.Drawing.Size(162, 23);
            this.btnDeleteGroup.TabIndex = 18;
            this.btnDeleteGroup.Text = "Löschen";
            this.btnDeleteGroup.UseVisualStyleBackColor = true;
            this.btnDeleteGroup.Click += new System.EventHandler(this.btnDeleteGroup_Click);
            // 
            // btnAddSoftware
            // 
            this.btnAddSoftware.Location = new System.Drawing.Point(349, 375);
            this.btnAddSoftware.Name = "btnAddSoftware";
            this.btnAddSoftware.Size = new System.Drawing.Size(227, 23);
            this.btnAddSoftware.TabIndex = 17;
            this.btnAddSoftware.Text = "Hinzufügen";
            this.btnAddSoftware.UseVisualStyleBackColor = true;
            this.btnAddSoftware.Click += new System.EventHandler(this.btnAddSoftware_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.Location = new System.Drawing.Point(181, 375);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(162, 23);
            this.btnAddUser.TabIndex = 16;
            this.btnAddUser.Text = "Hinzufügen";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Location = new System.Drawing.Point(13, 374);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(162, 23);
            this.btnAddGroup.TabIndex = 15;
            this.btnAddGroup.Text = "Hinzufügen";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // TreeView3
            // 
            this.TreeView3.Location = new System.Drawing.Point(349, 27);
            this.TreeView3.Name = "TreeView3";
            treeNode1.Name = "Software";
            treeNode1.Text = "Programme";
            this.TreeView3.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.TreeView3.Size = new System.Drawing.Size(458, 342);
            this.TreeView3.TabIndex = 14;
            // 
            // TreeView2
            // 
            this.TreeView2.Location = new System.Drawing.Point(181, 27);
            this.TreeView2.Name = "TreeView2";
            treeNode2.Name = "User";
            treeNode2.Text = "Benutzer";
            this.TreeView2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.TreeView2.Size = new System.Drawing.Size(162, 342);
            this.TreeView2.TabIndex = 13;
            // 
            // TreeView1
            // 
            this.TreeView1.Location = new System.Drawing.Point(12, 27);
            this.TreeView1.Name = "TreeView1";
            treeNode3.Name = "Default";
            treeNode3.Text = "Default";
            treeNode4.Name = "Groups";
            treeNode4.Text = "Gruppen";
            this.TreeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
            this.TreeView1.Size = new System.Drawing.Size(163, 342);
            this.TreeView1.TabIndex = 12;
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BearbeitenToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(824, 24);
            this.MenuStrip.TabIndex = 23;
            this.MenuStrip.Text = "MenuStrip1";
            // 
            // btnAddExistingPacket
            // 
            this.btnAddExistingPacket.Location = new System.Drawing.Point(580, 375);
            this.btnAddExistingPacket.Name = "btnAddExistingPacket";
            this.btnAddExistingPacket.Size = new System.Drawing.Size(227, 23);
            this.btnAddExistingPacket.TabIndex = 24;
            this.btnAddExistingPacket.Text = "Vorhandenes Paket hinzufügen";
            this.btnAddExistingPacket.UseVisualStyleBackColor = true;
            this.btnAddExistingPacket.Click += new System.EventHandler(this.btnAddExistingPacket_Click);
            // 
            // icoSoftwareverteilung
            // 
            this.icoSoftwareverteilung.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.icoSoftwareverteilung.BalloonTipText = "Softwareverteilung";
            this.icoSoftwareverteilung.Icon = ((System.Drawing.Icon)(resources.GetObject("icoSoftwareverteilung.Icon")));
            this.icoSoftwareverteilung.Text = "Softwareverteilung";
            this.icoSoftwareverteilung.Visible = true;
            this.icoSoftwareverteilung.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ShowForm);
            // 
            // frmSoftwareverteilung
            // 
            this.ClientSize = new System.Drawing.Size(824, 472);
            this.Controls.Add(this.btnAddExistingPacket);
            this.Controls.Add(this.btnTransmit);
            this.Controls.Add(this.btnDeleteSoftware);
            this.Controls.Add(this.btnDeleteUser);
            this.Controls.Add(this.btnDeleteGroup);
            this.Controls.Add(this.btnAddSoftware);
            this.Controls.Add(this.btnAddUser);
            this.Controls.Add(this.btnAddGroup);
            this.Controls.Add(this.TreeView3);
            this.Controls.Add(this.TreeView2);
            this.Controls.Add(this.TreeView1);
            this.Controls.Add(this.MenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSoftwareverteilung";
            this.Text = "Benutzerübersicht";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnTransmit;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem1;
        internal System.Windows.Forms.Button btnDeleteSoftware;
        internal System.Windows.Forms.ToolStripMenuItem BearbeitenToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ServerToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem BeendenToolStripMenuItem;
        internal System.Windows.Forms.Button btnDeleteUser;
        internal System.Windows.Forms.Button btnDeleteGroup;
        internal System.Windows.Forms.Button btnAddSoftware;
        internal System.Windows.Forms.Button btnAddUser;
        internal System.Windows.Forms.Button btnAddGroup;
        internal System.Windows.Forms.TreeView TreeView3;
        internal System.Windows.Forms.TreeView TreeView2;
        internal System.Windows.Forms.TreeView TreeView1;
        internal System.Windows.Forms.MenuStrip MenuStrip;
        internal System.Windows.Forms.Button btnAddExistingPacket;
        private System.Windows.Forms.NotifyIcon icoSoftwareverteilung;
    }


