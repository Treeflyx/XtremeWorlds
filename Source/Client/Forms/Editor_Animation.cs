using Core.Globals;
using Microsoft.VisualBasic;

using Eto.Forms;
using Eto.Drawing;
using System.IO;

namespace Client
{
    public class Editor_Animation : Form
    {
        // Singleton access for legacy usage
        private static Editor_Animation? _instance;
        public static Editor_Animation Instance => _instance ??= new Editor_Animation();
        private bool _suppressIndexChanged;
        public NumericStepper nudSprite0 = new();
        public NumericStepper nudSprite1 = new();
        public NumericStepper nudLoopCount0 = new();
        public NumericStepper nudLoopCount1 = new();
        public NumericStepper nudFrameCount0 = new();
        public NumericStepper nudFrameCount1 = new();
        public NumericStepper nudLoopTime0 = new();
        public NumericStepper nudLoopTime1 = new();
        public Button btnSave = new();
        public Button btnDelete = new();
        public Button btnCopy = new();
        public Button btnCancel = new();
        public TextBox txtName = new();
        public ListBox lstIndex = new() { Width = 200 };
        public ComboBox cmbSound = new();
        public Drawable picSprite0 = new() { Size = new Size(192, 192), MinimumSize = new Size(192, 192) };
        public Drawable picSprite1 = new() { Size = new Size(192, 192), MinimumSize = new Size(192, 192) };
        private Core.Globals.Type.Animation _clipboardAnim;
        private bool _hasClipboardAnim;

        public Editor_Animation()
        {
            _instance = this;
            Title = "Animation Editor";
            ClientSize = new Size(600, 400);
            Padding = 10;
            InitializeComponent();
            Editors.AutoSizeWindow(this, 560, 360);
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            if (ReferenceEquals(_instance, this)) _instance = null;
        }

        private void InitializeComponent()
        {
            // Subscribe Load first
            Load += Editor_Animation_Load;

            nudSprite0 = new NumericStepper { MinValue = 0, MaxValue = GameState.NumAnimations };
            nudSprite1 = new NumericStepper { MinValue = 0, MaxValue = GameState.NumAnimations };
            nudLoopCount0 = new NumericStepper();
            nudLoopCount1 = new NumericStepper();
            nudFrameCount0 = new NumericStepper();
            nudFrameCount1 = new NumericStepper();
            nudLoopTime0 = new NumericStepper();
            nudLoopTime1 = new NumericStepper();
            btnSave = new Button { Text = "Save" };
            btnDelete = new Button { Text = "Delete" };
            btnCancel = new Button { Text = "Cancel" };
            btnCopy = new Button { Text = "Copy" };
            txtName = new TextBox { Width = 200 };
            lstIndex = new ListBox { Width = 200 };
            cmbSound = new ComboBox();
            picSprite0 = new Drawable { Size = new Size(192, 192), MinimumSize = new Size(192, 192) };
            picSprite1 = new Drawable { Size = new Size(192, 192), MinimumSize = new Size(192, 192) };

            nudSprite0.ValueChanged += NudSprite0_ValueChanged;
            nudSprite1.ValueChanged += NudSprite1_ValueChanged;
            nudLoopCount0.ValueChanged += NudLoopCount0_ValueChanged;
            nudLoopCount1.ValueChanged += NudLoopCount1_ValueChanged;
            nudFrameCount0.ValueChanged += NudFrameCount0_ValueChanged;
            nudFrameCount1.ValueChanged += NudFrameCount1_ValueChanged;
            nudLoopTime0.ValueChanged += NudLoopTime0_ValueChanged;
            nudLoopTime1.ValueChanged += NudLoopTime1_ValueChanged;
            btnSave.Click += BtnSave_Click;
            btnDelete.Click += BtnDelete_Click;
            btnCancel.Click += BtnCancel_Click;
            btnCopy.Click += (s, e) => CopyOrPasteAnimation();
            txtName.TextChanged += TxtName_TextChanged;
            lstIndex.SelectedIndexChanged += (s, e) =>
            {
                if (_suppressIndexChanged) return;
                if (lstIndex.SelectedIndex >= 0)
                    GameState.EditorIndex = lstIndex.SelectedIndex;
                lstIndex_Click(s, e);
            };
            cmbSound.SelectedIndexChanged += CmbSound_SelectedIndexChanged;
            this.Closed += Editor_Animation_FormClosing;

            picSprite0.Paint += PicSprite0_Paint;
            picSprite1.Paint += PicSprite1_Paint;

            // Left side: index list with header
            var left = new DynamicLayout { Padding = 0, Spacing = new Size(5, 5) };
            left.AddRow(new Label { Text = "Animations", Font = SystemFonts.Bold(11) });
            left.Add(lstIndex, yscale: true);

            // Right side: animation properties
            var right = new DynamicLayout { Padding = 0, Spacing = new Size(5, 5) };
            right.AddRow("Name:", txtName);
            right.AddRow("Sprite 1:", nudSprite0, "Frames:", nudFrameCount0, "Loop Time:", nudLoopTime0, picSprite0);
            right.AddRow("Sprite 2:", nudSprite1, "Frames:", nudFrameCount1, "Loop Time:", nudLoopTime1, picSprite1);
            right.AddRow("Loop Count 1:", nudLoopCount0, "Loop Count 2:", nudLoopCount1);
            right.AddRow("Sound:", cmbSound);
            right.AddRow(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 6, Items = { btnSave, btnDelete, btnCopy, btnCancel } });

