using System;
using System.IO;
using Client.Net;
using Eto.Forms;
using Core;
using Core.Globals;
using Microsoft.VisualBasic;
using Eto.Drawing;

namespace Client
{
    public class Editor_Item : Form
    {
        private static Editor_Item? _instance;
        public static Editor_Item Instance => _instance ??= new Editor_Item();
        private bool _suppressIndexChanged;
        // Copy/Paste clipboard for items
        private Core.Globals.Type.Item _clipboardItem;
        private bool _hasClipboardItem;

        // Core lists/controls
        public ListBox? lstIndex; // legacy name used by Editors.cs
        public TextBox? txtName;
        public TextArea? txtDescription;
        public NumericStepper? numIcon, numPaperdoll, numItemLvl, numPrice, numRarity, numSpeed, numDamage, numVitalMod, numEventId, numEventValue;
        public CheckBox? chkStackable, chkKnockBack;
        public ComboBox? cmbType, cmbSubType, cmbAnimation, cmbBind, cmbJobReq, cmbAccessReq, cmbTool, cmbSkills, cmbProjectile, cmbAmmo, cmbKnockBackTiles;
        public NumericStepper? numStrReq, numVitReq, numLuckReq, numIntReq, numSprReq, numLevelReq;
        public NumericStepper? numStrAdd, numVitAdd, numLuckAdd, numIntAdd, numSprAdd;
        public Drawable? iconPreview, paperdollPreview;
        public Button? btnSave, btnCancel, btnDelete, btnCopy;
        public Button? btnSpawn;
        public NumericStepper? numSpawnAmount;

        // Frames (logical group visibility mimic of old design)
        public GroupBox? fraEquipment, fraVitals, fraSkill, fraProjectile, fraEvents, fraRequirements, fraBasics;

        Bitmap? itemBmp, paperdollBmp;

        public Editor_Item()
        {
            _instance = this;
            Title = "Item Editor";
            ClientSize = new Size(1100, 800);
            Padding = 6;
            // Ensure Load is subscribed before wiring other events in BuildUi
            Load += (s,e) => InitData();
            Content = BuildUi();
            Editors.AutoSizeWindow(this, 1000, 720);
        }

        Eto.Forms.Control BuildUi()
        {
            lstIndex = new ListBox { Width = 200 };
            lstIndex.SelectedIndexChanged += (s,e) =>
            {
                if (_suppressIndexChanged) return;
                if (lstIndex.SelectedIndex >= 0)
                    GameState.EditorIndex = lstIndex.SelectedIndex;
                Editors.ItemEditorInit();
            };

            txtName = new TextBox { Width = 200 }; txtName.TextChanged += (s,e)=> UpdateName();
            txtDescription = new TextArea { Size = new Size(200,120) }; txtDescription.TextChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Description = Strings.Trim(txtDescription.Text); MarkChanged(); };

