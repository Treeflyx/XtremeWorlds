using Core.Globals;
using static Core.Globals.Command;

namespace Client.Game.UI.Windows;

public static class WinBank
{
    public static void OnDraw()
    {
        if (GameState.MyIndex < 0 || GameState.MyIndex > Constant.MaxPlayers)
        {
            return;
        }

        var winBank = Gui.GetWindowByName("winBank");
        if (winBank is null)
        {
            return;
        }

        var argpath = Path.Combine(DataPath.Gui, "34");

        GameClient.RenderTexture(ref argpath,
            winBank.X + 4,
            winBank.Y + 23,
            0, 0,
            winBank.Width - 8,
            winBank.Height - 27,
            4, 4);
        
        var height = 76;

        var xo = winBank.X;
        var yo = winBank.Y;

        var y = winBank.Y + 23;
        for (var i = 0; i < 5; i++)
        {
            if (i == 4)
            {
                height = 42;
            }

            var argpath1 = Path.Combine(DataPath.Gui, "35");

            GameClient.RenderTexture(ref argpath1, xo + 4, y, 0, 0, 76, height, 76, height);
            GameClient.RenderTexture(ref argpath1, xo + 80, y, 0, 0, 76, height, 76, height);
            GameClient.RenderTexture(ref argpath1, xo + 156, y, 0, 0, 76, height, 76, height);
            GameClient.RenderTexture(ref argpath1, xo + 232, y, 0, 0, 76, height, 76, height);
            GameClient.RenderTexture(ref argpath1, xo + 308, y, 0, 0, 79, height, 79, height);

            y += 76;
        }

        for (var slot = 0; slot < Constant.MaxBank; slot++)
        {
            var itemNum = GetBank(GameState.MyIndex, slot);
            if (itemNum is < 0 or >= Constant.MaxItems)
            {
                continue;
            }

            Item.StreamItem(itemNum);

            if (Gui.DragBox.Origin == PartOrigin.Bank &&
                Gui.DragBox.Slot == slot)
            {
                continue;
            }

            var itemIcon = Data.Item[itemNum].Icon;
            if (itemIcon <= 0 || itemIcon > GameState.NumItems)
            {
                continue;
            }

            var top = yo + GameState.BankTop + (GameState.BankOffsetY + 32) * (slot / GameState.BankColumns);
            var left = xo + GameState.BankLeft + (GameState.BankOffsetX + 32) * (slot % GameState.BankColumns);

            // draw icon
            var argpath6 = Path.Combine(DataPath.Items, itemIcon.ToString());

            GameClient.RenderTexture(ref argpath6, left, top, 0, 0, 32, 32, 32, 32);

            if (GetBankValue(GameState.MyIndex, slot) <= 1)
            {
                continue;
            }

            var amount = GetBankValue(GameState.MyIndex, slot);
            var amountColor = TextRenderer.GetColorForAmount(amount);

            TextRenderer.RenderText(GameLogic.ConvertCurrency(amount), left + 1, top + 20, amountColor, amountColor);
        }
    }

    public static void OnMouseMove()
    {
        if (Gui.DragBox.Type != DraggablePartType.None)
        {
            return;
        }

        var winBank = Gui.GetWindowByName("winBank");
        if (winBank is null)
        {
            return;
        }

        var winDescription = Gui.GetWindowByName("winDescription");
        if (winDescription is null)
        {
            return;
        }

        var slot = General.IsBank(winBank.X, winBank.Y);
        if (slot < 0)
        {
            winDescription.Visible = false;
            return;
        }

        if (Gui.DragBox.Type == DraggablePartType.Item &&
            Gui.DragBox.Value == slot)
        {
            return;
        }

        var x = winBank.X - winDescription.Width;
        if (x < 0)
        {
            x = winBank.X + winBank.Width;
        }

        var y = winBank.Y - 6;

        GameLogic.ShowItemDesc(x, y, GetBank(GameState.MyIndex, slot));
    }

    public static void OnMouseDown()
    {
        var winBank = Gui.GetWindowByName("winBank");
        if (winBank is null)
        {
            return;
        }

        var slot = General.IsBank(winBank.X, winBank.Y);
        if (slot >= 0)
        {
            ref var dragBox = ref Gui.DragBox;

            dragBox.Type = DraggablePartType.Item;
            dragBox.Value = GetBank(GameState.MyIndex, slot);
            dragBox.Origin = PartOrigin.Bank;
            dragBox.Slot = slot;

            var windowIndex = Gui.GetWindowIndex("winDragBox");
            var window = Gui.Windows[windowIndex];

            window.X = GameState.CurMouseX;
            window.Y = GameState.CurMouseY;
            window.MovedX = GameState.CurMouseX - window.X;
            window.MovedY = GameState.CurMouseY - window.Y;

            Gui.ShowWindow(windowIndex, resetPosition: false);

            winBank.State = ControlState.Normal;
        }

        OnMouseMove();
    }

    public static void OnDoubleClick()
    {
        var winBank = Gui.GetWindowByName("winBank");
        if (winBank is null)
        {
            return;
        }

        var slot = General.IsBank(winBank.X, winBank.Y);
        if (slot >= 0)
        {
            Bank.WithdrawItem(slot, GetBankValue(GameState.MyIndex, slot));

            return;
        }

        OnMouseMove();
    }

    public static void OnClose()
    {
        var winBank = Gui.GetWindowByName("winBank");
        if (winBank is null)
        {
            return;
        }

        if (winBank.Visible)
        {
            Bank.CloseBank();
        }
    }
}