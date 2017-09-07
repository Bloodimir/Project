using xServer.Kontroller;

namespace xServer.Formlar
{
    partial class FrmTersProxy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTersProxy));
            this.btnStart = new System.Windows.Forms.Button();
            this.lblLocalServerPort = new System.Windows.Forms.Label();
            this.nudServerPort = new System.Windows.Forms.NumericUpDown();
            this.tabCtrl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.killConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblProxyInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLoadBalance = new System.Windows.Forms.Label();
            this.lstConnections = new xServer.Kontroller.AeroListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.nudServerPort)).BeginInit();
            this.tabCtrl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            this.btnStart.Location = new System.Drawing.Point(261, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(114, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Dinlemeye Başla";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            this.lblLocalServerPort.AutoSize = true;
            this.lblLocalServerPort.Location = new System.Drawing.Point(22, 17);
            this.lblLocalServerPort.Name = "lblLocalServerPort";
            this.lblLocalServerPort.Size = new System.Drawing.Size(103, 13);
            this.lblLocalServerPort.TabIndex = 1;
            this.lblLocalServerPort.Text = "Yerel Sunucu Portu";
            this.nudServerPort.Location = new System.Drawing.Point(131, 15);
            this.nudServerPort.Maximum = new decimal(new int[] {
            65534,
            0,
            0,
            0});
            this.nudServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudServerPort.Name = "nudServerPort";
            this.nudServerPort.Size = new System.Drawing.Size(120, 22);
            this.nudServerPort.TabIndex = 2;
            this.nudServerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudServerPort.Value = new decimal(new int[] {
            3128,
            0,
            0,
            0});
            this.nudServerPort.ValueChanged += new System.EventHandler(this.nudServerPort_ValueChanged);
            this.tabCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrl.Controls.Add(this.tabPage1);
            this.tabCtrl.Location = new System.Drawing.Point(26, 115);
            this.tabCtrl.Name = "tabCtrl";
            this.tabCtrl.SelectedIndex = 0;
            this.tabCtrl.Size = new System.Drawing.Size(736, 274);
            this.tabCtrl.TabIndex = 3;
            this.tabPage1.Controls.Add(this.lstConnections);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(728, 248);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Açık Bağlantılar";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.killConnectionToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(156, 26);
            this.killConnectionToolStripMenuItem.Name = "killConnectionToolStripMenuItem";
            this.killConnectionToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.killConnectionToolStripMenuItem.Text = "Kill Connection";
            this.killConnectionToolStripMenuItem.Click += new System.EventHandler(this.killConnectionToolStripMenuItem_Click);
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(381, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(114, 23);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Dinlemeyi Durdur";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            this.lblProxyInfo.AutoSize = true;
            this.lblProxyInfo.Location = new System.Drawing.Point(23, 51);
            this.lblProxyInfo.Name = "lblProxyInfo";
            this.lblProxyInfo.Size = new System.Drawing.Size(309, 13);
            this.lblProxyInfo.TabIndex = 5;
            this.lblProxyInfo.Text = "Bu SOCKS5 Proxysine Bağlanın: 127.0.0.1:x (id/şifre istemez)";
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(469, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "DNS Sızmalarını Azaltmak İçin Bütün DNS Sorguları Kurban Üzerinden Gerçekleştiril" +
    "ecektir.";
            this.lblLoadBalance.AutoSize = true;
            this.lblLoadBalance.Location = new System.Drawing.Point(23, 84);
            this.lblLoadBalance.Name = "lblLoadBalance";
            this.lblLoadBalance.Size = new System.Drawing.Size(105, 13);
            this.lblLoadBalance.TabIndex = 7;
            this.lblLoadBalance.Text = "[Load Balance Info]";
            this.lblLoadBalance.Click += new System.EventHandler(this.lblLoadBalance_Click);
            this.lstConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lstConnections.ContextMenuStrip = this.contextMenuStrip;
            this.lstConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstConnections.FullRowSelect = true;
            this.lstConnections.GridLines = true;
            this.lstConnections.Location = new System.Drawing.Point(3, 3);
            this.lstConnections.Name = "lstConnections";
            this.lstConnections.Size = new System.Drawing.Size(722, 242);
            this.lstConnections.TabIndex = 0;
            this.lstConnections.UseCompatibleStateImageBehavior = false;
            this.lstConnections.View = System.Windows.Forms.View.Details;
            this.lstConnections.VirtualMode = true;
            this.lstConnections.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.LvConnections_RetrieveVirtualItem);
            this.columnHeader6.Text = "Kurban IP";
            this.columnHeader6.Width = 106;
            this.columnHeader7.Text = "Kurban Ülkesi";
            this.columnHeader7.Width = 106;
            this.columnHeader1.Text = "Hedef Sunucu";
            this.columnHeader1.Width = 135;
            this.columnHeader2.Text = "Hedef Port";
            this.columnHeader2.Width = 68;
            this.columnHeader3.Text = "Toplam Alınan";
            this.columnHeader3.Width = 105;
            this.columnHeader4.Text = "Toplam Yollanan";
            this.columnHeader4.Width = 95;
            this.columnHeader5.Text = "Proxy Türü";
            this.columnHeader5.Width = 90;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 402);
            this.Controls.Add(this.lblLoadBalance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblProxyInfo);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.tabCtrl);
            this.Controls.Add(this.nudServerPort);
            this.Controls.Add(this.lblLocalServerPort);
            this.Controls.Add(this.btnStart);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmReverseProxy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reverse Proxy []";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmReverseProxy_FormClosing);
            this.Load += new System.EventHandler(this.FrmReverseProxy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudServerPort)).EndInit();
            this.tabCtrl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblLocalServerPort;
        private System.Windows.Forms.NumericUpDown nudServerPort;
        private System.Windows.Forms.TabControl tabCtrl;
        private System.Windows.Forms.TabPage tabPage1;
        private AeroListView lstConnections;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblProxyInfo;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem killConnectionToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLoadBalance;
    }
}