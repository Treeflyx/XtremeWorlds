using System;
using Client.Net;
using Core;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Client
{

    static class Editors
    {

        #region Animation Editor

        public static void AnimationEditorInit()
        {
            GameState.EditorIndex = Editor_Animation.Instance.lstIndex.SelectedIndex;
            
            ref var withBlock = ref Data.Animation[GameState.EditorIndex];
            if (string.IsNullOrEmpty(withBlock.Sound))
            {
                Editor_Animation.Instance.cmbSound.SelectedIndex = 0;
            }
            else
            {
                for (int i = 0, loopTo = Editor_Animation.Instance.cmbSound.Items.Count; i < loopTo; i++)
                {
                    if (Editor_Animation.Instance.cmbSound.GetItemText(Editor_Animation.Instance.cmbSound.Items[i]) == withBlock.Sound)
                    {
                        Editor_Animation.Instance.cmbSound.SelectedIndex = i;
                        break;
                    }
                }
            }
            Editor_Animation.Instance.txtName.Text = withBlock.Name;

            Editor_Animation.Instance.nudSprite0.Value = withBlock.Sprite[0];
            Editor_Animation.Instance.nudFrameCount0.Value = withBlock.Frames[0];
            if (Data.Animation[GameState.EditorIndex].LoopCount[0] == 0)
                Data.Animation[GameState.EditorIndex].LoopCount[0] = 1;
            Editor_Animation.Instance.nudLoopCount0.Value = withBlock.LoopCount[0];
            if (Data.Animation[GameState.EditorIndex].LoopTime[0] == 0)
                Data.Animation[GameState.EditorIndex].LoopTime[0] = 1;
            Editor_Animation.Instance.nudLoopTime0.Value = withBlock.LoopTime[0];

            Editor_Animation.Instance.nudSprite1.Value = withBlock.Sprite[1];
            Editor_Animation.Instance.nudFrameCount1.Value = withBlock.Frames[1];
            if (Data.Animation[GameState.EditorIndex].LoopCount[1] == 0)
                Data.Animation[GameState.EditorIndex].LoopCount[1] = 1;
            Editor_Animation.Instance.nudLoopCount1.Value = withBlock.LoopCount[1];
            if (Data.Animation[GameState.EditorIndex].LoopTime[1] == 0)
                Data.Animation[GameState.EditorIndex].LoopTime[1] = 1;
            Editor_Animation.Instance.nudLoopTime1.Value = withBlock.LoopTime[1];

            GameState.AnimationChanged[GameState.EditorIndex] = true;
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
            GameState.EditorIndex = withBlock.lstIndex.SelectedIndex;

            withBlock.cmbDropSlot.SelectedIndex = 0;

            withBlock.txtName.Text = Data.Npc[GameState.EditorIndex].Name;
            withBlock.txtAttackSay.Text = Data.Npc[GameState.EditorIndex].AttackSay;
            withBlock.nudSprite.Value = Data.Npc[GameState.EditorIndex].Sprite;
            withBlock.nudSpawnSecs.Value = Data.Npc[GameState.EditorIndex].SpawnSecs;
            withBlock.cmbBehaviour.SelectedIndex = Data.Npc[GameState.EditorIndex].Behaviour;
            withBlock.cmbFaction.SelectedIndex = Data.Npc[GameState.EditorIndex].Faction;
            withBlock.nudRange.Value = Data.Npc[GameState.EditorIndex].Range;
            withBlock.nudChance.Value = Data.Npc[GameState.EditorIndex].DropChance[Editor_Npc.Instance.cmbDropSlot.SelectedIndex];
            withBlock.cmbItem.SelectedIndex = Data.Npc[GameState.EditorIndex].DropItem[Editor_Npc.Instance.cmbDropSlot.SelectedIndex];

            withBlock.nudAmount.Value = Data.Npc[GameState.EditorIndex].DropItemValue[Editor_Npc.Instance.cmbDropSlot.SelectedIndex];

            withBlock.nudHp.Value = Data.Npc[GameState.EditorIndex].Hp;
            withBlock.nudExp.Value = Data.Npc[GameState.EditorIndex].Exp;
            withBlock.nudLevel.Value = Data.Npc[GameState.EditorIndex].Level;
            withBlock.nudDamage.Value = Data.Npc[GameState.EditorIndex].Damage;

            withBlock.cmbSpawnPeriod.SelectedIndex = Data.Npc[GameState.EditorIndex].SpawnTime;

            withBlock.cmbAnimation.SelectedIndex = Data.Npc[GameState.EditorIndex].Animation;

            withBlock.nudStrength.Value = Data.Npc[GameState.EditorIndex].Stat[(int)Core.Stat.Strength];
            withBlock.nudIntelligence.Value = Data.Npc[GameState.EditorIndex].Stat[(int)Core.Stat.Intelligence];
            withBlock.nudSpirit.Value = Data.Npc[GameState.EditorIndex].Stat[(int)Core.Stat.Spirit];
            withBlock.nudLuck.Value = Data.Npc[GameState.EditorIndex].Stat[(int)Core.Stat.Luck];
            withBlock.nudVitality.Value = Data.Npc[GameState.EditorIndex].Stat[(int)Core.Stat.Vitality];

            withBlock.cmbSkill1.SelectedIndex = Data.Npc[GameState.EditorIndex].Skill[0];
            withBlock.cmbSkill2.SelectedIndex = Data.Npc[GameState.EditorIndex].Skill[1];
            withBlock.cmbSkill3.SelectedIndex = Data.Npc[GameState.EditorIndex].Skill[2];
            withBlock.cmbSkill4.SelectedIndex = Data.Npc[GameState.EditorIndex].Skill[3];
            withBlock.cmbSkill5.SelectedIndex = Data.Npc[GameState.EditorIndex].Skill[4];
            withBlock.cmbSkill6.SelectedIndex = Data.Npc[GameState.EditorIndex].Skill[5];

            Editor_Npc.Instance.DrawSprite();

            GameState.NpcChanged[GameState.EditorIndex] = true;
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
            int i;

            GameState.EditorIndex = Editor_Resource.Instance.lstIndex.SelectedIndex;

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
            
            Editor_Resource.Instance.Visible = true;

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
            GameState.EditorIndex = withBlock.lstIndex.SelectedIndex;

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
            GameState.EditorIndex = Editor_Shop.Instance.lstIndex.SelectedIndex;
            
            var withBlock = Editor_Shop.Instance;
            withBlock.txtName.Text = Data.Shop[GameState.EditorIndex].Name;

            if (Data.Shop[GameState.EditorIndex].BuyRate > 0)
            {
                withBlock.nudBuy.Value = Data.Shop[GameState.EditorIndex].BuyRate;
            }
            else
            {
                withBlock.nudBuy.Value = 100m;
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
                        Editor_Shop.Instance.lstTradeItem.Items.Add(i + 1 + ": " + withBlock.ItemValue + "x " + Core.Data.Item[withBlock.Item].Name + " for " + withBlock.CostValue + "x " + Core.Data.Item[withBlock.CostItem].Name);
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
            int i;
            
            var withBlock = Editor_Job.Instance;
            GameState.EditorIndex = withBlock.lstIndex.SelectedIndex;

            withBlock.txtName.Text = Data.Job[GameState.EditorIndex].Name;
            withBlock.txtDescription.Text = Data.Job[GameState.EditorIndex].Desc;

            if (Data.Job[GameState.EditorIndex].MaleSprite == 0)
                Data.Job[GameState.EditorIndex].MaleSprite = 1;
            withBlock.nudMaleSprite.Value = Data.Job[GameState.EditorIndex].MaleSprite;
            if (Data.Job[GameState.EditorIndex].FemaleSprite == 0)
                Data.Job[GameState.EditorIndex].FemaleSprite = 1;
            withBlock.nudFemaleSprite.Value = Data.Job[GameState.EditorIndex].FemaleSprite;

            withBlock.cmbItems.SelectedIndex = 0;

            int statCount = Enum.GetValues(typeof(Core.Stat)).Length;
            for (i = 0; i < statCount; i++)
            {
                if (Data.Job[GameState.EditorIndex].Stat[i] == 0)
                    Data.Job[GameState.EditorIndex].Stat[i] = 1;
            }

            withBlock.nudStrength.Value = Data.Job[GameState.EditorIndex].Stat[(int)Core.Stat.Strength];
            withBlock.nudLuck.Value = Data.Job[GameState.EditorIndex].Stat[(int)Core.Stat.Luck];
            withBlock.nudIntelligence.Value = Data.Job[GameState.EditorIndex].Stat[(int)Core.Stat.Intelligence];
            withBlock.nudVitality.Value = Data.Job[GameState.EditorIndex].Stat[(int)Core.Stat.Vitality];
            withBlock.nudSpirit.Value = Data.Job[GameState.EditorIndex].Stat[(int)Core.Stat.Spirit];
            withBlock.nudBaseExp.Value = Data.Job[GameState.EditorIndex].BaseExp;

            if (Data.Job[GameState.EditorIndex].StartMap == 0)
                Data.Job[GameState.EditorIndex].StartMap = 1;
            withBlock.nudStartMap.Value = Data.Job[GameState.EditorIndex].StartMap;
            withBlock.nudStartX.Value = Data.Job[GameState.EditorIndex].StartX;
            withBlock.nudStartY.Value = Data.Job[GameState.EditorIndex].StartY;

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
            int i;

            GameState.EditorIndex = Editor_Item.Instance.lstIndex.SelectedIndex;

            ref var withBlock = ref Core.Data.Item[GameState.EditorIndex];
            Editor_Item.Instance.txtName.Text = withBlock.Name;
            Editor_Item.Instance.txtDescription.Text = withBlock.Description;

            if (withBlock.Icon > Editor_Item.Instance.nudIcon.Maximum)
                withBlock.Icon = 0;
            Editor_Item.Instance.nudIcon.Value = withBlock.Icon;
            int itemCategoryCount = Enum.GetValues(typeof(ItemCategory)).Length;
            if (withBlock.Type < 0 || withBlock.Type >= itemCategoryCount)
                withBlock.Type = 0;
            Editor_Item.Instance.cmbType.SelectedIndex = withBlock.Type;
            Editor_Item.Instance.cmbAnimation.SelectedIndex = withBlock.Animation;

            if (withBlock.ItemLevel == 0)
                withBlock.ItemLevel = 1;
            Editor_Item.Instance.nudItemLvl.Value = withBlock.ItemLevel;

            // Type specific settings
            if (Editor_Item.Instance.cmbType.SelectedIndex == (int)ItemCategory.Equipment)
            {
                Editor_Item.Instance.fraEquipment.Visible = true;
                Editor_Item.Instance.nudDamage.Value = withBlock.Data2;
                Editor_Item.Instance.cmbTool.SelectedIndex = withBlock.Data3;

                Editor_Item.Instance.cmbSubType.SelectedIndex = withBlock.SubType;

                if (withBlock.Speed < 1000)
                    withBlock.Speed = 100;
                if (withBlock.Speed > Editor_Item.Instance.nudSpeed.Maximum)
                    withBlock.Speed = (int)Math.Round(Editor_Item.Instance.nudSpeed.Maximum);
                Editor_Item.Instance.nudSpeed.Value = withBlock.Speed;

                Editor_Item.Instance.nudStrength.Value = withBlock.AddStat[(int)Core.Stat.Strength];
                Editor_Item.Instance.nudIntelligence.Value = withBlock.AddStat[(int)Core.Stat.Intelligence];
                Editor_Item.Instance.nudVitality.Value = withBlock.AddStat[(int)Core.Stat.Vitality];
                Editor_Item.Instance.nudLuck.Value = withBlock.AddStat[(int)Core.Stat.Luck];
                Editor_Item.Instance.nudSpirit.Value = withBlock.AddStat[(int)Core.Stat.Spirit];

                if (withBlock.KnockBack == 1)
                {
                    Editor_Item.Instance.chkKnockBack.Checked = true;
                }
                else
                {
                    Editor_Item.Instance.chkKnockBack.Checked = false;
                }
                Editor_Item.Instance.cmbKnockBackTiles.SelectedIndex = withBlock.KnockBackTiles;
                Editor_Item.Instance.nudPaperdoll.Value = withBlock.Paperdoll;

                if (withBlock.SubType == (byte)Equipment.Weapon)
                {
                    Editor_Item.Instance.fraProjectile.Visible = true;
                }
                else
                {
                    Editor_Item.Instance.fraProjectile.Visible = false;
                }
            }
            else
            {
                Editor_Item.Instance.fraEquipment.Visible = false;
            }

            if (Editor_Item.Instance.cmbType.SelectedIndex == (int)ItemCategory.Consumable)
            {
                Editor_Item.Instance.fraVitals.Visible = true;
                Editor_Item.Instance.nudVitalMod.Value = withBlock.Data1;
            }
            else
            {
                Editor_Item.Instance.fraVitals.Visible = false;
            }

            if (Editor_Item.Instance.cmbType.SelectedIndex == (int)ItemCategory.Skill)
            {
                Editor_Item.Instance.fraSkill.Visible = true;
                Editor_Item.Instance.cmbSkills.SelectedIndex = withBlock.Data1;
            }
            else
            {
                Editor_Item.Instance.fraSkill.Visible = false;
            }

            if (Editor_Item.Instance.cmbType.SelectedIndex == (int)ItemCategory.Projectile)
            {
                Editor_Item.Instance.fraProjectile.Visible = true;
                Editor_Item.Instance.fraEquipment.Visible = true;
            }
            else if (withBlock.Type != (byte)ItemCategory.Equipment)
            {
                Editor_Item.Instance.fraProjectile.Visible = false;
            }

            if (Editor_Item.Instance.cmbType.SelectedIndex == (int)ItemCategory.Event)
            {
                Editor_Item.Instance.fraEvents.Visible = true;
                Editor_Item.Instance.nudEvent.Value = withBlock.Data1;
                Editor_Item.Instance.nudEventValue.Value = withBlock.Data2;
            }
            else
            {
                Editor_Item.Instance.fraEvents.Visible = false;
            }

            // Projectile
            Editor_Item.Instance.cmbProjectile.SelectedIndex = withBlock.Projectile;
            Editor_Item.Instance.cmbAmmo.SelectedIndex = withBlock.Ammo;

            // Basic requirements
            Editor_Item.Instance.cmbAccessReq.SelectedIndex = withBlock.AccessReq;
            Editor_Item.Instance.nudLevelReq.Value = withBlock.LevelReq;

            Editor_Item.Instance.nudStrReq.Value = withBlock.StatReq[(int)Core.Stat.Strength];
            Editor_Item.Instance.nudVitReq.Value = withBlock.StatReq[(int)Core.Stat.Vitality];
            Editor_Item.Instance.nudLuckReq.Value = withBlock.StatReq[(int)Core.Stat.Luck];
            Editor_Item.Instance.nudIntReq.Value = withBlock.StatReq[(int)Core.Stat.Intelligence];
            Editor_Item.Instance.nudSprReq.Value = withBlock.StatReq[(int)Core.Stat.Spirit];

            // Build cmbJobReq
            Editor_Item.Instance.cmbJobReq.Items.Clear();
            for (i = 0; i < Constant.MaxJobs; i++)
                Editor_Item.Instance.cmbJobReq.Items.Add(Data.Job[i].Name);

            Editor_Item.Instance.cmbJobReq.SelectedIndex = withBlock.JobReq;
            // Info
            Editor_Item.Instance.nudPrice.Value = withBlock.Price;
            Editor_Item.Instance.cmbBind.SelectedIndex = withBlock.BindType;
            Editor_Item.Instance.nudRarity.Value = withBlock.Rarity;

            if (withBlock.Stackable == 1)
            {
                Editor_Item.Instance.chkStackable.Checked = true;
            }
            else
            {
                Editor_Item.Instance.chkStackable.Checked = false;
            }

            Editor_Item.Instance.DrawIcon();

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
            int i;

            var withBlock = Editor_Moral.Instance;
            GameState.EditorIndex = withBlock.lstIndex.SelectedIndex;

            withBlock.txtName.Text = Data.Moral[GameState.EditorIndex].Name;
            withBlock.cmbColor.SelectedIndex = Data.Moral[GameState.EditorIndex].Color;
            withBlock.chkCanCast.Checked = Data.Moral[GameState.EditorIndex].CanCast;
            withBlock.chkCanPK.Checked = Data.Moral[GameState.EditorIndex].CanPk;
            withBlock.chkCanPickupItem.Checked = Data.Moral[GameState.EditorIndex].CanPickupItem;
            withBlock.chkCanDropItem.Checked = Data.Moral[GameState.EditorIndex].CanDropItem;
            withBlock.chkCanUseItem.Checked = Data.Moral[GameState.EditorIndex].CanUseItem;
            withBlock.chkDropItems.Checked = Data.Moral[GameState.EditorIndex].DropItems;
            withBlock.chkLoseExp.Checked = Data.Moral[GameState.EditorIndex].LoseExp;
            withBlock.chkPlayerBlock.Checked = Data.Moral[GameState.EditorIndex].PlayerBlock;
            withBlock.chkNpcBlock.Checked = Data.Moral[GameState.EditorIndex].NpcBlock;

            GameState.MoralChanged[GameState.EditorIndex] = true;
        }

        public static void ClearChanged_Moral()
        {
            for (int i = 0; i < Core.Constant.MaxMorals; i++)
                GameState.MoralChanged[i] = false;
        }
        #endregion

        #region Projectile Editor
        public static void ProjectileEditorInit()
        {
            GameState.EditorIndex = Editor_Projectile.Instance.lstIndex.SelectedIndex;
            
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