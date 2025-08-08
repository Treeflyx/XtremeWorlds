using DarkUI.Controls;
using DarkUI.Forms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public partial class Editor_Npc : DarkForm
    {
        // Shared instance of the form
        private static Editor_Npc _instance;

        // Public property to get the shared instance
        public static Editor_Npc Instance
        {
            get
            {
                // Create a new instance if one does not exist or if it has been disposed
                if (_instance is null || _instance.IsDisposed)
                {
                    _instance = new Editor_Npc();
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
            DarkGroupBox1 = new DarkGroupBox();
            lstIndex = new ListBox();
            DarkGroupBox2 = new DarkGroupBox();
            cmbSpawnPeriod = new DarkComboBox();
            DarkLabel30 = new DarkLabel();
            nudSpawnSecs = new DarkNumericUpDown();
            DarkLabel13 = new DarkLabel();
            nudDamage = new DarkNumericUpDown();
            DarkLabel12 = new DarkLabel();
            nudLevel = new DarkNumericUpDown();
            DarkLabel11 = new DarkLabel();
            nudExp = new DarkNumericUpDown();
            DarkLabel10 = new DarkLabel();
            nudHp = new DarkNumericUpDown();
            DarkLabel9 = new DarkLabel();
            cmbFaction = new DarkComboBox();
            DarkLabel8 = new DarkLabel();
            cmbBehaviour = new DarkComboBox();
            DarkLabel5 = new DarkLabel();
            cmbAnimation = new DarkComboBox();
            DarkLabel7 = new DarkLabel();
            DarkLabel4 = new DarkLabel();
            nudRange = new DarkNumericUpDown();
            nudSprite = new DarkNumericUpDown();
            DarkLabel3 = new DarkLabel();
            txtAttackSay = new DarkTextBox();
            DarkLabel2 = new DarkLabel();
            picSprite = new PictureBox();
            txtName = new DarkTextBox();
            DarkLabel1 = new DarkLabel();
            DarkGroupBox3 = new DarkGroupBox();
            cmbSkill6 = new DarkComboBox();
            DarkLabel17 = new DarkLabel();
            cmbSkill5 = new DarkComboBox();
            DarkLabel18 = new DarkLabel();
            cmbSkill4 = new DarkComboBox();
            DarkLabel19 = new DarkLabel();
            cmbSkill3 = new DarkComboBox();
            DarkLabel16 = new DarkLabel();
            cmbSkill2 = new DarkComboBox();
            DarkLabel15 = new DarkLabel();
            cmbSkill1 = new DarkComboBox();
            DarkLabel14 = new DarkLabel();
            DarkGroupBox4 = new DarkGroupBox();
            nudAmount = new DarkNumericUpDown();
            DarkLabel29 = new DarkLabel();
            cmbItem = new DarkComboBox();
            DarkLabel28 = new DarkLabel();
            cmbDropSlot = new DarkComboBox();
            nudChance = new DarkNumericUpDown();
            DarkLabel27 = new DarkLabel();
            DarkLabel26 = new DarkLabel();
            DarkGroupBox5 = new DarkGroupBox();
            nudSpirit = new DarkNumericUpDown();
            DarkLabel23 = new DarkLabel();
            nudIntelligence = new DarkNumericUpDown();
            DarkLabel24 = new DarkLabel();
            nudLuck = new DarkNumericUpDown();
            DarkLabel25 = new DarkLabel();
            nudVitality = new DarkNumericUpDown();
            DarkLabel22 = new DarkLabel();
            nudStrength = new DarkNumericUpDown();
            DarkLabel20 = new DarkLabel();
            btnCancel = new DarkButton();
            btnDelete = new DarkButton();
            btnSave = new DarkButton();
            DarkGroupBox1.SuspendLayout();
            DarkGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpawnSecs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDamage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudExp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudHp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRange).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSprite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSprite).BeginInit();
            DarkGroupBox3.SuspendLayout();
            DarkGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudAmount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudChance).BeginInit();
            DarkGroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpirit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudIntelligence).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLuck).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVitality).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStrength).BeginInit();
            SuspendLayout();
            // 
            // DarkGroupBox1
            // 
            DarkGroupBox1.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox1.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox1.Controls.Add(lstIndex);
            DarkGroupBox1.ForeColor = Color.Gainsboro;
            DarkGroupBox1.Location = new Point(6, 3);
            DarkGroupBox1.Margin = new Padding(6, 5, 6, 5);
            DarkGroupBox1.Name = "DarkGroupBox1";
            DarkGroupBox1.Padding = new Padding(6, 5, 6, 5);
            DarkGroupBox1.Size = new Size(379, 752);
            DarkGroupBox1.TabIndex = 0;
            DarkGroupBox1.TabStop = false;
            DarkGroupBox1.Text = "Npc List";
            // 
            // lstIndex
            // 
            lstIndex.BackColor = Color.FromArgb(45, 45, 48);
            lstIndex.BorderStyle = BorderStyle.FixedSingle;
            lstIndex.ForeColor = Color.Gainsboro;
            lstIndex.FormattingEnabled = true;
            lstIndex.Location = new Point(7, 30);
            lstIndex.Margin = new Padding(6, 5, 6, 5);
            lstIndex.Name = "lstIndex";
            lstIndex.Size = new Size(363, 702);
            lstIndex.TabIndex = 2;
            lstIndex.Click += lstIndex_Click;
            // 
            // DarkGroupBox2
            // 
            DarkGroupBox2.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox2.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox2.Controls.Add(cmbSpawnPeriod);
            DarkGroupBox2.Controls.Add(DarkLabel30);
            DarkGroupBox2.Controls.Add(nudSpawnSecs);
            DarkGroupBox2.Controls.Add(DarkLabel13);
            DarkGroupBox2.Controls.Add(nudDamage);
            DarkGroupBox2.Controls.Add(DarkLabel12);
            DarkGroupBox2.Controls.Add(nudLevel);
            DarkGroupBox2.Controls.Add(DarkLabel11);
            DarkGroupBox2.Controls.Add(nudExp);
            DarkGroupBox2.Controls.Add(DarkLabel10);
            DarkGroupBox2.Controls.Add(nudHp);
            DarkGroupBox2.Controls.Add(DarkLabel9);
            DarkGroupBox2.Controls.Add(cmbFaction);
            DarkGroupBox2.Controls.Add(DarkLabel8);
            DarkGroupBox2.Controls.Add(cmbBehaviour);
            DarkGroupBox2.Controls.Add(DarkLabel5);
            DarkGroupBox2.Controls.Add(cmbAnimation);
            DarkGroupBox2.Controls.Add(DarkLabel7);
            DarkGroupBox2.Controls.Add(DarkLabel4);
            DarkGroupBox2.Controls.Add(nudRange);
            DarkGroupBox2.Controls.Add(nudSprite);
            DarkGroupBox2.Controls.Add(DarkLabel3);
            DarkGroupBox2.Controls.Add(txtAttackSay);
            DarkGroupBox2.Controls.Add(DarkLabel2);
            DarkGroupBox2.Controls.Add(picSprite);
            DarkGroupBox2.Controls.Add(txtName);
            DarkGroupBox2.Controls.Add(DarkLabel1);
            DarkGroupBox2.ForeColor = Color.Gainsboro;
            DarkGroupBox2.Location = new Point(393, 3);
            DarkGroupBox2.Margin = new Padding(6, 5, 6, 5);
            DarkGroupBox2.Name = "DarkGroupBox2";
            DarkGroupBox2.Padding = new Padding(6, 5, 6, 5);
            DarkGroupBox2.Size = new Size(657, 445);
            DarkGroupBox2.TabIndex = 1;
            DarkGroupBox2.TabStop = false;
            DarkGroupBox2.Text = "Properties";
            // 
            // cmbSpawnPeriod
            // 
            cmbSpawnPeriod.DrawMode = DrawMode.OwnerDrawVariable;
            cmbSpawnPeriod.FormattingEnabled = true;
            cmbSpawnPeriod.Items.AddRange(new object[] { "Always", "Day", "Night", "Dawn", "Dusk" });
            cmbSpawnPeriod.Location = new Point(474, 388);
            cmbSpawnPeriod.Margin = new Padding(6, 5, 6, 5);
            cmbSpawnPeriod.Name = "cmbSpawnPeriod";
            cmbSpawnPeriod.Size = new Size(165, 32);
            cmbSpawnPeriod.TabIndex = 38;
            cmbSpawnPeriod.SelectedIndexChanged += CmbSpawnPeriod_SelectedIndexChanged;
            // 
            // DarkLabel30
            // 
            DarkLabel30.AutoSize = true;
            DarkLabel30.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel30.Location = new Point(391, 395);
            DarkLabel30.Margin = new Padding(6, 0, 6, 0);
            DarkLabel30.Name = "DarkLabel30";
            DarkLabel30.Size = new Size(77, 25);
            DarkLabel30.TabIndex = 37;
            DarkLabel30.Text = "Spawns:";
            // 
            // nudSpawnSecs
            // 
            nudSpawnSecs.Location = new Point(249, 392);
            nudSpawnSecs.Margin = new Padding(6, 5, 6, 5);
            nudSpawnSecs.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudSpawnSecs.Name = "nudSpawnSecs";
            nudSpawnSecs.Size = new Size(139, 31);
            nudSpawnSecs.TabIndex = 36;
            nudSpawnSecs.ValueChanged += NudSpawnSecs_ValueChanged;
            // 
            // DarkLabel13
            // 
            DarkLabel13.AutoSize = true;
            DarkLabel13.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel13.Location = new Point(10, 395);
            DarkLabel13.Margin = new Padding(6, 0, 6, 0);
            DarkLabel13.Name = "DarkLabel13";
            DarkLabel13.Size = new Size(220, 25);
            DarkLabel13.TabIndex = 35;
            DarkLabel13.Text = "Respawn Time in Seconds:";
            // 
            // nudDamage
            // 
            nudDamage.Location = new Point(443, 342);
            nudDamage.Margin = new Padding(6, 5, 6, 5);
            nudDamage.Name = "nudDamage";
            nudDamage.Size = new Size(200, 31);
            nudDamage.TabIndex = 34;
            nudDamage.ValueChanged += NudDamage_ValueChanged;
            // 
            // DarkLabel12
            // 
            DarkLabel12.AutoSize = true;
            DarkLabel12.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel12.Location = new Point(310, 345);
            DarkLabel12.Margin = new Padding(6, 0, 6, 0);
            DarkLabel12.Name = "DarkLabel12";
            DarkLabel12.Size = new Size(124, 25);
            DarkLabel12.TabIndex = 33;
            DarkLabel12.Text = "Base Damage:";
            // 
            // nudLevel
            // 
            nudLevel.Location = new Point(100, 342);
            nudLevel.Margin = new Padding(6, 5, 6, 5);
            nudLevel.Name = "nudLevel";
            nudLevel.Size = new Size(200, 31);
            nudLevel.TabIndex = 32;
            nudLevel.ValueChanged += NudLevel_ValueChanged;
            // 
            // DarkLabel11
            // 
            DarkLabel11.AutoSize = true;
            DarkLabel11.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel11.Location = new Point(10, 345);
            DarkLabel11.Margin = new Padding(6, 0, 6, 0);
            DarkLabel11.Name = "DarkLabel11";
            DarkLabel11.Size = new Size(55, 25);
            DarkLabel11.TabIndex = 31;
            DarkLabel11.Text = "Level:";
            // 
            // nudExp
            // 
            nudExp.Location = new Point(397, 292);
            nudExp.Margin = new Padding(6, 5, 6, 5);
            nudExp.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudExp.Name = "nudExp";
            nudExp.Size = new Size(247, 31);
            nudExp.TabIndex = 30;
            nudExp.ValueChanged += NudExp_ValueChanged;
            // 
            // DarkLabel10
            // 
            DarkLabel10.AutoSize = true;
            DarkLabel10.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel10.Location = new Point(289, 295);
            DarkLabel10.Margin = new Padding(6, 0, 6, 0);
            DarkLabel10.Name = "DarkLabel10";
            DarkLabel10.Size = new Size(93, 25);
            DarkLabel10.TabIndex = 29;
            DarkLabel10.Text = "Exp Given:";
            // 
            // nudHp
            // 
            nudHp.Location = new Point(100, 292);
            nudHp.Margin = new Padding(6, 5, 6, 5);
            nudHp.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudHp.Name = "nudHp";
            nudHp.Size = new Size(179, 31);
            nudHp.TabIndex = 28;
            nudHp.ValueChanged += NudHp_ValueChanged;
            // 
            // DarkLabel9
            // 
            DarkLabel9.AutoSize = true;
            DarkLabel9.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel9.Location = new Point(10, 295);
            DarkLabel9.Margin = new Padding(6, 0, 6, 0);
            DarkLabel9.Name = "DarkLabel9";
            DarkLabel9.Size = new Size(67, 25);
            DarkLabel9.TabIndex = 27;
            DarkLabel9.Text = "Health:";
            // 
            // cmbFaction
            // 
            cmbFaction.DrawMode = DrawMode.OwnerDrawFixed;
            cmbFaction.FormattingEnabled = true;
            cmbFaction.Items.AddRange(new object[] { "None", "Good", "Bad" });
            cmbFaction.Location = new Point(431, 238);
            cmbFaction.Margin = new Padding(6, 5, 6, 5);
            cmbFaction.Name = "cmbFaction";
            cmbFaction.Size = new Size(210, 32);
            cmbFaction.TabIndex = 26;
            cmbFaction.SelectedIndexChanged += CmbFaction_SelectedIndexChanged;
            // 
            // DarkLabel8
            // 
            DarkLabel8.AutoSize = true;
            DarkLabel8.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel8.Location = new Point(341, 245);
            DarkLabel8.Margin = new Padding(6, 0, 6, 0);
            DarkLabel8.Name = "DarkLabel8";
            DarkLabel8.Size = new Size(72, 25);
            DarkLabel8.TabIndex = 25;
            DarkLabel8.Text = "Faction:";
            // 
            // cmbBehaviour
            // 
            cmbBehaviour.DrawMode = DrawMode.OwnerDrawFixed;
            cmbBehaviour.FormattingEnabled = true;
            cmbBehaviour.Items.AddRange(new object[] { "Attack on sight", "Attack when attacked", "Friendly", "Shop keeper", "Guard", "Quest" });
            cmbBehaviour.Location = new Point(100, 238);
            cmbBehaviour.Margin = new Padding(6, 5, 6, 5);
            cmbBehaviour.Name = "cmbBehaviour";
            cmbBehaviour.Size = new Size(230, 32);
            cmbBehaviour.TabIndex = 24;
            cmbBehaviour.SelectedIndexChanged += CmbBehavior_SelectedIndexChanged;
            // 
            // DarkLabel5
            // 
            DarkLabel5.AutoSize = true;
            DarkLabel5.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel5.Location = new Point(10, 245);
            DarkLabel5.Margin = new Padding(6, 0, 6, 0);
            DarkLabel5.Name = "DarkLabel5";
            DarkLabel5.Size = new Size(84, 25);
            DarkLabel5.TabIndex = 23;
            DarkLabel5.Text = "Behavior:";
            // 
            // cmbAnimation
            // 
            cmbAnimation.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAnimation.FormattingEnabled = true;
            cmbAnimation.Location = new Point(123, 189);
            cmbAnimation.Margin = new Padding(6, 5, 6, 5);
            cmbAnimation.Name = "cmbAnimation";
            cmbAnimation.Size = new Size(177, 32);
            cmbAnimation.TabIndex = 21;
            cmbAnimation.SelectedIndexChanged += CmbAnimation_SelectedIndexChanged;
            // 
            // DarkLabel7
            // 
            DarkLabel7.AutoSize = true;
            DarkLabel7.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel7.Location = new Point(10, 192);
            DarkLabel7.Margin = new Padding(6, 0, 6, 0);
            DarkLabel7.Name = "DarkLabel7";
            DarkLabel7.Size = new Size(98, 25);
            DarkLabel7.TabIndex = 20;
            DarkLabel7.Text = "Animation:";
            // 
            // DarkLabel4
            // 
            DarkLabel4.AutoSize = true;
            DarkLabel4.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel4.Location = new Point(10, 142);
            DarkLabel4.Margin = new Padding(6, 0, 6, 0);
            DarkLabel4.Name = "DarkLabel4";
            DarkLabel4.Size = new Size(66, 25);
            DarkLabel4.TabIndex = 15;
            DarkLabel4.Text = "Range:";
            // 
            // nudRange
            // 
            nudRange.Location = new Point(100, 137);
            nudRange.Margin = new Padding(6, 5, 6, 5);
            nudRange.Name = "nudRange";
            nudRange.Size = new Size(180, 31);
            nudRange.TabIndex = 14;
            nudRange.ValueChanged += NudRange_ValueChanged;
            // 
            // nudSprite
            // 
            nudSprite.Location = new Point(361, 137);
            nudSprite.Margin = new Padding(6, 5, 6, 5);
            nudSprite.Name = "nudSprite";
            nudSprite.Size = new Size(160, 31);
            nudSprite.TabIndex = 13;
            nudSprite.Click += NudSprite_Click;
            // 
            // DarkLabel3
            // 
            DarkLabel3.AutoSize = true;
            DarkLabel3.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel3.Location = new Point(290, 142);
            DarkLabel3.Margin = new Padding(6, 0, 6, 0);
            DarkLabel3.Name = "DarkLabel3";
            DarkLabel3.Size = new Size(62, 25);
            DarkLabel3.TabIndex = 12;
            DarkLabel3.Text = "Sprite:";
            // 
            // txtAttackSay
            // 
            txtAttackSay.BackColor = Color.FromArgb(69, 73, 74);
            txtAttackSay.BorderStyle = BorderStyle.FixedSingle;
            txtAttackSay.ForeColor = Color.FromArgb(220, 220, 220);
            txtAttackSay.Location = new Point(100, 87);
            txtAttackSay.Margin = new Padding(6, 5, 6, 5);
            txtAttackSay.Name = "txtAttackSay";
            txtAttackSay.Size = new Size(421, 31);
            txtAttackSay.TabIndex = 11;
            txtAttackSay.TextChanged += TxtAttackSay_TextChanged;
            // 
            // DarkLabel2
            // 
            DarkLabel2.AutoSize = true;
            DarkLabel2.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel2.Location = new Point(10, 92);
            DarkLabel2.Margin = new Padding(6, 0, 6, 0);
            DarkLabel2.Name = "DarkLabel2";
            DarkLabel2.Size = new Size(44, 25);
            DarkLabel2.TabIndex = 10;
            DarkLabel2.Text = "Say:";
            // 
            // picSprite
            // 
            picSprite.BackColor = Color.Black;
            picSprite.BackgroundImageLayout = ImageLayout.None;
            picSprite.Location = new Point(533, 37);
            picSprite.Margin = new Padding(6, 5, 6, 5);
            picSprite.Name = "picSprite";
            picSprite.Size = new Size(46, 53);
            picSprite.TabIndex = 9;
            picSprite.TabStop = false;
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.FromArgb(220, 220, 220);
            txtName.Location = new Point(100, 37);
            txtName.Margin = new Padding(6, 5, 6, 5);
            txtName.Name = "txtName";
            txtName.Size = new Size(421, 31);
            txtName.TabIndex = 1;
            txtName.TextChanged += TxtName_TextChanged;
            // 
            // DarkLabel1
            // 
            DarkLabel1.AutoSize = true;
            DarkLabel1.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel1.Location = new Point(10, 42);
            DarkLabel1.Margin = new Padding(6, 0, 6, 0);
            DarkLabel1.Name = "DarkLabel1";
            DarkLabel1.Size = new Size(63, 25);
            DarkLabel1.TabIndex = 0;
            DarkLabel1.Text = "Name:";
            // 
            // DarkGroupBox3
            // 
            DarkGroupBox3.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox3.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox3.Controls.Add(cmbSkill6);
            DarkGroupBox3.Controls.Add(DarkLabel17);
            DarkGroupBox3.Controls.Add(cmbSkill5);
            DarkGroupBox3.Controls.Add(DarkLabel18);
            DarkGroupBox3.Controls.Add(cmbSkill4);
            DarkGroupBox3.Controls.Add(DarkLabel19);
            DarkGroupBox3.Controls.Add(cmbSkill3);
            DarkGroupBox3.Controls.Add(DarkLabel16);
            DarkGroupBox3.Controls.Add(cmbSkill2);
            DarkGroupBox3.Controls.Add(DarkLabel15);
            DarkGroupBox3.Controls.Add(cmbSkill1);
            DarkGroupBox3.Controls.Add(DarkLabel14);
            DarkGroupBox3.ForeColor = Color.Gainsboro;
            DarkGroupBox3.Location = new Point(393, 458);
            DarkGroupBox3.Margin = new Padding(6, 5, 6, 5);
            DarkGroupBox3.Name = "DarkGroupBox3";
            DarkGroupBox3.Padding = new Padding(6, 5, 6, 5);
            DarkGroupBox3.Size = new Size(657, 137);
            DarkGroupBox3.TabIndex = 2;
            DarkGroupBox3.TabStop = false;
            DarkGroupBox3.Text = "Skills";
            // 
            // cmbSkill6
            // 
            cmbSkill6.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkill6.FormattingEnabled = true;
            cmbSkill6.Location = new Point(474, 77);
            cmbSkill6.Margin = new Padding(6, 5, 6, 5);
            cmbSkill6.Name = "cmbSkill6";
            cmbSkill6.Size = new Size(173, 32);
            cmbSkill6.TabIndex = 11;
            cmbSkill6.SelectedIndexChanged += CmbSkill6_SelectedIndexChanged;
            // 
            // DarkLabel17
            // 
            DarkLabel17.AutoSize = true;
            DarkLabel17.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel17.Location = new Point(443, 83);
            DarkLabel17.Margin = new Padding(6, 0, 6, 0);
            DarkLabel17.Name = "DarkLabel17";
            DarkLabel17.Size = new Size(22, 25);
            DarkLabel17.TabIndex = 10;
            DarkLabel17.Text = "6";
            // 
            // cmbSkill5
            // 
            cmbSkill5.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkill5.FormattingEnabled = true;
            cmbSkill5.Location = new Point(259, 77);
            cmbSkill5.Margin = new Padding(6, 5, 6, 5);
            cmbSkill5.Name = "cmbSkill5";
            cmbSkill5.Size = new Size(173, 32);
            cmbSkill5.TabIndex = 9;
            cmbSkill5.SelectedIndexChanged += CmbSkill5_SelectedIndexChanged;
            // 
            // DarkLabel18
            // 
            DarkLabel18.AutoSize = true;
            DarkLabel18.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel18.Location = new Point(227, 83);
            DarkLabel18.Margin = new Padding(6, 0, 6, 0);
            DarkLabel18.Name = "DarkLabel18";
            DarkLabel18.Size = new Size(22, 25);
            DarkLabel18.TabIndex = 8;
            DarkLabel18.Text = "5";
            // 
            // cmbSkill4
            // 
            cmbSkill4.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkill4.FormattingEnabled = true;
            cmbSkill4.Location = new Point(41, 77);
            cmbSkill4.Margin = new Padding(6, 5, 6, 5);
            cmbSkill4.Name = "cmbSkill4";
            cmbSkill4.Size = new Size(173, 32);
            cmbSkill4.TabIndex = 7;
            cmbSkill4.SelectedIndexChanged += CmbSkill4_SelectedIndexChanged;
            // 
            // DarkLabel19
            // 
            DarkLabel19.AutoSize = true;
            DarkLabel19.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel19.Location = new Point(10, 83);
            DarkLabel19.Margin = new Padding(6, 0, 6, 0);
            DarkLabel19.Name = "DarkLabel19";
            DarkLabel19.Size = new Size(22, 25);
            DarkLabel19.TabIndex = 6;
            DarkLabel19.Text = "4";
            // 
            // cmbSkill3
            // 
            cmbSkill3.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkill3.FormattingEnabled = true;
            cmbSkill3.Location = new Point(474, 25);
            cmbSkill3.Margin = new Padding(6, 5, 6, 5);
            cmbSkill3.Name = "cmbSkill3";
            cmbSkill3.Size = new Size(173, 32);
            cmbSkill3.TabIndex = 5;
            cmbSkill3.SelectedIndexChanged += CmbSkill3_SelectedIndexChanged;
            // 
            // DarkLabel16
            // 
            DarkLabel16.AutoSize = true;
            DarkLabel16.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel16.Location = new Point(443, 30);
            DarkLabel16.Margin = new Padding(6, 0, 6, 0);
            DarkLabel16.Name = "DarkLabel16";
            DarkLabel16.Size = new Size(22, 25);
            DarkLabel16.TabIndex = 4;
            DarkLabel16.Text = "3";
            // 
            // cmbSkill2
            // 
            cmbSkill2.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkill2.FormattingEnabled = true;
            cmbSkill2.Location = new Point(259, 25);
            cmbSkill2.Margin = new Padding(6, 5, 6, 5);
            cmbSkill2.Name = "cmbSkill2";
            cmbSkill2.Size = new Size(173, 32);
            cmbSkill2.TabIndex = 3;
            cmbSkill2.SelectedIndexChanged += CmbSkill2_SelectedIndexChanged;
            // 
            // DarkLabel15
            // 
            DarkLabel15.AutoSize = true;
            DarkLabel15.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel15.Location = new Point(227, 30);
            DarkLabel15.Margin = new Padding(6, 0, 6, 0);
            DarkLabel15.Name = "DarkLabel15";
            DarkLabel15.Size = new Size(22, 25);
            DarkLabel15.TabIndex = 2;
            DarkLabel15.Text = "2";
            // 
            // cmbSkill1
            // 
            cmbSkill1.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkill1.FormattingEnabled = true;
            cmbSkill1.Location = new Point(41, 25);
            cmbSkill1.Margin = new Padding(6, 5, 6, 5);
            cmbSkill1.Name = "cmbSkill1";
            cmbSkill1.Size = new Size(173, 32);
            cmbSkill1.TabIndex = 1;
            cmbSkill1.SelectedIndexChanged += CmbSkill1_SelectedIndexChanged;
            // 
            // DarkLabel14
            // 
            DarkLabel14.AutoSize = true;
            DarkLabel14.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel14.Location = new Point(10, 30);
            DarkLabel14.Margin = new Padding(6, 0, 6, 0);
            DarkLabel14.Name = "DarkLabel14";
            DarkLabel14.Size = new Size(22, 25);
            DarkLabel14.TabIndex = 0;
            DarkLabel14.Text = "1";
            // 
            // DarkGroupBox4
            // 
            DarkGroupBox4.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox4.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox4.Controls.Add(nudAmount);
            DarkGroupBox4.Controls.Add(DarkLabel29);
            DarkGroupBox4.Controls.Add(cmbItem);
            DarkGroupBox4.Controls.Add(DarkLabel28);
            DarkGroupBox4.Controls.Add(cmbDropSlot);
            DarkGroupBox4.Controls.Add(nudChance);
            DarkGroupBox4.Controls.Add(DarkLabel27);
            DarkGroupBox4.Controls.Add(DarkLabel26);
            DarkGroupBox4.ForeColor = Color.Gainsboro;
            DarkGroupBox4.Location = new Point(393, 770);
            DarkGroupBox4.Margin = new Padding(6, 5, 6, 5);
            DarkGroupBox4.Name = "DarkGroupBox4";
            DarkGroupBox4.Padding = new Padding(6, 5, 6, 5);
            DarkGroupBox4.Size = new Size(657, 148);
            DarkGroupBox4.TabIndex = 3;
            DarkGroupBox4.TabStop = false;
            DarkGroupBox4.Text = "Drop Items";
            // 
            // nudAmount
            // 
            nudAmount.Location = new Point(447, 83);
            nudAmount.Margin = new Padding(6, 5, 6, 5);
            nudAmount.Name = "nudAmount";
            nudAmount.Size = new Size(200, 31);
            nudAmount.TabIndex = 7;
            nudAmount.ValueChanged += ScrlValue_Scroll;
            // 
            // DarkLabel29
            // 
            DarkLabel29.AutoSize = true;
            DarkLabel29.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel29.Location = new Point(341, 88);
            DarkLabel29.Margin = new Padding(6, 0, 6, 0);
            DarkLabel29.Name = "DarkLabel29";
            DarkLabel29.Size = new Size(81, 25);
            DarkLabel29.TabIndex = 6;
            DarkLabel29.Text = "Amount:";
            // 
            // cmbItem
            // 
            cmbItem.DrawMode = DrawMode.OwnerDrawFixed;
            cmbItem.FormattingEnabled = true;
            cmbItem.Location = new Point(110, 83);
            cmbItem.Margin = new Padding(6, 5, 6, 5);
            cmbItem.Name = "cmbItem";
            cmbItem.Size = new Size(198, 32);
            cmbItem.TabIndex = 5;
            cmbItem.SelectedIndexChanged += CmbItem_SelectedIndexChanged;
            // 
            // DarkLabel28
            // 
            DarkLabel28.AutoSize = true;
            DarkLabel28.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel28.Location = new Point(10, 88);
            DarkLabel28.Margin = new Padding(6, 0, 6, 0);
            DarkLabel28.Name = "DarkLabel28";
            DarkLabel28.Size = new Size(52, 25);
            DarkLabel28.TabIndex = 4;
            DarkLabel28.Text = "Item:";
            // 
            // cmbDropSlot
            // 
            cmbDropSlot.DrawMode = DrawMode.OwnerDrawFixed;
            cmbDropSlot.FormattingEnabled = true;
            cmbDropSlot.Items.AddRange(new object[] { "Slot 1", "Slot 2", "Slot 3", "Slot 4", "Slot 5" });
            cmbDropSlot.Location = new Point(110, 25);
            cmbDropSlot.Margin = new Padding(6, 5, 6, 5);
            cmbDropSlot.Name = "cmbDropSlot";
            cmbDropSlot.Size = new Size(198, 32);
            cmbDropSlot.TabIndex = 3;
            cmbDropSlot.SelectedIndexChanged += CmbDropSlot_SelectedIndexChanged;
            // 
            // nudChance
            // 
            nudChance.Location = new Point(490, 27);
            nudChance.Margin = new Padding(6, 5, 6, 5);
            nudChance.Name = "nudChance";
            nudChance.Size = new Size(157, 31);
            nudChance.TabIndex = 2;
            nudChance.ValueChanged += NudChance_ValueChanged;
            // 
            // DarkLabel27
            // 
            DarkLabel27.AutoSize = true;
            DarkLabel27.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel27.Location = new Point(341, 30);
            DarkLabel27.Margin = new Padding(6, 0, 6, 0);
            DarkLabel27.Name = "DarkLabel27";
            DarkLabel27.Size = new Size(138, 25);
            DarkLabel27.TabIndex = 1;
            DarkLabel27.Text = "Chance 1 out of";
            // 
            // DarkLabel26
            // 
            DarkLabel26.AutoSize = true;
            DarkLabel26.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel26.Location = new Point(10, 30);
            DarkLabel26.Margin = new Padding(6, 0, 6, 0);
            DarkLabel26.Name = "DarkLabel26";
            DarkLabel26.Size = new Size(93, 25);
            DarkLabel26.TabIndex = 0;
            DarkLabel26.Text = "Drop Slot:";
            // 
            // DarkGroupBox5
            // 
            DarkGroupBox5.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox5.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox5.Controls.Add(nudSpirit);
            DarkGroupBox5.Controls.Add(DarkLabel23);
            DarkGroupBox5.Controls.Add(nudIntelligence);
            DarkGroupBox5.Controls.Add(DarkLabel24);
            DarkGroupBox5.Controls.Add(nudLuck);
            DarkGroupBox5.Controls.Add(DarkLabel25);
            DarkGroupBox5.Controls.Add(nudVitality);
            DarkGroupBox5.Controls.Add(DarkLabel22);
            DarkGroupBox5.Controls.Add(nudStrength);
            DarkGroupBox5.Controls.Add(DarkLabel20);
            DarkGroupBox5.ForeColor = Color.Gainsboro;
            DarkGroupBox5.Location = new Point(393, 608);
            DarkGroupBox5.Margin = new Padding(6, 5, 6, 5);
            DarkGroupBox5.Name = "DarkGroupBox5";
            DarkGroupBox5.Padding = new Padding(6, 5, 6, 5);
            DarkGroupBox5.Size = new Size(657, 150);
            DarkGroupBox5.TabIndex = 4;
            DarkGroupBox5.TabStop = false;
            DarkGroupBox5.Text = "Stats";
            // 
            // nudSpirit
            // 
            nudSpirit.Location = new Point(537, 42);
            nudSpirit.Margin = new Padding(6, 5, 6, 5);
            nudSpirit.Name = "nudSpirit";
            nudSpirit.Size = new Size(106, 31);
            nudSpirit.TabIndex = 11;
            nudSpirit.ValueChanged += NudSpirit_ValueChanged;
            // 
            // DarkLabel23
            // 
            DarkLabel23.AutoSize = true;
            DarkLabel23.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel23.Location = new Point(447, 45);
            DarkLabel23.Margin = new Padding(6, 0, 6, 0);
            DarkLabel23.Name = "DarkLabel23";
            DarkLabel23.Size = new Size(57, 25);
            DarkLabel23.TabIndex = 10;
            DarkLabel23.Text = "Spirit:";
            // 
            // nudIntelligence
            // 
            nudIntelligence.Location = new Point(329, 87);
            nudIntelligence.Margin = new Padding(6, 5, 6, 5);
            nudIntelligence.Name = "nudIntelligence";
            nudIntelligence.Size = new Size(106, 31);
            nudIntelligence.TabIndex = 9;
            nudIntelligence.ValueChanged += NudIntelligence_ValueChanged;
            // 
            // DarkLabel24
            // 
            DarkLabel24.AutoSize = true;
            DarkLabel24.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel24.Location = new Point(214, 92);
            DarkLabel24.Margin = new Padding(6, 0, 6, 0);
            DarkLabel24.Name = "DarkLabel24";
            DarkLabel24.Size = new Size(105, 25);
            DarkLabel24.TabIndex = 8;
            DarkLabel24.Text = "Intelligence:";
            // 
            // nudLuck
            // 
            nudLuck.Location = new Point(100, 87);
            nudLuck.Margin = new Padding(6, 5, 6, 5);
            nudLuck.Name = "nudLuck";
            nudLuck.Size = new Size(106, 31);
            nudLuck.TabIndex = 7;
            nudLuck.ValueChanged += NudLuck_ValueChanged;
            // 
            // DarkLabel25
            // 
            DarkLabel25.AutoSize = true;
            DarkLabel25.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel25.Location = new Point(10, 92);
            DarkLabel25.Margin = new Padding(6, 0, 6, 0);
            DarkLabel25.Name = "DarkLabel25";
            DarkLabel25.Size = new Size(51, 25);
            DarkLabel25.TabIndex = 6;
            DarkLabel25.Text = "Luck:";
            // 
            // nudVitality
            // 
            nudVitality.Location = new Point(329, 38);
            nudVitality.Margin = new Padding(6, 5, 6, 5);
            nudVitality.Name = "nudVitality";
            nudVitality.Size = new Size(106, 31);
            nudVitality.TabIndex = 5;
            nudVitality.ValueChanged += NudVitality_ValueChanged;
            // 
            // DarkLabel22
            // 
            DarkLabel22.AutoSize = true;
            DarkLabel22.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel22.Location = new Point(239, 43);
            DarkLabel22.Margin = new Padding(6, 0, 6, 0);
            DarkLabel22.Name = "DarkLabel22";
            DarkLabel22.Size = new Size(69, 25);
            DarkLabel22.TabIndex = 4;
            DarkLabel22.Text = "Vitality:";
            // 
            // nudStrength
            // 
            nudStrength.Location = new Point(100, 37);
            nudStrength.Margin = new Padding(6, 5, 6, 5);
            nudStrength.Name = "nudStrength";
            nudStrength.Size = new Size(106, 31);
            nudStrength.TabIndex = 1;
            nudStrength.ValueChanged += NudStrength_ValueChanged;
            // 
            // DarkLabel20
            // 
            DarkLabel20.AutoSize = true;
            DarkLabel20.ForeColor = Color.FromArgb(220, 220, 220);
            DarkLabel20.Location = new Point(10, 42);
            DarkLabel20.Margin = new Padding(6, 0, 6, 0);
            DarkLabel20.Name = "DarkLabel20";
            DarkLabel20.Size = new Size(83, 25);
            DarkLabel20.TabIndex = 0;
            DarkLabel20.Text = "Strength:";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(17, 873);
            btnCancel.Margin = new Padding(6, 5, 6, 5);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(9, 8, 9, 8);
            btnCancel.Size = new Size(364, 45);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(17, 818);
            btnDelete.Margin = new Padding(6, 5, 6, 5);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(9, 8, 9, 8);
            btnDelete.Size = new Size(364, 45);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "Delete";
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(17, 763);
            btnSave.Margin = new Padding(6, 5, 6, 5);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(9, 8, 9, 8);
            btnSave.Size = new Size(364, 45);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save";
            btnSave.Click += BtnSave_Click;
            // 
            // Editor_Npc
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(1059, 928);
            Controls.Add(btnSave);
            Controls.Add(btnDelete);
            Controls.Add(btnCancel);
            Controls.Add(DarkGroupBox5);
            Controls.Add(DarkGroupBox4);
            Controls.Add(DarkGroupBox3);
            Controls.Add(DarkGroupBox2);
            Controls.Add(DarkGroupBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(6, 5, 6, 5);
            Name = "Editor_Npc";
            Text = "Npc Editor";
            FormClosing += Editor_Npc_FormClosing;
            Load += Editor_Npc_Load;
            DarkGroupBox1.ResumeLayout(false);
            DarkGroupBox2.ResumeLayout(false);
            DarkGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpawnSecs).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDamage).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudExp).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudHp).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRange).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSprite).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSprite).EndInit();
            DarkGroupBox3.ResumeLayout(false);
            DarkGroupBox3.PerformLayout();
            DarkGroupBox4.ResumeLayout(false);
            DarkGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudAmount).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudChance).EndInit();
            DarkGroupBox5.ResumeLayout(false);
            DarkGroupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpirit).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudIntelligence).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLuck).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVitality).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStrength).EndInit();
            ResumeLayout(false);

        }

        internal DarkGroupBox DarkGroupBox1;
        internal ListBox lstIndex;
        internal DarkGroupBox DarkGroupBox2;
        internal DarkTextBox txtName;
        internal DarkLabel DarkLabel1;
        internal PictureBox picSprite;
        internal DarkTextBox txtAttackSay;
        internal DarkLabel DarkLabel2;
        internal DarkNumericUpDown nudSprite;
        internal DarkLabel DarkLabel3;
        internal DarkLabel DarkLabel4;
        internal DarkNumericUpDown nudRange;
        internal DarkComboBox cmbAnimation;
        internal DarkLabel DarkLabel7;
        internal DarkComboBox cmbFaction;
        internal DarkLabel DarkLabel8;
        internal DarkComboBox cmbBehaviour;
        internal DarkLabel DarkLabel5;
        internal DarkNumericUpDown nudHp;
        internal DarkLabel DarkLabel9;
        internal DarkNumericUpDown nudExp;
        internal DarkLabel DarkLabel10;
        internal DarkNumericUpDown nudDamage;
        internal DarkLabel DarkLabel12;
        internal DarkNumericUpDown nudLevel;
        internal DarkLabel DarkLabel11;
        internal DarkNumericUpDown nudSpawnSecs;
        internal DarkLabel DarkLabel13;
        internal DarkGroupBox DarkGroupBox3;
        internal DarkGroupBox DarkGroupBox4;
        internal DarkGroupBox DarkGroupBox5;
        internal DarkComboBox cmbSkill1;
        internal DarkLabel DarkLabel14;
        internal DarkComboBox cmbSkill6;
        internal DarkLabel DarkLabel17;
        internal DarkComboBox cmbSkill5;
        internal DarkLabel DarkLabel18;
        internal DarkComboBox cmbSkill4;
        internal DarkLabel DarkLabel19;
        internal DarkComboBox cmbSkill3;
        internal DarkLabel DarkLabel16;
        internal DarkComboBox cmbSkill2;
        internal DarkLabel DarkLabel15;
        internal DarkNumericUpDown nudStrength;
        internal DarkLabel DarkLabel20;
        internal DarkNumericUpDown nudSpirit;
        internal DarkLabel DarkLabel23;
        internal DarkNumericUpDown nudIntelligence;
        internal DarkLabel DarkLabel24;
        internal DarkNumericUpDown nudLuck;
        internal DarkLabel DarkLabel25;
        internal DarkNumericUpDown nudVitality;
        internal DarkLabel DarkLabel22;
        internal DarkLabel DarkLabel26;
        internal DarkComboBox cmbDropSlot;
        internal DarkNumericUpDown nudChance;
        internal DarkLabel DarkLabel27;
        internal DarkNumericUpDown nudAmount;
        internal DarkLabel DarkLabel29;
        internal DarkComboBox cmbItem;
        internal DarkLabel DarkLabel28;
        internal DarkButton btnCancel;
        internal DarkButton btnDelete;
        internal DarkButton btnSave;
        internal DarkComboBox cmbSpawnPeriod;
        internal DarkLabel DarkLabel30;
    }
}