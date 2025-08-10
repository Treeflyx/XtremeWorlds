using System;
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
        _uiTimer.Elapsed += (_, _) => SafeUpdateEditors();
        _uiTimer.Start();

        app.Run();
    }

    private static void RunGame()
    {
        General.Client.Run();
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
            var ev = Editor_Event.Instance ?? new Editor_Event(); // Event editor is sealed; its constructor is public here.
            ev.Show();
            GameState.InitEventEditor = false;
        }

        if (GameState.InitAdminForm)
        {
            new Admin().Show();
            GameState.AdminPanel = true;
            GameState.InitAdminForm = false;
        }

        if (GameState.InitMapReport)
        {
            // TODO: Populate map report Eto form
            GameState.InitMapReport = false;
        }

        if (GameState.InitMapEditor)
        {
            var map = Editor_Map.Instance ?? new Editor_Map();
            GameState.MyEditorType = Core.EditorType.Map;
            GameState.EditorIndex = 1;
            map.Show();
            Editor_Map.MapEditorInit();
            GameState.InitMapEditor = false;
        }

        if (GameState.InitAnimationEditor)
        {
            var anim = Editor_Animation.Instance; // singleton Instance property handles creation
            GameState.MyEditorType = Core.EditorType.Animation;
            GameState.EditorIndex = 1;
            anim?.Show();
            if (anim != null)
            {
                anim.lstIndex.SelectedIndex = 0;
                Editors.AnimationEditorInit();
            }
            GameState.InitAnimationEditor = false;
        }

        if (GameState.InitItemEditor)
        {
            var item = Editor_Item.Instance; // private ctor, use Instance
            GameState.MyEditorType = Core.EditorType.Item;
            GameState.EditorIndex = 1;
            item?.Show();
            if (item?.lstIndex != null)
            {
                item.lstIndex.SelectedIndex = 0;
                Editors.ItemEditorInit();
            }
            GameState.InitItemEditor = false;
        }

        if (GameState.InitJobEditor)
        {
            var job = Editor_Job.Instance; // assume singleton pattern similar to others
            GameState.MyEditorType = Core.EditorType.Job;
            GameState.EditorIndex = 1;
            job?.Show();
            if (job?.lstIndex != null)
            {
                job.lstIndex.SelectedIndex = 0;
                Editors.JobEditorInit();
            }
            GameState.InitJobEditor = false;
        }

        if (GameState.InitMoralEditor)
        {
            var moral = Editor_Moral.Instance;
            GameState.MyEditorType = Core.EditorType.Moral;
            GameState.EditorIndex = 1;
            moral?.Show();
            if (moral?.lstIndex != null)
            {
                moral.lstIndex.SelectedIndex = 0;
                Editors.MoralEditorInit();
            }
            GameState.InitMoralEditor = false;
        }

        if (GameState.InitResourceEditor)
        {
            var res = Editor_Resource.Instance;
            GameState.MyEditorType = Core.EditorType.Resource;
            GameState.EditorIndex = 1;
            res?.Show();
            if (res?.lstIndex != null)
            {
                res.lstIndex.SelectedIndex = 0;
                Editors.ResourceEditorInit();
            }
            GameState.InitResourceEditor = false;
        }

        if (GameState.InitNpcEditor)
        {
            var npc = Editor_Npc.Instance ?? new Editor_Npc(); // public ctor we created
            GameState.MyEditorType = Core.EditorType.Npc;
            GameState.EditorIndex = 1;
            npc.Show();
            npc.lstIndex.SelectedIndex = 0;
            Editors.NpcEditorInit();
            GameState.InitNpcEditor = false;
        }

        if (GameState.InitSkillEditor)
        {
            var skill = Editor_Skill.Instance;
            GameState.MyEditorType = Core.EditorType.Skill;
            GameState.EditorIndex = 1;
            skill?.Show();
            if (skill?.lstIndex != null)
            {
                skill.lstIndex.SelectedIndex = 0;
                Editors.SkillEditorInit();
            }
            GameState.InitSkillEditor = false;
        }

        if (GameState.InitShopEditor)
        {
            var shop = Editor_Shop.Instance;
            GameState.MyEditorType = Core.EditorType.Shop;
            GameState.EditorIndex = 1;
            shop?.Show();
            if (shop?.lstIndex != null)
            {
                shop.lstIndex.SelectedIndex = 0;
                Editors.ShopEditorInit();
            }
            GameState.InitShopEditor = false;
        }

        if (GameState.InitProjectileEditor)
        {
            var proj = Editor_Projectile.Instance ?? new Editor_Projectile();
            GameState.MyEditorType = Core.EditorType.Projectile;
            GameState.EditorIndex = 1;
            proj.Show();
            proj.lstIndex.SelectedIndex = 0;
            Editors.ProjectileEditorInit();
            GameState.InitProjectileEditor = false;
        }

        if (GameState.InitScriptEditor)
        {
            var scr = Editor_Script.Instance; // private ctor singleton
            GameState.MyEditorType = Core.EditorType.Script;
            GameState.EditorIndex = 1;
            scr?.Show();
            if (scr != null) Script.ScriptEditorInit();
            GameState.InitScriptEditor = false;
        }

        // Invalidate redraw surfaces where needed (guard against null/partial migration)
        try
        {
            Editor_Map.Instance?.picBackSelect.Invalidate();
            Editor_Animation.Instance?.picSprite0.Invalidate();
            Editor_Animation.Instance?.picSprite1.Invalidate();
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