            var root = new TableLayout
            {
                Spacing = new Size(10, 10),
                Padding = new Padding(10),
                Rows =
                {
                    new TableRow(left, right)
                }
            };

            Content = root;
        }

        private void NudSprite0_ValueChanged(object? sender, EventArgs e)
        {
            EnsureAnimationArrays(ref Data.Animation[GameState.EditorIndex]);
            Data.Animation[GameState.EditorIndex].Sprite[0] = (int)Math.Round(nudSprite0.Value);
            UpdatePreviewSize(picSprite0, nudSprite0, nudFrameCount0);
            picSprite0.Invalidate();
        }

        private void NudSprite1_ValueChanged(object? sender, EventArgs e)
        {
            EnsureAnimationArrays(ref Data.Animation[GameState.EditorIndex]);
            Data.Animation[GameState.EditorIndex].Sprite[1] = (int)Math.Round(nudSprite1.Value);
            UpdatePreviewSize(picSprite1, nudSprite1, nudFrameCount1);
            picSprite1.Invalidate();
        }

        private void NudLoopCount0_ValueChanged(object? sender, EventArgs e)
        {
            EnsureAnimationArrays(ref Data.Animation[GameState.EditorIndex]);
            Data.Animation[GameState.EditorIndex].LoopCount[0] = (int)Math.Round(nudLoopCount0.Value);
        }

        private void NudLoopCount1_ValueChanged(object? sender, EventArgs e)
        {
            EnsureAnimationArrays(ref Data.Animation[GameState.EditorIndex]);
            Data.Animation[GameState.EditorIndex].LoopCount[1] = (int)Math.Round(nudLoopCount1.Value);
        }

        private void NudFrameCount0_ValueChanged(object? sender, EventArgs e)
        {
            EnsureAnimationArrays(ref Data.Animation[GameState.EditorIndex]);
            Data.Animation[GameState.EditorIndex].Frames[0] = (int)Math.Round(nudFrameCount0.Value);
            UpdatePreviewSize(picSprite0, nudSprite0, nudFrameCount0);
            picSprite0.Invalidate();
        }

        private void NudFrameCount1_ValueChanged(object? sender, EventArgs e)
        {
            EnsureAnimationArrays(ref Data.Animation[GameState.EditorIndex]);
            Data.Animation[GameState.EditorIndex].Frames[1] = (int)Math.Round(nudFrameCount1.Value);
            UpdatePreviewSize(picSprite1, nudSprite1, nudFrameCount1);
            picSprite1.Invalidate();
        }

        private void NudLoopTime0_ValueChanged(object? sender, EventArgs e)
        {
            EnsureAnimationArrays(ref Data.Animation[GameState.EditorIndex]);
            Data.Animation[GameState.EditorIndex].LoopTime[0] = (int)Math.Round(nudLoopTime0.Value);
        }

        private void NudLoopTime1_ValueChanged(object? sender, EventArgs e)
        {
            EnsureAnimationArrays(ref Data.Animation[GameState.EditorIndex]);
            Data.Animation[GameState.EditorIndex].LoopTime[1] = (int)Math.Round(nudLoopTime1.Value);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            Editors.AnimationEditorOK();
            Close();
        }

