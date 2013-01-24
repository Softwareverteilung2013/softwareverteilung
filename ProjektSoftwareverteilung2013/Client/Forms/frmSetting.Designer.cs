namespace Client.Forms
{
    partial class frmSetting
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
            this.LineShape = new System.Windows.Forms.RichTextBox();
            this.btnChangeSavePath = new System.Windows.Forms.Button();
            this.txtSavePath = new System.Windows.Forms.TextBox();
            this.lblSaveFolder = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtUpdateInterval = new System.Windows.Forms.TextBox();
            this.lblUpdateInterval = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LineShape
            // 
            this.LineShape.BackColor = System.Drawing.SystemColors.InfoText;
            this.LineShape.Location = new System.Drawing.Point(6, 116);
            this.LineShape.Name = "LineShape";
            this.LineShape.Size = new System.Drawing.Size(422, 1);
            this.LineShape.TabIndex = 15;
            this.LineShape.Text = "";
            // 
            // btnChangeSavePath
            // 
            this.btnChangeSavePath.Location = new System.Drawing.Point(353, 90);
            this.btnChangeSavePath.Name = "btnChangeSavePath";
            this.btnChangeSavePath.Size = new System.Drawing.Size(75, 23);
            this.btnChangeSavePath.TabIndex = 14;
            this.btnChangeSavePath.Text = "Ändern";
            this.btnChangeSavePath.UseVisualStyleBackColor = true;
            this.btnChangeSavePath.Click += new System.EventHandler(this.btnChangeSavePath_Click);
            // 
            // txtSavePath
            // 
            this.txtSavePath.Enabled = false;
            this.txtSavePath.Location = new System.Drawing.Point(97, 64);
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.Size = new System.Drawing.Size(331, 20);
            this.txtSavePath.TabIndex = 13;
            // 
            // lblSaveFolder
            // 
            this.lblSaveFolder.AutoSize = true;
            this.lblSaveFolder.Location = new System.Drawing.Point(11, 67);
            this.lblSaveFolder.Name = "lblSaveFolder";
            this.lblSaveFolder.Size = new System.Drawing.Size(61, 13);
            this.lblSaveFolder.TabIndex = 12;
            this.lblSaveFolder.Text = "Speicherort";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(272, 125);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Schließen";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(97, 12);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(331, 20);
            this.txtServerIP.TabIndex = 10;
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Location = new System.Drawing.Point(11, 15);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(48, 13);
            this.lblServerIP.TabIndex = 9;
            this.lblServerIP.Text = "ServerIP";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(353, 125);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Speichern";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtUpdateInterval
            // 
            this.txtUpdateInterval.Location = new System.Drawing.Point(97, 38);
            this.txtUpdateInterval.Name = "txtUpdateInterval";
            this.txtUpdateInterval.Size = new System.Drawing.Size(330, 20);
            this.txtUpdateInterval.TabIndex = 17;
            // 
            // lblUpdateInterval
            // 
            this.lblUpdateInterval.AutoSize = true;
            this.lblUpdateInterval.Location = new System.Drawing.Point(11, 41);
            this.lblUpdateInterval.Name = "lblUpdateInterval";
            this.lblUpdateInterval.Size = new System.Drawing.Size(78, 13);
            this.lblUpdateInterval.TabIndex = 16;
            this.lblUpdateInterval.Text = "Update Uhrzeit";
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 155);
            this.Controls.Add(this.txtUpdateInterval);
            this.Controls.Add(this.lblUpdateInterval);
            this.Controls.Add(this.LineShape);
            this.Controls.Add(this.btnChangeSavePath);
            this.Controls.Add(this.txtSavePath);
            this.Controls.Add(this.lblSaveFolder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtServerIP);
            this.Controls.Add(this.lblServerIP);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSetting";
            this.Text = "frmSetting";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox LineShape;
        private System.Windows.Forms.Button btnChangeSavePath;
        private System.Windows.Forms.TextBox txtSavePath;
        private System.Windows.Forms.Label lblSaveFolder;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Label lblServerIP;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtUpdateInterval;
        private System.Windows.Forms.Label lblUpdateInterval;
    }
}