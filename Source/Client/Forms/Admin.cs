using System;
using Client.Game.UI;
using Client.Net;
using Core.Configurations;
using Core.Globals;
using static Core.Globals.Command;
using Eto.Forms;
using Eto.Drawing;

namespace Client
{
    internal class Admin : Form
    {

        // Singleton instance for legacy static access
        private static Admin? _instance;
        public static Admin Instance => _instance ??= new Admin();

        // Controls
        public TextBox txtAdminName = null!;
        public DropDown cmbAccess = null!;
        public NumericStepper nudAdminSprite = null!, nudAdminMap = null!;
        public ListBox lstMaps = null!;
        public TabControl tabControl = null!;
        public Button btnAdminWarpTo = null!, btnAdminBan = null!, btnAdminKick = null!, btnAdminWarp2Me = null!, btnAdminWarpMe2 = null!, btnAdminSetAccess = null!, btnAdminSetSprite = null!;
        public Button btnMapReport = null!, btnRespawn = null!, btnALoc = null!, btnLevelUp = null!;
        public Button btnAnimationEditor = null!, btnJobEditor = null!, btnItemEditor = null!, btnMapEditor = null!, btnNpcEditor = null!, btnProjectiles = null!, btnResourceEditor = null!, btnShopEditor = null!, btnSkillEditor = null!, btnMoralEditor = null!, btnScriptEditor = null!;

