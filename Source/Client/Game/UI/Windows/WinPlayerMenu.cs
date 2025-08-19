using Core.Globals;
using static Core.Globals.Command;

namespace Client.Game.UI.Windows;

public static class WinPlayerMenu
{
    public static void OnClose()
    {
        Gui.HideWindow("winRightClickBG");
        Gui.HideWindow("winPlayerMenu");
    }
    
    public static void OnPartyInvite()
    {
        OnClose();
        
        Party.SendPartyRequest(GetPlayerName((int) GameState.PlayerMenuIndex));
    }

    public static void OnTradeRequest()
    {
        OnClose();
        
        Trade.SendTradeRequest(GetPlayerName((int) GameState.PlayerMenuIndex));
    }

    public static void OnGuildInvite()
    {
        OnClose();
        
        TextRenderer.AddText("System not yet in place.", (int) ColorName.BrightRed);
    }

    public static void OnPrivateMessage()
    {
        OnClose();
        
        TextRenderer.AddText("System not yet in place.", (int) ColorName.BrightRed);
    }
}