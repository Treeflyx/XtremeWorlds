using Client.Net;

namespace Client.Game.UI.Windows;

public static class WinEscMenu
{
    public static void OnClose()
    {
        Gui.HideWindow("winEscMenu");
    }

    public static void OnOptionsClick()
    {
        Gui.HideWindow("winEscMenu");
        Gui.ShowWindow("winOptions", true);
    }

    public static void OnMainMenuClick()
    {
        Gui.HideWindows();
        Gui.ShowWindow("winLogin");

        Sender.SendLogout();
    }

    public static void OnExitClick()
    {
        Gui.HideWindow("winEscMenu");

        General.DestroyGame();
    }
}