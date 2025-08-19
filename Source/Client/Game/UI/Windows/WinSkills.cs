using Core.Globals;
using static Core.Globals.Command;

namespace Client.Game.UI.Windows;

public static class WinSkills
{
    public static void OnDraw()
    {
        if (GameState.MyIndex < 0 || GameState.MyIndex >= Constant.MaxPlayers)
        {
            return;
        }

        var winSkills = Gui.GetWindowByName("winSkills");
        if (winSkills is null)
        {
            return;
        }

        // render green
        var greenPath = Path.Combine(DataPath.Gui, "34");
        
        GameClient.RenderTexture(ref greenPath, 
            winSkills.X + 4, 
            winSkills.Y + 23, 
            0, 0,
            winSkills.Width - 8, 
            winSkills.Height - 27, 
            4, 4);
        
        var height = 76;
        
        var x = winSkills.X;
        var y = winSkills.Y + 23;

        for (var i = 0; i < 4; i++)
        {
            if (i == 3)
            {
                height = 42;
            }
            
            var path = Path.Combine(DataPath.Gui, "35");
            
            GameClient.RenderTexture(ref path, x + 4, y, 0, 0, 76, height, 76, height);
            GameClient.RenderTexture(ref path, x + 80, y, 0, 0, 76, height, 76, height);
            GameClient.RenderTexture(ref path, x + 156, y, 0, 0, 42, height, 42, height);
            
            y += 76;
        }
        
        for (var slot = 0; slot < Constant.MaxPlayerSkills; slot++)
        {
            var skillNum = Data.Player[GameState.MyIndex].Skill[slot].Num;
            if (skillNum is < 0 or >= Constant.MaxSkills)
            {
                continue;
            }
            
            Database.StreamSkill(skillNum);

            if (Gui.DragBox.Origin == PartOrigin.SkillTree &&
                Gui.DragBox.Slot == slot)
            {
                continue;
            }
            
            var icon = Data.Skill[skillNum].Icon;
            if (icon < 0 || icon >= GameState.NumSkills)
            {
                continue;
            }
                    
            var top = winSkills.Y + GameState.SkillTop + (GameState.SkillOffsetY + 32) * (slot / GameState.SkillColumns);
            var left = winSkills.X + GameState.SkillLeft + (GameState.SkillOffsetX + 32) * (slot % GameState.SkillColumns);

            var iconPath = Path.Combine(DataPath.Skills, icon.ToString());
                        
            GameClient.RenderTexture(ref iconPath, left, top, 0, 0, 32, 32, 32, 32);
        }
    }

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

        var slot = General.IsSkill(winSkills.X, winSkills.Y);
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

        var x = winSkills.X - winDescription.Width;
        if (x < 0)
        {
            x = winSkills.X + winSkills.Width;
        }

        var y = winSkills.Y - 6;

        GameLogic.ShowSkillDesc(x, y, GetPlayerSkill(GameState.MyIndex, slot), slot);
    }

    public static void OnMouseDown()
    {
        var winSkills = Gui.GetWindowByName("winSkills");
        if (winSkills is null)
        {
            return;
        }

        var slot = General.IsSkill(winSkills.X, winSkills.Y);
        if (slot >= 0)
        {
            ref var dragBox = ref Gui.DragBox;

            dragBox.Type = DraggablePartType.Skill;
            dragBox.Value = Data.Player[GameState.MyIndex].Skill[slot].Num;
            dragBox.Origin = PartOrigin.SkillTree;
            dragBox.Slot = slot;

            var windowIndex = Gui.GetWindowIndex("winDragBox");
            var window = Gui.Windows[windowIndex];

            window.X = GameState.CurMouseX;
            window.Y = GameState.CurMouseY;
            window.MovedX = GameState.CurMouseX - window.X;
            window.MovedY = GameState.CurMouseY - window.Y;

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

        var slot = General.IsSkill(winSkills.X, winSkills.Y);
        if (slot >= 0)
        {
            Player.PlayerCastSkill(slot);
        }

        OnMouseMove();
    }
}