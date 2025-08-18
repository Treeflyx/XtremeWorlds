using Core.Globals;
using Microsoft.Xna.Framework;

namespace Client.Game.UI.Controls;

public sealed class Button : Control
{
    public override void Render(int x, int y)
    {
        var design = GetActiveDesign();
        if (design != Design.None)
        {
            DesignRenderer.Render(design, X + x, Y + y, Width, Height);
        }

        var image = GetActiveImage();
        if (image is not null)
        {
            var path = Path.Combine(Texture[(int) State], image.Value.ToString());

            GameClient.RenderTexture(ref path,
                X + x,
                Y + y, 0, 0,
                Width, Height,
                Width, Height);
        }

        if (Icon > 0)
        {
            var gfxInfo = GameClient.GetGfxInfo(Path.Combine(DataPath.Items, Icon.ToString()));
            if (gfxInfo is not null)
            {
                var path = Path.Combine(DataPath.Items, Icon.ToString());

                GameClient.RenderTexture(ref path,
                    X + x + XOffset,
                    Y + y + YOffset, 0, 0,
                    gfxInfo.Width, gfxInfo.Height,
                    gfxInfo.Width, gfxInfo.Height);
            }
        }

        var size = TextRenderer.Fonts[Font].MeasureString(Text);

        var paddingX = size.X / 6.0d;
        var paddingY = size.Y / 6.0d;

        var textX = X + x + XOffset + (Width - size.X) / 2 + paddingX - 4;
        var textY = Y + y + YOffset + (Height - size.Y) / 2 + paddingY;

        TextRenderer.RenderText(Text,
            (int) Math.Round(textX),
            (int) Math.Round(textY),
            Color, Color.Black,
            Font);
    }
}