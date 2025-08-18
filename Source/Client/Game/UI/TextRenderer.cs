using System.Text;
using Core.Configurations;
using Core.Globals;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core.Globals.Command;

namespace Client.Game.UI;

public static class TextRenderer
{
    public static readonly Dictionary<Font, SpriteFont> Fonts = new();

    public static Color GetColorForAmount(int amount)
    {
        return amount switch
        {
            < 1000000 => Color.White,
            < 10000000 => Color.Yellow,
            _ => Color.LightGreen
        };
    }

    public static string CensorText(string input)
    {
        return new string('*', input.Length);
    }

    public static string SanitizeText(string text, SpriteFont font)
    {
        if (string.IsNullOrEmpty(text))
        {
            return "";
        }

        var sanitizedText = new StringBuilder();

        foreach (var ch in text)
        {
            if (font.Characters.Contains(ch))
            {
                sanitizedText.Append(ch);
            }
        }

        return sanitizedText.ToString();
    }

    // Get the width of the text with optional scaling
    public static int GetTextWidth(string text, Font font = Font.Georgia, float textSize = 1.0f)
    {
        if (!Fonts.TryGetValue(font, out var spriteFont))
        {
            throw new ArgumentException("Font not found.");
        }

        var sanitizedText = SanitizeText(text, spriteFont);
        var textDimensions = spriteFont.MeasureString(sanitizedText);
        return (int) Math.Round(textDimensions.X * textSize);
    }

    // Get the height of the text with optional scaling

    public static void AddText(string text, int color, long alpha = 255L, byte channel = 0)
    {
        // wordwrap
        string[] wrappedLines = null;
        WordWrap(text, Font.Georgia, Gui.Windows[Gui.GetWindowIndex("winChat")].Width, ref wrappedLines);

        GameState.ChatHighIndex += wrappedLines.Length;

        if (GameState.ChatHighIndex > Constant.ChatLines)
            GameState.ChatHighIndex = Constant.ChatLines;

        // Move the rest of the chat lines up
        for (var i = (int) GameState.ChatHighIndex - wrappedLines.Length; i > 0; i--)
        {
            Data.Chat[i] = Data.Chat[i - 1];
        }

        for (int i = wrappedLines.Length - 1, chatIndex = 0; i >= 0; i--, chatIndex++)
        {
            // Add the wrapped line to the chat
            Data.Chat[chatIndex].Text = wrappedLines[i];
            Data.Chat[chatIndex].Color = color;
            Data.Chat[chatIndex].Visible = true;
            Data.Chat[chatIndex].Timer = General.GetTickCount();
            Data.Chat[chatIndex].Channel = channel;
        }
    }

    public static void WordWrap(string text, Font font, long maxLineLen, ref string[] theArray)
    {
        var lineCount = 0L;

        // Too small of text
        if (Strings.Len(text) < 2)
        {
            theArray = new string[2];
            theArray[1] = text;
            return;
        }

        // default values
        var b = 1L;
        var lastSpace = 1L;
        var size = 0L;
        long tmpNum = Strings.Len(text);

        var loopTo = tmpNum;
        for (var i = 1L; i <= loopTo; i++)
        {
            // if it's a space, store it
            switch (Strings.Mid(text, (int) i, 1) ?? "")
            {
                case " ":
                {
                    lastSpace = i;
                    break;
                }
            }

            // Add up the size
            size = size + 10L;

            // Check for too large of a size
            if (size > maxLineLen)
            {
                // Check if the last space was too far back
                if (i - lastSpace > 10L)
                {
                    // Too far away to the last space, so break at the last character
                    lineCount = lineCount + 1L;
                    Array.Resize(ref theArray, (int) (lineCount));
                    theArray[(int) lineCount - 1] = Strings.Mid(text, (int) b, (int) (i - 1L - b));
                    b = i - 1L;
                    size = 0L;
                }
                else
                {
                    // Break at the last space to preserve the word
                    lineCount = lineCount + 1L;
                    Array.Resize(ref theArray, (int) (lineCount));

                    // Ensure b is within valid range
                    if (b < 0L)
                        b = 0L;

                    if (b > text.Length)
                        b = text.Length;

                    // Ensure the length parameter is not negative
                    var substringLength = (int) (lastSpace - b);
                    if (substringLength < 0)
                        substringLength = 0;

                    // Extract the substring and assign it to the array
                    theArray[(int) lineCount - 1] = Strings.Mid(text, (int) b, substringLength);

                    b = lastSpace + 1L;
                    // Count all the words we ignored (the ones that weren't printed, but are before "i")
                    size = GetTextWidth(Strings.Mid(text, (int) lastSpace, (int) (i - lastSpace)), font);
                }
            }

            // Remainder
            if (i == Strings.Len(text))
            {
                if (b != i)
                {
                    lineCount = lineCount + 1L;
                    Array.Resize(ref theArray, (int) (lineCount));
                    theArray[(int) lineCount - 1] = Strings.Mid(text, (int) b, (int) i);
                }
            }
        }
    }

