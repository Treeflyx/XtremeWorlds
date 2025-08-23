using System;
using System.Linq;
using System.Net.Http.Headers;
using Client.Net;
using Core.Globals;
using Eto.Forms;
using Eto.Drawing;

namespace Client;

public static class Program
{
    private static UITimer? _uiTimer;
    private static bool _editorsDisposed;
    private static Form? _rootForm; // hidden form to keep Eto alive

    [STAThread]
    public static void Main()
    {
        // Start game loop on background thread so Eto UI thread stays responsive
    var gameThread = new System.Threading.Thread(RunGame) { IsBackground = false };
        gameThread.Start();

        // Start Eto application & periodic UI updater
        // Explicitly specify Eto platform for Linux (Gtk) to avoid auto-detect failure
        // NOTE: Ensure package Eto.Platform.Gtk is referenced in the project (added centrally in Directory.Packages.props)
    var app = new Application(Eto.Platform.Detect);
        _uiTimer = new UITimer { Interval = 0.05 }; // 50ms (~20fps) for editor UI refresh logic
        _uiTimer.Elapsed += UiTimerOnElapsed;
        _uiTimer.Start();

        // Keep Eto running even if all editor windows are closed by using a hidden root form
        _rootForm = new Form
        {
            Title = string.Empty,
            ShowInTaskbar = false,
            ClientSize = new Size(1, 1),
        };
    _rootForm.Shown += (s, e) => ((Form)s!).Visible = false;

        app.Run(_rootForm);
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
        catch (Exception ex)
        {
            // Prevent UI timer from dying due to unexpected exceptions
            try { System.Console.WriteLine($"[UiTimer] Exception: {ex}"); } catch { }
        }
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
            GameState.CameraZoom = 1.0f;
            GameState.InitMapEditor = false;
        }

        if (GameState.InitAnimationEditor)
        {
            GameState.MyEditorType = EditorType.Animation;
            GameState.EditorIndex = 0;
            new Editor_Animation().Show();
            GameState.InitAnimationEditor = false;
        }

        if (GameState.InitItemEditor)
        {
            GameState.MyEditorType = EditorType.Item;
            GameState.EditorIndex = 0;
            new Editor_Item().Show();
            GameState.InitItemEditor = false;
        }

        if (GameState.InitJobEditor)
        {
            GameState.MyEditorType = EditorType.Job;
            GameState.EditorIndex = 0;
            new Editor_Job().Show();
            GameState.InitJobEditor = false;
        }

        if (GameState.InitMoralEditor)
        {
            GameState.MyEditorType = EditorType.Moral;
            GameState.EditorIndex = 0;
            new Editor_Moral().Show();
            GameState.InitMoralEditor = false;
        }

        if (GameState.InitResourceEditor)
        {
            GameState.MyEditorType = EditorType.Resource;
            GameState.EditorIndex = 0;
            new Editor_Resource().Show();
            GameState.InitResourceEditor = false;
        }

        if (GameState.InitNpcEditor)
        {
            GameState.MyEditorType = EditorType.Npc;
            GameState.EditorIndex = 0;
            new Editor_Npc().Show();
            GameState.InitNpcEditor = false;
        }

        if (GameState.InitSkillEditor)
        {
            GameState.MyEditorType = EditorType.Skill;
            GameState.EditorIndex = 0;
            new Editor_Skill().Show();
            GameState.InitSkillEditor = false;
        }

        if (GameState.InitShopEditor)
        {
            GameState.MyEditorType = EditorType.Shop;
            GameState.EditorIndex = 0;
            new Editor_Shop().Show();
            GameState.InitShopEditor = false;
        }

        if (GameState.InitProjectileEditor)
        {
            GameState.MyEditorType = EditorType.Projectile;
            GameState.EditorIndex = 0;
            new Editor_Projectile().Show();
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
            if (GameState.MyEditorType == EditorType.Map)
                Editor_Map.Instance?.picBackSelect.Invalidate();

            if (GameState.MyEditorType == EditorType.Animation)
            {
                Editor_Animation.Instance?.picSprite0?.Invalidate();
                Editor_Animation.Instance?.picSprite1?.Invalidate();
            }
        }
        catch { /* some editors may not be instantiated yet */ }
        if (!GameState.InGame)
        {
            // Reset disposal flag when (re)entering non-game state
            _editorsDisposed = false;
        }
        else if (!_editorsDisposed)
        {
            // Dispose editors once when entering game state
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

    // Called when the game is exiting to stop the Eto loop cleanly
    public static void QuitEto()
    {
        try
        {
            // Stop the UI timer to avoid further callbacks during shutdown
            try { _uiTimer?.Stop(); } catch { }

            // Close all open Eto windows on the UI thread, then close the hidden root form
            Application.Instance?.AsyncInvoke(() =>
            {
                try
                {
                    // Close all windows except the hidden root, if present
                    foreach (var win in Application.Instance.Windows.ToList())
                    {
                        try
                        {
                            if (!ReferenceEquals(win, _rootForm) && win.Visible)
                                win.Close();
                        }
                        catch { }
                    }

                    // Finally close the root form to end Application.Run
                    _rootForm?.Close();
                }
                catch
                {
                    // As a fallback, request application quit
                    try { Application.Instance.Quit(); } catch { }
                }
            });
        }
        catch { }
    }
}