using Client.Net;
using Core.Configurations;
using Core.Globals;

namespace Client.Game.UI.Windows;

public static class WinLogin
{
    public static void OnRegisterClick()
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

        var control = winLogin.Controls[Gui.GetControlIndex("winLogin", "chkSaveUsername")];
        if (control.Value == 0)
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