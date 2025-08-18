using Core.Globals;
using static Core.Globals.Command;

namespace Client.Game.UI.Windows;

public static class WinTrade
{
    public static void OnDraw()
    {
        var winTrade = Gui.GetWindowByName("winTrade");
        if (winTrade is null)
        {
            return;
        }
        
        var xo = winTrade.X;
        var yo = winTrade.Y;
        var width = winTrade.Width;
        var height = winTrade.Height;
        
        var argpath = Path.Combine(DataPath.Gui, 34.ToString());
        
        // render green
        GameClient.RenderTexture(ref argpath, xo + 4, yo + 23, 0, 0, width - 8, height - 27, 4, 4); // ?
        GameClient.RenderTexture(ref argpath, xo + 4, yo + 23, 100, 100, width - 8, 18, width - 8, 18); // Top
        GameClient.RenderTexture(ref argpath, xo + 4, yo + 40, 350, 0, 5, height - 45, 5, height - 45); // Left
        GameClient.RenderTexture(ref argpath, xo + width - 9, yo + 40, 350, 0, 5, height - 45, 5, height - 45); // Right
        GameClient.RenderTexture(ref argpath, xo + 203, yo + 40, 350, 0, 6, height - 45, 6, height - 45); // Center
        GameClient.RenderTexture(ref argpath, xo + 4, yo + 307, 100, 100, width - 8, 75, width - 8, 75); // Bottom
        
        var y = yo + 40;
        for (var i = 0; i < 5; i++)
        {
            if (i == 4)
            {
                height = 38;
            }
            
            var argpath6 = Path.Combine(DataPath.Gui, 35.ToString());
            
            GameClient.RenderTexture(ref argpath6, xo + 4 + 5, y, 0, 0, 76, 76, 76, 76);
            GameClient.RenderTexture(ref argpath6, xo + 80 + 5, y, 0, 0, 76, 76, 76, 76);
            GameClient.RenderTexture(ref argpath6, xo + 156 + 5, y, 0, 0, 42, 76, 42, 76);
            
            y += 76;
        }
        
        y = yo + 40;
        for (var i = 0; i < 5; i++)
        {
            if (i == 4)
            {
                height = 38;
            }
            
            var argpath9 = Path.Combine(DataPath.Gui, 35.ToString());
            
            GameClient.RenderTexture(ref argpath9, xo + 4 + 205, y, 0, 0, 76, 76, 76, 76);
            GameClient.RenderTexture(ref argpath9, xo + 80 + 205, y, 0, 0, 76, 76, 76, 76);
            GameClient.RenderTexture(ref argpath9, xo + 156 + 205, y, 0, 0, 42, 76, 42, 76);

            y += 76;
        }
    }
    
    public static void OnClose()
    {
        Gui.HideWindow("winTrade");

        Trade.SendDeclineTrade();
    }

    public static void OnAccept()
    {
        Trade.SendAcceptTrade();
    }

    public static void OnYourTradeClick()
    {
        var winTrade = Gui.GetWindowByName("winTrade");
        if (winTrade is null)
        {
            return;
        }

        var picYour = winTrade.GetChild("picYour");
        var x = winTrade.X + picYour.X;
        var y = winTrade.Y + picYour.Y;

        var slot = General.IsTrade(x, y);
        if (slot >= 0)
        {
            if (Data.TradeYourOffer[slot].Num == -1)
            {
                return;
            }

            if (GetPlayerInv(GameState.MyIndex, Data.TradeYourOffer[slot].Num) == -1)
            {
                return;
            }

            Trade.UntradeItem(slot);
        }

        OnYourTradeMouseMove();
    }

    public static void OnYourTradeMouseMove()
    {
        var winTrade = Gui.GetWindowByName("winTrade");
        if (winTrade is null)
        {
            return;
        }

        var winDescription = Gui.GetWindowByName("winDescription");
        if (winDescription is null)
        {
            return;
        }

        var picYour = winTrade.GetChild("picYour");
        var slotX = winTrade.X + picYour.X;
        var slotY = winTrade.Y + picYour.Y;

        var slot = General.IsTrade(slotX, slotY);
        if (YourOfferIsEmpty(slot))
        {
            winDescription.Visible = false;
            return;
        }

        var x = winTrade.X - winDescription.Width;
        if (x < 0)
        {
            x = winTrade.X + winTrade.Width;
        }

        var y = winTrade.Y - 6;

        GameLogic.ShowItemDesc(x, y, GetPlayerInv(GameState.MyIndex, Data.TradeYourOffer[slot].Num));
    }

    public static void OnTheirTradeMouseMove()
    {
        var winTrade = Gui.GetWindowByName("winTrade");
        if (winTrade is null)
        {
            return;
        }

        var winDescription = Gui.GetWindowByName("winDescription");
        if (winDescription is null)
        {
            return;
        }

        var picTheir = winTrade.GetChild("picTheir");
        var slotX = winTrade.X + picTheir.X;
        var slotY = winTrade.Y + picTheir.Y;

        var slot = General.IsTrade(slotX, slotY);
        if (TheirOfferIsEmpty(slot))
        {
            winDescription.Visible = false;
            return;
        }

        var x = winTrade.X - winDescription.Width;
        if (x < 0)
        {
            x = winTrade.X + winTrade.Width;
        }

        var y = winTrade.Y - 6;

        GameLogic.ShowItemDesc(x, y, Data.TradeTheirOffer[slot].Num);
    }

    private static bool YourOfferIsEmpty(int slot)
    {
        return slot < 0 || Data.TradeYourOffer[slot].Num == -1 || GetPlayerInv(GameState.MyIndex, Data.TradeYourOffer[slot].Num) == -1;
    }

    private static bool TheirOfferIsEmpty(int slot)
    {
        return slot < 0 || Data.TradeTheirOffer[slot].Num == -1;
    }
}