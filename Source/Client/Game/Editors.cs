using System;
using Client.Net;
using Core;
using Core.Globals;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Eto.Forms;
using Eto.Drawing;

namespace Client
{

    static class Editors
    {
        // Auto size helper: adjusts window size to preferred content size within bounds on first show
        public static void AutoSizeWindow(Form form, int minWidth = 400, int minHeight = 300, int maxWidth = 1600, int maxHeight = 1000)
        {
            form.Shown += (s, e) =>
            {
                try
                {
                    if (form.Content == null) return;
                    var pref = form.Content.GetPreferredSize(Size.MaxValue);
                    int w = Math.Min(Math.Max((int)pref.Width + form.Padding.Left + form.Padding.Right, minWidth), maxWidth);
                    int h = Math.Min(Math.Max((int)pref.Height + form.Padding.Top + form.Padding.Bottom, minHeight), maxHeight);
                    // Only enlarge; don't shrink below current if user already resized
                    if (w > form.ClientSize.Width || h > form.ClientSize.Height)
                        form.ClientSize = new Size(w, h);
                }
                catch { /* ignore sizing errors */ }
            };
        }

        // Simple modal numeric prompt to replace VB Interaction.InputBox on cross-platform
        public static int? PromptIndex(Form owner, string title, string message, int min, int max, int defaultValue)
        {
            var dlg = new Dialog { Title = title, ClientSize = new Size(360, 140), Padding = 10 };
            var num = new NumericStepper { MinValue = min, MaxValue = max, Value = defaultValue, DecimalPlaces = 0 };
            var ok = new Button { Text = "OK" };
            var cancel = new Button { Text = "Cancel" };
            int? result = null;
            ok.Click += (s, e) => { result = (int)Math.Round(num.Value); dlg.Close(); };
            cancel.Click += (s, e) => { result = null; dlg.Close(); };
            var layout = new DynamicLayout { Spacing = new Size(6, 6) };
            layout.AddRow(new Label { Text = message });
            layout.AddRow(num);
            layout.AddRow(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 6, Items = { ok, cancel } });
            dlg.Content = layout;
            dlg.ShowModal(owner);
            return result;
        }

        #region Animation Editor

        public static void AnimationEditorInit()
        {  
            ref var withBlock = ref Data.Animation[GameState.EditorIndex];
            EnsureAnimationArrays(ref withBlock);
            if (string.IsNullOrEmpty(withBlock.Sound))
            {
                Editor_Animation.Instance!.cmbSound!.SelectedIndex = 0;
            }
            else
            {
                for (int i = 0, loopTo = Editor_Animation.Instance!.cmbSound!.Items.Count; i < loopTo; i++)
                {
                    var raw = Editor_Animation.Instance!.cmbSound!.Items[i];
                    string text = raw switch { Eto.Forms.ListItem li => li.Text, _ => raw?.ToString() ?? string.Empty };
                    if (text == withBlock.Sound)
                    {
                        Editor_Animation.Instance!.cmbSound!.SelectedIndex = i;
                        break;
                    }
                }
            }
            Editor_Animation.Instance!.txtName!.Text = withBlock.Name;

            Editor_Animation.Instance!.nudSprite0!.Value = withBlock.Sprite[0];
            Editor_Animation.Instance!.nudFrameCount0!.Value = withBlock.Frames[0];
            if (Data.Animation[GameState.EditorIndex].LoopCount[0] == 0)
                Data.Animation[GameState.EditorIndex].LoopCount[0] = 1;
            Editor_Animation.Instance!.nudLoopCount0!.Value = withBlock.LoopCount[0];
            if (Data.Animation[GameState.EditorIndex].LoopTime[0] == 0)
                Data.Animation[GameState.EditorIndex].LoopTime[0] = 1;
            Editor_Animation.Instance!.nudLoopTime0!.Value = withBlock.LoopTime[0];

            Editor_Animation.Instance!.nudSprite1!.Value = withBlock.Sprite[1];
            Editor_Animation.Instance!.nudFrameCount1!.Value = withBlock.Frames[1];
            if (Data.Animation[GameState.EditorIndex].LoopCount[1] == 0)
                Data.Animation[GameState.EditorIndex].LoopCount[1] = 1;
            Editor_Animation.Instance!.nudLoopCount1!.Value = withBlock.LoopCount[1];
            if (Data.Animation[GameState.EditorIndex].LoopTime[1] == 0)
                Data.Animation[GameState.EditorIndex].LoopTime[1] = 1;
            Editor_Animation.Instance!.nudLoopTime1!.Value = withBlock.LoopTime[1];

            GameState.AnimationChanged[GameState.EditorIndex] = true;
        }

