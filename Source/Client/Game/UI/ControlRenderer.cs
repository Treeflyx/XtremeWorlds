using Client.Game.UI.Controls;
using Core.Globals;
using Microsoft.Xna.Framework;

namespace Client.Game.UI;

public static class ControlRenderer
{
    private static void RenderPictureBox(Control control, int x, int y)
    {
        if (control.Design[(int) control.State] > 0)
        {
            DesignRenderer.Render(control.Design[(int) control.State], control.Left + x, control.Top + y, control.Width, control.Height, control.Alpha);
        }

        if (control.Image[(int) control.State] <= 0)
        {
            return;
        }

        var path = Path.Combine(control.Texture[(int) control.State], control.Image[(int) control.State].ToString());

        GameClient.RenderTexture(ref path,
            control.Left + x,
            control.Top + y, 0, 0,
            control.Width, control.Height,
            control.Width, control.Height,
            (byte) control.Alpha);
    }

    private static void RenderButton(Control control, int x, int y)
    {
        if (control.Design[(int) control.State] > 0)
        {
            DesignRenderer.Render(
                control.Design[(int) control.State],
                control.Left + x,
                control.Top + y,
                control.Width,
                control.Height);
        }

        if (control.Image[(int) control.State] > 0L)
        {
            var path = Path.Combine(control.Texture[(int) control.State], control.Image[(int) control.State].ToString());

            GameClient.RenderTexture(ref path,
                control.Left + x,
                control.Top + y, 0, 0,
                control.Width, control.Height,
                control.Width, control.Height);
        }

        if (control.Icon > 0)
        {
            var gfxInfo = GameClient.GetGfxInfo(Path.Combine(DataPath.Items, control.Icon.ToString()));
            if (gfxInfo is not null)
            {
                var path = Path.Combine(DataPath.Items, control.Icon.ToString());

                GameClient.RenderTexture(ref path,
                    control.Left + x + control.XOffset,
                    control.Top + y + control.YOffset, 0, 0,
                    gfxInfo.Width, gfxInfo.Height,
                    gfxInfo.Width, gfxInfo.Height);
            }
        }

        var size = Text.Fonts[control.Font].MeasureString(control.Text);

        var paddingX = size.X / 6.0d;
        var paddingY = size.Y / 6.0d;

        var textX = control.Left + x + control.XOffset + (control.Width - size.X) / 2 + paddingX - 4;
        var textY = control.Top + y + control.YOffset + (control.Height - size.Y) / 2 + paddingY;

        Text.RenderText(control.Text,
            (int) Math.Round(textX),
            (int) Math.Round(textY),
            control.Color, Color.Black,
            control.Font);
    }

    private static void RenderLabel(Control control, int xO, int yO)
    {
        var textArray = default(string[]);
        long count;
        long i;
        var yOffset = 0L;
        long left;

        if (control.Text.Length <= 0 || control.Font <= 0)
        {
            return;
        }

        switch (control.Align)
        {
            case Alignment.Left:
            {
                if (Text.GetTextWidth(control.Text, control.Font) > control.Width)
                {
                    Text.WordWrap(control.Text, control.Font, control.Width, ref textArray);
                    count = textArray.Length;
                    var loopTo = count;
                    for (i = 0L; i < loopTo; i++)
                    {
                        var actualSize = Text.Fonts[control.Font].MeasureString(textArray[(int) i]);
                        var actualWidth = actualSize.X;
                        var padding = actualWidth / 6.0d;
                        left = (long) Math.Round(control.Left + xO + control.XOffset + padding);

                        Text.RenderText(textArray[(int) i], (int) left, (int) (control.Top + yO + control.YOffset + yOffset), control.Color, Color.Black, control.Font);
                        yOffset += 14L;
                    }
                }
                else
                {
                    var actualSize = Text.Fonts[control.Font].MeasureString(control.Text);

                    left = control.Left + xO + control.XOffset;

                    Text.RenderText(control.Text, (int) left, control.Top + yO + control.YOffset, control.Color, Color.Black, control.Font);
                }

                break;
            }

            case Alignment.Right:
            {
                if (Text.GetTextWidth(control.Text, control.Font) > control.Width)
                {
                    Text.WordWrap(control.Text, control.Font, control.Width, ref textArray);
                    count = textArray.Length;
                    var loopTo1 = count;
                    for (i = 0L; i < loopTo1; i++)
                    {
                        var actualSize = Text.Fonts[control.Font].MeasureString(textArray[(int) i]);
                        var actualWidth = actualSize.X;
                        var padding = actualWidth / 6.0d;
                        left = (long) Math.Round(control.Left + control.Width - actualWidth + xO + control.XOffset + padding);

                        Text.RenderText(textArray[(int) i], (int) left, (int) (control.Top + yO + control.YOffset + yOffset), control.Color, Color.Black, control.Font);
                        yOffset += 14L;
                    }
                }
                else
                {
                    var actualSize = Text.Fonts[control.Font].MeasureString(control.Text);

                    left = (long) Math.Round(control.Left + control.Width - actualSize.X + xO + control.XOffset);

                    Text.RenderText(control.Text, (int) left, control.Top + yO + control.YOffset, control.Color, Color.Black, control.Font);
                }

                break;
            }

            case Alignment.Center:
            {
                if (Text.GetTextWidth(control.Text, control.Font) > control.Width)
                {
                    Text.WordWrap(control.Text, control.Font, control.Width, ref textArray);
                    count = textArray.Length;

                    var loopTo2 = count;
                    for (i = 0L; i < loopTo2; i++)
                    {
                        var actualSize = Text.Fonts[control.Font].MeasureString(textArray[(int) i]);
                        var actualWidth = actualSize.X;
                        var actualHeight = actualSize.Y;
                        var padding = actualWidth / 8.0d;
                        left = (long) Math.Round(control.Left + (control.Width - actualWidth) / 2.0d + xO + control.XOffset + padding - 4d);
                        var top = control.Top + yO + control.YOffset + yOffset + (control.Height - actualHeight) / 2.0d;

                        Text.RenderText(textArray[(int) i], (int) left, (int) Math.Round(top), control.Color, Color.Black, control.Font);
                        yOffset += 14L;
                    }
                }
                else
                {
                    var actualSize = Text.Fonts[control.Font].MeasureString(control.Text);
                    var actualWidth = actualSize.X;
                    var actualHeight = actualSize.Y;
                    var padding = actualWidth / 8.0d;
                    left = (long) Math.Round(control.Left + (control.Width - actualWidth) / 2.0d + xO + control.XOffset + padding - 4d);
                    var top = control.Top + yO + control.YOffset + (control.Height - actualHeight) / 2.0d;

                    Text.RenderText(control.Text, (int) left, (int) Math.Round(top), control.Color, Color.Black, control.Font);
                }

                break;
            }
        }
    }

