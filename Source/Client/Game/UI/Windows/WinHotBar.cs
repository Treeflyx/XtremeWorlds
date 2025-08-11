using Client.Net;
using Core.Globals;
using Microsoft.Xna.Framework;
using static Core.Globals.Command;

namespace Client.Game.UI.Windows;

public static class WinHotBar
{
    public static void OnDraw()
    {
        if (GameState.MyIndex < 0 || GameState.MyIndex > Constant.MaxPlayers)
        {
            return;
        }

        var winHotbar = Gui.GetWindowByName("winHotbar");
        if (winHotbar is null)
        {
            return;
        }

        var argpath = Path.Combine(DataPath.Gui, "31");

        GameClient.RenderTexture(ref argpath, winHotbar.Left - 1, winHotbar.Top + 3, 0, 0, 11, 26, 11, 26);
        GameClient.RenderTexture(ref argpath, winHotbar.Left + 407, winHotbar.Top + 3, 0, 0, 11, 26, 11, 26);

        for (var slot = 0; slot < Constant.MaxHotbar; slot++)
        {
            var x = winHotbar.Left + GameState.HotbarLeft + slot * GameState.HotbarOffsetX;
            var y = winHotbar.Top + GameState.HotbarTop;

            if (slot != Constant.MaxHotbar - 1)
            {
                var argpath2 = Path.Combine(DataPath.Gui, "32");

                GameClient.RenderTexture(ref argpath2, x + 30, y + 3, 0, 0, 13, 26, 13, 26);
            }

            var argpath3 = Path.Combine(DataPath.Gui, "30");

            GameClient.RenderTexture(ref argpath3, x - 2, y - 2, 0, 0, 36, 36, 36, 36);

            if (Gui.DragBox.Origin != PartOrigin.Hotbar || Gui.DragBox.Slot != slot)
            {
                switch (Data.Player[GameState.MyIndex].Hotbar[slot].SlotType)
                {
                    case (byte) PartOrigin.Inventory:
                        DrawInventorySlot(slot, x, y);
                        break;

                    case (byte) PartOrigin.SkillTree:
                        DrawSkillTreeSlot(slot, x, y);
                        break;
                }
            }

            var slotNumber = slot + 1;
            if (slotNumber > 9)
            {
                slotNumber = 0;
            }

            var slotNumberStr = slotNumber.ToString();

            Text.RenderText(slotNumberStr, x + 4, y + 19, Color.White, Color.White);
        }
    }

    public static void OnMouseDown()
    {
        var winHotbar = Gui.GetWindowByName("winHotbar");
        if (winHotbar is null)
        {
            return;
        }

        var slot = GameLogic.IsHotbar(winHotbar.Left, winHotbar.Top);
        if (slot >= 0)
        {
            ref var dragBox = ref Gui.DragBox;

            dragBox.Type = Data.Player[GameState.MyIndex].Hotbar[slot].SlotType switch
            {
                1 => (DraggablePartType) PartOrigin.Inventory,
                2 => (DraggablePartType) PartOrigin.SkillTree,
                _ => dragBox.Type
            };

            dragBox.Value = Data.Player[GameState.MyIndex].Hotbar[slot].Slot;
            dragBox.Origin = PartOrigin.Hotbar;
            dragBox.Slot = slot;

            var windowIndex = Gui.GetWindowIndex("winDragBox");
            var winDragBox = Gui.Windows[windowIndex];

            winDragBox.Left = GameState.CurMouseX;
            winDragBox.Top = GameState.CurMouseY;
            winDragBox.MovedX = GameState.CurMouseX - winDragBox.Left;
            winDragBox.MovedY = GameState.CurMouseY - winDragBox.Top;

            Gui.ShowWindow(windowIndex, resetPosition: false);

            winHotbar.State = ControlState.Normal;
        }

        OnMouseMove();
    }

    public static void OnDoubleClick()
    {
        var winHotbar = Gui.GetWindowByName("winHotbar");
        if (winHotbar is null)
        {
            return;
        }

        var slot = GameLogic.IsHotbar(winHotbar.Left, winHotbar.Top);
        if (slot >= 0)
        {
            Sender.SendUseHotbarSlot(slot);
        }

        OnMouseMove();
    }

    public static void OnMouseMove()
    {
        if (Gui.DragBox.Type != (int) PartOrigin.None)
        {
            return;
        }

        var winHotbar = Gui.GetWindowByName("winHotbar");
        if (winHotbar is null)
        {
            return;
        }

        var winDescription = Gui.GetWindowByName("winDescription");
        if (winDescription is null)
        {
            return;
        }

        var slot = GameLogic.IsHotbar(winHotbar.Left, winHotbar.Top);
        if (slot < 0)
        {
            winDescription.Visible = false;
            return;
        }

        if (Gui.DragBox.Origin == PartOrigin.Hotbar &&
            Gui.DragBox.Slot == slot)
        {
            return;
        }

        var x = winHotbar.Left - winDescription.Width;
        if (x < 0)
        {
            x = winHotbar.Left + winHotbar.Width;
        }

        var y = winHotbar.Top - 6;

        switch (Data.Player[GameState.MyIndex].Hotbar[slot].SlotType)
        {
            case 1: // Inventory
                GameLogic.ShowItemDesc(x, y, Data.Player[GameState.MyIndex].Hotbar[slot].Slot);
                break;

            case 2: // Skill
                GameLogic.ShowSkillDesc(x, y, Data.Player[GameState.MyIndex].Hotbar[slot].Slot, 0L);
                break;
        }
    }

    private static void DrawInventorySlot(int slot, int x, int y)
    {
        var itemNum = Data.Player[GameState.MyIndex].Hotbar[slot].Slot;

        Item.StreamItem(itemNum);

        if (Data.Item[itemNum].Name.Length <= 0 || Data.Item[itemNum].Icon <= 0)
        {
            return;
        }

        var path = Path.Combine(DataPath.Items, Data.Item[itemNum].Icon.ToString());

        GameClient.RenderTexture(ref path, x, y, 0, 0, 32, 32, 32, 32);
    }

    private static void DrawSkillTreeSlot(int slot, int x, int y)
    {
        var skillNum = Data.Player[GameState.MyIndex].Hotbar[slot].Slot;

        Database.StreamSkill(skillNum);

        if (Data.Skill[skillNum].Name.Length == 0 ||
            Data.Skill[skillNum].Icon <= 0)
        {
            return;
        }

        var path = Path.Combine(DataPath.Skills, Data.Skill[skillNum].Icon.ToString());

        GameClient.RenderTexture(ref path, x, y, 0, 0, 32, 32, 32, 32);

        for (var i = 0; i < Constant.MaxPlayerSkills; i++)
        {
            if (GetPlayerSkill(GameState.MyIndex, i) < 0 ||
                GetPlayerSkill(GameState.MyIndex, i) != skillNum ||
                GetPlayerSkillCd(GameState.MyIndex, i) <= 0)
            {
                continue;
            }

            GameClient.RenderTexture(ref path, x, y, 0, 0, 32, 32, 32, 32, 255, 100, 100, 100);
        }
    }
}