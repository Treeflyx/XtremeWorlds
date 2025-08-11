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

    public ListBox? lstJobs, lstStartItems;
    public TextBox? txtName;
    public TextArea? txtDesc;
    public TextArea txtDescription => txtDesc!;
    public ComboBox? cmbItems;
    public NumericStepper? numStr, numLck, numEnd, numInt, numVit, numSpr, numBaseExp;
    public NumericStepper? numStartMap, numStartX, numStartY;
    public NumericStepper? numMaleSprite, numFemaleSprite, numItemAmount;
    public Button? btnSetItem, btnSave, btnDelete, btnCancel;
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
        ClientSize = new Size(880, 600);
        Padding = 6;
        Content = BuildUi();
        Editors.AutoSizeWindow(this, 760, 520);
        Shown += (s, e) => InitData();
    }

    Eto.Forms.Control BuildUi()
    {
        lstJobs = new ListBox { Width = 220 };
        lstJobs.SelectedIndexChanged += (s, e) => ChangeJob();
        txtName = new TextBox(); txtName.TextChanged += (s, e) => UpdateName();
        txtDesc = new TextArea { Size = new Size(200, 120) }; txtDesc.TextChanged += (s, e) => Data.Job[GameState.EditorIndex].Desc = txtDesc.Text;

        numStr = Stat(); numStr.ValueChanged += (s, e) => SetStat(Core.Globals.Stat.Strength, numStr);
        numLck = Stat(); numLck.ValueChanged += (s, e) => SetStat(Core.Globals.Stat.Luck, numLck);
        numEnd = Stat(); numEnd.ValueChanged += (s, e) => SetStat(Core.Globals.Stat.Luck, numEnd); // original mapping
        numInt = Stat(); numInt.ValueChanged += (s, e) => SetStat(Core.Globals.Stat.Intelligence, numInt);
        numVit = Stat(); numVit.ValueChanged += (s, e) => SetStat(Core.Globals.Stat.Vitality, numVit);
        numSpr = Stat(); numSpr.ValueChanged += (s, e) => SetStat(Core.Globals.Stat.Spirit, numSpr);
        numBaseExp = new NumericStepper { MinValue = 0, MaxValue = 5_000_000, Increment = 10 }; numBaseExp.ValueChanged += (s, e) => Data.Job[GameState.EditorIndex].BaseExp = (int)numBaseExp.Value;

        numStartMap = new NumericStepper { MinValue = 0, MaxValue = int.MaxValue }; numStartMap.ValueChanged += (s, e) => Data.Job[GameState.EditorIndex].StartMap = (int)numStartMap.Value;
        numStartX = new NumericStepper { MinValue = 0, MaxValue = 255 }; numStartX.ValueChanged += (s, e) => Data.Job[GameState.EditorIndex].StartX = (byte)numStartX.Value;
        numStartY = new NumericStepper { MinValue = 0, MaxValue = 255 }; numStartY.ValueChanged += (s, e) => Data.Job[GameState.EditorIndex].StartY = (byte)numStartY.Value;

        numMaleSprite = new NumericStepper { MinValue = 0, MaxValue = GameState.NumCharacters }; numMaleSprite.ValueChanged += (s, e) => { Data.Job[GameState.EditorIndex].MaleSprite = (int)numMaleSprite.Value; LoadSprites(); };
        numFemaleSprite = new NumericStepper { MinValue = 0, MaxValue = GameState.NumCharacters }; numFemaleSprite.ValueChanged += (s, e) => { Data.Job[GameState.EditorIndex].FemaleSprite = (int)numFemaleSprite.Value; LoadSprites(); };

        lstStartItems = new ListBox { Height = 140 };
        cmbItems = new ComboBox { Width = 180 };
        numItemAmount = new NumericStepper { MinValue = 1, MaxValue = 999, Value = 1 };
        btnSetItem = new Button { Text = "Set Slot" }; btnSetItem.Click += (s, e) => SetStartItem();

        btnSave = new Button { Text = "Save" }; btnSave.Click += (s, e) => { Editors.JobEditorOK(); Close(); };
        btnDelete = new Button { Text = "Delete" }; btnDelete.Click += (s, e) => { Database.ClearJob(GameState.EditorIndex); ReloadPanel(); };
        btnCancel = new Button { Text = "Cancel" }; btnCancel.Click += (s, e) => { Editors.JobEditorCancel(); Close(); };

        malePreview = new Drawable { Size = new Size(72,72), BackgroundColor = Colors.Black };
        femalePreview = new Drawable { Size = new Size(72,72), BackgroundColor = Colors.Black };
        malePreview.Paint += (s, e) => DrawPreview(e.Graphics, maleBmp, malePreview.Size);
        femalePreview.Paint += (s, e) => DrawPreview(e.Graphics, femaleBmp, femalePreview.Size);

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

        var items = Box("Start Items", new TableLayout
        {
            Spacing = new Size(4,4),
            Rows =
            {
                new TableRow(lstStartItems),
                new TableRow(new Label{Text="Item"}, cmbItems, new Label{Text="Amt"}, numItemAmount, btnSetItem)
            }
        });

        var left = new DynamicLayout { Spacing = new Size(4,4) };
        left.AddRow(new Label{Text="Jobs", Font = SystemFonts.Bold(11)});
        left.Add(lstJobs, yscale:true);
        left.Add(null);

        var right = new DynamicLayout { Spacing = new Size(6,6) };
        right.AddRow(stats);
        right.AddRow(start);
        right.AddRow(sprites);
        right.AddRow(items);
        right.AddRow(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 6, Items = { btnSave, btnDelete, btnCancel } });

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
        lstJobs!.Items.Clear();
        for (int i = 0; i < Constant.MaxJobs; i++) lstJobs.Items.Add((i + 1) + ": " + Data.Job[i].Name);
        lstJobs.SelectedIndex = GameState.EditorIndex >= 0 ? GameState.EditorIndex : 0;
        cmbItems!.Items.Clear();
        for (int i = 0; i < Constant.MaxItems; i++) cmbItems.Items.Add((i + 1) + ": " + Data.Item[i].Name);
        cmbItems.SelectedIndex = 0;
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
            lstJobs.Items.RemoveAt(i);
            lstJobs.Items.Insert(i, new ListItem { Text = (GameState.EditorIndex + 1) + ": " + job.Name });
            lstJobs.SelectedIndex = i;
        }
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
            malePreview!.Invalidate(); femalePreview!.Invalidate();
        }

        public void DrawPreview(Graphics g, Bitmap? bmp, Size size)
        {
            g.Clear(Colors.Black);
            if (bmp == null) return;
            int fw = bmp.Width / 4; int fh = bmp.Height / 4;
            g.DrawImage(bmp, new RectangleF(0,0,size.Width,size.Height), new Rectangle(0,0,fw,fh));
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