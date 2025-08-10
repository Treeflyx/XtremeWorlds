using System;
using System.IO;
using Client.Net;
using Core;
using Microsoft.VisualBasic;
using Eto.Forms;
using System;
using System.IO;
using Eto.Drawing;

namespace Client
{
    public class Editor_Skill : Form
    {
        private static Editor_Skill? _instance;
        public static Editor_Skill Instance => _instance ??= new Editor_Skill();

        // Public controls (names preserved)
        public ListBox lstIndex = new ListBox();
        public TextBox txtName = new TextBox();
        public ComboBox cmbType = new ComboBox();
        public NumericStepper nudMp = new NumericStepper { MinValue = 0 };
        public NumericStepper nudLevel = new NumericStepper { MinValue = 0 };
        public ComboBox cmbAccessReq = new ComboBox();
        public ComboBox cmbJob = new ComboBox();
        public NumericStepper nudCast = new NumericStepper { MinValue = 0 };
        public NumericStepper nudCool = new NumericStepper { MinValue = 0 };
        public NumericStepper nudIcon = new NumericStepper { MinValue = 0 };
        public NumericStepper nudMap = new NumericStepper { MinValue = 0 };
        public NumericStepper nudX = new NumericStepper { MinValue = 0 };
        public NumericStepper nudY = new NumericStepper { MinValue = 0 };
        public ComboBox cmbDir = new ComboBox();
        public NumericStepper nudVital = new NumericStepper { MinValue = 0 };
        public NumericStepper nudDuration = new NumericStepper { MinValue = 0 };
        public NumericStepper nudInterval = new NumericStepper { MinValue = 0 };
        public NumericStepper nudRange = new NumericStepper { MinValue = 0 };
        public CheckBox chkAoE = new CheckBox { Text = "AoE" };
        public NumericStepper nudAoE = new NumericStepper { MinValue = 0 };
        public ComboBox cmbAnimCast = new ComboBox();
        public ComboBox cmbAnim = new ComboBox();
        public NumericStepper nudStun = new NumericStepper { MinValue = 0 };
        public CheckBox chkProjectile = new CheckBox { Text = "Projectile" };
        public ComboBox cmbProjectile = new ComboBox();
        public CheckBox chkKnockBack = new CheckBox { Text = "KnockBack" };
        public ComboBox cmbKnockBackTiles = new ComboBox();
        private Button btnSave = new Button { Text = "Save" };
        private Button btnDelete = new Button { Text = "Delete" };
        private Button btnCancel = new Button { Text = "Cancel" };
        private Button btnLearn = new Button { Text = "Learn" };
        public Drawable picSprite = new Drawable { Size = new Size(64, 64) };

        private Editor_Skill()
        {
            Title = "Skill Editor";
            ClientSize = new Size(900, 560);
            Padding = 10;
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (ReferenceEquals(_instance, this)) _instance = null;
            Editors.SkillEditorCancel();
        }

