namespace Client.Game.UI.Controls;

public sealed class TextBox : Control
{
    public bool Censor { get; set; }
    
    public override void Render(int x, int y)
    {
        // Always use the new TextBox design (20.png) for all textboxes
        DesignRenderer.Render(Design.TextBox, X + x, Y + y, Width, Height, Alpha);

        var image = GetActiveImage();
        if (image is not null)
        {
            var path = Path.Combine(Texture[(int) State], image.Value.ToString());

            GameClient.RenderTexture(ref path,
                X + x,
                Y + y, 0, 0,
                Width,
                Height,
                Width,
                Height,
                (byte) Alpha);
        }

        string input = null;

        if (Gui.ActiveWindow?.ActiveControl == this)
        {
            input = GameState.ChatShowLine;
        }

        var text = ((Censor ? TextRenderer.CensorText(Text) : Text) + input).Replace("\0", string.Empty);
        var textSize = TextRenderer.Fonts[Font].MeasureString(text);

        TextRenderer.RenderText(
            text,
            X + x + XOffset,
            Y + y + YOffset + (int) (Height - textSize.Y) / 2,
            Color,
            Microsoft.Xna.Framework.Color.Black,
            Font);
    }
}