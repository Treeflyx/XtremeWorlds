using Core.Configurations;
using Core.Globals;

namespace Client.Game.UI.Windows;

public static class WinOptions
{
    public static void OnConfirm()
    {
        var restartRequired = false;

        var winOptions = Gui.GetWindowByName("winOptions");
        if (winOptions is null)
        {
            return;
        }

        var checkBoxMusic = winOptions.GetChild("chkMusic");
        var checkBoxSound = winOptions.GetChild("chkSound");
        var checkBoxAutoTile = winOptions.GetChild("chkAutotile");
        var checkBoxFullscreen = winOptions.GetChild("chkFullscreen");
        var comboBoxResolution = winOptions.GetChild("cmbRes");

        // Music
        var enabled = checkBoxMusic.Value != 0;
        if (SettingsManager.Instance.Music != enabled)
        {
            SettingsManager.Instance.Music = enabled;

            if (!enabled)
            {
                TextRenderer.AddText("Music turned off.", (int) ColorName.BrightGreen);

                Sound.StopMusic();
            }
            else
            {
                TextRenderer.AddText("Music tured on.", (int) ColorName.BrightGreen);

                var music = GameState.InGame ? Data.MyMap.Music : SettingsManager.Instance.Music.ToString();
                if (music != "None.")
                {
                    Sound.PlayMusic(music);
                }
                else
                {
                    Sound.StopMusic();
                }
            }
        }

        // Sound
        enabled = checkBoxSound.Value != 0;
        if (SettingsManager.Instance.Sound != enabled)
        {
            SettingsManager.Instance.Sound = enabled;

            TextRenderer.AddText(!enabled ? "Sound turned off." : "Sound tured on.", (int) ColorName.BrightGreen);
        }


        // autotiles
        enabled = checkBoxAutoTile.Value != 0;
        if (SettingsManager.Instance.Autotile != enabled)
        {
            SettingsManager.Instance.Autotile = enabled;
            if (!enabled)
            {
                if (GameState.InGame)
                {
                    TextRenderer.AddText("Autotiles turned off.", (int) ColorName.BrightGreen);
                    Autotile.InitAutotiles();
                }
            }
            else if (GameState.InGame)
            {
                TextRenderer.AddText("Autotiles turned on.", (int) ColorName.BrightGreen);
                Autotile.InitAutotiles();
            }
        }


        // Fullscreen
        enabled = checkBoxFullscreen.Value != 0;
        if (SettingsManager.Instance.Fullscreen != enabled)
        {
            SettingsManager.Instance.Fullscreen = enabled;

            restartRequired = true;
        }

        // Resolution
        if (comboBoxResolution.Value > 0 & comboBoxResolution.Value <= 13)
        {
            SettingsManager.Instance.Resolution = (byte) comboBoxResolution.Value;

            restartRequired = true;
        }

        SettingsManager.Save();

        if (GameState.InGame && restartRequired)
        {
            TextRenderer.AddText("Some changes will take effect next time you load the game.", (int) ColorName.BrightGreen);
        }

        OnClose();
    }

    public static void OnClose()
    {
        Gui.HideWindow("winOptions");
        Gui.ShowWindow("winEscMenu");
    }
}