using Microsoft.Xna.Framework;

namespace Client.Game.UI.Controls;

public sealed class ComboBox : Control
{
    private const int ArrowSprite = 66;

    public List<string> Items { get; } = [];
    
    public override void Render(int x, int y)
    {
        switch (Design)
        {
            case Design.ComboBoxNormal:
                DesignRenderer.Render(Design.TextBlack, X + x, Y + y, Width, Height);

                if (Value > 0)
                {
                    if (Value <= Items.Count - 1)
                    {
                        TextRenderer.RenderText(Items[Value], X + x, Y + y, Color, Color.Black);
                    }
                }

                var path = Path.Combine(Texture[0], ArrowSprite.ToString());

                GameClient.RenderTexture(ref path, X + x + Width, Y + y, 0, 0, 5, 4, 5, 4);
                break;
        }
    }
}