        private void TxtName_TextChanged(object? sender, EventArgs e)
        {
            int tmpindex;
            tmpindex = lstIndex.SelectedIndex;
            Data.Animation[GameState.EditorIndex].Name = Strings.Trim(txtName.Text);
            _suppressIndexChanged = true;
            try
            {
                lstIndex.Items.RemoveAt(GameState.EditorIndex);
                lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Data.Animation[GameState.EditorIndex].Name}" });
                lstIndex.SelectedIndex = tmpindex;
            }
            finally { _suppressIndexChanged = false; }
        }

        private void lstIndex_Click(object? sender, EventArgs e)
        {
            Editors.AnimationEditorInit();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            int tmpindex;

            Animation.ClearAnimation(GameState.EditorIndex);

            tmpindex = lstIndex.SelectedIndex;
            _suppressIndexChanged = true;
            try
            {
                lstIndex.Items.RemoveAt(GameState.EditorIndex);
                lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Data.Animation[GameState.EditorIndex].Name}" });
                lstIndex.SelectedIndex = tmpindex;
            }
            finally { _suppressIndexChanged = false; }

            Editors.AnimationEditorInit();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            Editors.AnimationEditorCancel();
            Close();
        }

        private void Editor_Animation_Load(object? sender, EventArgs e)
        {
            _suppressIndexChanged = true;
            try
            {
                lstIndex.Items.Clear();

                // Add the names
                for (int i = 0; i < Constant.MaxAnimations; i++)
                    lstIndex.Items.Add(i + 1 + ": " + Data.Animation[i].Name);
                lstIndex.SelectedIndex = GameState.EditorIndex >= 0 ? GameState.EditorIndex : 0;

                // find the music we have set
                cmbSound.Items.Clear();

                General.CacheSound();

                for (int i = 0, loopTo = Information.UBound(Sound.SoundCache); i < loopTo; i++)
                    cmbSound.Items.Add(Sound.SoundCache[i]);

                nudSprite0.MaxValue = GameState.NumAnimations;
                nudSprite1.MaxValue = GameState.NumAnimations;
            }
            finally { _suppressIndexChanged = false; }

            Editors.AnimationEditorInit();

            // After init, ensure previews match the actual frame size
            UpdatePreviewSize(picSprite0, nudSprite0, nudFrameCount0);
            UpdatePreviewSize(picSprite1, nudSprite1, nudFrameCount1);
            picSprite0.Invalidate();
            picSprite1.Invalidate();
        }

        private void CopyOrPasteAnimation()
        {
            int src = GameState.EditorIndex;
            if (!_hasClipboardAnim)
            {
                if (src < 0 || src >= Constant.MaxAnimations) return;
                var a = Data.Animation[src];
                _clipboardAnim = a; // struct copy
                if (a.Sprite != null) _clipboardAnim.Sprite = (int[])a.Sprite.Clone();
                if (a.Frames != null) _clipboardAnim.Frames = (int[])a.Frames.Clone();
                if (a.LoopCount != null) _clipboardAnim.LoopCount = (int[])a.LoopCount.Clone();
                if (a.LoopTime != null) _clipboardAnim.LoopTime = (int[])a.LoopTime.Clone();
                _hasClipboardAnim = true;
                btnCopy.Text = "Paste";
                return;
            }

            int def = GameState.EditorIndex + 1;
            var oneBased = Editors.PromptIndex(this, "Paste Animation", $"Paste animation into index (1..{Constant.MaxAnimations}):", 1, Constant.MaxAnimations, def);
            if (oneBased == null) return;
            int dst = oneBased.Value - 1;
            var n = _clipboardAnim;
            if (n.Sprite != null) n.Sprite = (int[])n.Sprite.Clone();
            if (n.Frames != null) n.Frames = (int[])n.Frames.Clone();
            if (n.LoopCount != null) n.LoopCount = (int[])n.LoopCount.Clone();
            if (n.LoopTime != null) n.LoopTime = (int[])n.LoopTime.Clone();
            EnsureAnimationArrays(ref n);
            Data.Animation[dst] = n;
            GameState.AnimationChanged[dst] = true;

            if (lstIndex != null && dst >= 0 && dst < lstIndex.Items.Count)
            {
                _suppressIndexChanged = true;
                try
                {
                    lstIndex.Items.RemoveAt(dst);
                    lstIndex.Items.Insert(dst, new ListItem { Text = $"{dst + 1}: {Data.Animation[dst].Name}" });
                    lstIndex.SelectedIndex = dst;
                }
                finally { _suppressIndexChanged = false; }
            }
            GameState.EditorIndex = dst;
            Editors.AnimationEditorInit();
        }

        private void CmbSound_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Store the actual selected sound text, not the index
            var sel = cmbSound.SelectedValue as ListItem;
            var text = sel?.Text ?? cmbSound.SelectedValue?.ToString() ?? string.Empty;
            Data.Animation[GameState.EditorIndex].Sound = text;
        }

        private void Editor_Animation_FormClosing(object? sender, EventArgs e)
        {
            Editors.AnimationEditorCancel();
        }

        public void ProcessAnimation(Graphics graphics, NumericStepper animationControl, NumericStepper frameCountControl, NumericStepper loopCountControl, int animationTimerIndex, Drawable drawable)
        {
            try
            {
                int animationNum = (int)Math.Round(animationControl.Value);
                if (animationNum <= 0 || animationNum > GameState.NumAnimations)
                {
                    // Clear area
                    graphics.Clear(Colors.Transparent);
                    return;
                }

                var imagePath = System.IO.Path.Combine(DataPath.Animations, animationNum + GameState.GfxExt);
                if (!System.IO.File.Exists(imagePath))
                {
                    graphics.Clear(Colors.Transparent);
                    return;
                }

                using (var img = new Bitmap(imagePath))
                {
                    int columns = (int)Math.Round(frameCountControl.Value);
                    graphics.Clear(Colors.Transparent);

                    // Determine source frame rectangle
                    Rectangle srcRect;
                    if (columns <= 0)
                    {
                        // No columns specified; treat the whole image as a single frame
                        srcRect = new Rectangle(0, 0, img.Width, img.Height);
                    }
                    else
                    {
                        int frameWidth = Math.Max(1, img.Width / columns);
                        // Dynamic division for height: infer square frames; if not tall enough, fall back
                        int inferredRows = frameWidth > 0 ? img.Height / frameWidth : 0;
                        int frameHeight = inferredRows > 0 ? frameWidth : img.Height;
                        srcRect = new Rectangle(0, 0, frameWidth, frameHeight); // first frame only
                    }

                    // Compute destination rectangle: native size (no upscaling), centered and clipped
                    var bounds = drawable.Size;
                    int drawW = Math.Min(srcRect.Width, bounds.Width);
                    int drawH = Math.Min(srcRect.Height, bounds.Height);
                    int offX = (bounds.Width - drawW) / 2;
                    int offY = (bounds.Height - drawH) / 2;
                    var destRect = new RectangleF(offX, offY, drawW, drawH);

                    graphics.DrawImage(img, destRect, srcRect);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing animation: {ex.Message}");
                graphics.Clear(Colors.Transparent);
            }
        }

        private static void EnsureAnimationArrays(ref Core.Globals.Type.Animation a)
        {
            if (a.Sprite == null || a.Sprite.Length < 2) a.Sprite = a.Sprite != null ? Resize(a.Sprite, 2) : new int[2];
            if (a.Frames == null || a.Frames.Length < 2) a.Frames = a.Frames != null ? Resize(a.Frames, 2) : new int[2];
            if (a.LoopCount == null || a.LoopCount.Length < 2) a.LoopCount = a.LoopCount != null ? Resize(a.LoopCount, 2) : new int[2];
            if (a.LoopTime == null || a.LoopTime.Length < 2) a.LoopTime = a.LoopTime != null ? Resize(a.LoopTime, 2) : new int[2];
            if (a.LoopCount[0] == 0) a.LoopCount[0] = 1;
            if (a.LoopCount[1] == 0) a.LoopCount[1] = 1;
            if (a.LoopTime[0] == 0) a.LoopTime[0] = 1;
            if (a.LoopTime[1] == 0) a.LoopTime[1] = 1;
        }

        private static int[] Resize(int[] arr, int len)
        {
            var n = new int[len];
            Array.Copy(arr, n, Math.Min(arr.Length, len));
            return n;
        }

    private void UpdatePreviewSize(Drawable drawable, NumericStepper animationControl, NumericStepper frameCountControl)
        {
            try
            {
                int animationNum = (int)Math.Round(animationControl.Value);
                if (animationNum <= 0 || animationNum > GameState.NumAnimations)
                {
                    // fallback size
                    return;
                }

                var imagePath = Path.Combine(DataPath.Animations, animationNum + GameState.GfxExt);
                if (!File.Exists(imagePath))
                {
                    return;
                }

                using (var img = new Bitmap(imagePath))
                {
                    int columns = (int)Math.Round(frameCountControl.Value);
                    int frameWidth = columns > 0 ? Math.Max(1, img.Width / columns) : img.Width;
                    int inferredRows = frameWidth > 0 ? img.Height / frameWidth : 0;
                    int frameHeight = columns > 0 ? (inferredRows > 0 ? frameWidth : img.Height) : img.Height;
                    var newSize = new Size(192, 192);
                    if (drawable.Size != newSize)
                    {
                        drawable.Size = newSize;
                        drawable.MinimumSize = newSize;
                    }
                }
            }
            catch
            {
                // ignore sizing errors; keep current size
            }
        }

        private void PicSprite0_Paint(object? sender, PaintEventArgs e)
        {
            ProcessAnimation(e.Graphics, nudSprite0, nudFrameCount0, nudLoopTime0, 0, picSprite0);
        }

        private void PicSprite1_Paint(object? sender, PaintEventArgs e)
        {
            ProcessAnimation(e.Graphics, nudSprite1, nudFrameCount1, nudLoopTime1, 1, picSprite1);
        }
    }
}