using Eto.Forms;
using Eto.Drawing;
using Microsoft.VisualBasic;
using Core;
using System;
using System.IO;
using Core.Globals;

namespace Client
{
    public class Editor_Resource : Form
    {
        // Singleton access for legacy usage
        private static Editor_Resource? _instance;
        public static Editor_Resource Instance => _instance ??= new Editor_Resource();
        public ListBox lstIndex = new ListBox();
        public TextBox txtName = new TextBox { Width = 200 };
        public TextBox txtMessage = new TextBox();
        public TextBox txtMessage2 = new TextBox();
        public ComboBox cmbType = new ComboBox();
        public NumericStepper nudNormalPic = new NumericStepper { MinValue = 0 };
        public NumericStepper nudExhaustedPic = new NumericStepper { MinValue = 0 };
        public ComboBox cmbRewardItem = new ComboBox();
        public NumericStepper nudRewardExp = new NumericStepper { MinValue = 0, MaxValue = 1000000 };
        public ComboBox cmbTool = new ComboBox();
        public NumericStepper nudHealth = new NumericStepper { MinValue = 0 };
        public NumericStepper nudRespawn = new NumericStepper { MinValue = 0, MaxValue = 1000000 };
        public ComboBox cmbAnimation = new ComboBox();
        public NumericStepper nudLvlReq = new NumericStepper { MinValue = 0 };
        public Button btnSave = new Button { Text = "Save" };
        public Button btnDelete = new Button { Text = "Delete" };
        public Button btnCancel = new Button { Text = "Cancel" };
        public Drawable picNormalpic = new Drawable { Size = new Size(96, 96) };
        public Drawable picExhaustedPic = new Drawable { Size = new Size(96, 96) };

        public Editor_Resource()
        {
            _instance = this;
            Title = "Resource Editor";
            ClientSize = new Size(760, 480);
            Padding = 10;
            InitializeComponent();
            Editors.AutoSizeWindow(this, 680, 440);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (ReferenceEquals(_instance, this)) _instance = null;
            Editors.ResourceEditorCancel();
        }

        private void InitializeComponent()
        {
            // Events wiring (converted from WinForms)
            lstIndex.SelectedIndexChanged += (s, e) => LstIndex_Click();
            txtName.TextChanged += (s, e) => TxtName_TextChanged();
            txtMessage.TextChanged += (s, e) => TxtMessage_TextChanged();
            txtMessage2.TextChanged += (s, e) => TxtMessage2_TextChanged();
            cmbType.SelectedIndexChanged += (s, e) => CmbType_SelectedIndexChanged();
            nudNormalPic.ValueChanged += (s, e) => NudNormalPic_ValueChanged();
            nudExhaustedPic.ValueChanged += (s, e) => NudExhaustedPic_ValueChanged();
            cmbRewardItem.SelectedIndexChanged += (s, e) => CmbRewardItem_SelectedIndexChanged();
            nudRewardExp.ValueChanged += (s, e) => NudRewardExp_ValueChanged();
            cmbTool.SelectedIndexChanged += (s, e) => CmbTool_SelectedIndexChanged();
            nudHealth.ValueChanged += (s, e) => NudHealth_ValueChanged();
            nudRespawn.ValueChanged += (s, e) => NudRespawn_ValueChanged();
            cmbAnimation.SelectedIndexChanged += (s, e) => CmbAnimation_SelectedIndexChanged();
            nudLvlReq.ValueChanged += (s, e) => NudLvlReq_ValueChanged();
            btnSave.Click += (s, e) => BtnSave_Click();
            btnDelete.Click += (s, e) => BtnDelete_Click();
            btnCancel.Click += (s, e) => BtnCancel_Click();
            Load += (s, e) => Editor_Resource_Load();

            picNormalpic.Paint += (s, e) => DrawSprite(e.Graphics, (int)Math.Round(nudNormalPic.Value), picNormalpic);
            picExhaustedPic.Paint += (s, e) => DrawSprite(e.Graphics, (int)Math.Round(nudExhaustedPic.Value), picExhaustedPic);

            // Layout definition
            var listLayout = new DynamicLayout { Spacing = new Size(5, 5) };
            listLayout.AddRow(new Label { Text = "Resources", Font = SystemFonts.Bold(12) });
            listLayout.Add(lstIndex, yscale: true);

            var imagesLayout = new TableLayout
            {
                Spacing = new Size(10, 10),
                Rows =
                {
                    new TableRow(new Label { Text = "Normal Pic:" }, nudNormalPic, picNormalpic,
                              new Label { Text = "Exhausted Pic:" }, nudExhaustedPic, picExhaustedPic)
                }
            };

            var rightLayout = new DynamicLayout { Spacing = new Size(5,5) };
            rightLayout.AddRow("Name:", txtName);
            rightLayout.AddRow("Success Msg:", txtMessage);
            rightLayout.AddRow("Empty Msg:", txtMessage2);
            rightLayout.AddRow("Type:", cmbType);
            rightLayout.Add(imagesLayout);
            rightLayout.AddRow("Reward Item:", cmbRewardItem);
            rightLayout.AddRow("Reward Exp:", nudRewardExp);
            rightLayout.AddRow("Tool Req:", cmbTool, "Animation:", cmbAnimation);
            rightLayout.AddRow("Health:", nudHealth, "Respawn:", nudRespawn);
            rightLayout.AddRow("Level Req:", nudLvlReq);
            // buttons moved to right panel bottom (main control view)
            rightLayout.Add(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 6, Items = { btnSave, btnDelete, btnCancel } }); // order enforced