        private static void EnsureAnimationArrays(ref Core.Globals.Type.Animation a)
        {
            // Ensure arrays exist and have at least length 2
            if (a.Sprite == null) a.Sprite = new int[2];
            else if (a.Sprite.Length < 2) Array.Resize(ref a.Sprite, 2);

            if (a.Frames == null) a.Frames = new int[2];
            else if (a.Frames.Length < 2) Array.Resize(ref a.Frames, 2);

            if (a.LoopCount == null) a.LoopCount = new int[2];
            else if (a.LoopCount.Length < 2) Array.Resize(ref a.LoopCount, 2);

            if (a.LoopTime == null) a.LoopTime = new int[2];
            else if (a.LoopTime.Length < 2) Array.Resize(ref a.LoopTime, 2);

            // Sensible minimums to prevent zero/invalid state
            if (a.LoopCount[0] == 0) a.LoopCount[0] = 1;
            if (a.LoopCount[1] == 0) a.LoopCount[1] = 1;
            if (a.LoopTime[0] == 0) a.LoopTime[0] = 1;
            if (a.LoopTime[1] == 0) a.LoopTime[1] = 1;
        }

        public static void AnimationEditorOK()
        {
            int i;

            for (i = 0; i < Constant.MaxAnimations; i++)
            {
                if (GameState.AnimationChanged[i])
                {
                    Sender.SendSaveAnimation(i);
                }
            }

            GameState.MyEditorType = EditorType.None;
            ClearChanged_Animation();
            Sender.SendCloseEditor();
        }

        public static void AnimationEditorCancel()
        {
            GameState.MyEditorType = EditorType.None;
            ClearChanged_Animation();
            Animation.ClearAnimations();
            Sender.SendCloseEditor();
        }

        public static void ClearChanged_Animation()
        {
            for (int i = 0; i < Constant.MaxAnimations; i++)
                GameState.AnimationChanged[i] = false;
        }

        #endregion

        #region Npc Editor

        public static void NpcEditorInit()
        {
            var withBlock = Editor_Npc.Instance;
            withBlock.cmbDropSlot.SelectedIndex = 0;

            // Normalize arrays to avoid null/index errors
            EnsureNpcArrays(ref Data.Npc[GameState.EditorIndex]);

            withBlock.txtName.Text = Data.Npc[GameState.EditorIndex].Name;
            withBlock.txtAttackSay.Text = Data.Npc[GameState.EditorIndex].AttackSay;
            withBlock.nudSprite.Value = Data.Npc[GameState.EditorIndex].Sprite;
            withBlock.nudSpawnSecs.Value = Data.Npc[GameState.EditorIndex].SpawnSecs;
            withBlock.cmbBehaviour.SelectedIndex = Data.Npc[GameState.EditorIndex].Behaviour;
            withBlock.cmbFaction.SelectedIndex = Data.Npc[GameState.EditorIndex].Faction;
            withBlock.nudRange.Value = Data.Npc[GameState.EditorIndex].Range;
            withBlock.nudChance.Value = Data.Npc[GameState.EditorIndex].DropChance[Editor_Npc.Instance!.cmbDropSlot.SelectedIndex];
            withBlock.cmbItem.SelectedIndex = Data.Npc[GameState.EditorIndex].DropItem[Editor_Npc.Instance.cmbDropSlot.SelectedIndex];

            withBlock.nudAmount.Value = Data.Npc[GameState.EditorIndex].DropItemValue[Editor_Npc.Instance.cmbDropSlot.SelectedIndex];

            withBlock.nudHp.Value = Data.Npc[GameState.EditorIndex].Hp;
            withBlock.nudExp.Value = Data.Npc[GameState.EditorIndex].Exp;
            withBlock.nudLevel.Value = Data.Npc[GameState.EditorIndex].Level;
            withBlock.nudDamage.Value = Data.Npc[GameState.EditorIndex].Damage;

            withBlock.cmbSpawnPeriod.SelectedIndex = Data.Npc[GameState.EditorIndex].SpawnTime;

            withBlock.cmbAnimation.SelectedIndex = Data.Npc[GameState.EditorIndex].Animation;

            withBlock.nudStrength.Value = Data.Npc[GameState.EditorIndex].Stat[(int)Stat.Strength];
            withBlock.nudIntelligence.Value = Data.Npc[GameState.EditorIndex].Stat[(int)Stat.Intelligence];
            withBlock.nudSpirit.Value = Data.Npc[GameState.EditorIndex].Stat[(int)Stat.Spirit];
            withBlock.nudLuck.Value = Data.Npc[GameState.EditorIndex].Stat[(int)Stat.Luck];
            withBlock.nudVitality.Value = Data.Npc[GameState.EditorIndex].Stat[(int)Stat.Vitality];

            withBlock.cmbSkill1.SelectedIndex = Data.Npc[GameState.EditorIndex].Skill[0];
            withBlock.cmbSkill2.SelectedIndex = Data.Npc[GameState.EditorIndex].Skill[1];
            withBlock.cmbSkill3.SelectedIndex = Data.Npc[GameState.EditorIndex].Skill[2];
            withBlock.cmbSkill4.SelectedIndex = Data.Npc[GameState.EditorIndex].Skill[3];
            withBlock.cmbSkill5.SelectedIndex = Data.Npc[GameState.EditorIndex].Skill[4];
            withBlock.cmbSkill6.SelectedIndex = Data.Npc[GameState.EditorIndex].Skill[5];

            Editor_Npc.Instance.DrawSprite();

            GameState.NpcChanged[GameState.EditorIndex] = true;
        }

