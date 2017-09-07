using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Kütüphane_Takip_Programı
{
    public partial class kitapteslimal : Form
    {
        private readonly OleDbConnection baglan =
            new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath +
                                "\\mdb\\kutuphane.mdb");

        public int gelenid;

        public kitapteslimal()
        {
            InitializeComponent();
        }

        private void kitapteslimal_Load(object sender, EventArgs e)
        {
            var frm1 = (Form1) Application.OpenForms["Form1"];
            if (frm1.dataGridView5.Visible)
            {
                textBox1.Text = frm1.dataGridView5.SelectedRows[0].Cells[4].Value.ToString();
                textBox2.Text = frm1.dataGridView5.SelectedRows[0].Cells[1].Value.ToString();
                gelenid = Convert.ToInt32(frm1.dataGridView5.SelectedRows[0].Cells[0].Value);
            }
            else if (frm1.dataGridView4.Visible)
            {
                textBox1.Text = frm1.dataGridView4.SelectedRows[0].Cells[4].Value.ToString();
                textBox2.Text = frm1.dataGridView4.SelectedRows[0].Cells[1].Value.ToString();
                gelenid = Convert.ToInt32(frm1.dataGridView4.SelectedRows[0].Cells[0].Value);
            }
            else
            {
                textBox1.Text = frm1.dataGridView3.SelectedRows[0].Cells[4].Value.ToString();
                textBox2.Text = frm1.dataGridView3.SelectedRows[0].Cells[1].Value.ToString();
                gelenid = Convert.ToInt32(frm1.dataGridView5.SelectedRows[0].Cells[0].Value);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var frm1 = (Form1) Application.OpenForms["Form1"];
            var gdurum = "Teslim Edildi";
            if (baglan.State == ConnectionState.Closed) baglan.Open();
            var guncelle2 =
                new OleDbCommand(
                    "update emanet set teslimtarihi = '" + dateTimePicker1.Text + "' , durum = " + 0 + " , gdurum = '" +
                    gdurum + "' where id=" + gelenid + "", baglan);
            guncelle2.ExecuteNonQuery();
            baglan.Close();

            if (baglan.State == ConnectionState.Closed) baglan.Open();
            var guncelle = new OleDbCommand("update kitaplar set stok= stok+1 where barkodno='" + textBox1.Text + "'",
                baglan);
            guncelle.ExecuteNonQuery();
            baglan.Close();
            MessageBox.Show("Kayıt Başarılı Bir Şekilde Gerçekleştirildi...");
            frm1.kitap_oku();
            frm1.dataGridView2.Visible = false;
            frm1.dataGridView1.Visible = false;
            frm1.dataGridView3.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
