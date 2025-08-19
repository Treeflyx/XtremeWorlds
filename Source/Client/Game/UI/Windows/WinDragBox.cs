using System.Diagnostics.CodeAnalysis;
using Client.Net;
using Core.Globals;
using static Core.Globals.Command;
using Type = Core.Globals.Type;

namespace Client.Game.UI.Windows;

[SuppressMessage("ReSharper", "PossibleLossOfFraction")]
public static class WinDragBox
{
    public static void OnDraw()
    {
        var winDragBox = Gui.GetWindowByName("winDragBox");
        if (winDragBox is null)
        {
            return;
        }

        var x = winDragBox.X;
        var y = winDragBox.Y;

        if (Gui.DragBox.Type == DraggablePartType.None)
        {
            return;
        }

        ref var dragBox = ref Gui.DragBox;
        switch (dragBox.Type)
        {
            case DraggablePartType.Item:
                if (dragBox.Value >= 0)
                {
                    var icon = Data.Item[dragBox.Value].Icon;
                    var iconPath = Path.Combine(DataPath.Items, icon.ToString());

                    GameClient.RenderTexture(ref iconPath, x, y, 0, 0, 32, 32, 32, 32);
                }

                break;

            case DraggablePartType.Skill:
                if (dragBox.Value >= 0)
                {
                    var icon = Data.Skill[dragBox.Value].Icon;
                    var iconPath = Path.Combine(DataPath.Skills, icon.ToString());

                    GameClient.RenderTexture(ref iconPath, x, y, 0, 0, 32, 32, 32, 32);
                }

                break;
        }
    }

    public static void DragBox_Check()
    {
        Window? targetWindow = null;

        var winDragBox = Gui.GetWindowByName("winDragBox");
        if (winDragBox is null)
        {
            return;
        }

        if (Gui.DragBox.Type == DraggablePartType.None)
        {
            return;
        }

        foreach (var window in Gui.Windows.Values)
        {
            if (!window.Visible || window.Name == "winDragBox")
            {
                continue;
            }

            if (GameState.CurMouseX < window.X ||
                GameState.CurMouseX > window.X + window.Width ||
                GameState.CurMouseY < window.Y ||
                GameState.CurMouseY > window.Y + window.Height)
            {
                continue;
            }

            targetWindow ??= window;

            if (window.ZOrder > targetWindow.ZOrder)
            {
                targetWindow = window;
            }
        }

        if (targetWindow is not null)
        {
            switch (targetWindow.Name)
            {
                case "winBank":
                    DropOnBank(targetWindow);
                    break;

                case "winInventory":
                    DropOnInventory(targetWindow);
                    break;

                case "winSkills":
                    DropOnSkills(targetWindow);
                    break;

                case "winHotbar":
                    DropOnHotBar(targetWindow);
                    break;
            }
        }
        else
        {
            DropWithoutTarget();
        }

        Gui.HideWindow("winDragBox");

        ref var dragBox = ref Gui.DragBox;

        dragBox.Type = DraggablePartType.None;
        dragBox.Slot = 0;
        dragBox.Origin = PartOrigin.None;
        dragBox.Value = 0;
    }

    private static void DropOnBank(Window window)
    {
        switch (Gui.DragBox.Origin)
        {
            case PartOrigin.Bank:
                if (Gui.DragBox.Type == DraggablePartType.Item)
                {
                    for (var slot = 0; slot <= Constant.MaxBank; slot++)
                    {
                        Type.Rect rect;

                        rect.Top = window.Y + GameState.BankTop + (GameState.BankOffsetY + 32) * (slot / GameState.BankColumns);
                        rect.Bottom = rect.Top + 32;
                        rect.Left = window.X + GameState.BankLeft + (GameState.BankOffsetX + 32) * (slot % GameState.BankColumns);
                        rect.Right = rect.Left + 32;

                        if (GameState.CurMouseX < rect.Left ||
                            GameState.CurMouseX > rect.Right ||
                            GameState.CurMouseY < rect.Top ||
                            GameState.CurMouseY > rect.Bottom)
                        {
                            continue;
                        }

                        if (Gui.DragBox.Slot == slot)
                        {
                            continue;
                        }

                        Bank.ChangeBankSlots(Gui.DragBox.Slot, slot);
                        break;
                    }
                }

                break;

            case PartOrigin.Inventory:
                if (Gui.DragBox.Type == DraggablePartType.Item)
                {
                    if (Data.Item[GetPlayerInv(GameState.MyIndex, Gui.DragBox.Slot)].Type != (byte) ItemCategory.Currency)
                    {
                        Bank.DepositItem(Gui.DragBox.Slot, 1);
                    }
                    else
                    {
                        GameLogic.Dialogue("Deposit Item", "Enter the deposit quantity.", "", DialogueType.DepositItem, DialogueStyle.Input, Gui.DragBox.Slot);
                    }
                }

                break;
        }
    }

