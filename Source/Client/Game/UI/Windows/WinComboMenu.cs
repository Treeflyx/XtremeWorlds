using Client.Game.UI.Controls;

namespace Client.Game.UI.Windows;

public static class WinComboMenu
{
    /// <summary>
    /// Returns true if the combo menu is currently open for the given window/control.
    /// </summary>
    public static bool IsOpen(Window window, int controlIndex)
    {
        var winComboMenu = Gui.GetWindowByName("winComboMenu");
        if (winComboMenu is null || !winComboMenu.Visible)
            return false;
        // Check if the menu is for the current ComboBox
        if (window.Controls[controlIndex] is ComboBox comboBox && winComboMenu.ParentControl == comboBox)
            return true;
        return false;
    }
    
    public static void Close()
    {
        Gui.HideWindow("winComboMenuBG");
        Gui.HideWindow("winComboMenu");
    }

    public static void Show(Window window, int controlIndex)
    {
        if (window.Controls[controlIndex] is not ComboBox comboBox)
        {
            return;
        }

        var winComboMenu = Gui.GetWindowByName("winComboMenu");
        if (winComboMenu is null)
        {
            return;
        }

        winComboMenu.ParentControl = comboBox;
        winComboMenu.Height = 2 + comboBox.Items.Count * 16;
        winComboMenu.X = window.X + comboBox.X + 2;

        var y = window.Y + comboBox.Y + comboBox.Height;
        if (y + winComboMenu.Height > GameState.ResolutionHeight)
        {
            y = GameState.ResolutionHeight - winComboMenu.Height;
        }

        winComboMenu.Y = y;
        winComboMenu.Width = comboBox.Width - 4;
        winComboMenu.List = comboBox.Items;
        winComboMenu.Value = comboBox.Value;
        winComboMenu.Group = 0;
        winComboMenu.Visible = true;

        Gui.ShowWindow("winComboMenuBG", true, false);
        Gui.ShowWindow("winComboMenu", true, false);
    }
}