    private static void RenderCheckBox(Control control, int xO, int yO)
    {
        var textArray = default(string[]);
        var taddText = default(string);
        var yOffset = 0L;
        long sprite;
        int left;

        switch (control.Design[0])
        {
            case Design.CheckboxNormal:
            {
                sprite = control.Value == 0L ? 2L : 3L;

                // render box
                var argpath4 = Path.Combine(control.Texture[0], sprite.ToString());
                GameClient.RenderTexture(ref argpath4, control.Left + xO, control.Top + yO, 0, 0, 16, 16, 16, 16);

                // find text position
                left = control.Align switch
                {
                    Alignment.Left => control.Left + 18 + xO,
                    Alignment.Right => control.Left + 18 + (control.Width - 18) - Text.GetTextWidth(control.Text, control.Font) + xO,
                    Alignment.Center => (int) Math.Round(control.Left + 18 + (control.Width - 18) / 2d - Text.GetTextWidth(control.Text, control.Font) / 2d + xO),
                    _ => 0
                };

                Text.RenderText(control.Text, left, control.Top + yO, control.Color, Color.Black);
                break;
            }

            case Design.CheckboxChat:
            {
                control.Alpha = control.Value == 0L ? 150 : 255;

                // render box
                var argpath5 = Path.Combine(DataPath.Gui, 51.ToString());
                GameClient.RenderTexture(ref argpath5, control.Left + xO, control.Top + yO, 0, 0, 49, 23, 49, 23);

                // render text
                left = (int) Math.Round(control.Left + 22L - Text.GetTextWidth(control.Text, control.Font) / 2d + xO);
                Text.RenderText(control.Text, left + 8, (int) (control.Top + yO + 4L), control.Color, Color.Black);
                break;
            }

            case Design.CheckboxBuying:
            {
                sprite = control.Value == 0L ? 58L : 56L;
                var argpath6 = Path.Combine(control.Texture[0], sprite.ToString());
                GameClient.RenderTexture(ref argpath6, control.Left + xO, control.Top + yO, 0, 0, 49, 20, 49, 20);
                break;
            }

            case Design.CheckboxSelling:
            {
                sprite = control.Value == 0L ? 59L : 57L;
                var argpath7 = Path.Combine(control.Texture[0], sprite.ToString());
                GameClient.RenderTexture(ref argpath7, control.Left + xO, control.Top + yO, 0, 0, 49, 20, 49, 20);
                break;
            }
        }
    }

    private static void RenderComboBox(Control control, int xO, int yO)
    {
        switch (control.Design[0])
        {
            case Design.ComboBoxNormal:
            {
                // draw the background
                DesignRenderer.Render(Design.TextBlack, control.Left + xO, control.Top + yO, control.Width, control.Height);

                // render the text
                if (control.Value > 0L)
                {
                    if (control.Value <= control.List.Count - 1)
                    {
                        Text.RenderText(control.List[control.Value], control.Left + xO, control.Top + yO, control.Color, Color.Black);
                    }
                }

                // draw the little arrow
                var argpath8 = Path.Combine(control.Texture[0], "66");
                GameClient.RenderTexture(ref argpath8, control.Left + xO + control.Width, control.Top + yO, 0, 0, 5, 4, 5, 4);
                break;
            }
        }
    }

    public static void RenderControl(Window window, Control control)
    {
        var x = window.Left;
        var y = window.Top;

        switch (control)
        {
            case TextBox textBox:
                textBox.Render(x, y, window);
                break;

            default:
                switch (control.Type)
                {
                    case ControlType.PictureBox:
                        RenderPictureBox(control, x, y);
                        break;

                    case ControlType.TextBox:
                        break;

                    case ControlType.Button:
                        RenderButton(control, x, y);
                        break;

                    case ControlType.Label:
                        RenderLabel(control, x, y);
                        break;

                    case ControlType.Checkbox:
                        RenderCheckBox(control, x, y);
                        break;

                    case ControlType.ComboMenu:
                        RenderComboBox(control, x, y);
                        break;
                }

                break;
        }

        control.OnDraw?.Invoke();
    }
}