        private static void EnsureNpcArrays(ref Core.Globals.Type.Npc n)
        {
            int statCount = Enum.GetValues(typeof(Stat)).Length;
            if (n.Stat == null) n.Stat = new byte[statCount];
            else if (n.Stat.Length < statCount) Array.Resize(ref n.Stat, statCount);

            if (n.DropChance == null) n.DropChance = new int[6];
            else if (n.DropChance.Length < 6) Array.Resize(ref n.DropChance, 6);

            if (n.DropItem == null) n.DropItem = new int[6];
            else if (n.DropItem.Length < 6) Array.Resize(ref n.DropItem, 6);

            if (n.DropItemValue == null) n.DropItemValue = new int[6];
            else if (n.DropItemValue.Length < 6) Array.Resize(ref n.DropItemValue, 6);

            if (n.Skill == null) n.Skill = new byte[7];
            else if (n.Skill.Length < 7) Array.Resize(ref n.Skill, 7);
        }

        public static void NpcEditorOK()
        {
            for (int i = 0; i < Constant.MaxNpcs; i++)
            {
                if (GameState.NpcChanged[i])
                {
                    Sender.SendSaveNpc(i);
                }
            }

            GameState.MyEditorType = EditorType.None;
            ClearChanged_Npc();
            Sender.SendCloseEditor();
        }

        public static void NpcEditorCancel()
        {
            GameState.MyEditorType = EditorType.None;
            ClearChanged_Npc();
            Database.ClearNpcs();
            Sender.SendCloseEditor();
        }

        public static void ClearChanged_Npc()
        {
            for (int i = 0; i < Constant.MaxNpcs; i++)
                GameState.NpcChanged[i] = false;
        }

        #endregion

        #region Resource Editor
        public static void ClearChanged_Resource()
        {
            GameState.ResourceChanged = new bool[Constant.MaxResources];
        }

        public static void ResourceEditorInit()
        {
            var withBlock = Editor_Resource.Instance;
            withBlock.txtName.Text = Data.Resource[GameState.EditorIndex].Name;
            withBlock.txtMessage.Text = Data.Resource[GameState.EditorIndex].SuccessMessage;
            withBlock.txtMessage2.Text = Data.Resource[GameState.EditorIndex].EmptyMessage;
            withBlock.cmbType.SelectedIndex = Data.Resource[GameState.EditorIndex].ResourceType;
            withBlock.nudNormalPic.Value = Data.Resource[GameState.EditorIndex].ResourceImage;
            withBlock.nudExhaustedPic.Value = Data.Resource[GameState.EditorIndex].ExhaustedImage;
            withBlock.cmbRewardItem.SelectedIndex = Data.Resource[GameState.EditorIndex].ItemReward;
            withBlock.nudRewardExp.Value = Data.Resource[GameState.EditorIndex].ExpReward;
            withBlock.cmbTool.SelectedIndex = Data.Resource[GameState.EditorIndex].ToolRequired;
            withBlock.nudHealth.Value = Data.Resource[GameState.EditorIndex].Health;
            withBlock.nudRespawn.Value = Data.Resource[GameState.EditorIndex].RespawnTime;
            withBlock.cmbAnimation.SelectedIndex = Data.Resource[GameState.EditorIndex].Animation;
            withBlock.nudLvlReq.Value = Data.Resource[GameState.EditorIndex].LvlRequired;
 
            GameState.ResourceChanged[GameState.EditorIndex] = true;
        }

