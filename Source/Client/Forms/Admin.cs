using System;
using Client.Net;
using Core.Localization;
using static Core.Global.Command;

namespace Client
{
    internal class Admin : Form
    {
        // Controls
        TextBox txtAdminName;
        DropDown cmbAccess;
        NumericStepper nudAdminSprite, nudAdminMap;
        ListBox lstMaps;
        TabControl tabControl;
        Button btnAdminWarpTo, btnAdminBan, btnAdminKick, btnAdminWarp2Me, btnAdminWarpMe2, btnAdminSetAccess, btnAdminSetSprite;
        Button btnMapReport, btnRespawn, btnALoc, btnLevelUp;
        Button btnAnimationEditor, btnJobEditor, btnItemEditor, btnMapEditor, btnNpcEditor, btnProjectiles, btnResourceEditor, btnShopEditor, btnSkillEditor, btnMoralEditor, btnScriptEditor;

        public Admin()
        {
            Title = "Admin Panel";
            ClientSize = new Size(600, 700);
            Resizable = false;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Moderation Tab
            txtAdminName = new TextBox { PlaceholderText = "Player Name" };
            cmbAccess = new DropDown { Items = { "Normal Player", "Moderator (GM)", "Mapper", "Developer", "Owner" }, SelectedIndex = 0 };
            nudAdminSprite = new NumericUpDown { MinValue = 0, MaxValue = GameState.NumCharacters};
            nudAdminMap = new NumericUpDown { MinValue = 0, MaxValue = Core.Constant.MaxMaps };

            btnAdminWarpTo = new Button { Text = "Warp To Map" };
            btnAdminBan = new Button { Text = "Ban Player" };
            btnAdminKick = new Button { Text = "Kick Player" };
            btnAdminWarp2Me = new Button { Text = "Warp Player To Me" };
            btnAdminWarpMe2 = new Button { Text = "Warp Me To Player" };
            btnAdminSetAccess = new Button { Text = "Set Access" };
            btnAdminSetSprite = new Button { Text = "Set Player Sprite" };

            btnAdminWarpTo.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper) ShowDenied(); else NetworkSend.WarpTo((int)nudAdminMap.Value); };
            btnAdminBan.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper) ShowDenied(); else NetworkSend.SendBan(txtAdminName.Text.Trim()); };
            btnAdminKick.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper) ShowDenied(); else NetworkSend.SendKick(txtAdminName.Text.Trim()); };
            btnAdminWarp2Me.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper) ShowDenied(); else if (!IsNumeric(txtAdminName.Text.Trim())) NetworkSend.WarpToMe(txtAdminName.Text.Trim()); };
            btnAdminWarpMe2.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper) ShowDenied(); else if (!IsNumeric(txtAdminName.Text.Trim())) NetworkSend.WarpMeTo(txtAdminName.Text.Trim()); };
            btnAdminSetAccess.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Owner) ShowDenied(); else if (!IsNumeric(txtAdminName.Text.Trim()) && cmbAccess.SelectedIndex >= 0) NetworkSend.SendSetAccess(txtAdminName.Text, (byte)(cmbAccess.SelectedIndex + 1)); };
            btnAdminSetSprite.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper) ShowDenied(); else NetworkSend.SendSetSprite((int)nudAdminSprite.Value); };

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
                    new TableRow(btnAdminBan, btnAdminKick, btnAdminWarp2Me, btnAdminWarpMe2),
                }
            };

            // Map List Tab
            lstMaps = new ListBox();
            btnMapReport = new Button { Text = "Refresh List" };
            btnMapReport.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper) ShowDenied(); else NetworkSend.SendRequestMapReport(); };
            lstMaps.MouseDoubleClick += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper) ShowDenied(); else if (lstMaps.SelectedIndex >= 0) NetworkSend.WarpTo(lstMaps.SelectedIndex + 1); };
            var mapListLayout = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows = { lstMaps, btnMapReport }
            };

            // Map Tools Tab
            btnRespawn = new Button { Text = "Respawn Map" };
            btnALoc = new Button { Text = "Location" };
            btnRespawn.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper) ShowDenied(); else Map.SendMapRespawn(); };
            btnALoc.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper) ShowDenied(); else GameState.BLoc = !GameState.BLoc; };
            var mapToolsLayout = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows = { btnRespawn, btnALoc }
            };

            // Editors Tab
            btnLevelUp = new Button { Text = "Level Up" };
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

            btnLevelUp.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer) ShowDenied(); else NetworkSend.SendRequestLevelUp(); };
            btnAnimationEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer) ShowDenied(); else NetworkSend.SendRequestEditAnimation(); };
            btnJobEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer) ShowDenied(); else NetworkSend.SendRequestEditJob(); };
            btnItemEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer) ShowDenied(); else NetworkSend.SendRequestEditItem(); };
            btnMapEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper) ShowDenied(); else Map.SendRequestEditMap(); };
            btnNpcEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer) ShowDenied(); else NetworkSend.SendRequestEditNpc(); };
            btnProjectiles.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer) ShowDenied(); else Projectile.SendRequestEditProjectiles(); };
            btnResourceEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer) ShowDenied(); else NetworkSend.SendRequestEditResource(); };
            btnShopEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer) ShowDenied(); else NetworkSend.SendRequestEditShop(); };
            btnSkillEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer) ShowDenied(); else NetworkSend.SendRequestEditSkill(); };
            btnMoralEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer) ShowDenied(); else NetworkSend.SendRequestEditMoral(); };
            btnScriptEditor.Click += (s, e) => { if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Owner) ShowDenied(); else NetworkSend.SendRequestEditScript(0); };

            var editorsLayout = new TableLayout
            {
                Padding = 10,
                Spacing = new Size(5, 5),
                Rows =
                {
                    new TableRow(btnLevelUp, btnAnimationEditor, btnJobEditor, btnItemEditor),
                    new TableRow(btnMapEditor, btnNpcEditor, btnProjectiles, btnResourceEditor),
                    new TableRow(btnShopEditor, btnSkillEditor, btnMoralEditor, btnScriptEditor)
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
            Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
        }

        private bool IsNumeric(string s)
        {
            return int.TryParse(s, out _);
        }
    }
}
