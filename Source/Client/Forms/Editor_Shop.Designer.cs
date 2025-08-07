using DarkUI.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public partial class Editor_Shop : Form
    {

        // Shared instance of the form
        private static Editor_Shop _instance;

        // Public property to get the shared instance
        public static Editor_Shop Instance
        {
            get
            {
                // Create a new instance if one does not exist or if it has been disposed
                if (_instance is null || _instance.IsDisposed)
                {
                    _instance = new Editor_Shop();
                }
                return _instance;
            }
        }

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing & components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            DarkGroupBox1 = new DarkUI.Controls.DarkGroupBox();
            lstIndex = new ListBox();
            DarkGroupBox2 = new DarkUI.Controls.DarkGroupBox();
            nudBuy = new DarkUI.Controls.DarkNumericUpDown();
            DarkGroupBox3 = new DarkUI.Controls.DarkGroupBox();
            btnDeleteTrade = new DarkUI.Controls.DarkButton();
            btnUpdate = new DarkUI.Controls.DarkButton();
            nudCostValue = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel8 = new DarkUI.Controls.cmbLabel();
            cmbCostItem = new DarkUI.Controls.DarkComboBox();
            cmbLabel7 = new DarkUI.Controls.cmbLabel();
            nudItemValue = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel6 = new DarkUI.Controls.cmbLabel();
            cmbItem = new DarkUI.Controls.DarkComboBox();
            cmbLabel5 = new DarkUI.Controls.cmbLabel();
            lstTradeItem = new ListBox();
            cmbLabel4 = new DarkUI.Controls.cmbLabel();
            cmbLabel3 = new DarkUI.Controls.cmbLabel();
            txtName = new DarkUI.Controls.DarkTextBox();
            cmbLabel1 = new DarkUI.Controls.cmbLabel();
            btnCancel = new DarkUI.Controls.DarkButton();
            btnDelete = new DarkUI.Controls.DarkButton();
            btnSave = new DarkUI.Controls.DarkButton();
            DarkGroupBox1.SuspendLayout();
            DarkGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudBuy).BeginInit();
            DarkGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudCostValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudItemValue).BeginInit();
            SuspendLayout();
            // 
            // DarkGroupBox1
            // 
            DarkGroupBox1.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox1.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox1.Controls.Add(lstIndex);
            DarkGroupBox1.ForeColor = Color.Gainsboro;
            DarkGroupBox1.Location = new Point(4, 3);
            DarkGroupBox1.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox1.Name = "DarkGroupBox1";
            DarkGroupBox1.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox1.Size = new Size(244, 257);
            DarkGroupBox1.TabIndex = 0;
            DarkGroupBox1.TabStop = false;
            DarkGroupBox1.Text = "Shop List";
            // 
            // lstIndex
            // 
            lstIndex.BackColor = Color.FromArgb(45, 45, 48);
            lstIndex.BorderStyle = BorderStyle.FixedSingle;
            lstIndex.ForeColor = Color.Gainsboro;
            lstIndex.FormattingEnabled = true;
            lstIndex.Location = new Point(7, 22);
            lstIndex.Margin = new Padding(4, 3, 4, 3);
            lstIndex.Name = "lstIndex";
            lstIndex.Size = new Size(228, 227);
            lstIndex.TabIndex = 1;
            lstIndex.Click += lstIndex_Click;
            // 
            // DarkGroupBox2
            // 
            DarkGroupBox2.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox2.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox2.Controls.Add(nudBuy);
            DarkGroupBox2.Controls.Add(DarkGroupBox3);
            DarkGroupBox2.Controls.Add(cmbLabel4);
            DarkGroupBox2.Controls.Add(cmbLabel3);
            DarkGroupBox2.Controls.Add(txtName);
            DarkGroupBox2.Controls.Add(cmbLabel1);
            DarkGroupBox2.ForeColor = Color.Gainsboro;
            DarkGroupBox2.Location = new Point(254, 3);
            DarkGroupBox2.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox2.Name = "DarkGroupBox2";
            DarkGroupBox2.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox2.Size = new Size(483, 362);
            DarkGroupBox2.TabIndex = 1;
            DarkGroupBox2.TabStop = false;
            DarkGroupBox2.Text = "Properties";
            // 
            // nudBuy
            // 
            nudBuy.Location = new Point(103, 52);
            nudBuy.Margin = new Padding(4, 3, 4, 3);
            nudBuy.Name = "nudBuy";
            nudBuy.Size = new Size(93, 23);
            nudBuy.TabIndex = 53;
            // 
            // DarkGroupBox3
            // 
            DarkGroupBox3.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox3.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox3.Controls.Add(btnDeleteTrade);
            DarkGroupBox3.Controls.Add(btnUpdate);
            DarkGroupBox3.Controls.Add(nudCostValue);
            DarkGroupBox3.Controls.Add(cmbLabel8);
            DarkGroupBox3.Controls.Add(cmbCostItem);
            DarkGroupBox3.Controls.Add(cmbLabel7);
            DarkGroupBox3.Controls.Add(nudItemValue);
            DarkGroupBox3.Controls.Add(cmbLabel6);
            DarkGroupBox3.Controls.Add(cmbItem);
            DarkGroupBox3.Controls.Add(cmbLabel5);
            DarkGroupBox3.Controls.Add(lstTradeItem);
            DarkGroupBox3.ForeColor = Color.Gainsboro;
            DarkGroupBox3.Location = new Point(14, 81);
            DarkGroupBox3.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox3.Name = "DarkGroupBox3";
            DarkGroupBox3.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox3.Size = new Size(468, 275);
            DarkGroupBox3.TabIndex = 52;
            DarkGroupBox3.TabStop = false;
            DarkGroupBox3.Text = "Items the Shop Sells";
            // 
            // btnDeleteTrade
            // 
            btnDeleteTrade.Location = new Point(237, 243);
            btnDeleteTrade.Margin = new Padding(4, 3, 4, 3);
            btnDeleteTrade.Name = "btnDeleteTrade";
            btnDeleteTrade.Padding = new Padding(6);
            btnDeleteTrade.Size = new Size(88, 27);
            btnDeleteTrade.TabIndex = 53;
            btnDeleteTrade.Text = "Delete";
            btnDeleteTrade.Click += BtnDeleteTrade_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(142, 243);
            btnUpdate.Margin = new Padding(4, 3, 4, 3);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Padding = new Padding(6);
            btnUpdate.Size = new Size(88, 27);
            btnUpdate.TabIndex = 52;
            btnUpdate.Text = "Update";
            btnUpdate.Click += BtnUpdate_Click;
            // 
            // nudCostValue
            // 
            nudCostValue.Location = new Point(346, 212);
            nudCostValue.Margin = new Padding(4, 3, 4, 3);
            nudCostValue.Name = "nudCostValue";
            nudCostValue.Size = new Size(114, 23);
            nudCostValue.TabIndex = 51;
            // 
            // cmbLabel8
            // 
            cmbLabel8.AutoSize = true;
            cmbLabel8.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel8.Location = new Point(286, 215);
            cmbLabel8.Margin = new Padding(4, 0, 4, 0);
            cmbLabel8.Name = "cmbLabel8";
            cmbLabel8.Size = new Size(54, 15);
            cmbLabel8.TabIndex = 50;
            cmbLabel8.Text = "Amount:";
            // 
            // cmbCostItem
            // 
            cmbCostItem.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCostItem.FormattingEnabled = true;
            cmbCostItem.Location = new Point(86, 212);
            cmbCostItem.Margin = new Padding(4, 3, 4, 3);
            cmbCostItem.Name = "cmbCostItem";
            cmbCostItem.Size = new Size(192, 24);
            cmbCostItem.TabIndex = 49;
            // 
            // cmbLabel7
            // 
            cmbLabel7.AutoSize = true;
            cmbLabel7.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel7.Location = new Point(7, 216);
            cmbLabel7.Margin = new Padding(4, 0, 4, 0);
            cmbLabel7.Name = "cmbLabel7";
            cmbLabel7.Size = new Size(61, 15);
            cmbLabel7.TabIndex = 48;
            cmbLabel7.Text = "Item Cost:";
            // 
            // nudItemValue
            // 
            nudItemValue.Location = new Point(346, 182);
            nudItemValue.Margin = new Padding(4, 3, 4, 3);
            nudItemValue.Name = "nudItemValue";
            nudItemValue.Size = new Size(114, 23);
            nudItemValue.TabIndex = 47;
            // 
            // cmbLabel6
            // 
            cmbLabel6.AutoSize = true;
            cmbLabel6.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel6.Location = new Point(286, 185);
            cmbLabel6.Margin = new Padding(4, 0, 4, 0);
            cmbLabel6.Name = "cmbLabel6";
            cmbLabel6.Size = new Size(54, 15);
            cmbLabel6.TabIndex = 46;
            cmbLabel6.Text = "Amount:";
            // 
            // cmbItem
            // 
            cmbItem.DrawMode = DrawMode.OwnerDrawFixed;
            cmbItem.FormattingEnabled = true;
            cmbItem.Location = new Point(86, 181);
            cmbItem.Margin = new Padding(4, 3, 4, 3);
            cmbItem.Name = "cmbItem";
            cmbItem.Size = new Size(192, 24);
            cmbItem.TabIndex = 45;
            // 
            // cmbLabel5
            // 
            cmbLabel5.AutoSize = true;
            cmbLabel5.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel5.Location = new Point(7, 185);
            cmbLabel5.Margin = new Padding(4, 0, 4, 0);
            cmbLabel5.Name = "cmbLabel5";
            cmbLabel5.Size = new Size(69, 15);
            cmbLabel5.TabIndex = 44;
            cmbLabel5.Text = "Item to Sell:";
            // 
            // lstTradeItem
            // 
            lstTradeItem.BackColor = Color.FromArgb(45, 45, 48);
            lstTradeItem.BorderStyle = BorderStyle.FixedSingle;
            lstTradeItem.ForeColor = Color.Gainsboro;
            lstTradeItem.FormattingEnabled = true;
            lstTradeItem.Items.AddRange(new object[] { "1.", "2.", "3.", "4.", "5.", "6.", "7.", "8." });
            lstTradeItem.Location = new Point(9, 24);
            lstTradeItem.Margin = new Padding(4, 3, 4, 3);
            lstTradeItem.Name = "lstTradeItem";
            lstTradeItem.Size = new Size(453, 152);
            lstTradeItem.TabIndex = 43;
            // 
            // cmbLabel4
            // 
            cmbLabel4.AutoSize = true;
            cmbLabel4.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel4.Location = new Point(204, 54);
            cmbLabel4.Margin = new Padding(4, 0, 4, 0);
            cmbLabel4.Name = "cmbLabel4";
            cmbLabel4.Size = new Size(109, 15);
            cmbLabel4.TabIndex = 51;
            cmbLabel4.Text = "% of the Item Value";
            // 
            // cmbLabel3
            // 
            cmbLabel3.AutoSize = true;
            cmbLabel3.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel3.Location = new Point(14, 60);
            cmbLabel3.Margin = new Padding(4, 0, 4, 0);
            cmbLabel3.Name = "cmbLabel3";
            cmbLabel3.Size = new Size(81, 15);
            cmbLabel3.TabIndex = 49;
            cmbLabel3.Text = "Buyback Rate:";
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.FromArgb(220, 220, 220);
            txtName.Location = new Point(69, 23);
            txtName.Margin = new Padding(4, 3, 4, 3);
            txtName.Name = "txtName";
            txtName.Size = new Size(264, 23);
            txtName.TabIndex = 46;
            txtName.TextChanged += TxtName_TextChanged;
            // 
            // cmbLabel1
            // 
            cmbLabel1.AutoSize = true;
            cmbLabel1.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel1.Location = new Point(14, 23);
            cmbLabel1.Margin = new Padding(4, 0, 4, 0);
            cmbLabel1.Name = "cmbLabel1";
            cmbLabel1.Size = new Size(42, 15);
            cmbLabel1.TabIndex = 45;
            cmbLabel1.Text = "Name:";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(13, 338);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(6);
            btnCancel.Size = new Size(228, 27);
            btnCancel.TabIndex = 55;
            btnCancel.Text = "Cancel";
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(13, 305);
            btnDelete.Margin = new Padding(4, 3, 4, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(6);
            btnDelete.Size = new Size(228, 27);
            btnDelete.TabIndex = 54;
            btnDelete.Text = "Delete";
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(13, 272);
            btnSave.Margin = new Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(6);
            btnSave.Size = new Size(228, 27);
            btnSave.TabIndex = 53;
            btnSave.Text = "Save";
            btnSave.Click += BtnSave_Click;
            // 
            // Editor_Shop
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(744, 374);
            Controls.Add(btnCancel);
            Controls.Add(DarkGroupBox2);
            Controls.Add(btnDelete);
            Controls.Add(DarkGroupBox1);
            Controls.Add(btnSave);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Editor_Shop";
            Text = "Shop Editor";
            FormClosing += Editor_Shop_FormClosing;
            Load += Editor_Shop_Load;
            DarkGroupBox1.ResumeLayout(false);
            DarkGroupBox2.ResumeLayout(false);
            DarkGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudBuy).EndInit();
            DarkGroupBox3.ResumeLayout(false);
            DarkGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudCostValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudItemValue).EndInit();
            ResumeLayout(false);

        }

        internal DarkGroupBox DarkGroupBox1;
        internal ListBox lstIndex;
        internal DarkGroupBox DarkGroupBox2;
        internal DarkTextBox txtName;
        internal cmbLabel cmbLabel1;
        internal cmbLabel cmbLabel4;
        internal cmbLabel cmbLabel3;
        internal DarkGroupBox DarkGroupBox3;
        internal DarkComboBox cmbItem;
        internal cmbLabel cmbLabel5;
        internal ListBox lstTradeItem;
        internal DarkNumericUpDown nudItemValue;
        internal cmbLabel cmbLabel6;
        internal cmbLabel cmbLabel7;
        internal DarkComboBox cmbCostItem;
        internal DarkNumericUpDown nudCostValue;
        internal cmbLabel cmbLabel8;
        internal DarkButton btnUpdate;
        internal DarkButton btnDeleteTrade;
        internal DarkButton btnCancel;
        internal DarkButton btnDelete;
        internal DarkButton btnSave;
        internal DarkNumericUpDown nudBuy;
    }
}