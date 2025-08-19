using Core.Globals;

namespace Client.Game.UI.Windows;

public static class WinBars
{
    public static void OnDraw()
    {
        var winBars = Gui.GetWindowByName("winBars");
        if (winBars is null)
        {
            return;
        }

        var x = winBars.X;
        var y = winBars.Y;

        var hpBarTexturePath = Path.Combine(DataPath.Gui, "27");
        var spBarTexturePath = Path.Combine(DataPath.Gui, "28");
        var xpBarTexturePath = Path.Combine(DataPath.Gui, "29");

        GameClient.RenderTexture(ref hpBarTexturePath,
            x + 15, y + 15, 0, 0,
            GameState.BarWidthGuiHp, 13,
            GameState.BarWidthGuiHp, 13);

        GameClient.RenderTexture(ref spBarTexturePath,
            x + 15, y + 32, 0, 0,
            GameState.BarWidthGuiSp, 13,
            GameState.BarWidthGuiSp, 13);

        GameClient.RenderTexture(ref xpBarTexturePath,
            x + 15, y + 49, 0, 0,
            GameState.BarWidthGuiExp, 13,
            GameState.BarWidthGuiExp, 13);
    }
}