using Core.Globals;
using Microsoft.Xna.Framework;

namespace Client.Game.UI.Windows;

public static class WinDescription
{
    public static void OnDraw()
    {
        if (GameState.DescItem == -1 || GameState.DescType == 0)
        {
            return;
        }

        var winDescription = Gui.GetWindowByName("winDescription");
        if (winDescription is null)
        {
            return;
        }

        var x = winDescription.X;
        var y = winDescription.Y;

        switch (GameState.DescType)
        {
            case 1: // Inventory Item
            {
                var iconPath = Path.Combine(DataPath.Items, Data.Item[GameState.DescItem].Icon.ToString());

                GameClient.RenderTexture(ref iconPath, x + 20, y + 34, 0, 0, 64, 64, 32, 32);

                break;
            }

            case 2: // Skill Icon
            {
                var picBar = winDescription.GetChild("picBar");
                if (picBar.Visible)
                {
                    var argpath1 = Path.Combine(DataPath.Gui, "45");

                    GameClient.RenderTexture(ref argpath1,
                        x + picBar.X,
                        y + picBar.Y, 0, 12,
                        picBar.Value, 12,
                        picBar.Value, 12);
                }
                
                var iconPath = Path.Combine(DataPath.Skills, Data.Item[GameState.DescItem].Icon.ToString());

                GameClient.RenderTexture(ref iconPath, x + 20, y + 34, 0, 0, 64, 64, 32, 32);

                break;
            }
        }

        if (GameState.Description is null)
        {
            return;
        }

        var offset = 18;
        for (var i = 0; i < GameState.Description.Length; i++)
        {
            TextRenderer.RenderText(GameState.Description[i].Caption,
                x + 140 - TextRenderer.GetTextWidth(GameState.Description[i].Caption) / 2,
                y + offset,
                GameClient.ToXnaColor(GameState.Description[i].Color),
                Color.Black);

            offset += 12;
        }
    }
}