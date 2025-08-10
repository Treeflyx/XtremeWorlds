using System;
using Client.Net;
using Core.Localization;
using Microsoft.VisualBasic;
using static Core.Global.Command;

namespace Client
{

    internal partial class Admin
    {
        protected override void WndProc(ref Message m)
        {
            const int WM_MOUSEACTIVATE = 0x0021;
            const int WM_NCHITTEST = 0x0084;

            if (m.Msg == WM_MOUSEACTIVATE)
            {
                // Immediately activate and process the click.
                m.Result = new IntPtr(1); // MA_ACTIVATE
                return;
            }
            else if (m.Msg == WM_NCHITTEST)
            {
                // Let the window know the mouse is in client area.
                m.Result = new IntPtr(1); // HTCLIENT
                return;
            }

            base.WndProc(ref m);
        }

        public Admin()
        {
            InitializeComponent();
        }

        private void FrmAdmin_Load(object sender, EventArgs e)
        {
            Sender.SendRequestMapReport();
            cmbAccess.SelectedIndex = 0;
        }

        #region Moderation

        private void BtnAdminWarpTo_Click(object sender, EventArgs e)
        {

            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.WarpTo((int)Math.Round(nudAdminMap.Value));
        }

        private void BtnAdminBan_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendBan(Strings.Trim(txtAdminName.Text));
        }

        private void BtnAdminKick_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendKick(Strings.Trim(txtAdminName.Text));
        }

        private void BtnAdminWarp2Me_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            if (Information.IsNumeric(Strings.Trim(txtAdminName.Text)))
                return;

            Sender.WarpToMe(Strings.Trim(txtAdminName.Text));
        }

        private void BtnAdminWarpMe2_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            if (Information.IsNumeric(Strings.Trim(txtAdminName.Text)))
            {
                return;
            }

            Sender.WarpMeTo(Strings.Trim(txtAdminName.Text));
        }

        private void BtnAdminSetAccess_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Owner)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            if (Information.IsNumeric(Strings.Trim(txtAdminName.Text)) | cmbAccess.SelectedIndex < 0)
            {
                return;
            }

            Sender.SendSetAccess(txtAdminName.Text, (byte)(cmbAccess.SelectedIndex + 1));
        }

        private void BtnAdminSetSprite_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendSetSprite((int)Math.Round(nudAdminSprite.Value));
        }

        #endregion

        #region Editors

        private void btnAnimationEditor_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendRequestEditAnimation();
        }

        private void btnJobEditor_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendRequestEditJob();
        }

        private void btnItemEditor_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendRequestEditItem();
        }

        private void BtnMapEditor_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Map.SendRequestEditMap();
        }

        private void btnNpcEditor_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendRequestEditNpc();
        }
        private void btnProjectiles_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Projectile.SendRequestEditProjectiles();
        }

        private void btnResourceEditor_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendRequestEditResource();
        }

        private void btnShopEditor_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendRequestEditShop();
        }

        private void btnSkillEditor_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendRequestEditSkill();
        }

        private void FrmAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            GameState.AdminPanel = false;
        }

        private void BtnMapReport_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }
            Sender.SendRequestMapReport();
        }

        private void LstMaps_DoubleClick(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            if (lstMaps.SelectedIndices.Count == 0 || lstMaps.SelectedIndices[0] == 0)
                return;

            Sender.WarpTo(lstMaps.SelectedIndices[0] + 1);
        }

        #endregion

        #region Misc
        private void BtnLevelUp_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendRequestLevelUp();
        }

        private void BtnALoc_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            GameState.BLoc = !GameState.BLoc;
        }

        private void BtnRespawn_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Mapper)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Map.SendMapRespawn();
        }

        private void btnMoralEditor_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core.AccessLevel.Developer)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendRequestEditMoral();
        }


        private void btnScriptEditor_Click(object sender, EventArgs e)
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int)Core. AccessLevel.Owner)
            {
                Client.Text.AddText(LocalesManager.Get("AccessDenied"), (int)Core.Color.BrightRed);
                return;
            }

            Sender.SendRequestEditScript(0);
        }

        #endregion
    }
}