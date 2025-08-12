using Client.Net;
using Core.Configurations;
using Core.Globals;

namespace Client.Game.UI.Windows;

public static class WinLogin
{
    public static void OnExit()
    {
        General.DestroyGame();
    }
    
    public static void OnLogin()
    {
        var window = Gui.GetWindowByName("winLogin");
        if (window is null)
        {
            return;
        }

        var username = window.GetChild("txtUsername").Text;
        var password = window.GetChild("txtPassword").Text;

        if (Network.IsConnected)
        {
            Sender.SendLogin(username, password);
        }
        else
        {
            GameLogic.Dialogue("Invalid Connection", "Cannot connect to game server.", "Please try again.", DialogueType.Alert);
        }
    }

    public static void OnRegister()
    {
        if (!Network.IsConnected)
        {
            GameLogic.Dialogue(
                "Invalid Connection",
                "Cannot connect to game server.",
                "Please try again.",
                DialogueType.Alert);

            return;
        }

        Gui.HideWindows();

        WinRegister.ClearPasswords();

        Gui.ShowWindow("winRegister");
    }

    public static void OnSaveUserClicked()
    {
        var winLogin = Gui.GetWindowByName("winLogin");
        if (winLogin is null)
        {
            return;
        }
        
        var checkBoxSaveUsername = winLogin.GetChild("chkSaveUsername");
        if (checkBoxSaveUsername.Value == 0)
        {
            SettingsManager.Instance.SaveUsername = false;
            SettingsManager.Instance.Username = "";
        }
        else
        {
            SettingsManager.Instance.SaveUsername = true;
        }

        SettingsManager.Save();
    }
}