    private static void DropOnInventory(Window window)
    {
        switch (Gui.DragBox.Origin)
        {
            case PartOrigin.Inventory:
                if (Gui.DragBox.Type == DraggablePartType.Item)
                {
                    for (var slot = 0; slot < Constant.MaxInv; slot++)
                    {
                        Type.Rect rect;

                        rect.Top = window.Y + GameState.InvTop + (GameState.InvOffsetY + 32) * (slot / GameState.InvColumns);
                        rect.Bottom = rect.Top + 32;
                        rect.Left = window.X + GameState.InvLeft + (GameState.InvOffsetX + 32) * (slot % GameState.InvColumns);
                        rect.Right = rect.Left + 32;

                        if (GameState.CurMouseX < rect.Left ||
                            GameState.CurMouseX > rect.Right ||
                            GameState.CurMouseY < rect.Top ||
                            GameState.CurMouseY > rect.Bottom)
                        {
                            continue;
                        }

                        if (Gui.DragBox.Slot != slot)
                        {
                            Sender.SendChangeInvSlots(Gui.DragBox.Slot, slot);
                        }

                        break;
                    }
                }

                break;

            case PartOrigin.Bank:
                if (Gui.DragBox.Type == DraggablePartType.Item)
                {
                    if (Data.Item[GetBank(GameState.MyIndex, (byte) Gui.DragBox.Slot)].Type != (byte) ItemCategory.Currency)
                    {
                        Bank.WithdrawItem((byte) Gui.DragBox.Slot, 0);
                    }
                    else
                    {
                        GameLogic.Dialogue("Withdraw Item", "Enter the amount you wish to withdraw.", "", DialogueType.WithdrawItem, DialogueStyle.Input, Gui.DragBox.Slot);
                    }
                }

                break;
        }
    }

    private static void DropOnSkills(Window window)
    {
        if (Gui.DragBox.Origin != PartOrigin.SkillTree ||
            Gui.DragBox.Type != DraggablePartType.Skill)
        {
            return;
        }

        for (var slot = 0; slot < Constant.MaxPlayerSkills; slot++)
        {
            Type.Rect rect;

            rect.Top = window.Y + GameState.SkillTop + (GameState.SkillOffsetY + 32) * (slot / GameState.SkillColumns);
            rect.Bottom = rect.Top + 32;
            rect.Left = window.X + GameState.SkillLeft + (GameState.SkillOffsetX + 32) * (slot % GameState.SkillColumns);
            rect.Right = rect.Left + 32;

            if (GameState.CurMouseX < rect.Left ||
                GameState.CurMouseX > rect.Right ||
                GameState.CurMouseY < rect.Top ||
                GameState.CurMouseY > rect.Bottom)
            {
                continue;
            }

            if (Gui.DragBox.Slot != slot)
            {
                Sender.SendChangeSkillSlots(Gui.DragBox.Slot, slot);
            }

            break;
        }
    }

    private static void DropOnHotBar(Window window)
    {
        if (Gui.DragBox.Origin == PartOrigin.None ||
            Gui.DragBox.Type == DraggablePartType.None)
        {
            return;
        }

        for (var slot = 0; slot < Constant.MaxHotbar; slot++)
        {
            Type.Rect rect;

            rect.Top = window.Y + GameState.HotbarTop;
            rect.Bottom = rect.Top + 32;
            rect.Left = window.X + GameState.HotbarLeft + slot * GameState.HotbarOffsetX;
            rect.Right = rect.Left + 32;

            if (GameState.CurMouseX < rect.Left ||
                GameState.CurMouseX > rect.Right ||
                GameState.CurMouseY < rect.Top ||
                GameState.CurMouseY > rect.Bottom)
            {
                continue;
            }

            if (Gui.DragBox.Origin != PartOrigin.Hotbar)
            {
                switch (Gui.DragBox.Type)
                {
                    case DraggablePartType.Item:
                        Sender.SendSetHotbarSlot((int) PartOrigin.Inventory, slot, Gui.DragBox.Slot, Gui.DragBox.Value);
                        break;

                    case DraggablePartType.Skill:
                        Sender.SendSetHotbarSlot((int) PartOrigin.SkillTree, slot, Gui.DragBox.Slot, Gui.DragBox.Value);
                        break;
                }
            }
            else if (Gui.DragBox.Slot != slot)
            {
                Sender.SendSetHotbarSlot((int) PartOrigin.Hotbar, slot, Gui.DragBox.Slot, Gui.DragBox.Value);
            }

            break;
        }
    }

    private static void DropWithoutTarget()
    {
        switch (Gui.DragBox.Origin)
        {
            case PartOrigin.Inventory:
                if (Data.Item[GetPlayerInv(GameState.MyIndex, Gui.DragBox.Slot)].Type != (byte) ItemCategory.Currency)
                {
                    Sender.SendDropItem(Gui.DragBox.Slot, GetPlayerInv(GameState.MyIndex, Gui.DragBox.Slot));
                }
                else
                {
                    GameLogic.Dialogue("Drop Item", "Please choose how many to drop.", "", DialogueType.DropItem, DialogueStyle.Input, Gui.DragBox.Slot);
                }

                break;

            case PartOrigin.SkillTree:
                Sender.ForgetSkill(Gui.DragBox.Slot);
                break;

            case PartOrigin.Hotbar:
                Sender.SendSetHotbarSlot((int) Gui.DragBox.Origin, Gui.DragBox.Slot, Gui.DragBox.Slot, 0);
                break;
        }
    }
}