using Core.Globals;

namespace Client.Game.UI.Windows;

public static class WinDialogue
{
    public static void OnOkay()
    {
        GameLogic.DialogueHandler(1L);
    }

    public static void OnYes()
    {
        GameLogic.DialogueHandler(2L);
    }

    public static void OnNo()
    {
        GameLogic.DialogueHandler(3L);
    }

    public static void OnClose()
    {
        switch (GameState.DiaStyle)
        {
            case DialogueStyle.Okay:
                GameLogic.DialogueHandler(1L);
                break;

            case DialogueStyle.YesNo:
                GameLogic.DialogueHandler(3L);
                break;
        }
    }
}