    public static void RenderText(string text, int x, int y, Color frontColor, Color backColor, Font font = Font.Georgia)
    {
        if (text == null) return;
        var sanitizedText = new string(text.Where(c => Fonts[font].Characters.Contains(c)).ToArray());
        GameClient.SpriteBatch.DrawString(Fonts[font], sanitizedText, new Vector2(x + 1, y + 1), backColor, 0.0f, Vector2.Zero, 12f / 16.0f, SpriteEffects.None, 0.0f);
        GameClient.SpriteBatch.DrawString(Fonts[font], sanitizedText, new Vector2(x, y), frontColor, 0.0f, Vector2.Zero, 12f / 16.0f, SpriteEffects.None, 0.0f);
    }

    public static void DrawMapAttributes()
    {
        int tA;

        var loopTo = (int) GameState.TileView.Right;
        for (var x = (int) GameState.TileView.Left; x < loopTo; x++)
        {
            var loopTo1 = (int) GameState.TileView.Bottom;
            for (var y = (int) GameState.TileView.Top; y < loopTo1; y++)
            {
                if (GameLogic.IsValidMapPoint(x, y))
                {
                    {
                        ref var withBlock = ref Data.MyMap.Tile[x, y];
                        var tX = (int) Math.Round(GameLogic.ConvertMapX(x * GameState.SizeX) - 4 + GameState.SizeX * 0.5d);
                        var tY = (int) Math.Round(GameLogic.ConvertMapY(y * GameState.SizeY) - 7 + GameState.SizeY * 0.5d);

                        if (GameState.EditorAttribute == 1)
                        {
                            tA = (int) withBlock.Type;
                        }
                        else
                        {
                            tA = (int) withBlock.Type2;
                        }

                        switch (tA)
                        {
                            case (int) TileType.Blocked:
                            {
                                RenderText("B", tX, tY, Color.Red, Color.Black);
                                break;
                            }
                            case (int) TileType.Warp:
                            {
                                RenderText("W", tX, tY, Color.Blue, Color.Black);
                                break;
                            }
                            case (int) TileType.Item:
                            {
                                RenderText("I", tX, tY, Color.White, Color.Black);
                                break;
                            }
                            case (int) TileType.NpcAvoid:
                            {
                                RenderText("N", tX, tY, Color.White, Color.Black);
                                break;
                            }
                            case (int) TileType.Resource:
                            {
                                RenderText("R", tX, tY, Color.Green, Color.Black);
                                break;
                            }
                            case (int) TileType.NpcSpawn:
                            {
                                RenderText("S", tX, tY, Color.Yellow, Color.Black);
                                break;
                            }
                            case (int) TileType.Shop:
                            {
                                RenderText("S", tX, tY, Color.Blue, Color.Black);
                                break;
                            }
                            case (int) TileType.Bank:
                            {
                                RenderText("B", tX, tY, Color.Blue, Color.Black);
                                break;
                            }
                            case (int) TileType.Heal:
                            {
                                RenderText("H", tX, tY, Color.Green, Color.Black);
                                break;
                            }
                            case (int) TileType.Trap:
                            {
                                RenderText("T", tX, tY, Color.Red, Color.Black);
                                break;
                            }
                            case (int) TileType.Animation:
                            {
                                RenderText("A", tX, tY, Color.Red, Color.Black);
                                break;
                            }
                            case (int) TileType.NoCrossing:
                            {
                                RenderText("X", tX, tY, Color.Red, Color.Black);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    public static void DrawNpcName(int mapNpcNum)
    {
        int textY;
        var color = default(Color);
        var backColor = default(Color);

        double npcNum = Data.MyMapNpc[mapNpcNum].Num;

        if (npcNum < 0 | npcNum > Constant.MaxNpcs)
            return;

        switch (Data.Npc[(int) npcNum].Behaviour)
        {
            case 0: // attack on sight
            {
                color = Color.Red;
                backColor = Color.Black;
                break;
            }
            case 1:
            case 4: // attack when attacked + guard
            {
                color = Color.Green;
                backColor = Color.Black;
                break;
            }
            case 2:
            case 3:
            case 5: // friendly + shopkeeper + quest
            {
                color = Color.Yellow;
                backColor = Color.Black;
                break;
            }
        }

        var textX = GameLogic.ConvertMapX(Data.MyMapNpc[mapNpcNum].X) + GameState.SizeX / 2 - 6;
        textX -= (int) (GetTextWidth(Data.Npc[(int) npcNum].Name) / 6d);

        if (Data.Npc[(int) npcNum].Sprite < 1 | Data.Npc[(int) npcNum].Sprite > GameState.NumCharacters)
        {
            textY = GameLogic.ConvertMapY(Data.MyMapNpc[mapNpcNum].Y) - 16;
        }
        else
        {
            textY = GameLogic.ConvertMapY((int) (Data.MyMapNpc[mapNpcNum].Y - GameClient.GetGfxInfo(Path.Combine(DataPath.Characters, Data.Npc[(int) npcNum].Sprite.ToString())).Height / 4d + 16d));
        }

        // Draw name
        RenderText(Data.Npc[(int) npcNum].Name, textX, textY, color, backColor);
    }

    public static void DrawEventName(int index)
    {
        var textY = 0;

        var color = Color.Yellow;
        var backcolor = Color.Black;

        var name = Data.MapEvents[index].Name;

        // calc pos
        var textX = GameLogic.ConvertMapX(Data.MapEvents[index].X) + GameState.SizeX / 2 - 6;
        textX -= GetTextWidth(name) / 6;

        if (Data.MapEvents[index].GraphicType == 0)
        {
            textY = GameLogic.ConvertMapY(Data.MapEvents[index].Y) - 16;
        }
        else if (Data.MapEvents[index].GraphicType == 1)
        {
            if (Data.MapEvents[index].Graphic < 1 | Data.MapEvents[index].Graphic > GameState.NumCharacters)
            {
                textY = GameLogic.ConvertMapY(Data.MapEvents[index].Y) - 16;
            }
            else
            {
                // Determine location for text
                textY = GameLogic.ConvertMapY(Data.MapEvents[index].Y) - GameClient.GetGfxInfo(Path.Combine(DataPath.Characters, Data.MapEvents[index].Graphic.ToString())).Height / 4 + 16;
            }
        }
        else if (Data.MapEvents[index].GraphicType == 2)
        {
            if (Data.MapEvents[index].GraphicY2 > 0)
            {
                textX = textX + Data.MapEvents[index].GraphicY2 * GameState.SizeY / 2 - 16;
                textY = GameLogic.ConvertMapY(Data.MapEvents[index].Y) - Data.MapEvents[index].GraphicY2 * GameState.SizeY + 16;
            }
            else
            {
                textY = GameLogic.ConvertMapY(Data.MapEvents[index].Y) - 32 + 16;
            }
        }

        // Draw name
        RenderText(name, textX, textY, color, backcolor);
    }

    public static void DrawActionMsg(int index)
    {
        var x = 0;
        var y = 0;
        var time = 0;

        // how long we want each message to appear
        switch (Data.ActionMsg[index].Type)
        {
            case (int) ActionMessageType.Static:
            {
                time = 1500;

                if (Data.ActionMsg[index].Y > 0)
                {
                    x = Data.ActionMsg[index].X + Conversion.Int(GameState.SizeX / 2) - Strings.Len(Data.ActionMsg[index].Message) / 2 * 8;
                    y = Data.ActionMsg[index].Y - Conversion.Int(GameState.SizeY / 2) - 2;
                }
                else
                {
                    x = Data.ActionMsg[index].X + Conversion.Int(GameState.SizeX / 2) - Strings.Len(Data.ActionMsg[index].Message) / 2 * 8;
                    y = Data.ActionMsg[index].Y - Conversion.Int(GameState.SizeY / 2) + 18;
                }

                break;
            }

            case (int) ActionMessageType.Scroll:
            {
                time = 1500;

                if (Data.ActionMsg[index].Y > 0)
                {
                    x = Data.ActionMsg[index].X + Conversion.Int(GameState.SizeX / 2) - Strings.Len(Data.ActionMsg[index].Message) / 2 * 8;
                    y = (int) Math.Round(Data.ActionMsg[index].Y - Conversion.Int(GameState.SizeY / 2) - 2 - Data.ActionMsg[index].Scroll * 0.6d);
                    Data.ActionMsg[index].Scroll = Data.ActionMsg[index].Scroll + 1;
                }
                else
                {
                    x = Data.ActionMsg[index].X + Conversion.Int(GameState.SizeX / 2) - Strings.Len(Data.ActionMsg[index].Message) / 2 * 8;
                    y = (int) Math.Round(Data.ActionMsg[index].Y - Conversion.Int(GameState.SizeY / 2) + 18 + Data.ActionMsg[index].Scroll * 0.6d);
                    Data.ActionMsg[index].Scroll = Data.ActionMsg[index].Scroll + 1;
                }

                break;
            }

            case (int) ActionMessageType.Screen:
            {
                time = 3000;

                // This will kill any action screen messages that there in the system
                for (int i = byte.MaxValue; i >= 0; i -= 1)
                {
                    if (Data.ActionMsg[i].Type == (int) ActionMessageType.Screen)
                    {
                        if (i != index)
                        {
                            GameLogic.ClearActionMsg((byte) index);
                            index = i;
                        }
                    }
                }

                x = GameState.ResolutionWidth / 2 - Strings.Len(Data.ActionMsg[index].Message) / 2 * 8;
                y = 425;
                break;
            }
        }

        x = GameLogic.ConvertMapX(x);
        y = GameLogic.ConvertMapY(y);

        if (General.GetTickCount() < Data.ActionMsg[index].Created + time)
        {
            RenderText(Data.ActionMsg[index].Message, x, y, GameClient.QbColorToXnaColor(Data.ActionMsg[index].Color), Color.Black);
        }
        else
        {
            GameLogic.ClearActionMsg((byte) index);
        }
    }

    public static void DrawChat()
    {
        var yOffset = 0L;
        string tmpText;
        var topWidth = 0;
        string[] tmpArray;

        // set the position
        var xO = 19L;
        xO += Gui.Windows[Gui.GetWindowIndex("winChat")].X;
        long yO = GameState.ResolutionHeight - 45;
        var width = (int) Gui.Windows[Gui.GetWindowIndex("winChat")].Width;

        // loop through chat
        var rLines = 1;
        var i = GameState.ChatScroll;

        while (rLines < 8)
        {
            if (i >= Constant.ChatLines)
                break;
            var lineCount = 1;

            // exit out early if we come to a blank string
            if (Strings.Len(Data.Chat[(int) i].Text) == 0)
                break;

            // get visible state
            var isVisible = true;
            if (GameState.InSmallChat)
            {
                if (!Data.Chat[(int) i].Visible)
                    isVisible = false;
            }

            if (SettingsManager.Instance.ChannelState[Data.Chat[i].Channel] == 0)
                isVisible = false;

            // make sure it's visible
            if (isVisible)
            {
                // render line
                var color = Data.Chat[(int) i].Color;
                var color2 = GameClient.QbColorToXnaColor(color);

                // check if we need to word wrap
                if (GetTextWidth(Data.Chat[i].Text) > width)
                {
                    // word wrap
                    string[] wrappedLines = null;
                    WordWrap(Data.Chat[(int) i].Text, Font.Georgia, width, ref wrappedLines);

                    // continue on
                    yOffset = yOffset - 10 * wrappedLines.Length;
                    for (var j = 0; j < wrappedLines.Length; j++)
                    {
                        RenderText(wrappedLines[j], (int) xO, (int) (yO + yOffset + 10 * j), color2, color2);
                    }

                    rLines += wrappedLines.Length;

                    // set the top width
                    var loopTo = Information.UBound(wrappedLines);
                    for (var x = 0; x < loopTo; x++)
                    {
                        if (GetTextWidth(wrappedLines[x]) > topWidth)
                            topWidth = GetTextWidth(wrappedLines[x]);
                    }
                }
                else
                {
                    // normal
                    yOffset = yOffset - 12L; // Adjusted spacing from 14 to 12

                    RenderText(Data.Chat[(int) i].Text, (int) xO, (int) (yO + yOffset), color2, color2);
                    rLines = rLines + 1;

                    // set the top width
                    if (GetTextWidth(Data.Chat[(int) i].Text) > topWidth)
                        topWidth = GetTextWidth(Data.Chat[(int) i].Text);
                }
            }

            // increment chat pointer
            i = i + 1L;
        }

        // get the height of the small chat box
        GameLogic.SetChatHeight(rLines * 12); // Adjusted spacing from 14 to 12
        GameLogic.SetChatWidth(topWidth);
    }

    public static void DrawMapName()
    {
        RenderText(Data.MyMap.Name, (int) Math.Round(GameState.ResolutionWidth / 2d - GetTextWidth(Data.MyMap.Name)), 10, GameState.DrawMapNameColor, Color.Black);
    }

    public static void DrawPlayerName(int index)
    {
        int textY;
        var color = default(Color);
        var backColor = default(Color);

        // Check access level
        if (!GetPlayerPk(index))
        {
            switch (GetPlayerAccess(index))
            {
                case (int) AccessLevel.Player:
                    color = Color.White;
                    backColor = Color.Black;
                    break;

                case (int) AccessLevel.Moderator:
                    color = Color.Cyan;
                    backColor = Color.White;
                    break;

                case (int) AccessLevel.Mapper:
                    color = Color.Green;
                    backColor = Color.Black;
                    break;

                case (int) AccessLevel.Developer:
                    color = Color.Blue;
                    backColor = Color.Black;
                    break;

                case (int) AccessLevel.Owner:
                    color = Color.Yellow;
                    backColor = Color.Black;
                    break;
            }
        }
        else
        {
            color = Color.Red;
        }

        var name = Data.Player[index].Name;

        // calc pos
        var textX = GameLogic.ConvertMapX(GetPlayerRawX(index)) + 8;
        textX = (int) Math.Round(textX - GetTextWidth(name) / 6d);

        if (GetPlayerSprite(index) <= 0 | GetPlayerSprite(index) > GameState.NumCharacters)
        {
            textY = GameLogic.ConvertMapY(GetPlayerRawY(index)) - 16;
        }
        else
        {
            // Determine location for text
            textY = (int) Math.Round((decimal) GameLogic.ConvertMapY((int) (GetPlayerRawY(index) - GameClient.GetGfxInfo(Path.Combine(DataPath.Characters, GetPlayerSprite(index).ToString())).Height / 4d + 16d)));
        }

        // Draw name
        RenderText(name, textX, textY, color, backColor);
    }
}