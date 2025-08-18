using Client.Net;
using Core.Globals;
using static Core.Globals.Command;

namespace Client.Game.UI.Windows;

public static class WinCharacter
{
    private static readonly Equipment[] EquipmentTypes = Enum.GetValues<Equipment>();

    public static void Update()
    {
        UpdateBars();

        var winCharacter = Gui.GetWindowByName("winCharacter");
        if (winCharacter is null)
        {
            return;
        }

        winCharacter.GetChild("lblHealth").Text = "Health";
        winCharacter.GetChild("lblSpirit").Text = "Spirit";
        winCharacter.GetChild("lblExperience").Text = "Exp";
        winCharacter.GetChild("lblHealth2").Text = GetPlayerVital(GameState.MyIndex, Vital.Health) + "/" + GetPlayerMaxVital(GameState.MyIndex, Vital.Health);
        winCharacter.GetChild("lblSpirit2").Text = GetPlayerVital(GameState.MyIndex, Vital.Stamina) + "/" + GetPlayerMaxVital(GameState.MyIndex, Vital.Stamina);
        winCharacter.GetChild("lblExperience2").Text = Data.Player[GameState.MyIndex].Exp + "/" + GameState.NextlevelExp;
    }

    private static void UpdateBars()
    {
        var winBars = Gui.GetWindowByName("winBars");
        if (winBars is null)
        {
            return;
        }

        winBars.GetChild("lblHP").Text = GetPlayerVital(GameState.MyIndex, Vital.Health) + "/" + GetPlayerMaxVital(GameState.MyIndex, Vital.Health);
        winBars.GetChild("lblMP").Text = GetPlayerVital(GameState.MyIndex, Vital.Stamina) + "/" + GetPlayerMaxVital(GameState.MyIndex, Vital.Stamina);
        winBars.GetChild("lblEXP").Text = GetPlayerExp(GameState.MyIndex) + "/" + GameState.NextlevelExp;
    }

    public static void OnDrawCharacter()
    {
        if (GameState.MyIndex < 0 || GameState.MyIndex > Constant.MaxPlayers)
        {
            return;
        }

        var winCharacter = Gui.GetWindowByName("winCharacter");
        if (winCharacter is null)
        {
            return;
        }

        var x = winCharacter.X;
        var y = winCharacter.Y;

        // Render bottom
        var argpath = Path.Combine(DataPath.Gui, "37");
        GameClient.RenderTexture(ref argpath, x + 4, y + 314, 0, 0, 40, 38, 40, 38);
        GameClient.RenderTexture(ref argpath, x + 44, y + 314, 0, 0, 40, 38, 40, 38);
        GameClient.RenderTexture(ref argpath, x + 84, y + 314, 0, 0, 40, 38, 40, 38);
        GameClient.RenderTexture(ref argpath, x + 124, y + 314, 0, 0, 46, 38, 46, 38);

        // render top wood
        var argpath4 = Path.Combine(DataPath.Gui, "1");
        GameClient.RenderTexture(ref argpath4, x + 4, y + 23, 100, 100, 166, 291, 166, 291);

        for (var i = 0; i < EquipmentTypes.Length; i++)
        {
            var itemNum = GetPlayerEquipment(GameState.MyIndex, EquipmentTypes[i]);
            if (itemNum < 0)
            {
                continue;
            }

            var itemIcon = Data.Item[itemNum].Icon;
            if (itemIcon <= 0 || itemIcon >= GameState.NumItems)
            {
                continue;
            }

            x = winCharacter.X + GameState.EqLeft + (GameState.EqOffsetX + 32) * (i % GameState.EqColumns);
            y = winCharacter.Y + GameState.EqTop;

            var path = Path.Combine(DataPath.Items, itemIcon.ToString());

            GameClient.RenderTexture(ref path, x, y, 0, 0, 32, 32, 32, 32);
        }
    }

    public static void OnDoubleClick()
    {
        var winCharacter = Gui.GetWindowByName("winCharacter");
        if (winCharacter is null)
        {
            return;
        }

        var slot = General.IsEq(winCharacter.X, winCharacter.Y);
        if (slot >= 0)
        {
            Sender.SendUnequip(slot);
        }

        OnMouseMove();
    }

    public static void OnMouseMove()
    {
        if (Gui.DragBox.Type != DraggablePartType.None)
        {
            return;
        }

        var winCharacter = Gui.GetWindowByName("winCharacter");
        if (winCharacter is null)
        {
            return;
        }

        var winDescription = Gui.GetWindowByName("winDescription");
        if (winDescription is null)
        {
            return;
        }

        var slot = General.IsEq(winCharacter.X, winCharacter.Y);
        if (slot < 0)
        {
            winDescription.Visible = false;
            return;
        }

        var x = winCharacter.X - winDescription.Width;
        if (x < 0)
        {
            x = winCharacter.X + winCharacter.Width;
        }

        var y = winCharacter.Y - 6;

        GameLogic.ShowEqDesc(x, y, slot);
    }

    public static void OnSpendPoint1()
    {
        Sender.SendTrainStat(0);
    }

    public static void OnSpendPoint2()
    {
        Sender.SendTrainStat(1);
    }

    public static void OnSpendPoint3()
    {
        Sender.SendTrainStat(2);
    }

    public static void OnSpendPoint4()
    {
        Sender.SendTrainStat(3);
    }

    public static void OnSpendPoint5()
    {
        Sender.SendTrainStat(4);
    }
}