        private void InitializeComponent()
        {
            // Populate static combos (basic placeholders; actual data is set elsewhere)
            cmbType.Items.Add("Damage");
            cmbType.Items.Add("Heal");
            cmbType.Items.Add("Buff");
            cmbType.Items.Add("Debuff");

            cmbAccessReq.Items.Add("None");
            cmbAccessReq.Items.Add("Admin");

            cmbDir.Items.Add("Up");
            cmbDir.Items.Add("Down");
            cmbDir.Items.Add("Left");
            cmbDir.Items.Add("Right");

            cmbKnockBackTiles.Items.Add("0");
            cmbKnockBackTiles.Items.Add("1");
            cmbKnockBackTiles.Items.Add("2");
            cmbKnockBackTiles.Items.Add("3");

            // Wiring events
            lstIndex.SelectedIndexChanged += (s, e) => LstIndex_Click();
            txtName.TextChanged += (s, e) => TxtName_TextChanged();
            cmbType.SelectedIndexChanged += (s, e) => CmbType_SelectedIndexChanged();
            nudMp.ValueChanged += (s, e) => NudMp_ValueChanged();
            nudLevel.ValueChanged += (s, e) => NudLevel_ValueChanged();
            cmbAccessReq.SelectedIndexChanged += (s, e) => CmbAccessReq_SelectedIndexChanged();
            cmbJob.SelectedIndexChanged += (s, e) => CmbJob_SelectedIndexChanged();
            nudCast.ValueChanged += (s, e) => NudCast_ValueChanged();
            nudCool.ValueChanged += (s, e) => NudCool_ValueChanged();
            nudIcon.ValueChanged += (s, e) => NudIcon_ValueChanged();
            nudMap.ValueChanged += (s, e) => NudMap_ValueChanged();
            cmbDir.SelectedIndexChanged += (s, e) => CmbDir_SelectedIndexChanged();
            nudX.ValueChanged += (s, e) => NudX_ValueChanged();
            nudY.ValueChanged += (s, e) => NudY_ValueChanged();
            nudVital.ValueChanged += (s, e) => NudVital_ValueChanged();
            nudDuration.ValueChanged += (s, e) => NudDuration_ValueChanged();
            nudInterval.ValueChanged += (s, e) => NudInterval_ValueChanged();
            nudRange.ValueChanged += (s, e) => NudRange_ValueChanged();
            chkAoE.CheckedChanged += (s, e) => ChkAoE_CheckedChanged();
            nudAoE.ValueChanged += (s, e) => NudAoE_ValueChanged();
            cmbAnimCast.SelectedIndexChanged += (s, e) => CmbAnimCast_SelectedIndexChanged();
            cmbAnim.SelectedIndexChanged += (s, e) => CmbAnim_SelectedIndexChanged();
            nudStun.ValueChanged += (s, e) => NudStun_ValueChanged();
            chkProjectile.CheckedChanged += (s, e) => ChkProjectile_CheckedChanged();
            cmbProjectile.SelectedIndexChanged += (s, e) => CmbProjectile_SelectedIndexChanged();
            chkKnockBack.CheckedChanged += (s, e) => ChkKnockBack_CheckedChanged();
            cmbKnockBackTiles.SelectedIndexChanged += (s, e) => CmbKnockBackTiles_SelectedIndexChanged();
            btnSave.Click += (s, e) => BtnSave_Click();
            btnDelete.Click += (s, e) => BtnDelete_Click();
            btnCancel.Click += (s, e) => BtnCancel_Click();
            btnLearn.Click += (s, e) => BtnLearn_Click();
            Load += (s, e) => Editor_Skill_Load();

            picSprite.Paint += (s, e) => DrawIcon(e.Graphics, (int)Math.Round(nudIcon.Value));

            // Layouts
            var listLayout = new DynamicLayout { Spacing = new Size(5,5) };
            listLayout.AddRow(new Label { Text = "Skills" });
            listLayout.Add(lstIndex, yscale: true);

            var general = new DynamicLayout { Spacing = new Size(4,4) };
            general.AddRow("Name:", txtName, "Type:", cmbType, "Access:", cmbAccessReq, "Job:", cmbJob);
            general.AddRow("MP:", nudMp, "Level:", nudLevel, "Cast:", nudCast, "Cool:", nudCool);
            general.AddRow("Icon:", nudIcon, picSprite, "Map:", nudMap, "Dir:", cmbDir);
            general.AddRow("X:", nudX, "Y:", nudY, "Vital:", nudVital, "Dur:", nudDuration);
            general.AddRow("Interval:", nudInterval, "Range:", nudRange, chkAoE, "AoE Size:", nudAoE);
            general.AddRow("Cast Anim:", cmbAnimCast, "Skill Anim:", cmbAnim, "Stun:", nudStun);
            general.AddRow(chkProjectile, "Projectile:", cmbProjectile, chkKnockBack, "KB Tiles:", cmbKnockBackTiles);

            var buttons = new StackLayout { Orientation = Orientation.Horizontal, Spacing = 5, Items = { btnSave, btnDelete, btnCancel, btnLearn } };

            var rightLayout = new DynamicLayout { Spacing = new Size(6,6) };
            rightLayout.Add(general);
            rightLayout.Add(buttons);

            Content = new TableLayout
            {
                Spacing = new Size(10,10),
                Rows = { new TableRow(new TableCell(listLayout, true), new TableCell(rightLayout, true)) }
            };
        }

        private void Editor_Skill_Load()
        {
            lstIndex.Items.Clear();
            for (int i = 0; i < Constant.MaxSkills; i++)
                lstIndex.Items.Add($"{i + 1}: {Strings.Trim(Core.Data.Skill[i].Name)}");

            cmbAnimCast.Items.Clear();
            cmbAnim.Items.Clear();
            for (int i = 0; i < Constant.MaxAnimations; i++)
            {
                cmbAnimCast.Items.Add($"{i + 1}: {Core.Data.Animation[i].Name}");
                cmbAnim.Items.Add($"{i + 1}: {Core.Data.Animation[i].Name}");
            }

            cmbProjectile.Items.Clear();
            for (int i = 0; i < Constant.MaxAnimations; i++)
                cmbProjectile.Items.Add($"{i + 1}: {Core.Data.Projectile[i].Name}");

            cmbJob.Items.Clear();
            for (int i = 0; i < Constant.MaxJobs; i++)
                cmbJob.Items.Add($"{i + 1}: {Core.Data.Job[i].Name.Trim()}");

            if (lstIndex.Items.Count > 0)
            {
                lstIndex.SelectedIndex = 0;
                Editors.SkillEditorInit();
            }
        }

