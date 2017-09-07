using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Kütüphane_Takip_Programı
{
    public partial class Kitapekle : Form
    {
        private readonly OleDbConnection baglan =
            new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath +
                                "\\mdb\\kutuphane.mdb");

        public int b;
        private int gelen_id;


        public Kitapekle()
        {
            InitializeComponent();
        }

        public void veri_oku()
        {
            
            int stk;
            if (baglan.State == ConnectionState.Closed) baglan.Open();
            var komut = new OleDbCommand("Select * from kitaplar where id=" + gelen_id + "", baglan);
            var oku = komut.ExecuteReader();
            while (oku.Read())
            {
                textBox1.Text = oku.GetString(1);
                textBox2.Text = oku.GetString(2);
                textBox3.Text = oku.GetString(3);
                textBox4.Text = oku.GetString(4);
                textBox7.Text = oku.GetString(5);
                textBox5.Text = oku.GetString(6);
                stk = oku.GetInt32(7);
                textBox10.Text = stk.ToString();
            }

            baglan.Close();
        }
        public void kitapekle_Load(object sender, EventArgs e)
        {
            var frm1 = (Form1)Application.OpenForms["Form1"];

            if (frm1.say == 0)
            {
            }
            else
            {
                gelen_id = Convert.ToInt32(frm1.dataGridView1.SelectedRows[0].Cells[0].Value);
                veri_oku();
            }
        }
        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(SüreBitince,
                    null, timeout, System.Threading.Timeout.Infinite);
                MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void SüreBitince(object state)
            {
                IntPtr mbWnd = FindWindow("#32770", _caption);
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var frm1 = (Form1)Application.OpenForms["Form1"];
            try
            {
                if (baglan.State == ConnectionState.Closed) baglan.Open();
                var kaydet =
                    new OleDbCommand(
                        "insert into kitaplar (barkodno,kitapadi,yazaradi,yayinevi,baskitarihi,dolapno,stok) values('" +
                        textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" +
                        textBox7.Text + "','" + textBox5.Text + "','" +
                        textBox10.Text + "')", baglan);
                kaydet.ExecuteNonQuery();
                AutoClosingMessageBox.Show("Kitap Başarılı Bir Şekilde Eklendi...", "Ekleme Başarılı", 150);

                baglan.Close();
            }
            catch
            {
                AutoClosingMessageBox.Show("Lütfen Bilgilerinizi Kontrol Ediniz...", "Ekleme Başarısız", 3000);
            }
            frm1.kitap_oku();
            frm1.dataGridView2.Visible = false;
            frm1.dataGridView1.Visible = true;
            baglan.Close();
            int Cemal = Convert.ToInt32(textBox1.Text);
            {
                textBox1.Text = (Cemal + 1).ToString();
            }
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox7.Text = "";
            textBox10.Text = "1";
            textBox5.Select();
            textBox5.Focus();
            textBox2.Select();
            textBox2.Focus();

        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            var frm1 = (Form1)Application.OpenForms["Form1"];
            var guncelle =
                new OleDbCommand(
                    "update kitaplar set barkodno='" + textBox1.Text + "',kitapadi='" + textBox2.Text + "',yazaradi='" +
                    textBox3.Text + "',yayinevi='" + textBox4.Text + "',baskitarihi='" + textBox7.Text + "',dolapno='" +
                    textBox5.Text + "',stok=" + textBox10.Text + " where id=" + gelen_id + "",
                    baglan);
            baglan.Open();
            guncelle.ExecuteNonQuery();
            MessageBox.Show("Kayıt Güncelleme İşlemi Başarılı Bir Şekilde Gerçekleştirildi", "Kütüphane Takip Programı",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            frm1.kitap_oku();
            frm1.dataGridView2.Visible = false;
            frm1.dataGridView1.Visible = true;
            baglan.Close();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }     
        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    var frm1 = (Form1)Application.OpenForms["Form1"];
                    try
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        var kaydet =
                            new OleDbCommand(
                                "insert into kitaplar (barkodno,kitapadi,yazaradi,yayinevi,baskitarihi,dolapno,stok) values('" +
                                textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" +
                                textBox7.Text + "','" + textBox5.Text + "','" +
                                textBox10.Text + "')", baglan);
                        kaydet.ExecuteNonQuery();
                        AutoClosingMessageBox.Show("Kitap Başarılı Bir Şekilde Eklendi...", "Ekleme Başarılı", 150);

                        baglan.Close();
                    }
                    catch
                    {
                        AutoClosingMessageBox.Show("Lütfen Bilgilerinizi Kontrol Ediniz...", "Ekleme Başarısız", 3000);
                    }
                    frm1.kitap_oku();
                    frm1.dataGridView2.Visible = false;
                    frm1.dataGridView1.Visible = true;
                    baglan.Close();
                }
                int Cemal = Convert.ToInt32(textBox1.Text);
                if (e.KeyCode == Keys.Enter)
                {
                    textBox1.Text = (Cemal + 1).ToString();
                }

                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox7.Text = "";
                textBox10.Text = "1";
                textBox5.Select();
                textBox5.Focus();
                textBox2.Select();
                textBox2.Focus();
            }
        }

 
        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    var frm1 = (Form1)Application.OpenForms["Form1"];
                    try
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        var kaydet =
                            new OleDbCommand(
                                "insert into kitaplar (barkodno,kitapadi,yazaradi,yayinevi,baskitarihi,dolapno,stok) values('" +
                                textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" +
                                textBox7.Text + "','" + textBox5.Text + "','" +
                                textBox10.Text + "')", baglan);
                        kaydet.ExecuteNonQuery();
                        AutoClosingMessageBox.Show("Kitap Başarılı Bir Şekilde Eklendi...", "Ekleme Başarılı", 150);

                        baglan.Close();
                    }
                    catch
                    {
                        AutoClosingMessageBox.Show("Lütfen Bilgilerinizi Kontrol Ediniz...", "Ekleme Başarısız", 3000);
                    }
                    frm1.kitap_oku();
                    frm1.dataGridView2.Visible = false;
                    frm1.dataGridView1.Visible = true;
                    baglan.Close();
                }
                int Cemal = Convert.ToInt32(textBox1.Text);
                if (e.KeyCode == Keys.Enter)
                {
                    textBox1.Text = (Cemal + 1).ToString();
                }

                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox7.Text = "";
                textBox10.Text = "1";
                textBox5.Select();
                textBox5.Focus();
                textBox2.Select();
                textBox2.Focus();
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    var frm1 = (Form1)Application.OpenForms["Form1"];
                    try
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        var kaydet =
                            new OleDbCommand(
                                "insert into kitaplar (barkodno,kitapadi,yazaradi,yayinevi,baskitarihi,dolapno,stok) values('" +
                                textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" +
                                textBox7.Text + "','" + textBox5.Text + "','" +
                                textBox10.Text + "')", baglan);
                        kaydet.ExecuteNonQuery();
                        AutoClosingMessageBox.Show("Kitap Başarılı Bir Şekilde Eklendi...", "Ekleme Başarılı", 150);

                        baglan.Close();
                    }
                    catch
                    {
                        AutoClosingMessageBox.Show("Lütfen Bilgilerinizi Kontrol Ediniz...", "Ekleme Başarısız", 3000);
                    }
                    frm1.kitap_oku();
                    frm1.dataGridView2.Visible = false;
                    frm1.dataGridView1.Visible = true;
                    baglan.Close();
                }
                int Cemal = Convert.ToInt32(textBox1.Text);
                if (e.KeyCode == Keys.Enter)
                {
                    textBox1.Text = (Cemal + 1).ToString();
                }

                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox7.Text = "";
                textBox10.Text = "1";
                textBox5.Select();
                textBox5.Focus();
                textBox2.Select();
                textBox2.Focus();
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    var frm1 = (Form1)Application.OpenForms["Form1"];
                    try
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        var kaydet =
                            new OleDbCommand(
                                "insert into kitaplar (barkodno,kitapadi,yazaradi,yayinevi,baskitarihi,dolapno,stok) values('" +
                                textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" +
                                textBox7.Text + "','" + textBox5.Text + "','" +
                                textBox10.Text + "')", baglan);
                        kaydet.ExecuteNonQuery();
                        AutoClosingMessageBox.Show("Kitap Başarılı Bir Şekilde Eklendi...", "Ekleme Başarılı", 150);

                        baglan.Close();
                    }
                    catch
                    {
                        AutoClosingMessageBox.Show("Lütfen Bilgilerinizi Kontrol Ediniz...", "Ekleme Başarısız", 3000);
                    }
                    frm1.kitap_oku();
                    frm1.dataGridView2.Visible = false;
                    frm1.dataGridView1.Visible = true;
                    baglan.Close();
                }
                int Cemal = Convert.ToInt32(textBox1.Text);
                if (e.KeyCode == Keys.Enter)
                {
                    textBox1.Text = (Cemal + 1).ToString();
                }

                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox7.Text = "";
                textBox10.Text = "1";
                textBox5.Select();
                textBox5.Focus();
                textBox2.Select();
                textBox2.Focus();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    var frm1 = (Form1)Application.OpenForms["Form1"];
                    try
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        var kaydet =
                            new OleDbCommand(
                                "insert into kitaplar (barkodno,kitapadi,yazaradi,yayinevi,baskitarihi,dolapno,stok) values('" +
                                textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" +
                                textBox7.Text + "','" + textBox5.Text + "','" +
                                textBox10.Text + "')", baglan);
                        kaydet.ExecuteNonQuery();
                        AutoClosingMessageBox.Show("Kitap Başarılı Bir Şekilde Eklendi...", "Ekleme Başarılı", 150);

                        baglan.Close();
                    }
                    catch
                    {
                        AutoClosingMessageBox.Show("Lütfen Bilgilerinizi Kontrol Ediniz...", "Ekleme Başarısız", 3000);
                    }
                    frm1.kitap_oku();
                    frm1.dataGridView2.Visible = false;
                    frm1.dataGridView1.Visible = true;
                    baglan.Close();
                }
                int Cemal = Convert.ToInt32(textBox1.Text);
                if (e.KeyCode == Keys.Enter)
                {
                    textBox1.Text = (Cemal + 1).ToString();
                }

                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox7.Text = "";
                textBox10.Text = "1";
                textBox5.Select();
                textBox5.Focus();
                textBox2.Select();
                textBox2.Focus();
            }
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    var frm1 = (Form1)Application.OpenForms["Form1"];
                    try
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        var kaydet =
                            new OleDbCommand(
                                "insert into kitaplar (barkodno,kitapadi,yazaradi,yayinevi,baskitarihi,dolapno,stok) values('" +
                                textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" +
                                textBox7.Text + "','" + textBox5.Text + "','" +
                                textBox10.Text + "')", baglan);
                        kaydet.ExecuteNonQuery();
                        AutoClosingMessageBox.Show("Kitap Başarılı Bir Şekilde Eklendi...", "Ekleme Başarılı", 150);

                        baglan.Close();
                    }
                    catch
                    {
                        AutoClosingMessageBox.Show("Lütfen Bilgilerinizi Kontrol Ediniz...", "Ekleme Başarısız", 3000);
                    }
                    frm1.kitap_oku();
                    frm1.dataGridView2.Visible = false;
                    frm1.dataGridView1.Visible = true;
                    baglan.Close();
                }
                int Cemal = Convert.ToInt32(textBox1.Text);
                if (e.KeyCode == Keys.Enter)
                {
                    textBox1.Text = (Cemal + 1).ToString();
                }

                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox7.Text = "";
                textBox10.Text = "1";
                textBox5.Select();
                textBox5.Focus();
                textBox2.Select();
                textBox2.Focus();
        }  }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    var frm1 = (Form1)Application.OpenForms["Form1"];
                    try
                    {
                        if (baglan.State == ConnectionState.Closed) baglan.Open();
                        var kaydet =
                            new OleDbCommand(
                                "insert into kitaplar (barkodno,kitapadi,yazaradi,yayinevi,baskitarihi,dolapno,stok) values('" +
                                textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" +
                                textBox7.Text + "','" + textBox5.Text + "','" +
                                textBox10.Text + "')", baglan);
                        kaydet.ExecuteNonQuery();
                        AutoClosingMessageBox.Show("Kitap Başarılı Bir Şekilde Eklendi...", "Ekleme Başarılı", 150);

                        baglan.Close();
                    }
                    catch
                    {
                        AutoClosingMessageBox.Show("Lütfen Bilgilerinizi Kontrol Ediniz...", "Ekleme Başarısız", 3000);
                    }
                    frm1.kitap_oku();
                    frm1.dataGridView2.Visible = false;
                    frm1.dataGridView1.Visible = true;
                    baglan.Close();
                }
                int Cemal = Convert.ToInt32(textBox1.Text);
                if (e.KeyCode == Keys.Enter)
                {
                    textBox1.Text = (Cemal + 1).ToString();
                }

                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox7.Text = "";
                textBox10.Text = "1";
                textBox5.Select();
                textBox5.Focus();
                textBox2.Select();
                textBox2.Focus();
        }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            }
        }



    } 
     
