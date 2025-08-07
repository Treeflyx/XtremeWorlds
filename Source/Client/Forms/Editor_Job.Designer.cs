using DarkUI.Controls;
using DarkUI.Forms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public partial class Editor_Job : DarkForm
    {

        // Shared instance of the form
        private static Editor_Job _instance;

        // Public property to get the shared instance
        public static Editor_Job Instance
        {
            get
            {
                // Create a new instance if one does not exist or if it has been disposed
                if (_instance is null || _instance.IsDisposed)
                {
                    _instance = new Editor_Job();
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
            DarkGroupBox7 = new DarkUI.Controls.DarkGroupBox();
            nudStartY = new DarkUI.Controls.DarkNumericUpDown();
            nudStartY.Click += new EventHandler(NumStartY_Click);
            cmbLabel15 = new DarkUI.Controls.cmbLabel();
            nudStartX = new DarkUI.Controls.DarkNumericUpDown();
            nudStartX.Click += new EventHandler(NumStartX_Click);
            cmbLabel14 = new DarkUI.Controls.cmbLabel();
            nudStartMap = new DarkUI.Controls.DarkNumericUpDown();
            nudStartMap.Click += new EventHandler(NumStartMap_Click);
            cmbLabel13 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox6 = new DarkUI.Controls.DarkGroupBox();
            btnItemAdd = new DarkUI.Controls.DarkButton();
            btnItemAdd.Click += new EventHandler(BtnItemAdd_Click);
            nudItemAmount = new DarkUI.Controls.DarkNumericUpDown();
            cmbLabel12 = new DarkUI.Controls.cmbLabel();
            cmbItems = new DarkUI.Controls.DarkComboBox();
            cmbLabel11 = new DarkUI.Controls.cmbLabel();
            lstStartItems = new ListBox();
            DarkGroupBox5 = new DarkUI.Controls.DarkGroupBox();
            nudBaseExp = new DarkUI.Controls.DarkNumericUpDown();
            nudBaseExp.Click += new EventHandler(NumBaseExp_ValueChanged);
            cmbLabel10 = new DarkUI.Controls.cmbLabel();
            nudSpirit = new DarkUI.Controls.DarkNumericUpDown();
            nudSpirit.Click += new EventHandler(NumSpirit_ValueChanged);
            cmbLabel8 = new DarkUI.Controls.cmbLabel();
            cmbLabel9 = new DarkUI.Controls.cmbLabel();
            nudVitality = new DarkUI.Controls.DarkNumericUpDown();
            nudVitality.Click += new EventHandler(NumVitality_ValueChanged);
            nudLuck = new DarkUI.Controls.DarkNumericUpDown();
            nudLuck.Click += new EventHandler(NumLuck_ValueChanged);
            cmbLabel6 = new DarkUI.Controls.cmbLabel();
            cmbLabel7 = new DarkUI.Controls.cmbLabel();
            nudIntelligence = new DarkUI.Controls.DarkNumericUpDown();
            nudIntelligence.Click += new EventHandler(NumIntelligence_ValueChanged);
            nudStrength = new DarkUI.Controls.DarkNumericUpDown();
            nudStrength.Click += new EventHandler(NumStrength_ValueChanged);
            cmbLabel5 = new DarkUI.Controls.cmbLabel();
            cmbLabel3 = new DarkUI.Controls.cmbLabel();
            DarkGroupBox4 = new DarkUI.Controls.DarkGroupBox();
            nudFemaleSprite = new DarkUI.Controls.DarkNumericUpDown();
            nudFemaleSprite.Click += new EventHandler(nudFemaleSprite_Click);
            cmbLabel4 = new DarkUI.Controls.cmbLabel();
            picFemale = new PictureBox();
            DarkGroupBox3 = new DarkUI.Controls.DarkGroupBox();
            nudMaleSprite = new DarkUI.Controls.DarkNumericUpDown();
            nudMaleSprite.Click += new EventHandler(nudMaleSprite_Click);
            lblMaleSprite = new DarkUI.Controls.cmbLabel();
            picMale = new PictureBox();
            txtDescription = new DarkUI.Controls.DarkTextBox();
            txtDescription.TextChanged += new EventHandler(TxtDescription_TextChanged);
            cmbLabel2 = new DarkUI.Controls.cmbLabel();
            txtName = new DarkUI.Controls.DarkTextBox();
            txtName.TextChanged += new EventHandler(TxtName_TextChanged);
            cmbLabel1 = new DarkUI.Controls.cmbLabel();
            btnCancel = new DarkUI.Controls.DarkButton();
            btnCancel.Click += new EventHandler(BtnCancel_Click);
            btnSave = new DarkUI.Controls.DarkButton();
            btnSave.Click += new EventHandler(BtnSave_Click);
            btnDelete = new DarkUI.Controls.DarkButton();
            btnDelete.Click += new EventHandler(btnDelete_Click);
            DarkGroupBox1.SuspendLayout();
            DarkGroupBox2.SuspendLayout();
            DarkGroupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudStartY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStartX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStartMap).BeginInit();
            DarkGroupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudItemAmount).BeginInit();
            DarkGroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudBaseExp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSpirit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVitality).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLuck).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudIntelligence).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStrength).BeginInit();
            DarkGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudFemaleSprite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picFemale).BeginInit();
            DarkGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMaleSprite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picMale).BeginInit();
            SuspendLayout();
            // 
            // DarkGroupBox1
            // 
            DarkGroupBox1.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox1.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox1.Controls.Add(lstIndex);
            DarkGroupBox1.ForeColor = Color.Gainsboro;
            DarkGroupBox1.Location = new Point(2, 3);
            DarkGroupBox1.Margin = new Padding(5, 5, 5, 5);
            DarkGroupBox1.Name = "DarkGroupBox1";
            DarkGroupBox1.Padding = new Padding(5, 5, 5, 5);
            DarkGroupBox1.Size = new Size(288, 742);
            DarkGroupBox1.TabIndex = 0;
            DarkGroupBox1.TabStop = false;
            DarkGroupBox1.Text = "Job List";
            // 
            // lstIndex
            // 
            lstIndex.BackColor = Color.FromArgb(45, 45, 48);
            lstIndex.BorderStyle = BorderStyle.FixedSingle;
            lstIndex.ForeColor = Color.Gainsboro;
            lstIndex.FormattingEnabled = true;
            lstIndex.ItemHeight = 25;
            lstIndex.Location = new Point(10, 27);
            lstIndex.Margin = new Padding(5, 5, 5, 5);
            lstIndex.Name = "lstIndex";
            lstIndex.Size = new Size(265, 702);
            lstIndex.TabIndex = 0;
            // 
            // DarkGroupBox2
            // 
            DarkGroupBox2.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox2.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox2.Controls.Add(DarkGroupBox7);
            DarkGroupBox2.Controls.Add(DarkGroupBox6);
            DarkGroupBox2.Controls.Add(DarkGroupBox5);
            DarkGroupBox2.Controls.Add(DarkGroupBox4);
            DarkGroupBox2.Controls.Add(DarkGroupBox3);
            DarkGroupBox2.Controls.Add(txtDescription);
            DarkGroupBox2.Controls.Add(cmbLabel2);
            DarkGroupBox2.Controls.Add(txtName);
            DarkGroupBox2.Controls.Add(cmbLabel1);
            DarkGroupBox2.ForeColor = Color.Gainsboro;
            DarkGroupBox2.Location = new Point(302, 3);
            DarkGroupBox2.Margin = new Padding(5, 5, 5, 5);
            DarkGroupBox2.Name = "DarkGroupBox2";
            DarkGroupBox2.Padding = new Padding(5, 5, 5, 5);
            DarkGroupBox2.Size = new Size(568, 909);
            DarkGroupBox2.TabIndex = 1;
            DarkGroupBox2.TabStop = false;
            DarkGroupBox2.Text = "Properties";
            // 
            // DarkGroupBox7
            // 
            DarkGroupBox7.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox7.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox7.Controls.Add(nudStartY);
            DarkGroupBox7.Controls.Add(cmbLabel15);
            DarkGroupBox7.Controls.Add(nudStartX);
            DarkGroupBox7.Controls.Add(cmbLabel14);
            DarkGroupBox7.Controls.Add(nudStartMap);
            DarkGroupBox7.Controls.Add(cmbLabel13);
            DarkGroupBox7.ForeColor = Color.Gainsboro;
            DarkGroupBox7.Location = new Point(10, 812);
            DarkGroupBox7.Margin = new Padding(5, 5, 5, 5);
            DarkGroupBox7.Name = "DarkGroupBox7";
            DarkGroupBox7.Padding = new Padding(5, 5, 5, 5);
            DarkGroupBox7.Size = new Size(547, 84);
            DarkGroupBox7.TabIndex = 8;
            DarkGroupBox7.TabStop = false;
            DarkGroupBox7.Text = "Starting Point";
            // 
            // nudStartY
            // 
            nudStartY.Location = new Point(457, 27);
            nudStartY.Margin = new Padding(5, 5, 5, 5);
            nudStartY.Name = "nudStartY";
            nudStartY.Size = new Size(80, 31);
            nudStartY.TabIndex = 5;
            // 
            // cmbLabel15
            // 
            cmbLabel15.AutoSize = true;
            cmbLabel15.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel15.Location = new Point(377, 30);
            cmbLabel15.Margin = new Padding(5, 0, 5, 0);
            cmbLabel15.Name = "cmbLabel15";
            cmbLabel15.Size = new Size(57, 25);
            cmbLabel15.TabIndex = 4;
            cmbLabel15.Text = "Start :";
            // 
            // nudStartX
            // 
            nudStartX.Location = new Point(280, 27);
            nudStartX.Margin = new Padding(5, 5, 5, 5);
            nudStartX.Name = "nudStartX";
            nudStartX.Size = new Size(80, 31);
            nudStartX.TabIndex = 3;
            // 
            // cmbLabel14
            // 
            cmbLabel14.AutoSize = true;
            cmbLabel14.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel14.Location = new Point(200, 30);
            cmbLabel14.Margin = new Padding(5, 0, 5, 0);
            cmbLabel14.Name = "cmbLabel14";
            cmbLabel14.Size = new Size(68, 25);
            cmbLabel14.TabIndex = 2;
            cmbLabel14.Text = "Start X:";
            // 
            // nudStartMap
            // 
            nudStartMap.Location = new Point(113, 27);
            nudStartMap.Margin = new Padding(5, 5, 5, 5);
            nudStartMap.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudStartMap.Name = "nudStartMap";
            nudStartMap.Size = new Size(77, 31);
            nudStartMap.TabIndex = 1;
            nudStartMap.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // cmbLabel13
            // 
            cmbLabel13.AutoSize = true;
            cmbLabel13.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel13.Location = new Point(10, 30);
            cmbLabel13.Margin = new Padding(5, 0, 5, 0);
            cmbLabel13.Name = "cmbLabel13";
            cmbLabel13.Size = new Size(93, 25);
            cmbLabel13.TabIndex = 0;
            cmbLabel13.Text = "Start Map:";
            // 
            // DarkGroupBox6
            // 
            DarkGroupBox6.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox6.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox6.Controls.Add(btnItemAdd);
            DarkGroupBox6.Controls.Add(nudItemAmount);
            DarkGroupBox6.Controls.Add(cmbLabel12);
            DarkGroupBox6.Controls.Add(cmbItems);
            DarkGroupBox6.Controls.Add(cmbLabel11);
            DarkGroupBox6.Controls.Add(lstStartItems);
            DarkGroupBox6.ForeColor = Color.Gainsboro;
            DarkGroupBox6.Location = new Point(10, 597);
            DarkGroupBox6.Margin = new Padding(5, 5, 5, 5);
            DarkGroupBox6.Name = "DarkGroupBox6";
            DarkGroupBox6.Padding = new Padding(5, 5, 5, 5);
            DarkGroupBox6.Size = new Size(547, 203);
            DarkGroupBox6.TabIndex = 7;
            DarkGroupBox6.TabStop = false;
            DarkGroupBox6.Text = "Starting Items";
            // 
            // btnItemAdd
            // 
            btnItemAdd.Location = new Point(305, 140);
            btnItemAdd.Margin = new Padding(5, 5, 5, 5);
            btnItemAdd.Name = "btnItemAdd";
            btnItemAdd.Padding = new Padding(8, 10, 8, 10);
            btnItemAdd.Size = new Size(232, 50);
            btnItemAdd.TabIndex = 6;
            btnItemAdd.Text = "Update";
            // 
            // nudItemAmount
            // 
            nudItemAmount.Location = new Point(392, 97);
            nudItemAmount.Margin = new Padding(5, 5, 5, 5);
            nudItemAmount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudItemAmount.Name = "nudItemAmount";
            nudItemAmount.Size = new Size(145, 31);
            nudItemAmount.TabIndex = 5;
            nudItemAmount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // cmbLabel12
            // 
            cmbLabel12.AutoSize = true;
            cmbLabel12.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel12.Location = new Point(305, 100);
            cmbLabel12.Margin = new Padding(5, 0, 5, 0);
            cmbLabel12.Name = "cmbLabel12";
            cmbLabel12.Size = new Size(81, 25);
            cmbLabel12.TabIndex = 4;
            cmbLabel12.Text = "Amount:";
            // 
            // cmbItems
            // 
            cmbItems.DrawMode = DrawMode.OwnerDrawFixed;
            cmbItems.FormattingEnabled = true;
            cmbItems.Location = new Point(305, 45);
            cmbItems.Margin = new Padding(5, 5, 5, 5);
            cmbItems.Name = "cmbItems";
            cmbItems.Size = new Size(229, 32);
            cmbItems.TabIndex = 3;
            // 
            // cmbLabel11
            // 
            cmbLabel11.AutoSize = true;
            cmbLabel11.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel11.Location = new Point(305, 13);
            cmbLabel11.Margin = new Padding(5, 0, 5, 0);
            cmbLabel11.Name = "cmbLabel11";
            cmbLabel11.Size = new Size(89, 25);
            cmbLabel11.TabIndex = 2;
            cmbLabel11.Text = "Start Item";
            // 
            // lstStartItems
            // 
            lstStartItems.BackColor = Color.FromArgb(45, 45, 48);
            lstStartItems.BorderStyle = BorderStyle.FixedSingle;
            lstStartItems.ForeColor = Color.Gainsboro;
            lstStartItems.FormattingEnabled = true;
            lstStartItems.ItemHeight = 25;
            lstStartItems.Items.AddRange(new object[] { "1", "2", "3", "4", "5" });
            lstStartItems.Location = new Point(10, 37);
            lstStartItems.Margin = new Padding(5, 5, 5, 5);
            lstStartItems.Name = "lstStartItems";
            lstStartItems.Size = new Size(284, 152);
            lstStartItems.TabIndex = 1;
            // 
            // DarkGroupBox5
            // 
            DarkGroupBox5.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox5.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox5.Controls.Add(nudBaseExp);
            DarkGroupBox5.Controls.Add(cmbLabel10);
            DarkGroupBox5.Controls.Add(nudSpirit);
            DarkGroupBox5.Controls.Add(cmbLabel8);
            DarkGroupBox5.Controls.Add(cmbLabel9);
            DarkGroupBox5.Controls.Add(nudVitality);
            DarkGroupBox5.Controls.Add(nudLuck);
            DarkGroupBox5.Controls.Add(cmbLabel6);
            DarkGroupBox5.Controls.Add(cmbLabel7);
            DarkGroupBox5.Controls.Add(nudIntelligence);
            DarkGroupBox5.Controls.Add(nudStrength);
            DarkGroupBox5.Controls.Add(cmbLabel5);
            DarkGroupBox5.Controls.Add(cmbLabel3);
            DarkGroupBox5.ForeColor = Color.Gainsboro;
            DarkGroupBox5.Location = new Point(10, 391);
            DarkGroupBox5.Margin = new Padding(5, 5, 5, 5);
            DarkGroupBox5.Name = "DarkGroupBox5";
            DarkGroupBox5.Padding = new Padding(5, 5, 5, 5);
            DarkGroupBox5.Size = new Size(547, 191);
            DarkGroupBox5.TabIndex = 6;
            DarkGroupBox5.TabStop = false;
            DarkGroupBox5.Text = "Start Stats";
            // 
            // nudBaseExp
            // 
            nudBaseExp.Location = new Point(170, 135);
            nudBaseExp.Margin = new Padding(5, 5, 5, 5);
            nudBaseExp.Name = "nudBaseExp";
            nudBaseExp.Size = new Size(172, 31);
            nudBaseExp.TabIndex = 13;
            // 
            // cmbLabel10
            // 
            cmbLabel10.AutoSize = true;
            cmbLabel10.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel10.Location = new Point(10, 138);
            cmbLabel10.Margin = new Padding(5, 0, 5, 0);
            cmbLabel10.Name = "cmbLabel10";
            cmbLabel10.Size = new Size(85, 25);
            cmbLabel10.TabIndex = 12;
            cmbLabel10.Text = "Base Exp:";
            // 
            // nudSpirit
            // 
            nudSpirit.Location = new Point(462, 77);
            nudSpirit.Margin = new Padding(5, 5, 5, 5);
            nudSpirit.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSpirit.Name = "nudSpirit";
            nudSpirit.Size = new Size(75, 31);
            nudSpirit.TabIndex = 11;
            nudSpirit.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // cmbLabel8
            // 
            cmbLabel8.AutoSize = true;
            cmbLabel8.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel8.Location = new Point(352, 80);
            cmbLabel8.Margin = new Padding(5, 0, 5, 0);
            cmbLabel8.Name = "cmbLabel8";
            cmbLabel8.Size = new Size(57, 25);
            cmbLabel8.TabIndex = 9;
            cmbLabel8.Text = "Spirit:";
            // 
            // cmbLabel9
            // 
            cmbLabel9.AutoSize = true;
            cmbLabel9.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel9.Location = new Point(352, 30);
            cmbLabel9.Margin = new Padding(5, 0, 5, 0);
            cmbLabel9.Name = "cmbLabel9";
            cmbLabel9.Size = new Size(98, 25);
            cmbLabel9.TabIndex = 8;
            cmbLabel9.Text = "Endurance:";
            // 
            // nudVitality
            // 
            nudVitality.Location = new Point(267, 77);
            nudVitality.Margin = new Padding(5, 5, 5, 5);
            nudVitality.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudVitality.Name = "nudVitality";
            nudVitality.Size = new Size(75, 31);
            nudVitality.TabIndex = 7;
            nudVitality.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // nudLuck
            // 
            nudLuck.Location = new Point(267, 27);
            nudLuck.Margin = new Padding(5, 5, 5, 5);
            nudLuck.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudLuck.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudLuck.Name = "nudLuck";
            nudLuck.Size = new Size(75, 31);
            nudLuck.TabIndex = 6;
            nudLuck.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // cmbLabel6
            // 
            cmbLabel6.AutoSize = true;
            cmbLabel6.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel6.Location = new Point(200, 80);
            cmbLabel6.Margin = new Padding(5, 0, 5, 0);
            cmbLabel6.Name = "cmbLabel6";
            cmbLabel6.Size = new Size(69, 25);
            cmbLabel6.TabIndex = 5;
            cmbLabel6.Text = "Vitality:";
            // 
            // cmbLabel7
            // 
            cmbLabel7.AutoSize = true;
            cmbLabel7.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel7.Location = new Point(200, 30);
            cmbLabel7.Margin = new Padding(5, 0, 5, 0);
            cmbLabel7.Name = "cmbLabel7";
            cmbLabel7.Size = new Size(51, 25);
            cmbLabel7.TabIndex = 4;
            cmbLabel7.Text = "Luck:";
            // 
            // nudIntelligence
            // 
            nudIntelligence.Location = new Point(115, 77);
            nudIntelligence.Margin = new Padding(5, 5, 5, 5);
            nudIntelligence.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudIntelligence.Name = "nudIntelligence";
            nudIntelligence.Size = new Size(75, 31);
            nudIntelligence.TabIndex = 3;
            nudIntelligence.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // nudStrength
            // 
            nudStrength.Location = new Point(115, 27);
            nudStrength.Margin = new Padding(5, 5, 5, 5);
            nudStrength.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudStrength.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudStrength.Name = "nudStrength";
            nudStrength.Size = new Size(75, 31);
            nudStrength.TabIndex = 2;
            nudStrength.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // cmbLabel5
            // 
            cmbLabel5.AutoSize = true;
            cmbLabel5.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel5.Location = new Point(10, 80);
            cmbLabel5.Margin = new Padding(5, 0, 5, 0);
            cmbLabel5.Name = "cmbLabel5";
            cmbLabel5.Size = new Size(105, 25);
            cmbLabel5.TabIndex = 1;
            cmbLabel5.Text = "Intelligence:";
            // 
            // cmbLabel3
            // 
            cmbLabel3.AutoSize = true;
            cmbLabel3.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel3.Location = new Point(10, 30);
            cmbLabel3.Margin = new Padding(5, 0, 5, 0);
            cmbLabel3.Name = "cmbLabel3";
            cmbLabel3.Size = new Size(83, 25);
            cmbLabel3.TabIndex = 0;
            cmbLabel3.Text = "Strength:";
            // 
            // DarkGroupBox4
            // 
            DarkGroupBox4.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox4.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox4.Controls.Add(nudFemaleSprite);
            DarkGroupBox4.Controls.Add(cmbLabel4);
            DarkGroupBox4.Controls.Add(picFemale);
            DarkGroupBox4.ForeColor = Color.Gainsboro;
            DarkGroupBox4.Location = new Point(288, 166);
            DarkGroupBox4.Margin = new Padding(5, 5, 5, 5);
            DarkGroupBox4.Name = "DarkGroupBox4";
            DarkGroupBox4.Padding = new Padding(5, 5, 5, 5);
            DarkGroupBox4.Size = new Size(268, 213);
            DarkGroupBox4.TabIndex = 5;
            DarkGroupBox4.TabStop = false;
            DarkGroupBox4.Text = "Female Sprite";
            // 
            // nudFemaleSprite
            // 
            nudFemaleSprite.Location = new Point(80, 162);
            nudFemaleSprite.Margin = new Padding(5, 5, 5, 5);
            nudFemaleSprite.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudFemaleSprite.Name = "nudFemaleSprite";
            nudFemaleSprite.Size = new Size(92, 31);
            nudFemaleSprite.TabIndex = 18;
            nudFemaleSprite.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // cmbLabel4
            // 
            cmbLabel4.AutoSize = true;
            cmbLabel4.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel4.Location = new Point(12, 165);
            cmbLabel4.Margin = new Padding(5, 0, 5, 0);
            cmbLabel4.Name = "cmbLabel4";
            cmbLabel4.Size = new Size(62, 25);
            cmbLabel4.TabIndex = 17;
            cmbLabel4.Text = "Sprite:";
            // 
            // picFemale
            // 
            picFemale.BackColor = Color.FromArgb(64, 64, 64);
            picFemale.BackgroundImageLayout = ImageLayout.None;
            picFemale.Location = new Point(178, 20);
            picFemale.Margin = new Padding(5, 5, 5, 5);
            picFemale.Name = "picFemale";
            picFemale.Size = new Size(80, 123);
            picFemale.TabIndex = 14;
            picFemale.TabStop = false;
            // 
            // DarkGroupBox3
            // 
            DarkGroupBox3.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox3.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox3.Controls.Add(nudMaleSprite);
            DarkGroupBox3.Controls.Add(lblMaleSprite);
            DarkGroupBox3.Controls.Add(picMale);
            DarkGroupBox3.ForeColor = Color.Gainsboro;
            DarkGroupBox3.Location = new Point(10, 166);
            DarkGroupBox3.Margin = new Padding(5, 5, 5, 5);
            DarkGroupBox3.Name = "DarkGroupBox3";
            DarkGroupBox3.Padding = new Padding(5, 5, 5, 5);
            DarkGroupBox3.Size = new Size(268, 213);
            DarkGroupBox3.TabIndex = 4;
            DarkGroupBox3.TabStop = false;
            DarkGroupBox3.Text = "Male Sprite";
            // 
            // nudMaleSprite
            // 
            nudMaleSprite.Location = new Point(80, 162);
            nudMaleSprite.Margin = new Padding(5, 5, 5, 5);
            nudMaleSprite.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudMaleSprite.Name = "nudMaleSprite";
            nudMaleSprite.Size = new Size(92, 31);
            nudMaleSprite.TabIndex = 12;
            nudMaleSprite.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblMaleSprite
            // 
            lblMaleSprite.AutoSize = true;
            lblMaleSprite.ForeColor = Color.FromArgb(220, 220, 220);
            lblMaleSprite.Location = new Point(12, 165);
            lblMaleSprite.Margin = new Padding(5, 0, 5, 0);
            lblMaleSprite.Name = "lblMaleSprite";
            lblMaleSprite.Size = new Size(62, 25);
            lblMaleSprite.TabIndex = 11;
            lblMaleSprite.Text = "Sprite:";
            // 
            // picMale
            // 
            picMale.BackColor = Color.FromArgb(64, 64, 64);
            picMale.BackgroundImageLayout = ImageLayout.None;
            picMale.Location = new Point(178, 20);
            picMale.Margin = new Padding(5, 5, 5, 5);
            picMale.Name = "picMale";
            picMale.Size = new Size(80, 123);
            picMale.TabIndex = 8;
            picMale.TabStop = false;
            // 
            // txtDescription
            // 
            txtDescription.BackColor = Color.FromArgb(69, 73, 74);
            txtDescription.BorderStyle = BorderStyle.FixedSingle;
            txtDescription.Font = new Font("Segoe UI", 8.25f);
            txtDescription.ForeColor = Color.FromArgb(220, 220, 220);
            txtDescription.Location = new Point(125, 90);
            txtDescription.Margin = new Padding(5, 5, 5, 5);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(430, 64);
            txtDescription.TabIndex = 3;
            // 
            // cmbLabel2
            // 
            cmbLabel2.AutoSize = true;
            cmbLabel2.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel2.Location = new Point(12, 110);
            cmbLabel2.Margin = new Padding(5, 0, 5, 0);
            cmbLabel2.Name = "cmbLabel2";
            cmbLabel2.Size = new Size(106, 25);
            cmbLabel2.TabIndex = 2;
            cmbLabel2.Text = "Description:";
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.Font = new Font("Segoe UI", 8.25f);
            txtName.ForeColor = Color.FromArgb(220, 220, 220);
            txtName.Location = new Point(83, 27);
            txtName.Margin = new Padding(5, 5, 5, 5);
            txtName.Name = "txtName";
            txtName.Size = new Size(472, 29);
            txtName.TabIndex = 1;
            // 
            // cmbLabel1
            // 
            cmbLabel1.AutoSize = true;
            cmbLabel1.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel1.Location = new Point(10, 30);
            cmbLabel1.Margin = new Padding(5, 0, 5, 0);
            cmbLabel1.Name = "cmbLabel1";
            cmbLabel1.Size = new Size(63, 25);
            cmbLabel1.TabIndex = 0;
            cmbLabel1.Text = "Name:";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(12, 867);
            btnCancel.Margin = new Padding(5, 5, 5, 5);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(8, 10, 8, 10);
            btnCancel.Size = new Size(265, 45);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(12, 756);
            btnSave.Margin = new Padding(5, 5, 5, 5);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(8, 10, 8, 10);
            btnSave.Size = new Size(265, 45);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(12, 812);
            btnDelete.Margin = new Padding(5, 5, 5, 5);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(8, 10, 8, 10);
            btnDelete.Size = new Size(265, 45);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "Delete";
            // 
            // Editor_Job
            // 
            AutoScaleDimensions = new SizeF(10.0f, 25.0f);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(879, 920);
            Controls.Add(btnDelete);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(DarkGroupBox2);
            Controls.Add(DarkGroupBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(5, 5, 5, 5);
            Name = "Editor_Job";
            Text = "Job Editor";
            DarkGroupBox1.ResumeLayout(false);
            DarkGroupBox2.ResumeLayout(false);
            DarkGroupBox2.PerformLayout();
            DarkGroupBox7.ResumeLayout(false);
            DarkGroupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudStartY).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStartX).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStartMap).EndInit();
            DarkGroupBox6.ResumeLayout(false);
            DarkGroupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudItemAmount).EndInit();
            DarkGroupBox5.ResumeLayout(false);
            DarkGroupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudBaseExp).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSpirit).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVitality).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLuck).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudIntelligence).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStrength).EndInit();
            DarkGroupBox4.ResumeLayout(false);
            DarkGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudFemaleSprite).EndInit();
            ((System.ComponentModel.ISupportInitialize)picFemale).EndInit();
            DarkGroupBox3.ResumeLayout(false);
            DarkGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMaleSprite).EndInit();
            ((System.ComponentModel.ISupportInitialize)picMale).EndInit();
            Load += new EventHandler(Editor_Job_Load);
            FormClosing += new FormClosingEventHandler(Editor_Job_FormClosing);
            ResumeLayout(false);
        }

        internal DarkGroupBox DarkGroupBox1;
        internal DarkGroupBox DarkGroupBox2;
        internal DarkTextBox txtName;
        internal cmbLabel cmbLabel1;
        internal DarkTextBox txtDescription;
        internal cmbLabel cmbLabel2;
        internal ListBox lstIndex;
        internal DarkGroupBox DarkGroupBox4;
        internal DarkGroupBox DarkGroupBox3;
        internal PictureBox picMale;
        internal cmbLabel cmbLabel4;
        internal PictureBox picFemale;
        internal cmbLabel lblMaleSprite;
        internal DarkButton btnCancel;
        internal DarkButton btnSave;
        internal DarkGroupBox DarkGroupBox5;
        internal DarkNumericUpDown nudFemaleSprite;
        internal DarkNumericUpDown nudMaleSprite;
        internal cmbLabel cmbLabel5;
        internal cmbLabel cmbLabel3;
        internal DarkNumericUpDown nudIntelligence;
        internal DarkNumericUpDown nudStrength;
        internal DarkNumericUpDown nudVitality;
        internal DarkNumericUpDown nudLuck;
        internal cmbLabel cmbLabel6;
        internal cmbLabel cmbLabel7;
        internal DarkNumericUpDown nudSpirit;
        internal DarkNumericUpDown nudEndurance;
        internal cmbLabel cmbLabel8;
        internal cmbLabel cmbLabel9;
        internal DarkNumericUpDown nudBaseExp;
        internal cmbLabel cmbLabel10;
        internal DarkGroupBox DarkGroupBox6;
        internal ListBox lstStartItems;
        internal DarkComboBox cmbItems;
        internal cmbLabel cmbLabel11;
        internal DarkNumericUpDown nudItemAmount;
        internal cmbLabel cmbLabel12;
        internal DarkButton btnItemAdd;
        internal DarkGroupBox DarkGroupBox7;
        internal DarkNumericUpDown nudStartMap;
        internal cmbLabel cmbLabel13;
        internal DarkNumericUpDown nudStartY;
        internal cmbLabel cmbLabel15;
        internal DarkNumericUpDown nudStartX;
        internal cmbLabel cmbLabel14;
        internal DarkButton btnDelete;
    }
}