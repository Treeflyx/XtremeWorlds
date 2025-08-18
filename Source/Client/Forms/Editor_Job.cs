// Clean Eto.Forms implementation of Job Editor.
using System;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using Core;
using Core.Globals;
using Microsoft.VisualBasic;

namespace Client
{
    public class Editor_Job : Form
    {
    // Singleton access to mirror legacy usage pattern in Main.cs
    private static Editor_Job? _instance;
    public static Editor_Job Instance => _instance ??= new Editor_Job();
    private bool _suppressIndexChanged;
    // Copy/Paste clipboard for jobs
    private Core.Globals.Type.Job _clipboardJob;
    private bool _hasClipboardJob;

    public ListBox? lstJobs, lstStartItems;
    public TextBox? txtName;
    public TextArea? txtDesc;
    public TextArea txtDescription => txtDesc!;
    public ComboBox? cmbItems;
    public NumericStepper? numStr, numLck, numEnd, numInt, numVit, numSpr, numBaseExp;
    public NumericStepper? numStartMap, numStartX, numStartY;
    public NumericStepper? numMaleSprite, numFemaleSprite, numItemAmount;
    public Button? btnSetItem, btnSave, btnDelete, btnCopy, btnCancel;
    public Drawable? malePreview, femalePreview;
    Bitmap? maleBmp, femaleBmp;

    // Legacy compatibility: expose primary list as lstIndex expected by existing logic
    public ListBox lstIndex => lstJobs!; // legacy name

    // Legacy alias properties expected by Editors.cs (nud* naming convention)
    public NumericStepper nudStrength => numStr!;
    public NumericStepper nudLuck => numLck!;
    public NumericStepper nudIntelligence => numInt!;
    public NumericStepper nudVitality => numVit!;
    public NumericStepper nudSpirit => numSpr!;
    public NumericStepper nudBaseExp => numBaseExp!;
    public NumericStepper nudStartMap => numStartMap!;
    public NumericStepper nudStartX => numStartX!;
    public NumericStepper nudStartY => numStartY!;
    public NumericStepper nudMaleSprite => numMaleSprite!;
    public NumericStepper nudFemaleSprite => numFemaleSprite!;

    public Editor_Job()
    {
        _instance = this;
        Title = "Job Editor";
        ClientSize = new Size(720, 500);
        Padding = 6;
            // Ensure Load is subscribed first before building UI and wiring events
            Load += (s, e) => InitData();
        Content = BuildUi();
        Editors.AutoSizeWindow(this, 600, 420);
    }

