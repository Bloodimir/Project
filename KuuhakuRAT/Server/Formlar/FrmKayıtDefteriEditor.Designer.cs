namespace xServer.Formlar
{
    partial class FrmKayıtDefteriEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmKayıtDefteriEditor));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tvRegistryDirectory = new xServer.Kontroller.RegistryTreeView();
            this.imageRegistryDirectoryList = new System.Windows.Forms.ImageList(this.components);
            this.lstRegistryKeys = new xServer.Kontroller.AeroListView();
            this.hName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageRegistryKeyTypeList = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.selectedStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyBinaryDataToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyNewtoolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.newToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.keyToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.stringValueToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.binaryValueToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.dWORD32bitValueToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.qWORD64bitValueToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.multiStringValueToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.expandableStringValueToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tv_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.stringValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.binaryValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dWORD32bitValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qWORD64bitValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multiStringValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandableStringValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedItem_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyBinaryDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lst_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.keyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.stringValueToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.binaryValueToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dWORD32bitValueToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.qWORD64bitValueToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.multiStringValueToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.expandableStringValueToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.tv_ContextMenuStrip.SuspendLayout();
            this.selectedItem_ContextMenuStrip.SuspendLayout();
            this.lst_ContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.splitContainer, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.statusStrip, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.menuStrip, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1239, 724);
            this.tableLayoutPanel.TabIndex = 0;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(3, 28);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Panel1.Controls.Add(this.tvRegistryDirectory);
            this.splitContainer.Panel2.Controls.Add(this.lstRegistryKeys);
            this.splitContainer.Size = new System.Drawing.Size(1233, 671);
            this.splitContainer.SplitterDistance = 411;
            this.splitContainer.TabIndex = 0;
            this.tvRegistryDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvRegistryDirectory.HideSelection = false;
            this.tvRegistryDirectory.ImageIndex = 0;
            this.tvRegistryDirectory.ImageList = this.imageRegistryDirectoryList;
            this.tvRegistryDirectory.Location = new System.Drawing.Point(0, 0);
            this.tvRegistryDirectory.Name = "tvRegistryDirectory";
            this.tvRegistryDirectory.SelectedImageIndex = 0;
            this.tvRegistryDirectory.Size = new System.Drawing.Size(411, 671);
            this.tvRegistryDirectory.TabIndex = 0;
            this.tvRegistryDirectory.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvRegistryDirectory_AfterLabelEdit);
            this.tvRegistryDirectory.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvRegistryDirectory_BeforeExpand);
            this.tvRegistryDirectory.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvRegistryDirectory_BeforeSelect);
            this.tvRegistryDirectory.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvRegistryDirectory_NodeMouseClick);
            this.tvRegistryDirectory.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvRegistryDirectory_KeyUp);
            this.imageRegistryDirectoryList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageRegistryDirectoryList.ImageStream")));
            this.imageRegistryDirectoryList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageRegistryDirectoryList.Images.SetKeyName(0, "folder.png");
            this.lstRegistryKeys.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hName,
            this.hType,
            this.hValue});
            this.lstRegistryKeys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRegistryKeys.FullRowSelect = true;
            this.lstRegistryKeys.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstRegistryKeys.HideSelection = false;
            this.lstRegistryKeys.Location = new System.Drawing.Point(0, 0);
            this.lstRegistryKeys.Name = "lstRegistryKeys";
            this.lstRegistryKeys.Size = new System.Drawing.Size(818, 671);
            this.lstRegistryKeys.SmallImageList = this.imageRegistryKeyTypeList;
            this.lstRegistryKeys.TabIndex = 0;
            this.lstRegistryKeys.UseCompatibleStateImageBehavior = false;
            this.lstRegistryKeys.View = System.Windows.Forms.View.Details;
            this.lstRegistryKeys.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lstRegistryKeys_AfterLabelEdit);
            this.lstRegistryKeys.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstRegistryKeys_ItemSelectionChanged);
            this.lstRegistryKeys.Enter += new System.EventHandler(this.lstRegistryKeys_Enter);
            this.lstRegistryKeys.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstRegistryKeys_KeyUp);
            this.lstRegistryKeys.Leave += new System.EventHandler(this.lstRegistryKeys_Leave);
            this.lstRegistryKeys.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstRegistryKeys_MouseClick);
            this.hName.Text = "İsim";
            this.hName.Width = 203;
            this.hType.Text = "Tür";
            this.hType.Width = 149;
            this.hValue.Text = "Değer";
            this.hValue.Width = 384;
            this.imageRegistryKeyTypeList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageRegistryKeyTypeList.ImageStream")));
            this.imageRegistryKeyTypeList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageRegistryKeyTypeList.Images.SetKeyName(0, "reg_string.png");
            this.imageRegistryKeyTypeList.Images.SetKeyName(1, "reg_binary.png");
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 702);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1239, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            this.selectedStripStatusLabel.Name = "selectedStripStatusLabel";
            this.selectedStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(120, 24);
            this.menuStrip.TabIndex = 2;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.fileToolStripMenuItem.Text = "Dosya";
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Çıkış";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.menuStripExit_Click);
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifyToolStripMenuItem1,
            this.modifyBinaryDataToolStripMenuItem1,
            this.modifyNewtoolStripSeparator,
            this.newToolStripMenuItem2,
            this.toolStripSeparator6,
            this.deleteToolStripMenuItem2,
            this.renameToolStripMenuItem2});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.editToolStripMenuItem.Text = "Değiştir";
            this.modifyToolStripMenuItem1.Enabled = false;
            this.modifyToolStripMenuItem1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modifyToolStripMenuItem1.Name = "modifyToolStripMenuItem1";
            this.modifyToolStripMenuItem1.Size = new System.Drawing.Size(201, 22);
            this.modifyToolStripMenuItem1.Text = "Düzenle...";
            this.modifyToolStripMenuItem1.Visible = false;
            this.modifyToolStripMenuItem1.Click += new System.EventHandler(this.modifyRegistryValue_Click);
            this.modifyBinaryDataToolStripMenuItem1.Enabled = false;
            this.modifyBinaryDataToolStripMenuItem1.Name = "modifyBinaryDataToolStripMenuItem1";
            this.modifyBinaryDataToolStripMenuItem1.Size = new System.Drawing.Size(201, 22);
            this.modifyBinaryDataToolStripMenuItem1.Text = "Binary Verisini Düzenle...";
            this.modifyBinaryDataToolStripMenuItem1.Visible = false;
            this.modifyBinaryDataToolStripMenuItem1.Click += new System.EventHandler(this.modifyBinaryDataRegistryValue_Click);
            this.modifyNewtoolStripSeparator.Name = "modifyNewtoolStripSeparator";
            this.modifyNewtoolStripSeparator.Size = new System.Drawing.Size(198, 6);
            this.modifyNewtoolStripSeparator.Visible = false;
            this.newToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyToolStripMenuItem2,
            this.toolStripSeparator7,
            this.stringValueToolStripMenuItem2,
            this.binaryValueToolStripMenuItem2,
            this.dWORD32bitValueToolStripMenuItem2,
            this.qWORD64bitValueToolStripMenuItem2,
            this.multiStringValueToolStripMenuItem2,
            this.expandableStringValueToolStripMenuItem2});
            this.newToolStripMenuItem2.Name = "newToolStripMenuItem2";
            this.newToolStripMenuItem2.Size = new System.Drawing.Size(201, 22);
            this.newToolStripMenuItem2.Text = "Yeni";
            this.keyToolStripMenuItem2.Name = "keyToolStripMenuItem2";
            this.keyToolStripMenuItem2.Size = new System.Drawing.Size(219, 22);
            this.keyToolStripMenuItem2.Text = "Anahtar";
            this.keyToolStripMenuItem2.Click += new System.EventHandler(this.createNewRegistryKey_Click);
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(216, 6);
            this.stringValueToolStripMenuItem2.Name = "stringValueToolStripMenuItem2";
            this.stringValueToolStripMenuItem2.Size = new System.Drawing.Size(219, 22);
            this.stringValueToolStripMenuItem2.Text = "String Değeri";
            this.stringValueToolStripMenuItem2.Click += new System.EventHandler(this.createStringRegistryValue_Click);
            this.binaryValueToolStripMenuItem2.Name = "binaryValueToolStripMenuItem2";
            this.binaryValueToolStripMenuItem2.Size = new System.Drawing.Size(219, 22);
            this.binaryValueToolStripMenuItem2.Text = "Binary Değeri";
            this.binaryValueToolStripMenuItem2.Click += new System.EventHandler(this.createBinaryRegistryValue_Click);
            this.dWORD32bitValueToolStripMenuItem2.Name = "dWORD32bitValueToolStripMenuItem2";
            this.dWORD32bitValueToolStripMenuItem2.Size = new System.Drawing.Size(219, 22);
            this.dWORD32bitValueToolStripMenuItem2.Text = "32 Bit DWORD Değeri";
            this.dWORD32bitValueToolStripMenuItem2.Click += new System.EventHandler(this.createDwordRegistryValue_Click);
            this.qWORD64bitValueToolStripMenuItem2.Name = "qWORD64bitValueToolStripMenuItem2";
            this.qWORD64bitValueToolStripMenuItem2.Size = new System.Drawing.Size(219, 22);
            this.qWORD64bitValueToolStripMenuItem2.Text = "64 Bit DWORD Değeri";
            this.qWORD64bitValueToolStripMenuItem2.Click += new System.EventHandler(this.createQwordRegistryValue_Click);
            this.multiStringValueToolStripMenuItem2.Name = "multiStringValueToolStripMenuItem2";
            this.multiStringValueToolStripMenuItem2.Size = new System.Drawing.Size(219, 22);
            this.multiStringValueToolStripMenuItem2.Text = "Çoklu-String Değeri";
            this.multiStringValueToolStripMenuItem2.Click += new System.EventHandler(this.createMultiStringRegistryValue_Click);
            this.expandableStringValueToolStripMenuItem2.Name = "expandableStringValueToolStripMenuItem2";
            this.expandableStringValueToolStripMenuItem2.Size = new System.Drawing.Size(219, 22);
            this.expandableStringValueToolStripMenuItem2.Text = "Genişletilebilir String Değeri";
            this.expandableStringValueToolStripMenuItem2.Click += new System.EventHandler(this.createExpandStringRegistryValue_Click);
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(198, 6);
            this.deleteToolStripMenuItem2.Enabled = false;
            this.deleteToolStripMenuItem2.Name = "deleteToolStripMenuItem2";
            this.deleteToolStripMenuItem2.ShortcutKeyDisplayString = "Del";
            this.deleteToolStripMenuItem2.Size = new System.Drawing.Size(201, 22);
            this.deleteToolStripMenuItem2.Text = "Sil";
            this.deleteToolStripMenuItem2.Click += new System.EventHandler(this.menuStripDelete_Click);
            this.renameToolStripMenuItem2.Enabled = false;
            this.renameToolStripMenuItem2.Name = "renameToolStripMenuItem2";
            this.renameToolStripMenuItem2.Size = new System.Drawing.Size(201, 22);
            this.renameToolStripMenuItem2.Text = "Yeniden Adlandır";
            this.renameToolStripMenuItem2.Click += new System.EventHandler(this.menuStripRename_Click);
            this.tv_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem,
            this.renameToolStripMenuItem});
            this.tv_ContextMenuStrip.Name = "contextMenuStrip";
            this.tv_ContextMenuStrip.Size = new System.Drawing.Size(118, 76);
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyToolStripMenuItem,
            this.toolStripSeparator2,
            this.stringValueToolStripMenuItem,
            this.binaryValueToolStripMenuItem,
            this.dWORD32bitValueToolStripMenuItem,
            this.qWORD64bitValueToolStripMenuItem,
            this.multiStringValueToolStripMenuItem,
            this.expandableStringValueToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.newToolStripMenuItem.Text = "Yeni";
            this.keyToolStripMenuItem.Name = "keyToolStripMenuItem";
            this.keyToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.keyToolStripMenuItem.Text = "Anahtar";
            this.keyToolStripMenuItem.Click += new System.EventHandler(this.createNewRegistryKey_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(196, 6);
            this.stringValueToolStripMenuItem.Name = "stringValueToolStripMenuItem";
            this.stringValueToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.stringValueToolStripMenuItem.Text = "String Değeri";
            this.stringValueToolStripMenuItem.Click += new System.EventHandler(this.createStringRegistryValue_Click);
            this.binaryValueToolStripMenuItem.Name = "binaryValueToolStripMenuItem";
            this.binaryValueToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.binaryValueToolStripMenuItem.Text = "Binary Değeri";
            this.binaryValueToolStripMenuItem.Click += new System.EventHandler(this.createBinaryRegistryValue_Click);
            this.dWORD32bitValueToolStripMenuItem.Name = "dWORD32bitValueToolStripMenuItem";
            this.dWORD32bitValueToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.dWORD32bitValueToolStripMenuItem.Text = "32 Bit DWORD Değeri";
            this.dWORD32bitValueToolStripMenuItem.Click += new System.EventHandler(this.createDwordRegistryValue_Click);
            this.qWORD64bitValueToolStripMenuItem.Name = "qWORD64bitValueToolStripMenuItem";
            this.qWORD64bitValueToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.qWORD64bitValueToolStripMenuItem.Text = "64 Bit QWORD Değeri";
            this.qWORD64bitValueToolStripMenuItem.Click += new System.EventHandler(this.createQwordRegistryValue_Click);
            this.multiStringValueToolStripMenuItem.Name = "multiStringValueToolStripMenuItem";
            this.multiStringValueToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.multiStringValueToolStripMenuItem.Text = "Çoklu String Değeri";
            this.multiStringValueToolStripMenuItem.Click += new System.EventHandler(this.createMultiStringRegistryValue_Click);
            this.expandableStringValueToolStripMenuItem.Name = "expandableStringValueToolStripMenuItem";
            this.expandableStringValueToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.expandableStringValueToolStripMenuItem.Text = "Genişletilebilir String Değeri";
            this.expandableStringValueToolStripMenuItem.Click += new System.EventHandler(this.createExpandStringRegistryValue_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(114, 6);
            this.deleteToolStripMenuItem.Enabled = false;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem.Text = "Sil";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteRegistryKey_Click);
            this.renameToolStripMenuItem.Enabled = false;
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.renameToolStripMenuItem.Text = "Yeniden Adlandır";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameRegistryKey_Click);
            this.selectedItem_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifyToolStripMenuItem,
            this.modifyBinaryDataToolStripMenuItem,
            this.toolStripSeparator3,
            this.deleteToolStripMenuItem1,
            this.renameToolStripMenuItem1});
            this.selectedItem_ContextMenuStrip.Name = "selectedItem_ContextMenuStrip";
            this.selectedItem_ContextMenuStrip.Size = new System.Drawing.Size(185, 98);
            this.modifyToolStripMenuItem.Enabled = false;
            this.modifyToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            this.modifyToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.modifyToolStripMenuItem.Text = "Düzenle...";
            this.modifyToolStripMenuItem.Click += new System.EventHandler(this.modifyRegistryValue_Click);
            this.modifyBinaryDataToolStripMenuItem.Enabled = false;
            this.modifyBinaryDataToolStripMenuItem.Name = "modifyBinaryDataToolStripMenuItem";
            this.modifyBinaryDataToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.modifyBinaryDataToolStripMenuItem.Text = "Binary Değerini Düzenle...";
            this.modifyBinaryDataToolStripMenuItem.Click += new System.EventHandler(this.modifyBinaryDataRegistryValue_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(181, 6);
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.deleteToolStripMenuItem1.Text = "Sil";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteRegistryValue_Click);
            this.renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            this.renameToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.renameToolStripMenuItem1.Text = "Yeniden Adlandır";
            this.renameToolStripMenuItem1.Click += new System.EventHandler(this.renameRegistryValue_Click);
            this.lst_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1});
            this.lst_ContextMenuStrip.Name = "lst_ContextMenuStrip";
            this.lst_ContextMenuStrip.Size = new System.Drawing.Size(99, 26);
            this.newToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyToolStripMenuItem1,
            this.toolStripSeparator4,
            this.stringValueToolStripMenuItem1,
            this.binaryValueToolStripMenuItem1,
            this.dWORD32bitValueToolStripMenuItem1,
            this.qWORD64bitValueToolStripMenuItem1,
            this.multiStringValueToolStripMenuItem1,
            this.expandableStringValueToolStripMenuItem1});
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(98, 22);
            this.newToolStripMenuItem1.Text = "Yeni";
            this.keyToolStripMenuItem1.Name = "keyToolStripMenuItem1";
            this.keyToolStripMenuItem1.Size = new System.Drawing.Size(199, 22);
            this.keyToolStripMenuItem1.Text = "Anahtar";
            this.keyToolStripMenuItem1.Click += new System.EventHandler(this.createNewRegistryKey_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(196, 6);
            this.stringValueToolStripMenuItem1.Name = "stringValueToolStripMenuItem1";
            this.stringValueToolStripMenuItem1.Size = new System.Drawing.Size(199, 22);
            this.stringValueToolStripMenuItem1.Text = "String Değeri";
            this.stringValueToolStripMenuItem1.Click += new System.EventHandler(this.createStringRegistryValue_Click);
            this.binaryValueToolStripMenuItem1.Name = "binaryValueToolStripMenuItem1";
            this.binaryValueToolStripMenuItem1.Size = new System.Drawing.Size(199, 22);
            this.binaryValueToolStripMenuItem1.Text = "Binary Değeri";
            this.binaryValueToolStripMenuItem1.Click += new System.EventHandler(this.createBinaryRegistryValue_Click);
            this.dWORD32bitValueToolStripMenuItem1.Name = "dWORD32bitValueToolStripMenuItem1";
            this.dWORD32bitValueToolStripMenuItem1.Size = new System.Drawing.Size(199, 22);
            this.dWORD32bitValueToolStripMenuItem1.Text = "32 Bit DWORD Değeri";
            this.dWORD32bitValueToolStripMenuItem1.Click += new System.EventHandler(this.createDwordRegistryValue_Click);
            this.qWORD64bitValueToolStripMenuItem1.Name = "qWORD64bitValueToolStripMenuItem1";
            this.qWORD64bitValueToolStripMenuItem1.Size = new System.Drawing.Size(199, 22);
            this.qWORD64bitValueToolStripMenuItem1.Text = "64 Bit QWORD Değeri";
            this.multiStringValueToolStripMenuItem1.Name = "multiStringValueToolStripMenuItem1";
            this.multiStringValueToolStripMenuItem1.Size = new System.Drawing.Size(199, 22);
            this.multiStringValueToolStripMenuItem1.Text = "Çoklu String Değeri";
            this.multiStringValueToolStripMenuItem1.Click += new System.EventHandler(this.createMultiStringRegistryValue_Click);
            this.expandableStringValueToolStripMenuItem1.Name = "expandableStringValueToolStripMenuItem1";
            this.expandableStringValueToolStripMenuItem1.Size = new System.Drawing.Size(199, 22);
            this.expandableStringValueToolStripMenuItem1.Text = "Genişletilebilir String Değeri";
            this.expandableStringValueToolStripMenuItem1.Click += new System.EventHandler(this.createExpandStringRegistryValue_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1239, 724);
            this.Controls.Add(this.tableLayoutPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FrmRegistryEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kayıt Defteri Düzenleyicisi";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRegistryEditor_FormClosing);
            this.Load += new System.EventHandler(this.FrmRegistryEditor_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tv_ContextMenuStrip.ResumeLayout(false);
            this.selectedItem_ContextMenuStrip.ResumeLayout(false);
            this.lst_ContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Kontroller.RegistryTreeView tvRegistryDirectory;
        private Kontroller.AeroListView lstRegistryKeys;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel selectedStripStatusLabel;
        private System.Windows.Forms.ImageList imageRegistryDirectoryList;
        private System.Windows.Forms.ColumnHeader hName;
        private System.Windows.Forms.ColumnHeader hType;
        private System.Windows.Forms.ColumnHeader hValue;
        private System.Windows.Forms.ImageList imageRegistryKeyTypeList;
        private System.Windows.Forms.ContextMenuStrip tv_ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem stringValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem binaryValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dWORD32bitValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qWORD64bitValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem multiStringValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandableStringValueToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip selectedItem_ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modifyBinaryDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip lst_ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem keyToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem stringValueToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem binaryValueToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dWORD32bitValueToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem qWORD64bitValueToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem multiStringValueToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem expandableStringValueToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem modifyBinaryDataToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator modifyNewtoolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem keyToolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem stringValueToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem binaryValueToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem dWORD32bitValueToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem qWORD64bitValueToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem multiStringValueToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem expandableStringValueToolStripMenuItem2;
    }
}