using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using Kütüphane_Takip_Programı;
 

namespace Kütüphane_Takip_Programı
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();        
        }
        private static bool çıkış = true;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && çıkış)
                System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath.ToString() + "\\mdb\\kutuphane.mdb");

        OleDbCommand komut = new OleDbCommand();
        OleDbCommand komut2 = new OleDbCommand();
        OleDbCommand komut3 = new OleDbCommand();
        OleDbDataAdapter adaptor = new OleDbDataAdapter();
        DataSet ds = new DataSet();
        
       

        public void kitap_oku()
        {

            if (baglan.State == ConnectionState.Closed) baglan.Open();
            ds.Clear();
            adaptor = new OleDbDataAdapter("Select * From kitaplar ", baglan);
            adaptor.Fill(ds, "kitaplar");
            dataGridView1.DataSource = ds.Tables["kitaplar"];
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Barkod No";
            dataGridView1.Columns[2].HeaderText = "Kitap Adı"; 
            dataGridView1.Columns[3].HeaderText = "Yazar Adı";
            dataGridView1.Columns[4].HeaderText = "Yayın Evi";
            dataGridView1.Columns[5].HeaderText = "Baskı Tarihi";
            dataGridView1.Columns[6].HeaderText = "Dolap No";
            dataGridView1.Columns[7].HeaderText = "Stok";
            dataGridView1.Columns[0].Width = 62;
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].Width = 320;
            dataGridView1.Columns[3].Width = 165;
            dataGridView1.Columns[4].Width = 115;
            dataGridView1.Columns[5].Width = 70;
            dataGridView1.Columns[6].Width = 80;
            dataGridView1.Columns[7].Width = 40;

            baglan.Close();
        }

        public void zimmet_oku()
        {
            if (baglan.State == ConnectionState.Closed) baglan.Open();
            ds.Clear();
            adaptor = new OleDbDataAdapter("Select emanet.id,okuyucular.tckimlikno,okuyucular.adi,okuyucular.soyadi,kitaplar.barkodno,kitaplar.kitapadi,kitaplar.yazaradi,emanet.bastarihi,emanet.bittarihi From okuyucular,kitaplar,emanet where okuyucular.tckimlikno=emanet.tckimlikno and kitaplar.barkodno=emanet.kitapbarkod and emanet.durum=1 ", baglan);
            adaptor.Fill(ds, "okuyucular,kitaplar,emanet");
            dataGridView3.DataSource = ds.Tables["okuyucular,kitaplar,emanet"];

            dataGridView3.Columns[0].HeaderText = "ID";
            dataGridView3.Columns[4].HeaderText = "Barkod No";
            dataGridView3.Columns[1].HeaderText = "Okul No";
            dataGridView3.Columns[2].HeaderText = "Adı";
            dataGridView3.Columns[3].HeaderText = "Soyadı";
            dataGridView3.Columns[5].HeaderText = "Kitap Adı";
            dataGridView3.Columns[6].HeaderText = "Yazar Adı";
            dataGridView3.Columns[7].HeaderText = "Başlangıç Tarihi";
            dataGridView3.Columns[8].HeaderText = "Bitiş Tarihi";
            dataGridView3.Columns[0].Width = 60;
            dataGridView3.Columns[1].Width = 85;
            dataGridView3.Columns[2].Width = 95;
            dataGridView3.Columns[3].Width = 95;
            dataGridView3.Columns[4].Width = 85;
            dataGridView3.Columns[5].Width = 205;
            dataGridView3.Columns[6].Width = 140;
            dataGridView3.Columns[7].Width = 100;
            dataGridView3.Columns[8].Width = 100;



            baglan.Close();
        }

        public void okuyucu_oku()
        {
            if (baglan.State == ConnectionState.Closed) baglan.Open();
            ds.Clear();
            adaptor = new OleDbDataAdapter("Select * From okuyucular ", baglan);
            adaptor.Fill(ds, "okuyucular");
            dataGridView2.DataSource = ds.Tables["okuyucular"];

            dataGridView2.Columns[0].HeaderText = "ID";
            dataGridView2.Columns[1].HeaderText = "Okul No";
            dataGridView2.Columns[2].HeaderText = "Adı";
            dataGridView2.Columns[3].HeaderText = "Soyadı";
            dataGridView2.Columns[4].HeaderText = "Uye Tarihi";
            dataGridView2.Columns[5].HeaderText = "Cinsiyet";   
            dataGridView2.Columns[6].HeaderText = "Adres";
            dataGridView2.Columns[7].HeaderText = "Durum";

            dataGridView2.Columns[0].Width = 60;
            dataGridView2.Columns[1].Width = 100;
            dataGridView2.Columns[2].Width = 150;
            dataGridView2.Columns[3].Width = 150;
            dataGridView2.Columns[4].Width = 100;
            dataGridView2.Columns[5].Width = 100;
            dataGridView2.Columns[6].Width = 200;
            dataGridView2.Columns[7].Width = 100;

            baglan.Close();
        }
        public int Gdurum;
        public void okunan_kitaplar()
        {
            Form1 frm1 = (Form1)Application.OpenForms["Form1"];

            OleDbCommand komut = new OleDbCommand();
            OleDbDataAdapter adaptor = new OleDbDataAdapter();
            DataSet ds = new DataSet();

            if (baglan.State == ConnectionState.Closed) baglan.Open();
            ds.Clear();
            adaptor = new OleDbDataAdapter("Select emanet.id,okuyucular.tckimlikno,okuyucular.adi,okuyucular.soyadi,kitaplar.barkodno,kitaplar.kitapadi,kitaplar.yazaradi,emanet.bastarihi,emanet.bittarihi,emanet.gdurum From okuyucular,kitaplar,emanet where okuyucular.tckimlikno=emanet.tckimlikno and kitaplar.barkodno=emanet.kitapbarkod and emanet.durum= " + Gdurum + " ", baglan);
            adaptor.Fill(ds, "okuyucular,kitaplar,emanet");
            frm1.dataGridView5.DataSource = ds.Tables["okuyucular,kitaplar,emanet"];

            frm1.dataGridView5.Columns[0].HeaderText = "ID";
            frm1.dataGridView5.Columns[4].HeaderText = "Barkod No";
            frm1.dataGridView5.Columns[1].HeaderText = "Okul No";
            frm1.dataGridView5.Columns[2].HeaderText = "Adı";
            frm1.dataGridView5.Columns[3].HeaderText = "Soyadı";
            frm1.dataGridView5.Columns[5].HeaderText = "Kitap Adı";
            frm1.dataGridView5.Columns[6].HeaderText = "Yazar Adı";
            frm1.dataGridView5.Columns[7].HeaderText = "Başlangıç Tarihi";
            frm1.dataGridView5.Columns[8].HeaderText = "Bitiş Tarihi";
            frm1.dataGridView5.Columns[9].HeaderText = "Durum";

            frm1.dataGridView5.Columns[0].Width = 50;
            frm1.dataGridView5.Columns[1].Width = 85;
            frm1.dataGridView5.Columns[2].Width = 110;
            frm1.dataGridView5.Columns[3].Width = 110;
            frm1.dataGridView5.Columns[4].Width = 80;
            frm1.dataGridView5.Columns[4].Width = 150;
            frm1.dataGridView5.Columns[6].Width = 120;
            frm1.dataGridView5.Columns[7].Width = 90;
            frm1.dataGridView5.Columns[8].Width = 90;
            frm1.dataGridView5.Columns[9].Width = 85;
            baglan.Close();

        }
        public int kontrol2 = 1;
        string tarih;
        private void Form1_Load(object sender, EventArgs e)
        {
            button1.PerformClick();
            SayArtıkOç();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            dataGridView4.Visible = false;
            dataGridView5.Visible = false;
            dataGridView1.Visible = true;

            comboBox1.Items.Clear();
            comboBox1.Text = "";

            comboBox1.Items.Add("Kitap Adına Göre");
            comboBox1.Items.Add("Yazara Göre");
            comboBox1.Items.Add("Barkod Numarasına Göre");

            kitap_oku();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            dataGridView3.Visible = false;
            dataGridView4.Visible = false;
            dataGridView5.Visible = false;
            dataGridView2.Visible = true;

            comboBox1.Items.Clear();
            comboBox1.Text = "";
            comboBox1.Items.Add("Okul Numarasına Göre");
            okuyucu_oku();
        }

        public void SayArtıkOç()
        {
            int anan;
            int anan2;
            int anan3;
            baglan.Open();

            komut.Connection = baglan;
            komut.CommandText = "select Count (*) From kitaplar WHERE stok > 1";

            komut2.Connection = baglan;
            komut2.CommandText = "select Count (*) From okuyucular";

            komut3.Connection = baglan;
            komut3.CommandText = "select Count (*) From kitaplar WHERE stok = 0";

            anan = int.Parse(komut.ExecuteScalar().ToString());
            anan2 = int.Parse(komut2.ExecuteScalar().ToString());
            anan3 = int.Parse(komut3.ExecuteScalar().ToString());

            label7.Text = "Toplam Kitap Sayısı: " + anan.ToString() + anan3.ToString();
            label8.Text = "Toplam Öğrenci Sayısı: " + anan2.ToString();
            baglan.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            say = 0;
            Kitapekle kitapekle = new Kitapekle();
            kitapekle.button2.Enabled = false;
            kitapekle.button1.Enabled = true;

            kitapekle.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            say2 = 0;
            okuyucuekle okuyucuekle = new okuyucuekle();
            okuyucuekle.button1.Enabled = true;
            okuyucuekle.button2.Enabled = false;
            okuyucuekle.Show();
        }
        public int say = 0;
        public int say2 = 0;
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

            Kitapekle kitapekle = new Kitapekle();
            say = say + 1;
            kitapekle.button1.Enabled = false;

            kitapekle.ShowDialog();

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            okuyucuekle öğrenciekle = new okuyucuekle();
            say2 = say2 + 1;
            öğrenciekle.button1.Enabled = false;
            öğrenciekle.ShowDialog();
        }

        private void dataGridView2_Click(object sender, EventArgs e)
        {
            textBox4.Text = dataGridView2.SelectedRows[0].Cells[2].Value.ToString() + dataGridView2.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int stkno = 0;

                if (baglan.State == ConnectionState.Closed) baglan.Open();
                OleDbCommand komut = new OleDbCommand("Select stok from kitaplar where barkodno='" + textBox1.Text + "'", baglan);
                OleDbDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    stkno = oku.GetInt32(0);
                }
                baglan.Close();

                if (stkno == 0)
                {
                    MessageBox.Show("Bu Kitap Stokta Bulunmamaktadır...");
                }

                else
                {
                    if ((textBox1.Text != ""))
                    {
                        kitapver ktpver = new kitapver();
                        ktpver.ShowDialog();
                    }


                    else
                    {

                        MessageBox.Show("Öncelikle Kitap ve Öğrenci Seçiniz...");
                    }
                }
            }  
            catch
            {

                MessageBox.Show("Öncelikle Kitap ve Öğrenci Seçiniz...");

            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = false;
            dataGridView4.Visible = false;
            dataGridView5.Visible = false;
            dataGridView3.Visible = true;

            comboBox1.Items.Clear();
            comboBox1.Text = "";

            comboBox1.Items.Add("Kitap Barkod Numarasına Göre");
            comboBox1.Items.Add("Okul Numarasına Göre");
            comboBox1.Items.Add("Emanet Bitiş Tarihine Göre");

            zimmet_oku();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView3.Visible == true)
            {
                kitapteslimal ktpal = new kitapteslimal();
                ktpal.ShowDialog();
            }
            else
            {
                MessageBox.Show("Öncelikle Emanet Kitaplar Düğmesine Tıklayınız ve Açılan Listeden Teslim  Edecek Öğrenciyi Seçiniz...  ", "Kütüphane Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dataGridView3_DoubleClick(object sender, EventArgs e)
        {
            kitapteslimal ktpteslim = new kitapteslimal();

            ktpteslim.ShowDialog();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Öncelikle Arama Türü Seçmelisiniz...", "Kütüphane Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                if (dataGridView1.Visible == true)
                {
                    if (comboBox1.Text == "Kitap Adına Göre")
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        ds.Clear();
                        adaptor = new OleDbDataAdapter("Select * from kitaplar where kitapadi Like'" + textBox5.Text + "%'", baglan);
                        adaptor.Fill(ds, "kitaplar");
                        dataGridView1.DataSource = ds.Tables["kitaplar"];
                        baglan.Close();
                    }

                    if (comboBox1.Text == "Yazara Göre")
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        ds.Clear();
                        adaptor = new OleDbDataAdapter("Select * from kitaplar where yazaradi Like'" + textBox5.Text + "%'", baglan);
                        adaptor.Fill(ds, "kitaplar");
                        dataGridView1.DataSource = ds.Tables["kitaplar"];
                        baglan.Close();
                    }

                    if (comboBox1.Text == "Barkod Numarasına Göre")
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        ds.Clear();
                        adaptor = new OleDbDataAdapter("Select * from kitaplar where barkodno Like'" + textBox5.Text + "%'", baglan);
                        adaptor.Fill(ds, "kitaplar");
                        dataGridView1.DataSource = ds.Tables["kitaplar"];
                        baglan.Close();
                    }

                }

                if (dataGridView2.Visible == true)
                {
                    OleDbDataAdapter adaptor = new OleDbDataAdapter();
                    if (comboBox1.Text == "Okul Numarasına Göre")
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        ds.Clear();
                        adaptor = new OleDbDataAdapter("Select * from okuyucular where tckimlikno Like'" + textBox5.Text + "%'", baglan);
                        adaptor.Fill(ds, "okuyucular");
                        dataGridView2.DataSource = ds.Tables["okuyucular"];
                        baglan.Close();
                    }

                }

                if (dataGridView3.Visible == true)
                {
                    if (comboBox1.Text == "Kitap Barkod Numarasına Göre")
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        ds.Clear();
                        adaptor = new OleDbDataAdapter("Select emanet.id,okuyucular.tckimlikno,okuyucular.adi,okuyucular.soyadi,kitaplar.barkodno,kitaplar.kitapadi,kitaplar.yazaradi,emanet.bastarihi,emanet.bittarihi From okuyucular,kitaplar,emanet where okuyucular.tckimlikno=emanet.tckimlikno and kitaplar.barkodno=emanet.kitapbarkod and emanet.durum=1 and emanet.kitapbarkod Like'" + textBox5.Text + "%' ", baglan);
                        adaptor.Fill(ds, "okuyucular,kitaplar,emanet");
                        dataGridView3.DataSource = ds.Tables["okuyucular,kitaplar,emanet"];
                        baglan.Close();
                    }

                    if (comboBox1.Text == "Okul Numarasına Göre")
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        ds.Clear();
                        adaptor = new OleDbDataAdapter("Select emanet.id,okuyucular.tckimlikno,okuyucular.adi,okuyucular.soyadi,kitaplar.barkodno,kitaplar.kitapadi,kitaplar.yazaradi,emanet.bastarihi,emanet.bittarihi From okuyucular,kitaplar,emanet where okuyucular.tckimlikno=emanet.tckimlikno and kitaplar.barkodno=emanet.kitapbarkod and emanet.durum=1 and emanet.tckimlikno Like'" + textBox5.Text + "%' ", baglan);
                        adaptor.Fill(ds, "okuyucular,kitaplar,emanet");
                        dataGridView3.DataSource = ds.Tables["okuyucular,kitaplar,emanet"];
                        baglan.Close();
                    }

                    if (comboBox1.Text == "Emanet Bitiş Tarihine Göre")
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        ds.Clear();
                        adaptor = new OleDbDataAdapter("Select emanet.id,okuyucular.tckimlikno,okuyucular.adi,okuyucular.soyadi,kitaplar.barkodno,kitaplar.kitapadi,kitaplar.yazaradi,emanet.bastarihi,emanet.bittarihi From okuyucular,kitaplar,emanet where okuyucular.tckimlikno=emanet.tckimlikno and kitaplar.barkodno=emanet.kitapbarkod and emanet.durum=1 and emanet.bittarihi Like'" + textBox5.Text + "%' ", baglan);
                        adaptor.Fill(ds, "okuyucular,kitaplar,emanet");
                        dataGridView3.DataSource = ds.Tables["okuyucular,kitaplar,emanet"];
                        baglan.Close();
                    }
                    dataGridView3.Columns[0].HeaderText = "ID";
                    dataGridView3.Columns[4].HeaderText = "Barkod No";
                    dataGridView3.Columns[1].HeaderText = "Okul kNo";
                    dataGridView3.Columns[2].HeaderText = "Adı";
                    dataGridView3.Columns[3].HeaderText = "Soyadı";
                    dataGridView3.Columns[5].HeaderText = "Kitap Adı";
                    dataGridView3.Columns[6].HeaderText = "Yazar Adı";
                    dataGridView3.Columns[9].HeaderText = "Başlangıç Tarihi";
                    dataGridView3.Columns[10].HeaderText = "Bitiş Tarihi";


                    dataGridView3.Columns[0].Width = 60;
                    dataGridView3.Columns[1].Width = 110;
                    dataGridView3.Columns[2].Width = 110;
                    dataGridView3.Columns[3].Width = 90;
                    dataGridView3.Columns[4].Width = 90;
                    dataGridView3.Columns[5].Width = 190;
                    dataGridView3.Columns[6].Width = 130;
                    dataGridView3.Columns[7].Width = 90;
                    dataGridView3.Columns[8].Width = 90;
                }




            }


        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = false;
            dataGridView3.Visible = false;
            dataGridView5.Visible = false;
            dataGridView4.Visible = true;

            tarih = DateTime.Now.ToShortDateString();
            if (baglan.State == ConnectionState.Closed) baglan.Open();
            ds.Clear();
            adaptor = new OleDbDataAdapter("Select emanet.id,okuyucular.tckimlikno,okuyucular.adi,okuyucular.soyadi,kitaplar.barkodno,kitaplar.kitapadi,kitaplar.yazaradi,emanet.bastarihi,emanet.bittarihi From okuyucular,kitaplar,emanet where okuyucular.tckimlikno=emanet.tckimlikno and kitaplar.barkodno=emanet.kitapbarkod and emanet.durum=1 and emanet.bittarihi < '" + tarih + "'", baglan);
            adaptor.Fill(ds, "okuyucular,kitaplar,emanet");
            dataGridView4.DataSource = ds.Tables["okuyucular,kitaplar,emanet"];

            dataGridView4.Columns[0].HeaderText = "ID";
            dataGridView4.Columns[4].HeaderText = "Barkod No";
            dataGridView4.Columns[1].HeaderText = "Okul No";
            dataGridView4.Columns[2].HeaderText = "Adı";
            dataGridView4.Columns[3].HeaderText = "Soyadı";
            dataGridView4.Columns[5].HeaderText = "Kitap Adı";
            dataGridView4.Columns[6].HeaderText = "Yazar Adı";
            dataGridView4.Columns[7].HeaderText = "Başlangıç Tarihi";
            dataGridView4.Columns[8].HeaderText = "Bitiş Tarihi";


            dataGridView4.Columns[0].Width = 50;
            dataGridView4.Columns[1].Width = 65;
            dataGridView4.Columns[2].Width = 130;
            dataGridView4.Columns[3].Width = 130;
            dataGridView4.Columns[4].Width = 65;
            dataGridView4.Columns[5].Width = 180;
            dataGridView4.Columns[6].Width = 150;
            dataGridView4.Columns[7].Width = 100;
            dataGridView4.Columns[8].Width = 100;



            baglan.Close();
        }



        private void dataGridView5_DoubleClick(object sender, EventArgs e)
        {
            if (Gdurum == 1)
            {
                kitapteslimal ktpteslim = new kitapteslimal();

                ktpteslim.ShowDialog();

            }
        }

        private void dataGridView4_DoubleClick(object sender, EventArgs e)
        {
            kitapteslimal ktpteslim = new kitapteslimal();

            ktpteslim.ShowDialog();
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (System.IO.File.Exists(saveFileDialog1.FileName))
                    {
                        System.IO.File.Delete(saveFileDialog1.FileName);
                    }
                    System.IO.File.Copy(Application.StartupPath.ToString() + "\\mdb\\kutuphane.mdb", saveFileDialog1.FileName);
                    MessageBox.Show("Yedek alma işlemi tamamlandı !");
                }
                else
                {
                    MessageBox.Show("Yedekle işlemi iptal edildi !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    

        private void button10_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (System.IO.File.Exists(Application.StartupPath.ToString() + "\\mdb\\kutuphane.mdb"))
                {
                    System.IO.File.Delete(Application.StartupPath.ToString() + "\\mdb\\kutuphane.mdb");
                }
                System.IO.File.Copy(openFileDialog1.FileName, Application.StartupPath.ToString() + "\\mdb\\kutuphane.mdb");
                MessageBox.Show("Geri yükleme işlemi tamamlandı !");
            }
            else
            {
                MessageBox.Show("Geri yükleme işlemi iptal edildi !");
            }
        }
    

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            var frm1 = new Not();
            frm1.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            var frm1 = new Yardım();
            frm1.Show();
            this.Hide();
        }
    }
}
