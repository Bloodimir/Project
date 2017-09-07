using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Kütüphane_Takip_Programı
{
    public partial class okuyucuekle : Form
    {
        private readonly OleDbConnection baglan =
            new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath +
                                "\\mdb\\kutuphane.mdb");

        public int durum;
        public int gelen_id;

        public okuyucuekle()
        {
            InitializeComponent();
        }

        public void veri_oku()
        {
            int drm;
            if (baglan.State == ConnectionState.Closed) baglan.Open();
            var komut = new OleDbCommand("Select * from okuyucular where id=" + gelen_id + "", baglan);
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                textBox1.Text = oku.GetString(1);
                textBox2.Text = oku.GetString(2);
                textBox3.Text = oku.GetString(3);
                textBox8.Text = oku.GetString(4);
                comboBox1.Text = oku.GetString(5);
                drm = oku.GetInt32(7);

                textBox10.Text = oku.GetString(6);
                if (drm == 0)
                {
                    comboBox2.Text = "PASİF";
                }
                if (drm == 1)
                {
                    comboBox2.Text = "AKTİF";
                }
            }

            baglan.Close();
        }

        private void okuyucuekle_Load(object sender, EventArgs e)
        {
            var frm1 = (Form1) Application.OpenForms["Form1"];

            if (frm1.say2 == 0)
            {
            }
            else
            {
                gelen_id = Convert.ToInt32(frm1.dataGridView2.SelectedRows[0].Cells[0].Value);
                veri_oku();
            }

            if (baglan.State == ConnectionState.Closed) baglan.Open();
            var sorgu = "Select COUNT(id) From emanet where tckimlikno= '" + textBox1.Text + "' and durum=1";
            var kom = new OleDbCommand(sorgu, baglan);
            var gelen_count = Convert.ToInt32(kom.ExecuteScalar());
            textBox11.Text = gelen_count.ToString();
            baglan.Close();

            if (baglan.State == ConnectionState.Closed) baglan.Open();
            var sorgu2 = "Select COUNT(id) From emanet where tckimlikno= '" + textBox1.Text + "' and durum=0";
            var kom2 = new OleDbCommand(sorgu2, baglan);
            var gelen_count2 = Convert.ToInt32(kom2.ExecuteScalar());
            textBox9.Text = gelen_count2.ToString();
            baglan.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int durum;

                if (comboBox2.SelectedIndex == 0)
                {
                    durum = 1;
                }
                else
                {
                    durum = 0;
                }

                if (baglan.State == ConnectionState.Closed) baglan.Open();
                var kaydet =
                    new OleDbCommand(
                        "insert into okuyucular (tckimlikno,adi,soyadi,utarihi,cinsiyet,adres,durum) values('" +
                        textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox8.Text + "','" +
                        comboBox1.Text + "','" + textBox10.Text + "','" + durum + "')", baglan);
                kaydet.ExecuteNonQuery();
                MessageBox.Show("Okuyucu Başarılı Bir Şekilde Eklendi...");

                baglan.Close();
            }
            catch
            {
                MessageBox.Show("Lütfen Bilgilerinizi Kontrol Ediniz...");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int durum;

            if (comboBox2.SelectedIndex == 0)
            {
                durum = 1;
            }
            else
            {
                durum = 0;
            }

            var frm1 = (Form1) Application.OpenForms["Form1"];
            var guncelle =
                new OleDbCommand(
                    "update okuyucular set tckimlikno=" + textBox1.Text + ",adi='" + textBox2.Text + "',soyadi='" +
                    textBox3.Text + "',utarihi='" + textBox8.Text + "',cinsiyet='" + comboBox1.Text + "',durum=" + durum +
                    " ,adres='" + textBox10.Text + "' where id=" + gelen_id + "", baglan);
            baglan.Open();
            guncelle.ExecuteNonQuery();
            MessageBox.Show("Kayıt Güncelleme İşlemi Başarılı Bir Şekilde Gerçekleştirildi", "Kütüphane Takip Programı",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            frm1.okuyucu_oku();
            frm1.dataGridView1.Visible = false;
            frm1.dataGridView2.Visible = true;
            baglan.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var frm1 = (Form1) Application.OpenForms["Form1"];
            if (baglan.State == ConnectionState.Closed) baglan.Open();
            var sil = new OleDbCommand("delete from okuyucular where id =" + gelen_id + "", baglan);

            sil.ExecuteNonQuery();
            MessageBox.Show("Kayıt Silme İşlemi Başarılı Bir Şekilde Gerçekleştirildi", "Kütüphane Takip Programı",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            frm1.okuyucu_oku();
            baglan.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var frm1 = (Form1) Application.OpenForms["Form1"];
            frm1.dataGridView2.Visible = false;
            frm1.dataGridView1.Visible = false;
            frm1.dataGridView4.Visible = false;
            frm1.dataGridView3.Visible = false;
            frm1.dataGridView5.Visible = true;
            frm1.Show();
            frm1.Gdurum = 0;
            frm1.okunan_kitaplar();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var frm1 = (Form1) Application.OpenForms["Form1"];
            frm1.dataGridView2.Visible = false;
            frm1.dataGridView1.Visible = false;
            frm1.dataGridView4.Visible = false;
            frm1.dataGridView3.Visible = false;
            frm1.dataGridView5.Visible = true;
            frm1.Show();
            frm1.Gdurum = 1;
            frm1.okunan_kitaplar();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "Karşıyaka Lisesi";
            textBox11.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}