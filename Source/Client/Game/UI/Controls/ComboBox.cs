using Microsoft.Xna.Framework;

namespace Client.Game.UI.Controls;

public sealed class ComboBox : Control
{
    private const int ArrowSprite = 66;

    public List<string> Items { get; } = [];

    // Add a public property for Value (selection index)
    public new int Value { get; set; }

    public override void Render(int x, int y)
    {
        switch (Design)
        {
            case Design.ComboBoxNormal:
                DesignRenderer.Render(Design.TextBlack, X + x, Y + y, Width, Height);

                // Always display the selected item if Value is in range
                if (Items.Count > 0 && Value >= 0 && Value < Items.Count)
                {
                    TextRenderer.RenderText(Items[Value], X + x, Y + y, Color, Color.Black);
                }

                var path = Path.Combine(Texture[0], ArrowSprite.ToString());

                GameClient.RenderTexture(ref path, X + x + Width, Y + y, 0, 0, 5, 4, 5, 4);
                break;
        }
    }
}