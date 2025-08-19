using Core.Globals;
using Microsoft.Xna.Framework;

namespace Client.Game.UI;

public static class WindowRenderer
{
    public static void Render(Window window)
    {
        if (window.Design[0] == Design.ComboMenuNormal)
        {
            var path = Path.Combine(DataPath.Gui, "1");

            GameClient.RenderTexture(ref path,
                window.X, window.Y, 0, 0,
                window.Width, window.Height,
                157, 0, 0, 0);

            if (window.List.Count == 0)
            {
                return;
            }

            var y = window.Y + 2;
            var x = window.X;

            for (var i = 0; i < window.List.Count - 1; i++)
            {
                if (i == window.Value || i == window.Group)
                {
                    GameClient.RenderTexture(ref path, x, y - 1, 0, 0, window.Width, 15, 255, 0, 0, 0);
                }
                
                var left = x + window.Width / 2 - TextRenderer.GetTextWidth(window.List[i], window.Font) / 2;

                TextRenderer.RenderText(window.List[i], left, y, Color.White, Color.Black);

                y += 16;
            }

            return;
        }

        switch (window.Design[(int) window.State])
        {
            case Design.WindowBlack:
                RenderWindowBlack(window);
                break;

            case Design.WindowNormal:
                RenderWindowNormal(window);
                break;

            case Design.WindowNoBar:
                RenderWindowNoBar(window);
                break;

            case Design.WindowEmpty:
                RenderWindowEmpty(window);
                break;

            case Design.WindowDescription:
                RenderWindowDescription(window);
                break;

            case Design.WindowWithShadow:
                RenderWindowWithShadow(window);
                break;

            case Design.WindowParty:
                RenderWindowParty(window);
                break;
        }

        window.OnDraw?.Invoke();
    }

    private static void RenderWindowBlack(Window window)
    {
        var path = Path.Combine(DataPath.Gui, "61");

        GameClient.RenderTexture(ref path, window.X, window.Y, 0, 0, window.Width, window.Height, 190, 255, 255);
    }

    private static void RenderWindowNormal(Window window)
    {
        var path = Path.Combine(DataPath.Items, window.Icon.ToString());

        DesignRenderer.Render(Design.Wood, window.X, window.Y, window.Width, window.Height);
        DesignRenderer.Render(Design.Green, window.X, window.Y, window.Width, 23);

        GameClient.RenderTexture(ref path,
            window.X + window.XOffset,
            window.Y - 16 + window.YOffset, 0, 0,
            window.Width, window.Height,
            window.Width, window.Height);

        TextRenderer.RenderText(window.Text, window.X + 32, window.Y + 4, Color.White, Color.Black);
    }

    private static void RenderWindowNoBar(Window window)
    {
        DesignRenderer.Render(Design.Wood, window.X, window.Y, window.Width, window.Height);
    }

    private static void RenderWindowEmpty(Window window)
    {
        var path = Path.Combine(DataPath.Items, window.Icon.ToString());

        DesignRenderer.Render(Design.WoodEmpty, window.X, window.Y, window.Width, window.Height);
        DesignRenderer.Render(Design.Green, window.X, window.Y, window.Width, 23);

        GameClient.RenderTexture(ref path,
            window.X + window.XOffset,
            window.Y - 16 + window.YOffset, 0, 0,
            window.Width, window.Height,
            window.Width, window.Height);

        TextRenderer.RenderText(window.Text, window.X + 32, window.Y + 4, Color.White, Color.Black);
    }

    private static void RenderWindowDescription(Window window)
    {
        DesignRenderer.Render(Design.WindowDescription, window.X, window.Y, window.Width, window.Height);
    }

    private static void RenderWindowWithShadow(Window window)
    {
        DesignRenderer.Render(Design.WindowWithShadow, window.X, window.Y, window.Width, window.Height);
    }

    private static void RenderWindowParty(Window window)
    {
        DesignRenderer.Render(Design.WindowParty, window.X, window.Y, window.Width, window.Height);
    }
}