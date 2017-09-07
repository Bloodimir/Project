using System;
using System.Windows.Forms;

namespace Kütüphane_Takip_Programı
{
    public partial class ana : Form
    {
        public ana()
        {
            InitializeComponent();
        }

        private void ana_Load(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var frm1 = new Yardım();
            frm1.Show();
            Hide();
        }
    }
}