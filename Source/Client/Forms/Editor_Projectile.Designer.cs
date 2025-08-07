using DarkUI.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public partial class Editor_Projectile : Form
    {

        // Shared instance of the form
        private static Editor_Projectile _instance;

        // Public property to get the shared instance
        public static Editor_Projectile Instance
        {
            get
            {
                // Create a new instance if one does not exist or if it has been disposed
                if (_instance is null || _instance.IsDisposed)
                {
                    _instance = new Editor_Projectile();
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
            lstIndex.Click += new EventHandler(lstIndex_Click);
            DarkGroupBox2 = new DarkUI.Controls.DarkGroupBox();
            cmbLabel5 = new DarkUI.Controls.cmbLabel();
            cmbLabel4 = new DarkUI.Controls.cmbLabel();
            nudDamage = new DarkUI.Controls.DarkNumericUpDown();
            nudDamage.Click += new EventHandler(NudDamage_ValueChanged);
            nudSpeed = new DarkUI.Controls.DarkNumericUpDown();
            nudSpeed.Click += new EventHandler(NudSpeed_ValueChanged);
            cmbLabel3 = new DarkUI.Controls.cmbLabel();
            nudRange = new DarkUI.Controls.DarkNumericUpDown();
            nudRange.Click += new EventHandler(NudRange_ValueChanged);
            nudPic = new DarkUI.Controls.DarkNumericUpDown();
            nudPic.Click += new EventHandler(NudPic_ValueChanged);
            cmbLabel2 = new DarkUI.Controls.cmbLabel();
            picProjectile = new PictureBox();
            txtName = new DarkUI.Controls.DarkTextBox();
            txtName.TextChanged += new EventHandler(TxtName_TextChanged);
            cmbLabel1 = new DarkUI.Controls.cmbLabel();
            btnSave = new DarkUI.Controls.DarkButton();
            btnSave.Click += new EventHandler(BtnSave_Click);
            btnCancel = new DarkUI.Controls.DarkButton();
            btnCancel.Click += new EventHandler(BtnCancel_Click);
            btnDelete = new DarkUI.Controls.DarkButton();
            btnDelete.Click += new EventHandler(btnDelete_Click);
            DarkGroupBox1.SuspendLayout();
            DarkGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDamage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRange).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picProjectile).BeginInit();
            SuspendLayout();
            // 
            // DarkGroupBox1
            // 
            DarkGroupBox1.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox1.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox1.Controls.Add(lstIndex);
            DarkGroupBox1.ForeColor = Color.Gainsboro;
            DarkGroupBox1.Location = new Point(8, 6);
            DarkGroupBox1.Margin = new Padding(8, 6, 8, 6);
            DarkGroupBox1.Name = "DarkGroupBox1";
            DarkGroupBox1.Padding = new Padding(8, 6, 8, 6);
            DarkGroupBox1.Size = new Size(407, 460);
            DarkGroupBox1.TabIndex = 0;
            DarkGroupBox1.TabStop = false;
            DarkGroupBox1.Text = "Projectile List";
            // 
            // lstIndex
            // 
            lstIndex.BackColor = Color.FromArgb(45, 45, 48);
            lstIndex.BorderStyle = BorderStyle.FixedSingle;
            lstIndex.ForeColor = Color.Gainsboro;
            lstIndex.FormattingEnabled = true;
            lstIndex.Location = new Point(13, 43);
            lstIndex.Margin = new Padding(8, 6, 8, 6);
            lstIndex.Name = "lstIndex";
            lstIndex.Size = new Size(379, 386);
            lstIndex.TabIndex = 1;
            // 
            // DarkGroupBox2
            // 
            DarkGroupBox2.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox2.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox2.Controls.Add(cmbLabel5);
            DarkGroupBox2.Controls.Add(cmbLabel4);
            DarkGroupBox2.Controls.Add(nudDamage);
            DarkGroupBox2.Controls.Add(nudSpeed);
            DarkGroupBox2.Controls.Add(cmbLabel3);
            DarkGroupBox2.Controls.Add(nudRange);
            DarkGroupBox2.Controls.Add(nudPic);
            DarkGroupBox2.Controls.Add(cmbLabel2);
            DarkGroupBox2.Controls.Add(picProjectile);
            DarkGroupBox2.Controls.Add(txtName);
            DarkGroupBox2.Controls.Add(cmbLabel1);
            DarkGroupBox2.ForeColor = Color.Gainsboro;
            DarkGroupBox2.Location = new Point(427, 6);
            DarkGroupBox2.Margin = new Padding(8, 6, 8, 6);
            DarkGroupBox2.Name = "DarkGroupBox2";
            DarkGroupBox2.Padding = new Padding(8, 6, 8, 6);
            DarkGroupBox2.Size = new Size(538, 672);
            DarkGroupBox2.TabIndex = 1;
            DarkGroupBox2.TabStop = false;
            DarkGroupBox2.Text = "Properties";
            // 
            // cmbLabel5
            // 
            cmbLabel5.AutoSize = true;
            cmbLabel5.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel5.Location = new Point(13, 480);
            cmbLabel5.Margin = new Padding(8, 0, 8, 0);
            cmbLabel5.Name = "cmbLabel5";
            cmbLabel5.Size = new Size(224, 32);
            cmbLabel5.TabIndex = 11;
            cmbLabel5.Text = "Additional Damage:";
            // 
            // cmbLabel4
            // 
            cmbLabel4.AutoSize = true;
            cmbLabel4.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel4.Location = new Point(13, 416);
            cmbLabel4.Margin = new Padding(8, 0, 8, 0);
            cmbLabel4.Name = "cmbLabel4";
            cmbLabel4.Size = new Size(86, 32);
            cmbLabel4.TabIndex = 10;
            cmbLabel4.Text = "Speed:";
            // 
            // nudDamage
            // 
            nudDamage.Location = new Point(258, 476);
            nudDamage.Margin = new Padding(8, 6, 8, 6);
            nudDamage.Name = "nudDamage";
            nudDamage.Size = new Size(260, 39);
            nudDamage.TabIndex = 9;
            // 
            // nudSpeed
            // 
            nudSpeed.Location = new Point(258, 412);
            nudSpeed.Margin = new Padding(8, 6, 8, 6);
            nudSpeed.Name = "nudSpeed";
            nudSpeed.Size = new Size(260, 39);
            nudSpeed.TabIndex = 8;
            // 
            // cmbLabel3
            // 
            cmbLabel3.AutoSize = true;
            cmbLabel3.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel3.Location = new Point(13, 352);
            cmbLabel3.Margin = new Padding(8, 0, 8, 0);
            cmbLabel3.Name = "cmbLabel3";
            cmbLabel3.Size = new Size(86, 32);
            cmbLabel3.TabIndex = 7;
            cmbLabel3.Text = "Range:";
            // 
            // nudRange
            // 
            nudRange.Location = new Point(258, 348);
            nudRange.Margin = new Padding(8, 6, 8, 6);
            nudRange.Name = "nudRange";
            nudRange.Size = new Size(260, 39);
            nudRange.TabIndex = 6;
            // 
            // nudPic
            // 
            nudPic.Location = new Point(258, 284);
            nudPic.Margin = new Padding(8, 6, 8, 6);
            nudPic.Name = "nudPic";
            nudPic.Size = new Size(260, 39);
            nudPic.TabIndex = 5;
            // 
            // cmbLabel2
            // 
            cmbLabel2.AutoSize = true;
            cmbLabel2.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel2.Location = new Point(13, 288);
            cmbLabel2.Margin = new Padding(8, 0, 8, 0);
            cmbLabel2.Name = "cmbLabel2";
            cmbLabel2.Size = new Size(92, 32);
            cmbLabel2.TabIndex = 4;
            cmbLabel2.Text = "Picture:";
            // 
            // picProjectile
            // 
            picProjectile.BackColor = Color.Black;
            picProjectile.Location = new Point(18, 111);
            picProjectile.Margin = new Padding(8, 6, 8, 6);
            picProjectile.Name = "picProjectile";
            picProjectile.Size = new Size(498, 158);
            picProjectile.TabIndex = 3;
            picProjectile.TabStop = false;
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.FromArgb(220, 220, 220);
            txtName.Location = new Point(208, 47);
            txtName.Margin = new Padding(8, 6, 8, 6);
            txtName.Name = "txtName";
            txtName.Size = new Size(306, 39);
            txtName.TabIndex = 1;
            // 
            // cmbLabel1
            // 
            cmbLabel1.AutoSize = true;
            cmbLabel1.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel1.Location = new Point(13, 52);
            cmbLabel1.Margin = new Padding(8, 0, 8, 0);
            cmbLabel1.Name = "cmbLabel1";
            cmbLabel1.Size = new Size(83, 32);
            cmbLabel1.TabIndex = 0;
            cmbLabel1.Text = "Name:";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(21, 480);
            btnSave.Margin = new Padding(8, 6, 8, 6);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(11, 12, 11, 12);
            btnSave.Size = new Size(381, 58);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(21, 620);
            btnCancel.Margin = new Padding(8, 6, 8, 6);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(11, 12, 11, 12);
            btnCancel.Size = new Size(381, 58);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(21, 550);
            btnDelete.Margin = new Padding(8, 6, 8, 6);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(11, 12, 11, 12);
            btnDelete.Size = new Size(381, 58);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Delete";
            // 
            // Editor_Projectile
            // 
            AutoScaleDimensions = new SizeF(13.0f, 32.0f);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(979, 689);
            Controls.Add(btnDelete);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(DarkGroupBox2);
            Controls.Add(DarkGroupBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(8, 6, 8, 6);
            Name = "Editor_Projectile";
            Text = "Projectile Editor";
            DarkGroupBox1.ResumeLayout(false);
            DarkGroupBox2.ResumeLayout(false);
            DarkGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudDamage).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRange).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPic).EndInit();
            ((System.ComponentModel.ISupportInitialize)picProjectile).EndInit();
            Load += new EventHandler(Editor_Projectile_Load);
            FormClosing += new FormClosingEventHandler(Editor_Projectile_FormClosing);
            ResumeLayout(false);

        }

        internal DarkGroupBox DarkGroupBox1;
        internal ListBox lstIndex;
        internal DarkGroupBox DarkGroupBox2;
        internal DarkTextBox txtName;
        internal cmbLabel cmbLabel1;
        internal PictureBox picProjectile;
        internal DarkNumericUpDown nudRange;
        internal DarkNumericUpDown nudPic;
        internal cmbLabel cmbLabel2;
        internal cmbLabel cmbLabel3;
        internal DarkNumericUpDown nudDamage;
        internal DarkNumericUpDown nudSpeed;
        internal cmbLabel cmbLabel4;
        internal cmbLabel cmbLabel5;
        internal DarkButton btnSave;
        internal DarkButton btnCancel;
        internal DarkButton btnDelete;
    }
}