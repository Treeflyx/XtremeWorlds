using Core.Globals;
using Microsoft.VisualBasic;

using Eto.Forms;
using Eto.Drawing;

namespace Client
{
    public class Editor_Animation : Form
    {
        // Singleton access for legacy usage
        private static Editor_Animation? _instance;
        public static Editor_Animation Instance => _instance ??= new Editor_Animation();

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
    public Button btnCancel = new();
    public TextBox txtName = new();
    public ListBox lstIndex = new();
    public ComboBox cmbSound = new();
    public Drawable picSprite0 = new() { Size = new Size(96,96) };
    public Drawable picSprite1 = new() { Size = new Size(96,96) };

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
            txtName = new TextBox();
            lstIndex = new ListBox();
            cmbSound = new ComboBox();
            picSprite0 = new Drawable { Size = new Size(96, 96) };
            picSprite1 = new Drawable { Size = new Size(96, 96) };

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
            txtName.TextChanged += TxtName_TextChanged;
            lstIndex.SelectedIndexChanged += lstIndex_Click;
            cmbSound.SelectedIndexChanged += CmbSound_SelectedIndexChanged;
            this.Closed += Editor_Animation_FormClosing;

            picSprite0.Paint += PicSprite0_Paint;
            picSprite1.Paint += PicSprite1_Paint;

            // Left side: index list with header
            var left = new DynamicLayout { Padding = 0, Spacing = new Size(5,5) };
            left.AddRow(new Label{ Text = "Animations", Font = SystemFonts.Bold(11)});
            left.Add(lstIndex, yscale: true);

            // Right side: animation properties
            var right = new DynamicLayout { Padding = 0, Spacing = new Size(5,5) };
            right.AddRow("Name:", txtName);
            right.AddRow("Sprite 0:", nudSprite0, "Frames:", nudFrameCount0, "Loop Time:", nudLoopTime0, picSprite0);
            right.AddRow("Sprite 1:", nudSprite1, "Frames:", nudFrameCount1, "Loop Time:", nudLoopTime1, picSprite1);
            right.AddRow("Loop Count 0:", nudLoopCount0, "Loop Count 1:", nudLoopCount1);
            right.AddRow("Sound:", cmbSound);
            right.AddRow(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 6, Items = { btnSave, btnDelete, btnCancel } });

            var root = new TableLayout
            {
                Spacing = new Size(10,10),
                Padding = new Padding(10),
                Rows =
                {
                    new TableRow(left, right)
                }
            };

            Content = root;
            Load += Editor_Animation_Load;
        }

    private void NudSprite0_ValueChanged(object? sender, EventArgs e)
        {
            Data.Animation[GameState.EditorIndex].Sprite[0] = (int)Math.Round(nudSprite0.Value);
        }

    private void NudSprite1_ValueChanged(object? sender, EventArgs e)
        {
            Data.Animation[GameState.EditorIndex].Sprite[1] = (int)Math.Round(nudSprite1.Value);
        }

    private void NudLoopCount0_ValueChanged(object? sender, EventArgs e)
        {
            Data.Animation[GameState.EditorIndex].LoopCount[0] = (int)Math.Round(nudLoopCount0.Value);
        }

    private void NudLoopCount1_ValueChanged(object? sender, EventArgs e)
        {
            Data.Animation[GameState.EditorIndex].LoopCount[1] = (int)Math.Round(nudLoopCount1.Value);
        }

    private void NudFrameCount0_ValueChanged(object? sender, EventArgs e)
        {
            Data.Animation[GameState.EditorIndex].Frames[0] = (int)Math.Round(nudFrameCount0.Value);
        }

    private void NudFrameCount1_ValueChanged(object? sender, EventArgs e)
        {
            Data.Animation[GameState.EditorIndex].Frames[1] = (int)Math.Round(nudFrameCount1.Value);
        }

    private void NudLoopTime0_ValueChanged(object? sender, EventArgs e)
        {
            Data.Animation[GameState.EditorIndex].LoopTime[0] = (int)Math.Round(nudLoopTime0.Value);
        }

    private void NudLoopTime1_ValueChanged(object? sender, EventArgs e)
        {
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
            lstIndex.Items.RemoveAt(GameState.EditorIndex);
            lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Data.Animation[GameState.EditorIndex].Name}" });
            lstIndex.SelectedIndex = tmpindex;
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
            lstIndex.Items.RemoveAt(GameState.EditorIndex);
            lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Data.Animation[GameState.EditorIndex].Name}" });
            lstIndex.SelectedIndex = tmpindex;

            Editors.AnimationEditorInit();
        }

    private void BtnCancel_Click(object? sender, EventArgs e)
        {
            Editors.AnimationEditorCancel();
            Close();
        }

    private void Editor_Animation_Load(object? sender, EventArgs e)
        {
            lstIndex.Items.Clear();

            // Add the names
            for (int i = 0; i < Constant.MaxAnimations; i++)
                lstIndex.Items.Add(i + 1 + ": " + Data.Animation[i].Name);

            // find the music we have set
            cmbSound.Items.Clear();

            General.CacheSound();

            for (int i = 0, loopTo = Information.UBound(Sound.SoundCache); i < loopTo; i++)
                cmbSound.Items.Add(Sound.SoundCache[i]);

            nudSprite0.MaxValue = GameState.NumAnimations;
            nudSprite1.MaxValue = GameState.NumAnimations;
        }

    private void CmbSound_SelectedIndexChanged(object? sender, EventArgs e)
        {
            Data.Animation[GameState.EditorIndex].Sound = cmbSound.SelectedIndex.ToString();
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
                    if (columns <= 0)
                    {
                        graphics.DrawImage(img, 0, 0, drawable.Width, drawable.Height);
                        return;
                    }

                    int frameWidth = img.Width / columns;
                    int frameHeight = img.Height;
                    int rows = frameHeight > 0 ? img.Height / frameHeight : 1;
                    int frameCount = rows * columns;

                    int looptime = (int)Math.Round(loopCountControl.Value);
                    if (GameState.AnimEditorTimer[animationTimerIndex] + looptime <= Environment.TickCount)
                    {
                        if (GameState.AnimEditorFrame[animationTimerIndex] >= frameCount)
                        {
                            GameState.AnimEditorFrame[animationTimerIndex] = 1;
                        }
                        else
                        {
                            GameState.AnimEditorFrame[animationTimerIndex] += 1;
                        }
                        GameState.AnimEditorTimer[animationTimerIndex] = Environment.TickCount;
                    }

                    if (frameCountControl.Value > 0)
                    {
                        int frameIndex = GameState.AnimEditorFrame[animationTimerIndex] - 1;
                        int column = frameIndex % columns;
                        int row = frameIndex / columns;

                        var srcRect = new Eto.Drawing.Rectangle(column * frameWidth, row * frameHeight, frameWidth, frameHeight);
                        var destRect = new Eto.Drawing.RectangleF(0, 0, drawable.Width, drawable.Height);
                        graphics.Clear(Colors.Transparent);
                        graphics.DrawImage(img, destRect, srcRect);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing animation: {ex.Message}");
                graphics.Clear(Colors.Transparent);
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