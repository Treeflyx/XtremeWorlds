using System;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using Microsoft.VisualBasic;
using Core;
using Core.Globals;

namespace Client
{
    public sealed class Editor_Projectile : Form
    {
        // Singleton access for legacy usage
        private static Editor_Projectile? _instance;
        public static Editor_Projectile Instance => _instance ??= new Editor_Projectile();
        public ListBox lstIndex = null!;
        public TextBox txtName = null!;
        public NumericStepper nudPic = null!;
        public NumericStepper nudRange = null!;
        public NumericStepper nudSpeed = null!;
        public NumericStepper nudDamage = null!;
        public Drawable picProjectile = null!;

        public Button btnSave = null!;
        public Button btnCancel = null!;
        public Button btnDelete = null!;

        private bool _initializing;

        public Editor_Projectile()
        {
            _instance = this;
            Title = "Projectile Editor";
            ClientSize = new Size(750, 430);
            Resizable = true;
            MinimumSize = new Size(750, 430);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Left list
            lstIndex = new ListBox { Width = 220 }; // make width consistent and allow vertical expansion via StackLayoutItem
            lstIndex.SelectedIndexChanged += (s, e) =>
            {
                if (_initializing) return;
                if (lstIndex.SelectedIndex < 0) return;
                Editors.ProjectileEditorInit();
            };

            // Right side controls
            txtName = new TextBox();
            txtName.TextChanged += (s, e) =>
            {
                if (_initializing) return;
                int tmpindex = lstIndex.SelectedIndex;
                if (tmpindex < 0) return;
                Data.Projectile[GameState.EditorIndex].Name = Strings.Trim(txtName.Text);
                RefreshListEntry(GameState.EditorIndex);
                lstIndex.SelectedIndex = tmpindex;
                GameState.ProjectileChanged[GameState.EditorIndex] = true;
            };

            nudPic = new NumericStepper { MinValue = 0, MaxValue = GameState.NumProjectiles, DecimalPlaces = 0, Width = 80 };
            nudPic.ValueChanged += (s, e) =>
            {
                if (_initializing) return;
                Data.Projectile[GameState.EditorIndex].Sprite = (int)nudPic.Value;
                Drawicon();
                GameState.ProjectileChanged[GameState.EditorIndex] = true;
            };

            nudRange = new NumericStepper { MinValue = 0, MaxValue = 255, DecimalPlaces = 0, Width = 80 };
            nudRange.ValueChanged += (s, e) =>
            {
                if (_initializing) return;
                Data.Projectile[GameState.EditorIndex].Range = (byte)nudRange.Value;
                GameState.ProjectileChanged[GameState.EditorIndex] = true;
            };

            nudSpeed = new NumericStepper { MinValue = 0, MaxValue = 1000, DecimalPlaces = 0, Width = 80 };
            nudSpeed.ValueChanged += (s, e) =>
            {
                if (_initializing) return;
                Data.Projectile[GameState.EditorIndex].Speed = (int)nudSpeed.Value;
                GameState.ProjectileChanged[GameState.EditorIndex] = true;
            };

            nudDamage = new NumericStepper { MinValue = 0, MaxValue = 100000, DecimalPlaces = 0, Width = 80 };
            nudDamage.ValueChanged += (s, e) =>
            {
                if (_initializing) return;
                Data.Projectile[GameState.EditorIndex].Damage = (int)nudDamage.Value;
                GameState.ProjectileChanged[GameState.EditorIndex] = true;
            };

            picProjectile = new Drawable { Size = new Size(96, 96), BackgroundColor = Colors.Black };
            picProjectile.Paint += (s, e) =>
            {
                if (_iconBitmap != null)
                {
                    e.Graphics.DrawImage(_iconBitmap, 0, 0, 96, 96);
                }
            };

            btnSave = new Button { Text = "Save" };
            btnSave.Click += (s, e) =>
            {
                Editors.ProjectileEditorOK();
                Close();
            };

            btnCancel = new Button { Text = "Cancel" };
            btnCancel.Click += (s, e) =>
            {
                Editors.ProjectileEditorCancel();
                Close();
            };

            btnDelete = new Button { Text = "Delete" };
            btnDelete.Click += (s, e) =>
            {
                if (lstIndex.SelectedIndex < 0) return;
                Projectile.ClearProjectile(GameState.EditorIndex);
                RefreshListEntry(GameState.EditorIndex);
                Editors.ProjectileEditorInit();
            };

            var grid = new TableLayout
            {
                Spacing = new Size(6, 6),
                Padding = new Padding(8),
                Rows =
                {
                    new TableRow(new TableCell(new Label{Text="Name:", VerticalAlignment=VerticalAlignment.Center}, false), txtName),
                    new TableRow(new TableCell(new Label{Text="Sprite:", VerticalAlignment=VerticalAlignment.Center}, false), nudPic),
                    new TableRow(new TableCell(new Label{Text="Range:", VerticalAlignment=VerticalAlignment.Center}, false), nudRange),
                    new TableRow(new TableCell(new Label{Text="Speed:", VerticalAlignment=VerticalAlignment.Center}, false), nudSpeed),
                    new TableRow(new TableCell(new Label{Text="Damage:", VerticalAlignment=VerticalAlignment.Center}, false), nudDamage),
                    new TableRow(new TableCell(new Label{Text="Preview:", VerticalAlignment=VerticalAlignment.Center}, false), picProjectile),
                    new TableRow(new TableCell(null, true), new StackLayout
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 6,
                        Items = { btnSave, btnDelete, btnCancel } // enforce order
                    })
                }
            };