            numIcon = Num(0, GameState.NumItems); numIcon.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Icon = (int)numIcon.Value; LoadItemIcon(); MarkChanged(); };
            numPaperdoll = Num(0, GameState.NumPaperdolls); numPaperdoll.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Paperdoll = (int)numPaperdoll.Value; LoadPaperdoll(); MarkChanged(); };
            numItemLvl = Num(1, 255); numItemLvl.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].ItemLevel = (byte)numItemLvl.Value; MarkChanged(); };
            numPrice = Num(0, int.MaxValue); numPrice.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Price = (int)numPrice.Value; MarkChanged(); };
            numRarity = Num(0, 255); numRarity.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Rarity = (byte)numRarity.Value; MarkChanged(); };
            numSpeed = Num(100, 10000); numSpeed.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Speed = (int)numSpeed.Value; MarkChanged(); };
            numDamage = Num(0, 100000); numDamage.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Data2 = (int)numDamage.Value; MarkChanged(); };
            numVitalMod = Num(0, 100000); numVitalMod.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Data1 = (int)numVitalMod.Value; MarkChanged(); };
            numEventId = Num(0, 100000); numEventId.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Data1 = (int)numEventId.Value; MarkChanged(); };
            numEventValue = Num(0, 100000); numEventValue.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Data2 = (int)numEventValue.Value; MarkChanged(); };

            chkStackable = new CheckBox { Text = "Stackable" }; chkStackable.CheckedChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Stackable = chkStackable.Checked==true? (byte)1:(byte)0; MarkChanged(); };
            chkKnockBack = new CheckBox { Text = "Knockback" }; chkKnockBack.CheckedChanged += (s,e)=> { Data.Item[GameState.EditorIndex].KnockBack = chkKnockBack.Checked==true? (byte)1:(byte)0; MarkChanged(); };

            cmbType = new ComboBox(); cmbType.SelectedIndexChanged += (s,e)=> ChangeType();
            cmbSubType = new ComboBox(); cmbSubType.SelectedIndexChanged += (s,e)=> { Data.Item[GameState.EditorIndex].SubType = (byte)cmbSubType.SelectedIndex; TogglePanels(); MarkChanged(); };
            cmbAnimation = new ComboBox(); cmbAnimation.SelectedIndexChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Animation = cmbAnimation.SelectedIndex; MarkChanged(); };
            cmbBind = new ComboBox(); cmbBind.SelectedIndexChanged += (s,e)=> { Data.Item[GameState.EditorIndex].BindType = (byte)cmbBind.SelectedIndex; MarkChanged(); };
            cmbJobReq = new ComboBox(); cmbJobReq.SelectedIndexChanged += (s,e)=> { Data.Item[GameState.EditorIndex].JobReq = cmbJobReq.SelectedIndex; MarkChanged(); };
            cmbAccessReq = new ComboBox(); cmbAccessReq.SelectedIndexChanged += (s,e)=> { Data.Item[GameState.EditorIndex].AccessReq = cmbAccessReq.SelectedIndex; MarkChanged(); };
            cmbTool = new ComboBox(); cmbTool.SelectedIndexChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Data3 = cmbTool.SelectedIndex; MarkChanged(); };
            cmbSkills = new ComboBox(); cmbSkills.SelectedIndexChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Data1 = cmbSkills.SelectedIndex; MarkChanged(); };
            cmbProjectile = new ComboBox(); cmbProjectile.SelectedIndexChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Projectile = cmbProjectile.SelectedIndex; MarkChanged(); };
            cmbAmmo = new ComboBox(); cmbAmmo.SelectedIndexChanged += (s,e)=> { Data.Item[GameState.EditorIndex].Ammo = cmbAmmo.SelectedIndex; MarkChanged(); };
            cmbKnockBackTiles = new ComboBox(); cmbKnockBackTiles.SelectedIndexChanged += (s,e)=> { Data.Item[GameState.EditorIndex].KnockBackTiles = (byte)cmbKnockBackTiles.SelectedIndex; MarkChanged(); };

            numLevelReq = Num(0, 500); numLevelReq.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].LevelReq = (int)numLevelReq.Value; MarkChanged(); };
            numStrReq = StatReq(Stat.Strength);
            numVitReq = StatReq(Stat.Vitality);
            numLuckReq = StatReq(Stat.Luck);
            numIntReq = StatReq(Stat.Intelligence);
            numSprReq = StatReq(Stat.Spirit);

            numStrAdd = StatAdd(Stat.Strength);
            numVitAdd = StatAdd(Stat.Vitality);
            numLuckAdd = StatAdd(Stat.Luck);
            numIntAdd = StatAdd(Stat.Intelligence);
            numSprAdd = StatAdd(Stat.Spirit);

            iconPreview = new Drawable { Size = new Size(32,32), BackgroundColor = Colors.Transparent }; iconPreview.Paint += (s,e)=> { if(itemBmp!=null) e.Graphics.DrawImage(itemBmp,0,0); };
            iconPreview.Paint += (s,e)=> {
                if(itemBmp!=null) {
                    int fw = itemBmp.Width / 4;
                    int fh = itemBmp.Height / 4;
                    iconPreview.Size = new Size(fw, fh);
                    e.Graphics.DrawImage(itemBmp, new Rectangle(0,0,fw,fh), new Rectangle(0,0,fw,fh));
                }
            };
            paperdollPreview = new Drawable { Size = new Size(64,64), BackgroundColor = Colors.Transparent };
            paperdollPreview.Paint += (s,e)=> {
                if(paperdollBmp!=null) {
                    int fw = paperdollBmp.Width / 4;
                    int fh = paperdollBmp.Height / 4;
                    paperdollPreview.Size = new Size(fw, fh);
                    e.Graphics.DrawImage(paperdollBmp, new Rectangle(0,0,fw,fh), new Rectangle(0,0,fw,fh));
                }
            };

            btnSave = new Button { Text = "Save" }; btnSave.Click += (s,e)=> { Editors.ItemEditorOK(); Close(); };
            btnCancel = new Button { Text = "Cancel" }; btnCancel.Click += (s,e)=> { Editors.ItemEditorCancel(); Close(); };
            btnDelete = new Button { Text = "Delete" }; btnDelete.Click += (s,e)=> { Item.ClearItem(GameState.EditorIndex); Editors.ItemEditorInit(); MarkChanged(); };
            btnCopy = new Button { Text = "Copy" };
            btnCopy.Click += (s,e)=> CopyOrPasteItem();
            btnSpawn = new Button { Text = "Spawn" }; btnSpawn.Click += (s,e)=> { Sender.SendSpawnItem(GameState.EditorIndex, (int)numSpawnAmount!.Value); };
            numSpawnAmount = Num(1, int.MaxValue); numSpawnAmount.Value = 1;

            fraBasics = Group("Basics", new Eto.Forms.TableLayout
            {
                Spacing = new Size(4,4),
                Rows =
                {
                    // Name fixed width like other editor; wrap in non-scaling TableCell
                    new TableRow(new TableCell(new Label{Text="Name"}), new TableCell(txtName, scaleWidth: false), null, null),
                    new TableRow(new Label{Text="Paperdoll"}, numPaperdoll, paperdollPreview, null),
                    // Move SubType up next to Description for quicker access
                    new TableRow(new Label{Text="Description"}, txtDescription, new Label{Text="SubType"}, cmbSubType),
                    new TableRow(new Label{Text="Icon"}, numIcon, iconPreview, null, null),
                    new TableRow(new Label{Text="Type"}, cmbType, new Label{Text="Animation"}, cmbAnimation),
                    new TableRow(new Label{Text="Bind"}, cmbBind, new Label{Text="Item Lvl"}, numItemLvl),
                    new TableRow(new Label{Text="Price"}, numPrice, new Label{Text="Rarity"}, numRarity, chkStackable)
                }
            });

            fraEquipment = Group("Equipment", new Eto.Forms.TableLayout
            {
                Spacing = new Size(4,4),
                Rows =
                {
                    new TableRow(new Label{Text="Damage"}, numDamage, new Label{Text="Speed"}, numSpeed),
                    new TableRow(new Label{Text="Tool"}, cmbTool, chkKnockBack, new Label{Text="KB Tiles"}, cmbKnockBackTiles),
                    new TableRow(new Label{Text="Add STR"}, numStrAdd, new Label{Text="Add VIT"}, numVitAdd),
                    new TableRow(new Label{Text="Add LCK"}, numLuckAdd, new Label{Text="Add INT"}, numIntAdd),
                    new TableRow(new Label{Text="Add SPR"}, numSprAdd, null, null)
                }
            });

            fraVitals = Group("Consumable", new Eto.Forms.TableLayout
            {
                Rows = { new TableRow(new Label{Text="Vital Mod"}, numVitalMod) }
            });

            fraSkill = Group("Skill", new Eto.Forms.TableLayout
            {
                Rows = { new TableRow(new Label{Text="Skill"}, cmbSkills) }
            });

            fraProjectile = Group("Projectile", new Eto.Forms.TableLayout
            {
                Rows =
                {
                    new TableRow(new Label{Text="Projectile"}, cmbProjectile),
                    new TableRow(new Label{Text="Ammo"}, cmbAmmo)
                }
            });

            fraEvents = Group("Event", new Eto.Forms.TableLayout
            {
                Rows =
                {
                    new TableRow(new Label{Text="Event Id"}, numEventId),
                    new TableRow(new Label{Text="Event Val"}, numEventValue)
                }
            });

            fraRequirements = Group("Requirements", new Eto.Forms.TableLayout
            {
                Spacing = new Size(4,4),
                Rows =
                {
                    new TableRow(new Label{Text="Job"}, cmbJobReq, new Label{Text="Access"}, cmbAccessReq),
                    new TableRow(new Label{Text="Level"}, numLevelReq, null, null),
                    new TableRow(new Label{Text="Req STR"}, numStrReq, new Label{Text="Req VIT"}, numVitReq),
                    new TableRow(new Label{Text="Req LCK"}, numLuckReq, new Label{Text="Req INT"}, numIntReq),
                    new TableRow(new Label{Text="Req SPR"}, numSprReq, null, null)
                }
            });

            var left = new DynamicLayout { Spacing = new Size(4,4) };
            left.AddRow(new Label{Text="Items", Font = SystemFonts.Bold(12)});
            left.Add(lstIndex, yscale:true);

            var mid = new DynamicLayout { Spacing = new Size(6,6) };
            mid.AddRow(fraBasics);
            mid.AddRow(fraEquipment);
            mid.AddRow(fraVitals);
            mid.AddRow(fraSkill);
            mid.AddRow(fraProjectile);
            mid.AddRow(fraEvents);
            // Action bar at bottom with Save/Delete/Copy/Cancel and Spawn controls
            mid.AddRow(new StackLayout{Orientation=Orientation.Horizontal,Spacing=6,Items={btnSave,btnDelete,btnCopy,btnCancel,btnSpawn,numSpawnAmount}});

            var right = new DynamicLayout { Spacing = new Size(6,6) };
            right.AddRow(fraRequirements);

            // Wrap mid+right in a scrollable so the editor gets a vertical scrollbar when content overflows
            var midRight = new TableLayout
            {
                Spacing = new Size(8,8),
                Rows = { new TableRow(mid, right) }
            };
            var scroll = new Scrollable
            {
                Content = midRight,
                ExpandContentWidth = true // avoid horizontal scrollbar; content stretches to available width
            };

            return new Eto.Forms.TableLayout
            {
                Padding = 4,
                Spacing = new Size(8,8),
                Rows = { new TableRow(left, scroll) }
            };
        }

        NumericStepper Num(int min, int max) => new NumericStepper { MinValue = min, MaxValue = max, Increment = 1 };

        NumericStepper StatReq(Stat stat)
        {
            var n = Num(0, 999);
            n.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].StatReq[(int)stat] = (byte)n.Value; MarkChanged(); };
            return n;
        }
        NumericStepper StatAdd(Stat stat)
        {
            var n = Num(0, 999);
            n.ValueChanged += (s,e)=> { Data.Item[GameState.EditorIndex].AddStat[(int)stat] = (byte)n.Value; MarkChanged(); };
            return n;
        }

        GroupBox Group(string text, Eto.Forms.Control content) => new GroupBox { Text = text, Content = content };

    void InitData()
        {
            _suppressIndexChanged = true;
            try
            {
                lstIndex!.Items.Clear();
                for (int i = 0; i < Constant.MaxItems; i++) lstIndex.Items.Add((i+1)+": "+Data.Item[i].Name);
                lstIndex.SelectedIndex = GameState.EditorIndex >= 0 ? GameState.EditorIndex : 0;

                cmbAnimation!.Items.Clear(); for (int i=0;i<Constant.MaxAnimations;i++) cmbAnimation.Items.Add((i+1)+": "+Data.Animation[i].Name);
                cmbProjectile!.Items.Clear(); for (int i=0;i<Constant.MaxVariables;i++) cmbProjectile.Items.Add((i+1)+": "+Data.Projectile[i].Name);
                cmbAmmo!.Items.Clear(); for (int i=0;i<Constant.MaxItems;i++) cmbAmmo.Items.Add((i+1)+": "+Data.Item[i].Name);
                cmbSkills!.Items.Clear(); for (int i=0;i<Constant.MaxSkills;i++) cmbSkills.Items.Add((i+1)+": "+Data.Skill[i].Name);
                cmbJobReq!.Items.Clear(); for (int i=0;i<Constant.MaxJobs;i++) cmbJobReq.Items.Add(Data.Job[i].Name);
                cmbAccessReq!.Items.Clear();
                foreach (var name in Enum.GetNames(typeof(Core.Globals.AccessLevel)))
                    cmbAccessReq.Items.Add(name);
                cmbBind!.Items.Clear(); cmbBind.Items.Add("None"); cmbBind.Items.Add("Pickup"); cmbBind.Items.Add("Equip");
                cmbTool!.Items.Clear();
                foreach (var name in Enum.GetNames(typeof(ToolType)))
                    cmbTool.Items.Add(name);
                cmbKnockBackTiles!.Items.Clear(); for(int i=0;i<6;i++) cmbKnockBackTiles.Items.Add(i+" tile");

                cmbType!.Items.Clear();
                foreach(var name in Enum.GetNames(typeof(ItemCategory))) cmbType.Items.Add(name);
            }
            finally { _suppressIndexChanged = false; }

            Editors.ItemEditorInit(); // will populate controls & preview
            TogglePanels();
            LoadItemIcon(); LoadPaperdoll();
        }

        void UpdateName()
        {
            Data.Item[GameState.EditorIndex].Name = Strings.Trim(txtName!.Text);
            if (lstIndex!.SelectedIndex >= 0)
            {
                int i = lstIndex.SelectedIndex;
                _suppressIndexChanged = true;
                try
                {
                    lstIndex.Items.RemoveAt(i);
                    lstIndex.Items.Insert(i, new ListItem{ Text = (i+1)+": "+ Data.Item[i].Name });
                    lstIndex.SelectedIndex = i;
                }
                finally { _suppressIndexChanged = false; }
            }
            MarkChanged();
        }

        void ChangeType()
        {
            Data.Item[GameState.EditorIndex].Type = (byte)cmbType!.SelectedIndex;
            BuildSubtypeList();
            TogglePanels();
            MarkChanged();
        }

        void BuildSubtypeList()
        {
            cmbSubType!.Items.Clear();
            cmbSubType.Enabled = false;
            var type = (ItemCategory)cmbType!.SelectedIndex;
            switch(type)
            {
                case ItemCategory.Equipment:
                    cmbSubType.Items.Add("Weapon");
                    cmbSubType.Items.Add("Armor");
                    cmbSubType.Items.Add("Helmet");
                    cmbSubType.Items.Add("Shield");
                    cmbSubType.Enabled = true;
                    break;
                case ItemCategory.Consumable:
                    cmbSubType.Items.Add("HP");
                    cmbSubType.Items.Add("SP");
                    cmbSubType.Items.Add("Exp");
                    cmbSubType.Enabled = true;
                    break;
                case ItemCategory.Event:
                    cmbSubType.Items.Add("Switches");
                    cmbSubType.Items.Add("Variables");
                    cmbSubType.Items.Add("Custom Script");
                    cmbSubType.Items.Add("Key");
                    cmbSubType.Enabled = true;
                    break;
            }
            if (cmbSubType.Items.Count > 0)
            {
                var sub = Data.Item[GameState.EditorIndex].SubType;
                if (sub < 0 || sub >= cmbSubType.Items.Count) sub = 0; 
                cmbSubType.SelectedIndex = sub;
            }
        }

        void TogglePanels()
        {
            var type = (ItemCategory)cmbType!.SelectedIndex;
            fraEquipment!.Visible = type == ItemCategory.Equipment || type == ItemCategory.Projectile;
            fraVitals!.Visible = type == ItemCategory.Consumable;
            fraSkill!.Visible = type == ItemCategory.Skill;
            fraProjectile!.Visible = type == ItemCategory.Projectile || (type == ItemCategory.Equipment && Data.Item[GameState.EditorIndex].SubType == (byte)Equipment.Weapon);
            fraEvents!.Visible = type == ItemCategory.Event;
        }


        void LoadItemIcon()
        {
            itemBmp?.Dispose(); itemBmp = null;
            int num = (int)numIcon!.Value;
            if (num >=1 && num <= GameState.NumItems)
            {
                var path = System.IO.Path.Combine(DataPath.Items, num + GameState.GfxExt);
                if (File.Exists(path)) itemBmp = new Bitmap(path);
            }
            iconPreview!.Invalidate();
        }

        void LoadPaperdoll()
        {
            paperdollBmp?.Dispose(); paperdollBmp = null;
            int num = (int)numPaperdoll!.Value;
            if (num >=1 && num <= GameState.NumPaperdolls)
            {
                var path = System.IO.Path.Combine(DataPath.Paperdolls, num + GameState.GfxExt);
                if (File.Exists(path)) paperdollBmp = new Bitmap(path);
            }
            paperdollPreview!.Invalidate();
        }

    void MarkChanged()
        {
            if (GameState.EditorIndex >= 0 && GameState.EditorIndex < GameState.ItemChanged.Length)
                GameState.ItemChanged[GameState.EditorIndex] = true;
        }

    // Legacy alias properties for Editors.cs naming (nud*, cmb*)
    public NumericStepper nudIcon => numIcon!;
    public NumericStepper nudPaperdoll => numPaperdoll!;
    public NumericStepper nudItemLvl => numItemLvl!;
    public NumericStepper nudPrice => numPrice!;
    public NumericStepper nudRarity => numRarity!;
    public NumericStepper nudSpeed => numSpeed!;
    public NumericStepper nudDamage => numDamage!;
    public NumericStepper nudVitalMod => numVitalMod!;
    public NumericStepper nudEvent => numEventId!;
    public NumericStepper nudEventValue => numEventValue!;
    public NumericStepper nudLevelReq => numLevelReq!;
    public NumericStepper nudStrReq => numStrReq!;
    public NumericStepper nudVitReq => numVitReq!;
    public NumericStepper nudLuckReq => numLuckReq!;
    public NumericStepper nudIntReq => numIntReq!;
    public NumericStepper nudSprReq => numSprReq!;
    public NumericStepper nudStrength => numStrAdd!;
    public NumericStepper nudIntelligence => numIntAdd!;
    public NumericStepper nudVitality => numVitAdd!;
    public NumericStepper nudLuck => numLuckAdd!;
    public NumericStepper nudSpirit => numSprAdd!;
    // ComboBoxes already public by field declaration; no additional alias properties needed

    // Legacy drawing methods accessed by Editors.cs
    public void DrawIcon() { iconPreview?.Invalidate(); }

        protected override void OnClosed(EventArgs e)
        {
            itemBmp?.Dispose(); paperdollBmp?.Dispose();
            Editors.ItemEditorCancel();
            base.OnClosed(e);
        }

        void CopyOrPasteItem()
        {
            int src = GameState.EditorIndex;
            if (!_hasClipboardItem)
            {
                if (src < 0 || src >= Constant.MaxItems) return;
                // Copy
                var s = Data.Item[src];
                _clipboardItem = s;
                if (s.AddStat != null) _clipboardItem.AddStat = (byte[])s.AddStat.Clone();
                if (s.StatReq != null) _clipboardItem.StatReq = (byte[])s.StatReq.Clone();
                _hasClipboardItem = true;
                btnCopy!.Text = "Paste";
                return;
            }

            // Paste
            int def = GameState.EditorIndex + 1;
            var oneBased = Editors.PromptIndex(this, "Paste Item", $"Paste item into index (1..{Constant.MaxItems}):", 1, Constant.MaxItems, def);
            if (oneBased == null) return;
            int dst = oneBased.Value - 1;
            var n = _clipboardItem;
            if (n.AddStat != null) n.AddStat = (byte[])n.AddStat.Clone();
            if (n.StatReq != null) n.StatReq = (byte[])n.StatReq.Clone();
            Data.Item[dst] = n;
            GameState.ItemChanged[dst] = true;

            if (lstIndex != null && dst >= 0 && dst < lstIndex.Items.Count)
            {
                _suppressIndexChanged = true;
                try
                {
                    lstIndex.Items.RemoveAt(dst);
                    lstIndex.Items.Insert(dst, new ListItem { Text = (dst + 1) + ": " + Data.Item[dst].Name });
                    lstIndex.SelectedIndex = dst;
                }
                finally { _suppressIndexChanged = false; }
            }
            GameState.EditorIndex = dst;
            Editors.ItemEditorInit();
        }
    }
}