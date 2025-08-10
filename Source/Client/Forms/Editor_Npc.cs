using System;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using Microsoft.VisualBasic;
using Core;

namespace Client
{
    public sealed class Editor_Npc : Form
    {
        public static Editor_Npc? Instance { get; private set; }

        // Public controls referenced externally
        public ListBox lstIndex = null!;
        public TextBox txtName = null!;
        public TextBox txtAttackSay = null!;
        public NumericStepper nudSprite = null!;
        public NumericStepper nudSpawnSecs = null!;
        public ComboBox cmbBehaviour = null!;
        public ComboBox cmbFaction = null!;
        public NumericStepper nudRange = null!;
        public NumericStepper nudChance = null!;
        public ComboBox cmbItem = null!;
        public NumericStepper nudAmount = null!;
        public NumericStepper nudHp = null!;
        public NumericStepper nudExp = null!;
        public NumericStepper nudLevel = null!;
        public NumericStepper nudDamage = null!;
        public ComboBox cmbSpawnPeriod = null!;
        public ComboBox cmbAnimation = null!;
        public NumericStepper nudStrength = null!;
        public NumericStepper nudIntelligence = null!;
        public NumericStepper nudSpirit = null!;
        public NumericStepper nudLuck = null!;
        public NumericStepper nudVitality = null!;
        public ComboBox cmbSkill1 = null!;
        public ComboBox cmbSkill2 = null!;
        public ComboBox cmbSkill3 = null!;
        public ComboBox cmbSkill4 = null!;
        public ComboBox cmbSkill5 = null!;
        public ComboBox cmbSkill6 = null!;
        public ComboBox cmbDropSlot = null!;
        public Drawable picSprite = null!;

        private Button btnSave = null!;
        private Button btnCancel = null!;
        private Button btnDelete = null!;

        private Bitmap? _spriteBitmap;
        private bool _initializing;

        public Editor_Npc()
        {
            Instance = this;
            Title = "NPC Editor";
            ClientSize = new Size(1050, 600);
            MinimumSize = new Size(1050, 600);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            lstIndex = new ListBox { Size = new Size(220, -1) };
            lstIndex.SelectedIndexChanged += (s, e) =>
            {
                if (_initializing) return;
                if (lstIndex.SelectedIndex < 0) return;
                Editors.NpcEditorInit();
            };

            txtName = new TextBox();
            txtName.TextChanged += (s, e) =>
            {
                if (_initializing) return;
                if (lstIndex.SelectedIndex < 0) return;
                int idx = lstIndex.SelectedIndex;
                Data.Npc[GameState.EditorIndex].Name = Strings.Trim(txtName.Text);
                RefreshListEntry(idx);
                GameState.NpcChanged[GameState.EditorIndex] = true;
            };

            txtAttackSay = new TextBox();
            txtAttackSay.TextChanged += (s, e) =>
            {
                if (_initializing) return;
                Data.Npc[GameState.EditorIndex].AttackSay = txtAttackSay.Text;
                GameState.NpcChanged[GameState.EditorIndex] = true;
            };

            nudSprite = new NumericStepper { MinValue = 0, MaxValue = GameState.NumCharacters, DecimalPlaces = 0, Width = 80 };
            nudSprite.ValueChanged += (s, e) =>
            {
                if (_initializing) return;
                Data.Npc[GameState.EditorIndex].Sprite = (int)nudSprite.Value;
                DrawSprite();
                GameState.NpcChanged[GameState.EditorIndex] = true;
            };

            nudSpawnSecs = new NumericStepper { MinValue = 0, MaxValue = 3600, DecimalPlaces = 0, Width = 80 };
            nudSpawnSecs.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].SpawnSecs = (int)nudSpawnSecs.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };

