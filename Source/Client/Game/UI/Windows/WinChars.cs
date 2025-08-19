using Client.Net;
using Core.Globals;

namespace Client.Game.UI.Windows;

public static class WinChars
{
    public static void OnSelectCharacter1Click()
    {
        Sender.SendUseChar(1);
    }

    public static void OnSelectCharacter2Click()
    {
        Sender.SendUseChar(2);
    }

    public static void OnSelectCharacter3Click()
    {
        Sender.SendUseChar(3);
    }

    private static void TryDeleteCharacter(int slot)
    {
        GameLogic.Dialogue(
            "Delete Character",
            "Deleting this character is permanent.",
            "Delete this character?",
            DialogueType.DeleteCharacter,
            DialogueStyle.YesNo,
            slot);
    }

    public static void OnDeleteCharacter1Click()
    {
        TryDeleteCharacter(1);
    }

    public static void OnDeleteCharacter2Click()
    {
        TryDeleteCharacter(1);
    }

    public static void OnDeleteCharacter3Click()
    {
        TryDeleteCharacter(1);
    }

    private static void TryCreateCharacter(int slot)
    {
        GameState.CharNum = (byte) slot;
        GameLogic.ShowJobs();
    }

    public static void OnCreateCharacter1Click()
    {
        TryCreateCharacter(1);
    }

    public static void OnCreateCharacter2Click()
    {
        TryCreateCharacter(2);
    }

    public static void OnCreateCharacter3Click()
    {
        TryCreateCharacter(3);
    }

    public static void OnClose()
    {
        Gui.HideWindows();
        Gui.ShowWindow("winLogin");
    }
    
    public static void OnDraw()
    {
        var winChars = Gui.GetWindowByName("winChars");
        if (winChars is null)
        {
            return;
        }
        
        var x = winChars.X + 24;
        var y = winChars.Y;
        
        for (var i = 0; i <= Constant.MaxChars - 1; i++)
        {
            if (!string.IsNullOrEmpty(GameState.CharName[i]))
            {
                if (GameState.CharSprite[i] > 0) // Ensure character sprite is valid
                {
                    var spritePath = Path.Combine(DataPath.Characters, GameState.CharSprite[i].ToString());
                    var sprite = GameClient.GetGfxInfo(spritePath);
                    if (sprite is null)
                    {
                        continue;
                    }

                    var w = sprite.Width / 4;
                    var h = sprite.Height / 4;
                    
                    if (GameState.CharSprite[i] <= GameState.NumCharacters)
                    {
                        GameClient.RenderTexture(ref spritePath, x + 30, y + 100, 0, 0, w, h, w, h);
                    }
                }
            }

            // Move to the next position for the next character
            x += 110;
        }
    }
}