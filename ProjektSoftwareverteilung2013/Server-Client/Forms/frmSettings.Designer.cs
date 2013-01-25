namespace Server_Client.Forms
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.btnSave = new System.Windows.Forms.Button();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtSavePath = new System.Windows.Forms.TextBox();
            this.lblSaveFolder = new System.Windows.Forms.Label();
            this.btnChangeSavePath = new System.Windows.Forms.Button();
            this.LineShape = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(354, 102);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Speichern";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Location = new System.Drawing.Point(12, 15);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(48, 13);
            this.lblServerIP.TabIndex = 1;
            this.lblServerIP.Text = "ServerIP";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(83, 12);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(346, 20);
            this.txtServerIP.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(273, 102);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Schließen";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtSavePath
            // 
            this.txtSavePath.Enabled = false;
            this.txtSavePath.Location = new System.Drawing.Point(83, 38);
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.Size = new System.Drawing.Size(346, 20);
            this.txtSavePath.TabIndex = 5;
            // 
            // lblSaveFolder
            // 
            this.lblSaveFolder.AutoSize = true;
            this.lblSaveFolder.Location = new System.Drawing.Point(12, 41);
            this.lblSaveFolder.Name = "lblSaveFolder";
            this.lblSaveFolder.Size = new System.Drawing.Size(61, 13);
            this.lblSaveFolder.TabIndex = 4;
            this.lblSaveFolder.Text = "Speicherort";
            // 
            // btnChangeSavePath
            // 
            this.btnChangeSavePath.Location = new System.Drawing.Point(354, 64);
            this.btnChangeSavePath.Name = "btnChangeSavePath";
            this.btnChangeSavePath.Size = new System.Drawing.Size(75, 23);
            this.btnChangeSavePath.TabIndex = 6;
            this.btnChangeSavePath.Text = "Ändern";
            this.btnChangeSavePath.UseVisualStyleBackColor = true;
            this.btnChangeSavePath.Click += new System.EventHandler(this.btnChangeSavePath_Click);
            // 
            // LineShape
            // 
            this.LineShape.BackColor = System.Drawing.SystemColors.InfoText;
            this.LineShape.Location = new System.Drawing.Point(7, 93);
            this.LineShape.Name = "LineShape";
            this.LineShape.Size = new System.Drawing.Size(422, 1);
            this.LineShape.TabIndex = 7;
            this.LineShape.Text = "";
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 130);
            this.Controls.Add(this.LineShape);
            this.Controls.Add(this.btnChangeSavePath);
            this.Controls.Add(this.txtSavePath);
            this.Controls.Add(this.lblSaveFolder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtServerIP);
            this.Controls.Add(this.lblServerIP);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSettings";
            this.Text = "frmSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblServerIP;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtSavePath;
        private System.Windows.Forms.Label lblSaveFolder;
        private System.Windows.Forms.Button btnChangeSavePath;
        private System.Windows.Forms.RichTextBox LineShape;

    }
}