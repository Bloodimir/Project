namespace xServer.Formlar
{
    partial class FrmBaşlangıcaEkle
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBaşlangıcaEkle));
            this.groupAutostartItem = new System.Windows.Forms.GroupBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupAutostartItem.SuspendLayout();
            this.SuspendLayout();
            this.groupAutostartItem.Controls.Add(this.lblType);
            this.groupAutostartItem.Controls.Add(this.cmbType);
            this.groupAutostartItem.Controls.Add(this.txtPath);
            this.groupAutostartItem.Controls.Add(this.txtName);
            this.groupAutostartItem.Controls.Add(this.lblPath);
            this.groupAutostartItem.Controls.Add(this.lblName);
            this.groupAutostartItem.Location = new System.Drawing.Point(12, 12);
            this.groupAutostartItem.Name = "groupAutostartItem";
            this.groupAutostartItem.Size = new System.Drawing.Size(653, 105);
            this.groupAutostartItem.TabIndex = 0;
            this.groupAutostartItem.TabStop = false;
            this.groupAutostartItem.Text = "Oto-Başlangıç Öğesi";
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(40, 74);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(25, 13);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Tip:";
            this.lblType.Click += new System.EventHandler(this.lblType_Click);
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(74, 71);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(573, 21);
            this.cmbType.TabIndex = 5;
            this.toolTip1.SetToolTip(this.cmbType, "Oto-Başlangıç Öğesinin Uzak Tipi.");
            this.txtPath.Location = new System.Drawing.Point(74, 43);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(573, 22);
            this.txtPath.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtPath, "Oto-Başlangıç Öğesinin Uzak Dizini.");
            this.txtPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPath_KeyPress);
            this.txtName.Location = new System.Drawing.Point(74, 15);
            this.txtName.MaxLength = 64;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(573, 22);
            this.txtName.TabIndex = 1;
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
  
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(35, 46);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(36, 13);
            this.lblPath.TabIndex = 2;
            this.lblPath.Text = "Dizin:";
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(40, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(30, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "İsim:";
            this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAdd.Location = new System.Drawing.Point(576, 123);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(89, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "&Ekle";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(449, 123);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click); 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(677, 158);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupAutostartItem);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddToAutostart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Oto-Başlangıca Ekle";
            this.groupAutostartItem.ResumeLayout(false);
            this.groupAutostartItem.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupAutostartItem;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}