        public static void ResourceEditorOK()
        {
            int i;

            for (i = 0; i < Constant.MaxResources; i++)
            {
                if (GameState.ResourceChanged[i])
                {
                    Sender.SendSaveResource(i);
                }
            }

            GameState.MyEditorType = EditorType.None;
            ClearChanged_Resource();
            Sender.SendCloseEditor();
        }

        public static void ResourceEditorCancel()
        {
            GameState.MyEditorType = EditorType.None;
            ClearChanged_Resource();
            MapResource.ClearResources();
            Sender.SendCloseEditor();
        }

        #endregion

        #region Skill Editor

        public static void SkillEditorInit()
        {
            var withBlock = Editor_Skill.Instance;

            withBlock.cmbAnimCast.SelectedIndex = 0;
            withBlock.cmbAnim.SelectedIndex = 0;

            // set values
            withBlock.txtName.Text = Strings.Trim(Data.Skill[GameState.EditorIndex].Name);
            withBlock.cmbType.SelectedIndex = Data.Skill[GameState.EditorIndex].Type;
            withBlock.nudMp.Value = Data.Skill[GameState.EditorIndex].MpCost;
            withBlock.nudLevel.Value = Data.Skill[GameState.EditorIndex].LevelReq;
            withBlock.cmbAccessReq.SelectedIndex = Data.Skill[GameState.EditorIndex].AccessReq;
            withBlock.cmbJob.SelectedIndex = Data.Skill[GameState.EditorIndex].JobReq;
            withBlock.nudCast.Value = Data.Skill[GameState.EditorIndex].CastTime;
            withBlock.nudCool.Value = Data.Skill[GameState.EditorIndex].CdTime;
            withBlock.nudIcon.Value = Data.Skill[GameState.EditorIndex].Icon;
            withBlock.nudMap.Value = Data.Skill[GameState.EditorIndex].Map;
            withBlock.nudX.Value = Data.Skill[GameState.EditorIndex].X;
            withBlock.nudY.Value = Data.Skill[GameState.EditorIndex].Y;
            withBlock.cmbDir.SelectedIndex = Data.Skill[GameState.EditorIndex].Dir;
            withBlock.nudVital.Value = Data.Skill[GameState.EditorIndex].Vital;
            withBlock.nudDuration.Value = Data.Skill[GameState.EditorIndex].Duration;
            withBlock.nudInterval.Value = Data.Skill[GameState.EditorIndex].Interval;
            withBlock.nudRange.Value = Data.Skill[GameState.EditorIndex].Range;

            withBlock.chkAoE.Checked = Data.Skill[GameState.EditorIndex].IsAoE;

            withBlock.nudAoE.Value = Data.Skill[GameState.EditorIndex].AoE;
            withBlock.cmbAnimCast.SelectedIndex = Data.Skill[GameState.EditorIndex].CastAnim;
            withBlock.cmbAnim.SelectedIndex = Data.Skill[GameState.EditorIndex].SkillAnim;
            withBlock.nudStun.Value = Data.Skill[GameState.EditorIndex].StunDuration;

            if (Data.Skill[GameState.EditorIndex].IsProjectile == 1)
            {
                withBlock.chkProjectile.Checked = true;
            }
            else
            {
                withBlock.chkProjectile.Checked = false;
            }
            withBlock.cmbProjectile.SelectedIndex = Data.Skill[GameState.EditorIndex].Projectile;

            if (Data.Skill[GameState.EditorIndex].KnockBack == 1)
            {
                withBlock.chkKnockBack.Checked = true;
            }
            else
            {
                withBlock.chkKnockBack.Checked = false;
            }
            withBlock.cmbKnockBackTiles.SelectedIndex = Data.Skill[GameState.EditorIndex].KnockBackTiles;

            Editor_Skill.Instance.DrawIcon();
          
            GameState.SkillChanged[GameState.EditorIndex] = true;
        }

