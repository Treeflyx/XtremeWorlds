using Core.Globals;
using Microsoft.Xna.Framework;

namespace Client.Game.UI.Controls;

public sealed class CheckBox : Control
{
    private const int SpriteUnchecked = 2;
    private const int SpriteChecked = 3;
    private const int SpriteChat = 51;
    private const int SpriteBuyingChecked = 56;
    private const int SpriteSellingChecked = 57;
    private const int SpriteBuyingUnchecked = 58;
    private const int SpriteSellingUnchecked = 59;

    public int Group { get; set; }
    
    public override void Render(int x, int y)
    {
        switch (Design)
        {
            default:
            case Design.CheckboxNormal:
                RenderNormal(x, y);
                break;

            case Design.CheckboxChat:
                RenderChat(x, y);
                break;

            case Design.CheckboxBuying:
                RenderBuying(x, y);
                break;

            case Design.CheckboxSelling:
                RenderSelling(x, y);
                break;
        }
    }

    private void RenderNormal(int x, int y)
    {
        var sprite = Value == 0 ? SpriteUnchecked : SpriteChecked;
        var path = Path.Combine(Texture[0], sprite.ToString());

        GameClient.RenderTexture(ref path, X + x, Y + y, 0, 0, 16, 16, 16, 16);

        var left = Align switch
        {
            Alignment.Left => X + 18 + x,
            Alignment.Right => X + 18 + (Width - 18) - TextRenderer.GetTextWidth(Text, Font) + x,
            Alignment.Center => (int) Math.Round(X + 18 + (Width - 18) / 2d - TextRenderer.GetTextWidth(Text, Font) / 2d + x),
            _ => 0
        };

        TextRenderer.RenderText(Text, left, Y + y, Color, Color.Black);
    }

    private void RenderChat(int x, int y)
    {
        Alpha = Value == 0 ? 150 : 255;

        var path = Path.Combine(DataPath.Gui, SpriteChat.ToString());

        GameClient.RenderTexture(ref path, X + x, Y + y, 0, 0, 49, 23, 49, 23);

        var left = (int) Math.Round(X + 22 - TextRenderer.GetTextWidth(Text, Font) / 2d + x);

        TextRenderer.RenderText(Text, left + 8, Y + y + 4, Color, Color.Black);
    }

    private void RenderBuying(int x, int y)
    {
        var sprite = Value == 0L ? SpriteBuyingUnchecked : SpriteBuyingChecked;
        var path = Path.Combine(Texture[0], sprite.ToString());

        GameClient.RenderTexture(ref path, X + x, Y + y, 0, 0, 49, 20, 49, 20);
    }

    private void RenderSelling(int x, int y)
    {
        var sprite = Value == 0L ? SpriteSellingUnchecked : SpriteSellingChecked;
        var path = Path.Combine(Texture[0], sprite.ToString());

        GameClient.RenderTexture(ref path, X + x, Y + y, 0, 0, 49, 20, 49, 20);
    }
}