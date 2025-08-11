using Core.Globals;
using Microsoft.Xna.Framework;

namespace Client.Game.UI.Windows;

public static class WinJobs
{
    public static void OnDrawFace()
    {
        var winJobs = Gui.GetWindowByName("winJobs");
        if (winJobs is null)
        {
            return;
        }

        var faceIndex = GameState.NewCharJob switch
        {
            0 => 1, // Warrior
            1 => 2, // Wizard
            2 => 3, // Whisperer
            _ => 0
        };

        var facePath = Path.Combine(DataPath.Characters, faceIndex.ToString());
        var faceTexture = GameClient.GetGfxInfo(facePath);
        if (faceTexture is null)
        {
            return;
        }

        var w = faceTexture.Width / 4;
        var h = faceTexture.Height / 4;

        GameClient.RenderTexture(ref facePath,
            winJobs.Left + 50,
            winJobs.Top + 90,
            0, 0, w, h, w, h);
    }

    public static void Jobs_DrawText()
    {
        const int lineHeight = 14;

        var winJobs = Gui.GetWindowByName("winJobs");
        if (winJobs is null)
        {
            return;
        }

        var lines = default(string[]);

        var text = Data.Job[GameState.NewCharJob].Desc;

        Text.WordWrap(text, winJobs.Font, 330, ref lines);

        var y = winJobs.Top + 60;

        foreach (var line in lines)
        {
            var x = winJobs.Left + 118 + 200 / 2 - Text.GetTextWidth(line, winJobs.Font) / 2;

            var textClean = new string(line.Where(c => Text.Fonts[winJobs.Font].Characters.Contains(c)).ToArray());
            var textSize = Text.Fonts[winJobs.Font].MeasureString(textClean);

            var padding = (int) (textSize.X / 6);

            Text.RenderText(line, x + padding, y, Color.White, Color.Black);

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