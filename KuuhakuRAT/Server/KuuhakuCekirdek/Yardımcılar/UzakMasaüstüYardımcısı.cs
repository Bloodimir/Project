using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace xServer.KuuhakuCekirdek.Yardımcılar
{
    public static class UzakMasaüstüYardımcısı
    {
        public static Bitmap GetDesktop(int screenNumber)
        {
            var bounds = Screen.AllScreens[screenNumber].Bounds;
            var screenshot = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            using (Graphics graph = Graphics.FromImage(screenshot))
            {
                graph.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
                return screenshot;
            }
        }
    }
}
