using DarkUI.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public partial class Editor_Skill : Form
    {
        // Shared instance of the form
        private static Editor_Skill _instance;

        // Public property to get the shared instance
        public static Editor_Skill Instance
        {
            get
            {
                // Create a new instance if one does not exist or if it has been disposed
                if (_instance is null || _instance.IsDisposed)
                {
                    _instance = new Editor_Skill();
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
            btnLearn = new DarkUI.Controls.DarkButton();
            DarkGroupBox5 = new DarkUI.Controls.DarkGroupBox();
            DarkGroupBox8 = new DarkUI.Controls.DarkGroupBox();
            cmbAnim = new DarkUI.Controls.DarkComboBox();
            cmbLabel23 = new DarkUI.Controls.cmbLabel();
            cmbAnimCast = new DarkUI.Controls.DarkComboBox();
            cmbLabel22 = new DarkUI.Controls.cmbLabel();
            nudStun = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel21 = new DarkUI.Controls.cmbLabel();
            cmbLabel20 = new DarkUI.Controls.cmbLabel();
            nudAoE = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel19 = new DarkUI.Controls.cmbLabel();
            chkAoE = new DarkUI.Controls.DarkCheckBox();
            cmbLabel18 = new DarkUI.Controls.cmbLabel();
            nudRange = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel17 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox7 = new DarkUI.Controls.DarkGroupBox();
            nudInterval = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel16 = new DarkUI.Controls.cmbLabel();
            nudDuration = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel15 = new DarkUI.Controls.cmbLabel();
            nudVital = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel14 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox6 = new DarkUI.Controls.DarkGroupBox();
            nudY = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel13 = new DarkUI.Controls.cmbLabel();
            nudX = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel12 = new DarkUI.Controls.cmbLabel();
            cmbDir = new DarkUI.Controls.DarkComboBox();
            cmbLabel11 = new DarkUI.Controls.cmbLabel();
            nudMap = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel10 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox3 = new DarkUI.Controls.DarkGroupBox();
            chkKnockBack = new DarkUI.Controls.DarkCheckBox();
            cmbKnockBackTiles = new DarkUI.Controls.DarkComboBox();
            cmbProjectile = new DarkUI.Controls.DarkComboBox();
            chkProjectile = new DarkUI.Controls.DarkCheckBox();
            nudIcon = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel9 = new DarkUI.Controls.cmbLabel();
            picSprite = new PictureBox();
            nudCool = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel8 = new DarkUI.Controls.cmbLabel();
            nudCast = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel7 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox4 = new DarkUI.Controls.DarkGroupBox();
            cmbLabel6 = new DarkUI.Controls.cmbLabel();
            cmbJob = new DarkUI.Controls.DarkComboBox();
            cmbAccessReq = new DarkUI.Controls.DarkComboBox();
            cmbLabel5 = new DarkUI.Controls.cmbLabel();
            nudLevel = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel4 = new DarkUI.Controls.cmbLabel();
            nudMp = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel3 = new DarkUI.Controls.cmbLabel();
            cmbType = new DarkUI.Controls.DarkComboBox();
            cmbLabel2 = new DarkUI.Controls.cmbLabel();
            txtName = new DarkUI.Controls.DarkTextBox();
            cmbLabel1 = new DarkUI.Controls.cmbLabel();
            btnDelete = new DarkUI.Controls.DarkButton();
            btnCancel = new DarkUI.Controls.DarkButton();
            btnSave = new DarkUI.Controls.DarkButton();
            DarkGroupBox1.SuspendLayout();
            DarkGroupBox2.SuspendLayout();
            DarkGroupBox5.SuspendLayout();
            DarkGroupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudStun).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAoE).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRange).BeginInit();
            DarkGroupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudInterval).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDuration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVital).BeginInit();
            DarkGroupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMap).BeginInit();
            DarkGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSprite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCool).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCast).BeginInit();
            DarkGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMp).BeginInit();
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
            DarkGroupBox1.Size = new Size(214, 357);
            DarkGroupBox1.TabIndex = 0;
            DarkGroupBox1.TabStop = false;
            DarkGroupBox1.Text = "Skill List";
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
            lstIndex.Size = new Size(199, 332);
            lstIndex.TabIndex = 1;
            lstIndex.Click += lstIndex_Click;
            // 
            // DarkGroupBox2
            // 
            DarkGroupBox2.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox2.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox2.Controls.Add(btnLearn);
            DarkGroupBox2.Controls.Add(DarkGroupBox5);
            DarkGroupBox2.Controls.Add(DarkGroupBox3);
            DarkGroupBox2.ForeColor = Color.Gainsboro;
            DarkGroupBox2.Location = new Point(224, 3);
            DarkGroupBox2.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox2.Name = "DarkGroupBox2";
            DarkGroupBox2.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox2.Size = new Size(722, 455);
            DarkGroupBox2.TabIndex = 1;
            DarkGroupBox2.TabStop = false;
            DarkGroupBox2.Text = "Properties";
            // 
            // btnLearn
            // 
            btnLearn.Location = new Point(11, 413);
            btnLearn.Margin = new Padding(4, 3, 4, 3);
            btnLearn.Name = "btnLearn";
            btnLearn.Padding = new Padding(6, 6, 6, 6);
            btnLearn.Size = new Size(88, 27);
            btnLearn.TabIndex = 11;
            btnLearn.Text = "Learn";
            btnLearn.Click += btnLearn_Click;
            // 
            // DarkGroupBox5
            // 
            DarkGroupBox5.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox5.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox5.Controls.Add(DarkGroupBox8);
            DarkGroupBox5.Controls.Add(DarkGroupBox7);
            DarkGroupBox5.Controls.Add(DarkGroupBox6);
            DarkGroupBox5.ForeColor = Color.Gainsboro;
            DarkGroupBox5.Location = new Point(399, 22);
            DarkGroupBox5.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox5.Name = "DarkGroupBox5";
            DarkGroupBox5.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox5.Size = new Size(312, 423);
            DarkGroupBox5.TabIndex = 1;
            DarkGroupBox5.TabStop = false;
            DarkGroupBox5.Text = "Data";
            // 
            // DarkGroupBox8
            // 
            DarkGroupBox8.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox8.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox8.Controls.Add(cmbAnim);
            DarkGroupBox8.Controls.Add(cmbLabel23);
            DarkGroupBox8.Controls.Add(cmbAnimCast);
            DarkGroupBox8.Controls.Add(cmbLabel22);
            DarkGroupBox8.Controls.Add(nudStun);
            DarkGroupBox8.Controls.Add(cmbLabel21);
            DarkGroupBox8.Controls.Add(cmbLabel20);
            DarkGroupBox8.Controls.Add(nudAoE);
            DarkGroupBox8.Controls.Add(cmbLabel19);
            DarkGroupBox8.Controls.Add(chkAoE);
            DarkGroupBox8.Controls.Add(cmbLabel18);
            DarkGroupBox8.Controls.Add(nudRange);
            DarkGroupBox8.Controls.Add(cmbLabel17);
            DarkGroupBox8.ForeColor = Color.Gainsboro;
            DarkGroupBox8.Location = new Point(7, 209);
            DarkGroupBox8.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox8.Name = "DarkGroupBox8";
            DarkGroupBox8.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox8.Size = new Size(296, 209);
            DarkGroupBox8.TabIndex = 2;
            DarkGroupBox8.TabStop = false;
            DarkGroupBox8.Text = "Cast Settings";
            // 
            // cmbAnim
            // 
            cmbAnim.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAnim.FormattingEnabled = true;
            cmbAnim.Location = new Point(121, 177);
            cmbAnim.Margin = new Padding(4, 3, 4, 3);
            cmbAnim.Name = "cmbAnim";
            cmbAnim.Size = new Size(167, 24);
            cmbAnim.TabIndex = 12;
            cmbAnim.SelectedIndexChanged += CmbAnim_Scroll;
            // 
            // cmbLabel23
            // 
            cmbLabel23.AutoSize = true;
            cmbLabel23.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel23.Location = new Point(7, 180);
            cmbLabel23.Margin = new Padding(4, 0, 4, 0);
            cmbLabel23.Name = "cmbLabel23";
            cmbLabel23.Size = new Size(66, 15);
            cmbLabel23.TabIndex = 11;
            cmbLabel23.Text = "Animation:";
            // 
            // cmbAnimCast
            // 
            cmbAnimCast.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAnimCast.FormattingEnabled = true;
            cmbAnimCast.Location = new Point(121, 145);
            cmbAnimCast.Margin = new Padding(4, 3, 4, 3);
            cmbAnimCast.Name = "cmbAnimCast";
            cmbAnimCast.Size = new Size(167, 24);
            cmbAnimCast.TabIndex = 10;
            cmbAnimCast.SelectedIndexChanged += CmbAnimCast_Scroll;
            // 
            // cmbLabel22
            // 
            cmbLabel22.AutoSize = true;
            cmbLabel22.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel22.Location = new Point(7, 149);
            cmbLabel22.Margin = new Padding(4, 0, 4, 0);
            cmbLabel22.Name = "cmbLabel22";
            cmbLabel22.Size = new Size(92, 15);
            cmbLabel22.TabIndex = 9;
            cmbLabel22.Text = "Cast Animation:";
            // 
            // nudStun
            // 
            nudStun.Location = new Point(175, 110);
            nudStun.Margin = new Padding(4, 3, 4, 3);
            nudStun.Name = "nudStun";
            nudStun.Size = new Size(88, 23);
            nudStun.TabIndex = 8;
            nudStun.ValueChanged += NudStun_Scroll;
            // 
            // cmbLabel21
            // 
            cmbLabel21.AutoSize = true;
            cmbLabel21.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel21.Location = new Point(7, 112);
            cmbLabel21.Margin = new Padding(4, 0, 4, 0);
            cmbLabel21.Name = "cmbLabel21";
            cmbLabel21.Size = new Size(113, 15);
            cmbLabel21.TabIndex = 7;
            cmbLabel21.Text = "Stun Duration(secs):";
            // 
            // cmbLabel20
            // 
            cmbLabel20.AutoSize = true;
            cmbLabel20.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel20.Location = new Point(125, 82);
            cmbLabel20.Margin = new Padding(4, 0, 4, 0);
            cmbLabel20.Name = "cmbLabel20";
            cmbLabel20.Size = new Size(130, 15);
            cmbLabel20.TabIndex = 6;
            cmbLabel20.Text = "Tiles. Hint: 0 is self-cast";
            // 
            // nudAoE
            // 
            nudAoE.Location = new Point(63, 80);
            nudAoE.Margin = new Padding(4, 3, 4, 3);
            nudAoE.Name = "nudAoE";
            nudAoE.Size = new Size(55, 23);
            nudAoE.TabIndex = 5;
            nudAoE.ValueChanged += NudAoE_Scroll;
            // 
            // cmbLabel19
            // 
            cmbLabel19.AutoSize = true;
            cmbLabel19.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel19.Location = new Point(7, 82);
            cmbLabel19.Margin = new Padding(4, 0, 4, 0);
            cmbLabel19.Name = "cmbLabel19";
            cmbLabel19.Size = new Size(31, 15);
            cmbLabel19.TabIndex = 4;
            cmbLabel19.Text = "AoE:";
            // 
            // chkAoE
            // 
            chkAoE.AutoSize = true;
            chkAoE.Location = new Point(10, 53);
            chkAoE.Margin = new Padding(4, 3, 4, 3);
            chkAoE.Name = "chkAoE";
            chkAoE.Size = new Size(82, 19);
            chkAoE.TabIndex = 3;
            chkAoE.Text = "Is AoE Skill";
            chkAoE.CheckedChanged += ChkAOE_CheckedChanged;
            // 
            // cmbLabel18
            // 
            cmbLabel18.AutoSize = true;
            cmbLabel18.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel18.Location = new Point(125, 27);
            cmbLabel18.Margin = new Padding(4, 0, 4, 0);
            cmbLabel18.Name = "cmbLabel18";
            cmbLabel18.Size = new Size(130, 15);
            cmbLabel18.TabIndex = 2;
            cmbLabel18.Text = "Tiles. Hint: 0 is self-cast";
            // 
            // nudRange
            // 
            nudRange.Location = new Point(63, 23);
            nudRange.Margin = new Padding(4, 3, 4, 3);
            nudRange.Name = "nudRange";
            nudRange.Size = new Size(55, 23);
            nudRange.TabIndex = 1;
            nudRange.ValueChanged += NudRange_Scroll;
            // 
            // cmbLabel17
            // 
            cmbLabel17.AutoSize = true;
            cmbLabel17.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel17.Location = new Point(7, 27);
            cmbLabel17.Margin = new Padding(4, 0, 4, 0);
            cmbLabel17.Name = "cmbLabel17";
            cmbLabel17.Size = new Size(43, 15);
            cmbLabel17.TabIndex = 0;
            cmbLabel17.Text = "Range:";
            // 
            // DarkGroupBox7
            // 
            DarkGroupBox7.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox7.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox7.Controls.Add(nudInterval);
            DarkGroupBox7.Controls.Add(cmbLabel16);
            DarkGroupBox7.Controls.Add(nudDuration);
            DarkGroupBox7.Controls.Add(cmbLabel15);
            DarkGroupBox7.Controls.Add(nudVital);
            DarkGroupBox7.Controls.Add(cmbLabel14);
            DarkGroupBox7.ForeColor = Color.Gainsboro;
            DarkGroupBox7.Location = new Point(7, 113);
            DarkGroupBox7.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox7.Name = "DarkGroupBox7";
            DarkGroupBox7.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox7.Size = new Size(296, 89);
            DarkGroupBox7.TabIndex = 1;
            DarkGroupBox7.TabStop = false;
            DarkGroupBox7.Text = "HoT & DoT Settings ";
            // 
            // nudInterval
            // 
            nudInterval.Location = new Point(237, 52);
            nudInterval.Margin = new Padding(4, 3, 4, 3);
            nudInterval.Name = "nudInterval";
            nudInterval.Size = new Size(52, 23);
            nudInterval.TabIndex = 5;
            nudInterval.ValueChanged += NudInterval_Scroll;
            // 
            // cmbLabel16
            // 
            cmbLabel16.AutoSize = true;
            cmbLabel16.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel16.Location = new Point(177, 55);
            cmbLabel16.Margin = new Padding(4, 0, 4, 0);
            cmbLabel16.Name = "cmbLabel16";
            cmbLabel16.Size = new Size(49, 15);
            cmbLabel16.TabIndex = 4;
            cmbLabel16.Text = "Interval:";
            // 
            // nudDuration
            // 
            nudDuration.Location = new Point(105, 52);
            nudDuration.Margin = new Padding(4, 3, 4, 3);
            nudDuration.Name = "nudDuration";
            nudDuration.Size = new Size(52, 23);
            nudDuration.TabIndex = 3;
            nudDuration.ValueChanged += NudDuration_Scroll;
            // 
            // cmbLabel15
            // 
            cmbLabel15.AutoSize = true;
            cmbLabel15.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel15.Location = new Point(7, 55);
            cmbLabel15.Margin = new Padding(4, 0, 4, 0);
            cmbLabel15.Name = "cmbLabel15";
            cmbLabel15.Size = new Size(86, 15);
            cmbLabel15.TabIndex = 2;
            cmbLabel15.Text = "Duration(secs):";
            // 
            // nudVital
            // 
            nudVital.Location = new Point(170, 22);
            nudVital.Margin = new Padding(4, 3, 4, 3);
            nudVital.Name = "nudVital";
            nudVital.Size = new Size(119, 23);
            nudVital.TabIndex = 1;
            nudVital.ValueChanged += NudVital_Scroll;
            // 
            // cmbLabel14
            // 
            cmbLabel14.AutoSize = true;
            cmbLabel14.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel14.Location = new Point(7, 25);
            cmbLabel14.Margin = new Padding(4, 0, 4, 0);
            cmbLabel14.Name = "cmbLabel14";
            cmbLabel14.Size = new Size(153, 15);
            cmbLabel14.TabIndex = 0;
            cmbLabel14.Text = "Amount to heal or damage:";
            // 
            // DarkGroupBox6
            // 
            DarkGroupBox6.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox6.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox6.Controls.Add(nudY);
            DarkGroupBox6.Controls.Add(cmbLabel13);
            DarkGroupBox6.Controls.Add(nudX);
            DarkGroupBox6.Controls.Add(cmbLabel12);
            DarkGroupBox6.Controls.Add(cmbDir);
            DarkGroupBox6.Controls.Add(cmbLabel11);
            DarkGroupBox6.Controls.Add(nudMap);
            DarkGroupBox6.Controls.Add(cmbLabel10);
            DarkGroupBox6.ForeColor = Color.Gainsboro;
            DarkGroupBox6.Location = new Point(7, 16);
            DarkGroupBox6.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox6.Name = "DarkGroupBox6";
            DarkGroupBox6.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox6.Size = new Size(296, 90);
            DarkGroupBox6.TabIndex = 0;
            DarkGroupBox6.TabStop = false;
            DarkGroupBox6.Text = "Warp Settings";
            // 
            // nudY
            // 
            nudY.Location = new Point(205, 52);
            nudY.Margin = new Padding(4, 3, 4, 3);
            nudY.Name = "nudY";
            nudY.Size = new Size(80, 23);
            nudY.TabIndex = 7;
            nudY.ValueChanged += NudY_Scroll;
            // 
            // cmbLabel13
            // 
            cmbLabel13.AutoSize = true;
            cmbLabel13.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel13.Location = new Point(138, 55);
            cmbLabel13.Margin = new Padding(4, 0, 4, 0);
            cmbLabel13.Name = "cmbLabel13";
            cmbLabel13.Size = new Size(17, 15);
            cmbLabel13.TabIndex = 6;
            cmbLabel13.Text = "Y:";
            // 
            // nudX
            // 
            nudX.Location = new Point(50, 52);
            nudX.Margin = new Padding(4, 3, 4, 3);
            nudX.Name = "nudX";
            nudX.Size = new Size(80, 23);
            nudX.TabIndex = 5;
            nudX.ValueChanged += NudX_Scroll;
            // 
            // cmbLabel12
            // 
            cmbLabel12.AutoSize = true;
            cmbLabel12.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel12.Location = new Point(7, 55);
            cmbLabel12.Margin = new Padding(4, 0, 4, 0);
            cmbLabel12.Name = "cmbLabel12";
            cmbLabel12.Size = new Size(17, 15);
            cmbLabel12.TabIndex = 4;
            cmbLabel12.Text = "X:";
            // 
            // cmbDir
            // 
            cmbDir.DrawMode = DrawMode.OwnerDrawFixed;
            cmbDir.FormattingEnabled = true;
            cmbDir.Items.AddRange(new object[] { "Up", "Down", "Left", "Right" });
            cmbDir.Location = new Point(205, 20);
            cmbDir.Margin = new Padding(4, 3, 4, 3);
            cmbDir.Name = "cmbDir";
            cmbDir.Size = new Size(80, 24);
            cmbDir.TabIndex = 3;
            cmbDir.SelectedIndexChanged += CmbDir_SelectedIndexChanged;
            // 
            // cmbLabel11
            // 
            cmbLabel11.AutoSize = true;
            cmbLabel11.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel11.Location = new Point(138, 25);
            cmbLabel11.Margin = new Padding(4, 0, 4, 0);
            cmbLabel11.Name = "cmbLabel11";
            cmbLabel11.Size = new Size(58, 15);
            cmbLabel11.TabIndex = 2;
            cmbLabel11.Text = "Direction:";
            // 
            // nudMap
            // 
            nudMap.Location = new Point(50, 22);
            nudMap.Margin = new Padding(4, 3, 4, 3);
            nudMap.Name = "nudMap";
            nudMap.Size = new Size(80, 23);
            nudMap.TabIndex = 1;
            nudMap.ValueChanged += NudMap_Scroll;
            // 
            // cmbLabel10
            // 
            cmbLabel10.AutoSize = true;
            cmbLabel10.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel10.Location = new Point(7, 25);
            cmbLabel10.Margin = new Padding(4, 0, 4, 0);
            cmbLabel10.Name = "cmbLabel10";
            cmbLabel10.Size = new Size(34, 15);
            cmbLabel10.TabIndex = 0;
            cmbLabel10.Text = "Map:";
            // 
            // DarkGroupBox3
            // 
            DarkGroupBox3.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox3.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox3.Controls.Add(chkKnockBack);
            DarkGroupBox3.Controls.Add(cmbKnockBackTiles);
            DarkGroupBox3.Controls.Add(cmbProjectile);
            DarkGroupBox3.Controls.Add(chkProjectile);
            DarkGroupBox3.Controls.Add(nudIcon);
            DarkGroupBox3.Controls.Add(cmbLabel9);
            DarkGroupBox3.Controls.Add(picSprite);
            DarkGroupBox3.Controls.Add(nudCool);
            DarkGroupBox3.Controls.Add(cmbLabel8);
            DarkGroupBox3.Controls.Add(nudCast);
            DarkGroupBox3.Controls.Add(cmbLabel7);
            DarkGroupBox3.Controls.Add(DarkGroupBox4);
            DarkGroupBox3.Controls.Add(nudMp);
            DarkGroupBox3.Controls.Add(cmbLabel3);
            DarkGroupBox3.Controls.Add(cmbType);
            DarkGroupBox3.Controls.Add(cmbLabel2);
            DarkGroupBox3.Controls.Add(txtName);
            DarkGroupBox3.Controls.Add(cmbLabel1);
            DarkGroupBox3.ForeColor = Color.Gainsboro;
            DarkGroupBox3.Location = new Point(7, 22);
            DarkGroupBox3.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox3.Name = "DarkGroupBox3";
            DarkGroupBox3.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox3.Size = new Size(385, 373);
            DarkGroupBox3.TabIndex = 0;
            DarkGroupBox3.TabStop = false;
            DarkGroupBox3.Text = "Basic Settings";
            // 
            // chkKnockBack
            // 
            chkKnockBack.AutoSize = true;
            chkKnockBack.Location = new Point(10, 182);
            chkKnockBack.Margin = new Padding(4, 3, 4, 3);
            chkKnockBack.Name = "chkKnockBack";
            chkKnockBack.Size = new Size(120, 19);
            chkKnockBack.TabIndex = 61;
            chkKnockBack.Text = "Has knockback of";
            chkKnockBack.CheckedChanged += ChkKnockBack_CheckedChanged;
            // 
            // cmbKnockBackTiles
            // 
            cmbKnockBackTiles.DrawMode = DrawMode.OwnerDrawFixed;
            cmbKnockBackTiles.FormattingEnabled = true;
            cmbKnockBackTiles.Items.AddRange(new object[] { "No KnockBack", "1 Tile", "2 Tiles", "3 Tiles", "4 Tiles", "5 Tiles" });
            cmbKnockBackTiles.Location = new Point(178, 180);
            cmbKnockBackTiles.Margin = new Padding(4, 3, 4, 3);
            cmbKnockBackTiles.Name = "cmbKnockBackTiles";
            cmbKnockBackTiles.Size = new Size(199, 24);
            cmbKnockBackTiles.TabIndex = 60;
            cmbKnockBackTiles.SelectedIndexChanged += CmbKnockBackTiles_SelectedIndexChanged;
            // 
            // cmbProjectile
            // 
            cmbProjectile.DrawMode = DrawMode.OwnerDrawFixed;
            cmbProjectile.FormattingEnabled = true;
            cmbProjectile.Location = new Point(178, 149);
            cmbProjectile.Margin = new Padding(4, 3, 4, 3);
            cmbProjectile.Name = "cmbProjectile";
            cmbProjectile.Size = new Size(199, 24);
            cmbProjectile.TabIndex = 59;
            cmbProjectile.SelectedIndexChanged += ScrlProjectile_Scroll;
            // 
            // chkProjectile
            // 
            chkProjectile.AutoSize = true;
            chkProjectile.Location = new Point(10, 151);
            chkProjectile.Margin = new Padding(4, 3, 4, 3);
            chkProjectile.Name = "chkProjectile";
            chkProjectile.Size = new Size(103, 19);
            chkProjectile.TabIndex = 58;
            chkProjectile.Text = "Has Projectile?";
            chkProjectile.CheckedChanged += ChkProjectile_CheckedChanged;
            // 
            // nudIcon
            // 
            nudIcon.Location = new Point(84, 113);
            nudIcon.Margin = new Padding(4, 3, 4, 3);
            nudIcon.Name = "nudIcon";
            nudIcon.Size = new Size(93, 23);
            nudIcon.TabIndex = 57;
            nudIcon.Click += nudIcon_Click;
            // 
            // cmbLabel9
            // 
            cmbLabel9.AutoSize = true;
            cmbLabel9.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel9.Location = new Point(7, 110);
            cmbLabel9.Margin = new Padding(4, 0, 4, 0);
            cmbLabel9.Name = "cmbLabel9";
            cmbLabel9.Size = new Size(33, 15);
            cmbLabel9.TabIndex = 56;
            cmbLabel9.Text = "Icon:";
            // 
            // picSprite
            // 
            picSprite.BackColor = Color.Black;
            picSprite.Location = new Point(185, 113);
            picSprite.Margin = new Padding(4, 3, 4, 3);
            picSprite.Name = "picSprite";
            picSprite.Size = new Size(34, 29);
            picSprite.TabIndex = 55;
            picSprite.TabStop = false;
            // 
            // nudCool
            // 
            nudCool.Location = new Point(287, 83);
            nudCool.Margin = new Padding(4, 3, 4, 3);
            nudCool.Name = "nudCool";
            nudCool.Size = new Size(90, 23);
            nudCool.TabIndex = 12;
            nudCool.ValueChanged += NudCool_Scroll;
            // 
            // cmbLabel8
            // 
            cmbLabel8.AutoSize = true;
            cmbLabel8.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel8.Location = new Point(185, 87);
            cmbLabel8.Margin = new Padding(4, 0, 4, 0);
            cmbLabel8.Name = "cmbLabel8";
            cmbLabel8.Size = new Size(95, 15);
            cmbLabel8.TabIndex = 11;
            cmbLabel8.Text = "Cooldown Time:";
            // 
            // nudCast
            // 
            nudCast.Location = new Point(83, 83);
            nudCast.Margin = new Padding(4, 3, 4, 3);
            nudCast.Name = "nudCast";
            nudCast.Size = new Size(93, 23);
            nudCast.TabIndex = 10;
            nudCast.ValueChanged += NudCast_Scroll;
            // 
            // cmbLabel7
            // 
            cmbLabel7.AutoSize = true;
            cmbLabel7.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel7.Location = new Point(7, 80);
            cmbLabel7.Margin = new Padding(4, 0, 4, 0);
            cmbLabel7.Name = "cmbLabel7";
            cmbLabel7.Size = new Size(63, 15);
            cmbLabel7.TabIndex = 9;
            cmbLabel7.Text = "Cast Time:";
            // 
            // DarkGroupBox4
            // 
            DarkGroupBox4.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox4.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox4.Controls.Add(cmbLabel6);
            DarkGroupBox4.Controls.Add(cmbJob);
            DarkGroupBox4.Controls.Add(cmbAccessReq);
            DarkGroupBox4.Controls.Add(cmbLabel5);
            DarkGroupBox4.Controls.Add(nudLevel);
            DarkGroupBox4.Controls.Add(cmbLabel4);
            DarkGroupBox4.ForeColor = Color.Gainsboro;
            DarkGroupBox4.Location = new Point(7, 262);
            DarkGroupBox4.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox4.Name = "DarkGroupBox4";
            DarkGroupBox4.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox4.Size = new Size(371, 100);
            DarkGroupBox4.TabIndex = 8;
            DarkGroupBox4.TabStop = false;
            DarkGroupBox4.Text = "Requirements";
            // 
            // cmbLabel6
            // 
            cmbLabel6.AutoSize = true;
            cmbLabel6.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel6.Location = new Point(7, 55);
            cmbLabel6.Margin = new Padding(4, 0, 4, 0);
            cmbLabel6.Name = "cmbLabel6";
            cmbLabel6.Size = new Size(78, 15);
            cmbLabel6.TabIndex = 11;
            cmbLabel6.Text = "Job Required:";
            // 
            // cmbJob
            // 
            cmbJob.DrawMode = DrawMode.OwnerDrawFixed;
            cmbJob.FormattingEnabled = true;
            cmbJob.Location = new Point(110, 52);
            cmbJob.Margin = new Padding(4, 3, 4, 3);
            cmbJob.Name = "cmbJob";
            cmbJob.Size = new Size(254, 24);
            cmbJob.TabIndex = 10;
            cmbJob.SelectedIndexChanged += CmbClass_SelectedIndexChanged;
            // 
            // cmbAccessReq
            // 
            cmbAccessReq.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAccessReq.FormattingEnabled = true;
            cmbAccessReq.Items.AddRange(new object[] { "Player", "Moderator", "Mapper", "Developer", "Owner" });
            cmbAccessReq.Location = new Point(281, 20);
            cmbAccessReq.Margin = new Padding(4, 3, 4, 3);
            cmbAccessReq.Name = "cmbAccessReq";
            cmbAccessReq.Size = new Size(82, 24);
            cmbAccessReq.TabIndex = 9;
            cmbAccessReq.SelectedIndexChanged += CmbAccessReq_SelectedIndexChanged;
            // 
            // cmbLabel5
            // 
            cmbLabel5.AutoSize = true;
            cmbLabel5.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel5.Location = new Point(168, 25);
            cmbLabel5.Margin = new Padding(4, 0, 4, 0);
            cmbLabel5.Name = "cmbLabel5";
            cmbLabel5.Size = new Size(96, 15);
            cmbLabel5.TabIndex = 8;
            cmbLabel5.Text = "Access Required:";
            // 
            // nudLevel
            // 
            nudLevel.Location = new Point(110, 22);
            nudLevel.Margin = new Padding(4, 3, 4, 3);
            nudLevel.Name = "nudLevel";
            nudLevel.Size = new Size(51, 23);
            nudLevel.TabIndex = 7;
            nudLevel.ValueChanged += NudLevel_ValueChanged;
            // 
            // cmbLabel4
            // 
            cmbLabel4.AutoSize = true;
            cmbLabel4.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel4.Location = new Point(7, 25);
            cmbLabel4.Margin = new Padding(4, 0, 4, 0);
            cmbLabel4.Name = "cmbLabel4";
            cmbLabel4.Size = new Size(87, 15);
            cmbLabel4.TabIndex = 6;
            cmbLabel4.Text = "Level Required:";
            // 
            // nudMp
            // 
            nudMp.Location = new Point(286, 55);
            nudMp.Margin = new Padding(4, 3, 4, 3);
            nudMp.Name = "nudMp";
            nudMp.Size = new Size(90, 23);
            nudMp.TabIndex = 5;
            nudMp.ValueChanged += NudMp_ValueChanged;
            // 
            // cmbLabel3
            // 
            cmbLabel3.AutoSize = true;
            cmbLabel3.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel3.Location = new Point(223, 56);
            cmbLabel3.Margin = new Padding(4, 0, 4, 0);
            cmbLabel3.Name = "cmbLabel3";
            cmbLabel3.Size = new Size(55, 15);
            cmbLabel3.TabIndex = 4;
            cmbLabel3.Text = "MP Cost:";
            // 
            // cmbType
            // 
            cmbType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbType.FormattingEnabled = true;
            cmbType.Items.AddRange(new object[] { "Damage HP", "Damage MP", "Heal HP", "Heal MP", "Warp" });
            cmbType.Location = new Point(84, 53);
            cmbType.Margin = new Padding(4, 3, 4, 3);
            cmbType.Name = "cmbType";
            cmbType.Size = new Size(104, 24);
            cmbType.TabIndex = 3;
            cmbType.SelectedIndexChanged += CmbType_SelectedIndexChanged;
            // 
            // cmbLabel2
            // 
            cmbLabel2.AutoSize = true;
            cmbLabel2.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel2.Location = new Point(7, 50);
            cmbLabel2.Margin = new Padding(4, 0, 4, 0);
            cmbLabel2.Name = "cmbLabel2";
            cmbLabel2.Size = new Size(35, 15);
            cmbLabel2.TabIndex = 2;
            cmbLabel2.Text = "Type:";
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.FromArgb(220, 220, 220);
            txtName.Location = new Point(84, 22);
            txtName.Margin = new Padding(4, 3, 4, 3);
            txtName.Name = "txtName";
            txtName.Size = new Size(294, 23);
            txtName.TabIndex = 1;
            txtName.TextChanged += TxtName_TextChanged;
            // 
            // cmbLabel1
            // 
            cmbLabel1.AutoSize = true;
            cmbLabel1.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel1.Location = new Point(8, 25);
            cmbLabel1.Margin = new Padding(4, 0, 4, 0);
            cmbLabel1.Name = "cmbLabel1";
            cmbLabel1.Size = new Size(42, 15);
            cmbLabel1.TabIndex = 0;
            cmbLabel1.Text = "Name:";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(11, 398);
            btnDelete.Margin = new Padding(4, 3, 4, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(6, 5, 6, 5);
            btnDelete.Size = new Size(199, 27);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Delete";
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(11, 431);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(6, 5, 6, 5);
            btnCancel.Size = new Size(199, 27);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(11, 365);
            btnSave.Margin = new Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(6, 5, 6, 5);
            btnSave.Size = new Size(199, 27);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            btnSave.Click += BtnSave_Click;
            // 
            // Editor_Skill
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(952, 463);
            Controls.Add(btnCancel);
            Controls.Add(btnDelete);
            Controls.Add(DarkGroupBox2);
            Controls.Add(DarkGroupBox1);
            Controls.Add(btnSave);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(4, 3, 4, 3);
            Name = "Editor_Skill";
            Text = "Skill Editor";
            FormClosing += Editor_Skill_FormClosing;
            Load += Editor_Skill_Load;
            DarkGroupBox1.ResumeLayout(false);
            DarkGroupBox2.ResumeLayout(false);
            DarkGroupBox5.ResumeLayout(false);
            DarkGroupBox8.ResumeLayout(false);
            DarkGroupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudStun).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAoE).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRange).EndInit();
            DarkGroupBox7.ResumeLayout(false);
            DarkGroupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudInterval).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDuration).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVital).EndInit();
            DarkGroupBox6.ResumeLayout(false);
            DarkGroupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMap).EndInit();
            DarkGroupBox3.ResumeLayout(false);
            DarkGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSprite).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCool).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCast).EndInit();
            DarkGroupBox4.ResumeLayout(false);
            DarkGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMp).EndInit();
            ResumeLayout(false);
        }

        internal DarkGroupBox DarkGroupBox1;
        internal ListBox lstIndex;
        internal DarkGroupBox DarkGroupBox2;
        internal DarkGroupBox DarkGroupBox3;
        internal DarkTextBox txtName;
        internal cmbLabel cmbLabel1;
        internal DarkComboBox cmbType;
        internal cmbLabel cmbLabel2;
        internal DarkNumericUpDown nudMp;
        internal cmbLabel cmbLabel3;
        internal DarkNumericUpDown nudLevel;
        internal cmbLabel cmbLabel4;
        internal DarkGroupBox DarkGroupBox4;
        internal cmbLabel cmbLabel5;
        internal DarkComboBox cmbAccessReq;
        internal cmbLabel cmbLabel6;
        internal DarkComboBox cmbJob;
        internal DarkNumericUpDown nudCast;
        internal cmbLabel cmbLabel7;
        internal DarkNumericUpDown nudCool;
        internal cmbLabel cmbLabel8;
        internal cmbLabel cmbLabel9;
        internal PictureBox picSprite;
        internal DarkNumericUpDown nudIcon;
        internal DarkCheckBox chkProjectile;
        internal DarkComboBox cmbProjectile;
        internal DarkCheckBox chkKnockBack;
        internal DarkComboBox cmbKnockBackTiles;
        internal DarkGroupBox DarkGroupBox5;
        internal DarkGroupBox DarkGroupBox6;
        internal DarkNumericUpDown nudMap;
        internal cmbLabel cmbLabel10;
        internal cmbLabel cmbLabel11;
        internal DarkComboBox cmbDir;
        internal DarkNumericUpDown nudX;
        internal cmbLabel cmbLabel12;
        internal DarkNumericUpDown nudY;
        internal cmbLabel cmbLabel13;
        internal DarkGroupBox DarkGroupBox7;
        internal DarkNumericUpDown nudVital;
        internal cmbLabel cmbLabel14;
        internal DarkNumericUpDown nudDuration;
        internal cmbLabel cmbLabel15;
        internal DarkNumericUpDown nudInterval;
        internal cmbLabel cmbLabel16;
        internal DarkGroupBox DarkGroupBox8;
        internal cmbLabel cmbLabel18;
        internal DarkNumericUpDown nudRange;
        internal cmbLabel cmbLabel17;
        internal DarkCheckBox chkAoE;
        internal cmbLabel cmbLabel20;
        internal DarkNumericUpDown nudAoE;
        internal cmbLabel cmbLabel19;
        internal DarkNumericUpDown nudStun;
        internal cmbLabel cmbLabel21;
        internal cmbLabel cmbLabel22;
        internal DarkComboBox cmbAnimCast;
        internal DarkComboBox cmbAnim;
        internal cmbLabel cmbLabel23;
        internal DarkButton btnDelete;
        internal DarkButton btnCancel;
        internal DarkButton btnSave;
        internal DarkButton btnLearn;
    }
}