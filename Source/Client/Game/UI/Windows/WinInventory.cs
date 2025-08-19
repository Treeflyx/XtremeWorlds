using Client.Net;
using Core.Globals;
using static Core.Globals.Command;

namespace Client.Game.UI.Windows;

public static class WinInventory
{
    public static void OnDraw()
    {
        if (GameState.MyIndex < 0 || GameState.MyIndex > Constant.MaxPlayers)
        {
            return;
        }

        var winInventory = Gui.GetWindowByName("winInventory");
        if (winInventory is null)
        {
            return;
        }

        var xO = winInventory.X;
        var yO = winInventory.Y;
        var width = winInventory.Width;
        var height = winInventory.Height;

        // render green
        var greenTexturePath = Path.Combine(DataPath.Gui, "34");

        GameClient.RenderTexture(ref greenTexturePath, xO + 4, yO + 23, 0, 0, width - 8, height - 27, 4, 4);

        width = 76;
        height = 76;

        var y = yO + 23;

        for (var i = 0; i < 4; i++)
        {
            if (i == 3)
            {
                height = 38;
            }

            var slotTexturePath = Path.Combine(DataPath.Gui, "35");

            GameClient.RenderTexture(ref slotTexturePath, xO + 4, y, 0, 0, width, height, width, height);
            GameClient.RenderTexture(ref slotTexturePath, xO + 80, y, 0, 0, width, height, width, height);
            GameClient.RenderTexture(ref slotTexturePath, xO + 156, y, 0, 0, 42, height, 42, height);

            y += 76;
        }

        var woodTexturePath = Path.Combine(DataPath.Gui, "1");

        GameClient.RenderTexture(ref woodTexturePath, xO + 4, yO + 289, 100, 100, 194, 26, 194, 26);

        var skipItem = false;

        for (var slot = 0; slot < Constant.MaxInv; slot++)
        {
            var itemNum = GetPlayerInv(GameState.MyIndex, slot);
            if (itemNum is < 0 or >= Constant.MaxItems)
            {
                continue;
            }

            Item.StreamItem(itemNum);

            if (Gui.DragBox.Origin == PartOrigin.Inventory &&
                Gui.DragBox.Slot == slot)
            {
                continue;
            }

            var itemIcon = Data.Item[itemNum].Icon;

            // exit out if we're offering item in a trade.
            var amountModifier = 0;
            if (Trade.InTrade >= 0)
            {
                for (var tradeSlot = 0; tradeSlot < Constant.MaxInv; tradeSlot++)
                {
                    if (Data.TradeYourOffer[tradeSlot].Num < 0)
                    {
                        continue;
                    }

                    if (Data.TradeYourOffer[tradeSlot].Num != slot)
                    {
                        continue;
                    }

                    var tempItemNum = GetPlayerInv(GameState.MyIndex, Data.TradeYourOffer[tradeSlot].Num);

                    if (Data.Item[tempItemNum].Type != (byte) ItemCategory.Currency ||
                        Data.TradeYourOffer[tradeSlot].Value == GetPlayerInvValue(GameState.MyIndex, slot))
                    {
                        skipItem = true;
                    }
                    else
                    {
                        amountModifier = Data.TradeYourOffer[tradeSlot].Value;
                    }
                }
            }

            if (!skipItem)
            {
                if (itemIcon > 0 && itemIcon <= GameState.NumItems)
                {
                    var top = yO + GameState.InvTop + (GameState.InvOffsetY + 32) * (slot / GameState.InvColumns);
                    var left = xO + GameState.InvLeft + (GameState.InvOffsetX + 32) * (slot % GameState.InvColumns);

                    var iconPath = Path.Combine(DataPath.Items, itemIcon.ToString());

                    GameClient.RenderTexture(ref iconPath, left, top, 0, 0, 32, 32, 32, 32);

                    if (GetPlayerInvValue(GameState.MyIndex, slot) > 1)
                    {
                        y = top + 20;

                        var x = left + 1;
                        var amount = GetPlayerInvValue(GameState.MyIndex, slot) - amountModifier;
                        var amountColor = TextRenderer.GetColorForAmount(amount);

                        TextRenderer.RenderText(GameLogic.ConvertCurrency(amount), x, y, amountColor, amountColor);
                    }
                }
            }

            skipItem = false;
        }
    }

