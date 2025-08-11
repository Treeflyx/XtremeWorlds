using Core.Globals;
using static Core.Globals.Command;

namespace Client.Game.UI.Windows;

public static class WinSkills
{
    public static void OnMouseMove()
    {
        if (Gui.DragBox.Type != DraggablePartType.None)
        {
            return;
        }

        var winSkills = Gui.GetWindowByName("winSkills");
        if (winSkills is null)
        {
            return;
        }

        var winDescription = Gui.GetWindowByName("winDescription");
        if (winDescription is null)
        {
            return;
        }

        var slot = General.IsSkill(winSkills.Left, winSkills.Top);
        if (slot < 0)
        {
            winDescription.Visible = false;
            return;
        }

        if (Gui.DragBox.Type == DraggablePartType.Item &&
            Gui.DragBox.Value == slot)
        {
            return;
        }

        var x = winSkills.Left - winDescription.Width;
        if (x < 0)
        {
            x = winSkills.Left + winSkills.Width;
        }

        var y = winSkills.Top - 6;

        GameLogic.ShowSkillDesc(x, y, GetPlayerSkill(GameState.MyIndex, slot), slot);
    }

    public static void OnMouseDown()
    {
        var winSkills = Gui.GetWindowByName("winSkills");
        if (winSkills is null)
        {
            return;
        }

        var slot = General.IsSkill(winSkills.Left, winSkills.Top);
        if (slot >= 0)
        {
            ref var dragBox = ref Gui.DragBox;

            dragBox.Type = DraggablePartType.Skill;
            dragBox.Value = Data.Player[GameState.MyIndex].Skill[slot].Num;
            dragBox.Origin = PartOrigin.SkillTree;
            dragBox.Slot = slot;

            var windowIndex = Gui.GetWindowIndex("winDragBox");
            var window = Gui.Windows[windowIndex];

            window.Left = GameState.CurMouseX;
            window.Top = GameState.CurMouseY;
            window.MovedX = GameState.CurMouseX - window.Left;
            window.MovedY = GameState.CurMouseY - window.Top;

            Gui.ShowWindow(windowIndex, resetPosition: false);

            winSkills.State = ControlState.Normal;
        }

        OnMouseMove();
    }

    public static void OnDoubleClick()
    {
        var winSkills = Gui.GetWindowByName("winSkills");
        if (winSkills is null)
        {
            return;
        }

        var slot = General.IsSkill(winSkills.Left, winSkills.Top);
        if (slot >= 0)
        {
            Player.PlayerCastSkill(slot);
        }

        OnMouseMove();
    }
}