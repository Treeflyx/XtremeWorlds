using Core.Globals;
using Microsoft.Xna.Framework;

namespace Client.Game.UI;

public static class WindowRenderer
{
    public static void Render(int windowIndex)
    {
        if (windowIndex < 0 || windowIndex >= Gui.Windows.Count)
        {
            return;
        }

        var window = Gui.Windows[windowIndex];
        if (window.Censor)
        {
            window.Text = Text.CensorText(window.Text);
        }

        if (window.Design[0] == Design.ComboMenuNormal)
        {
            var path = Path.Combine(DataPath.Gui, "1");

            GameClient.RenderTexture(ref path,
                window.Left, window.Top, 0, 0,
                window.Width, window.Height,
                157, 0, 0, 0);

            if (window.List.Count == 0)
            {
                return;
            }

            var y = window.Top + 2;
            var x = window.Left;

            for (var i = 0; i < window.List.Count - 1; i++)
            {
                // Render selection
                if (i == window.Value || i == window.Group)
                {
                    GameClient.RenderTexture(ref path, x, y - 1, 0, 0, window.Width, 15, 255, 0, 0, 0);
                }

                // Render the text, centered
                var left = x + window.Width / 2 - Text.GetTextWidth(window.List[i], window.Font) / 2;

                Text.RenderText(window.List[i], left, y, Color.White, Color.Black);

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

        GameClient.RenderTexture(ref path, window.Left, window.Top, 0, 0, window.Width, window.Height, 190, 255, 255);
    }

    private static void RenderWindowNormal(Window window)
    {
        var path = Path.Combine(DataPath.Items, window.Icon.ToString());

        DesignRenderer.Render(Design.Wood, window.Left, window.Top, window.Width, window.Height);
        DesignRenderer.Render(Design.Green, window.Left, window.Top, window.Width, 23);

        GameClient.RenderTexture(ref path,
            window.Left + window.XOffset,
            window.Top - 16 + window.YOffset, 0, 0,
            window.Width, window.Height,
            window.Width, window.Height);

        Text.RenderText(window.Text, window.Left + 32, window.Top + 4, Color.White, Color.Black);
    }

    private static void RenderWindowNoBar(Window window)
    {
        DesignRenderer.Render(Design.Wood, window.Left, window.Top, window.Width, window.Height);
    }

    private static void RenderWindowEmpty(Window window)
    {
        var path = Path.Combine(DataPath.Items, window.Icon.ToString());

        DesignRenderer.Render(Design.WoodEmpty, window.Left, window.Top, window.Width, window.Height);
        DesignRenderer.Render(Design.Green, window.Left, window.Top, window.Width, 23);

        GameClient.RenderTexture(ref path,
            window.Left + window.XOffset,
            window.Top - 16 + window.YOffset, 0, 0,
            window.Width, window.Height,
            window.Width, window.Height);

        Text.RenderText(window.Text, window.Left + 32, window.Top + 4, Color.White, Color.Black);
    }

    private static void RenderWindowDescription(Window window)
    {
        DesignRenderer.Render(Design.WindowDescription, window.Left, window.Top, window.Width, window.Height);
    }

    private static void RenderWindowWithShadow(Window window)
    {
        DesignRenderer.Render(Design.WindowWithShadow, window.Left, window.Top, window.Width, window.Height);
    }

    private static void RenderWindowParty(Window window)
    {
        DesignRenderer.Render(Design.WindowParty, window.Left, window.Top, window.Width, window.Height);
    }
}