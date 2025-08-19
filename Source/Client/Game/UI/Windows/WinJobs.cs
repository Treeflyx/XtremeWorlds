using Core.Globals;
using Microsoft.Xna.Framework;

namespace Client.Game.UI.Windows;

public static class WinJobs
{
    public static void OnDrawSprite()
    {
        var winJobs = Gui.GetWindowByName("winJobs");
        if (winJobs is null)
        {
            return;
        }

        int spriteIndex;

        if (Data.Job[GameState.NewCharJob].Name == "")
        {
            spriteIndex = GameState.NewCharJob switch
            {
                0 => 1, // Warrior
                1 => 2, // Wizard
                2 => 3, // Whisperer
                _ => 0
            };
        }
        else
        {
            spriteIndex = winJobs.GetChild("chkMale").Value == 1 
                ? Data.Job[GameState.NewCharJob].MaleSprite 
                : Data.Job[GameState.NewCharJob].FemaleSprite;
        }


        var spritePath = Path.Combine(DataPath.Characters, spriteIndex.ToString());
        var spriteTexture = GameClient.GetGfxInfo(spritePath);
        if (spriteTexture is null)
        {
            return;
        }

        var w = spriteTexture.Width / 4;
        var h = spriteTexture.Height / 4;

        GameClient.RenderTexture(ref spritePath,
            winJobs.X + 50,
            winJobs.Y + 90,
            0, 0, w, h, w, h);
    }

    public static void OnDrawDescription()
    {
        const int lineHeight = 14;

        var winJobs = Gui.GetWindowByName("winJobs");
        if (winJobs is null)
        {
            return;
        }

        var lines = default(string[]);
        var text = "";

        // Get job description or use default
        if (Data.Job[GameState.NewCharJob].Desc == "")
        {
            switch (GameState.NewCharJob)
            {
                case 0: // Warrior
                    {
                        text = "The way of a warrior has never been an easy one. ...";
                        break;
                    }
                case 1: // Wizard
                    {
                        text = "Wizards are often mistrusted characters who ... enjoy setting things on fire.";
                        break;
                    }
                case 2: // Whisperer
                    {
                        text = "The art of healing comes with pressure and guilt, ...";
                        break;
                    }
            }
        }
        else
        {
            text = Data.Job[GameState.NewCharJob].Desc;
        }

        TextRenderer.WordWrap(text, winJobs.Font, 330, ref lines);

        var y = winJobs.Y + 60;

        foreach (var line in lines)
        {
            if (line == "") continue;
            
            var x = winJobs.X + 118 + 200 / 2 - TextRenderer.GetTextWidth(line, winJobs.Font) / 2;

            var textClean = new string(line.Where(c => TextRenderer.Fonts[winJobs.Font].Characters.Contains(c)).ToArray());
            var textSize = TextRenderer.Fonts[winJobs.Font].MeasureString(textClean);

            var padding = (int) (textSize.X / 6);

            TextRenderer.RenderText(line, x + padding, y, Color.White, Color.Black);

            y += lineHeight;
        }
    }

    public static void OnLeftClick()
    {
        var winJobs = Gui.GetWindowByName("winJobs");
        if (winJobs is null)
        {
            return;
        }

        GameState.NewCharJob -= 1;
        if (GameState.NewCharJob < 0)
        {
            GameState.NewCharJob = 0;
        }

        winJobs.GetChild("lblJobName").Text = Data.Job[GameState.NewCharJob].Name;
    }

    public static void OnRightClick()
    {
        var winJobs = Gui.GetWindowByName("winJobs");
        if (winJobs is null)
        {
            return;
        }

        if (GameState.NewCharJob >= Constant.MaxJobs - 1 || string.IsNullOrEmpty(Data.Job[GameState.NewCharJob].Desc) & GameState.NewCharJob >= Constant.MaxJobs)
        {
            return;
        }

        GameState.NewCharJob += 1;

        winJobs.GetChild("lblJobName").Text = Data.Job[GameState.NewCharJob].Name;
    }

    public static void OnAccept()
    {
        Gui.HideWindow("winJobs");
        Gui.ShowWindow("winNewChar");

        var winNewChar = Gui.GetWindowByName("winNewChar");
        if (winNewChar is null)
        {
            return;
        }

        winNewChar.GetChild("txtName").Text = "";
        winNewChar.GetChild("chkMale").Value = 1;
        winNewChar.GetChild("chkFemale").Value = 0;
    }

    public static void OnClose()
    {
        Gui.HideWindows();

        Gui.ShowWindow("winChars");
    }
}