    Eto.Forms.Control BuildUi()
    {
        // Primary lists and fields
        lstJobs = new ListBox { Width = 200 };
        lstJobs.SelectedIndexChanged += (s, e) =>
        {
            if (_suppressIndexChanged) return;
            if (lstJobs.SelectedIndex >= 0)
                GameState.EditorIndex = lstJobs.SelectedIndex;
            ChangeJob();
        };

        txtName = new TextBox { Width = 200 }; txtName.TextChanged += (s, e) => UpdateName();
        txtDesc = new TextArea { Size = new Size(200, 120) }; txtDesc.TextChanged += (s, e) => Data.Job[GameState.EditorIndex].Desc = txtDesc.Text;

        // Stats
        numStr = Stat(); numStr.ValueChanged += (s, e) => SetStat(Core.Globals.Stat.Strength, numStr);
        numLck = Stat(); numLck.ValueChanged += (s, e) => SetStat(Core.Globals.Stat.Luck, numLck);
        numEnd = Stat(); numEnd.ValueChanged += (s, e) => SetStat(Core.Globals.Stat.Luck, numEnd); // original mapping
        numInt = Stat(); numInt.ValueChanged += (s, e) => SetStat(Core.Globals.Stat.Intelligence, numInt);
        numVit = Stat(); numVit.ValueChanged += (s, e) => SetStat(Core.Globals.Stat.Vitality, numVit);
        numSpr = Stat(); numSpr.ValueChanged += (s, e) => SetStat(Core.Globals.Stat.Spirit, numSpr);
        numBaseExp = new NumericStepper { MinValue = 0, MaxValue = 5_000_000, Increment = 10 }; numBaseExp.ValueChanged += (s, e) => Data.Job[GameState.EditorIndex].BaseExp = (int)numBaseExp.Value;

        // Start position
        numStartMap = new NumericStepper { MinValue = 0, MaxValue = int.MaxValue }; numStartMap.ValueChanged += (s, e) => Data.Job[GameState.EditorIndex].StartMap = (int)numStartMap.Value;
        numStartX = new NumericStepper { MinValue = 0, MaxValue = 255 }; numStartX.ValueChanged += (s, e) => Data.Job[GameState.EditorIndex].StartX = (byte)numStartX.Value;
        numStartY = new NumericStepper { MinValue = 0, MaxValue = 255 }; numStartY.ValueChanged += (s, e) => Data.Job[GameState.EditorIndex].StartY = (byte)numStartY.Value;

        // Sprites
        numMaleSprite = new NumericStepper { MinValue = 0, MaxValue = GameState.NumCharacters }; numMaleSprite.ValueChanged += (s, e) => { Data.Job[GameState.EditorIndex].MaleSprite = (int)numMaleSprite.Value; LoadSprites(); };
        numFemaleSprite = new NumericStepper { MinValue = 0, MaxValue = GameState.NumCharacters }; numFemaleSprite.ValueChanged += (s, e) => { Data.Job[GameState.EditorIndex].FemaleSprite = (int)numFemaleSprite.Value; LoadSprites(); };

        // Items
        lstStartItems = new ListBox { Height = 140, Width = 200 };
        cmbItems = new ComboBox { Width = 180 };
        numItemAmount = new NumericStepper { MinValue = 1, MaxValue = 999, Value = 1 };
        btnSetItem = new Button { Text = "Set Slot" }; btnSetItem.Click += (s, e) => SetStartItem();

        // Actions
    btnSave = new Button { Text = "Save" }; btnSave.Click += (s, e) => { Editors.JobEditorOK(); Close(); };
    btnDelete = new Button { Text = "Delete" }; btnDelete.Click += (s, e) => { Database.ClearJob(GameState.EditorIndex); ReloadPanel(); };
    btnCopy = new Button { Text = "Copy" }; btnCopy.Click += (s, e) => CopyOrPasteJob();
        btnCancel = new Button { Text = "Cancel" }; btnCancel.Click += (s, e) => { Editors.JobEditorCancel(); Close(); };

        // Previews
        malePreview = new Drawable { Size = new Size(72,72), BackgroundColor = Colors.Black };
        femalePreview = new Drawable { Size = new Size(72,72), BackgroundColor = Colors.Black };
        malePreview.Paint += (s, e) => DrawPreview(e.Graphics, maleBmp, malePreview.Size);
        femalePreview.Paint += (s, e) => DrawPreview(e.Graphics, femaleBmp, femalePreview.Size);

        // Helper to box groups
        GroupBox Box(string text, Eto.Forms.Control content) => new GroupBox { Text = text, Content = content };

        var stats = Box("Stats", new TableLayout
        {
            Spacing = new Size(4,4),
            Rows =
            {
                new TableRow(new Label{Text="STR"}, numStr, new Label{Text="LCK"}, numLck),
                new TableRow(new Label{Text="END"}, numEnd, new Label{Text="INT"}, numInt),
                new TableRow(new Label{Text="VIT"}, numVit, new Label{Text="SPR"}, numSpr),
                new TableRow(new Label{Text="BaseExp"}, numBaseExp, null, null)
            }
        });

        var start = Box("Start Position", new TableLayout
        {
            Spacing = new Size(4,4),
            Rows = { new TableRow(new Label{Text="Map"}, numStartMap, new Label{Text="X"}, numStartX, new Label{Text="Y"}, numStartY) }
        });

        var sprites = Box("Sprites", new TableLayout
        {
            Spacing = new Size(4,4),
            Rows = { new TableRow(new Label{Text="Male"}, numMaleSprite, malePreview, new Label{Text="Female"}, numFemaleSprite, femalePreview) }
        });

        var itemsLayout = new DynamicLayout { Spacing = new Size(4,4) };
        itemsLayout.AddRow(lstStartItems);
        itemsLayout.AddRow(new Label{Text="Item"}, cmbItems, new Label{Text="Amount"}, numItemAmount, btnSetItem);
        var items = Box("Start Items", itemsLayout);

    // Left side: just the jobs list (scales vertically)
    var left = new DynamicLayout { Spacing = new Size(4,4) };
    left.Add(lstJobs, yscale: true);

    // Right content wrapped in a scrollable to keep window compact
    var rightContent = new DynamicLayout { Spacing = new Size(6,6) };
    // Name row at the top (non-scaling textbox like other editors)
    var nameRow = new TableLayout
    {
        Spacing = new Size(4,4),
        Rows = { new TableRow(new Label{Text="Name:"}, new TableCell(txtName, scaleWidth: false), null, null) }
    };
    rightContent.Add(nameRow);
    rightContent.Add(stats);
    rightContent.Add(items);
    rightContent.AddRow(start);
    rightContent.AddRow(sprites);
    rightContent.AddRow(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 6, Items = { btnSave, btnDelete, btnCopy, btnCancel } });
    var right = new Scrollable { Content = rightContent, ExpandContentWidth = true };