        public static void SkillEditorOK()
        {
            int i;

            for (i = 0; i < Constant.MaxSkills; i++)
            {
                if (GameState.SkillChanged[i])
                {
                    Sender.SendSaveSkill(i);
                }
            }

            GameState.MyEditorType = EditorType.None;
            ClearChanged_Skill();
            Sender.SendCloseEditor();
        }

        public static void SkillEditorCancel()
        {
            GameState.MyEditorType = EditorType.None;
            ClearChanged_Skill();
            Database.ClearSkills();
            Sender.SendCloseEditor();
        }

        public static void ClearChanged_Skill()
        {
            for (int i = 0; i < Constant.MaxSkills; i++)
                GameState.SkillChanged[i] = false;
        }

        #endregion

        #region Shop editor
        public static void ShopEditorInit()
        {            
            var withBlock = Editor_Shop.Instance;
            withBlock.txtName.Text = Data.Shop[GameState.EditorIndex].Name;

            if (Data.Shop[GameState.EditorIndex].BuyRate > 0)
            {
                withBlock.nudBuy.Value = Data.Shop[GameState.EditorIndex].BuyRate;
            }
            else
            {
                withBlock.nudBuy.Value = 100d; // NumericStepper uses double
            }

            withBlock.cmbItem.SelectedIndex = 0;
            withBlock.cmbCostItem.SelectedIndex = 0;
            
            UpdateShopTrade();
            GameState.ShopChanged[GameState.EditorIndex] = true;
        }

        public static void UpdateShopTrade()
        {
            int i;

            Editor_Shop.Instance.lstTradeItem.Items.Clear();

            for (i = 0; i < Constant.MaxTrades; i++)
            {
                {
                    ref var withBlock = ref Data.Shop[GameState.EditorIndex].TradeItem[i];
                    // if none, show as none
                    if (withBlock.Item == -1 & withBlock.CostItem == -1)
                    {
                        Editor_Shop.Instance.lstTradeItem.Items.Add("Empty Trade Slot");
                    }
                    else
                    {
                        Editor_Shop.Instance.lstTradeItem.Items.Add(i + 1 + ": " + withBlock.ItemValue + "x " + Data.Item[withBlock.Item].Name + " for " + withBlock.CostValue + "x " + Data.Item[withBlock.CostItem].Name);
                    }
                }
            }

            Editor_Shop.Instance.lstTradeItem.SelectedIndex = 0;
        }

        public static void ShopEditorOK()
        {
            int i;

            for (i = 0; i < Constant.MaxShops; i++)
            {
                if (GameState.ShopChanged[i])
                {
                    Sender.SendSaveShop(i);
                }
            }

            GameState.MyEditorType = EditorType.None;
            ClearChanged_Shop();
            Sender.SendCloseEditor();
        }

        public static void ShopEditorCancel()
        {
            GameState.MyEditorType = EditorType.None;
            ClearChanged_Shop();
            Shop.ClearShops();
            Sender.SendCloseEditor();
        }

        public static void ClearChanged_Shop()
        {
            for (int i = 0; i < Constant.MaxShops; i++)
                GameState.ShopChanged[i] = false;
        }

        #endregion

        #region Job Editor
        public static void JobEditorOK()
        {
            for (int i = 0; i < Constant.MaxJobs; i++)
            {
                if (GameState.JobChanged[i])
                {
                    Sender.SendSaveJob(i);
                }
            }
            GameState.MyEditorType = EditorType.None;
            Sender.SendCloseEditor();
        }

        public static void JobEditorCancel()
        {
            GameState.MyEditorType = EditorType.None;
            ClearChanged_Job();
            Database.ClearJobs();
            Sender.SendCloseEditor();
        }

