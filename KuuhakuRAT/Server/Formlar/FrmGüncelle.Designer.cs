﻿namespace xServer.Formlar
{
    partial class FrmGüncelle
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGüncelle));
            this.btnUpdate = new System.Windows.Forms.Button();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.lblInformation = new System.Windows.Forms.Label();
            this.groupLocalFile = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupURL = new System.Windows.Forms.GroupBox();
            this.radioLocalFile = new System.Windows.Forms.RadioButton();
            this.radioURL = new System.Windows.Forms.RadioButton();
            this.groupLocalFile.SuspendLayout();
            this.groupURL.SuspendLayout();
            this.SuspendLayout();
            this.btnUpdate.Location = new System.Drawing.Point(353, 235);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(138, 23);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Client\'i Güncelle";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            this.txtURL.Location = new System.Drawing.Point(56, 25);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(320, 22);
            this.txtURL.TabIndex = 1;
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(20, 28);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(30, 13);
            this.lblURL.TabIndex = 0;
            this.lblURL.Text = "URL:";
            this.lblInformation.AutoSize = true;
            this.lblInformation.Location = new System.Drawing.Point(12, 240);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(331, 13);
            this.lblInformation.TabIndex = 4;
            this.lblInformation.Text = "Önceki clientiniz ile aynı ayarları kullandığınızdan emin olunuz.";
            this.groupLocalFile.Controls.Add(this.btnBrowse);
            this.groupLocalFile.Controls.Add(this.txtPath);
            this.groupLocalFile.Controls.Add(this.label1);
            this.groupLocalFile.Location = new System.Drawing.Point(12, 35);
            this.groupLocalFile.Name = "groupLocalFile";
            this.groupLocalFile.Size = new System.Drawing.Size(479, 75);
            this.groupLocalFile.TabIndex = 1;
            this.groupLocalFile.TabStop = false;
            this.btnBrowse.Location = new System.Drawing.Point(382, 23);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Gözat";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            this.txtPath.Location = new System.Drawing.Point(59, 24);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(317, 22);
            this.txtPath.TabIndex = 1;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Dizin:";
            this.groupURL.Controls.Add(this.txtURL);
            this.groupURL.Controls.Add(this.lblURL);
            this.groupURL.Enabled = false;
            this.groupURL.Location = new System.Drawing.Point(12, 139);
            this.groupURL.Name = "groupURL";
            this.groupURL.Size = new System.Drawing.Size(479, 75);
            this.groupURL.TabIndex = 3;
            this.groupURL.TabStop = false;
            this.radioLocalFile.AutoSize = true;
            this.radioLocalFile.Checked = true;
            this.radioLocalFile.Location = new System.Drawing.Point(12, 12);
            this.radioLocalFile.Name = "radioLocalFile";
            this.radioLocalFile.Size = new System.Drawing.Size(150, 17);
            this.radioLocalFile.TabIndex = 0;
            this.radioLocalFile.TabStop = true;
            this.radioLocalFile.Text = "Yerel Dosyadan Güncelle";
            this.radioLocalFile.UseVisualStyleBackColor = true;
            this.radioLocalFile.CheckedChanged += new System.EventHandler(this.radioLocalFile_CheckedChanged);
            this.radioURL.AutoSize = true;
            this.radioURL.Location = new System.Drawing.Point(12, 116);
            this.radioURL.Name = "radioURL";
            this.radioURL.Size = new System.Drawing.Size(116, 17);
            this.radioURL.TabIndex = 2;
            this.radioURL.Text = "URL\'den Güncelle";
            this.radioURL.UseVisualStyleBackColor = true;
            this.radioURL.CheckedChanged += new System.EventHandler(this.radioURL_CheckedChanged);
            this.AcceptButton = this.btnUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 275);
            this.Controls.Add(this.radioURL);
            this.Controls.Add(this.radioLocalFile);
            this.Controls.Add(this.lblInformation);
            this.Controls.Add(this.groupURL);
            this.Controls.Add(this.groupLocalFile);
            this.Controls.Add(this.btnUpdate);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Güncelle []";
            this.Load += new System.EventHandler(this.FrmUpdate_Load);
            this.groupLocalFile.ResumeLayout(false);
            this.groupLocalFile.PerformLayout();
            this.groupURL.ResumeLayout(false);
            this.groupURL.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.Label lblInformation;
        private System.Windows.Forms.GroupBox groupLocalFile;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupURL;
        private System.Windows.Forms.RadioButton radioLocalFile;
        private System.Windows.Forms.RadioButton radioURL;
        private System.Windows.Forms.Button btnBrowse;
    }
}