            Content = new Splitter
            {
                Position = 240,
                Panel1 = new StackLayout
                {
                    Padding = 8,
                    Spacing = 4,
                    Items =
                    {
                        new Label{ Text = "Projectiles", Font = SystemFonts.Bold(12)},
                        new StackLayoutItem(lstIndex, expand: true)
                    }
                },
                Panel2 = new Scrollable { Content = grid }
            };

            Shown += (s, e) => LoadData();
            Closed += (s, e) =>
            {
                if (GameState.MyEditorType == EditorType.Projectile)
                {
                    Editors.ProjectileEditorCancel();
                }
                if (_instance == this) _instance = null;
            };
        }

        private void LoadData()
        {
            _initializing = true;
            lstIndex.Items.Clear();
            for (int i = 0; i < Constant.MaxProjectiles; i++)
            {
                lstIndex.Items.Add(new ListItem { Text = (i + 1) + ": " + Data.Projectile[i].Name });
            }
            if (lstIndex.Items.Count > 0) lstIndex.SelectedIndex = 0;
            nudPic.MaxValue = GameState.NumProjectiles;
            _initializing = false;
            if (lstIndex.Items.Count > 0) Editors.ProjectileEditorInit();
        }

        private void RefreshListEntry(int index)
        {
            if (index < 0 || index >= lstIndex.Items.Count) return;
            // Eto ListBox uses ListItem objects; replace the text
            if (lstIndex.Items[index] is ListItem item)
            {
                item.Text = (index + 1) + ": " + Data.Projectile[index].Name;
                lstIndex.Invalidate();
            }
        }
        private Bitmap? _iconBitmap;

        public void Drawicon()
        {
            int iconNum = (int)nudPic.Value;

            _iconBitmap = null;
            picProjectile.Invalidate();

            if (iconNum < 1 || iconNum > GameState.NumProjectiles) return;

            var path = System.IO.Path.Combine(DataPath.Projectiles, iconNum + GameState.GfxExt);
            if (!File.Exists(path)) return;

            try
            {
                using (var fs = File.OpenRead(path))
                {
                    _iconBitmap = new Bitmap(fs);
                }
            }
            catch
            {
                _iconBitmap = null;
            }
            picProjectile.Invalidate();
        }
    }
}