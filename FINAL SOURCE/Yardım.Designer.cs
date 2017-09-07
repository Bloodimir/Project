namespace Kütüphane_Takip_Programı
{
    partial class Yardım
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Yardım));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnKabulEt = new System.Windows.Forms.Button();
            this.chkOkuma = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(660, 66);
            this.label1.TabIndex = 43;
            this.label1.Text = "1-) Kitapları Sağınızda Bulunan Bilgisayarın Arkasındaki Dolaptan Alıp En Son Ver" +
    "ilen ID Numarasından Ardışık Olarak Eklemeye Devam Ediniz.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(12, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(660, 79);
            this.label2.TabIndex = 44;
            this.label2.Text = "2-) Emanet Kitap Eklemek İçin Öncellikle Kitap Listesinden Kitabı Seçmeli Daha So" +
    "nrada Okuyucu Listesinden Okuyucuyu Seçip Emaneti Kaydet Tuşuna Basmalısız.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label3.ForeColor = System.Drawing.Color.Maroon;
            this.label3.Location = new System.Drawing.Point(12, 219);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(660, 69);
            this.label3.TabIndex = 45;
            this.label3.Text = "3-) Kitap Eklerken Eklenmesi Zorunlu Olan Veriler Kitap Adı - Barkod No - Dolap N" +
    "o - Stok";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Cursor = System.Windows.Forms.Cursors.Default;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label6.ForeColor = System.Drawing.Color.Maroon;
            this.label6.Location = new System.Drawing.Point(-2, 345);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(674, 51);
            this.label6.TabIndex = 49;
            this.label6.Text = "5-) Kitap Eklerken \"Enter\" Tuşunu Kullanarak Ekleyebilirsiniz Mouse Gereksinimi Y" +
    "oktur.";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnKabulEt
            // 
            this.btnKabulEt.Enabled = false;
            this.btnKabulEt.Location = new System.Drawing.Point(553, 444);
            this.btnKabulEt.Name = "btnKabulEt";
            this.btnKabulEt.Size = new System.Drawing.Size(114, 80);
            this.btnKabulEt.TabIndex = 50;
            this.btnKabulEt.Text = "Kabul Et";
            this.btnKabulEt.UseVisualStyleBackColor = true;
            this.btnKabulEt.Click += new System.EventHandler(this.btnKabulEt_Click);
            // 
            // chkOkuma
            // 
            this.chkOkuma.AutoSize = true;
            this.chkOkuma.Location = new System.Drawing.Point(553, 422);
            this.chkOkuma.Name = "chkOkuma";
            this.chkOkuma.Size = new System.Drawing.Size(115, 17);
            this.chkOkuma.TabIndex = 51;
            this.chkOkuma.Text = "Okudum && Anladım";
            this.chkOkuma.UseVisualStyleBackColor = true;
            this.chkOkuma.CheckedChanged += new System.EventHandler(this.chkOkuma_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(552, 399);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(115, 17);
            this.checkBox1.TabIndex = 52;
            this.checkBox1.Text = "Bir Daha Gösterme";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label4.ForeColor = System.Drawing.Color.Green;
            this.label4.Location = new System.Drawing.Point(-1, 419);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(548, 105);
            this.label4.TabIndex = 46;
            this.label4.Text = "Programda Bir Sorun Çıkması Durumunda 11-B Sınıfına Gelebilirsiniz. \rOKAN UĞUR - " +
    "UMUT BERKAY KARA - YILMAZ DEMELİOĞLU ELVİN TURGUT - MİNA POYRAZ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label5.ForeColor = System.Drawing.Color.Green;
            this.label5.Location = new System.Drawing.Point(12, 279);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(660, 66);
            this.label5.TabIndex = 53;
            this.label5.Text = "4-) Bir Öğrenci Kitap Alırken Öncelikle Öğrenci Listesinde Kaydı Yok İse Önce Kay" +
    "ıt Açıp Öyle Emaneti Kaydediniz.";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Yardım
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(679, 541);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.chkOkuma);
            this.Controls.Add(this.btnKabulEt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(695, 575);
            this.MinimumSize = new System.Drawing.Size(695, 575);
            this.Name = "Yardım";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YÖNERGE - YARDIM VE DESTEK";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Yardım_FormClosing);
            this.Load += new System.EventHandler(this.yardım_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnKabulEt;
        private System.Windows.Forms.CheckBox chkOkuma;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;


    }
}