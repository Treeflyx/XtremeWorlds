using Core.Globals;

namespace Client.Game.UI.Windows;

public static class WinNewChar
{
    public static void OnDrawSprite()
    {
        var winNewChar = Gui.GetWindowByName("winNewChar");
        if (winNewChar is null)
        {
            return;
        }

        var spriteIndex = GameState.NewCnarGender == Sex.Male ? Data.Job[GameState.NewCharJob].MaleSprite : Data.Job[GameState.NewCharJob].FemaleSprite;
        if (spriteIndex == 0)
        {
            spriteIndex = 1;
        }

        var spritePath = Path.Combine(DataPath.Characters, spriteIndex.ToString());
        var sprite = GameClient.GetGfxInfo(Path.Combine(DataPath.Characters, spriteIndex.ToString()));
        if (sprite is null)
        {
            return;
        }

        var w = sprite.Width / 4;
        var h = sprite.Height / 4;

        GameClient.RenderTexture(ref spritePath,
            winNewChar.X + 190,
            winNewChar.Y + 100, 0, 0,
            w, h, w, h);
    }

    public static void OnLeftClick()
    {
        var spriteIndex = GameState.NewCnarGender == Sex.Male ? Data.Job[GameState.NewCharJob].MaleSprite : Data.Job[GameState.NewCharJob].FemaleSprite;
        if (GameState.NewCharSprite < 0)
        {
            GameState.NewCharSprite = spriteIndex;
        }
        else
        {
            GameState.NewCharSprite -= 1;
        }
    }

    public static void OnRightClick()
    {
        var spriteIndex = GameState.NewCnarGender == Sex.Male
            ? Data.Job[GameState.NewCharJob].MaleSprite
            : Data.Job[GameState.NewCharJob].FemaleSprite;

        if (GameState.NewCharSprite >= spriteIndex)
        {
            GameState.NewCharSprite = 1;
        }
        else
        {
            GameState.NewCharSprite += 1;
        }
    }

    public static void OnMaleChecked()
    {
        GameState.NewCharSprite = 1;
        GameState.NewCnarGender = Sex.Male;

        var winNewChar = Gui.GetWindowByName("winNewChar");
        if (winNewChar is null)
        {
            return;
        }

        if (winNewChar.GetChild("chkMale").Value != 0)
        {
            return;
        }

        winNewChar.GetChild("chkFemale").Value = 0;
        winNewChar.GetChild("chkMale").Value = 1;
    }

    public static void OnFemaleChecked()
    {
        GameState.NewCharSprite = 1;
        GameState.NewCnarGender = Sex.Female;

        var winNewChar = Gui.GetWindowByName("winNewChar");
        if (winNewChar is null)
        {
            return;
        }

        if (winNewChar.GetChild("chkFemale").Value != 0)
        {
            return;
        }

        winNewChar.GetChild("chkFemale").Value = 1;
        winNewChar.GetChild("chkMale").Value = 0;
    }

    public static void OnCancel()
    {
        var winNewChar = Gui.GetWindowByName("winNewChar");
        if (winNewChar is null)
        {
            return;
        }

        winNewChar.GetChild("txtName").Text = "";
        winNewChar.GetChild("chkMale").Value = 0;
        winNewChar.GetChild("chkFemale").Value = 0;

        GameState.NewCharSprite = 1;
        GameState.NewCnarGender = Sex.Male;

        Gui.HideWindows();
        Gui.ShowWindow(Gui.GetWindowIndex("winJobs"));
    }

    public static void OnAccept()
    {
        var winNewChar = Gui.GetWindowByName("winNewChar");
        if (winNewChar is null)
        {
            return;
        }

        var name = winNewChar.GetChild("txtName").Text;

        Gui.HideWindows();

        GameLogic.AddChar(name, (int) GameState.NewCnarGender, GameState.NewCharJob, GameState.NewCharSprite);
    }
}