            Content = new TableLayout
            {
                Spacing = new Size(10,10),
                Rows =
                {
                    new TableRow(new TableCell(listLayout, true), new TableCell(rightLayout, true))
                }
            };
        }

        private void Editor_Resource_Load()
        {
            lstIndex.Items.Clear();
            for (int i = 0; i < Constant.MaxResources; i++)
                lstIndex.Items.Add($"{i + 1}: {Data.Resource[i].Name}");

            cmbRewardItem.Items.Clear();
            for (int i = 0; i < Constant.MaxItems; i++)
                cmbRewardItem.Items.Add($"{i + 1}: {Data.Item[i].Name}");

            cmbAnimation.Items.Clear();
            for (int i = 0; i < Constant.MaxAnimations; i++)
                cmbAnimation.Items.Add($"{i + 1}: {Data.Animation[i].Name}");

            cmbType.Items.Clear();
            foreach (var name in Enum.GetValues(typeof(ResourceSkill)))
            {
                if (name is ToolType toolType && toolType != ToolType.None)
                {
                    cmbTool.Items.Add(name.ToString());
                }
            }
            

            // Populate from ToolType enum to stay in sync with core values
            cmbTool.Items.Clear();
            foreach (var name in Enum.GetNames(typeof(ToolType)))
                cmbTool.Items.Add(name);

            nudExhaustedPic.MaxValue = GameState.NumResources;
            nudNormalPic.MaxValue = GameState.NumResources;
            nudRespawn.MaxValue = 1000000;

            if (lstIndex.Items.Count > 0)
            {
                lstIndex.SelectedIndex = 0;
                Editors.ResourceEditorInit();
            }
        }

        private void LstIndex_Click() => Editors.ResourceEditorInit();
        private void BtnSave_Click() { Editors.ResourceEditorOK(); Close(); }
        private void BtnCancel_Click() { Editors.ResourceEditorCancel(); Close(); }

        private void BtnDelete_Click()
        {
            int tmpindex = lstIndex.SelectedIndex;
            MapResource.ClearResource(GameState.EditorIndex);
            lstIndex.Items.RemoveAt(GameState.EditorIndex);
            lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Data.Resource[GameState.EditorIndex].Name}" });
            lstIndex.SelectedIndex = tmpindex;
            Editors.ResourceEditorInit();
        }

        private void TxtName_TextChanged()
        {
            if (lstIndex.SelectedIndex < 0) return;
            int tmpindex = lstIndex.SelectedIndex;
            Data.Resource[GameState.EditorIndex].Name = txtName.Text;
            lstIndex.Items.RemoveAt(GameState.EditorIndex);
            lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Data.Resource[GameState.EditorIndex].Name}" });
            lstIndex.SelectedIndex = tmpindex;
        }

        private void TxtMessage_TextChanged() => Data.Resource[GameState.EditorIndex].SuccessMessage = Strings.Trim(txtMessage.Text);
        private void TxtMessage2_TextChanged() => Data.Resource[GameState.EditorIndex].EmptyMessage = Strings.Trim(txtMessage2.Text);
        private void CmbType_SelectedIndexChanged() => Data.Resource[GameState.EditorIndex].ResourceType = cmbType.SelectedIndex;
        private void NudNormalPic_ValueChanged() { Data.Resource[GameState.EditorIndex].ResourceImage = (int)Math.Round(nudNormalPic.Value); picNormalpic.Invalidate(); }
        private void NudExhaustedPic_ValueChanged() { Data.Resource[GameState.EditorIndex].ExhaustedImage = (int)Math.Round(nudExhaustedPic.Value); picExhaustedPic.Invalidate(); }
        private void CmbRewardItem_SelectedIndexChanged() => Data.Resource[GameState.EditorIndex].ItemReward = cmbRewardItem.SelectedIndex;
        private void NudRewardExp_ValueChanged() => Data.Resource[GameState.EditorIndex].ExpReward = (int)Math.Round(nudRewardExp.Value);
        private void CmbTool_SelectedIndexChanged() => Data.Resource[GameState.EditorIndex].ToolRequired = cmbTool.SelectedIndex;
        private void NudHealth_ValueChanged() => Data.Resource[GameState.EditorIndex].Health = (int)Math.Round(nudHealth.Value);
        private void NudRespawn_ValueChanged() => Data.Resource[GameState.EditorIndex].RespawnTime = (int)Math.Round(nudRespawn.Value);
        private void CmbAnimation_SelectedIndexChanged() => Data.Resource[GameState.EditorIndex].Animation = cmbAnimation.SelectedIndex;
        private void NudLvlReq_ValueChanged() => Data.Resource[GameState.EditorIndex].LvlRequired = (int)Math.Round(nudLvlReq.Value);

        private void DrawSprite(Graphics g, int spriteNum, Drawable target)
        {
            if (spriteNum < 1 || spriteNum > GameState.NumResources)
            {
                g.Clear(Colors.Transparent);
                return;
            }
            var path = System.IO.Path.Combine(DataPath.Resources, spriteNum + GameState.GfxExt);
            if (!File.Exists(path)) { g.Clear(Colors.Transparent); return; }
            try
            {
                using (var bmp = new Bitmap(path))
                {
                    int fw = bmp.Width / 4;
                    int fh = bmp.Height / 4;
                    target.Size = new Size(fw, fh);
                    g.DrawImage(bmp, new RectangleF(0,0,fw,fh), new Rectangle(0,0,fw,fh));
                }
            }
            catch { g.Clear(Colors.Transparent); }
        }
    }
}