        public static void JobEditorInit()
        {
            var withBlock = Editor_Job.Instance;
            withBlock.txtName!.Text = Data.Job[GameState.EditorIndex].Name;
            withBlock.txtDescription!.Text = Data.Job[GameState.EditorIndex].Desc;

            if (Data.Job[GameState.EditorIndex].MaleSprite == 0)
                Data.Job[GameState.EditorIndex].MaleSprite = 1;
            withBlock.nudMaleSprite!.Value = Data.Job[GameState.EditorIndex].MaleSprite;
            if (Data.Job[GameState.EditorIndex].FemaleSprite == 0)
                Data.Job[GameState.EditorIndex].FemaleSprite = 1;
            withBlock.nudFemaleSprite!.Value = Data.Job[GameState.EditorIndex].FemaleSprite;

            withBlock.cmbItems!.SelectedIndex = 0;

            int statCount = Enum.GetValues(typeof(Stat)).Length;
            for (int i = 0; i < statCount; i++)
            {
                if (Data.Job[GameState.EditorIndex].Stat[i] == 0)
                    Data.Job[GameState.EditorIndex].Stat[i] = 1;
            }

            withBlock.nudStrength!.Value = Data.Job[GameState.EditorIndex].Stat[(int)Stat.Strength];
            withBlock.nudLuck!.Value = Data.Job[GameState.EditorIndex].Stat[(int)Stat.Luck];
            withBlock.nudIntelligence!.Value = Data.Job[GameState.EditorIndex].Stat[(int)Stat.Intelligence];
            withBlock.nudVitality!.Value = Data.Job[GameState.EditorIndex].Stat[(int)Stat.Vitality];
            withBlock.nudSpirit!.Value = Data.Job[GameState.EditorIndex].Stat[(int)Stat.Spirit];
            withBlock.nudBaseExp!.Value = Data.Job[GameState.EditorIndex].BaseExp;

            if (Data.Job[GameState.EditorIndex].StartMap == 0)
                Data.Job[GameState.EditorIndex].StartMap = 1;
            withBlock.nudStartMap!.Value = Data.Job[GameState.EditorIndex].StartMap;
            withBlock.nudStartX!.Value = Data.Job[GameState.EditorIndex].StartX;
            withBlock.nudStartY!.Value = Data.Job[GameState.EditorIndex].StartY;

            GameState.JobChanged[GameState.EditorIndex] = true;
            withBlock.DrawPreview();
        }

        public static void ClearChanged_Job()
        {
            for (int i = 0; i < Constant.MaxJobs; i++)
                GameState.JobChanged[i] = false;
        }


