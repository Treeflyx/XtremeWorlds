using System;
using Client.Net;
using Core.Globals;
using Eto.Forms;

namespace Client;

public static class Program
{
    private static UITimer? _uiTimer;
    private static bool _editorsDisposed;

    [STAThread]
    public static void Main()
    {
        // Start game loop on background thread so Eto UI thread stays responsive
        var gameThread = new System.Threading.Thread(RunGame) { IsBackground = true };
        gameThread.Start();

        // Start Eto application & periodic UI updater
        // Explicitly specify Eto platform for Linux (Gtk) to avoid auto-detect failure
        // NOTE: Ensure package Eto.Platform.Gtk is referenced in the project (added centrally in Directory.Packages.props)
        var app = new Application(Eto.Platform.Detect);
        _uiTimer = new UITimer { Interval = 0.05 }; // 50ms (~20fps) for editor UI refresh logic
        _uiTimer.Elapsed += UiTimerOnElapsed;
        _uiTimer.Start();

        app.Run();
    }

    private static void RunGame()
    {
        General.Client.Run();
    }

    private static void UiTimerOnElapsed(object? sender, EventArgs e)
    {
        SafeUpdateEditors();
    }

    private static void SafeUpdateEditors()
    {
        try
        {
            UpdateEditors();
        }
        catch (ObjectDisposedException) { }
        catch (InvalidOperationException) { }
    }

    private static void UpdateEditors()
    {
        // Event Editor
        if (GameState.InitEventEditor)
        {
            new Editor_Event().Show();
            GameState.InitEventEditor = false;
        }

        if (GameState.InitAdminForm)
        {
            new Admin().Show();
            Sender.SendRequestMapReport();
            GameState.AdminPanel = true;
            GameState.InitAdminForm = false;
        }

        if (GameState.InitMapReport)
        {
            for (int i = 1, loopTo = GameState.MapNames.Length; i < loopTo; i++)
            {
                var admin = Admin.Instance;
                admin.lstMaps.Items.Add(new ListItem { Text = $"{i}: {GameState.MapNames[i]}" });
            }
                
            GameState.InitMapReport = false;
        }

        if (GameState.InitMapEditor)
        {
            GameState.MyEditorType = EditorType.Map;
            GameState.EditorIndex = 0;
            new Editor_Map().Show();
            Editor_Map.MapEditorInit();
            GameState.InitMapEditor = false;
        }

        if (GameState.InitAnimationEditor)
        {
            GameState.MyEditorType = EditorType.Animation;
            GameState.EditorIndex = 0;
            new Editor_Animation().Show();
            Editors.AnimationEditorInit();
            GameState.InitAnimationEditor = false;
        }

        if (GameState.InitItemEditor)
        {
            GameState.MyEditorType = EditorType.Item;
            GameState.EditorIndex = 0;
            new Editor_Item().Show();
            Editors.ItemEditorInit();
            GameState.InitItemEditor = false;
        }

        if (GameState.InitJobEditor)
        {
            GameState.MyEditorType = EditorType.Job;
            GameState.EditorIndex = 0;
            new Editor_Job().Show();
            Editors.JobEditorInit();
            GameState.InitJobEditor = false;
        }

        if (GameState.InitMoralEditor)
        {
            GameState.MyEditorType = EditorType.Moral;
            GameState.EditorIndex = 0;
            new Editor_Moral().Show();
            Editors.MoralEditorInit();
            GameState.InitMoralEditor = false;
        }

        if (GameState.InitResourceEditor)
        {
            GameState.MyEditorType = EditorType.Resource;
            GameState.EditorIndex = 0;
            new Editor_Resource().Show();
            Editors.ResourceEditorInit();
            GameState.InitResourceEditor = false;
        }

        if (GameState.InitNpcEditor)
        {
            GameState.MyEditorType = EditorType.Npc;
            GameState.EditorIndex = 0;
            new Editor_Npc().Show();
            Editors.NpcEditorInit();
            GameState.InitNpcEditor = false;
        }

        if (GameState.InitSkillEditor)
        {
            GameState.MyEditorType = EditorType.Skill;
            GameState.EditorIndex = 0;
            new Editor_Skill().Show();
            Editors.SkillEditorInit();
            GameState.InitSkillEditor = false;
        }

        if (GameState.InitShopEditor)
        {
            GameState.MyEditorType = EditorType.Shop;
            GameState.EditorIndex = 0;
            new Editor_Shop().Show();
            Editors.ShopEditorInit();
            GameState.InitShopEditor = false;
        }

        if (GameState.InitProjectileEditor)
        {
            GameState.MyEditorType = EditorType.Projectile;
            GameState.EditorIndex = 0;
            new Editor_Projectile().Show();
            Editors.ProjectileEditorInit();
            GameState.InitProjectileEditor = false;
        }

        if (GameState.InitScriptEditor)
        {
            GameState.MyEditorType = EditorType.Script;
            GameState.EditorIndex = 0;
            new Editor_Script().Show();
            GameState.InitScriptEditor = false;
        }

        // Invalidate redraw surfaces where needed (guard against null/partial migration)
        try
        {
            Editor_Map.Instance?.picBackSelect.Invalidate();
            Editor_Animation.Instance?.picSprite0?.Invalidate();
            Editor_Animation.Instance?.picSprite1?.Invalidate();
        }
        catch { /* some editors may not be instantiated yet */ }
        if (GameState.InGame)
        {
            // Reset disposal flag when (re)entering game
            _editorsDisposed = false;
        }
        else if (!_editorsDisposed)
        {
            // Dispose editors once when leaving game state
            try { Editor_Item.Instance?.Dispose(); } catch { }
            try { Editor_Job.Instance?.Dispose(); } catch { }
            try { Editor_Map.Instance?.Dispose(); } catch { }
            try { Editor_Event.Instance?.Dispose(); } catch { }
            try { Editor_Npc.Instance?.Dispose(); } catch { }
            try { Editor_Projectile.Instance?.Dispose(); } catch { }
            try { Editor_Resource.Instance?.Dispose(); } catch { }
            try { Editor_Shop.Instance?.Dispose(); } catch { }
            try { Editor_Skill.Instance?.Dispose(); } catch { }
            try { Editor_Animation.Instance?.Dispose(); } catch { }
            try { Editor_Moral.Instance?.Dispose(); } catch { }
            try { Editor_Script.Instance?.Dispose(); } catch { }
            // TODO: track Admin form instance and close if open
            _editorsDisposed = true;
        }
    }
}