        private void LstIndex_Click() => Editors.SkillEditorInit();
        private void BtnSave_Click() { Editors.SkillEditorOK(); Close(); }
        private void BtnCancel_Click() { Editors.SkillEditorCancel(); Close(); }
        private void BtnDelete_Click()
        {
            int tmpindex = lstIndex.SelectedIndex;
            Database.ClearSkill(GameState.EditorIndex);
            lstIndex.Items.RemoveAt(GameState.EditorIndex);
            lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Core.Data.Skill[GameState.EditorIndex].Name}" });
            lstIndex.SelectedIndex = tmpindex;
            Editors.SkillEditorInit();
        }
        private void BtnLearn_Click() => Sender.SendLearnSkill(GameState.EditorIndex);

        private void TxtName_TextChanged()
        {
            if (lstIndex.SelectedIndex < 0) return;
            int tmpindex = lstIndex.SelectedIndex;
            Core.Data.Skill[GameState.EditorIndex].Name = Strings.Trim(txtName.Text);
            lstIndex.Items.RemoveAt(GameState.EditorIndex);
            lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Core.Data.Skill[GameState.EditorIndex].Name}" });
            lstIndex.SelectedIndex = tmpindex;
        }
        private void CmbType_SelectedIndexChanged() => Core.Data.Skill[GameState.EditorIndex].Type = (byte)cmbType.SelectedIndex;
        private void NudMp_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].MpCost = (int)Math.Round(nudMp.Value);
        private void NudLevel_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].LevelReq = (int)Math.Round(nudLevel.Value);
        private void CmbAccessReq_SelectedIndexChanged() => Core.Data.Skill[GameState.EditorIndex].AccessReq = cmbAccessReq.SelectedIndex;
        private void CmbJob_SelectedIndexChanged() => Core.Data.Skill[GameState.EditorIndex].JobReq = cmbJob.SelectedIndex;
        private void NudCast_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].CastTime = (int)Math.Round(nudCast.Value);
        private void NudCool_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].CdTime = (int)Math.Round(nudCool.Value);
        private void NudIcon_ValueChanged() { Core.Data.Skill[GameState.EditorIndex].Icon = (int)Math.Round(nudIcon.Value); picSprite.Invalidate(); }
        private void NudMap_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].Map = (int)Math.Round(nudMap.Value);
        private void CmbDir_SelectedIndexChanged() => Core.Data.Skill[GameState.EditorIndex].Dir = (byte)cmbDir.SelectedIndex;
        private void NudX_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].X = (int)Math.Round(nudX.Value);
        private void NudY_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].Y = (int)Math.Round(nudY.Value);
        private void NudVital_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].Vital = (int)Math.Round(nudVital.Value);
        private void NudDuration_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].Duration = (int)Math.Round(nudDuration.Value);
        private void NudInterval_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].Interval = (int)Math.Round(nudInterval.Value);
        private void NudRange_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].Range = (int)Math.Round(nudRange.Value);
        private void ChkAoE_CheckedChanged() => Core.Data.Skill[GameState.EditorIndex].IsAoE = chkAoE.Checked == true;
        private void NudAoE_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].AoE = (int)Math.Round(nudAoE.Value);
        private void CmbAnimCast_SelectedIndexChanged() => Core.Data.Skill[GameState.EditorIndex].CastAnim = cmbAnimCast.SelectedIndex;
        private void CmbAnim_SelectedIndexChanged() => Core.Data.Skill[GameState.EditorIndex].SkillAnim = cmbAnim.SelectedIndex;
        private void NudStun_ValueChanged() => Core.Data.Skill[GameState.EditorIndex].StunDuration = (int)Math.Round(nudStun.Value);
        private void ChkProjectile_CheckedChanged() => Core.Data.Skill[GameState.EditorIndex].IsProjectile = chkProjectile.Checked == true ? 1 : 0;
        private void CmbProjectile_SelectedIndexChanged() => Core.Data.Skill[GameState.EditorIndex].Projectile = cmbProjectile.SelectedIndex;
    private void ChkKnockBack_CheckedChanged() => Core.Data.Skill[GameState.EditorIndex].KnockBack = (byte)(chkKnockBack.Checked == true ? 1 : 0);
        private void CmbKnockBackTiles_SelectedIndexChanged() => Core.Data.Skill[GameState.EditorIndex].KnockBackTiles = (byte)cmbKnockBackTiles.SelectedIndex;

        private void DrawIcon(Graphics g, int iconNum)
        {
            if (iconNum < 1 || iconNum > GameState.NumSkills)
            {
                g.Clear(Colors.Transparent);
                return;
            }
            var path = System.IO.Path.Combine(Core.Path.Skills, iconNum + GameState.GfxExt);
            if (!File.Exists(path)) { g.Clear(Colors.Transparent); return; }
            try
            {
                using (var bmp = new Bitmap(path))
                {
                    g.DrawImage(bmp, new RectangleF(0,0,picSprite.Width,picSprite.Height));
                }
            }
            catch { g.Clear(Colors.Transparent); }
        }

        public void DrawIcon() => picSprite.Invalidate();
    }
}