        return new TableLayout
        {
            Padding = 4,
            Spacing = new Size(8,8),
            Rows = { new TableRow(left, right) }
        };
    }

    NumericStepper Stat() => new NumericStepper { MinValue = 0, MaxValue = 999, Increment = 1 };

    void InitData()
    {
        _suppressIndexChanged = true;
        try
        {
            lstJobs!.Items.Clear();
            for (int i = 0; i < Constant.MaxJobs; i++) lstJobs.Items.Add((i + 1) + ": " + Data.Job[i].Name);
            lstJobs.SelectedIndex = GameState.EditorIndex >= 0 ? GameState.EditorIndex : 0;
            cmbItems!.Items.Clear();
            for (int i = 0; i < Constant.MaxItems; i++) cmbItems.Items.Add((i + 1) + ": " + Data.Item[i].Name);
            cmbItems.SelectedIndex = 0;
        }
        finally { _suppressIndexChanged = false; }

        ReloadPanel();
    }

void ChangeJob() { if (lstJobs!.SelectedIndex >= 0) { GameState.EditorIndex = lstJobs.SelectedIndex; ReloadPanel(); } }

    void ReloadPanel()
    {
        var job = Data.Job[GameState.EditorIndex];
        txtName!.Text = job.Name; txtDesc!.Text = job.Desc;
        numStr!.Value = job.Stat[(int)Core.Globals.Stat.Strength];
        numLck!.Value = job.Stat[(int)Core.Globals.Stat.Luck];
        numEnd!.Value = job.Stat[(int)Core.Globals.Stat.Luck];
        numInt!.Value = job.Stat[(int)Core.Globals.Stat.Intelligence];
        numVit!.Value = job.Stat[(int)Core.Globals.Stat.Vitality];
        numSpr!.Value = job.Stat[(int)Core.Globals.Stat.Spirit];
        numBaseExp!.Value = job.BaseExp;
        numStartMap!.Value = job.StartMap; numStartX!.Value = job.StartX; numStartY!.Value = job.StartY;
        numMaleSprite!.Value = job.MaleSprite; numFemaleSprite!.Value = job.FemaleSprite;
        lstStartItems!.Items.Clear();
        for (int i = 0; i < Constant.MaxDropItems; i++)
        {
            int id = job.StartItem[i]; int amt = job.StartValue[i];
            string name = id >= 0 && id < Constant.MaxItems ? Data.Item[id].Name : "(None)";
            lstStartItems.Items.Add(name + " x " + amt);
        }
        lstStartItems.SelectedIndex = 0;
        LoadSprites();
    }

    void UpdateName()
    {
        var job = Data.Job[GameState.EditorIndex];
        job.Name = Strings.Trim(txtName!.Text);
        if (lstJobs!.SelectedIndex >= 0)
        {
            // Replace item by removing and inserting new ListItem
            int i = lstJobs.SelectedIndex;
            _suppressIndexChanged = true;
            try
            {
                lstJobs.Items.RemoveAt(i);
                lstJobs.Items.Insert(i, new ListItem { Text = (GameState.EditorIndex + 1) + ": " + job.Name });
                lstJobs.SelectedIndex = i;
            }
            finally { _suppressIndexChanged = false; }
        }
    }

