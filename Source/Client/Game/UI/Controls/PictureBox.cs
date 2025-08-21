namespace Client.Game.UI.Controls;

public sealed class PictureBox : Control
{
    public override void Render(int x, int y)
    {
        var design = GetActiveDesign();
        if (design != Design.None)
        {
            DesignRenderer.Render(design, X + x, Y + y, Width, Height, Alpha);
        }

        var image = GetActiveImage();
        if (image is null)
        {
            return;
        }

        var path = Path.Combine(Texture[(int) State], image.Value.ToString());

        GameClient.RenderTexture(ref path, X + x, Y + y, 0, 0, Width, Height, Width, Height, 255 / Alpha);
    }
}