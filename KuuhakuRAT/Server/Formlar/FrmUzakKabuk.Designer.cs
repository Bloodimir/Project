﻿namespace xServer.Formlar
{
    partial class FrmUzakKabuk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUzakKabuk));
            this.txtConsoleOutput = new System.Windows.Forms.RichTextBox();
            this.txtConsoleInput = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtConsoleOutput
            // 
            this.txtConsoleOutput.BackColor = System.Drawing.Color.Black;
            this.txtConsoleOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtConsoleOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsoleOutput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConsoleOutput.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtConsoleOutput.Location = new System.Drawing.Point(3, 3);
            this.txtConsoleOutput.Name = "txtConsoleOutput";
            this.txtConsoleOutput.ReadOnly = true;
            this.txtConsoleOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtConsoleOutput.Size = new System.Drawing.Size(631, 297);
            this.txtConsoleOutput.TabIndex = 1;
            this.txtConsoleOutput.Text = "";
            this.txtConsoleOutput.TextChanged += new System.EventHandler(this.txtConsoleOutput_TextChanged);
            this.txtConsoleOutput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtConsoleOutput_KeyPress);
            // 
            // txtConsoleInput
            // 
            this.txtConsoleInput.BackColor = System.Drawing.Color.Black;
            this.txtConsoleInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtConsoleInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsoleInput.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConsoleInput.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtConsoleInput.Location = new System.Drawing.Point(3, 306);
            this.txtConsoleInput.MaxLength = 200;
            this.txtConsoleInput.Name = "txtConsoleInput";
            this.txtConsoleInput.Size = new System.Drawing.Size(631, 16);
            this.txtConsoleInput.TabIndex = 0;
            this.txtConsoleInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtConsoleInput_KeyDown);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.txtConsoleOutput, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.txtConsoleInput, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(637, 323);
            this.tableLayoutPanel.TabIndex = 2;
            // 
            // FrmUzakKabuk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 323);
            this.Controls.Add(this.tableLayoutPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmUzakKabuk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Uzak Kabuk []";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRemoteShell_FormClosing);
            this.Load += new System.EventHandler(this.FrmRemoteShell_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtConsoleInput;
        private System.Windows.Forms.RichTextBox txtConsoleOutput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}