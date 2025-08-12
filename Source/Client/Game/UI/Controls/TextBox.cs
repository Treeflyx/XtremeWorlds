namespace Client.Game.UI.Controls;

public sealed class TextBox : Control
{
    public void Render(int xO, int yO, Window window)
    {
        string textInput = null;

        if (Design[(int) State] > 0L)
        {
            DesignRenderer.Render(
                Design[(int) State],
                Left + xO,
                Top + yO,
                Width,
                Height,
                Alpha);
        }

        if (Image[(int) State] > 0)
        {
            var path = Path.Combine(Texture[(int) State], Image[(int) State].ToString());

            GameClient.RenderTexture(ref path,
                Left + xO,
                Top + yO, 0, 0,
                Width,
                Height,
                Width,
                Height,
                (byte) Alpha);
        }

        if (Gui.ActiveWindow == window && window.ActiveControl == this)
        {
            textInput = GameState.ChatShowLine;
        }

        var text = ((Censor ? Client.Game.UI.Text.CensorText(Text) : Text) + textInput).Replace("\0", string.Empty);
        var textSize = Client.Game.UI.Text.Fonts[Font].MeasureString(text);

        var left = Left + xO + XOffset;
        var top = Top + yO + YOffset + (Height - textSize.Y) / 2.0d;

        Client.Game.UI.Text.RenderText(text, left, (int) Math.Round(top), Color, Microsoft.Xna.Framework.Color.Black, Font);
    }
}