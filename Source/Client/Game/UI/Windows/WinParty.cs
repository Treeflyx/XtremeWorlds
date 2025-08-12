using Core.Globals;
using static Core.Globals.Command;

namespace Client.Game.UI.Windows;

public static class WinParty
{
    public static void Update()
    {
        var winParty = Gui.GetWindowByName("winParty");
        if (winParty is null)
        {
            return;
        }

        if (Data.MyParty.Leader == 0)
        {
            Gui.HideWindow("winParty");
            return;
        }

        Gui.ShowWindow("winParty");

        for (var i = 0; i < 4; i++)
        {
            winParty.GetChild("lblName" + i).Text = "";
            winParty.GetChild("picEmptyBar_HP" + i).Visible = false;
            winParty.GetChild("picEmptyBar_SP" + i).Visible = false;
            winParty.GetChild("picBar_HP" + i).Visible = false;
            winParty.GetChild("picBar_SP" + i).Visible = false;
            winParty.GetChild("picShadow" + i).Visible = false;
            winParty.GetChild("picChar" + i).Visible = false;
            winParty.GetChild("picChar" + i).Value = 0;
        }

        var frame = 0;
        for (var i = 0; i < Data.MyParty.MemberCount; i++)
        {
            var playerIndex = Data.MyParty.Member[i];
            if (playerIndex <= 0)
            {
                continue;
            }

            if (playerIndex == GameState.MyIndex || !IsPlaying(playerIndex))
            {
                continue;
            }

            winParty.GetChild("lblName" + frame).Visible = true;
            winParty.GetChild("lblName" + frame).Text = GetPlayerName(playerIndex);
            winParty.GetChild("picShadow" + frame).Visible = true;
            winParty.GetChild("picChar" + frame).Visible = true;
            winParty.GetChild("picChar" + frame).Value = playerIndex;

            for (var x = 0; x <= 4; x++)
            {
                winParty.GetChild("picChar" + frame).Image[x] = GetPlayerSprite(playerIndex);
                winParty.GetChild("picChar" + frame).Texture[x] = DataPath.Characters;
            }

            winParty.GetChild("picEmptyBar_HP" + frame).Visible = true;
            winParty.GetChild("picEmptyBar_SP" + frame).Visible = true;
            winParty.GetChild("picBar_HP" + frame).Visible = true;
            winParty.GetChild("picBar_SP" + frame).Visible = true;

            frame++;
        }

        GameLogic.UpdatePartyBars();

        winParty.Height = Data.MyParty.MemberCount switch
        {
            2 => 78,
            3 => 118,
            4 => 158,
            _ => 0
        };
    }
}