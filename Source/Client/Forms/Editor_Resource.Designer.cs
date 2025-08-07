using DarkUI.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public partial class Editor_Resource : Form
    {

        // Shared instance of the form
        private static Editor_Resource _instance;

        // Public property to get the shared instance
        public static Editor_Resource Instance
        {
            get
            {
                // Create a new instance if one does not exist or if it has been disposed
                if (_instance is null || _instance.IsDisposed)
                {
                    _instance = new Editor_Resource();
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
            DarkGroupBox5 = new DarkUI.Controls.DarkGroupBox();
            nudRewardExp = new DarkUI.Controls.DarkNumericUpDown();
            nudRewardExp.ValueChanged += new EventHandler(ScrlRewardExp_Scroll);
            cmbLabel13 = new DarkUI.Controls.cmbLabel();
            cmbRewardItem = new DarkUI.Controls.DarkComboBox();
            cmbRewardItem.SelectedIndexChanged += new EventHandler(ScrlRewardItem_Scroll);
            cmbLabel12 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox4 = new DarkUI.Controls.DarkGroupBox();
            nudLvlReq = new DarkUI.Controls.DarkNumericUpDown();
            nudLvlReq.ValueChanged += new EventHandler(ScrlLvlReq_Scroll);
            cmbLabel11 = new DarkUI.Controls.cmbLabel();
            cmbTool = new DarkUI.Controls.DarkComboBox();
            cmbTool.SelectedIndexChanged += new EventHandler(CmbTool_SelectedIndexChanged);
            cmbLabel10 = new DarkUI.Controls.cmbLabel();
            cmbAnimation = new DarkUI.Controls.DarkComboBox();
            cmbAnimation.SelectedIndexChanged += new EventHandler(ScrlAnim_Scroll);
            cmbLabel9 = new DarkUI.Controls.cmbLabel();
            nudRespawn = new DarkUI.Controls.DarkNumericUpDown();
            nudRespawn.ValueChanged += new EventHandler(ScrlRespawn_Scroll);
            cmbLabel8 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox3 = new DarkUI.Controls.DarkGroupBox();
            nudExhaustedPic = new DarkUI.Controls.DarkNumericUpDown();
            nudExhaustedPic.ValueChanged += new EventHandler(ScrlExhaustedPic_Scroll);
            nudNormalPic = new DarkUI.Controls.DarkNumericUpDown();
            nudNormalPic.ValueChanged += new EventHandler(ScrlNormalPic_Scroll);
            cmbLabel7 = new DarkUI.Controls.cmbLabel();
            cmbLabel6 = new DarkUI.Controls.cmbLabel();
            picExhaustedPic = new PictureBox();
            picNormalpic = new PictureBox();
            nudHealth = new DarkUI.Controls.DarkNumericUpDown();
            nudHealth.ValueChanged += new EventHandler(ScrlHealth_Scroll);
            cmbLabel5 = new DarkUI.Controls.cmbLabel();
            cmbType = new DarkUI.Controls.DarkComboBox();
            cmbType.SelectedIndexChanged += new EventHandler(CmbType_SelectedIndexChanged);
            cmbLabel4 = new DarkUI.Controls.cmbLabel();
            cmbLabel3 = new DarkUI.Controls.cmbLabel();
            txtMessage2 = new DarkUI.Controls.DarkTextBox();
            txtMessage2.TextChanged += new EventHandler(TxtMessage2_TextChanged);
            txtMessage = new DarkUI.Controls.DarkTextBox();
            txtMessage.TextChanged += new EventHandler(TxtMessage_TextChanged);
            cmbLabel2 = new DarkUI.Controls.cmbLabel();
            txtName = new DarkUI.Controls.DarkTextBox();
            txtName.TextChanged += new EventHandler(TxtName_TextChanged);
            cmbLabel1 = new DarkUI.Controls.cmbLabel();
            btnSave = new DarkUI.Controls.DarkButton();
            btnSave.Click += new EventHandler(BtnSave_Click);
            btnDelete = new DarkUI.Controls.DarkButton();
            btnDelete.Click += new EventHandler(BtnDelete_Click);
            btnCancel = new DarkUI.Controls.DarkButton();
            btnCancel.Click += new EventHandler(BtnCancel_Click);
            DarkGroupBox1.SuspendLayout();
            DarkGroupBox2.SuspendLayout();
            DarkGroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudRewardExp).BeginInit();
            DarkGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudLvlReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRespawn).BeginInit();
            DarkGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudExhaustedPic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudNormalPic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picExhaustedPic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picNormalpic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudHealth).BeginInit();
            SuspendLayout();
            // 
            // DarkGroupBox1
            // 
            DarkGroupBox1.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox1.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox1.Controls.Add(lstIndex);
            DarkGroupBox1.ForeColor = Color.Gainsboro;
            DarkGroupBox1.Location = new Point(8, 4);
            DarkGroupBox1.Margin = new Padding(8, 6, 8, 6);
            DarkGroupBox1.Name = "DarkGroupBox1";
            DarkGroupBox1.Padding = new Padding(8, 6, 8, 6);
            DarkGroupBox1.Size = new Size(468, 815);
            DarkGroupBox1.TabIndex = 0;
            DarkGroupBox1.TabStop = false;
            DarkGroupBox1.Text = "Resource List";
            // 
            // lstIndex
            // 
            lstIndex.BackColor = Color.FromArgb(45, 45, 48);
            lstIndex.BorderStyle = BorderStyle.FixedSingle;
            lstIndex.ForeColor = Color.Gainsboro;
            lstIndex.FormattingEnabled = true;
            lstIndex.Location = new Point(18, 47);
            lstIndex.Margin = new Padding(8, 6, 8, 6);
            lstIndex.Name = "lstIndex";
            lstIndex.Size = new Size(422, 738);
            lstIndex.TabIndex = 1;
            // 
            // DarkGroupBox2
            // 
            DarkGroupBox2.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox2.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox2.Controls.Add(DarkGroupBox5);
            DarkGroupBox2.Controls.Add(DarkGroupBox4);
            DarkGroupBox2.Controls.Add(cmbAnimation);
            DarkGroupBox2.Controls.Add(cmbLabel9);
            DarkGroupBox2.Controls.Add(nudRespawn);
            DarkGroupBox2.Controls.Add(cmbLabel8);
            DarkGroupBox2.Controls.Add(DarkGroupBox3);
            DarkGroupBox2.Controls.Add(nudHealth);
            DarkGroupBox2.Controls.Add(cmbLabel5);
            DarkGroupBox2.Controls.Add(cmbType);
            DarkGroupBox2.Controls.Add(cmbLabel4);
            DarkGroupBox2.Controls.Add(cmbLabel3);
            DarkGroupBox2.Controls.Add(txtMessage2);
            DarkGroupBox2.Controls.Add(txtMessage);
            DarkGroupBox2.Controls.Add(cmbLabel2);
            DarkGroupBox2.Controls.Add(txtName);
            DarkGroupBox2.Controls.Add(cmbLabel1);
            DarkGroupBox2.ForeColor = Color.Gainsboro;
            DarkGroupBox2.Location = new Point(486, 4);
            DarkGroupBox2.Margin = new Padding(8, 6, 8, 6);
            DarkGroupBox2.Name = "DarkGroupBox2";
            DarkGroupBox2.Padding = new Padding(8, 6, 8, 6);
            DarkGroupBox2.Size = new Size(791, 1030);
            DarkGroupBox2.TabIndex = 1;
            DarkGroupBox2.TabStop = false;
            DarkGroupBox2.Text = "Properties";
            // 
            // DarkGroupBox5
            // 
            DarkGroupBox5.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox5.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox5.Controls.Add(nudRewardExp);
            DarkGroupBox5.Controls.Add(cmbLabel13);
            DarkGroupBox5.Controls.Add(cmbRewardItem);
            DarkGroupBox5.Controls.Add(cmbLabel12);
            DarkGroupBox5.ForeColor = Color.Gainsboro;
            DarkGroupBox5.Location = new Point(18, 908);
            DarkGroupBox5.Margin = new Padding(8, 6, 8, 6);
            DarkGroupBox5.Name = "DarkGroupBox5";
            DarkGroupBox5.Padding = new Padding(8, 6, 8, 6);
            DarkGroupBox5.Size = new Size(750, 108);
            DarkGroupBox5.TabIndex = 16;
            DarkGroupBox5.TabStop = false;
            DarkGroupBox5.Text = "Rewards";
            // 
            // nudRewardExp
            // 
            nudRewardExp.Location = new Point(522, 34);
            nudRewardExp.Margin = new Padding(8, 6, 8, 6);
            nudRewardExp.Name = "nudRewardExp";
            nudRewardExp.Size = new Size(212, 39);
            nudRewardExp.TabIndex = 3;
            // 
            // cmbLabel13
            // 
            cmbLabel13.AutoSize = true;
            cmbLabel13.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel13.Location = new Point(373, 38);
            cmbLabel13.Margin = new Padding(8, 0, 8, 0);
            cmbLabel13.Name = "cmbLabel13";
            cmbLabel13.Size = new Size(134, 32);
            cmbLabel13.TabIndex = 2;
            cmbLabel13.Text = "Exp:";
            // 
            // cmbRewardItem
            // 
            cmbRewardItem.DrawMode = DrawMode.OwnerDrawFixed;
            cmbRewardItem.FormattingEnabled = true;
            cmbRewardItem.Location = new Point(91, 32);
            cmbRewardItem.Margin = new Padding(8, 6, 8, 6);
            cmbRewardItem.Name = "cmbRewardItem";
            cmbRewardItem.Size = new Size(256, 40);
            cmbRewardItem.TabIndex = 1;
            // 
            // cmbLabel12
            // 
            cmbLabel12.AutoSize = true;
            cmbLabel12.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel12.Location = new Point(13, 38);
            cmbLabel12.Margin = new Padding(8, 0, 8, 0);
            cmbLabel12.Name = "cmbLabel12";
            cmbLabel12.Size = new Size(67, 32);
            cmbLabel12.TabIndex = 0;
            cmbLabel12.Text = "Item:";
            // 
            // DarkGroupBox4
            // 
            DarkGroupBox4.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox4.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox4.Controls.Add(nudLvlReq);
            DarkGroupBox4.Controls.Add(cmbLabel11);
            DarkGroupBox4.Controls.Add(cmbTool);
            DarkGroupBox4.Controls.Add(cmbLabel10);
            DarkGroupBox4.ForeColor = Color.Gainsboro;
            DarkGroupBox4.Location = new Point(18, 785);
            DarkGroupBox4.Margin = new Padding(8, 6, 8, 6);
            DarkGroupBox4.Name = "DarkGroupBox4";
            DarkGroupBox4.Padding = new Padding(8, 6, 8, 6);
            DarkGroupBox4.Size = new Size(750, 108);
            DarkGroupBox4.TabIndex = 15;
            DarkGroupBox4.TabStop = false;
            DarkGroupBox4.Text = "Requirements";
            // 
            // nudLvlReq
            // 
            nudLvlReq.Location = new Point(557, 34);
            nudLvlReq.Margin = new Padding(8, 6, 8, 6);
            nudLvlReq.Name = "nudLvlReq";
            nudLvlReq.Size = new Size(178, 39);
            nudLvlReq.TabIndex = 3;
            // 
            // cmbLabel11
            // 
            cmbLabel11.AutoSize = true;
            cmbLabel11.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel11.Location = new Point(418, 38);
            cmbLabel11.Margin = new Padding(8, 0, 8, 0);
            cmbLabel11.Name = "cmbLabel11";
            cmbLabel11.Size = new Size(124, 32);
            cmbLabel11.TabIndex = 2;
            cmbLabel11.Text = "Skill Level:";
            // 
            // cmbTool
            // 
            cmbTool.DrawMode = DrawMode.OwnerDrawFixed;
            cmbTool.FormattingEnabled = true;
            cmbTool.Items.AddRange(new object[] { "None", "Hatchet", "Pickaxe", "Fishing Rod" });
            cmbTool.Location = new Point(182, 32);
            cmbTool.Margin = new Padding(8, 6, 8, 6);
            cmbTool.Name = "cmbTool";
            cmbTool.Size = new Size(217, 40);
            cmbTool.TabIndex = 1;
            // 
            // cmbLabel10
            // 
            cmbLabel10.AutoSize = true;
            cmbLabel10.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel10.Location = new Point(13, 38);
            cmbLabel10.Margin = new Padding(8, 0, 8, 0);
            cmbLabel10.Name = "cmbLabel10";
            cmbLabel10.Size = new Size(156, 32);
            cmbLabel10.TabIndex = 0;
            cmbLabel10.Text = "Tool Needed:";
            // 
            // cmbAnimation
            // 
            cmbAnimation.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAnimation.FormattingEnabled = true;
            cmbAnimation.Location = new Point(554, 300);
            cmbAnimation.Margin = new Padding(8, 6, 8, 6);
            cmbAnimation.Name = "cmbAnimation";
            cmbAnimation.Size = new Size(212, 40);
            cmbAnimation.TabIndex = 14;
            // 
            // cmbLabel9
            // 
            cmbLabel9.AutoSize = true;
            cmbLabel9.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel9.Location = new Point(418, 305);
            cmbLabel9.Margin = new Padding(8, 0, 8, 0);
            cmbLabel9.Name = "cmbLabel9";
            cmbLabel9.Size = new Size(129, 32);
            cmbLabel9.TabIndex = 13;
            cmbLabel9.Text = "Animation:";
            // 
            // nudRespawn
            // 
            nudRespawn.Location = new Point(238, 300);
            nudRespawn.Margin = new Padding(8, 6, 8, 6);
            nudRespawn.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudRespawn.Name = "nudRespawn";
            nudRespawn.Size = new Size(167, 39);
            nudRespawn.TabIndex = 12;
            // 
            // cmbLabel8
            // 
            cmbLabel8.AutoSize = true;
            cmbLabel8.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel8.Location = new Point(13, 305);
            cmbLabel8.Margin = new Padding(8, 0, 8, 0);
            cmbLabel8.Name = "cmbLabel8";
            cmbLabel8.Size = new Size(172, 32);
            cmbLabel8.TabIndex = 11;
            cmbLabel8.Text = "Respawn Time:";
            // 
            // DarkGroupBox3
            // 
            DarkGroupBox3.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox3.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox3.Controls.Add(nudExhaustedPic);
            DarkGroupBox3.Controls.Add(nudNormalPic);
            DarkGroupBox3.Controls.Add(cmbLabel7);
            DarkGroupBox3.Controls.Add(cmbLabel6);
            DarkGroupBox3.Controls.Add(picExhaustedPic);
            DarkGroupBox3.Controls.Add(picNormalpic);
            DarkGroupBox3.ForeColor = Color.Gainsboro;
            DarkGroupBox3.Location = new Point(18, 367);
            DarkGroupBox3.Margin = new Padding(8, 6, 8, 6);
            DarkGroupBox3.Name = "DarkGroupBox3";
            DarkGroupBox3.Padding = new Padding(8, 6, 8, 6);
            DarkGroupBox3.Size = new Size(750, 404);
            DarkGroupBox3.TabIndex = 10;
            DarkGroupBox3.TabStop = false;
            DarkGroupBox3.Text = "Graphics";
            // 
            // nudExhaustedPic
            // 
            nudExhaustedPic.Location = new Point(580, 337);
            nudExhaustedPic.Margin = new Padding(8, 6, 8, 6);
            nudExhaustedPic.Name = "nudExhaustedPic";
            nudExhaustedPic.Size = new Size(156, 39);
            nudExhaustedPic.TabIndex = 49;
            // 
            // nudNormalPic
            // 
            nudNormalPic.Location = new Point(186, 337);
            nudNormalPic.Margin = new Padding(8, 6, 8, 6);
            nudNormalPic.Name = "nudNormalPic";
            nudNormalPic.Size = new Size(156, 39);
            nudNormalPic.TabIndex = 48;
            // 
            // cmbLabel7
            // 
            cmbLabel7.AutoSize = true;
            cmbLabel7.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel7.Location = new Point(401, 341);
            cmbLabel7.Margin = new Padding(8, 0, 8, 0);
            cmbLabel7.Name = "cmbLabel7";
            cmbLabel7.Size = new Size(159, 32);
            cmbLabel7.TabIndex = 47;
            cmbLabel7.Text = "Empty Image:";
            // 
            // cmbLabel6
            // 
            cmbLabel6.AutoSize = true;
            cmbLabel6.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel6.Location = new Point(8, 341);
            cmbLabel6.Margin = new Padding(8, 0, 8, 0);
            cmbLabel6.Name = "cmbLabel6";
            cmbLabel6.Size = new Size(171, 32);
            cmbLabel6.TabIndex = 46;
            cmbLabel6.Text = "Normal Image:";
            // 
            // picExhaustedPic
            // 
            picExhaustedPic.BackColor = Color.Black;
            picExhaustedPic.BackgroundImageLayout = ImageLayout.Zoom;
            picExhaustedPic.Location = new Point(407, 47);
            picExhaustedPic.Margin = new Padding(8, 6, 8, 6);
            picExhaustedPic.Name = "picExhaustedPic";
            picExhaustedPic.Size = new Size(329, 276);
            picExhaustedPic.TabIndex = 45;
            picExhaustedPic.TabStop = false;
            // 
            // picNormalpic
            // 
            picNormalpic.BackColor = Color.Black;
            picNormalpic.BackgroundImageLayout = ImageLayout.Zoom;
            picNormalpic.Location = new Point(13, 47);
            picNormalpic.Margin = new Padding(8, 6, 8, 6);
            picNormalpic.Name = "picNormalpic";
            picNormalpic.Size = new Size(329, 276);
            picNormalpic.TabIndex = 44;
            picNormalpic.TabStop = false;
            // 
            // nudHealth
            // 
            nudHealth.Location = new Point(637, 236);
            nudHealth.Margin = new Padding(8, 6, 8, 6);
            nudHealth.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudHealth.Name = "nudHealth";
            nudHealth.Size = new Size(132, 39);
            nudHealth.TabIndex = 9;
            // 
            // cmbLabel5
            // 
            cmbLabel5.AutoSize = true;
            cmbLabel5.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel5.Location = new Point(511, 241);
            cmbLabel5.Margin = new Padding(8, 0, 8, 0);
            cmbLabel5.Name = "cmbLabel5";
            cmbLabel5.Size = new Size(114, 32);
            cmbLabel5.TabIndex = 8;
            cmbLabel5.Text = "HitPoints:";
            // 
            // cmbType
            // 
            cmbType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbType.FormattingEnabled = true;
            cmbType.Items.AddRange(new object[] { "None", "Herb", "Tree", "Mine", "Fishing Spot" });
            cmbType.Location = new Point(236, 235);
            cmbType.Margin = new Padding(8, 6, 8, 6);
            cmbType.Name = "cmbType";
            cmbType.Size = new Size(256, 40);
            cmbType.TabIndex = 7;
            // 
            // cmbLabel4
            // 
            cmbLabel4.AutoSize = true;
            cmbLabel4.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel4.Location = new Point(13, 241);
            cmbLabel4.Margin = new Padding(8, 0, 8, 0);
            cmbLabel4.Name = "cmbLabel4";
            cmbLabel4.Size = new Size(70, 32);
            cmbLabel4.TabIndex = 6;
            cmbLabel4.Text = "Type:";
            // 
            // cmbLabel3
            // 
            cmbLabel3.AutoSize = true;
            cmbLabel3.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel3.Location = new Point(13, 175);
            cmbLabel3.Margin = new Padding(8, 0, 8, 0);
            cmbLabel3.Name = "cmbLabel3";
            cmbLabel3.Size = new Size(155, 32);
            cmbLabel3.TabIndex = 5;
            cmbLabel3.Text = "Fail Message:";
            // 
            // txtMessage2
            // 
            txtMessage2.BackColor = Color.FromArgb(69, 73, 74);
            txtMessage2.BorderStyle = BorderStyle.FixedSingle;
            txtMessage2.ForeColor = Color.FromArgb(220, 220, 220);
            txtMessage2.Location = new Point(236, 171);
            txtMessage2.Margin = new Padding(8, 6, 8, 6);
            txtMessage2.Name = "txtMessage2";
            txtMessage2.Size = new Size(531, 39);
            txtMessage2.TabIndex = 4;
            // 
            // txtMessage
            // 
            txtMessage.BackColor = Color.FromArgb(69, 73, 74);
            txtMessage.BorderStyle = BorderStyle.FixedSingle;
            txtMessage.ForeColor = Color.FromArgb(220, 220, 220);
            txtMessage.Location = new Point(236, 107);
            txtMessage.Margin = new Padding(8, 6, 8, 6);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(531, 39);
            txtMessage.TabIndex = 3;
            // 
            // cmbLabel2
            // 
            cmbLabel2.AutoSize = true;
            cmbLabel2.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel2.Location = new Point(13, 111);
            cmbLabel2.Margin = new Padding(8, 0, 8, 0);
            cmbLabel2.Name = "cmbLabel2";
            cmbLabel2.Size = new Size(202, 32);
            cmbLabel2.TabIndex = 2;
            cmbLabel2.Text = "Success Message:";
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.FromArgb(220, 220, 220);
            txtName.Location = new Point(236, 43);
            txtName.Margin = new Padding(8, 6, 8, 6);
            txtName.Name = "txtName";
            txtName.Size = new Size(531, 39);
            txtName.TabIndex = 1;
            // 
            // cmbLabel1
            // 
            cmbLabel1.AutoSize = true;
            cmbLabel1.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel1.Location = new Point(13, 47);
            cmbLabel1.Margin = new Padding(8, 0, 8, 0);
            cmbLabel1.Name = "cmbLabel1";
            cmbLabel1.Size = new Size(83, 32);
            cmbLabel1.TabIndex = 0;
            cmbLabel1.Text = "Name:";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(26, 836);
            btnSave.Margin = new Padding(8, 6, 8, 6);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(11, 12, 11, 12);
            btnSave.Size = new Size(424, 58);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(26, 906);
            btnDelete.Margin = new Padding(8, 6, 8, 6);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(11, 12, 11, 12);
            btnDelete.Size = new Size(424, 58);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(26, 976);
            btnCancel.Margin = new Padding(8, 6, 8, 6);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(11, 12, 11, 12);
            btnCancel.Size = new Size(422, 58);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            // 
            // Editor_Resource
            // 
            AutoScaleDimensions = new SizeF(13.0f, 32.0f);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(1286, 1047);
            Controls.Add(btnCancel);
            Controls.Add(btnDelete);
            Controls.Add(btnSave);
            Controls.Add(DarkGroupBox2);
            Controls.Add(DarkGroupBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(8, 6, 8, 6);
            Name = "Editor_Resource";
            Text = "Resource Editor";
            DarkGroupBox1.ResumeLayout(false);
            DarkGroupBox2.ResumeLayout(false);
            DarkGroupBox2.PerformLayout();
            DarkGroupBox5.ResumeLayout(false);
            DarkGroupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudRewardExp).EndInit();
            DarkGroupBox4.ResumeLayout(false);
            DarkGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudLvlReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRespawn).EndInit();
            DarkGroupBox3.ResumeLayout(false);
            DarkGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudExhaustedPic).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudNormalPic).EndInit();
            ((System.ComponentModel.ISupportInitialize)picExhaustedPic).EndInit();
            ((System.ComponentModel.ISupportInitialize)picNormalpic).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudHealth).EndInit();
            Load += new EventHandler(Editor_Resource_Load);
            FormClosing += new FormClosingEventHandler(Editor_Resource_FormClosing);
            ResumeLayout(false);

        }

        internal DarkGroupBox DarkGroupBox1;
        internal ListBox lstIndex;
        internal DarkGroupBox DarkGroupBox2;
        internal cmbLabel cmbLabel3;
        internal DarkTextBox txtMessage2;
        internal DarkTextBox txtMessage;
        internal cmbLabel cmbLabel2;
        internal DarkTextBox txtName;
        internal cmbLabel cmbLabel1;
        internal DarkComboBox cmbType;
        internal cmbLabel cmbLabel4;
        internal DarkNumericUpDown nudHealth;
        internal cmbLabel cmbLabel5;
        internal DarkGroupBox DarkGroupBox3;
        internal PictureBox picExhaustedPic;
        internal PictureBox picNormalpic;
        internal DarkNumericUpDown nudExhaustedPic;
        internal DarkNumericUpDown nudNormalPic;
        internal cmbLabel cmbLabel7;
        internal cmbLabel cmbLabel6;
        internal DarkNumericUpDown nudRespawn;
        internal cmbLabel cmbLabel8;
        internal DarkComboBox cmbAnimation;
        internal cmbLabel cmbLabel9;
        internal DarkGroupBox DarkGroupBox4;
        internal DarkComboBox cmbTool;
        internal cmbLabel cmbLabel10;
        internal DarkNumericUpDown nudLvlReq;
        internal cmbLabel cmbLabel11;
        internal DarkGroupBox DarkGroupBox5;
        internal DarkComboBox cmbRewardItem;
        internal cmbLabel cmbLabel12;
        internal DarkNumericUpDown nudRewardExp;
        internal cmbLabel cmbLabel13;
        internal DarkButton btnSave;
        internal DarkButton btnDelete;
        internal DarkButton btnCancel;
    }
}