    void CopyOrPasteJob()
    {
        int src = GameState.EditorIndex;
        if (!_hasClipboardJob)
        {
            if (src < 0 || src >= Constant.MaxJobs) return;
            var s = Data.Job[src];
            _clipboardJob = s;
            if (s.Stat != null) _clipboardJob.Stat = (int[])s.Stat.Clone();
            if (s.StartItem != null) _clipboardJob.StartItem = (int[])s.StartItem.Clone();
            if (s.StartValue != null) _clipboardJob.StartValue = (int[])s.StartValue.Clone();
            _hasClipboardJob = true;
            btnCopy!.Text = "Paste";
            return;
        }

    int def = GameState.EditorIndex + 1;
    var oneBased = Editors.PromptIndex(this, "Paste Job", $"Paste job into index (1..{Constant.MaxJobs}):", 1, Constant.MaxJobs, def);
    if (oneBased == null) return;
    int dst = oneBased.Value - 1;
        var n = _clipboardJob;
        if (n.Stat != null) n.Stat = (int[])n.Stat.Clone();
        if (n.StartItem != null) n.StartItem = (int[])n.StartItem.Clone();
        if (n.StartValue != null) n.StartValue = (int[])n.StartValue.Clone();
        Data.Job[dst] = n;
        GameState.JobChanged[dst] = true;

        _suppressIndexChanged = true;
        try
        {
            lstJobs!.Items.RemoveAt(dst);
            lstJobs.Items.Insert(dst, new ListItem { Text = (dst + 1) + ": " + Data.Job[dst].Name });
            lstJobs.SelectedIndex = dst;
        }
        finally { _suppressIndexChanged = false; }
        GameState.EditorIndex = dst;
        ReloadPanel();
    }

    void SetStat(Stat stat, NumericStepper ctl) => Data.Job[GameState.EditorIndex].Stat[(int)stat] = (int)ctl.Value;
        void SetStartItem()
        {
            if (lstStartItems!.SelectedIndex < 0) return;
            var job = Data.Job[GameState.EditorIndex];
            job.StartItem[lstStartItems.SelectedIndex] = cmbItems!.SelectedIndex;
            job.StartValue[lstStartItems.SelectedIndex] = (int)numItemAmount!.Value;
            ReloadPanel();
        }

        void LoadSprites()
        {
            maleBmp?.Dispose(); femaleBmp?.Dispose();
            string malePath = System.IO.Path.Combine(DataPath.Characters, Data.Job[GameState.EditorIndex].MaleSprite + GameState.GfxExt);
            string femalePath = System.IO.Path.Combine(DataPath.Characters, Data.Job[GameState.EditorIndex].FemaleSprite + GameState.GfxExt);
            maleBmp = File.Exists(malePath) ? new Bitmap(malePath) : null;
            femaleBmp = File.Exists(femalePath) ? new Bitmap(femalePath) : null;
            if (maleBmp != null)
            {
                int fw = maleBmp.Width / 4;
                int fh = maleBmp.Height / 4;
                malePreview!.Size = new Size(fw, fh);
            }
            if (femaleBmp != null)
            {
                int fw = femaleBmp.Width / 4;
                int fh = femaleBmp.Height / 4;
                femalePreview!.Size = new Size(fw, fh);
            }
            malePreview!.Invalidate(); femalePreview!.Invalidate();
        }

        public void DrawPreview(Graphics g, Bitmap? bmp, Size size)
        {
            g.Clear(Colors.Transparent);
            if (bmp == null) return;
            int fw = bmp.Width / 4;
            int fh = bmp.Height / 4;
            // Draw only the first frame at 0,0, at its native size
            g.DrawImage(bmp, new RectangleF(0, 0, fw, fh), new Rectangle(0, 0, fw, fh));
        }

        // Parameterless wrapper used by Editors.cs legacy call pattern
        public void DrawPreview()
        {
            malePreview?.Invalidate();
            femalePreview?.Invalidate();
        }

        protected override void OnClosed(EventArgs e)
        {
            maleBmp?.Dispose(); femaleBmp?.Dispose();
            Editors.JobEditorCancel();
            base.OnClosed(e);
        }
    }
}