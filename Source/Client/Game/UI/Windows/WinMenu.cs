namespace Client.Game.UI.Windows;

public static class WinMenu
{
    public static void OnCharacterClick()
    {
        var windowIndex = Gui.GetWindowIndex("winCharacter");

        if (Gui.Windows[windowIndex].Visible)
        {
            Gui.HideWindow(windowIndex);
        }
        else
        {
            Gui.ShowWindow(windowIndex, resetPosition: false);
        }
    }

    public static void OnInventoryClick()
    {
        var windowIndex = Gui.GetWindowIndex("winInventory");

        if (Gui.Windows[windowIndex].Visible)
        {
            Gui.HideWindow(windowIndex);
        }
        else
        {
            Gui.ShowWindow(windowIndex, resetPosition: false);
        }
    }

    public static void OnSkillsClick()
    {
        var windowIndex = Gui.GetWindowIndex("winSkills");

        if (Gui.Windows[windowIndex].Visible)
        {
            Gui.HideWindow(windowIndex);
        }
        else
        {
            Gui.ShowWindow(windowIndex, resetPosition: false);
        }
    }

    public static void OnMapClick()
    {
        // TODO: Implement map window
    }

    public static void OnGuildClick()
    {
        // TODO: Implement guild window
    }

    public static void OnQuestClick()
    {
        // TODO: Implement quest window
    }
}