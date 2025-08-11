using Client.Net;
using Core.Globals;

namespace Client.Game.UI.Windows;

public static class WinRegister
{
    public static void OnRegister()
    {
        var winRegister = Gui.GetWindowByName("winRegister");
        if (winRegister is null)
        {
            return;
        }

        var username = winRegister.GetChild("txtUsername").Text;
        var password1 = winRegister.GetChild("txtPassword").Text;
        var password2 = winRegister.GetChild("txtRetypePassword").Text;

        if (password1 != password2)
        {
            GameLogic.Dialogue(
                "Register",
                "Passwords don't match.",
                "Please try again.",
                DialogueType.Alert);

            ClearPasswords();

            return;
        }

        if (!Network.IsConnected)
        {
            GameLogic.Dialogue(
                "Invalid Connection",
                "Cannot connect to game server.",
                "Please try again.",
                DialogueType.Alert);

            return;
        }

        Sender.SendRegister(username, password1);
    }

    public static void OnClose()
    {
        Gui.HideWindows();

        Gui.ShowWindow("winLogin");
    }

    public static void ClearPasswords()
    {
        var winRegister = Gui.GetWindowByName("winRegister");
        if (winRegister is not null)
        {
            winRegister.GetChild("txtPassword").Text = "";
            winRegister.GetChild("txtRetypePassword").Text = "";
        }

        var winLogin = Gui.GetWindowByName("winLogin");
        if (winLogin is not null)
        {
            winLogin.GetChild("txtPassword").Text = "";
        }
    }
}