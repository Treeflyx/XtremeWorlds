using Core.Configurations;
using Core.Globals;

namespace Client.Game.UI.Windows;

public static class WinChat
{
    public static void OnSayClick()
    {
        GameLogic.HandlePressEnter();
    }

    public static void OnDraw()
    {
        var winIndex = Gui.GetWindowByName("winChat");
        if (winIndex is null)
        {
            return;
        }

        var x = winIndex.X;
        var y = winIndex.Y + 16;

        DesignRenderer.Render(Design.WindowDescription, x, y, 352, 152);

        var path = Path.Combine(DataPath.Gui, 46.ToString());

        GameClient.RenderTexture(ref path, x + 7, y + 123, 0, 0, 171, 22, 171, 22);
        GameClient.RenderTexture(ref path, x + 174, y + 123, 0, 22, 171, 22, 171, 22);

        TextRenderer.DrawChat();
    }

    public static void OnDrawSmall()
    {
        var winChatSmall = Gui.GetWindowByName("winChatSmall");
        if (winChatSmall is null)
        {
            return;
        }

        if (GameState.ActChatWidth < 160)
        {
            GameState.ActChatWidth = 160;
        }

        if (GameState.ActChatHeight < 10)
        {
            GameState.ActChatHeight = 10;
        }

        var x = winChatSmall.X + 10;
        var y = GameState.ResolutionHeight - 10;

        DesignRenderer.Render(Design.WindowWithShadow, x, y, 160, 10);
    }

    private static void UpdateChatChannel(string checkBoxName, ChatChannel channel)
    {
        var winChat = Gui.GetWindowByName("winChat");

        var checkBox = winChat?.GetChild(checkBoxName);
        if (checkBox is null)
        {
            return;
        }

        SettingsManager.Instance.ChannelState[(int) channel] = (byte) checkBox.Value;
        SettingsManager.Save();
    }

    public static void OnGameChannelClicked()
    {
        UpdateChatChannel("chkGame", ChatChannel.Game);
    }

    public static void OnMapChannelClicked()
    {
        UpdateChatChannel("chkMap", ChatChannel.Map);
    }

    public static void OnBroadcastChannelClicked()
    {
        UpdateChatChannel("chkGlobal", ChatChannel.Broadcast);
    }

    public static void OnPartyChannelClicked()
    {
        UpdateChatChannel("chkParty", ChatChannel.Party);
    }

    public static void OnGuildChannelClicked()
    {
        UpdateChatChannel("chkGuild", ChatChannel.Guild);
    }

    public static void OnPrivateChannelClicked()
    {
        UpdateChatChannel("chkPlayer", ChatChannel.Private);
    }

    public static void OnUpButtonMouseDown()
    {
        GameState.ChatButtonUp = true;
    }

    public static void OnUpButtonMouseUp()
    {
        GameState.ChatButtonUp = false;
    }

    public static void OnDownButtonMouseDown()
    {
        GameState.ChatButtonDown = true;
    }

    public static void OnDownButtonMouseUp()
    {
        GameState.ChatButtonDown = false;
    }

    public static void Show()
    {
        var winChat = Gui.GetWindowByName("winChat");
        if (winChat is null)
        {
            return;
        }
        
        var windowIndex = Gui.GetWindowIndex("winChat");
        var controlIndex = Gui.GetControlIndex("winChat", "txtChat");

        Gui.ShowWindow("winChat", resetPosition: false);
        Gui.HideWindow("winChatSmall");

        Gui.ActiveWindow = winChat;
        Gui.SetActiveControl(windowIndex, controlIndex);
        Gui.Windows[windowIndex].Controls[controlIndex].Visible = true;

        GameState.InSmallChat = false;
        GameState.ChatScroll = 0;
    }

    public static void Hide()
    {
        var winChat = Gui.GetWindowByName("winChat");
        if (winChat is null)
        {
            return;
        }
        
        var windowIndex = Gui.GetWindowIndex("winChat");
        var controlIndex = Gui.GetControlIndex("winChat", "txtChat");

        Gui.ShowWindow("winChatSmall", resetPosition: false);
        Gui.HideWindow("winChat");

        Gui.ActiveWindow = winChat;
        Gui.SetActiveControl(windowIndex, controlIndex);
        Gui.Windows[windowIndex].Controls[controlIndex].Visible = false;

        GameState.InSmallChat = true;
        GameState.ChatScroll = 0;
    }
}