        public static void ItemEditorInit()
        {
            ref var withBlock = ref Data.Item[GameState.EditorIndex];
            Editor_Item.Instance!.txtName!.Text = withBlock.Name;
            Editor_Item.Instance!.txtDescription!.Text = withBlock.Description;

            if (withBlock.Icon > Editor_Item.Instance!.nudIcon!.MaxValue)
                withBlock.Icon = 0;
            Editor_Item.Instance!.nudIcon!.Value = withBlock.Icon;
            int itemCategoryCount = Enum.GetValues(typeof(ItemCategory)).Length;
            if (withBlock.Type < 0 || withBlock.Type >= itemCategoryCount)
                withBlock.Type = 0;
            Editor_Item.Instance!.cmbType!.SelectedIndex = withBlock.Type;
            Editor_Item.Instance!.cmbAnimation!.SelectedIndex = withBlock.Animation;

            if (withBlock.ItemLevel == 0)
                withBlock.ItemLevel = 1;
            Editor_Item.Instance.nudItemLvl.Value = withBlock.ItemLevel;

            // Type specific settings
            if (Editor_Item.Instance.cmbType.SelectedIndex == (int)ItemCategory.Equipment)
            {
                Editor_Item.Instance!.fraEquipment!.Visible = true;
                Editor_Item.Instance!.nudDamage!.Value = withBlock.Data2;
                Editor_Item.Instance!.cmbTool!.SelectedIndex = withBlock.Data3;

                Editor_Item.Instance!.cmbSubType!.SelectedIndex = withBlock.SubType;

                if (withBlock.Speed < 1000)
                    withBlock.Speed = 100;
                if (withBlock.Speed > Editor_Item.Instance!.nudSpeed!.MaxValue)
                    withBlock.Speed = (int)Math.Round(Editor_Item.Instance!.nudSpeed!.MaxValue);
                Editor_Item.Instance!.nudSpeed!.Value = withBlock.Speed;

                Editor_Item.Instance!.nudStrength!.Value = withBlock.AddStat[(int)Stat.Strength];
                Editor_Item.Instance!.nudIntelligence!.Value = withBlock.AddStat[(int)Stat.Intelligence];
                Editor_Item.Instance!.nudVitality!.Value = withBlock.AddStat[(int)Stat.Vitality];
                Editor_Item.Instance!.nudLuck!.Value = withBlock.AddStat[(int)Stat.Luck];
                Editor_Item.Instance!.nudSpirit!.Value = withBlock.AddStat[(int)Stat.Spirit];

                if (withBlock.KnockBack == 1)
                {
                    Editor_Item.Instance!.chkKnockBack!.Checked = true;
                }
                else
                {
                    Editor_Item.Instance!.chkKnockBack!.Checked = false;
                }
                Editor_Item.Instance!.cmbKnockBackTiles!.SelectedIndex = withBlock.KnockBackTiles;
                Editor_Item.Instance.nudPaperdoll.Value = withBlock.Paperdoll;

                if (withBlock.SubType == (byte)Equipment.Weapon)
                {
                    Editor_Item.Instance!.fraProjectile!.Visible = true;
                }
                else
                {
                    Editor_Item.Instance!.fraProjectile!.Visible = false;
                }
            }
            else
            {
                Editor_Item.Instance!.fraEquipment!.Visible = false;
            }

            if (Editor_Item.Instance.cmbType.SelectedIndex == (int)ItemCategory.Consumable)
            {
                Editor_Item.Instance!.fraVitals!.Visible = true;
                Editor_Item.Instance!.nudVitalMod!.Value = withBlock.Data1;
            }
            else
            {
                Editor_Item.Instance!.fraVitals!.Visible = false;
            }

            if (Editor_Item.Instance.cmbType.SelectedIndex == (int)ItemCategory.Skill)
            {
                Editor_Item.Instance!.fraSkill!.Visible = true;
                Editor_Item.Instance!.cmbSkills!.SelectedIndex = withBlock.Data1;
            }
            else
            {
                Editor_Item.Instance!.fraSkill!.Visible = false;
            }

            if (Editor_Item.Instance.cmbType.SelectedIndex == (int)ItemCategory.Projectile)
            {
                Editor_Item.Instance!.fraProjectile!.Visible = true;
                Editor_Item.Instance!.fraEquipment!.Visible = true;
            }
            else if (withBlock.Type != (byte)ItemCategory.Equipment)
            {
                Editor_Item.Instance!.fraProjectile!.Visible = false;
            }

            if (Editor_Item.Instance.cmbType.SelectedIndex == (int)ItemCategory.Event)
            {
                Editor_Item.Instance!.fraEvents!.Visible = true;
                Editor_Item.Instance!.nudEvent!.Value = withBlock.Data1;
                Editor_Item.Instance!.nudEventValue!.Value = withBlock.Data2;
            }
            else
            {
                Editor_Item.Instance!.fraEvents!.Visible = false;
            }

            // Projectile
            Editor_Item.Instance!.cmbProjectile!.SelectedIndex = withBlock.Projectile;
            Editor_Item.Instance!.cmbAmmo!.SelectedIndex = withBlock.Ammo;

            // Basic requirements
            Editor_Item.Instance!.cmbAccessReq!.SelectedIndex = withBlock.AccessReq;
            Editor_Item.Instance!.nudLevelReq!.Value = withBlock.LevelReq;

            Editor_Item.Instance!.nudStrReq!.Value = withBlock.StatReq[(int)Stat.Strength];
            Editor_Item.Instance!.nudVitReq!.Value = withBlock.StatReq[(int)Stat.Vitality];
            Editor_Item.Instance!.nudLuckReq!.Value = withBlock.StatReq[(int)Stat.Luck];
            Editor_Item.Instance!.nudIntReq!.Value = withBlock.StatReq[(int)Stat.Intelligence];
            Editor_Item.Instance!.nudSprReq!.Value = withBlock.StatReq[(int)Stat.Spirit];

            // Build cmbJobReq
            Editor_Item.Instance!.cmbJobReq!.Items.Clear();
            for (int j = 0; j < Constant.MaxJobs; j++)
                Editor_Item.Instance!.cmbJobReq!.Items.Add(Data.Job[j].Name);

            Editor_Item.Instance!.cmbJobReq!.SelectedIndex = withBlock.JobReq;
            // Info
            Editor_Item.Instance!.nudPrice!.Value = withBlock.Price;
            Editor_Item.Instance!.cmbBind!.SelectedIndex = withBlock.BindType;
            Editor_Item.Instance!.nudRarity!.Value = withBlock.Rarity;

            if (withBlock.Stackable == 1)
            {
                Editor_Item.Instance!.chkStackable!.Checked = true;
            }
            else
            {
                Editor_Item.Instance!.chkStackable!.Checked = false;
            }

            Editor_Item.Instance!.DrawIcon();

            GameState.ItemChanged[GameState.EditorIndex] = true;
        }

