using DarkUI.Controls;
using DarkUI.Forms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public partial class Editor_Item : DarkForm
    {

        // Shared instance of the form
        private static Editor_Item _instance;

        // Public property to get the shared instance
        public static Editor_Item Instance
        {
            get
            {
                // Create a new instance if one does not exist or if it has been disposed
                if (_instance is null || _instance.IsDisposed)
                {
                    _instance = new Editor_Item();
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
            cmbLabel1 = new DarkUI.Controls.cmbLabel();
            cmbLabel35 = new DarkUI.Controls.cmbLabel();
            fraEquipment = new DarkUI.Controls.DarkGroupBox();
            fraProjectile = new DarkUI.Controls.DarkGroupBox();
            cmbAmmo = new DarkUI.Controls.DarkComboBox();
            cmbLabel25 = new DarkUI.Controls.cmbLabel();
            cmbProjectile = new DarkUI.Controls.DarkComboBox();
            cmbLabel24 = new DarkUI.Controls.cmbLabel();
            nudPaperdoll = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel23 = new DarkUI.Controls.cmbLabel();
            picPaperdoll = new PictureBox();
            cmbKnockBackTiles = new DarkUI.Controls.DarkComboBox();
            cmbLabel16 = new DarkUI.Controls.cmbLabel();
            chkKnockBack = new DarkUI.Controls.DarkCheckBox();
            nudSpeed = new DarkUI.Controls.DarkNumericUpDown();
            lblSpeed = new DarkUI.Controls.cmbLabel();
            nudDamage = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel15 = new DarkUI.Controls.cmbLabel();
            cmbTool = new DarkUI.Controls.DarkComboBox();
            cmbLabel14 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox2 = new DarkUI.Controls.DarkGroupBox();
            nudSpirit = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel22 = new DarkUI.Controls.cmbLabel();
            nudIntelligence = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel21 = new DarkUI.Controls.cmbLabel();
            nudVitality = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel20 = new DarkUI.Controls.cmbLabel();
            nudLuck = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel19 = new DarkUI.Controls.cmbLabel();
            nudStrength = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel17 = new DarkUI.Controls.cmbLabel();
            btnBasics = new DarkUI.Controls.DarkButton();
            btnRequirements = new DarkUI.Controls.DarkButton();
            fraRequirements = new DarkUI.Controls.DarkGroupBox();
            cmbLabel28 = new DarkUI.Controls.cmbLabel();
            nudLevelReq = new DarkUI.Controls.DarkNumericUpDown();
            cmbAccessReq = new DarkUI.Controls.DarkComboBox();
            cmbLabel27 = new DarkUI.Controls.cmbLabel();
            cmbJobReq = new DarkUI.Controls.DarkComboBox();
            cmbLabel26 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox4 = new DarkUI.Controls.DarkGroupBox();
            nudSprReq = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel32 = new DarkUI.Controls.cmbLabel();
            nudIntReq = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel33 = new DarkUI.Controls.cmbLabel();
            nudVitReq = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel34 = new DarkUI.Controls.cmbLabel();
            nudLuckReq = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel29 = new DarkUI.Controls.cmbLabel();
            nudStrReq = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel31 = new DarkUI.Controls.cmbLabel();
            btnSave = new DarkUI.Controls.DarkButton();
            btnDelete = new DarkUI.Controls.DarkButton();
            btnCancel = new DarkUI.Controls.DarkButton();
            btnSpawn = new DarkUI.Controls.DarkButton();
            nudSpanwAmount = new DarkUI.Controls.DarkNumericUpDown();
            fraSkill = new DarkUI.Controls.DarkGroupBox();
            cmbSkills = new DarkUI.Controls.DarkComboBox();
            cmbLabel12 = new DarkUI.Controls.cmbLabel();
            fraVitals = new DarkUI.Controls.DarkGroupBox();
            nudVitalMod = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel11 = new DarkUI.Controls.cmbLabel();
            txtName = new DarkUI.Controls.DarkTextBox();
            cmbLabel2 = new DarkUI.Controls.cmbLabel();
            nudIcon = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel3 = new DarkUI.Controls.cmbLabel();
            nudRarity = new DarkUI.Controls.DarkNumericUpDown();
            picItem = new PictureBox();
            cmbLabel4 = new DarkUI.Controls.cmbLabel();
            cmbType = new DarkUI.Controls.DarkComboBox();
            cmbLabel5 = new DarkUI.Controls.cmbLabel();
            cmbSubType = new DarkUI.Controls.DarkComboBox();
            chkStackable = new DarkUI.Controls.DarkCheckBox();
            cmbLabel6 = new DarkUI.Controls.cmbLabel();
            cmbBind = new DarkUI.Controls.DarkComboBox();
            cmbLabel7 = new DarkUI.Controls.cmbLabel();
            nudPrice = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel8 = new DarkUI.Controls.cmbLabel();
            nudItemLvl = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel9 = new DarkUI.Controls.cmbLabel();
            cmbAnimation = new DarkUI.Controls.DarkComboBox();
            cmbLabel10 = new DarkUI.Controls.cmbLabel();
            txtDescription = new DarkUI.Controls.DarkTextBox();
            cmbLabel13 = new DarkUI.Controls.cmbLabel();
            fraEvents = new DarkUI.Controls.DarkGroupBox();
            nudEventValue = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel39 = new DarkUI.Controls.cmbLabel();
            nudEvent = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel38 = new DarkUI.Controls.cmbLabel();
            cmbLabel36 = new DarkUI.Controls.cmbLabel();
            fraBasics = new DarkUI.Controls.DarkGroupBox();
            DarkGroupBox1.SuspendLayout();
            fraEquipment.SuspendLayout();
            fraProjectile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPaperdoll).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPaperdoll).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDamage).BeginInit();
            DarkGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpirit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudIntelligence).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVitality).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLuck).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStrength).BeginInit();
            fraRequirements.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudLevelReq).BeginInit();
            DarkGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSprReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudIntReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVitReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLuckReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStrReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSpanwAmount).BeginInit();
            fraSkill.SuspendLayout();
            fraVitals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudVitalMod).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRarity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picItem).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudItemLvl).BeginInit();
            fraEvents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudEventValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudEvent).BeginInit();
            fraBasics.SuspendLayout();
            SuspendLayout();
            // 
            // DarkGroupBox1
            // 
            DarkGroupBox1.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox1.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox1.Controls.Add(lstIndex);
            DarkGroupBox1.ForeColor = Color.Gainsboro;
            DarkGroupBox1.Location = new Point(2, 2);
            DarkGroupBox1.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox1.Name = "DarkGroupBox1";
            DarkGroupBox1.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox1.Size = new Size(244, 467);
            DarkGroupBox1.TabIndex = 0;
            DarkGroupBox1.TabStop = false;
            DarkGroupBox1.Text = "Item List";
            // 
            // lstIndex
            // 
            lstIndex.BackColor = Color.FromArgb(45, 45, 48);
            lstIndex.BorderStyle = BorderStyle.FixedSingle;
            lstIndex.ForeColor = Color.Gainsboro;
            lstIndex.FormattingEnabled = true;
            lstIndex.Location = new Point(7, 16);
            lstIndex.Margin = new Padding(4, 3, 4, 3);
            lstIndex.Name = "lstIndex";
            lstIndex.Size = new Size(228, 452);
            lstIndex.TabIndex = 1;
            lstIndex.Click += lstIndex_Click;
            // 
            // cmbLabel1
            // 
            cmbLabel1.AutoSize = true;
            cmbLabel1.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel1.Location = new Point(7, 18);
            cmbLabel1.Margin = new Padding(4, 0, 4, 0);
            cmbLabel1.Name = "cmbLabel1";
            cmbLabel1.Size = new Size(42, 15);
            cmbLabel1.TabIndex = 0;
            cmbLabel1.Text = "Name:";
            // 
            // cmbLabel35
            // 
            cmbLabel35.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel35.Location = new Point(0, 0);
            cmbLabel35.Name = "cmbLabel35";
            cmbLabel35.Size = new Size(100, 23);
            cmbLabel35.TabIndex = 0;
            // 
            // fraEquipment
            // 
            fraEquipment.BackColor = Color.FromArgb(45, 45, 48);
            fraEquipment.BorderColor = Color.FromArgb(90, 90, 90);
            fraEquipment.Controls.Add(fraProjectile);
            fraEquipment.Controls.Add(nudPaperdoll);
            fraEquipment.Controls.Add(cmbLabel23);
            fraEquipment.Controls.Add(picPaperdoll);
            fraEquipment.Controls.Add(cmbKnockBackTiles);
            fraEquipment.Controls.Add(cmbLabel16);
            fraEquipment.Controls.Add(chkKnockBack);
            fraEquipment.Controls.Add(nudSpeed);
            fraEquipment.Controls.Add(lblSpeed);
            fraEquipment.Controls.Add(nudDamage);
            fraEquipment.Controls.Add(cmbLabel15);
            fraEquipment.Controls.Add(cmbTool);
            fraEquipment.Controls.Add(cmbLabel14);
            fraEquipment.Controls.Add(DarkGroupBox2);
            fraEquipment.ForeColor = Color.Gainsboro;
            fraEquipment.Location = new Point(253, 283);
            fraEquipment.Margin = new Padding(4, 3, 4, 3);
            fraEquipment.Name = "fraEquipment";
            fraEquipment.Padding = new Padding(4, 3, 4, 3);
            fraEquipment.Size = new Size(525, 283);
            fraEquipment.TabIndex = 2;
            fraEquipment.TabStop = false;
            fraEquipment.Text = "Equipment Settings";
            // 
            // fraProjectile
            // 
            fraProjectile.BackColor = Color.FromArgb(45, 45, 48);
            fraProjectile.BorderColor = Color.FromArgb(90, 90, 90);
            fraProjectile.Controls.Add(cmbAmmo);
            fraProjectile.Controls.Add(cmbLabel25);
            fraProjectile.Controls.Add(cmbProjectile);
            fraProjectile.Controls.Add(cmbLabel24);
            fraProjectile.ForeColor = Color.Gainsboro;
            fraProjectile.Location = new Point(140, 192);
            fraProjectile.Margin = new Padding(4, 3, 4, 3);
            fraProjectile.Name = "fraProjectile";
            fraProjectile.Padding = new Padding(4, 3, 4, 3);
            fraProjectile.Size = new Size(379, 80);
            fraProjectile.TabIndex = 63;
            fraProjectile.TabStop = false;
            fraProjectile.Text = "Projectile Settings";
            // 
            // cmbAmmo
            // 
            cmbAmmo.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAmmo.FormattingEnabled = true;
            cmbAmmo.Location = new Point(75, 46);
            cmbAmmo.Margin = new Padding(4, 3, 4, 3);
            cmbAmmo.Name = "cmbAmmo";
            cmbAmmo.Size = new Size(296, 24);
            cmbAmmo.TabIndex = 3;
            cmbAmmo.SelectedIndexChanged += CmbAmmo_SelectedIndexChanged;
            // 
            // cmbLabel25
            // 
            cmbLabel25.AutoSize = true;
            cmbLabel25.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel25.Location = new Point(22, 50);
            cmbLabel25.Margin = new Padding(4, 0, 4, 0);
            cmbLabel25.Name = "cmbLabel25";
            cmbLabel25.Size = new Size(47, 15);
            cmbLabel25.TabIndex = 2;
            cmbLabel25.Text = "Ammo:";
            // 
            // cmbProjectile
            // 
            cmbProjectile.DrawMode = DrawMode.OwnerDrawFixed;
            cmbProjectile.FormattingEnabled = true;
            cmbProjectile.Location = new Point(75, 15);
            cmbProjectile.Margin = new Padding(4, 3, 4, 3);
            cmbProjectile.Name = "cmbProjectile";
            cmbProjectile.Size = new Size(296, 24);
            cmbProjectile.TabIndex = 1;
            cmbProjectile.SelectedIndexChanged += CmbProjectile_SelectedIndexChanged;
            // 
            // cmbLabel24
            // 
            cmbLabel24.AutoSize = true;
            cmbLabel24.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel24.Location = new Point(9, 18);
            cmbLabel24.Margin = new Padding(4, 0, 4, 0);
            cmbLabel24.Name = "cmbLabel24";
            cmbLabel24.Size = new Size(59, 15);
            cmbLabel24.TabIndex = 0;
            cmbLabel24.Text = "Projectile:";
            // 
            // nudPaperdoll
            // 
            nudPaperdoll.Location = new Point(77, 249);
            nudPaperdoll.Margin = new Padding(4, 3, 4, 3);
            nudPaperdoll.Name = "nudPaperdoll";
            nudPaperdoll.Size = new Size(55, 23);
            nudPaperdoll.TabIndex = 59;
            nudPaperdoll.Click += NudPaperdoll_ValueChanged;
            // 
            // cmbLabel23
            // 
            cmbLabel23.AutoSize = true;
            cmbLabel23.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel23.Location = new Point(7, 252);
            cmbLabel23.Margin = new Padding(4, 0, 4, 0);
            cmbLabel23.Name = "cmbLabel23";
            cmbLabel23.Size = new Size(60, 15);
            cmbLabel23.TabIndex = 58;
            cmbLabel23.Text = "Paperdoll:";
            // 
            // picPaperdoll
            // 
            picPaperdoll.BackColor = Color.Black;
            picPaperdoll.Location = new Point(8, 192);
            picPaperdoll.Margin = new Padding(4, 3, 4, 3);
            picPaperdoll.Name = "picPaperdoll";
            picPaperdoll.Size = new Size(125, 55);
            picPaperdoll.TabIndex = 57;
            picPaperdoll.TabStop = false;
            // 
            // cmbKnockBackTiles
            // 
            cmbKnockBackTiles.DrawMode = DrawMode.OwnerDrawFixed;
            cmbKnockBackTiles.FormattingEnabled = true;
            cmbKnockBackTiles.Items.AddRange(new object[] { "No KnockBack", "1 Tile", "2 Tiles", "3 Tiles", "4 Tiles", "5 Tiles" });
            cmbKnockBackTiles.Location = new Point(379, 67);
            cmbKnockBackTiles.Margin = new Padding(4, 3, 4, 3);
            cmbKnockBackTiles.Name = "cmbKnockBackTiles";
            cmbKnockBackTiles.Size = new Size(138, 24);
            cmbKnockBackTiles.TabIndex = 8;
            cmbKnockBackTiles.SelectedIndexChanged += CmbKnockBackTiles_SelectedIndexChanged;
            // 
            // cmbLabel16
            // 
            cmbLabel16.AutoSize = true;
            cmbLabel16.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel16.Location = new Point(351, 70);
            cmbLabel16.Margin = new Padding(4, 0, 4, 0);
            cmbLabel16.Name = "cmbLabel16";
            cmbLabel16.Size = new Size(20, 15);
            cmbLabel16.TabIndex = 7;
            cmbLabel16.Text = "Of";
            // 
            // chkKnockBack
            // 
            chkKnockBack.AutoSize = true;
            chkKnockBack.Location = new Point(230, 69);
            chkKnockBack.Margin = new Padding(4, 3, 4, 3);
            chkKnockBack.Name = "chkKnockBack";
            chkKnockBack.Size = new Size(107, 19);
            chkKnockBack.TabIndex = 6;
            chkKnockBack.Text = "Has KnockBack";
            chkKnockBack.CheckedChanged += ChkKnockBack_CheckedChanged;
            // 
            // nudSpeed
            // 
            nudSpeed.Location = new Point(116, 68);
            nudSpeed.Margin = new Padding(4, 3, 4, 3);
            nudSpeed.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            nudSpeed.Name = "nudSpeed";
            nudSpeed.Size = new Size(107, 23);
            nudSpeed.TabIndex = 5;
            nudSpeed.Click += NudSpeed_ValueChanged;
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.ForeColor = Color.FromArgb(220, 220, 220);
            lblSpeed.Location = new Point(7, 70);
            lblSpeed.Margin = new Padding(4, 0, 4, 0);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(60, 15);
            lblSpeed.TabIndex = 4;
            lblSpeed.Text = "Speed: 0.1";
            // 
            // nudDamage
            // 
            nudDamage.Location = new Point(295, 23);
            nudDamage.Margin = new Padding(4, 3, 4, 3);
            nudDamage.Name = "nudDamage";
            nudDamage.Size = new Size(140, 23);
            nudDamage.TabIndex = 3;
            nudDamage.Click += NudDamage_ValueChanged;
            // 
            // cmbLabel15
            // 
            cmbLabel15.AutoSize = true;
            cmbLabel15.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel15.Location = new Point(230, 25);
            cmbLabel15.Margin = new Padding(4, 0, 4, 0);
            cmbLabel15.Name = "cmbLabel15";
            cmbLabel15.Size = new Size(54, 15);
            cmbLabel15.TabIndex = 2;
            cmbLabel15.Text = "Damage:";
            // 
            // cmbTool
            // 
            cmbTool.DrawMode = DrawMode.OwnerDrawFixed;
            cmbTool.FormattingEnabled = true;
            cmbTool.Items.AddRange(new object[] { "None", "Hatchet", "Rod", "Pickaxe", "Hoe" });
            cmbTool.Location = new Point(82, 22);
            cmbTool.Margin = new Padding(4, 3, 4, 3);
            cmbTool.Name = "cmbTool";
            cmbTool.Size = new Size(140, 24);
            cmbTool.TabIndex = 1;
            cmbTool.SelectedIndexChanged += CmbTool_SelectedIndexChanged;
            // 
            // cmbLabel14
            // 
            cmbLabel14.AutoSize = true;
            cmbLabel14.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel14.Location = new Point(7, 25);
            cmbLabel14.Margin = new Padding(4, 0, 4, 0);
            cmbLabel14.Name = "cmbLabel14";
            cmbLabel14.Size = new Size(59, 15);
            cmbLabel14.TabIndex = 0;
            cmbLabel14.Text = "Tool Type:";
            // 
            // DarkGroupBox2
            // 
            DarkGroupBox2.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox2.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox2.Controls.Add(nudSpirit);
            DarkGroupBox2.Controls.Add(cmbLabel22);
            DarkGroupBox2.Controls.Add(nudIntelligence);
            DarkGroupBox2.Controls.Add(cmbLabel21);
            DarkGroupBox2.Controls.Add(nudVitality);
            DarkGroupBox2.Controls.Add(cmbLabel20);
            DarkGroupBox2.Controls.Add(nudLuck);
            DarkGroupBox2.Controls.Add(cmbLabel19);
            DarkGroupBox2.Controls.Add(nudStrength);
            DarkGroupBox2.Controls.Add(cmbLabel17);
            DarkGroupBox2.ForeColor = Color.Gainsboro;
            DarkGroupBox2.Location = new Point(7, 98);
            DarkGroupBox2.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox2.Name = "DarkGroupBox2";
            DarkGroupBox2.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox2.Size = new Size(511, 88);
            DarkGroupBox2.TabIndex = 9;
            DarkGroupBox2.TabStop = false;
            DarkGroupBox2.Text = "Stats";
            // 
            // nudSpirit
            // 
            nudSpirit.Location = new Point(195, 50);
            nudSpirit.Margin = new Padding(4, 3, 4, 3);
            nudSpirit.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudSpirit.Name = "nudSpirit";
            nudSpirit.Size = new Size(58, 23);
            nudSpirit.TabIndex = 12;
            nudSpirit.Click += NudSpirit_ValueChanged;
            // 
            // cmbLabel22
            // 
            cmbLabel22.AutoSize = true;
            cmbLabel22.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel22.Location = new Point(146, 52);
            cmbLabel22.Margin = new Padding(4, 0, 4, 0);
            cmbLabel22.Name = "cmbLabel22";
            cmbLabel22.Size = new Size(37, 15);
            cmbLabel22.TabIndex = 11;
            cmbLabel22.Text = "Spirit:";
            // 
            // nudIntelligence
            // 
            nudIntelligence.Location = new Point(81, 50);
            nudIntelligence.Margin = new Padding(4, 3, 4, 3);
            nudIntelligence.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudIntelligence.Name = "nudIntelligence";
            nudIntelligence.Size = new Size(58, 23);
            nudIntelligence.TabIndex = 10;
            nudIntelligence.Click += NudIntelligence_ValueChanged;
            // 
            // cmbLabel21
            // 
            cmbLabel21.AutoSize = true;
            cmbLabel21.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel21.Location = new Point(7, 52);
            cmbLabel21.Margin = new Padding(4, 0, 4, 0);
            cmbLabel21.Name = "cmbLabel21";
            cmbLabel21.Size = new Size(71, 15);
            cmbLabel21.TabIndex = 9;
            cmbLabel21.Text = "Intelligence:";
            // 
            // nudVitality
            // 
            nudVitality.Location = new Point(56, 21);
            nudVitality.Margin = new Padding(4, 3, 4, 3);
            nudVitality.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudVitality.Name = "nudVitality";
            nudVitality.Size = new Size(58, 23);
            nudVitality.TabIndex = 8;
            nudVitality.Click += NudVitality_ValueChanged;
            // 
            // cmbLabel20
            // 
            cmbLabel20.AutoSize = true;
            cmbLabel20.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel20.Location = new Point(7, 23);
            cmbLabel20.Margin = new Padding(4, 0, 4, 0);
            cmbLabel20.Name = "cmbLabel20";
            cmbLabel20.Size = new Size(46, 15);
            cmbLabel20.TabIndex = 7;
            cmbLabel20.Text = "Vitality:";
            // 
            // nudLuck
            // 
            nudLuck.Location = new Point(331, 23);
            nudLuck.Margin = new Padding(4, 3, 4, 3);
            nudLuck.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudLuck.Name = "nudLuck";
            nudLuck.Size = new Size(58, 23);
            nudLuck.TabIndex = 6;
            nudLuck.Click += NudLuck_ValueChanged;
            // 
            // cmbLabel19
            // 
            cmbLabel19.AutoSize = true;
            cmbLabel19.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel19.Location = new Point(282, 25);
            cmbLabel19.Margin = new Padding(4, 0, 4, 0);
            cmbLabel19.Name = "cmbLabel19";
            cmbLabel19.Size = new Size(35, 15);
            cmbLabel19.TabIndex = 5;
            cmbLabel19.Text = "Luck:";
            // 
            // nudStrength
            // 
            nudStrength.Location = new Point(195, 21);
            nudStrength.Margin = new Padding(4, 3, 4, 3);
            nudStrength.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudStrength.Name = "nudStrength";
            nudStrength.Size = new Size(58, 23);
            nudStrength.TabIndex = 2;
            nudStrength.Click += NudStrength_ValueChanged;
            // 
            // cmbLabel17
            // 
            cmbLabel17.AutoSize = true;
            cmbLabel17.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel17.Location = new Point(138, 23);
            cmbLabel17.Margin = new Padding(4, 0, 4, 0);
            cmbLabel17.Name = "cmbLabel17";
            cmbLabel17.Size = new Size(55, 15);
            cmbLabel17.TabIndex = 1;
            cmbLabel17.Text = "Strength:";
            // 
            // btnBasics
            // 
            btnBasics.Location = new Point(253, 9);
            btnBasics.Margin = new Padding(4, 3, 4, 3);
            btnBasics.Name = "btnBasics";
            btnBasics.Padding = new Padding(6);
            btnBasics.Size = new Size(88, 27);
            btnBasics.TabIndex = 3;
            btnBasics.Text = "Properties";
            btnBasics.Click += BtnBasics_Click;
            // 
            // btnRequirements
            // 
            btnRequirements.Location = new Point(348, 9);
            btnRequirements.Margin = new Padding(4, 3, 4, 3);
            btnRequirements.Name = "btnRequirements";
            btnRequirements.Padding = new Padding(6);
            btnRequirements.Size = new Size(107, 27);
            btnRequirements.TabIndex = 4;
            btnRequirements.Text = "Requirements";
            btnRequirements.Click += BtnRequirements_Click;
            // 
            // fraRequirements
            // 
            fraRequirements.BackColor = Color.FromArgb(45, 45, 48);
            fraRequirements.BorderColor = Color.FromArgb(90, 90, 90);
            fraRequirements.Controls.Add(cmbLabel28);
            fraRequirements.Controls.Add(nudLevelReq);
            fraRequirements.Controls.Add(cmbAccessReq);
            fraRequirements.Controls.Add(cmbLabel27);
            fraRequirements.Controls.Add(cmbJobReq);
            fraRequirements.Controls.Add(cmbLabel26);
            fraRequirements.Controls.Add(DarkGroupBox4);
            fraRequirements.ForeColor = Color.Gainsboro;
            fraRequirements.Location = new Point(253, 43);
            fraRequirements.Margin = new Padding(4, 3, 4, 3);
            fraRequirements.Name = "fraRequirements";
            fraRequirements.Padding = new Padding(4, 3, 4, 3);
            fraRequirements.Size = new Size(525, 233);
            fraRequirements.TabIndex = 5;
            fraRequirements.TabStop = false;
            fraRequirements.Text = "Requirements";
            fraRequirements.Visible = false;
            // 
            // cmbLabel28
            // 
            cmbLabel28.AutoSize = true;
            cmbLabel28.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel28.Location = new Point(7, 78);
            cmbLabel28.Margin = new Padding(4, 0, 4, 0);
            cmbLabel28.Name = "cmbLabel28";
            cmbLabel28.Size = new Size(108, 15);
            cmbLabel28.TabIndex = 5;
            cmbLabel28.Text = "Level Requirement:";
            // 
            // nudLevelReq
            // 
            nudLevelReq.Location = new Point(140, 76);
            nudLevelReq.Margin = new Padding(4, 3, 4, 3);
            nudLevelReq.Name = "nudLevelReq";
            nudLevelReq.Size = new Size(140, 23);
            nudLevelReq.TabIndex = 4;
            nudLevelReq.Click += NudLevelReq_ValueChanged;
            // 
            // cmbAccessReq
            // 
            cmbAccessReq.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAccessReq.FormattingEnabled = true;
            cmbAccessReq.Items.AddRange(new object[] { "Player", "Moderator", "Mapper", "Developer", "Owneer" });
            cmbAccessReq.Location = new Point(140, 45);
            cmbAccessReq.Margin = new Padding(4, 3, 4, 3);
            cmbAccessReq.Name = "cmbAccessReq";
            cmbAccessReq.Size = new Size(206, 24);
            cmbAccessReq.TabIndex = 3;
            cmbAccessReq.SelectedIndexChanged += CmbAccessReq_SelectedIndexChanged;
            // 
            // cmbLabel27
            // 
            cmbLabel27.AutoSize = true;
            cmbLabel27.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel27.Location = new Point(7, 50);
            cmbLabel27.Margin = new Padding(4, 0, 4, 0);
            cmbLabel27.Name = "cmbLabel27";
            cmbLabel27.Size = new Size(117, 15);
            cmbLabel27.TabIndex = 2;
            cmbLabel27.Text = "Access Requirement:";
            // 
            // cmbJobReq
            // 
            cmbJobReq.DrawMode = DrawMode.OwnerDrawFixed;
            cmbJobReq.FormattingEnabled = true;
            cmbJobReq.Location = new Point(140, 15);
            cmbJobReq.Margin = new Padding(4, 3, 4, 3);
            cmbJobReq.Name = "cmbJobReq";
            cmbJobReq.Size = new Size(206, 24);
            cmbJobReq.TabIndex = 1;
            cmbJobReq.SelectedIndexChanged += CmbJobReq_SelectedIndexChanged;
            // 
            // cmbLabel26
            // 
            cmbLabel26.AutoSize = true;
            cmbLabel26.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel26.Location = new Point(7, 18);
            cmbLabel26.Margin = new Padding(4, 0, 4, 0);
            cmbLabel26.Name = "cmbLabel26";
            cmbLabel26.Size = new Size(99, 15);
            cmbLabel26.TabIndex = 0;
            cmbLabel26.Text = "Job Requirement:";
            // 
            // DarkGroupBox4
            // 
            DarkGroupBox4.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox4.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox4.Controls.Add(nudSprReq);
            DarkGroupBox4.Controls.Add(cmbLabel32);
            DarkGroupBox4.Controls.Add(nudIntReq);
            DarkGroupBox4.Controls.Add(cmbLabel33);
            DarkGroupBox4.Controls.Add(nudVitReq);
            DarkGroupBox4.Controls.Add(cmbLabel34);
            DarkGroupBox4.Controls.Add(nudLuckReq);
            DarkGroupBox4.Controls.Add(cmbLabel29);
            DarkGroupBox4.Controls.Add(nudStrReq);
            DarkGroupBox4.Controls.Add(cmbLabel31);
            DarkGroupBox4.ForeColor = Color.Gainsboro;
            DarkGroupBox4.Location = new Point(7, 111);
            DarkGroupBox4.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox4.Name = "DarkGroupBox4";
            DarkGroupBox4.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox4.Size = new Size(511, 115);
            DarkGroupBox4.TabIndex = 6;
            DarkGroupBox4.TabStop = false;
            DarkGroupBox4.Text = "Stat Requirements";
            // 
            // nudSprReq
            // 
            nudSprReq.Location = new Point(220, 74);
            nudSprReq.Margin = new Padding(4, 3, 4, 3);
            nudSprReq.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudSprReq.Name = "nudSprReq";
            nudSprReq.Size = new Size(58, 23);
            nudSprReq.TabIndex = 18;
            nudSprReq.Click += NudSprReq_ValueChanged;
            // 
            // cmbLabel32
            // 
            cmbLabel32.AutoSize = true;
            cmbLabel32.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel32.Location = new Point(139, 75);
            cmbLabel32.Margin = new Padding(4, 0, 4, 0);
            cmbLabel32.Name = "cmbLabel32";
            cmbLabel32.Size = new Size(37, 15);
            cmbLabel32.TabIndex = 17;
            cmbLabel32.Text = "Spirit:";
            // 
            // nudIntReq
            // 
            nudIntReq.Location = new Point(220, 25);
            nudIntReq.Margin = new Padding(4, 3, 4, 3);
            nudIntReq.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudIntReq.Name = "nudIntReq";
            nudIntReq.Size = new Size(58, 23);
            nudIntReq.TabIndex = 16;
            nudIntReq.Click += NudIntReq_ValueChanged;
            // 
            // cmbLabel33
            // 
            cmbLabel33.AutoSize = true;
            cmbLabel33.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel33.Location = new Point(139, 28);
            cmbLabel33.Margin = new Padding(4, 0, 4, 0);
            cmbLabel33.Name = "cmbLabel33";
            cmbLabel33.Size = new Size(71, 15);
            cmbLabel33.TabIndex = 15;
            cmbLabel33.Text = "Intelligence:";
            // 
            // nudVitReq
            // 
            nudVitReq.Location = new Point(72, 75);
            nudVitReq.Margin = new Padding(4, 3, 4, 3);
            nudVitReq.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudVitReq.Name = "nudVitReq";
            nudVitReq.Size = new Size(58, 23);
            nudVitReq.TabIndex = 14;
            nudVitReq.Click += NudVitReq_ValueChanged;
            // 
            // cmbLabel34
            // 
            cmbLabel34.AutoSize = true;
            cmbLabel34.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel34.Location = new Point(7, 76);
            cmbLabel34.Margin = new Padding(4, 0, 4, 0);
            cmbLabel34.Name = "cmbLabel34";
            cmbLabel34.Size = new Size(46, 15);
            cmbLabel34.TabIndex = 13;
            cmbLabel34.Text = "Vitality:";
            // 
            // nudLuckReq
            // 
            nudLuckReq.Location = new Point(337, 26);
            nudLuckReq.Margin = new Padding(4, 3, 4, 3);
            nudLuckReq.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudLuckReq.Name = "nudLuckReq";
            nudLuckReq.Size = new Size(58, 23);
            nudLuckReq.TabIndex = 12;
            nudLuckReq.Click += NudLuckReq_ValueChanged;
            // 
            // cmbLabel29
            // 
            cmbLabel29.AutoSize = true;
            cmbLabel29.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel29.Location = new Point(294, 28);
            cmbLabel29.Margin = new Padding(4, 0, 4, 0);
            cmbLabel29.Name = "cmbLabel29";
            cmbLabel29.Size = new Size(35, 15);
            cmbLabel29.TabIndex = 11;
            cmbLabel29.Text = "Luck:";
            // 
            // nudStrReq
            // 
            nudStrReq.Location = new Point(75, 27);
            nudStrReq.Margin = new Padding(4, 3, 4, 3);
            nudStrReq.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudStrReq.Name = "nudStrReq";
            nudStrReq.Size = new Size(58, 23);
            nudStrReq.TabIndex = 8;
            nudStrReq.Click += NudStrReq_ValueChanged;
            // 
            // cmbLabel31
            // 
            cmbLabel31.AutoSize = true;
            cmbLabel31.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel31.Location = new Point(7, 28);
            cmbLabel31.Margin = new Padding(4, 0, 4, 0);
            cmbLabel31.Name = "cmbLabel31";
            cmbLabel31.Size = new Size(55, 15);
            cmbLabel31.TabIndex = 7;
            cmbLabel31.Text = "Strength:";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(9, 473);
            btnSave.Margin = new Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(6);
            btnSave.Size = new Size(229, 27);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.Click += BtnSave_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(9, 506);
            btnDelete.Margin = new Padding(4, 3, 4, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(6);
            btnDelete.Size = new Size(229, 27);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "Delete";
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(9, 539);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(6);
            btnCancel.Size = new Size(229, 27);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnSpawn
            // 
            btnSpawn.Location = new Point(463, 9);
            btnSpawn.Margin = new Padding(4, 3, 4, 3);
            btnSpawn.Name = "btnSpawn";
            btnSpawn.Padding = new Padding(6);
            btnSpawn.Size = new Size(88, 27);
            btnSpawn.TabIndex = 10;
            btnSpawn.Text = "Spawn";
            btnSpawn.Click += btnSpawn_Click;
            // 
            // nudSpanwAmount
            // 
            nudSpanwAmount.Location = new Point(558, 14);
            nudSpanwAmount.Margin = new Padding(4, 3, 4, 3);
            nudSpanwAmount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSpanwAmount.Name = "nudSpanwAmount";
            nudSpanwAmount.Size = new Size(148, 23);
            nudSpanwAmount.TabIndex = 11;
            nudSpanwAmount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // fraSkill
            // 
            fraSkill.BackColor = Color.FromArgb(45, 45, 48);
            fraSkill.BorderColor = Color.FromArgb(90, 90, 90);
            fraSkill.Controls.Add(cmbSkills);
            fraSkill.Controls.Add(cmbLabel12);
            fraSkill.ForeColor = Color.Gainsboro;
            fraSkill.Location = new Point(284, 138);
            fraSkill.Margin = new Padding(4, 3, 4, 3);
            fraSkill.Name = "fraSkill";
            fraSkill.Padding = new Padding(4, 3, 4, 3);
            fraSkill.Size = new Size(233, 46);
            fraSkill.TabIndex = 24;
            fraSkill.TabStop = false;
            fraSkill.Text = "Skills";
            // 
            // cmbSkills
            // 
            cmbSkills.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkills.FormattingEnabled = true;
            cmbSkills.Location = new Point(48, 16);
            cmbSkills.Margin = new Padding(4, 3, 4, 3);
            cmbSkills.Name = "cmbSkills";
            cmbSkills.Size = new Size(178, 24);
            cmbSkills.TabIndex = 1;
            cmbSkills.SelectedIndexChanged += CmbSkills_SelectedIndexChanged;
            // 
            // cmbLabel12
            // 
            cmbLabel12.AutoSize = true;
            cmbLabel12.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel12.Location = new Point(7, 20);
            cmbLabel12.Margin = new Padding(4, 0, 4, 0);
            cmbLabel12.Name = "cmbLabel12";
            cmbLabel12.Size = new Size(31, 15);
            cmbLabel12.TabIndex = 0;
            cmbLabel12.Text = "Skill:";
            // 
            // fraVitals
            // 
            fraVitals.BackColor = Color.FromArgb(45, 45, 48);
            fraVitals.BorderColor = Color.FromArgb(90, 90, 90);
            fraVitals.Controls.Add(nudVitalMod);
            fraVitals.Controls.Add(cmbLabel11);
            fraVitals.ForeColor = Color.Gainsboro;
            fraVitals.Location = new Point(284, 138);
            fraVitals.Margin = new Padding(4, 3, 4, 3);
            fraVitals.Name = "fraVitals";
            fraVitals.Padding = new Padding(4, 3, 4, 3);
            fraVitals.Size = new Size(233, 46);
            fraVitals.TabIndex = 23;
            fraVitals.TabStop = false;
            fraVitals.Text = "Vitals";
            // 
            // nudVitalMod
            // 
            nudVitalMod.Location = new Point(78, 19);
            nudVitalMod.Margin = new Padding(4, 3, 4, 3);
            nudVitalMod.Name = "nudVitalMod";
            nudVitalMod.Size = new Size(148, 23);
            nudVitalMod.TabIndex = 1;
            nudVitalMod.Click += NudVitalMod_Click;
            // 
            // cmbLabel11
            // 
            cmbLabel11.AutoSize = true;
            cmbLabel11.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel11.Location = new Point(7, 20);
            cmbLabel11.Margin = new Padding(4, 0, 4, 0);
            cmbLabel11.Name = "cmbLabel11";
            cmbLabel11.Size = new Size(35, 15);
            cmbLabel11.TabIndex = 0;
            cmbLabel11.Text = "Mod:";
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.FromArgb(220, 220, 220);
            txtName.Location = new Point(85, 16);
            txtName.Margin = new Padding(4, 3, 4, 3);
            txtName.Name = "txtName";
            txtName.Size = new Size(266, 23);
            txtName.TabIndex = 1;
            txtName.TextChanged += TxtName_TextChanged;
            // 
            // cmbLabel2
            // 
            cmbLabel2.AutoSize = true;
            cmbLabel2.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel2.Location = new Point(358, 18);
            cmbLabel2.Margin = new Padding(4, 0, 4, 0);
            cmbLabel2.Name = "cmbLabel2";
            cmbLabel2.Size = new Size(33, 15);
            cmbLabel2.TabIndex = 2;
            cmbLabel2.Text = "Icon:";
            // 
            // nudIcon
            // 
            nudIcon.Location = new Point(415, 16);
            nudIcon.Margin = new Padding(4, 3, 4, 3);
            nudIcon.Name = "nudIcon";
            nudIcon.Size = new Size(57, 23);
            nudIcon.TabIndex = 3;
            nudIcon.Click += NudPic_Click;
            // 
            // cmbLabel3
            // 
            cmbLabel3.AutoSize = true;
            cmbLabel3.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel3.Location = new Point(358, 50);
            cmbLabel3.Margin = new Padding(4, 0, 4, 0);
            cmbLabel3.Name = "cmbLabel3";
            cmbLabel3.Size = new Size(40, 15);
            cmbLabel3.TabIndex = 4;
            cmbLabel3.Text = "Rarity:";
            // 
            // nudRarity
            // 
            nudRarity.Location = new Point(415, 46);
            nudRarity.Margin = new Padding(4, 3, 4, 3);
            nudRarity.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            nudRarity.Name = "nudRarity";
            nudRarity.Size = new Size(57, 23);
            nudRarity.TabIndex = 5;
            nudRarity.Click += NudRarity_ValueChanged;
            // 
            // picItem
            // 
            picItem.BackColor = Color.Black;
            picItem.Location = new Point(480, 16);
            picItem.Margin = new Padding(4, 3, 4, 3);
            picItem.Name = "picItem";
            picItem.Size = new Size(32, 32);
            picItem.TabIndex = 7;
            picItem.TabStop = false;
            // 
            // cmbLabel4
            // 
            cmbLabel4.AutoSize = true;
            cmbLabel4.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel4.Location = new Point(7, 50);
            cmbLabel4.Margin = new Padding(4, 0, 4, 0);
            cmbLabel4.Name = "cmbLabel4";
            cmbLabel4.Size = new Size(34, 15);
            cmbLabel4.TabIndex = 8;
            cmbLabel4.Text = "Type:";
            // 
            // cmbType
            // 
            cmbType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbType.FormattingEnabled = true;
            cmbType.Items.AddRange(new object[] { "Equipment", "Consumable", "Common Event", "Currency", "Skill", "Projectile"});
            cmbType.Location = new Point(85, 46);
            cmbType.Margin = new Padding(4, 3, 4, 3);
            cmbType.Name = "cmbType";
            cmbType.Size = new Size(140, 24);
            cmbType.TabIndex = 9;
            cmbType.SelectedIndexChanged += CmbType_SelectedIndexChanged;
            // 
            // cmbLabel5
            // 
            cmbLabel5.AutoSize = true;
            cmbLabel5.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel5.Location = new Point(7, 81);
            cmbLabel5.Margin = new Padding(4, 0, 4, 0);
            cmbLabel5.Name = "cmbLabel5";
            cmbLabel5.Size = new Size(59, 15);
            cmbLabel5.TabIndex = 10;
            cmbLabel5.Text = "Sub-Type:";
            // 
            // cmbSubType
            // 
            cmbSubType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSubType.FormattingEnabled = true;
            cmbSubType.Items.AddRange(new object[] { "Weapon", "Armor", "Helmet", "Shield", "Add HP", "Add MP", "Add SP", "Sub HP", "Sub MP", "Sub SP", "Experience", "Common Event", "Currency", "Skill" });
            cmbSubType.Location = new Point(85, 77);
            cmbSubType.Margin = new Padding(4, 3, 4, 3);
            cmbSubType.Name = "cmbSubType";
            cmbSubType.Size = new Size(140, 24);
            cmbSubType.TabIndex = 11;
            cmbSubType.SelectedIndexChanged += CmbSubType_SelectedIndexChanged;
            // 
            // chkStackable
            // 
            chkStackable.AutoSize = true;
            chkStackable.Location = new Point(265, 48);
            chkStackable.Margin = new Padding(4, 3, 4, 3);
            chkStackable.Name = "chkStackable";
            chkStackable.Size = new Size(76, 19);
            chkStackable.TabIndex = 12;
            chkStackable.Text = "Stackable";
            chkStackable.CheckedChanged += ChkStackable_CheckedChanged;
            // 
            // cmbLabel6
            // 
            cmbLabel6.AutoSize = true;
            cmbLabel6.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel6.Location = new Point(261, 81);
            cmbLabel6.Margin = new Padding(4, 0, 4, 0);
            cmbLabel6.Name = "cmbLabel6";
            cmbLabel6.Size = new Size(61, 15);
            cmbLabel6.TabIndex = 13;
            cmbLabel6.Text = "Bind Type:";
            // 
            // cmbBind
            // 
            cmbBind.DrawMode = DrawMode.OwnerDrawFixed;
            cmbBind.FormattingEnabled = true;
            cmbBind.Items.AddRange(new object[] { "None", "Bind on Pickup", "Bind on Equip" });
            cmbBind.Location = new Point(332, 77);
            cmbBind.Margin = new Padding(4, 3, 4, 3);
            cmbBind.Name = "cmbBind";
            cmbBind.Size = new Size(184, 24);
            cmbBind.TabIndex = 14;
            cmbBind.SelectedIndexChanged += CmbBind_SelectedIndexChanged;
            // 
            // cmbLabel7
            // 
            cmbLabel7.AutoSize = true;
            cmbLabel7.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel7.Location = new Point(7, 111);
            cmbLabel7.Margin = new Padding(4, 0, 4, 0);
            cmbLabel7.Name = "cmbLabel7";
            cmbLabel7.Size = new Size(36, 15);
            cmbLabel7.TabIndex = 15;
            cmbLabel7.Text = "Price:";
            // 
            // nudPrice
            // 
            nudPrice.Location = new Point(85, 108);
            nudPrice.Margin = new Padding(4, 3, 4, 3);
            nudPrice.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudPrice.Name = "nudPrice";
            nudPrice.Size = new Size(80, 23);
            nudPrice.TabIndex = 16;
            nudPrice.Click += NudPrice_ValueChanged;
            // 
            // cmbLabel8
            // 
            cmbLabel8.AutoSize = true;
            cmbLabel8.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel8.Location = new Point(173, 111);
            cmbLabel8.Margin = new Padding(4, 0, 4, 0);
            cmbLabel8.Name = "cmbLabel8";
            cmbLabel8.Size = new Size(37, 15);
            cmbLabel8.TabIndex = 17;
            cmbLabel8.Text = "Level:";
            // 
            // nudItemLvl
            // 
            nudItemLvl.Location = new Point(244, 108);
            nudItemLvl.Margin = new Padding(4, 3, 4, 3);
            nudItemLvl.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudItemLvl.Name = "nudItemLvl";
            nudItemLvl.Size = new Size(56, 23);
            nudItemLvl.TabIndex = 18;
            nudItemLvl.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudItemLvl.Click += NuditemLvl_ValueChanged;
            // 
            // cmbLabel9
            // 
            cmbLabel9.AutoSize = true;
            cmbLabel9.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel9.Location = new Point(307, 111);
            cmbLabel9.Margin = new Padding(4, 0, 4, 0);
            cmbLabel9.Name = "cmbLabel9";
            cmbLabel9.Size = new Size(66, 15);
            cmbLabel9.TabIndex = 19;
            cmbLabel9.Text = "Animation:";
            // 
            // cmbAnimation
            // 
            cmbAnimation.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAnimation.FormattingEnabled = true;
            cmbAnimation.Location = new Point(379, 107);
            cmbAnimation.Margin = new Padding(4, 3, 4, 3);
            cmbAnimation.Name = "cmbAnimation";
            cmbAnimation.Size = new Size(137, 24);
            cmbAnimation.TabIndex = 20;
            cmbAnimation.SelectedIndexChanged += CmbAnimation_SelectedIndexChanged;
            // 
            // cmbLabel10
            // 
            cmbLabel10.AutoSize = true;
            cmbLabel10.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel10.Location = new Point(7, 137);
            cmbLabel10.Margin = new Padding(4, 0, 4, 0);
            cmbLabel10.Name = "cmbLabel10";
            cmbLabel10.Size = new Size(70, 15);
            cmbLabel10.TabIndex = 21;
            cmbLabel10.Text = "Description:";
            // 
            // txtDescription
            // 
            txtDescription.BackColor = Color.FromArgb(69, 73, 74);
            txtDescription.BorderStyle = BorderStyle.FixedSingle;
            txtDescription.ForeColor = Color.FromArgb(220, 220, 220);
            txtDescription.Location = new Point(10, 156);
            txtDescription.Margin = new Padding(4, 3, 4, 3);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(266, 69);
            txtDescription.TabIndex = 22;
            txtDescription.TextChanged += TxtDescription_TextChanged;       
            // 
            // cmbLabel13
            // 
            cmbLabel13.AutoSize = true;
            cmbLabel13.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel13.Location = new Point(7, 20);
            cmbLabel13.Margin = new Padding(4, 0, 4, 0);
            cmbLabel13.Name = "cmbLabel13";
            cmbLabel13.Size = new Size(37, 15);
            cmbLabel13.TabIndex = 0;
            cmbLabel13.Text = "Num:";
            // 
            // fraEvents
            // 
            fraEvents.BackColor = Color.FromArgb(45, 45, 48);
            fraEvents.BorderColor = Color.FromArgb(90, 90, 90);
            fraEvents.Controls.Add(nudEventValue);
            fraEvents.Controls.Add(cmbLabel39);
            fraEvents.Controls.Add(nudEvent);
            fraEvents.Controls.Add(cmbLabel38);
            fraEvents.ForeColor = Color.Gainsboro;
            fraEvents.Location = new Point(292, 137);
            fraEvents.Margin = new Padding(4, 3, 4, 3);
            fraEvents.Name = "fraEvents";
            fraEvents.Padding = new Padding(4, 3, 4, 3);
            fraEvents.Size = new Size(233, 96);
            fraEvents.TabIndex = 27;
            fraEvents.TabStop = false;
            fraEvents.Text = "Events";
            // 
            // nudEventValue
            // 
            nudEventValue.Location = new Point(78, 57);
            nudEventValue.Margin = new Padding(4, 3, 4, 3);
            nudEventValue.Name = "nudEventValue";
            nudEventValue.Size = new Size(148, 23);
            nudEventValue.TabIndex = 5;
            // 
            // cmbLabel39
            // 
            cmbLabel39.AutoSize = true;
            cmbLabel39.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel39.Location = new Point(9, 61);
            cmbLabel39.Margin = new Padding(4, 0, 4, 0);
            cmbLabel39.Name = "cmbLabel39";
            cmbLabel39.Size = new Size(38, 15);
            cmbLabel39.TabIndex = 4;
            cmbLabel39.Text = "Value:";
            // 
            // nudEvent
            // 
            nudEvent.Location = new Point(78, 16);
            nudEvent.Margin = new Padding(4, 3, 4, 3);
            nudEvent.Name = "nudEvent";
            nudEvent.Size = new Size(148, 23);
            nudEvent.TabIndex = 1;
            nudEvent.ValueChanged += nudEvents_ValueChanged;
            // 
            // cmbLabel38
            // 
            cmbLabel38.AutoSize = true;
            cmbLabel38.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel38.Location = new Point(9, 20);
            cmbLabel38.Margin = new Padding(4, 0, 4, 0);
            cmbLabel38.Name = "cmbLabel38";
            cmbLabel38.Size = new Size(21, 15);
            cmbLabel38.TabIndex = 0;
            cmbLabel38.Text = "Id:";
            // 
            // cmbLabel36
            // 
            cmbLabel36.AutoSize = true;
            cmbLabel36.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel36.Location = new Point(7, 18);
            cmbLabel36.Margin = new Padding(4, 0, 4, 0);
            cmbLabel36.Name = "cmbLabel36";
            cmbLabel36.Size = new Size(42, 15);
            cmbLabel36.TabIndex = 28;
            cmbLabel36.Text = "Name:";
            // 
            // fraBasics
            // 
            fraBasics.BackColor = Color.FromArgb(45, 45, 48);
            fraBasics.BorderColor = Color.FromArgb(90, 90, 90);
            fraBasics.Controls.Add(cmbLabel36);
            fraBasics.Controls.Add(txtDescription);
            fraBasics.Controls.Add(cmbLabel10);
            fraBasics.Controls.Add(cmbAnimation);
            fraBasics.Controls.Add(cmbLabel9);
            fraBasics.Controls.Add(nudItemLvl);
            fraBasics.Controls.Add(cmbLabel8);
            fraBasics.Controls.Add(nudPrice);
            fraBasics.Controls.Add(cmbLabel7);
            fraBasics.Controls.Add(cmbBind);
            fraBasics.Controls.Add(cmbLabel6);
            fraBasics.Controls.Add(chkStackable);
            fraBasics.Controls.Add(cmbSubType);
            fraBasics.Controls.Add(cmbLabel5);
            fraBasics.Controls.Add(cmbType);
            fraBasics.Controls.Add(cmbLabel4);
            fraBasics.Controls.Add(picItem);
            fraBasics.Controls.Add(nudRarity);
            fraBasics.Controls.Add(cmbLabel3);
            fraBasics.Controls.Add(nudIcon);
            fraBasics.Controls.Add(cmbLabel2);
            fraBasics.Controls.Add(txtName);
            fraBasics.Controls.Add(fraVitals);
            fraBasics.Controls.Add(fraSkill);
            fraBasics.Controls.Add(fraEvents);
            fraBasics.ForeColor = Color.Gainsboro;
            fraBasics.Location = new Point(253, 43);
            fraBasics.Margin = new Padding(4, 3, 4, 3);
            fraBasics.Name = "fraBasics";
            fraBasics.Padding = new Padding(4, 3, 4, 3);
            fraBasics.Size = new Size(525, 233);
            fraBasics.TabIndex = 1;
            fraBasics.TabStop = false;
            fraBasics.Text = "Properties";
            // 
            // Editor_Item
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(785, 572);
            Controls.Add(nudSpanwAmount);
            Controls.Add(btnSpawn);
            Controls.Add(btnCancel);
            Controls.Add(btnDelete);
            Controls.Add(btnSave);
            Controls.Add(btnRequirements);
            Controls.Add(btnBasics);
            Controls.Add(DarkGroupBox1);
            Controls.Add(fraBasics);
            Controls.Add(fraRequirements);
            Controls.Add(fraEquipment);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(4, 3, 4, 3);
            Name = "Editor_Item";
            Text = "Item Editor";
            FormClosing += Editor_Item_FormClosing;
            Load += Editor_Item_Load;
            DarkGroupBox1.ResumeLayout(false);
            fraEquipment.ResumeLayout(false);
            fraEquipment.PerformLayout();
            fraProjectile.ResumeLayout(false);
            fraProjectile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPaperdoll).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPaperdoll).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDamage).EndInit();
            DarkGroupBox2.ResumeLayout(false);
            DarkGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpirit).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudIntelligence).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVitality).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLuck).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStrength).EndInit();
            fraRequirements.ResumeLayout(false);
            fraRequirements.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudLevelReq).EndInit();
            DarkGroupBox4.ResumeLayout(false);
            DarkGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSprReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudIntReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVitReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLuckReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStrReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSpanwAmount).EndInit();
            fraSkill.ResumeLayout(false);
            fraSkill.PerformLayout();
            fraVitals.ResumeLayout(false);
            fraVitals.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudVitalMod).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRarity).EndInit();
            ((System.ComponentModel.ISupportInitialize)picItem).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudItemLvl).EndInit();
            fraEvents.ResumeLayout(false);
            fraEvents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudEventValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudEvent).EndInit();
            fraBasics.ResumeLayout(false);
            fraBasics.PerformLayout();
            ResumeLayout(false);
        }

        internal DarkGroupBox DarkGroupBox1;
        internal ListBox lstIndex;
        internal cmbLabel cmbLabel1;
        internal DarkGroupBox fraEquipment;
        internal DarkComboBox cmbTool;
        internal cmbLabel cmbLabel14;
        internal cmbLabel cmbLabel15;
        internal DarkNumericUpDown nudDamage;
        internal cmbLabel lblSpeed;
        internal DarkNumericUpDown nudSpeed;
        internal DarkCheckBox chkKnockBack;
        internal DarkComboBox cmbKnockBackTiles;
        internal cmbLabel cmbLabel16;
        internal DarkGroupBox DarkGroupBox2;
        internal cmbLabel cmbLabel17;
        internal DarkNumericUpDown nudStrength;
        internal DarkNumericUpDown nudLuck;
        internal cmbLabel cmbLabel19;
        internal DarkNumericUpDown nudVitality;
        internal cmbLabel cmbLabel20;
        internal DarkNumericUpDown nudIntelligence;
        internal cmbLabel cmbLabel21;
        internal DarkNumericUpDown nudSpirit;
        internal cmbLabel cmbLabel22;
        internal DarkNumericUpDown nudPaperdoll;
        internal cmbLabel cmbLabel23;
        internal PictureBox picPaperdoll;
        internal DarkButton btnBasics;
        internal DarkButton btnRequirements;
        internal DarkGroupBox fraRequirements;
        internal DarkComboBox cmbJobReq;
        internal cmbLabel cmbLabel26;
        internal DarkComboBox cmbAccessReq;
        internal cmbLabel cmbLabel27;
        internal cmbLabel cmbLabel28;
        internal DarkNumericUpDown nudLevelReq;
        internal DarkGroupBox DarkGroupBox4;
        internal DarkNumericUpDown nudSprReq;
        internal cmbLabel cmbLabel32;
        internal DarkNumericUpDown nudIntReq;
        internal cmbLabel cmbLabel33;
        internal DarkNumericUpDown nudVitReq;
        internal cmbLabel cmbLabel34;
        internal DarkNumericUpDown nudLuckReq;
        internal cmbLabel cmbLabel29;
        internal DarkNumericUpDown nudStrReq;
        internal cmbLabel cmbLabel31;
        internal cmbLabel cmbLabel35;
        internal DarkButton btnSave;
        internal DarkButton btnDelete;
        internal DarkButton btnCancel;
        internal DarkButton btnSpawn;
        internal DarkNumericUpDown nudSpanwAmount;
        internal DarkGroupBox fraProjectile;
        internal DarkComboBox cmbAmmo;
        internal cmbLabel cmbLabel25;
        internal DarkComboBox cmbProjectile;
        internal cmbLabel cmbLabel24;
        internal DarkGroupBox fraSkill;
        internal DarkComboBox cmbSkills;
        internal cmbLabel cmbLabel12;
        internal DarkGroupBox fraVitals;
        internal DarkNumericUpDown nudVitalMod;
        internal cmbLabel cmbLabel11;
        internal DarkTextBox txtName;
        internal cmbLabel cmbLabel2;
        internal DarkNumericUpDown nudIcon;
        internal cmbLabel cmbLabel3;
        internal DarkNumericUpDown nudRarity;
        internal PictureBox picItem;
        internal cmbLabel cmbLabel4;
        internal DarkComboBox cmbType;
        internal cmbLabel cmbLabel5;
        internal DarkComboBox cmbSubType;
        internal DarkCheckBox chkStackable;
        internal cmbLabel cmbLabel6;
        internal DarkComboBox cmbBind;
        internal cmbLabel cmbLabel7;
        internal DarkNumericUpDown nudPrice;
        internal cmbLabel cmbLabel8;
        internal DarkNumericUpDown nudItemLvl;
        internal cmbLabel cmbLabel9;
        internal DarkComboBox cmbAnimation;
        internal cmbLabel cmbLabel10;
        internal DarkTextBox txtDescription;
        internal cmbLabel cmbLabel13;
        internal DarkGroupBox fraEvents;
        internal DarkNumericUpDown nudEventValue;
        internal cmbLabel cmbLabel39;
        internal DarkNumericUpDown nudEvent;
        internal cmbLabel cmbLabel38;
        internal cmbLabel cmbLabel36;
        internal DarkGroupBox fraBasics;
    }
}