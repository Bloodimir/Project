namespace xServer.Formlar
{
    partial class FrmYükleÇalıştır
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmYükleÇalıştır));
            this.btnUploadAndExecute = new System.Windows.Forms.Button();
            this.chkRunHidden = new System.Windows.Forms.CheckBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.btnUploadAndExecute.Location = new System.Drawing.Point(273, 37);
            this.btnUploadAndExecute.Name = "btnUploadAndExecute";
            this.btnUploadAndExecute.Size = new System.Drawing.Size(111, 23);
            this.btnUploadAndExecute.TabIndex = 4;
            this.btnUploadAndExecute.Text = "Yükle && Çalıştır";
            this.btnUploadAndExecute.UseVisualStyleBackColor = true;
            this.btnUploadAndExecute.Click += new System.EventHandler(this.btnUploadAndExecute_Click);
            this.chkRunHidden.AutoSize = true;
            this.chkRunHidden.Location = new System.Drawing.Point(50, 41);
            this.chkRunHidden.Name = "chkRunHidden";
            this.chkRunHidden.Size = new System.Drawing.Size(128, 17);
            this.chkRunHidden.TabIndex = 2;
            this.chkRunHidden.Text = "Dosyayı Gizli Çalıştır";
            this.chkRunHidden.UseVisualStyleBackColor = true;
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(12, 9);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(36, 13);
            this.lblPath.TabIndex = 0;
            this.lblPath.Text = "Dizin:";
            this.txtPath.Location = new System.Drawing.Point(51, 6);
            this.txtPath.MaxLength = 300;
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(333, 22);
            this.txtPath.TabIndex = 1;
            this.btnBrowse.Location = new System.Drawing.Point(184, 37);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(83, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Gözat";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 72);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.chkRunHidden);
            this.Controls.Add(this.btnUploadAndExecute);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUploadAndExecute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Yükle & Çalıştır []";
            this.Load += new System.EventHandler(this.FrmUploadAndExecute_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUploadAndExecute;
        private System.Windows.Forms.CheckBox chkRunHidden;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnBrowse;
    }
}