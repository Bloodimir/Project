using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Kütüphane_Takip_Programı
{
    public partial class kitapver : Form
    {
        private readonly OleDbConnection baglan =
            new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath +
                                "\\mdb\\kutuphane.mdb");

        public kitapver()
        {
            InitializeComponent();
        }

        private void kitapver_Load(object sender, EventArgs e)
        {
            var frm1 = (Form1) Application.OpenForms["Form1"];
            textBox1.Text = frm1.textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var frm1 = (Form1) Application.OpenForms["Form1"];
            try
            {
                var gdurum = "Teslim Edilmedi";
                if (baglan.State == ConnectionState.Closed) baglan.Open();
                var kaydet =
                    new OleDbCommand(
                        "insert into emanet (tckimlikno,kitapbarkod,bastarihi,bittarihi,durum,gdurum) values('" +
                        textBox2.Text + "','" + textBox1.Text + "','" + dateTimePicker1.Text + "','" +
                        dateTimePicker2.Text + "'," + 1 + " ,'" + gdurum + "')", baglan);
                kaydet.ExecuteNonQuery();
                baglan.Close();


                if (baglan.State == ConnectionState.Closed) baglan.Open();
                var guncelle =
                    new OleDbCommand("update kitaplar set stok= stok-1 where barkodno='" + textBox1.Text + "'", baglan);
                guncelle.ExecuteNonQuery();
                baglan.Close();
                MessageBox.Show("Kayıt Başarılı Bir Şekilde Gerçeklerştirildi...", "Kütüphane Takip Programı",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                frm1.kitap_oku();
                frm1.dataGridView2.Visible = false;
                frm1.dataGridView1.Visible = true;
            }
            catch
            {
                MessageBox.Show("Lütfen Bilgilerinizi Kontrol Ediniz...");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }
    }
}