        public static void ItemEditorCancel()
        {
            GameState.MyEditorType = EditorType.None;
            Item.ClearChangedItem();
            Item.ClearItems();
            Sender.SendCloseEditor();
        }

        public static void ItemEditorOK()
        {
            int i;

            for (i = 0; i < Constant.MaxItems; i++)
            {
                if (GameState.ItemChanged[i])
                {
                    Sender.SendSaveItem(i);
                }
            }

            GameState.MyEditorType = EditorType.None;
            Item.ClearChangedItem();
            Sender.SendCloseEditor();
        }

        #endregion

        #region Moral Editor
        public static void MoralEditorOK()
        {
            for (int i = 0; i < Constant.MaxMorals; i++)
            {
                if (GameState.MoralChanged[i])
                {
                    Sender.SendSaveMoral(i);
                }
            }
            GameState.MyEditorType = EditorType.None;
            Sender.SendCloseEditor();
        }

        public static void MoralEditorCancel()
        {
            GameState.MyEditorType = EditorType.None;
            ClearChanged_Moral();
            Moral.ClearMorals();
            Sender.SendCloseEditor();
        }

        public static void MoralEditorInit()
        {
            var moralBlock = Editor_Moral.Instance;
            moralBlock.txtName!.Text = Data.Moral[GameState.EditorIndex].Name;
            moralBlock.cmbColor!.SelectedIndex = Data.Moral[GameState.EditorIndex].Color;
            moralBlock.chkCanCast!.Checked = Data.Moral[GameState.EditorIndex].CanCast;
            moralBlock.chkCanPK!.Checked = Data.Moral[GameState.EditorIndex].CanPk;
            moralBlock.chkCanPickupItem!.Checked = Data.Moral[GameState.EditorIndex].CanPickupItem;
            moralBlock.chkCanDropItem!.Checked = Data.Moral[GameState.EditorIndex].CanDropItem;
            moralBlock.chkCanUseItem!.Checked = Data.Moral[GameState.EditorIndex].CanUseItem;
            moralBlock.chkDropItems!.Checked = Data.Moral[GameState.EditorIndex].DropItems;
            moralBlock.chkLoseExp!.Checked = Data.Moral[GameState.EditorIndex].LoseExp;
            moralBlock.chkPlayerBlock!.Checked = Data.Moral[GameState.EditorIndex].PlayerBlock;
            moralBlock.chkNpcBlock!.Checked = Data.Moral[GameState.EditorIndex].NpcBlock;
            GameState.MoralChanged[GameState.EditorIndex] = true;
        }

        public static void ClearChanged_Moral()
        {
            for (int i = 0; i < Constant.MaxMorals; i++)
                GameState.MoralChanged[i] = false;
        }
        #endregion

        #region Projectile Editor
        public static void ProjectileEditorInit()
        {            
            ref var withBlock = ref Data.Projectile[GameState.EditorIndex];
            Editor_Projectile.Instance.txtName.Text = Strings.Trim(withBlock.Name);
            Editor_Projectile.Instance.nudPic.Value = withBlock.Sprite;
            Editor_Projectile.Instance.nudRange.Value = withBlock.Range;
            Editor_Projectile.Instance.nudSpeed.Value = withBlock.Speed;
            Editor_Projectile.Instance.nudDamage.Value = withBlock.Damage;
            Editor_Projectile.Instance.Drawicon();
            GameState.ProjectileChanged[GameState.EditorIndex] = true;
        }

        public static void ProjectileEditorOK()
        {
            for (int i = 0; i < Constant.MaxProjectiles;  i++)
            {
                if (GameState.ProjectileChanged[i])
                {
                    Projectile.SendSaveProjectile(i);
                }
            }

            GameState.MyEditorType = EditorType.None;
            ClearChanged_Projectile();
            Sender.SendCloseEditor();
        }

        public static void ProjectileEditorCancel()
        {
            GameState.MyEditorType = EditorType.None;
            ClearChanged_Projectile();
            Projectile.ClearProjectile();
            Sender.SendCloseEditor();
        }

        public static void ClearChanged_Projectile()
        {
            for (int i = 0; i < Constant.MaxProjectiles;  i++)
                GameState.ProjectileChanged[i] = false;

        }

        #endregion

    }
}