        public Admin()
        {
            _instance = this;
            Title = "Admin Panel";
            ClientSize = new Size(640, 600);
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Moderation Tab
            // Initialize text and dropdown controls first to avoid null refs in event handlers
            txtAdminName = new TextBox(); 
            txtAdminName.Text = GetPlayerName(GameState.MyIndex); 
            cmbAccess = new DropDown();
            foreach (var name in Enum.GetNames(typeof(AccessLevel)))
            {
                cmbAccess.Items.Add(name);
            }

            nudAdminSprite = new NumericStepper { MinValue = 0, MaxValue = GameState.NumCharacters };
            nudAdminMap = new NumericStepper { MinValue = 0, MaxValue = Constant.MaxMaps };

            btnAdminWarpTo = new Button { Text = "Warp To Map" };
            btnAdminBan = new Button { Text = "Ban Player" };
            btnAdminKick = new Button { Text = "Kick Player" };
            btnAdminWarp2Me = new Button { Text = "Warp Player To Me" };
            btnAdminWarpMe2 = new Button { Text = "Warp Me To Player" };
            btnAdminSetAccess = new Button { Text = "Set Access" };
            btnAdminSetSprite = new Button { Text = "Set Player Sprite" };
            btnLevelUp = new Button { Text = "Level Up" };

            btnAdminWarpTo.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Mapper) ShowDenied(); else Sender.WarpTo((int)nudAdminMap.Value); };
            btnAdminBan.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Mapper) ShowDenied(); else Sender.SendBan(txtAdminName.Text.Trim()); };
            btnAdminKick.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Mapper) ShowDenied(); else Sender.SendKick(txtAdminName.Text.Trim()); };
            btnAdminWarp2Me.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Mapper) ShowDenied(); else if (!IsNumeric(txtAdminName.Text.Trim())) Sender.WarpToMe(txtAdminName.Text.Trim()); };
            btnAdminWarpMe2.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Mapper) ShowDenied(); else if (!IsNumeric(txtAdminName.Text.Trim())) Sender.WarpMeTo(txtAdminName.Text.Trim()); };
            btnAdminSetAccess.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Owner) ShowDenied(); else if (!IsNumeric(txtAdminName.Text.Trim()) && cmbAccess.SelectedIndex >= 0) Sender.SendSetAccess(txtAdminName.Text, (byte)(cmbAccess.SelectedIndex + 1)); };
            btnAdminSetSprite.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Mapper) ShowDenied(); else Sender.SendSetSprite((int)nudAdminSprite.Value); };

            var moderationLayout = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows =
                {
                    new TableRow(new Label{Text="Player Name:"}, txtAdminName),
                    new TableRow(new Label{Text="Access:"}, cmbAccess, btnAdminSetAccess),
                    new TableRow(new Label{Text="Map Number:"}, nudAdminMap, btnAdminWarpTo),
                    new TableRow(new Label{Text="Sprite:"}, nudAdminSprite, btnAdminSetSprite),
                    // Split action buttons across two rows for clearer sizing
                    new TableRow(btnAdminBan, btnAdminKick, btnLevelUp),
                    new TableRow(btnAdminWarp2Me, btnAdminWarpMe2)
                }
            };

            // Map List Tab
            // Enlarge map list area
            lstMaps = new ListBox { Size = new Size(-1, 420) }; // taller list
            btnMapReport = new Button { Text = "Refresh List" };
            btnMapReport.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Mapper) ShowDenied(); else Sender.SendRequestMapReport(); };
            lstMaps.MouseDoubleClick += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Mapper) ShowDenied(); else if (lstMaps.SelectedIndex >= 0) Sender.WarpTo(lstMaps.SelectedIndex + 1); };
            var mapListLayout = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows = { lstMaps, btnMapReport }
            };

            // Map Tools Tab
            btnRespawn = new Button { Text = "Respawn Map" };
            btnALoc = new Button { Text = "Location" };
            btnRespawn.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Mapper) ShowDenied(); else Map.SendMapRespawn(); };
            btnALoc.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Mapper) ShowDenied(); else GameState.BLoc = !GameState.BLoc; };
            var mapToolsLayout = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows = { btnRespawn, btnALoc }
            };

            // Editors Tab
            btnAnimationEditor = new Button { Text = "Animation Editor" };
            btnJobEditor = new Button { Text = "Job Editor" };
            btnItemEditor = new Button { Text = "Item Editor" };
            btnMapEditor = new Button { Text = "Map Editor" };
            btnNpcEditor = new Button { Text = "Npc Editor" };
            btnProjectiles = new Button { Text = "Projectile Editor" };
            btnResourceEditor = new Button { Text = "Resource Editor" };
            btnShopEditor = new Button { Text = "Shop Editor" };
            btnSkillEditor = new Button { Text = "Skill Editor" };
            btnMoralEditor = new Button { Text = "Moral Editor" };
            btnScriptEditor = new Button { Text = "Script Editor" };

            btnLevelUp.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Developer) ShowDenied(); else Sender.SendRequestLevelUp(); };
            btnAnimationEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Developer) ShowDenied(); else Sender.SendRequestEditAnimation(); };
            btnJobEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Developer) ShowDenied(); else Sender.SendRequestEditJob(); };
            btnItemEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Developer) ShowDenied(); else Sender.SendRequestEditItem(); };
            btnMapEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Mapper) ShowDenied(); else Map.SendRequestEditMap(); };
            btnNpcEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Developer) ShowDenied(); else Sender.SendRequestEditNpc(); };
            btnProjectiles.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Developer) ShowDenied(); else Projectile.SendRequestEditProjectiles(); };
            btnResourceEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Developer) ShowDenied(); else Sender.SendRequestEditResource(); };
            btnShopEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Developer) ShowDenied(); else Sender.SendRequestEditShop(); };
            btnSkillEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Developer) ShowDenied(); else Sender.SendRequestEditSkill(); };
            btnMoralEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Developer) ShowDenied(); else Sender.SendRequestEditMoral(); };
            btnScriptEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)AccessLevel.Owner) ShowDenied(); else Sender.SendRequestEditScript(0); };

            // Editors buttons stretch to fill width
            var editorsLayout = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows =
                {
                    new TableRow(
                        new TableCell(btnAnimationEditor, true),
                        new TableCell(btnJobEditor, true),
                        new TableCell(btnItemEditor, true),
                        new TableCell(btnMapEditor, true)
                    ),
                    new TableRow(
                        new TableCell(btnNpcEditor, true),
                        new TableCell(btnProjectiles, true),
                        new TableCell(btnResourceEditor, true),
                        new TableCell(btnShopEditor, true)
                    ),
                    new TableRow(
                        new TableCell(btnSkillEditor, true),
                        new TableCell(btnMoralEditor, true),
                        new TableCell(btnScriptEditor, true),
                        new TableCell(new Panel(), true) // spacer cell to balance columns
                    )
                }
            };

            tabControl = new TabControl
            {
                Pages =
                {
                    new TabPage { Text = "Moderation", Content = moderationLayout },
                    new TabPage { Text = "Map List", Content = mapListLayout },
                    new TabPage { Text = "Map Tools", Content = mapToolsLayout },
                    new TabPage { Text = "Editors", Content = editorsLayout }
                }
            };

            Content = tabControl;
            this.Closed += (s, e) => { GameState.AdminPanel = false; };
        }

        private void ShowDenied()
        {
            TextRenderer.AddText(LocalesManager.Get("AccessDenied"), (int)ColorName.BrightRed);
        }

        private bool IsNumeric(string s)
        {
            return int.TryParse(s, out _);
        }
    }
}
