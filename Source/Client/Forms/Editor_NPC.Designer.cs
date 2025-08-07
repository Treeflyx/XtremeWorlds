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
            DarkGroupBox1 = new DarkUI.Controls.DarkGroupBox();
            lstIndex = new ListBox();
            DarkGroupBox2 = new DarkUI.Controls.DarkGroupBox();
            cmbSpawnPeriod = new DarkUI.Controls.DarkComboBox();
            cmbLabel30 = new DarkUI.Controls.cmbLabel();
            nudSpawnSecs = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel13 = new DarkUI.Controls.cmbLabel();
            nudDamage = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel12 = new DarkUI.Controls.cmbLabel();
            nudLevel = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel11 = new DarkUI.Controls.cmbLabel();
            nudExp = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel10 = new DarkUI.Controls.cmbLabel();
            nudHp = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel9 = new DarkUI.Controls.cmbLabel();
            cmbFaction = new DarkUI.Controls.DarkComboBox();
            cmbLabel8 = new DarkUI.Controls.cmbLabel();
            cmbBehaviour = new DarkUI.Controls.DarkComboBox();
            cmbLabel5 = new DarkUI.Controls.cmbLabel();
            cmbAnimation = new DarkUI.Controls.DarkComboBox();
            cmbLabel7 = new DarkUI.Controls.cmbLabel();
            cmbLabel4 = new DarkUI.Controls.cmbLabel();
            nudRange = new DarkUI.Controls.DarkNumericUpDown();
            nudSprite = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel3 = new DarkUI.Controls.cmbLabel();
            txtAttackSay = new DarkUI.Controls.DarkTextBox();
            cmbLabel2 = new DarkUI.Controls.cmbLabel();
            picSprite = new PictureBox();
            txtName = new DarkUI.Controls.DarkTextBox();
            cmbLabel1 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox3 = new DarkUI.Controls.DarkGroupBox();
            cmbSkill6 = new DarkUI.Controls.DarkComboBox();
            cmbLabel17 = new DarkUI.Controls.cmbLabel();
            cmbSkill5 = new DarkUI.Controls.DarkComboBox();
            cmbLabel18 = new DarkUI.Controls.cmbLabel();
            cmbSkill4 = new DarkUI.Controls.DarkComboBox();
            cmbLabel19 = new DarkUI.Controls.cmbLabel();
            cmbSkill3 = new DarkUI.Controls.DarkComboBox();
            cmbLabel16 = new DarkUI.Controls.cmbLabel();
            cmbSkill2 = new DarkUI.Controls.DarkComboBox();
            cmbLabel15 = new DarkUI.Controls.cmbLabel();
            cmbSkill1 = new DarkUI.Controls.DarkComboBox();
            cmbLabel14 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox4 = new DarkUI.Controls.DarkGroupBox();
            nudAmount = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel29 = new DarkUI.Controls.cmbLabel();
            cmbItem = new DarkUI.Controls.DarkComboBox();
            cmbLabel28 = new DarkUI.Controls.cmbLabel();
            cmbDropSlot = new DarkUI.Controls.DarkComboBox();
            nudChance = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel27 = new DarkUI.Controls.cmbLabel();
            cmbLabel26 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox5 = new DarkUI.Controls.DarkGroupBox();
            nudSpirit = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel23 = new DarkUI.Controls.cmbLabel();
            nudIntelligence = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel24 = new DarkUI.Controls.cmbLabel();
            nudLuck = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel25 = new DarkUI.Controls.cmbLabel();
            nudVitality = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel22 = new DarkUI.Controls.cmbLabel();
            nudStrength = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel20 = new DarkUI.Controls.cmbLabel();
            btnCancel = new DarkUI.Controls.DarkButton();
            btnDelete = new DarkUI.Controls.DarkButton();
            btnSave = new DarkUI.Controls.DarkButton();
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
            DarkGroupBox1.Location = new Point(4, 2);
            DarkGroupBox1.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox1.Name = "DarkGroupBox1";
            DarkGroupBox1.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox1.Size = new Size(265, 451);
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
            lstIndex.Location = new Point(5, 18);
            lstIndex.Margin = new Padding(4, 3, 4, 3);
            lstIndex.Name = "lstIndex";
            lstIndex.Size = new Size(255, 422);
            lstIndex.TabIndex = 2;
            lstIndex.Click += lstIndex_Click;
            // 
            // DarkGroupBox2
            // 
            DarkGroupBox2.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox2.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox2.Controls.Add(cmbSpawnPeriod);
            DarkGroupBox2.Controls.Add(cmbLabel30);
            DarkGroupBox2.Controls.Add(nudSpawnSecs);
            DarkGroupBox2.Controls.Add(cmbLabel13);
            DarkGroupBox2.Controls.Add(nudDamage);
            DarkGroupBox2.Controls.Add(cmbLabel12);
            DarkGroupBox2.Controls.Add(nudLevel);
            DarkGroupBox2.Controls.Add(cmbLabel11);
            DarkGroupBox2.Controls.Add(nudExp);
            DarkGroupBox2.Controls.Add(cmbLabel10);
            DarkGroupBox2.Controls.Add(nudHp);
            DarkGroupBox2.Controls.Add(cmbLabel9);
            DarkGroupBox2.Controls.Add(cmbFaction);
            DarkGroupBox2.Controls.Add(cmbLabel8);
            DarkGroupBox2.Controls.Add(cmbBehaviour);
            DarkGroupBox2.Controls.Add(cmbLabel5);
            DarkGroupBox2.Controls.Add(cmbAnimation);
            DarkGroupBox2.Controls.Add(cmbLabel7);
            DarkGroupBox2.Controls.Add(cmbLabel4);
            DarkGroupBox2.Controls.Add(nudRange);
            DarkGroupBox2.Controls.Add(nudSprite);
            DarkGroupBox2.Controls.Add(cmbLabel3);
            DarkGroupBox2.Controls.Add(txtAttackSay);
            DarkGroupBox2.Controls.Add(cmbLabel2);
            DarkGroupBox2.Controls.Add(picSprite);
            DarkGroupBox2.Controls.Add(txtName);
            DarkGroupBox2.Controls.Add(cmbLabel1);
            DarkGroupBox2.ForeColor = Color.Gainsboro;
            DarkGroupBox2.Location = new Point(275, 2);
            DarkGroupBox2.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox2.Name = "DarkGroupBox2";
            DarkGroupBox2.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox2.Size = new Size(460, 267);
            DarkGroupBox2.TabIndex = 1;
            DarkGroupBox2.TabStop = false;
            DarkGroupBox2.Text = "Properties";
            // 
            // cmbSpawnPeriod
            // 
            cmbSpawnPeriod.DrawMode = DrawMode.OwnerDrawVariable;
            cmbSpawnPeriod.FormattingEnabled = true;
            cmbSpawnPeriod.Items.AddRange(new object[] { "Always", "Day", "Night", "Dawn", "Dusk" });
            cmbSpawnPeriod.Location = new Point(332, 233);
            cmbSpawnPeriod.Margin = new Padding(4, 3, 4, 3);
            cmbSpawnPeriod.Name = "cmbSpawnPeriod";
            cmbSpawnPeriod.Size = new Size(117, 24);
            cmbSpawnPeriod.TabIndex = 38;
            cmbSpawnPeriod.SelectedIndexChanged += CmbSpawnPeriod_SelectedIndexChanged;
            // 
            // cmbLabel30
            // 
            cmbLabel30.AutoSize = true;
            cmbLabel30.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel30.Location = new Point(274, 237);
            cmbLabel30.Margin = new Padding(4, 0, 4, 0);
            cmbLabel30.Name = "cmbLabel30";
            cmbLabel30.Size = new Size(50, 15);
            cmbLabel30.TabIndex = 37;
            cmbLabel30.Text = "Spawns:";
            // 
            // nudSpawnSecs
            // 
            nudSpawnSecs.Location = new Point(174, 235);
            nudSpawnSecs.Margin = new Padding(4, 3, 4, 3);
            nudSpawnSecs.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudSpawnSecs.Name = "nudSpawnSecs";
            nudSpawnSecs.Size = new Size(97, 23);
            nudSpawnSecs.TabIndex = 36;
            nudSpawnSecs.ValueChanged += NudSpawnSecs_ValueChanged;
            // 
            // cmbLabel13
            // 
            cmbLabel13.AutoSize = true;
            cmbLabel13.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel13.Location = new Point(7, 237);
            cmbLabel13.Margin = new Padding(4, 0, 4, 0);
            cmbLabel13.Name = "cmbLabel13";
            cmbLabel13.Size = new Size(147, 15);
            cmbLabel13.TabIndex = 35;
            cmbLabel13.Text = "Respawn Time in Seconds:";
            // 
            // nudDamage
            // 
            nudDamage.Location = new Point(310, 205);
            nudDamage.Margin = new Padding(4, 3, 4, 3);
            nudDamage.Name = "nudDamage";
            nudDamage.Size = new Size(140, 23);
            nudDamage.TabIndex = 34;
            nudDamage.ValueChanged += NudDamage_ValueChanged;
            // 
            // cmbLabel12
            // 
            cmbLabel12.AutoSize = true;
            cmbLabel12.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel12.Location = new Point(217, 207);
            cmbLabel12.Margin = new Padding(4, 0, 4, 0);
            cmbLabel12.Name = "cmbLabel12";
            cmbLabel12.Size = new Size(81, 15);
            cmbLabel12.TabIndex = 33;
            cmbLabel12.Text = "Base Damage:";
            // 
            // nudLevel
            // 
            nudLevel.Location = new Point(70, 205);
            nudLevel.Margin = new Padding(4, 3, 4, 3);
            nudLevel.Name = "nudLevel";
            nudLevel.Size = new Size(140, 23);
            nudLevel.TabIndex = 32;
            nudLevel.ValueChanged += NudLevel_ValueChanged;
            // 
            // cmbLabel11
            // 
            cmbLabel11.AutoSize = true;
            cmbLabel11.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel11.Location = new Point(7, 207);
            cmbLabel11.Margin = new Padding(4, 0, 4, 0);
            cmbLabel11.Name = "cmbLabel11";
            cmbLabel11.Size = new Size(37, 15);
            cmbLabel11.TabIndex = 31;
            cmbLabel11.Text = "Level:";
            // 
            // nudExp
            // 
            nudExp.Location = new Point(278, 175);
            nudExp.Margin = new Padding(4, 3, 4, 3);
            nudExp.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudExp.Name = "nudExp";
            nudExp.Size = new Size(173, 23);
            nudExp.TabIndex = 30;
            nudExp.ValueChanged += NudExp_ValueChanged;
            // 
            // cmbLabel10
            // 
            cmbLabel10.AutoSize = true;
            cmbLabel10.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel10.Location = new Point(202, 177);
            cmbLabel10.Margin = new Padding(4, 0, 4, 0);
            cmbLabel10.Name = "cmbLabel10";
            cmbLabel10.Size = new Size(61, 15);
            cmbLabel10.TabIndex = 29;
            cmbLabel10.Text = "Exp Given:";
            // 
            // nudHp
            // 
            nudHp.Location = new Point(70, 175);
            nudHp.Margin = new Padding(4, 3, 4, 3);
            nudHp.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudHp.Name = "nudHp";
            nudHp.Size = new Size(125, 23);
            nudHp.TabIndex = 28;
            nudHp.ValueChanged += NudHp_ValueChanged;
            // 
            // cmbLabel9
            // 
            cmbLabel9.AutoSize = true;
            cmbLabel9.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel9.Location = new Point(7, 177);
            cmbLabel9.Margin = new Padding(4, 0, 4, 0);
            cmbLabel9.Name = "cmbLabel9";
            cmbLabel9.Size = new Size(45, 15);
            cmbLabel9.TabIndex = 27;
            cmbLabel9.Text = "Health:";
            // 
            // cmbFaction
            // 
            cmbFaction.DrawMode = DrawMode.OwnerDrawFixed;
            cmbFaction.FormattingEnabled = true;
            cmbFaction.Items.AddRange(new object[] { "None", "Good", "Bad" });
            cmbFaction.Location = new Point(302, 143);
            cmbFaction.Margin = new Padding(4, 3, 4, 3);
            cmbFaction.Name = "cmbFaction";
            cmbFaction.Size = new Size(148, 24);
            cmbFaction.TabIndex = 26;
            cmbFaction.SelectedIndexChanged += CmbFaction_SelectedIndexChanged;
            // 
            // cmbLabel8
            // 
            cmbLabel8.AutoSize = true;
            cmbLabel8.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel8.Location = new Point(239, 147);
            cmbLabel8.Margin = new Padding(4, 0, 4, 0);
            cmbLabel8.Name = "cmbLabel8";
            cmbLabel8.Size = new Size(49, 15);
            cmbLabel8.TabIndex = 25;
            cmbLabel8.Text = "Faction:";
            // 
            // cmbBehaviour
            // 
            cmbBehaviour.DrawMode = DrawMode.OwnerDrawFixed;
            cmbBehaviour.FormattingEnabled = true;
            cmbBehaviour.Items.AddRange(new object[] { "Attack on sight", "Attack when attacked", "Friendly", "Shop keeper", "Guard", "Quest" });
            cmbBehaviour.Location = new Point(70, 143);
            cmbBehaviour.Margin = new Padding(4, 3, 4, 3);
            cmbBehaviour.Name = "cmbBehaviour";
            cmbBehaviour.Size = new Size(162, 24);
            cmbBehaviour.TabIndex = 24;
            cmbBehaviour.SelectedIndexChanged += CmbBehavior_SelectedIndexChanged;
            // 
            // cmbLabel5
            // 
            cmbLabel5.AutoSize = true;
            cmbLabel5.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel5.Location = new Point(7, 147);
            cmbLabel5.Margin = new Padding(4, 0, 4, 0);
            cmbLabel5.Name = "cmbLabel5";
            cmbLabel5.Size = new Size(56, 15);
            cmbLabel5.TabIndex = 23;
            cmbLabel5.Text = "Behavior:";
            // 
            // cmbAnimation
            // 
            cmbAnimation.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAnimation.FormattingEnabled = true;
            cmbAnimation.Location = new Point(84, 113);
            cmbAnimation.Margin = new Padding(4, 3, 4, 3);
            cmbAnimation.Name = "cmbAnimation";
            cmbAnimation.Size = new Size(125, 24);
            cmbAnimation.TabIndex = 21;
            cmbAnimation.SelectedIndexChanged += CmbAnimation_SelectedIndexChanged;
            // 
            // cmbLabel7
            // 
            cmbLabel7.AutoSize = true;
            cmbLabel7.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel7.Location = new Point(7, 115);
            cmbLabel7.Margin = new Padding(4, 0, 4, 0);
            cmbLabel7.Name = "cmbLabel7";
            cmbLabel7.Size = new Size(66, 15);
            cmbLabel7.TabIndex = 20;
            cmbLabel7.Text = "Animation:";
            // 
            // cmbLabel4
            // 
            cmbLabel4.AutoSize = true;
            cmbLabel4.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel4.Location = new Point(7, 85);
            cmbLabel4.Margin = new Padding(4, 0, 4, 0);
            cmbLabel4.Name = "cmbLabel4";
            cmbLabel4.Size = new Size(43, 15);
            cmbLabel4.TabIndex = 15;
            cmbLabel4.Text = "Range:";
            // 
            // nudRange
            // 
            nudRange.Location = new Point(70, 82);
            nudRange.Margin = new Padding(4, 3, 4, 3);
            nudRange.Name = "nudRange";
            nudRange.Size = new Size(126, 23);
            nudRange.TabIndex = 14;
            nudRange.ValueChanged += NudRange_ValueChanged;
            // 
            // nudSprite
            // 
            nudSprite.Location = new Point(253, 82);
            nudSprite.Margin = new Padding(4, 3, 4, 3);
            nudSprite.Name = "nudSprite";
            nudSprite.Size = new Size(112, 23);
            nudSprite.TabIndex = 13;
            nudSprite.Click += NudSprite_Click;
            // 
            // cmbLabel3
            // 
            cmbLabel3.AutoSize = true;
            cmbLabel3.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel3.Location = new Point(203, 85);
            cmbLabel3.Margin = new Padding(4, 0, 4, 0);
            cmbLabel3.Name = "cmbLabel3";
            cmbLabel3.Size = new Size(40, 15);
            cmbLabel3.TabIndex = 12;
            cmbLabel3.Text = "Sprite:";
            // 
            // txtAttackSay
            // 
            txtAttackSay.BackColor = Color.FromArgb(69, 73, 74);
            txtAttackSay.BorderStyle = BorderStyle.FixedSingle;
            txtAttackSay.ForeColor = Color.FromArgb(220, 220, 220);
            txtAttackSay.Location = new Point(70, 52);
            txtAttackSay.Margin = new Padding(4, 3, 4, 3);
            txtAttackSay.Name = "txtAttackSay";
            txtAttackSay.Size = new Size(295, 23);
            txtAttackSay.TabIndex = 11;
            txtAttackSay.TextChanged += TxtAttackSay_TextChanged;
            // 
            // cmbLabel2
            // 
            cmbLabel2.AutoSize = true;
            cmbLabel2.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel2.Location = new Point(7, 55);
            cmbLabel2.Margin = new Padding(4, 0, 4, 0);
            cmbLabel2.Name = "cmbLabel2";
            cmbLabel2.Size = new Size(28, 15);
            cmbLabel2.TabIndex = 10;
            cmbLabel2.Text = "Say:";
            // 
            // picSprite
            // 
            picSprite.BackColor = Color.Black;
            picSprite.BackgroundImageLayout = ImageLayout.None;
            picSprite.Location = new Point(372, 22);
            picSprite.Margin = new Padding(4, 3, 4, 3);
            picSprite.Name = "picSprite";
            picSprite.Size = new Size(32, 32);
            picSprite.TabIndex = 9;
            picSprite.TabStop = false;
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.FromArgb(220, 220, 220);
            txtName.Location = new Point(70, 22);
            txtName.Margin = new Padding(4, 3, 4, 3);
            txtName.Name = "txtName";
            txtName.Size = new Size(295, 23);
            txtName.TabIndex = 1;
            txtName.TextChanged += TxtName_TextChanged;
            // 
            // cmbLabel1
            // 
            cmbLabel1.AutoSize = true;
            cmbLabel1.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel1.Location = new Point(7, 25);
            cmbLabel1.Margin = new Padding(4, 0, 4, 0);
            cmbLabel1.Name = "cmbLabel1";
            cmbLabel1.Size = new Size(42, 15);
            cmbLabel1.TabIndex = 0;
            cmbLabel1.Text = "Name:";
            // 
            // DarkGroupBox3
            // 
            DarkGroupBox3.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox3.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox3.Controls.Add(cmbSkill6);
            DarkGroupBox3.Controls.Add(cmbLabel17);
            DarkGroupBox3.Controls.Add(cmbSkill5);
            DarkGroupBox3.Controls.Add(cmbLabel18);
            DarkGroupBox3.Controls.Add(cmbSkill4);
            DarkGroupBox3.Controls.Add(cmbLabel19);
            DarkGroupBox3.Controls.Add(cmbSkill3);
            DarkGroupBox3.Controls.Add(cmbLabel16);
            DarkGroupBox3.Controls.Add(cmbSkill2);
            DarkGroupBox3.Controls.Add(cmbLabel15);
            DarkGroupBox3.Controls.Add(cmbSkill1);
            DarkGroupBox3.Controls.Add(cmbLabel14);
            DarkGroupBox3.ForeColor = Color.Gainsboro;
            DarkGroupBox3.Location = new Point(275, 275);
            DarkGroupBox3.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox3.Name = "DarkGroupBox3";
            DarkGroupBox3.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox3.Size = new Size(460, 82);
            DarkGroupBox3.TabIndex = 2;
            DarkGroupBox3.TabStop = false;
            DarkGroupBox3.Text = "Skills";
            // 
            // cmbSkill6
            // 
            cmbSkill6.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkill6.FormattingEnabled = true;
            cmbSkill6.Location = new Point(332, 46);
            cmbSkill6.Margin = new Padding(4, 3, 4, 3);
            cmbSkill6.Name = "cmbSkill6";
            cmbSkill6.Size = new Size(122, 24);
            cmbSkill6.TabIndex = 11;
            cmbSkill6.SelectedIndexChanged += CmbSkill6_SelectedIndexChanged;
            // 
            // cmbLabel17
            // 
            cmbLabel17.AutoSize = true;
            cmbLabel17.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel17.Location = new Point(310, 50);
            cmbLabel17.Margin = new Padding(4, 0, 4, 0);
            cmbLabel17.Name = "cmbLabel17";
            cmbLabel17.Size = new Size(13, 15);
            cmbLabel17.TabIndex = 10;
            cmbLabel17.Text = "6";
            // 
            // cmbSkill5
            // 
            cmbSkill5.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkill5.FormattingEnabled = true;
            cmbSkill5.Location = new Point(181, 46);
            cmbSkill5.Margin = new Padding(4, 3, 4, 3);
            cmbSkill5.Name = "cmbSkill5";
            cmbSkill5.Size = new Size(122, 24);
            cmbSkill5.TabIndex = 9;
            cmbSkill5.SelectedIndexChanged += CmbSkill5_SelectedIndexChanged;
            // 
            // cmbLabel18
            // 
            cmbLabel18.AutoSize = true;
            cmbLabel18.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel18.Location = new Point(159, 50);
            cmbLabel18.Margin = new Padding(4, 0, 4, 0);
            cmbLabel18.Name = "cmbLabel18";
            cmbLabel18.Size = new Size(13, 15);
            cmbLabel18.TabIndex = 8;
            cmbLabel18.Text = "5";
            // 
            // cmbSkill4
            // 
            cmbSkill4.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkill4.FormattingEnabled = true;
            cmbSkill4.Location = new Point(29, 46);
            cmbSkill4.Margin = new Padding(4, 3, 4, 3);
            cmbSkill4.Name = "cmbSkill4";
            cmbSkill4.Size = new Size(122, 24);
            cmbSkill4.TabIndex = 7;
            cmbSkill4.SelectedIndexChanged += CmbSkill4_SelectedIndexChanged;
            // 
            // cmbLabel19
            // 
            cmbLabel19.AutoSize = true;
            cmbLabel19.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel19.Location = new Point(7, 50);
            cmbLabel19.Margin = new Padding(4, 0, 4, 0);
            cmbLabel19.Name = "cmbLabel19";
            cmbLabel19.Size = new Size(13, 15);
            cmbLabel19.TabIndex = 6;
            cmbLabel19.Text = "4";
            // 
            // cmbSkill3
            // 
            cmbSkill3.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkill3.FormattingEnabled = true;
            cmbSkill3.Location = new Point(332, 15);
            cmbSkill3.Margin = new Padding(4, 3, 4, 3);
            cmbSkill3.Name = "cmbSkill3";
            cmbSkill3.Size = new Size(122, 24);
            cmbSkill3.TabIndex = 5;
            cmbSkill3.SelectedIndexChanged += CmbSkill3_SelectedIndexChanged;
            // 
            // cmbLabel16
            // 
            cmbLabel16.AutoSize = true;
            cmbLabel16.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel16.Location = new Point(310, 18);
            cmbLabel16.Margin = new Padding(4, 0, 4, 0);
            cmbLabel16.Name = "cmbLabel16";
            cmbLabel16.Size = new Size(13, 15);
            cmbLabel16.TabIndex = 4;
            cmbLabel16.Text = "3";
            // 
            // cmbSkill2
            // 
            cmbSkill2.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkill2.FormattingEnabled = true;
            cmbSkill2.Location = new Point(181, 15);
            cmbSkill2.Margin = new Padding(4, 3, 4, 3);
            cmbSkill2.Name = "cmbSkill2";
            cmbSkill2.Size = new Size(122, 24);
            cmbSkill2.TabIndex = 3;
            cmbSkill2.SelectedIndexChanged += CmbSkill2_SelectedIndexChanged;
            // 
            // cmbLabel15
            // 
            cmbLabel15.AutoSize = true;
            cmbLabel15.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel15.Location = new Point(159, 18);
            cmbLabel15.Margin = new Padding(4, 0, 4, 0);
            cmbLabel15.Name = "cmbLabel15";
            cmbLabel15.Size = new Size(13, 15);
            cmbLabel15.TabIndex = 2;
            cmbLabel15.Text = "2";
            // 
            // cmbSkill1
            // 
            cmbSkill1.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSkill1.FormattingEnabled = true;
            cmbSkill1.Location = new Point(29, 15);
            cmbSkill1.Margin = new Padding(4, 3, 4, 3);
            cmbSkill1.Name = "cmbSkill1";
            cmbSkill1.Size = new Size(122, 24);
            cmbSkill1.TabIndex = 1;
            cmbSkill1.SelectedIndexChanged += CmbSkill1_SelectedIndexChanged;
            // 
            // cmbLabel14
            // 
            cmbLabel14.AutoSize = true;
            cmbLabel14.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel14.Location = new Point(7, 18);
            cmbLabel14.Margin = new Padding(4, 0, 4, 0);
            cmbLabel14.Name = "cmbLabel14";
            cmbLabel14.Size = new Size(13, 15);
            cmbLabel14.TabIndex = 0;
            cmbLabel14.Text = "1";
            // 
            // DarkGroupBox4
            // 
            DarkGroupBox4.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox4.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox4.Controls.Add(nudAmount);
            DarkGroupBox4.Controls.Add(cmbLabel29);
            DarkGroupBox4.Controls.Add(cmbItem);
            DarkGroupBox4.Controls.Add(cmbLabel28);
            DarkGroupBox4.Controls.Add(cmbDropSlot);
            DarkGroupBox4.Controls.Add(nudChance);
            DarkGroupBox4.Controls.Add(cmbLabel27);
            DarkGroupBox4.Controls.Add(cmbLabel26);
            DarkGroupBox4.ForeColor = Color.Gainsboro;
            DarkGroupBox4.Location = new Point(275, 462);
            DarkGroupBox4.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox4.Name = "DarkGroupBox4";
            DarkGroupBox4.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox4.Size = new Size(460, 89);
            DarkGroupBox4.TabIndex = 3;
            DarkGroupBox4.TabStop = false;
            DarkGroupBox4.Text = "Drop Items";
            // 
            // nudAmount
            // 
            nudAmount.Location = new Point(313, 50);
            nudAmount.Margin = new Padding(4, 3, 4, 3);
            nudAmount.Name = "nudAmount";
            nudAmount.Size = new Size(140, 23);
            nudAmount.TabIndex = 7;
            nudAmount.ValueChanged += ScrlValue_Scroll;
            // 
            // cmbLabel29
            // 
            cmbLabel29.AutoSize = true;
            cmbLabel29.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel29.Location = new Point(239, 53);
            cmbLabel29.Margin = new Padding(4, 0, 4, 0);
            cmbLabel29.Name = "cmbLabel29";
            cmbLabel29.Size = new Size(54, 15);
            cmbLabel29.TabIndex = 6;
            cmbLabel29.Text = "Amount:";
            // 
            // cmbItem
            // 
            cmbItem.DrawMode = DrawMode.OwnerDrawFixed;
            cmbItem.FormattingEnabled = true;
            cmbItem.Location = new Point(77, 50);
            cmbItem.Margin = new Padding(4, 3, 4, 3);
            cmbItem.Name = "cmbItem";
            cmbItem.Size = new Size(140, 24);
            cmbItem.TabIndex = 5;
            cmbItem.SelectedIndexChanged += CmbItem_SelectedIndexChanged;
            // 
            // cmbLabel28
            // 
            cmbLabel28.AutoSize = true;
            cmbLabel28.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel28.Location = new Point(7, 53);
            cmbLabel28.Margin = new Padding(4, 0, 4, 0);
            cmbLabel28.Name = "cmbLabel28";
            cmbLabel28.Size = new Size(34, 15);
            cmbLabel28.TabIndex = 4;
            cmbLabel28.Text = "Item:";
            // 
            // cmbDropSlot
            // 
            cmbDropSlot.DrawMode = DrawMode.OwnerDrawFixed;
            cmbDropSlot.FormattingEnabled = true;
            cmbDropSlot.Items.AddRange(new object[] { "Slot 1", "Slot 2", "Slot 3", "Slot 4", "Slot 5" });
            cmbDropSlot.Location = new Point(77, 15);
            cmbDropSlot.Margin = new Padding(4, 3, 4, 3);
            cmbDropSlot.Name = "cmbDropSlot";
            cmbDropSlot.Size = new Size(140, 24);
            cmbDropSlot.TabIndex = 3;
            cmbDropSlot.SelectedIndexChanged += CmbDropSlot_SelectedIndexChanged;
            // 
            // nudChance
            // 
            nudChance.Location = new Point(343, 16);
            nudChance.Margin = new Padding(4, 3, 4, 3);
            nudChance.Name = "nudChance";
            nudChance.Size = new Size(110, 23);
            nudChance.TabIndex = 2;
            nudChance.ValueChanged += NudChance_ValueChanged;
            // 
            // cmbLabel27
            // 
            cmbLabel27.AutoSize = true;
            cmbLabel27.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel27.Location = new Point(239, 18);
            cmbLabel27.Margin = new Padding(4, 0, 4, 0);
            cmbLabel27.Name = "cmbLabel27";
            cmbLabel27.Size = new Size(91, 15);
            cmbLabel27.TabIndex = 1;
            cmbLabel27.Text = "Chance 1 out of";
            // 
            // cmbLabel26
            // 
            cmbLabel26.AutoSize = true;
            cmbLabel26.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel26.Location = new Point(7, 18);
            cmbLabel26.Margin = new Padding(4, 0, 4, 0);
            cmbLabel26.Name = "cmbLabel26";
            cmbLabel26.Size = new Size(59, 15);
            cmbLabel26.TabIndex = 0;
            cmbLabel26.Text = "Drop Slot:";
            // 
            // DarkGroupBox5
            // 
            DarkGroupBox5.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox5.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox5.Controls.Add(nudSpirit);
            DarkGroupBox5.Controls.Add(cmbLabel23);
            DarkGroupBox5.Controls.Add(nudIntelligence);
            DarkGroupBox5.Controls.Add(cmbLabel24);
            DarkGroupBox5.Controls.Add(nudLuck);
            DarkGroupBox5.Controls.Add(cmbLabel25);
            DarkGroupBox5.Controls.Add(nudVitality);
            DarkGroupBox5.Controls.Add(cmbLabel22);
            DarkGroupBox5.Controls.Add(nudStrength);
            DarkGroupBox5.Controls.Add(cmbLabel20);
            DarkGroupBox5.ForeColor = Color.Gainsboro;
            DarkGroupBox5.Location = new Point(275, 365);
            DarkGroupBox5.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox5.Name = "DarkGroupBox5";
            DarkGroupBox5.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox5.Size = new Size(460, 90);
            DarkGroupBox5.TabIndex = 4;
            DarkGroupBox5.TabStop = false;
            DarkGroupBox5.Text = "Stats";
            // 
            // nudSpirit
            // 
            nudSpirit.Location = new Point(376, 25);
            nudSpirit.Margin = new Padding(4, 3, 4, 3);
            nudSpirit.Name = "nudSpirit";
            nudSpirit.Size = new Size(74, 23);
            nudSpirit.TabIndex = 11;
            nudSpirit.ValueChanged += NudSpirit_ValueChanged;
            // 
            // cmbLabel23
            // 
            cmbLabel23.AutoSize = true;
            cmbLabel23.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel23.Location = new Point(313, 27);
            cmbLabel23.Margin = new Padding(4, 0, 4, 0);
            cmbLabel23.Name = "cmbLabel23";
            cmbLabel23.Size = new Size(37, 15);
            cmbLabel23.TabIndex = 10;
            cmbLabel23.Text = "Spirit:";
            // 
            // nudIntelligence
            // 
            nudIntelligence.Location = new Point(230, 52);
            nudIntelligence.Margin = new Padding(4, 3, 4, 3);
            nudIntelligence.Name = "nudIntelligence";
            nudIntelligence.Size = new Size(74, 23);
            nudIntelligence.TabIndex = 9;
            nudIntelligence.ValueChanged += NudIntelligence_ValueChanged;
            // 
            // cmbLabel24
            // 
            cmbLabel24.AutoSize = true;
            cmbLabel24.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel24.Location = new Point(150, 55);
            cmbLabel24.Margin = new Padding(4, 0, 4, 0);
            cmbLabel24.Name = "cmbLabel24";
            cmbLabel24.Size = new Size(71, 15);
            cmbLabel24.TabIndex = 8;
            cmbLabel24.Text = "Intelligence:";
            // 
            // nudLuck
            // 
            nudLuck.Location = new Point(70, 52);
            nudLuck.Margin = new Padding(4, 3, 4, 3);
            nudLuck.Name = "nudLuck";
            nudLuck.Size = new Size(74, 23);
            nudLuck.TabIndex = 7;
            nudLuck.ValueChanged += NudLuck_ValueChanged;
            // 
            // cmbLabel25
            // 
            cmbLabel25.AutoSize = true;
            cmbLabel25.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel25.Location = new Point(7, 55);
            cmbLabel25.Margin = new Padding(4, 0, 4, 0);
            cmbLabel25.Name = "cmbLabel25";
            cmbLabel25.Size = new Size(35, 15);
            cmbLabel25.TabIndex = 6;
            cmbLabel25.Text = "Luck:";
            // 
            // nudVitality
            // 
            nudVitality.Location = new Point(230, 23);
            nudVitality.Margin = new Padding(4, 3, 4, 3);
            nudVitality.Name = "nudVitality";
            nudVitality.Size = new Size(74, 23);
            nudVitality.TabIndex = 5;
            nudVitality.ValueChanged += NudVitality_ValueChanged;
            // 
            // cmbLabel22
            // 
            cmbLabel22.AutoSize = true;
            cmbLabel22.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel22.Location = new Point(167, 26);
            cmbLabel22.Margin = new Padding(4, 0, 4, 0);
            cmbLabel22.Name = "cmbLabel22";
            cmbLabel22.Size = new Size(46, 15);
            cmbLabel22.TabIndex = 4;
            cmbLabel22.Text = "Vitality:";
            // 
            // nudStrength
            // 
            nudStrength.Location = new Point(70, 22);
            nudStrength.Margin = new Padding(4, 3, 4, 3);
            nudStrength.Name = "nudStrength";
            nudStrength.Size = new Size(74, 23);
            nudStrength.TabIndex = 1;
            nudStrength.ValueChanged += NudStrength_ValueChanged;
            // 
            // cmbLabel20
            // 
            cmbLabel20.AutoSize = true;
            cmbLabel20.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel20.Location = new Point(7, 25);
            cmbLabel20.Margin = new Padding(4, 0, 4, 0);
            cmbLabel20.Name = "cmbLabel20";
            cmbLabel20.Size = new Size(55, 15);
            cmbLabel20.TabIndex = 0;
            cmbLabel20.Text = "Strength:";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(12, 524);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(6, 5, 6, 5);
            btnCancel.Size = new Size(255, 27);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(12, 491);
            btnDelete.Margin = new Padding(4, 3, 4, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(6, 5, 6, 5);
            btnDelete.Size = new Size(255, 27);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "Delete";
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(12, 458);
            btnSave.Margin = new Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(6, 5, 6, 5);
            btnSave.Size = new Size(255, 27);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save";
            btnSave.Click += BtnSave_Click;
            // 
            // Editor_Npc
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(741, 557);
            Controls.Add(btnSave);
            Controls.Add(btnDelete);
            Controls.Add(btnCancel);
            Controls.Add(DarkGroupBox5);
            Controls.Add(DarkGroupBox4);
            Controls.Add(DarkGroupBox3);
            Controls.Add(DarkGroupBox2);
            Controls.Add(DarkGroupBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(4, 3, 4, 3);
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
        internal cmbLabel cmbLabel1;
        internal PictureBox picSprite;
        internal DarkTextBox txtAttackSay;
        internal cmbLabel cmbLabel2;
        internal DarkNumericUpDown nudSprite;
        internal cmbLabel cmbLabel3;
        internal cmbLabel cmbLabel4;
        internal DarkNumericUpDown nudRange;
        internal DarkComboBox cmbAnimation;
        internal cmbLabel cmbLabel7;
        internal DarkComboBox cmbFaction;
        internal cmbLabel cmbLabel8;
        internal DarkComboBox cmbBehaviour;
        internal cmbLabel cmbLabel5;
        internal DarkNumericUpDown nudHp;
        internal cmbLabel cmbLabel9;
        internal DarkNumericUpDown nudExp;
        internal cmbLabel cmbLabel10;
        internal DarkNumericUpDown nudDamage;
        internal cmbLabel cmbLabel12;
        internal DarkNumericUpDown nudLevel;
        internal cmbLabel cmbLabel11;
        internal DarkNumericUpDown nudSpawnSecs;
        internal cmbLabel cmbLabel13;
        internal DarkGroupBox DarkGroupBox3;
        internal DarkGroupBox DarkGroupBox4;
        internal DarkGroupBox DarkGroupBox5;
        internal DarkComboBox cmbSkill1;
        internal cmbLabel cmbLabel14;
        internal DarkComboBox cmbSkill6;
        internal cmbLabel cmbLabel17;
        internal DarkComboBox cmbSkill5;
        internal cmbLabel cmbLabel18;
        internal DarkComboBox cmbSkill4;
        internal cmbLabel cmbLabel19;
        internal DarkComboBox cmbSkill3;
        internal cmbLabel cmbLabel16;
        internal DarkComboBox cmbSkill2;
        internal cmbLabel cmbLabel15;
        internal DarkNumericUpDown nudStrength;
        internal cmbLabel cmbLabel20;
        internal DarkNumericUpDown nudSpirit;
        internal cmbLabel cmbLabel23;
        internal DarkNumericUpDown nudIntelligence;
        internal cmbLabel cmbLabel24;
        internal DarkNumericUpDown nudLuck;
        internal cmbLabel cmbLabel25;
        internal DarkNumericUpDown nudVitality;
        internal cmbLabel cmbLabel22;
        internal cmbLabel cmbLabel26;
        internal DarkComboBox cmbDropSlot;
        internal DarkNumericUpDown nudChance;
        internal cmbLabel cmbLabel27;
        internal DarkNumericUpDown nudAmount;
        internal cmbLabel cmbLabel29;
        internal DarkComboBox cmbItem;
        internal cmbLabel cmbLabel28;
        internal DarkButton btnCancel;
        internal DarkButton btnDelete;
        internal DarkButton btnSave;
        internal DarkComboBox cmbSpawnPeriod;
        internal cmbLabel cmbLabel30;
    }
}