            cmbBehaviour = new ComboBox();
            cmbBehaviour.SelectedIndexChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Behaviour = (byte)cmbBehaviour.SelectedIndex; GameState.NpcChanged[GameState.EditorIndex] = true; } };

            cmbFaction = new ComboBox();
            cmbFaction.SelectedIndexChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Faction = (byte)cmbFaction.SelectedIndex; GameState.NpcChanged[GameState.EditorIndex] = true; } };

            nudRange = new NumericStepper { MinValue = 0, MaxValue = 50, DecimalPlaces = 0, Width = 80 };
            nudRange.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Range = (byte)nudRange.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };

            cmbAnimation = new ComboBox();
            cmbAnimation.SelectedIndexChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Animation = cmbAnimation.SelectedIndex; GameState.NpcChanged[GameState.EditorIndex] = true; } };

            nudHp = new NumericStepper { MinValue = 0, MaxValue = 10000000, DecimalPlaces = 0, Width = 100 };
            nudHp.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Hp = (int)nudHp.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            nudExp = new NumericStepper { MinValue = 0, MaxValue = 10000000, DecimalPlaces = 0, Width = 100 };
            nudExp.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Exp = (int)nudExp.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            nudLevel = new NumericStepper { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 80 };
            nudLevel.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Level = (byte)nudLevel.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            nudDamage = new NumericStepper { MinValue = 0, MaxValue = 1000000, DecimalPlaces = 0, Width = 100 };
            nudDamage.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Damage = (int)nudDamage.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };

            cmbSpawnPeriod = new ComboBox();
            cmbSpawnPeriod.SelectedIndexChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].SpawnTime = (byte)cmbSpawnPeriod.SelectedIndex; GameState.NpcChanged[GameState.EditorIndex] = true; } };

            // Stats
            nudStrength = new NumericStepper { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 70 };
            nudStrength.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Stat[(int)Core.Stat.Strength] = (byte)nudStrength.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            nudIntelligence = new NumericStepper { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 70 };
            nudIntelligence.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Stat[(int)Core.Stat.Intelligence] = (byte)nudIntelligence.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            nudSpirit = new NumericStepper { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 70 };
            nudSpirit.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Stat[(int)Core.Stat.Spirit] = (byte)nudSpirit.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            nudLuck = new NumericStepper { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 70 };
            nudLuck.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Stat[(int)Core.Stat.Luck] = (byte)nudLuck.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            nudVitality = new NumericStepper { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 70 };
            nudVitality.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Stat[(int)Core.Stat.Vitality] = (byte)nudVitality.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };

            // Skills
            cmbSkill1 = new ComboBox(); cmbSkill1.SelectedIndexChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Skill[0] = (byte)cmbSkill1.SelectedIndex; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            cmbSkill2 = new ComboBox(); cmbSkill2.SelectedIndexChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Skill[1] = (byte)cmbSkill2.SelectedIndex; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            cmbSkill3 = new ComboBox(); cmbSkill3.SelectedIndexChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Skill[2] = (byte)cmbSkill3.SelectedIndex; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            cmbSkill4 = new ComboBox(); cmbSkill4.SelectedIndexChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Skill[3] = (byte)cmbSkill4.SelectedIndex; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            cmbSkill5 = new ComboBox(); cmbSkill5.SelectedIndexChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Skill[4] = (byte)cmbSkill5.SelectedIndex; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            cmbSkill6 = new ComboBox(); cmbSkill6.SelectedIndexChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].Skill[5] = (byte)cmbSkill6.SelectedIndex; GameState.NpcChanged[GameState.EditorIndex] = true; } };

            // Drops
            cmbDropSlot = new ComboBox();
            cmbDropSlot.SelectedIndexChanged += (s, e) =>
            {
                if (_initializing) return;
                SyncDropFields();
            };
            for (int i = 0; i < 6; i++) cmbDropSlot.Items.Add((i + 1).ToString());

            cmbItem = new ComboBox();
            cmbItem.SelectedIndexChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].DropItem[cmbDropSlot.SelectedIndex] = cmbItem.SelectedIndex; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            nudAmount = new NumericStepper { MinValue = 0, MaxValue = 1000000, DecimalPlaces = 0, Width = 100 };
            nudAmount.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].DropItemValue[cmbDropSlot.SelectedIndex] = (int)nudAmount.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };
            nudChance = new NumericStepper { MinValue = 0, MaxValue = 100, DecimalPlaces = 0, Width = 80 };
            nudChance.ValueChanged += (s, e) => { if (!_initializing) { Data.Npc[GameState.EditorIndex].DropChance[cmbDropSlot.SelectedIndex] = (int)nudChance.Value; GameState.NpcChanged[GameState.EditorIndex] = true; } };

            picSprite = new Drawable { Size = new Size(96, 96), BackgroundColor = Colors.Black };
            picSprite.Paint += (s, e) =>
            {
                if (_spriteBitmap != null)
                {
                    // Show first frame (image assumed 4x4 frames as original code divides by 4)
                    int frameW = _spriteBitmap.Width / 4;
                    int frameH = _spriteBitmap.Height / 4;
                    e.Graphics.DrawImage(_spriteBitmap, new Rectangle(0,0, frameW, frameH));
                }
            };

            btnSave = new Button { Text = "Save" };
            btnSave.Click += (s, e) => { Editors.NpcEditorOK(); Close(); };

            btnCancel = new Button { Text = "Cancel" };
            btnCancel.Click += (s, e) => { Editors.NpcEditorCancel(); Close(); };

            btnDelete = new Button { Text = "Delete" };
            btnDelete.Click += (s, e) =>
            {
                if (lstIndex.SelectedIndex < 0) return;
                Database.ClearNpc(GameState.EditorIndex);
                RefreshListEntry(GameState.EditorIndex);
                Editors.NpcEditorInit();
            };

            // Layout sections
            var generalGroup = new TableLayout
            {
                Spacing = new Size(4, 4),
                Rows =
                {
                    new TableRow(new Label{Text="Name:"}, txtName),
                    new TableRow(new Label{Text="Attack Say:"}, txtAttackSay),
                    new TableRow(new Label{Text="Sprite:"}, new StackLayout { Orientation=Orientation.Horizontal, Items = { nudSprite, picSprite } }),
                    new TableRow(new Label{Text="Animation:"}, cmbAnimation),
                    new TableRow(new Label{Text="Spawn Secs:"}, nudSpawnSecs),
                    new TableRow(new Label{Text="Spawn Period:"}, cmbSpawnPeriod),
                    new TableRow(new Label{Text="Behaviour:"}, cmbBehaviour),
                    new TableRow(new Label{Text="Faction:"}, cmbFaction),
                    new TableRow(new Label{Text="Range:"}, nudRange),
                    new TableRow(new Label{Text="Damage:"}, nudDamage)
                }
            };

            var statsGroup = new TableLayout
            {
                Spacing = new Size(4, 4),
                Rows =
                {
                    new TableRow(new Label{Text="HP:"}, nudHp),
                    new TableRow(new Label{Text="EXP:"}, nudExp),
                    new TableRow(new Label{Text="Level:"}, nudLevel),
                    new TableRow(new Label{Text="Strength:"}, nudStrength),
                    new TableRow(new Label{Text="Intelligence:"}, nudIntelligence),
                    new TableRow(new Label{Text="Spirit:"}, nudSpirit),
                    new TableRow(new Label{Text="Luck:"}, nudLuck),
                    new TableRow(new Label{Text="Vitality:"}, nudVitality)
                }
            };

            var skillsGroup = new TableLayout
            {
                Spacing = new Size(4, 4),
                Rows =
                {
                    new TableRow(new Label{Text="Skill 1:"}, cmbSkill1),
                    new TableRow(new Label{Text="Skill 2:"}, cmbSkill2),
                    new TableRow(new Label{Text="Skill 3:"}, cmbSkill3),
                    new TableRow(new Label{Text="Skill 4:"}, cmbSkill4),
                    new TableRow(new Label{Text="Skill 5:"}, cmbSkill5),
                    new TableRow(new Label{Text="Skill 6:"}, cmbSkill6)
                }
            };

            var dropsGroup = new TableLayout
            {
                Spacing = new Size(4, 4),
                Rows =
                {
                    new TableRow(new Label{Text="Slot:"}, cmbDropSlot),
                    new TableRow(new Label{Text="Item:"}, cmbItem),
                    new TableRow(new Label{Text="Amount:"}, nudAmount),
                    new TableRow(new Label{Text="Chance %:"}, nudChance)
                }
            };

            var rightPanel = new Scrollable
            {
                Content = new StackLayout
                {
                    Spacing = 10,
                    Items =
                    {
                        new GroupBox{ Text = "General", Content = generalGroup},
                        new GroupBox{ Text = "Stats", Content = statsGroup},
                        new GroupBox{ Text = "Skills", Content = skillsGroup},
                        new GroupBox{ Text = "Drops", Content = dropsGroup},
                        new StackLayout{ Orientation=Orientation.Horizontal, Spacing=6, Items={ btnSave, btnCancel, btnDelete } }
                    }
                }
            };

            Content = new Splitter
            {
                Position = 240,
                Panel1 = new StackLayout
                {
                    Padding = 8,
                    Spacing = 4,
                    Items = { new Label{ Text = "NPCs", Font = SystemFonts.Bold(12)}, lstIndex }
                },
                Panel2 = rightPanel
            };

            Shown += (s, e) => LoadData();
            Closed += (s, e) =>
            {
                if (GameState.MyEditorType == EditorType.Npc)
                {
                    Editors.NpcEditorCancel();
                }
                if (Instance == this) Instance = null;
            };
        }

        private void LoadData()
        {
            _initializing = true;
            lstIndex.Items.Clear();
            for (int i = 0; i < Constant.MaxNpcs; i++)
            {
                lstIndex.Items.Add(new ListItem { Text = (i + 1) + ": " + Strings.Trim(Data.Npc[i].Name) });
            }
            // populate animations
            cmbAnimation.Items.Clear();
            for (int i = 0; i < Constant.MaxAnimations; i++)
                cmbAnimation.Items.Add((i + 1) + ": " + Data.Animation[i].Name);
            // populate skills
            void fillSkills(ComboBox cmb)
            {
                cmb.Items.Clear();
                for (int i = 0; i < Constant.MaxSkills; i++)
                    cmb.Items.Add((i + 1) + ": " + Data.Skill[i].Name);
            }
            fillSkills(cmbSkill1); fillSkills(cmbSkill2); fillSkills(cmbSkill3);
            fillSkills(cmbSkill4); fillSkills(cmbSkill5); fillSkills(cmbSkill6);

            // populate items
            cmbItem.Items.Clear();
            for (int i = 0; i < Constant.MaxItems; i++)
                cmbItem.Items.Add((i + 1) + ": " + Core.Data.Item[i].Name);

            cmbBehaviour.Items.Clear();
            cmbBehaviour.Items.Add("Stationary");
            cmbBehaviour.Items.Add("Roam");
            cmbBehaviour.Items.Add("Aggressive");

            cmbFaction.Items.Clear();
            cmbFaction.Items.Add("Neutral");
            cmbFaction.Items.Add("Friendly");
            cmbFaction.Items.Add("Hostile");

            cmbSpawnPeriod.Items.Clear();
            cmbSpawnPeriod.Items.Add("Any");
            cmbSpawnPeriod.Items.Add("Day");
            cmbSpawnPeriod.Items.Add("Night");

            if (lstIndex.Items.Count > 0) lstIndex.SelectedIndex = 0;
            cmbDropSlot.SelectedIndex = 0;
            _initializing = false;
            if (lstIndex.Items.Count > 0) Editors.NpcEditorInit();
        }

        private void SyncDropFields()
        {
            if (lstIndex.SelectedIndex < 0 || cmbDropSlot.SelectedIndex < 0) return;
            _initializing = true;
            cmbItem.SelectedIndex = Data.Npc[GameState.EditorIndex].DropItem[cmbDropSlot.SelectedIndex];
            nudAmount.Value = Data.Npc[GameState.EditorIndex].DropItemValue[cmbDropSlot.SelectedIndex];
            nudChance.Value = Data.Npc[GameState.EditorIndex].DropChance[cmbDropSlot.SelectedIndex];
            _initializing = false;
        }

        private void RefreshListEntry(int index)
        {
            if (index < 0 || index >= lstIndex.Items.Count) return;
            if (lstIndex.Items[index] is ListItem item)
            {
                item.Text = (index + 1) + ": " + Strings.Trim(Data.Npc[index].Name);
                lstIndex.Invalidate();
            }
        }

        public void DrawSprite()
        {
            int sprite = (int)nudSprite.Value;
            _spriteBitmap = null;
            picSprite.Invalidate();
            if (sprite < 1 || sprite > GameState.NumCharacters) return;
            var path = System.IO.Path.Combine(Core.Path.Characters, sprite + GameState.GfxExt);
            if (!File.Exists(path)) return;
            try
            {
                using (var fs = File.OpenRead(path))
                {
                    _spriteBitmap = new Bitmap(fs);
                }
            }
            catch { _spriteBitmap = null; }
            picSprite.Invalidate();
        }
    }
}