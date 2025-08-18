using Core.Globals;
using Microsoft.Xna.Framework;

namespace Client.Game.UI.Controls;

public sealed class Label : Control
{
    public override void Render(int x, int y)
    {
        if (string.IsNullOrEmpty(Text) || Font == Font.None)
        {
            return;
        }

        switch (Align)
        {
            default:
            case Alignment.Left:
                RenderLeftAligned(x, y);
                break;

            case Alignment.Right:
                RenderRightAligned(x, y);
                break;

            case Alignment.Center:
                RenderCenterAligned(x, y);
                break;
        }
    }

    private void RenderLeftAligned(int x, int y)
    {
        if (TextRenderer.GetTextWidth(Text, Font) <= Width)
        {
            TextRenderer.RenderText(Text, X + x + XOffset, Y + y + YOffset, Color, Color.Black, Font);
            return;
        }

        var lines = Array.Empty<string>();
        var lineOffset = 0;

        TextRenderer.WordWrap(Text, Font, Width, ref lines);

        foreach (var line in lines)
        {
            var size = TextRenderer.Fonts[Font].MeasureString(line);
            var padding = (int) (size.X / 6);

            TextRenderer.RenderText(line,
                X + x + XOffset + padding,
                Y + y + YOffset + lineOffset,
                Color, Color.Black, Font);

            lineOffset += 14;
        }
    }

    private void RenderRightAligned(int x, int y)
    {
        if (TextRenderer.GetTextWidth(Text, Font) <= Width)
        {
            var size = TextRenderer.Fonts[Font].MeasureString(Text);

            TextRenderer.RenderText(Text,
                X + Width - (int) size.X + x + XOffset,
                Y + y + YOffset,
                Color, Color.Black, Font);

            return;
        }

        var lines = Array.Empty<string>();
        var lineOffset = 0;

        TextRenderer.WordWrap(Text, Font, Width, ref lines);

        foreach (var line in lines)
        {
            var size = TextRenderer.Fonts[Font].MeasureString(line);
            var padding = (int) (size.X / 6);

            TextRenderer.RenderText(line,
                X + Width - (int) size.X + x + XOffset + padding,
                Y + y + YOffset + lineOffset,
                Color, Color.Black, Font);

            lineOffset += 14;
        }
    }

    private void RenderCenterAligned(int x, int y)
    {
        if (TextRenderer.GetTextWidth(Text, Font) <= Width)
        {
            var size = TextRenderer.Fonts[Font].MeasureString(Text);
            var padding = (int) (size.X / 8);

            TextRenderer.RenderText(Text,
                X + (Width - (int) size.X) / 2 + x + XOffset + padding - 4,
                Y + y + YOffset + (Height - (int) size.Y) / 2,
                Color, Color.Black, Font);

            return;
        }

        var lines = Array.Empty<string>();
        var lineOffset = 0;

        TextRenderer.WordWrap(Text, Font, Width, ref lines);

        foreach (var line in lines)
        {
            var size = TextRenderer.Fonts[Font].MeasureString(line);
            var padding = (int) (size.X / 8);

            TextRenderer.RenderText(line,
                X + (Width - (int) size.X) / 2 + x + XOffset + padding - 4,
                Y + y + YOffset + lineOffset + (Height - (int) size.Y) / 2,
                Color, Color.Black, Font);

            lineOffset += 14;
        }
    }
}