    public static void OnMouseDown()
    {
        if (Trade.InTrade == 1)
        {
            return;
        }

        var winInventory = Gui.GetWindowByName("winInventory");
        if (winInventory is null)
        {
            return;
        }

        var slot = General.IsInv(winInventory.X, winInventory.Y);
        if (slot >= 0)
        {
            ref var dragBox = ref Gui.DragBox;

            dragBox.Type = DraggablePartType.Item;
            dragBox.Value = GetPlayerInv(GameState.MyIndex, slot);
            dragBox.Origin = PartOrigin.Inventory;
            dragBox.Slot = slot;

            var windowIndex = Gui.GetWindowIndex("winDragBox");
            var window = Gui.Windows[windowIndex];

            window.X = GameState.CurMouseX;
            window.Y = GameState.CurMouseY;
            window.MovedX = GameState.CurMouseX - window.X;
            window.MovedY = GameState.CurMouseY - window.Y;

            Gui.ShowWindow(windowIndex, resetPosition: false);

            winInventory.State = ControlState.Normal;
        }

        OnMouseMove();
    }

    public static void OnDoubleClick()
    {
        var winInventory = Gui.GetWindowByName("winInventory");
        if (winInventory is null)
        {
            return;
        }

        var slot = General.IsInv(winInventory.X, winInventory.Y);
        if (slot >= 0)
        {
            if (GameState.InBank)
            {
                Bank.DepositItem(slot, GetPlayerInvValue(GameState.MyIndex, slot));
                return;
            }

            if (GameState.InShop >= 0)
            {
                Shop.SellItem(slot);
                return;
            }

            if (Trade.InTrade >= 0)
            {
                for (var i = 0; i < Constant.MaxInv; i++)
                {
                    if (Data.TradeYourOffer[i].Num != slot)
                    {
                        continue;
                    }

                    if (Data.Item[GetPlayerInv(GameState.MyIndex, Data.TradeYourOffer[i].Num)].Type != (byte) ItemCategory.Currency)
                    {
                        return;
                    }

                    if (Data.TradeYourOffer[i].Value == GetPlayerInvValue(GameState.MyIndex, Data.TradeYourOffer[i].Num))
                    {
                        return;
                    }
                }

                if (Data.Item[GetPlayerInv(GameState.MyIndex, slot)].Type == (byte) ItemCategory.Currency)
                {
                    GameLogic.Dialogue(
                        "Select Amount",
                        "Please choose how many to offer.", "",
                        DialogueType.TradeAmount,
                        DialogueStyle.Input,
                        slot);

                    return;
                }

                Trade.TradeItem(slot, 0);

                return;
            }
        }

        Sender.SendUseItem(slot);

        OnMouseMove();
    }

    public static void OnMouseMove()
    {
        if (Gui.DragBox.Type != DraggablePartType.None)
        {
            return;
        }

        var winInventory = Gui.GetWindowByName("winInventory");
        if (winInventory is null)
        {
            return;
        }

        var winDescription = Gui.GetWindowByName("winDescription");
        if (winDescription is null)
        {
            return;
        }

        var slot = General.IsInv(winInventory.X, winInventory.Y);
        if (slot < 0)
        {
            winDescription.Visible = false;
            return;
        }

        if (Trade.InTrade >= 0)
        {
            for (var i = 0; i < Constant.MaxInv; i++)
            {
                if (Data.TradeYourOffer[i].Num != slot)
                {
                    continue;
                }

                if (Data.Item[GetPlayerInv(GameState.MyIndex, Data.TradeYourOffer[i].Num)].Type != (byte) ItemCategory.Currency)
                {
                    return;
                }

                if (Data.TradeYourOffer[i].Value == GetPlayerInvValue(GameState.MyIndex, Data.TradeYourOffer[i].Num))
                {
                    return;
                }
            }
        }

        if (Gui.DragBox.Type == DraggablePartType.Item &&
            Gui.DragBox.Value == slot)
        {
            return;
        }

        var x = winInventory.X - winDescription.Width;
        if (x < 0)
        {
            x = winInventory.X + winInventory.Width;
        }

        var y = winInventory.Y - 6;

        GameLogic.ShowInvDesc(x, y, slot);
    }
}