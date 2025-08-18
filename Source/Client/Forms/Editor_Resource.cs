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
        private bool _suppressIndexChanged;
        public ListBox lstIndex = new ListBox { Width = 200 };
        public TextBox txtName = new TextBox { Width = 200 };
        public TextBox txtMessage = new TextBox { Width = 200 };
        public TextBox txtMessage2 = new TextBox { Width = 200 };
        public ComboBox cmbType = new ComboBox { Width = 200 };
        public NumericStepper nudNormalPic = new NumericStepper { MinValue = 0, Width = 80 };
        public NumericStepper nudExhaustedPic = new NumericStepper { MinValue = 0, Width = 80 };
        public ComboBox cmbRewardItem = new ComboBox { Width = 200 };
        public NumericStepper nudRewardExp = new NumericStepper { MinValue = 0, MaxValue = 1000000, Width = 120 };
        public ComboBox cmbTool = new ComboBox { Width = 200 };
        public NumericStepper nudHealth = new NumericStepper { MinValue = 0, Width = 100 };
        public NumericStepper nudRespawn = new NumericStepper { MinValue = 0, MaxValue = 1000000, Width = 120 };
        public ComboBox cmbAnimation = new ComboBox { Width = 200 };
        public NumericStepper nudLvlReq = new NumericStepper { MinValue = 0, Width = 100 };
        public Button btnSave = new Button { Text = "Save" };
        public Button btnDelete = new Button { Text = "Delete" };
        public Button btnCopy = new Button { Text = "Copy" };
        public Button btnCancel = new Button { Text = "Cancel" };
        private Core.Globals.Type.Resource _clipboardResource;
        private bool _hasClipboardResource;
        public Drawable picNormalpic = new Drawable { Size = new Size(150, 130), MinimumSize = new Size(150, 130) };
        public Drawable picExhaustedPic = new Drawable { Size = new Size(150, 130), MinimumSize = new Size(150, 130) };

        public Editor_Resource()
        {
            _instance = this;
            Title = "Resource Editor";
            ClientSize = new Size(1000, 680);
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
            // Subscribe Load first
            Load += (s, e) => Editor_Resource_Load();

            // Events wiring (converted from WinForms)
            lstIndex.SelectedIndexChanged += (s, e) =>
            {
                if (_suppressIndexChanged) return;
                if (lstIndex.SelectedIndex >= 0)
                    GameState.EditorIndex = lstIndex.SelectedIndex;
                LstIndex_Click();
            };
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
            btnCopy.Click += (s, e) =>
            {
                int src = GameState.EditorIndex;
                if (!_hasClipboardResource)
                {
                    if (src < 0 || src >= Constant.MaxResources) return;
                    _clipboardResource = Data.Resource[src];
                    _hasClipboardResource = true;
                    btnCopy.Text = "Paste";
                    return;
                }
                int def = GameState.EditorIndex + 1;
                var oneBased = Editors.PromptIndex(this, "Paste Resource", $"Paste resource into index (1..{Constant.MaxResources}):", 1, Constant.MaxResources, def);
                if (oneBased == null) return;
                int dst = oneBased.Value - 1;
                var nRes = _clipboardResource;
                Data.Resource[dst] = nRes;
                GameState.ResourceChanged[dst] = true;
                _suppressIndexChanged = true;
                try
                {
                    lstIndex.Items.RemoveAt(dst);
                    lstIndex.Items.Insert(dst, new ListItem { Text = $"{dst + 1}: {Data.Resource[dst].Name}" });
                    lstIndex.SelectedIndex = dst;
                }
                finally { _suppressIndexChanged = false; }
                Editors.ResourceEditorInit();
            };
            btnCancel.Click += (s, e) => BtnCancel_Click();


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
            new TableRow(new Label { Text = "Normal:" }, nudNormalPic, picNormalpic,
                  new Label { Text = "Exhausted:" }, nudExhaustedPic, picExhaustedPic)
                }
            };

            var rightLayout = new DynamicLayout { Spacing = new Size(5,5), Padding = new Padding(0) };
            // Top fields in a dedicated 2-column grid with a scaling spacer column
            var fieldsLayout = new TableLayout
            {
                Spacing = new Size(5, 6),
                Rows =
                {
                    new TableRow(
                        new TableCell(new Label { Text = "Name:", TextAlignment = TextAlignment.Left }, scaleWidth: false),
                        new TableCell(txtName, scaleWidth: false),
                        new TableCell(null, scaleWidth: true)
                    ),
                    new TableRow(
                        new TableCell(new Label { Text = "Success Msg:", TextAlignment = TextAlignment.Left }, scaleWidth: false),
                        new TableCell(txtMessage, scaleWidth: false),
                        new TableCell(null, scaleWidth: true)
                    ),
                    new TableRow(
                        new TableCell(new Label { Text = "Empty Msg:", TextAlignment = TextAlignment.Left }, scaleWidth: false),
                        new TableCell(txtMessage2, scaleWidth: false),
                        new TableCell(null, scaleWidth: true)
                    ),
                    new TableRow(
                        new TableCell(new Label { Text = "Type:", TextAlignment = TextAlignment.Left }, scaleWidth: false),
                        new TableCell(cmbType, scaleWidth: false),
                        new TableCell(null, scaleWidth: true)
                    )
                }
            };
            rightLayout.Add(fieldsLayout);
            rightLayout.Add(imagesLayout);

            // Remaining fields in another table to keep pairs tight to labels
            var detailsLayout = new TableLayout
            {
                Spacing = new Size(5, 6),
                Rows =
                {
                    // Reward Item
                    new TableRow(
                        new TableCell(new Label { Text = "Reward Item:", TextAlignment = TextAlignment.Left }, scaleWidth: false),
                        new TableCell(cmbRewardItem, scaleWidth: false),
                        new TableCell(null, scaleWidth: true)
                    ),
                    // Reward Exp
                    new TableRow(
                        new TableCell(new Label { Text = "Reward Exp:", TextAlignment = TextAlignment.Left }, scaleWidth: false),
                        new TableCell(nudRewardExp, scaleWidth: false),
                        new TableCell(null, scaleWidth: true)
                    ),
                    // Tool Req / Animation
                    new TableRow(
                        new TableCell(new Label { Text = "Tool Req:", TextAlignment = TextAlignment.Left }, scaleWidth: false),
                        new TableCell(cmbTool, scaleWidth: false),
                        new TableCell(new Label { Text = "Animation:", TextAlignment = TextAlignment.Left }, scaleWidth: false),
                        new TableCell(cmbAnimation, scaleWidth: false),
                        new TableCell(null, scaleWidth: true)
                    ),
                    // Health / Respawn
                    new TableRow(
                        new TableCell(new Label { Text = "Health:", TextAlignment = TextAlignment.Left }, scaleWidth: false),
                        new TableCell(nudHealth, scaleWidth: false),
                        new TableCell(new Label { Text = "Respawn:", TextAlignment = TextAlignment.Left }, scaleWidth: false),
                        new TableCell(nudRespawn, scaleWidth: false),
                        new TableCell(null, scaleWidth: true)
                    ),
                    // Level Req
                    new TableRow(
                        new TableCell(new Label { Text = "Level Req:", TextAlignment = TextAlignment.Left }, scaleWidth: false),
                        new TableCell(nudLvlReq, scaleWidth: false),
                        new TableCell(null, scaleWidth: true)
                    )
                }
            };
            rightLayout.Add(detailsLayout);

            // buttons moved to right panel bottom (main control view)
            rightLayout.Add(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 6, HorizontalContentAlignment = HorizontalAlignment.Left, Items = { btnSave, btnDelete, btnCopy, btnCancel } }); // order enforced

        Content = new TableLayout
            {
                Spacing = new Size(10,10),
                Rows =
                {
            // Left list pane fixed to preferred width (list is set to 200px)
            new TableRow(new TableCell(listLayout), new TableCell(rightLayout, true))
                }
            };
        }

        private void Editor_Resource_Load()
        {
            _suppressIndexChanged = true;
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
                cmbType.Items.Add(name.ToString());
            }

            cmbTool.Items.Clear();
            foreach (var name in Enum.GetNames(typeof(ToolType)))
                cmbTool.Items.Add(name);

            nudExhaustedPic.MaxValue = GameState.NumResources;
            nudNormalPic.MaxValue = GameState.NumResources;
            nudRespawn.MaxValue = 1000000;

            // Select current index without triggering handlers
            if (lstIndex.Items.Count > 0)
            {
                var idx = GameState.EditorIndex;
                if (idx < 0 || idx >= lstIndex.Items.Count) idx = 0;
                lstIndex.SelectedIndex = idx;
            }
            _suppressIndexChanged = false;
            // Initialize once post-population
            if (lstIndex.SelectedIndex >= 0)
                Editors.ResourceEditorInit();
        }

        private void LstIndex_Click() => Editors.ResourceEditorInit();
        private void BtnSave_Click() { Editors.ResourceEditorOK(); Close(); }
        private void BtnCancel_Click() { Editors.ResourceEditorCancel(); Close(); }

        private void BtnDelete_Click()
        {
            int tmpindex = lstIndex.SelectedIndex;
            MapResource.ClearResource(GameState.EditorIndex);
            _suppressIndexChanged = true;
            lstIndex.Items.RemoveAt(GameState.EditorIndex);
            lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Data.Resource[GameState.EditorIndex].Name}" });
            lstIndex.SelectedIndex = tmpindex;
            _suppressIndexChanged = false;
            Editors.ResourceEditorInit();
        }

        private void TxtName_TextChanged()
        {
            if (lstIndex.SelectedIndex < 0) return;
            int tmpindex = lstIndex.SelectedIndex;
            Data.Resource[GameState.EditorIndex].Name = txtName.Text;
            _suppressIndexChanged = true;
            lstIndex.Items.RemoveAt(GameState.EditorIndex);
            lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Data.Resource[GameState.EditorIndex].Name}" });
            lstIndex.SelectedIndex = tmpindex;
            _suppressIndexChanged = false;
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

            var bounds = target.Size;
            int fw = Math.Min(bmp.Width, bounds.Width);
            int fh = Math.Min(bmp.Height, bounds.Height);
            int ox = (bounds.Width - fw) / 2;
            int oy = (bounds.Height - fh) / 2;
            g.Clear(Colors.Transparent);
            g.DrawImage(bmp, new RectangleF(ox, oy, fw, fh), new Rectangle(0, 0, fw, fh));
                }
            }
            catch { g.Clear(Colors.Transparent); }
        }
    }
}