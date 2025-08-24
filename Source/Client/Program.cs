using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Client.Game.UI;
using Client.Game.UI.Windows;
using Client.Net;
using Core.Configurations;
using Core.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static Core.Globals.Command;
using Type = Core.Globals.Type;

namespace Client
{
    public class GameClient : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager? Graphics;
        public static SpriteBatch? SpriteBatch;

        public static readonly ConcurrentDictionary<string, Texture2D> TextureCache = new();
        public static readonly ConcurrentDictionary<string, GfxInfo> GfxInfoCache = new();

        private static int _gameFps;
        private static readonly object FpsLock = new();

        // Safely set FPS with a lock
        public static void SetFps(int newFps)
        {
            lock (FpsLock)
                _gameFps = newFps;
        }

        // Safely get FPS with a lock
        public static int GetFps()
        {
            lock (FpsLock)
                return _gameFps;
        }

        // State tracking variables
        // Shared keyboard and mouse states for cross-thread access
        public static KeyboardState CurrentKeyboardState;
        public static KeyboardState PreviousKeyboardState;
        public static MouseState CurrentMouseState;
        public static MouseState PreviousMouseState;

        // Keep track of the key states to avoid repeated input
        public static readonly Dictionary<Keys, bool> KeyStates = new();

        // Define a dictionary to store the last time a key was processed
        public static Dictionary<Keys, DateTime> KeyRepeatTimers = new();

        // Minimum interval (in milliseconds) between repeated key inputs
        private const byte KeyRepeatInterval = 200;

        // Lock object to ensure thread safety
        public static readonly object InputLock = new();

        // Track the previous scroll value to compute delta
        private static readonly object ScrollLock = new();
        private static int _prevScrollWheelValue = 0;

        private TimeSpan _elapsedTime = TimeSpan.Zero;

        public static RenderTarget2D? RenderTarget;
        public static Texture2D? TransparentTexture;
        public static Texture2D? PixelTexture;

        // Add a timer to prevent spam
        private static DateTime _lastInputTime = DateTime.MinValue;
        private const int InputCooldown = 250;

        // Handle Escape key to toggle menus
        private static DateTime _lastMouseClickTime = DateTime.MinValue;
        private const int MouseClickCooldown = 250;
        private static DateTime _lastSearchTime = DateTime.MinValue;

        // Ensure this class exists to store graphic info
        public class GfxInfo
        {
            public int Width;
            public int Height;
        }

        public static GfxInfo GetGfxInfo(string key)
        {
            // Check if the key does not end with ".gfxext" and append if needed
            if (!key.EndsWith(GameState.GfxExt, StringComparison.OrdinalIgnoreCase))
            {
                key += GameState.GfxExt;
            }

            // Ensure the texture is loaded so GfxInfoCache gets populated
            var texture = GetTexture(key) ?? LoadTexture(key);

            if (!GfxInfoCache.TryGetValue(key, out var result) || result is null)
            {
                // If still not available, return a harmless placeholder size (1x1)
                Debug.WriteLine($"Warning: GfxInfo for key '{key}' not found; using placeholder 1x1.");
                return new GfxInfo { Width = 1, Height = 1 };
            }

            return result;
        }
        
        public GameClient()
        {
            (GameState.ResolutionWidth, GameState.ResolutionHeight) = General.GetResolutionSize(SettingsManager.Instance.Resolution);

            Graphics = new GraphicsDeviceManager(this);

            // Set basic properties for GraphicsDeviceManager
            ref var withBlock = ref Graphics;
            withBlock.GraphicsProfile = GraphicsProfile.Reach;
            withBlock.IsFullScreen = SettingsManager.Instance.Fullscreen;
            withBlock.PreferredBackBufferWidth = GameState.ResolutionWidth;
            withBlock.PreferredBackBufferHeight = GameState.ResolutionHeight;
            withBlock.SynchronizeWithVerticalRetrace = SettingsManager.Instance.Vsync;
            IsFixedTimeStep = false;
            withBlock.PreferHalfPixelOffset = true;
            withBlock.PreferMultiSampling = false;

            // Allow resizing and keep backbuffer in sync with window size when windowed
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnClientSizeChanged;

            // Add handler for PreparingDeviceSettings
            Graphics.PreparingDeviceSettings += (sender, args) =>
            {
                args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
                args.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 8;
            };

#if DEBUG
            IsMouseVisible = true;
#endif
            Content.RootDirectory = "Content";

            // Handle Exiting without forcing a hard shutdown
            Exiting += (s, e) => {
                // Let General perform graceful shutdown tasks
                General.DestroyGame();
                // Also end the Eto UI loop cleanly
                try { Client.Program.QuitEto(); } catch { }
            };
        }

        private void OnClientSizeChanged(object? sender, EventArgs e)
        {
            // In windowed mode, track the window client size as the backbuffer size
            if (Graphics is null || Graphics.IsFullScreen)
                return;

            var bounds = Window.ClientBounds;

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            if (Graphics.PreferredBackBufferWidth != bounds.Width || Graphics.PreferredBackBufferHeight != bounds.Height)
            {
                Graphics.PreferredBackBufferWidth = bounds.Width;
                Graphics.PreferredBackBufferHeight = bounds.Height;
                try { Graphics.ApplyChanges(); } catch { }
            }
        }

        protected override void Initialize()
        {
            Window.Title = SettingsManager.Instance.GameName;
            
            // Create the RenderTarget2D with the same size as the screen
            RenderTarget = new RenderTarget2D(Graphics?.GraphicsDevice,
                Graphics?.GraphicsDevice.PresentationParameters.BackBufferWidth ?? 0,
                Graphics?.GraphicsDevice.PresentationParameters.BackBufferHeight ?? 0, false,
                Graphics?.GraphicsDevice.PresentationParameters.BackBufferFormat ?? SurfaceFormat.Color, DepthFormat.Depth24);

            // Apply changes to GraphicsDeviceManager
            try
            {
                Graphics.ApplyChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GraphicsDevice initialization failed: {ex.Message}");
                throw;
            }

            base.Initialize();
        }

        protected override void BeginRun()
        {
            base.BeginRun();
            // Start TCP client after the window has loaded
            try { _ = Network.Start(); }
            catch (Exception ex) { Debug.WriteLine($"Network start error: {ex.Message}"); }
        }

        static void LoadFonts()
        {
            // Get all defined font enum values except None (assumed to be 0)
            var fontValues = Enum.GetValues(typeof(Font));
            for (int i = 1; i < fontValues.Length; i++)
            {
                var val = fontValues.GetValue(i);
                if (val is not Font f)
                {
                    continue;
                }

                try
                {
                    TextRenderer.Fonts[f] = LoadFont(DataPath.Fonts, f);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to load font {f}: {ex.Message}");
                }
            }
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            TransparentTexture = new Texture2D(GraphicsDevice, 1, 1);
            TransparentTexture.SetData([Color.White]);
            PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            PixelTexture.SetData([Color.White]);

            LoadFonts();

            // Kick off heavy startup work on a background thread to avoid freezing the main thread
            GameState.IsLoading = true;
            _ = Task.Run(() =>
            {
                try
                {
                    General.Startup();
                }
                finally
                {
                    GameState.IsLoading = false;
                }
            });

            try
            {
                var cursorPath = Path.Combine(DataPath.Misc, "Cursor.png");
                if (!File.Exists(cursorPath))
                {
                    // Fallback to Content relative path if the asset base is different
                    var fallback = Path.Combine(Content.RootDirectory, "Graphics", "Misc", "Cursor.png");
                    cursorPath = File.Exists(fallback) ? fallback : cursorPath;
                }

                if (File.Exists(cursorPath))
                {
                    using var fs = new FileStream(cursorPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using var tempTex = Texture2D.FromStream(Graphics?.GraphicsDevice, fs);
                    Mouse.SetCursor(MouseCursor.FromTexture2D(tempTex, 0, 0));
                }
                else
                {
                    Mouse.SetCursor(MouseCursor.Arrow);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Cursor load failed: {ex.Message}");
                Mouse.SetCursor(MouseCursor.Arrow);
            }
        }

        public static SpriteFont LoadFont(string path, Font font)
        {
            return General.Client.Content.Load<SpriteFont>(Path.Combine(path, ((int) font).ToString()));
        }

        public static Color ToXnaColor(System.Drawing.Color drawingColor)
        {
            return new Color(drawingColor.R, drawingColor.G, drawingColor.B, drawingColor.A);
        }

        public static System.Drawing.Color ToDrawingColor(Color xnaColor)
        {
            return System.Drawing.Color.FromArgb(xnaColor.A, xnaColor.R, xnaColor.G, xnaColor.B);
        }

        public static void RenderTexture(ref string path, int dX, int dY, int sX, int sY, int dW, int dH, int sW = 1,
            int sH = 1, float alpha = 1.0f, byte red = 255, byte green = 255, byte blue = 255)
        {
            path = DataPath.EnsureFileExtension(path);

            // Retrieve the texture
            var texture = GetTexture(path);

            if (texture is null)
            {
                return;
            }

            // Draw directly in native render-target coordinates. Global composition handles scaling
            // to the backbuffer with pillarbox/letterbox as needed. Avoid using backbuffer sizes here
            // to prevent double-scaling during window resizes.
            var destRect = new Rectangle(dX, dY, dW, dH);
            var srcRect = new Rectangle(sX, sY, sW, sH);
            var color = new Color(red, green, blue, (byte) 255) * alpha;

            SpriteBatch?.Draw(texture, destRect, srcRect, color);
        }

        public static Texture2D GetTexture(string path)
        {
            if (!TextureCache.ContainsKey(path))
            {
                var texture = LoadTexture(path);
                return texture;
            }

            return TextureCache[path];
        }

        public static Texture2D LoadTexture(string path)
        {
            try
            {
                // Check if the key does not end with ".gfxext" and append if needed  
                if (!path.EndsWith(GameState.GfxExt, StringComparison.OrdinalIgnoreCase))
                {
                    path += GameState.GfxExt;
                }

                // Open the file stream with FileShare.Read to allow other processes to read the file  
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var texture = Texture2D.FromStream(Graphics.GraphicsDevice, stream);

                    // Cache graphics information  
                    var gfxInfo = new GfxInfo()
                    {
                        Width = texture.Width,
                        Height = texture.Height
                    };
                    GfxInfoCache.TryAdd(path, gfxInfo);

                    TextureCache[path] = texture;

                    return texture;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading texture from {path}: {ex.Message}");
                return new Texture2D(Graphics?.GraphicsDevice, 1, 1);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            Graphics.GraphicsDevice.Clear(Color.Black);

            // Update GUI mouse position before drawing GUI (ensures correct UI hover/click)
            var mousePosGame = GetMousePosition("game");
            GameState.CurMouseXGame = mousePosGame.Item1;
            GameState.CurMouseYGame = mousePosGame.Item2;

            // Choose native resolution: window/backbuffer in windowed; selected resolution in fullscreen
            var ppNow = GraphicsDevice.PresentationParameters;
            int bbWNow = ppNow.BackBufferWidth;
            int bbHNow = ppNow.BackBufferHeight;
            bool isFullscreenNow = Graphics?.IsFullScreen ?? false;
            int nativeWidth, nativeHeight;
            if (isFullscreenNow)
            {
                var sel = General.GetResolutionSize(SettingsManager.Instance.Resolution);
                // Clamp to 16:9 aspect to avoid ultrawide stretching for now
                int w = sel.Item1;
                int h = sel.Item2;
                float aspect = 16f / 9f;
                // Recompute height from width to enforce 16:9
                h = (int)Math.Round(w / aspect);
                nativeWidth = Math.Max(1, w);
                nativeHeight = Math.Max(1, h);
            }
            else
            {
                // Windowed: use the current backbuffer/window size as native
                // This restores previous camera behavior (more view with larger window)
                nativeWidth = Math.Max(1, bbWNow);
                nativeHeight = Math.Max(1, bbHNow);
            }
            // Update effective native size and trigger GUI rebuild if changed
            bool guiSizeChanged = GameState.ResolutionWidth != nativeWidth || GameState.ResolutionHeight != nativeHeight;
            GameState.ResolutionWidth = nativeWidth;
            GameState.ResolutionHeight = nativeHeight;

            // Only recreate if needed
            if (RenderTarget == null || RenderTarget.Width != nativeWidth || RenderTarget.Height != nativeHeight)
            {
                if (RenderTarget != null)
                    RenderTarget.Dispose();
                RenderTarget = new RenderTarget2D(GraphicsDevice, nativeWidth, nativeHeight, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            }

            // --- Render game/menu to RenderTarget (zoomed) ---
            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(Color.Black);

            if (GameState.IsLoading || GameState.GettingMap)
            {
                // Draw loading screen onto the RenderTarget
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                var loadingText = "Loading...";
                if (TextRenderer.Fonts.TryGetValue(Font.Georgia, out var font))
                {
                    var size = font.MeasureString(loadingText);
                    var x = (nativeWidth - size.X) / 2f;
                    var y = (nativeHeight - size.Y) / 2f;
                    SpriteBatch.DrawString(font, loadingText, new Vector2(x, y), Color.White);
                }
                SpriteBatch.End();
            }
            else if (GameState.InGame == true)
            {
                // Draw the actual game onto the RenderTarget
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                Render_Game();
                SpriteBatch.End();
            }

            // After drawing to RenderTarget, reset to back buffer for composition
            GraphicsDevice.SetRenderTarget(null);

            if (!GameState.GettingMap)
            {
                // --- Render GUI to guiRenderTarget (not zoomed) ---
                if (_guiRenderTarget == null || _guiRenderTarget.Width != nativeWidth || _guiRenderTarget.Height != nativeHeight)
                {
                    _guiRenderTarget?.Dispose();
                    _guiRenderTarget = new RenderTarget2D(GraphicsDevice, nativeWidth, nativeHeight, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
                }

                // Update GUI mouse position before drawing GUI (ensures correct UI hover/click)
                // This uses only GUI scale, not game zoom, so GUI input is always correct regardless of zoom
                var mousePosGui = GetMousePosition("gui");
                GameState.CurMouseXGui = mousePosGui.Item1;
                GameState.CurMouseYGui = mousePosGui.Item2;

                GraphicsDevice.SetRenderTarget(_guiRenderTarget);
                GraphicsDevice.Clear(Color.Transparent);
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                if (GameState.InMenu)
                    Gui.DrawMenuBackground();
                Gui.Render();
                TextRenderer.DrawMapName();
                SpriteBatch.End();

                // After drawing to _guiRenderTarget, reset to back buffer
                GraphicsDevice.SetRenderTarget(null);

                int backBufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
                int backBufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
                // In fullscreen, force pillarbox (black bars left/right) at 16:9; in windowed, fill 1:1.
                Rectangle destRect;
                bool isFullscreenNow2 = Graphics?.IsFullScreen ?? false;
                if (isFullscreenNow2)
                {
                    // Force 16:9 pillarbox regardless of screen aspect
                    float targetAspect = 16f / 9f;
                    int height = backBufferHeight;
                    int width = (int)(height * targetAspect);
                    int x = (backBufferWidth - width) / 2;
                    destRect = new Rectangle(x, 0, width, height);
                }
                else
                {
                    // Windowed: scale to fit window with letterbox/pillarbox based on native aspect (often equals window, so no bars)
                    float targetAspect = nativeHeight == 0 ? 16f / 9f : (float)nativeWidth / nativeHeight;
                    float screenAspect = backBufferHeight == 0 ? targetAspect : (float)backBufferWidth / backBufferHeight;
                    if (screenAspect > targetAspect)
                    {
                        // Pillarbox: full height
                        int height = backBufferHeight;
                        int width = (int)(height * targetAspect);
                        int x = (backBufferWidth - width) / 2;
                        destRect = new Rectangle(x, 0, width, height);
                    }
                    else
                    {
                        // Letterbox: full width
                        int width = backBufferWidth;
                        int height = (int)(width / targetAspect);
                        int y = (backBufferHeight - height) / 2;
                        destRect = new Rectangle(0, y, width, height);
                    }
                }

                using (var targetBatch = new SpriteBatch(GraphicsDevice))
                {
                    targetBatch.Begin(samplerState: SamplerState.PointClamp);
                    // Draw the game/menu to the backbuffer (letterboxed in fullscreen, 1:1 in windowed)
                    if (RenderTarget != null)
                        targetBatch.Draw(RenderTarget, destRect, Color.White);

                    // Draw GUI to match the same destination rectangle
                    if (_guiRenderTarget != null)
                        // In fullscreen, force pillarbox (black bars left/right) at 16:9; in windowed, fill 1:1.
                        Rectangle destRect;
                        bool isFullscreenNow2 = Graphics?.IsFullScreen ?? false;
                        if (isFullscreenNow2)
                        {
                            // Force 16:9 pillarbox regardless of screen aspect
                            float targetAspect = 16f / 9f;
                            int height = backBufferHeight;
                            int width = (int)(height * targetAspect);
                            int x = (backBufferWidth - width) / 2;
                            destRect = new Rectangle(x, 0, width, height);
                        }
                        else
                        {
                            // Windowed: scale to fit window with letterbox/pillarbox based on native aspect (often equals window, so no bars)
                            float targetAspect = nativeHeight == 0 ? 16f / 9f : (float)nativeWidth / nativeHeight;
                            float screenAspect = backBufferHeight == 0 ? targetAspect : (float)backBufferWidth / backBufferHeight;
                            if (screenAspect > targetAspect)
                            {
                                // Pillarbox: full height
                                int height = backBufferHeight;
                                int width = (int)(height * targetAspect);
                                int x = (backBufferWidth - width) / 2;
                                destRect = new Rectangle(x, 0, width, height);
                            }
                            else
                            {
                                // Letterbox: full width
                                int width = backBufferWidth;
                                int height = (int)(width / targetAspect);
                                int y = (backBufferHeight - height) / 2;
                                destRect = new Rectangle(0, y, width, height);
                            }
                        }
            }

            if (GameState.MyEditorType == EditorType.Map)
            {
                if (IsKeyStateActive(Keys.Z))
                {
                    Editor_Map.MapEditorUndo();
                }

                if (IsKeyStateActive(Keys.Y))
                {
                    Editor_Map.MapEditorRedo();
                }
            }

            // Camera zoom with mouse wheel (range 0.5 to 4.0)
            int currentWheel = Mouse.GetState().ScrollWheelValue;
            if (currentWheel != _prevScrollWheelValue)
            {
                int delta = currentWheel - _prevScrollWheelValue;
                if (delta != 0 && GameState.MyEditorType != EditorType.Map)
                {
                    float zoomDelta = delta > 0 ? 0.1f : -0.1f;
                    GameState.CameraZoom += zoomDelta;
                    GameState.CameraZoom = Math.Clamp(GameState.CameraZoom, 0.5f, 2.0f);
                }
                _prevScrollWheelValue = currentWheel;
            }

            if (IsKeyStateActive(Keys.F12))
            {
                TakeScreenshot();
            }

            SetFps(_gameFps + 1);
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime.TotalSeconds >= 1d)
            {
                SetFps(0);
                
                _elapsedTime = TimeSpan.Zero;
            }

            Loop.Game();

            base.Update(gameTime);
        }

        // Reset keyboard and mouse states
        private static void ResetInputStates()
        {
            CurrentKeyboardState = new KeyboardState();
            PreviousKeyboardState = new KeyboardState();
            CurrentMouseState = new MouseState();
            PreviousMouseState = new MouseState();
        }

        private static void UpdateKeyCache()
        {
            // Get the current keyboard state
            var keyboardState = Keyboard.GetState();

            // Update the previous and current states
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = keyboardState;
        }

        private static void UpdateMouseCache()
        {
            // Get the current mouse state
            var mouseState = Mouse.GetState();

            // Update the previous and current states
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = mouseState;
        }

        public static int GetMouseScrollDelta()
        {
            lock (ScrollLock)
                // Calculate the scroll delta between the previous and current states
                return CurrentMouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue;
        }

        public static bool IsKeyStateActive(Keys key)
        {
            if (CanProcessKey(key) == true)
            {
                // Check if the key is down in the current keyboard state
                return CurrentKeyboardState.IsKeyDown(key);
            }

            return default;
        }

        public static Tuple<int, int> GetMousePosition(string mode = "gui")
        {
            int mouseX = CurrentMouseState.X;
            int mouseY = CurrentMouseState.Y;

            bool isFullscreenNow = Graphics?.IsFullScreen ?? false;
            int backBufferWidth = Graphics?.GraphicsDevice.PresentationParameters.BackBufferWidth ?? 0;
            int backBufferHeight = Graphics?.GraphicsDevice.PresentationParameters.BackBufferHeight ?? 0;
            if (!isFullscreenNow)
            {
                // Windowed: scale to fit; invert based on selected/native aspect
                // In windowed mode native matches backbuffer now
                int nW = GameState.ResolutionWidth;
                int nH = GameState.ResolutionHeight;
                if (nW <= 0 || nH <= 0 || backBufferWidth <= 0 || backBufferHeight <= 0)
                    return new Tuple<int, int>(mouseX, mouseY);
                float wTargetAspect = (float)nW / nH;
                float wScreenAspect = (float)backBufferWidth / backBufferHeight;
                int wViewX, wViewY, wViewW, wViewH;
                if (wScreenAspect > wTargetAspect)
                {
                    wViewH = backBufferHeight;
                    wViewW = (int)(wViewH * wTargetAspect);
                    wViewX = (backBufferWidth - wViewW) / 2;
                    wViewY = 0;
                }
                else
                {
                    wViewW = backBufferWidth;
                    wViewH = (int)(wViewW / wTargetAspect);
                    wViewX = 0;
                    wViewY = (backBufferHeight - wViewH) / 2;
                }
                if (mouseX < wViewX || mouseY < wViewY || mouseX >= wViewX + wViewW || mouseY >= wViewY + wViewH)
                    return new Tuple<int, int>(-1, -1);
                float wSx = (float)(mouseX - wViewX) / wViewW;
                float wSy = (float)(mouseY - wViewY) / wViewH;
                int wMappedX = (int)(wSx * nW);
                int wMappedY = (int)(wSy * nH);
                return new Tuple<int, int>(wMappedX, wMappedY);
            }

            // Fullscreen: apply pillarbox mapping using 16:9 target aspect
            // Fullscreen mapping uses forced 16:9 pillarbox
            var (tW, tH) = General.GetResolutionSize(SettingsManager.Instance.Resolution);
            if (tW <= 0 || tH <= 0 || backBufferWidth <= 0 || backBufferHeight <= 0)
                return new Tuple<int, int>(mouseX, mouseY);

            // Force 16:9 pillarbox: view spans full height
            float targetAspect = 16f / 9f;
            int viewH = backBufferHeight;
            int viewW = (int)(viewH * targetAspect);
            int viewX = (backBufferWidth - viewW) / 2;
            int viewY = 0;

            if (mouseX < viewX || mouseY < viewY || mouseX >= viewX + viewW || mouseY >= viewY + viewH)
                return new Tuple<int, int>(-1, -1);

            // Map to target resolution space
            float sx = (float)(mouseX - viewX) / viewW;
            float sy = (float)(mouseY - viewY) / viewH;
            int mappedX = (int)(sx * tW);
            int mappedY = (int)(sy * tH);
            return new Tuple<int, int>(mappedX, mappedY);
        }

        // Compute the native-space pivot for zooming: target center if valid, else player center
        private static Vector2 GetZoomPivotNative()
        {
            int worldX = GetPlayerRawX(GameState.MyIndex) + GameState.SizeX / 2;
            int worldY = GetPlayerRawY(GameState.MyIndex) + GameState.SizeY / 2;

            if (GameState.MyTarget >= 0)
            {
                if (GameState.MyTargetType == (int)TargetType.Player)
                {
                    int t = GameState.MyTarget;
                    if (IsPlaying(t))
                    {
                        // Same map check
                        if (Data.Player[t].Map == Data.Player[GameState.MyIndex].Map)
                        {
                            worldX = GetPlayerRawX(t) + GameState.SizeX / 2;
                            worldY = GetPlayerRawY(t) + GameState.SizeY / 2;
                        }
                    }
                }
                else if (GameState.MyTargetType == (int)TargetType.Npc)
                {
                    int n = GameState.MyTarget;
                    if (n >= 0 && n < Data.MyMapNpc.Length && Data.MyMapNpc[n].Num >= 0)
                    {
                        worldX = Data.MyMapNpc[n].X + GameState.SizeX / 2;
                        worldY = Data.MyMapNpc[n].Y + GameState.SizeY / 2;
                    }
                }
            }

            int px = GameLogic.ConvertMapX(worldX);
            int py = GameLogic.ConvertMapY(worldY);
            return new Vector2(px, py);
        }

        public static bool IsMouseButtonDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                {
                    return CurrentMouseState.LeftButton == ButtonState.Pressed;
                }
                case MouseButton.Right:
                {
                    return CurrentMouseState.RightButton == ButtonState.Pressed;
                }
                case MouseButton.Middle:
                {
                    return CurrentMouseState.MiddleButton == ButtonState.Pressed;
                }

                default:
                {
                    return false;
                }
            }
        }

        public static bool IsMouseButtonUp(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                {
                    return CurrentMouseState.LeftButton == ButtonState.Released;
                }
                case MouseButton.Right:
                {
                    return CurrentMouseState.RightButton == ButtonState.Released;
                }
                case MouseButton.Middle:
                {
                    return CurrentMouseState.MiddleButton == ButtonState.Released;
                }

                default:
                {
                    return false;
                }
            }
        }

        public static void ProcessInputs()
        {
            // Get the mouse position from the cache

            // --- Update both game and GUI mouse/world positions globally ---
            // Game context
            var mousePosGame = GetMousePosition("game");
            int mouseXGame = mousePosGame.Item1;
            int mouseYGame = mousePosGame.Item2;
            GameState.CurMouseXGame = mouseXGame;
            GameState.CurMouseYGame = mouseYGame;
            if (mouseXGame >= 0 && mouseYGame >= 0)
            {
                // Absolute world tile under the mouse: floor((cameraOffsetPx + mousePx) / tileSize)
                GameState.CurXGame = (int)Math.Floor((GameState.Camera.Left + mouseXGame) / (double)GameState.SizeX);
                GameState.CurYGame = (int)Math.Floor((GameState.Camera.Top + mouseYGame) / (double)GameState.SizeY);
            }
            else
            {
                GameState.CurXGame = -1;
                GameState.CurYGame = -1;
            }

            // GUI context
            var mousePosGui = GetMousePosition("gui");
            int mouseXGui = mousePosGui.Item1;
            int mouseYGui = mousePosGui.Item2;
            GameState.CurMouseXGui = mouseXGui;
            GameState.CurMouseYGui = mouseYGui;
            if (mouseXGui >= 0 && mouseYGui >= 0)
            {
                // GUI maps to native space; still convert to absolute world tile using camera offsets
                GameState.CurXGui = (int)Math.Floor((GameState.Camera.Left + mouseXGui) / (double)GameState.SizeX);
                GameState.CurYGui = (int)Math.Floor((GameState.Camera.Top + mouseYGui) / (double)GameState.SizeY);
            }
            else
            {
                GameState.CurXGui = -1;
                GameState.CurYGui = -1;
            }

            // For compatibility, set legacy variables to GAME context by default (for targeting, etc)
            GameState.CurX = GameState.CurXGame;
            GameState.CurY = GameState.CurYGame;
            GameState.CurMouseX = GameState.CurMouseXGame;
            GameState.CurMouseY = GameState.CurMouseYGame;

            // Check for action keys
            GameState.VbKeyControl = CurrentKeyboardState.IsKeyDown(Keys.LeftControl);
            GameState.VbKeyShift = CurrentKeyboardState.IsKeyDown(Keys.LeftShift);

            if (IsKeyStateActive(Keys.F8))
            {
                var uiPath = Path.Combine(DataPath.Skins, SettingsManager.Instance.Skin + ".cs");

                if (!File.Exists(uiPath))
                {
                    Console.WriteLine($"File not found: {uiPath}");
                }
                else
                {
                    // Open with default text editor
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = uiPath,
                        UseShellExecute = true
                    });
                }
            }

            if (IsKeyStateActive(Keys.F5))
            {
                UIScript.Load();
                Gui.Init();
            }

            // Handle Escape key to toggle menus
            if (IsKeyStateActive(Keys.Escape))
            {
                // First: clear target with server if one is selected
                int prevTarget = GameState.MyTarget;
                int prevTargetType = GameState.MyTargetType;
                if (prevTarget >= 0 && prevTargetType >= 0)
                {
                    int? clearTileX = null;
                    int? clearTileY = null;
                    if (prevTargetType == (int)TargetType.Player)
                    {
                        if (IsPlaying(prevTarget) && GetPlayerMap(prevTarget) == GetPlayerMap(GameState.MyIndex))
                        {
                            clearTileX = GetPlayerX(prevTarget);
                            clearTileY = GetPlayerY(prevTarget);
                        }
                    }
                    else if (prevTargetType == (int)TargetType.Npc)
                    {
                        if (prevTarget >= 0 && prevTarget < Data.MyMapNpc.Length && Data.MyMapNpc[prevTarget].Num >= 0)
                        {
                            clearTileX = (int)Math.Floor(Data.MyMapNpc[prevTarget].X / 32d);
                            clearTileY = (int)Math.Floor(Data.MyMapNpc[prevTarget].Y / 32d);
                        }
                    }

                    // Clear locally first per requirement
                    GameState.MyTarget = -1;
                    GameState.MyTargetType = 0;

                    // Notify server to toggle/clear the same tile
                    if (clearTileX.HasValue && clearTileY.HasValue)
                    {
                        Sender.PlayerSearch(clearTileX.Value, clearTileY.Value, 0);
                    }

                    // If we just cleared a target, stop here (don’t open/close menus this press)
                    return;
                }

                if (GameState.InMenu == true)
                    return;

                // Hide options screen
                if (IsWindowVisible("winOptions"))
                {
                    Gui.HideWindow("winOptions");
                    WinComboMenu.Close();
                    return;
                }

                // hide/show chat window
                if (IsWindowVisible("winChat"))
                {
                    if (Gui.TryGetControl("winChat", "txtChat", out var chatCtrl))
                    {
                        chatCtrl!.Text = "";
                    }
                    WinChat.Hide();
                    return;
                }

                if (IsWindowVisible("winEscMenu"))
                {
                    Gui.HideWindow("winEscMenu");
                    return;
                }

                if (IsWindowVisible("winShop"))
                {
                    Shop.CloseShop();
                    return;
                }

                if (IsWindowVisible("winBank"))
                {
                    Bank.CloseBank();
                    return;
                }

                if (IsWindowVisible("winTrade"))
                {
                    Trade.SendDeclineTrade();
                    return;
                }

                if (IsWindowVisible("winInventory"))
                {
                    Gui.HideWindow("winInventory");
                    return;
                }

                if (IsWindowVisible("winCharacter"))
                {
                    Gui.HideWindow("winCharacter");
                    return;
                }

                if (IsWindowVisible("winSkills"))
                {
                    Gui.HideWindow("winSkills");
                    return;
                }

                // show them
                if (!IsWindowVisible("winChat"))
                {
                    Gui.ShowWindow("winEscMenu", true);
                    return;
                }
            }

            if (CurrentKeyboardState.IsKeyDown(Keys.Space) || (IsMouseButtonDown(MouseButton.Left) && GameState.CurX == GetPlayerX(GameState.MyIndex) && GameState.CurY == GetPlayerY(GameState.MyIndex)))
            {
                GameLogic.CheckMapGetItem();
            }

            if (CurrentKeyboardState.IsKeyDown(Keys.Insert))
            {
                Sender.SendRequestAdmin();
            }

            HandleMouseInputs();
            HandleActiveWindowInput();
            HandleTextInput();

            if (GameState.InGame)
            {
                // Check for movement keys
                UpdateMovementKeys();

                HandleHotbarInput();

                // Exit if escape menu is open
                if (IsWindowVisible("winEscMenu"))
                    return;

                // Check for input cooldown
                if (!IsInputCooldownElapsed())
                    return;

                // Process toggle actions
                HandleWindowToggle(Keys.I, "winInventory", WinMenu.OnInventoryClick);
                HandleWindowToggle(Keys.C, "winCharacter", WinMenu.OnCharacterClick);
                HandleWindowToggle(Keys.K, "winSkills", WinMenu.OnSkillsClick);

                // Handle chat input
                if (CurrentKeyboardState.IsKeyDown(Keys.Enter))
                {
                    if (IsWindowVisible("winChatSmall"))
                    {
                        WinChat.Show();
                        GameState.InSmallChat = false;
                    }
                    else
                    {
                        GameLogic.HandlePressEnter();
                    }

                    UpdateLastInputTime();
                }
            }
        }

        // Helper methods
        private static void UpdateMovementKeys()
        {
            GameState.DirUp = CurrentKeyboardState.IsKeyDown(Keys.W) | CurrentKeyboardState.IsKeyDown(Keys.Up);
            GameState.DirDown = CurrentKeyboardState.IsKeyDown(Keys.S) | CurrentKeyboardState.IsKeyDown(Keys.Down);
            GameState.DirLeft = CurrentKeyboardState.IsKeyDown(Keys.A) | CurrentKeyboardState.IsKeyDown(Keys.Left);
            GameState.DirRight = CurrentKeyboardState.IsKeyDown(Keys.D) | CurrentKeyboardState.IsKeyDown(Keys.Right);
        }

        private static bool IsWindowVisible(string windowName)
        {
            return Gui.TryGetWindow(windowName, out var window) && window!.Visible;
        }

        private static bool IsInputCooldownElapsed()
        {
            return (DateTime.Now - _lastInputTime).TotalMilliseconds >= InputCooldown;
        }

        private static bool IsSearchCooldownElapsed()
        {
            return (DateTime.Now - _lastSearchTime).TotalMilliseconds >= InputCooldown;
        }

        private static void UpdateLastInputTime()
        {
            _lastInputTime = DateTime.Now;
        }

        private static void HandleWindowToggle(Keys key, string windowName, Action toggleAction)
        {
            if (CurrentKeyboardState.IsKeyDown(key) && !IsWindowVisible("winChat"))
            {
                toggleAction.Invoke();
                UpdateLastInputTime();
            }
        }

        private static void HandleActiveWindowInput()
        {
            // Check if there is an active window and that it is visible.
            if (Gui.ActiveWindow is not null && Gui.ActiveWindow.Visible)
            {
                // Check if an active control exists.
                if (Gui.ActiveWindow.ActiveControl is not null)
                {
                    // Get the active control.
                    var activeControl = Gui.ActiveWindow.ActiveControl;

                    // Check if the Enter key is active and can be processed.
                    if (IsKeyStateActive(Keys.Enter))
                    {
                        // Handle Enter: Call the control's callback or activate a new control.
                        activeControl.CallBack[(int) ControlState.FocusEnter]?.Invoke();
                    }

                    // Check if the Tab key is active and can be processed
                    if (IsKeyStateActive(Keys.Tab))
                    {
                        Gui.FocusNextControl();
                    }
                }
            }
        }

        // Handles the hotbar key presses using KeyboardState
        private static void HandleHotbarInput()
        {
            if (GameState.InSmallChat)
            {
                // Iterate through hotbar slots and check for corresponding keys
                for (int i = 0; i < Constant.MaxHotbar; i++)
                {
                    // Check if the corresponding hotbar key is pressed
                    if (CurrentKeyboardState.IsKeyDown((Keys) ((int) Keys.D0 + i)))
                    {
                        Sender.SendUseHotbarSlot(i);
                        return; // Exit once the matching slot is used
                    }
                }
            }
        }

        private static void HandleTextInput()
        {
            // Iterate over all pressed keys  
            foreach (Keys key in CurrentKeyboardState.GetPressedKeys())
            {
                // Check for special keys and skip processing
                if (key == Keys.Tab || key == Keys.LeftShift || key == Keys.RightShift || key == Keys.LeftControl ||
                    key == Keys.RightControl || key == Keys.LeftAlt || key == Keys.RightAlt)
                {
                    continue;
                }

                if (IsKeyStateActive(key))
                {
                    // Handle Backspace key separately  
                    if (key == Keys.Back)
                    {
                        var activeControl = Gui.GetActiveControl();

                        if (activeControl is not null && activeControl.Visible && activeControl.Text.Length > 0)
                        {
                            // Modify the text and update it back in the window  
                            activeControl.Text = activeControl.Text.Substring(0, activeControl.Text.Length - 1);
                            Gui.UpdateActiveControl(activeControl);
                        }

                        continue; // Move to the next key  
                    }

                    // Convert key to a character, considering Shift key  
                    char? character = ConvertKeyToChar(key, CurrentKeyboardState.IsKeyDown(Keys.LeftShift));

                    // If the character is valid, update the active control's text  
                    if (character.HasValue)
                    {
                        var activeControl = Gui.GetActiveControl();

                        if (activeControl is not null && activeControl.Visible && activeControl.Enabled)
                        {
                            string text = activeControl.Text + character.Value;
                            if (TextRenderer.GetTextWidth(text) < activeControl.Width)
                            {
                                // Append character to the control's text  
                                activeControl.Text += character.Value;
                                Gui.UpdateActiveControl(activeControl);
                                continue; // Move to the next key  
                            }
                        }
                    }

                    KeyStates.Remove(key);
                    KeyRepeatTimers.Remove(key);
                }
            }
        }

        // Check if the key can be processed (with interval-based repeat logic)
        private static bool CanProcessKey(Keys key)
        {
            var now = DateTime.Now;
            if (CurrentKeyboardState.IsKeyDown(key))
            {
                if (IsKeyPressedOnce(key) || !KeyRepeatTimers.ContainsKey(key) ||
                    (now - KeyRepeatTimers[key]).TotalMilliseconds >= KeyRepeatInterval)
                {
                    // If the key is released, remove it from KeyStates and reset the timer
                    KeyStates.Remove(key);
                    KeyRepeatTimers.Remove(key);
                    KeyRepeatTimers[key] = now; // Update the timer for the key
                    return true;
                }
            }

            return false;
        }

        private static bool IsKeyPressedOnce(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }

        // Convert a key to a character (if possible)
        private static char? ConvertKeyToChar(Keys key, bool shiftPressed)
        {
            // Handle alphabetic keys
            if (key >= Keys.A && key <= Keys.Z)
            {
                char baseChar = (char)('A' + ((int)key - (int)Keys.A));
                return shiftPressed ? baseChar : char.ToLower(baseChar);
            }

            // Handle numeric keys (0-9)
            if (key >= Keys.D0 && key <= Keys.D9)
            {
                char digit = (char)('0' + ((int)key - (int)Keys.D0));
                return shiftPressed ? General.GetShiftedDigit(digit) : digit;
            }

            // Handle space key
            if (key == Keys.Space)
                return ' ';

            // Handle the "/" character (typically mapped to OemQuestion)
            if (key == Keys.OemQuestion)
            {
                return shiftPressed ? '?' : '/';
            }

            // Ignore unsupported keys (e.g., function keys, control keys)
            return null;
        }

        private static void HandleMouseInputs()
        {
            HandleMouseClick();
            HandleScrollWheel();
        }

        // Ensure GUI event handlers receive GUI-scaled coordinates
        private static void HandleGuiEvent(ControlState state)
        {
            // Save legacy game-context values
            int prevMouseX = GameState.CurMouseX;
            int prevMouseY = GameState.CurMouseY;
            int prevCurX = GameState.CurX;
            int prevCurY = GameState.CurY;

            // Swap to GUI-space values
            GameState.CurMouseX = GameState.CurMouseXGui;
            GameState.CurMouseY = GameState.CurMouseYGui;
            GameState.CurX = GameState.CurXGui;
            GameState.CurY = GameState.CurYGui;

            // Dispatch the GUI event
            Gui.HandleInterfaceEvents(state);

            // Restore game legacy values
            GameState.CurMouseX = prevMouseX;
            GameState.CurMouseY = prevMouseY;
            GameState.CurX = prevCurX;
            GameState.CurY = prevCurY;
        }

        private static void HandleScrollWheel()
        {
            // Handle scroll wheel (assuming delta calculation happens elsewhere)
            int scrollValue = GetMouseScrollDelta();
            if (scrollValue > 0)
            {
                GameLogic.ScrollChatBox(0); // Scroll up

                if (GameState.MyEditorType == EditorType.Map)
                {
                    if (CurrentKeyboardState.IsKeyDown(Keys.LeftShift))
                    {
                        if (GameState.CurLayer > 0)
                        {
                            GameState.CurLayer -= 1;
                        }
                    }
                    else if (GameState.CurTileset > 1)
                    {
                        GameState.CurTileset -= 1;
                        // Sync the slider if the editor is open
                        if (Editor_Map.Instance != null)
                        {
                            Eto.Forms.Application.Instance.AsyncInvoke(() =>
                                Editor_Map.Instance.sldTileSet.Value = GameState.CurTileset
                            );
                        }
                    }
                }
            }
            else if (scrollValue < 0)
            {
                GameLogic.ScrollChatBox(1); // Scroll down

                if (GameState.MyEditorType == EditorType.Map)
                {
                    if (CurrentKeyboardState.IsKeyDown(Keys.LeftShift))
                    {
                        if (GameState.CurLayer < Enum.GetValues(typeof(MapLayer)).Length - 1)
                        {
                            GameState.CurLayer += 1;
                        }
                    }
                    else if (GameState.CurTileset < GameState.NumTileSets)
                    {
                        GameState.CurTileset += 1;
                        // Sync the slider if the editor is open
                        if (Editor_Map.Instance != null)
                        {
                            Eto.Forms.Application.Instance.AsyncInvoke(() =>
                                Editor_Map.Instance.sldTileSet.Value = GameState.CurTileset
                            );
                        }
                    }
                }

                if (scrollValue != 0)
                {
                    HandleGuiEvent(ControlState.MouseScroll);
                }
            }
        }

        private static void HandleMouseClick()
        {
            int currentTime = Environment.TickCount;

            // Handle MouseMove event when the mouse moves
            if (CurrentMouseState.X != PreviousMouseState.X || CurrentMouseState.Y != PreviousMouseState.Y)
            {
                HandleGuiEvent(ControlState.MouseMove);
            }

            // Check for MouseDown event (button pressed)
        if (IsMouseButtonDown(MouseButton.Left))
            {
                if ((DateTime.Now - _lastMouseClickTime).TotalMilliseconds >= MouseClickCooldown)
                {
                    HandleGuiEvent(ControlState.MouseDown);
                    _lastMouseClickTime = DateTime.Now; // Update last mouse click time
                    GameState.LastLeftClickTime = currentTime; // Track time for double-click detection
                    GameState.ClickCount++;
                }

                if (GameState.ClickCount >= 2)
                {
            HandleGuiEvent(ControlState.DoubleClick);
                }
            }

            // Double-click detection for left button
            if ((DateTime.Now - _lastMouseClickTime).TotalMilliseconds >= GameState.DoubleClickTImer)
            {
                GameState.ClickCount = 0;
                GameState.Info = false;
            }

            // Check for MouseUp event (button released)
            if (IsMouseButtonUp(MouseButton.Left))
            {
                HandleGuiEvent(ControlState.MouseUp);
            }

            for (int i = 1; i < Gui.Windows.Count; i++)
            {
                // Check if active control is hovered (GUI context)
                if (Gui.Windows[i].Controls != null)
                {
                    for (int j = 0; j < Gui.Windows[i].Controls.Count; j++)
                    {
                        if (GameState.CurMouseXGui >= Gui.Windows[i].X &&
                            GameState.CurMouseXGui <= Gui.Windows[i].Width + Gui.Windows[i].X &&
                            GameState.CurMouseYGui >= Gui.Windows[i].Y &&
                            GameState.CurMouseYGui <= Gui.Windows[i].Height + Gui.Windows[i].Y)
                        {
                            if (Gui.Windows[i].Controls[j].State != ControlState.Normal)
                            {
                                return;
                            }
                        }
                    }
                }
            }

            // In-game interactions for left click
            if (GameState.InGame == true)
            {
                if (GameState.MyEditorType == EditorType.Map)
                {
                    Editor_Map.MapEditorMouseDown(GameState.CurXGame, GameState.CurYGame, false);
                }

                if (IsSearchCooldownElapsed())
                {
                    if (IsMouseButtonDown(MouseButton.Left))
                    {
                        Player.CheckAttack(true);
                        Sender.PlayerSearch(GameState.CurXGame, GameState.CurYGame, 0);
                        _lastSearchTime = DateTime.Now;
                    }
                }

                // Right-click interactions
                if (IsMouseButtonDown(MouseButton.Right))
                {
                    int slotNum = -1;
                    if (Gui.TryGetWindow("winHotbar", out var winHotbar))
                    {
                        slotNum = (int) GameLogic.IsHotbar(winHotbar!.X, winHotbar!.Y);
                    }

                    if (slotNum >= 0L)
                    {
                        Sender.SendDeleteHotbar(slotNum);
                    }

                    if (GameState.VbKeyShift == true)
                    {
                        // Admin warp if Shift is held and the player has moderator access
                        if (GetPlayerAccess(GameState.MyIndex) >= (int) AccessLevel.Moderator)
                        {
                            Sender.AdminWarp(GameState.CurXGame, GameState.CurYGame);
                        }
                    }
                    else
                    {
                        // Handle right-click menu
                        HandleRightClickMenu();
                    }
                }
            }
        }

        private static void HandleRightClickMenu()
        {
            // Use game-space mouse position for world interactions (target/admin warp/player search)
            var mousePosGame = GetMousePosition("game");
            int mouseXGame = mousePosGame.Item1;
            int mouseYGame = mousePosGame.Item2;

            // Use gui-space mouse position for UI
            var mousePosGui = GetMousePosition("gui");
            int mouseXGui = mousePosGui.Item1;
            int mouseYGui = mousePosGui.Item2;

            for (int i = 0; i < Constant.MaxPlayers; i++)
            {
                if (IsPlaying(i) && GetPlayerMap(i) == GetPlayerMap(GameState.MyIndex))
                {
                    if (GetPlayerX(i) == GameState.CurXGame && GetPlayerY(i) == GameState.CurYGame)
                    {
                        // Show player menu at GUI mouse position (for UI popups)
                        GameLogic.ShowPlayerMenu(i, mouseXGui, mouseYGui);
                    }
                }
            }

            // Perform player search at the current cursor position (game-space)
            Sender.PlayerSearch(GameState.CurXGame, GameState.CurYGame, 1);
        }


        public static void TakeScreenshot()
        {
            // Set the render target to our RenderTarget2D
            Graphics.GraphicsDevice.SetRenderTarget(RenderTarget);

            // Clear the render target with a transparent background
            Graphics.GraphicsDevice.Clear(Color.Transparent);

            // Draw everything to the render target
            General.Client.Draw(new GameTime()); // Assuming Draw handles your game rendering

            // Reset the render target to the back buffer (main display)
            Graphics.GraphicsDevice.SetRenderTarget(null);

            // Save the contents of the RenderTarget2D to a PNG file
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            using (var stream = new FileStream($"screenshot_{timestamp}.png", FileMode.Create))
            {
                RenderTarget.SaveAsPng(stream, RenderTarget.Width, RenderTarget.Height);
            }
        }

        // Draw a filled rectangle with an optional outline
        public static void DrawRectangle(Vector2 position, Vector2 size, Color fillColor, Color outlineColor, float outlineThickness)
        {
            // Create a 1x1 white texture for drawing
            var whiteTexture = new Texture2D(SpriteBatch.GraphicsDevice, 1, 1);

            whiteTexture.SetData([Color.White]);

            // Draw the filled rectangle
            SpriteBatch.Draw(whiteTexture, new Rectangle(position.ToPoint(), size.ToPoint()), fillColor);

            // Draw the outline if thickness > 0
            if (outlineThickness > 0f)
            {
                // Create the four sides of the outline
                var left = new Rectangle(position.ToPoint(),
                    new Point((int) Math.Round(outlineThickness), (int) Math.Round(size.Y)));

                var top = new Rectangle(position.ToPoint(),
                    new Point((int) Math.Round(size.X), (int) Math.Round(outlineThickness)));

                var right = new Rectangle(
                    new Point((int) Math.Round(position.X + size.X - outlineThickness), (int) Math.Round(position.Y)),
                    new Point((int) Math.Round(outlineThickness), (int) Math.Round(size.Y)));

                var bottom =
                    new Rectangle(
                        new Point((int) Math.Round(position.X), (int) Math.Round(position.Y + size.Y - outlineThickness)),
                        new Point((int) Math.Round(size.X), (int) Math.Round(outlineThickness)));

                // Draw the outline rectangles
                SpriteBatch.Draw(whiteTexture, left, outlineColor);
                SpriteBatch.Draw(whiteTexture, top, outlineColor);
                SpriteBatch.Draw(whiteTexture, right, outlineColor);
                SpriteBatch.Draw(whiteTexture, bottom, outlineColor);
            }

            // Dispose the texture to free memory
            whiteTexture.Dispose();
        }

        private static void DrawOutlineRectangle(int x, int y, int width, int height, Color color, float thickness)
        {
            var whiteTexture = new Texture2D(SpriteBatch.GraphicsDevice, 1, 1);

            // Define four rectangles for the outline
            var left = new Rectangle(x, y, (int) Math.Round(thickness), height);
            var top = new Rectangle(x, y, width, (int) Math.Round(thickness));
            var right = new Rectangle((int) Math.Round(x + width - thickness), y, (int) Math.Round(thickness), height);
            var bottom = new Rectangle(x, (int) Math.Round(y + height - thickness), width, (int) Math.Round(thickness));

            // Draw the outline
            SpriteBatch.Draw(whiteTexture, left, color);
            SpriteBatch.Draw(whiteTexture, top, color);
            SpriteBatch.Draw(whiteTexture, right, color);
            SpriteBatch.Draw(whiteTexture, bottom, color);
        }

        public static Color QbColorToXnaColor(int qbColor)
        {
            switch (qbColor)
            {
                case (int) ColorName.Black:
                {
                    return Color.Black;
                }
                case (int) ColorName.Blue:
                {
                    return Color.Blue;
                }
                case (int) ColorName.Green:
                {
                    return Color.Green;
                }
                case (int) ColorName.Cyan:
                {
                    return Color.Cyan;
                }
                case (int) ColorName.Red:
                {
                    return Color.Red;
                }
                case (int) ColorName.Magenta:
                {
                    return Color.Magenta;
                }
                case (int) ColorName.Brown:
                {
                    return Color.Brown;
                }
                case (int) ColorName.Gray:
                {
                    return Color.LightGray;
                }
                case (int) ColorName.DarkGray:
                {
                    return Color.Gray;
                }
                case (int) ColorName.BrightBlue:
                {
                    return Color.LightBlue;
                }
                case (int) ColorName.BrightGreen:
                {
                    return Color.LightGreen;
                }
                case (int) ColorName.BrightCyan:
                {
                    return Color.LightCyan;
                }
                case (int) ColorName.BrightRed:
                {
                    return Color.LightCoral;
                }
                case (int) ColorName.Pink:
                {
                    return Color.Orchid;
                }
                case (int) ColorName.Yellow:
                {
                    return Color.Yellow;
                }
                case (int) ColorName.White:
                {
                    return Color.White;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(qbColor), "Invalid QbColor value.");
                }
            }
        }

        public static void DrawEmote(int x2, int y2, int sprite)
        {
            Rectangle rec;
            int x;
            int y;
            int anim;

            if (sprite < 1 | sprite > GameState.NumEmotes)
                return;

            if (GameState.ShowAnimLayers)
            {
                anim = 1;
            }
            else
            {
                anim = 0;
            }

            rec.Y = 0;
            rec.Height = GameState.SizeX;
            rec.X = (int) Math.Round(anim *
                                     (GetGfxInfo(Path.Combine(DataPath.Emotes, sprite.ToString())).Width /
                                      2d));
            rec.Width = (int) Math.Round(GetGfxInfo(Path.Combine(DataPath.Emotes, sprite.ToString())).Width /
                                         2d);

            x = GameLogic.ConvertMapX(x2);
            y = GameLogic.ConvertMapY(y2) - (GameState.SizeY + 16);

            string argPath = Path.Combine(DataPath.Emotes, sprite.ToString());
            RenderTexture(ref argPath, x, y, rec.X, rec.Y, rec.Width, rec.Height);
        }

        public static void DrawDirections(int x, int y)
        {
            Rectangle rec;
            int i;

            // render grid
            rec.Y = 24;
            rec.X = 0;
            rec.Width = 32;
            rec.Height = 32;

            string argPath = Path.Combine(DataPath.Misc, "Direction");
            RenderTexture(ref argPath, GameLogic.ConvertMapX(x * GameState.SizeX),
                GameLogic.ConvertMapY(y * GameState.SizeY),
                rec.X, rec.Y, rec.Width, rec.Height, rec.Width, rec.Height);

            // render dir blobs
            for (i = 0; i < 4; i++)
            {
                rec.X = i * 8;
                rec.Width = 8;

                // find out whether render blocked or not
                bool LocalIsDirBlocked()
                {
                    byte argdir = (byte) i;
                    var n = GameLogic.IsDirBlocked(ref Data.MyMap.Tile[x, y].DirBlock, ref argdir);
                    return n;
                }

                if (!LocalIsDirBlocked())
                {
                    rec.Y = 8;
                }
                else
                {
                    rec.Y = 16;
                }

                rec.Height = 8;

                string argPath1 = Path.Combine(DataPath.Misc, "Direction");
                RenderTexture(ref argPath1, GameLogic.ConvertMapX(x * GameState.SizeX) + GameState.DirArrowX[i],
                    GameLogic.ConvertMapY(y * GameState.SizeY) + GameState.DirArrowY[i], rec.X, rec.Y, rec.Width,
                    rec.Height,
                    rec.Width, rec.Height);
            }
        }

        public static void DrawPaperdoll(int x2, int y2, int sprite, int anim, int spritetop)
        {
            Rectangle rec;
            int x;
            int y;
            int width;
            int height;

            if (sprite < 1 | sprite > GameState.NumPaperdolls)
                return;

            rec.Y = (int) Math.Round(spritetop *
                GetGfxInfo(Path.Combine(DataPath.Paperdolls, sprite.ToString())).Height / 4d);
            rec.Height =
                (int) Math.Round(GetGfxInfo(Path.Combine(DataPath.Paperdolls, sprite.ToString())).Height /
                                 4d);
            rec.X = (int) Math.Round(anim *
                GetGfxInfo(Path.Combine(DataPath.Paperdolls, sprite.ToString())).Width / 4d);
            rec.Width = (int) Math.Round(
                GetGfxInfo(Path.Combine(DataPath.Paperdolls, sprite.ToString())).Width /
                4d);

            x = GameLogic.ConvertMapX(x2);
            y = GameLogic.ConvertMapY(y2);
            width = rec.Right - rec.Left;
            height = rec.Bottom - rec.Top;

            string argPath = Path.Combine(DataPath.Paperdolls, sprite.ToString());
            RenderTexture(ref argPath, x, y, rec.X, rec.Y, rec.Width, rec.Height);
        }

        public static void DrawNpc(int mapNpcNum)
        {
            byte anim;
            int x;
            int y;
            int sprite;
            var spriteLeft = default(int);
            Rectangle rect;
            int attackSpeed = 1000;

            // Check if Npc exists
            if (Data.MyMapNpc[(int) mapNpcNum].Num < 0 ||
                Data.MyMapNpc[(int) mapNpcNum].Num > Constant.MaxNpcs)
                return;

            x = (int) Math.Floor((double) Data.MyMapNpc[(int) mapNpcNum].X / 32);
            y = (int) Math.Floor((double) Data.MyMapNpc[(int) mapNpcNum].Y / 32);

            // Ensure Npc is within the tile view range
            if (x < GameState.TileView.Left |
                x > GameState.TileView.Right)
                return;

            if (y < GameState.TileView.Top |
                y > GameState.TileView.Bottom)
                return;

            // Stream Npc if not yet loaded
            Database.StreamNpc((int) Data.MyMapNpc[(int) mapNpcNum].Num);

            // Get the sprite of the Npc
            sprite = Data.Npc[(int) Data.MyMapNpc[(int) mapNpcNum].Num].Sprite;

            // Validate sprite
            if (sprite < 1 | sprite > GameState.NumCharacters)
                return;

            // Reset animation frame
            anim = 0;

            // Check for attacking animation
            if (Data.MyMapNpc[(int) mapNpcNum].AttackTimer + attackSpeed / 2d > General.GetTickCount() &&
                Data.MyMapNpc[(int) mapNpcNum].Attacking == 1)
            {
                anim = 3;
            }
            else
            {
                anim = (byte) Data.MyMapNpc[(int) mapNpcNum].Steps;
            }

            // Reset attacking state if attack timer has passed
            {
                ref var withBlock = ref Data.MyMapNpc[(int) mapNpcNum];
                if (withBlock.AttackTimer + attackSpeed < General.GetTickCount())
                {
                    withBlock.Attacking = 0;
                    withBlock.AttackTimer = 0;
                }
            }

            // Set sprite sheet position based on direction
            switch (Data.MyMapNpc[(int) mapNpcNum].Dir)
            {
                case (int) Direction.Up:
                {
                    spriteLeft = 3;
                    break;
                }
                case (int) Direction.Right:
                {
                    spriteLeft = 2;
                    break;
                }
                case (int) Direction.Down:
                {
                    spriteLeft = 0;
                    break;
                }
                case (int) Direction.Left:
                {
                    spriteLeft = 1;
                    break;
                }
            }

            // Create the rectangle for rendering the sprite
            rect = new Rectangle(
                (int) Math.Round(anim *
                                 (GetGfxInfo(Path.Combine(DataPath.Characters, sprite.ToString())).Width /
                                  4d)),
                (int) Math.Round(spriteLeft *
                                 (GetGfxInfo(Path.Combine(DataPath.Characters, sprite.ToString())).Height /
                                  4d)),
                (int) Math.Round(GetGfxInfo(Path.Combine(DataPath.Characters, sprite.ToString())).Width / 4d),
                (int) Math.Round(GetGfxInfo(Path.Combine(DataPath.Characters, sprite.ToString())).Height /
                                 4d));

            // Calculate X and Y coordinates for rendering
            x = (int) Math.Round(Data.MyMapNpc[(int) mapNpcNum].X -
                                 (GetGfxInfo(Path.Combine(DataPath.Characters, sprite.ToString())).Width /
                                  4d -
                                  32d) / 2d);

            if (GetGfxInfo(Path.Combine(DataPath.Characters, sprite.ToString())).Height / 4d > 32d)
            {
                // Larger sprites need an offset for height adjustment
                y = (int) Math.Round(Data.MyMapNpc[(int) mapNpcNum].Y -
                                     (GetGfxInfo(Path.Combine(DataPath.Characters, sprite.ToString()))
                                             .Height /
                                         4d - 32d));
            }
            else
            {
                // Normal sprite height
                y = Data.MyMapNpc[(int) mapNpcNum].Y;
            }

            // Draw shadow and Npc sprite
            // DrawShadow(x, y + 16)
            DrawCharacterSprite(sprite, x, y, rect);
        }

        public static void DrawMapItem(int itemNum)
        {
            Rectangle srcRec;
            Rectangle destRec;
            int picNum;
            int x;
            int y;

            if (Data.MyMapItem[itemNum].Num < 0 | Data.MyMapItem[itemNum].Num > Constant.MaxItems)
                return;

            Item.StreamItem(Data.MyMapItem[itemNum].Num);

            picNum = Data.Item[Data.MyMapItem[itemNum].Num].Icon;

            if (picNum < 1 | picNum > GameState.NumItems)
                return;

            ref var withBlock = ref Data.MyMapItem[itemNum];

            if (Math.Floor((double) withBlock.X / 32) < GameState.TileView.Left | Math.Floor((double) withBlock.X / 32) > GameState.TileView.Right)
                return;

            if (Math.Floor((double) withBlock.Y / 32) < GameState.TileView.Top | Math.Floor((double) withBlock.Y / 32) > GameState.TileView.Bottom)
                return;

            srcRec = new Rectangle(0, 0, GameState.SizeX, GameState.SizeY);
            destRec = new Rectangle(GameLogic.ConvertMapX(Data.MyMapItem[itemNum].X),
                GameLogic.ConvertMapY(Data.MyMapItem[itemNum].Y), GameState.SizeX, GameState.SizeY);

            x = GameLogic.ConvertMapX(Data.MyMapItem[itemNum].X);
            y = GameLogic.ConvertMapY(Data.MyMapItem[itemNum].Y);

            string argPath = Path.Combine(DataPath.Items, picNum.ToString());
            RenderTexture(ref argPath, x, y, srcRec.X, srcRec.Y, srcRec.Width, srcRec.Height, srcRec.Width,
                srcRec.Height);
        }

        public static void DrawCharacterSprite(int sprite, int x2, int y2, Rectangle sRect)
        {
            int x;
            int y;

            if (sprite < 1 | sprite > GameState.NumCharacters)
                return;

            x = GameLogic.ConvertMapX(x2);
            y = GameLogic.ConvertMapY(y2);

            string argPath = Path.Combine(DataPath.Characters, sprite.ToString());
            RenderTexture(ref argPath, x, y, sRect.X, sRect.Y, sRect.Width, sRect.Height, sRect.Width, sRect.Height);
        }

        public static void DrawBlood(int index)
        {
            Rectangle srcRec;
            Rectangle destRec;
            int x;
            int y;

            {
                ref var withBlock = ref Data.Blood[index];
                if (withBlock.X < GameState.TileView.Left | withBlock.X > GameState.TileView.Right)
                    return;
                if (withBlock.Y < GameState.TileView.Top | withBlock.Y > GameState.TileView.Bottom)
                    return;

                // check if we should be seeing it
                if (withBlock.Timer + 20000 < General.GetTickCount())
                    return;

                x = GameLogic.ConvertMapX(Data.Blood[index].X);
                y = GameLogic.ConvertMapY(Data.Blood[index].Y);

                srcRec = new Rectangle((withBlock.Sprite - 1) * GameState.SizeX, 0, GameState.SizeX, GameState.SizeY);
                destRec = new Rectangle(GameLogic.ConvertMapX(withBlock.X),
                    GameLogic.ConvertMapY(withBlock.Y), GameState.SizeX, GameState.SizeY);

                string argPath = Path.Combine(DataPath.Misc, "Blood");
                RenderTexture(ref argPath, x, y, srcRec.X, srcRec.Y, srcRec.Width, srcRec.Height);
            }
        }

        public static void DrawBars()
        {
            long left;
            long top;
            long width;
            long height;
            long tmpX;
            long tmpY;
            var barWidth = default(long);
            long i;
            long npcNum;

            // dynamic bar calculations
            width = GetGfxInfo(Path.Combine(DataPath.Misc, "Bars")).Width;
            height = (long) Math.Round(GetGfxInfo(Path.Combine(DataPath.Misc, "Bars")).Height / 4d);

            // render Npc health bars
            for (i = 0L; i < Constant.MaxMapNpcs; i++)
            {
                npcNum = (long) Data.MyMapNpc[(int) i].Num;
                // exists?
                if (npcNum >= 0L && npcNum <= Constant.MaxNpcs)
                {
                    // alive?
                    if (Data.MyMapNpc[(int) i].Vital[(int) Vital.Health] > 0 &
                        Data.MyMapNpc[(int) i].Vital[(int) Vital.Health] < Data.Npc[(int) npcNum].Hp)
                    {
                        // lock to Npc
                        tmpX = (long) Math.Round(Data.MyMapNpc[(int) i].X + 16 - width / 2d);
                        tmpY = Data.MyMapNpc[(int) i].Y + 35;

                        // calculate the width to fill
                        if (width > 0)
                            GameState.BarWidthNpcHpMax[(int) i] = (int) Math.Round(
                                Data.MyMapNpc[(int) i].Vital[(int) Vital.Health] / (double) width /
                                (Data.Npc[(int) npcNum].Hp / (double) width) * width);

                        // draw bar background
                        top = height * 3L; // HP bar background
                        left = 0L;
                        string argPath = Path.Combine(DataPath.Misc, "Bars");
                        RenderTexture(ref argPath, GameLogic.ConvertMapX((int) tmpX), GameLogic.ConvertMapY((int) tmpY),
                            (int) left, (int) top, (int) width, (int) height, (int) width, (int) height);

                        // draw the bar proper
                        top = 0L; // HP bar
                        left = 0L;
                        string argPath1 = Path.Combine(DataPath.Misc, "Bars");
                        RenderTexture(ref argPath1, GameLogic.ConvertMapX((int) tmpX), GameLogic.ConvertMapY((int) tmpY),
                            (int) left, (int) top, (int) GameState.BarWidthNpcHp[(int) i], (int) height,
                            (int) GameState.BarWidthNpcHp[(int) i], (int) height);
                    }
                }
            }

            for (i = 0L; i < Constant.MaxPlayers; i++)
            {
                if (GetPlayerMap((int) i) == GetPlayerMap((int) i))
                {
                    if (GetPlayerVital((int) i, Vital.Health) > 0 &
                        GetPlayerVital((int) i, Vital.Health) < GetPlayerMaxVital((int) i, Vital.Health))
                    {
                        // lock to Player
                        tmpX = (long) Math.Round(GetPlayerRawX((int) i) +
                            16 - width / 2d);
                        tmpY = GetPlayerRawY((int) i) + 35;

                        // calculate the width to fill
                        if (width > 0)
                            GameState.BarWidthPlayerHpMax[(int) i] = (int) Math.Round(
                                GetPlayerVital((int) i, Vital.Health) / (double) width /
                                (GetPlayerMaxVital((int) i, Vital.Health) / (double) width) * width);

                        // draw bar background
                        top = height * 3L; // HP bar background
                        left = 0L;
                        string argPath2 = Path.Combine(DataPath.Misc, "Bars");
                        RenderTexture(ref argPath2, GameLogic.ConvertMapX((int) tmpX), GameLogic.ConvertMapY((int) tmpY),
                            (int) left, (int) top, (int) width, (int) height, (int) width, (int) height);

                        // draw the bar proper
                        top = 0L; // HP bar
                        left = 0L;
                        string argPath3 = Path.Combine(DataPath.Misc, "Bars");
                        RenderTexture(ref argPath3, GameLogic.ConvertMapX((int) tmpX), GameLogic.ConvertMapY((int) tmpY),
                            (int) left, (int) top, (int) GameState.BarWidthPlayerHp[(int) i], (int) height,
                            (int) GameState.BarWidthPlayerHp[(int) i], (int) height);
                    }

                    if (GetPlayerVital((int) i, Vital.Stamina) > 0 &
                        GetPlayerVital((int) i, Vital.Stamina) < GetPlayerMaxVital((int) i, Vital.Stamina))
                    {
                        // lock to Player
                        tmpX = (long)Math.Round(GetPlayerRawX((int)i) +
                            16 - width / 2d);
                        tmpY = GetPlayerRawY((int)i) + 35 + height;

                        // calculate the width to fill
                        if (width > 0)
                            GameState.BarWidthPlayerSpMax[(int) i] = (int) Math.Round(
                                GetPlayerVital((int) i, Vital.Stamina) / (double) width /
                                (GetPlayerMaxVital((int) i, Vital.Stamina) / (double) width) * width);

                        // draw bar background
                        top = height * 3L; // SP bar background
                        left = 0L;
                        string argPath4 = Path.Combine(DataPath.Misc, "Bars");
                        RenderTexture(ref argPath4, GameLogic.ConvertMapX((int) tmpX), GameLogic.ConvertMapY((int) tmpY),
                            (int) left, (int) top, (int) width, (int) height, (int) width, (int) height);

                        // draw the bar proper
                        top = height * 1L; // SP bar
                        left = 0L;
                        string argPath5 = Path.Combine(DataPath.Misc, "Bars");
                        RenderTexture(ref argPath5, GameLogic.ConvertMapX((int) tmpX), GameLogic.ConvertMapY((int) tmpY),
                            (int) left, (int) top, (int) GameState.BarWidthPlayerSp[(int) i], (int) height,
                            (int) GameState.BarWidthPlayerSp[(int) i], (int) height);
                    }

                    if (GameState.SkillBuffer >= 0)
                    {
                        if ((int) Data.Player[(int) i].Skill[GameState.SkillBuffer].Num >= 0)
                        {
                            if (Data.Skill[(int) Data.Player[(int) i].Skill[GameState.SkillBuffer].Num]
                                    .CastTime >
                                0)
                            {
                                // lock to player
                                tmpX = (long)Math.Round(GetPlayerRawX((int)i) + 16 - width / 2d);

                                tmpY = GetPlayerRawY((int)i) + 35 + height;

                                // calculate the width to fill
                                if (width > 0L)
                                    barWidth = (long) Math.Round((General.GetTickCount() - GameState.SkillBufferTimer) /
                                        (double) (Data
                                            .Skill[(int) Data.Player[(int) i].Skill[GameState.SkillBuffer].Num]
                                            .CastTime * 1000) * width);

                                // draw bar background
                                top = height * 3L; // cooldown bar background
                                left = 0L;
                                string argPath6 = Path.Combine(DataPath.Misc, "Bars");
                                RenderTexture(ref argPath6, GameLogic.ConvertMapX((int) tmpX),
                                    GameLogic.ConvertMapY((int) tmpY), (int) left, (int) top, (int) width, (int) height,
                                    (int) width, (int) height);

                                // draw the bar proper
                                top = height * 2L; // cooldown bar
                                left = 0L;
                                string argPath7 = Path.Combine(DataPath.Misc, "Bars");
                                RenderTexture(ref argPath7, GameLogic.ConvertMapX((int) tmpX),
                                    GameLogic.ConvertMapY((int) tmpY), (int) left, (int) top, (int) barWidth, (int) height,
                                    (int) barWidth, (int) height);
                            }
                        }
                    }
                }
            }
        }

        public void DrawEyeDropper()
        {
            SpriteBatch.Begin();

            // Define rectangle parameters.
            var position = new Vector2(GameLogic.ConvertMapX(GameState.CurXGame), GameLogic.ConvertMapY(GameState.CurYGame));
            var size = new Vector2(GameState.SizeX, GameState.SizeX);
            var fillColor = Color.Transparent; // No fill
            var outlineColor = Color.Cyan; // Cyan outline
            int outlineThickness = 1; // Thickness of outline

            // Draw the rectangle with an outline.
            DrawRectangle(position, size, fillColor, outlineColor, outlineThickness);
            SpriteBatch.End();
        }

        public static void DrawGrid()
        {
            // Use a single Begin/End pair to improve performance
            SpriteBatch.Begin();

            // Iterate over the tiles in the visible range
            for (double x = GameState.TileView.Left - 1d, loopTo = GameState.TileView.Right + 1d; x < loopTo; x++)
            {
                for (double y = GameState.TileView.Top - 1d, loopTo1 = GameState.TileView.Bottom + 1d; y < loopTo1; y++)
                {
                    if (GameLogic.IsValidMapPoint((int) Math.Round(x), (int) Math.Round(y)))
                    {
                        // Calculate the tile position and size
                        int posX = GameLogic.ConvertMapX((int) Math.Round((x - 1d)));
                        int posY = GameLogic.ConvertMapY((int) Math.Round((y - 1d) * GameState.SizeY));
                        int rectWidth = GameState.SizeX;
                        int rectHeight = GameState.SizeY;

                        // Draw the transparent rectangle as the tile background
                        SpriteBatch.Draw(TransparentTexture, new Rectangle(posX, posY, rectWidth, rectHeight),
                            Color.Transparent);

                        // Define the outline color and thickness
                        var outlineColor = Color.White;
                        int thickness = 1;

                        // Draw the tile outline (top, bottom, left, right)
                        SpriteBatch.Draw(TransparentTexture, new Rectangle(posX, posY, rectWidth, thickness),
                            outlineColor); // Top
                        SpriteBatch.Draw(TransparentTexture,
                            new Rectangle(posX, posY + rectHeight - thickness, rectWidth, thickness),
                            outlineColor); // Bottom
                        SpriteBatch.Draw(TransparentTexture, new Rectangle(posX, posY, thickness, rectHeight),
                            outlineColor); // Left
                        SpriteBatch.Draw(TransparentTexture,
                            new Rectangle(posX + rectWidth - thickness, posY, thickness, rectHeight),
                            outlineColor); // Right
                    }
                }
            }

            SpriteBatch.End();
        }

        public static void DrawTarget(int x2, int y2)
        {
            Rectangle rec;
            int x;
            int y;
            int width;
            int height;

            rec.Y = 0;
            rec.Height = GetGfxInfo(Path.Combine(DataPath.Misc, "Target")).Height;
            rec.X = 0;
            rec.Width = (int) Math.Round(GetGfxInfo(Path.Combine(DataPath.Misc, "Target")).Width / 2d);
            x = GameLogic.ConvertMapX(x2 + 4);
            y = GameLogic.ConvertMapY(y2 - 32);
            width = rec.Right - rec.Left;
            height = rec.Bottom - rec.Top;

            string argPath = Path.Combine(DataPath.Misc, "Target");
            RenderTexture(ref argPath, x, y, rec.X, rec.Y, rec.Width, rec.Height, rec.Width, rec.Height);
        }

        public static Color ToMonoGameColor(System.Drawing.Color drawingColor)
        {
            return new Color(drawingColor.R, drawingColor.G, drawingColor.B, drawingColor.A);
        }

        public static void DrawHover(int x2, int y2)
        {
            Rectangle rec;
            int x;
            int y;
            int width;
            int height;

            rec.Y = 0;
            rec.Height = GetGfxInfo(Path.Combine(DataPath.Misc, "Target")).Height;
            rec.X = (int) Math.Round(GetGfxInfo(Path.Combine(DataPath.Misc, "Target")).Width / 2d);
            rec.Width = (int) Math.Round(GetGfxInfo(Path.Combine(DataPath.Misc, "Target")).Width / 2d +
                                         GetGfxInfo(Path.Combine(DataPath.Misc, "Target")).Width / 2d);

            x = GameLogic.ConvertMapX(x2 + 4);
            y = GameLogic.ConvertMapY(y2 - 32);
            width = rec.Right - rec.Left;
            height = rec.Bottom - rec.Top;

            string argPath = Path.Combine(DataPath.Misc, "Target");
            RenderTexture(ref argPath, x, y, rec.X, rec.Y, rec.Width, rec.Height, rec.Width, rec.Height);
        }

        public static void DrawChatBubble(long index)
        {
            var theArray = default(string[]);
            int x;
            int y;
            long i;
            var maxWidth = default(long);
            long x2;
            long y2;
            int color;
            long tmpNum;

            {
                ref var withBlock = ref Data.ChatBubble[(int) index];

                // exit out early
                if (withBlock.TargetType == 0)
                    return;

                color = withBlock.Color;

                // calculate position
                switch (withBlock.TargetType)
                {
                    case (byte) TargetType.Player:
                    {
                        // it's a player
                        if (!(GetPlayerMap(withBlock.Target) == GetPlayerMap(GameState.MyIndex)))
                            return;

                        // it's on our map - get co-ords
                        x = GameLogic.ConvertMapX(Data.Player[withBlock.Target].X) + 16;
                        y = GameLogic.ConvertMapY(Data.Player[withBlock.Target].Y) - 32;
                        break;
                    }
                    case (byte) TargetType.Event:
                    {
                        x = GameLogic.ConvertMapX(Data.MyMap.Event[withBlock.Target].X) + 16;
                        y = GameLogic.ConvertMapY(Data.MyMap.Event[withBlock.Target].Y) - 16;
                        break;
                    }

                    case (byte) TargetType.Npc:
                    {
                        x = GameLogic.ConvertMapX(Data.MyMapNpc[withBlock.Target].X) + 16;
                        y = GameLogic.ConvertMapY(Data.MyMapNpc[withBlock.Target].Y) - 32;
                        break;
                    }

                    default:
                    {
                        x = 0;
                        y = 0;
                        return;
                    }
                }

                withBlock.Msg = withBlock.Msg.Replace("\0", string.Empty);

                // word wrap
                TextRenderer.WordWrap(withBlock.Msg, Font.Georgia, GameState.ChatBubbleWidth, ref theArray);

                // find max width
                tmpNum = Information.UBound(theArray);

                var loopTo = tmpNum;
                for (i = 0L; i <= loopTo; i++)
                {
                    if (TextRenderer.GetTextWidth(theArray[(int) i], Font.Georgia) > maxWidth)
                        maxWidth = TextRenderer.GetTextWidth(theArray[(int) i], Font.Georgia);
                }

                // calculate the new position 
                x2 = x - maxWidth / 2L;
                y2 = y - (Information.UBound(theArray) + 1) * 12;

                // render bubble - top left
                string argPath = Path.Combine(DataPath.Gui, 33.ToString());
                RenderTexture(ref argPath, (int) (x2 - 9L), (int) (y2 - 5L), 0, 0, 9, 5, 9, 5);

                // top right
                string argPath1 = Path.Combine(DataPath.Gui, 33.ToString());
                RenderTexture(ref argPath1, (int) (x2 + maxWidth), (int) (y2 - 5L), 119, 0, 9, 5, 9, 5);

                // top
                string argPath2 = Path.Combine(DataPath.Gui, 33.ToString());
                RenderTexture(ref argPath2, (int) x2, (int) (y2 - 5L), 9, 0, (int) maxWidth, 5, 5, 5);

                // bottom left
                string argPath3 = Path.Combine(DataPath.Gui, 33.ToString());
                RenderTexture(ref argPath3, (int) (x2 - 9L), (int) y, 0, 19, 9, 6, 9, 6);

                // bottom right
                string argPath4 = Path.Combine(DataPath.Gui, 33.ToString());
                RenderTexture(ref argPath4, (int) (x2 + maxWidth), (int) y, 119, 19, 9, 6, 9, 6);

                // bottom - left half
                string argPath5 = Path.Combine(DataPath.Gui, 33.ToString());
                RenderTexture(ref argPath5, (int) x2, (int) y, 9, 19, (int) (maxWidth / 2L - 5L), 6, 6, 6);

                // bottom - right half
                string argPath6 = Path.Combine(DataPath.Gui, 33.ToString());
                RenderTexture(ref argPath6, (int) (x2 + maxWidth / 2L + 6L), (int) y, 9, 19, (int) (maxWidth / 2L - 5L), 6,
                    9,
                    6);

                // left
                string argPath7 = Path.Combine(DataPath.Gui, 33.ToString());
                RenderTexture(ref argPath7, (int) (x2 - 9L), (int) y2, 0, 6, 9, (Information.UBound(theArray) + 1) * 12, 9, 6);

                // right
                string argPath8 = Path.Combine(DataPath.Gui, 33.ToString());
                RenderTexture(ref argPath8, (int) (x2 + maxWidth), (int) y2, 119, 6, 9, (Information.UBound(theArray) + 1) * 12,
                    9,
                    6);

                // center
                string argPath9 = Path.Combine(DataPath.Gui, 33.ToString());
                RenderTexture(ref argPath9, (int) x2, (int) y2, 9, 5, (int) maxWidth, (Information.UBound(theArray) + 1) * 12, 9,
                    5);

                // little pointy bit
                string argPath10 = Path.Combine(DataPath.Gui, 33.ToString());
                RenderTexture(ref argPath10, (int) (x - 5L), (int) y, 58, 19, 11, 11, 11, 11);

                // render each line centralized
                tmpNum = Information.UBound(theArray);

                var loopTo1 = tmpNum;
                for (i = 0; i <= loopTo1; i++)
                {
                    if (theArray[(int) i] == null)
                        continue;

                    // Measure button text size and apply padding
                    var textSize = TextRenderer.Fonts[Font.Georgia].MeasureString(theArray[(int) i]);
                    float actualWidth = textSize.X;
                    float actualHeight = textSize.Y;

                    // Calculate horizontal and vertical centers with padding
                    double padding = (double) actualWidth / 6.0d;

                    TextRenderer.RenderText(theArray[(int) i],
                        (int) Math.Round(x - theArray[(int) i].Length / 2d - TextRenderer.GetTextWidth(theArray[(int) i]) / 2d +
                                         padding), (int) y2, QbColorToXnaColor(withBlock.Color),
                        Color.Black);
                    y2 = y2 + 12L;
                }

                // check if it's timed out - close it if so
                if (withBlock.Timer + 5000 < General.GetTickCount())
                {
                    withBlock.Active = false;
                }
            }
        }

        public static void DrawPlayer(int index)
        {
            byte anim;
            int x;
            int y;
            int spriteNum;
            var spriteleft = default(int);
            int attackSpeed;
            Rectangle rect;

            spriteNum = GetPlayerSprite(index);

            if (index < 0 | index > Constant.MaxPlayers)
                return;

            if (spriteNum <= 0 | spriteNum > GameState.NumCharacters)
                return;

            // speed from weapon
            if (GetPlayerEquipment(index, Equipment.Weapon) >= 0)
            {
                attackSpeed = Data.Item[GetPlayerEquipment(index, Equipment.Weapon)].Speed;
            }
            else
            {
                attackSpeed = 1000;
            }

            // Reset frame
            anim = 0;

            // Check for attacking animation
            if (Data.Player[index].AttackTimer + attackSpeed / 2d > General.GetTickCount())
            {
                if (Data.Player[index].Attacking == 1)
                {
                    anim = 3;
                }
            }
            else
            {
                anim = Data.Player[index].Steps;
            }

            // Check to see if we want to stop making him attack
            {
                ref var withBlock = ref Data.Player[index];
                if (withBlock.AttackTimer + attackSpeed < General.GetTickCount())
                {
                    withBlock.Attacking = 0;
                    withBlock.AttackTimer = 0;
                }
            }

            // Set the left
            switch (GetPlayerDir(index))
            {
                case (int) Direction.Up:
                {
                    spriteleft = 3;
                    break;
                }
                case (int) Direction.Right:
                {
                    spriteleft = 2;
                    break;
                }
                case (int) Direction.Down:
                {
                    spriteleft = 0;
                    break;
                }
                case (int) Direction.Left:
                {
                    spriteleft = 1;
                    break;
                }
                case (int) Direction.UpRight:
                {
                    spriteleft = 2;
                    break;
                }
                case (int) Direction.UpLeft:
                {
                    spriteleft = 1;
                    break;
                }
                case (int) Direction.DownLeft:
                {
                    spriteleft = 1;
                    break;
                }
                case (int) Direction.DownRight:
                {
                    spriteleft = 2;
                    break;
                }
            }

            var gfxInfo = GetGfxInfo(Path.Combine(DataPath.Characters, spriteNum.ToString()));
            if (gfxInfo == null)
            {
                // Handle the case where the graphic information is not found
                return;
            }

            // Calculate the X
            x = (int) Math.Round(Data.Player[index].X - (gfxInfo.Width / 4d - 32d) / 2d);

            // Is the player's height more than 32..?
            if ((gfxInfo.Height / 4) > 32)
            {
                // Create a 32 pixel offset for larger sprites
                y = (int) Math.Round(GetPlayerRawY(index) - (gfxInfo.Height / 4d - 32d));
            }
            else
            {
                // Proceed as normal
                y = GetPlayerRawY(index);
            }

            rect = new Rectangle((int) Math.Round(anim * (gfxInfo.Width / 4d)),
                (int) Math.Round(spriteleft * (gfxInfo.Height / 4d)), (int) Math.Round(gfxInfo.Width / 4d),
                (int) Math.Round(gfxInfo.Height / 4d));

            // render the actual sprite
            // DrawShadow(x, y + 16)
            DrawCharacterSprite(spriteNum, x, y, rect);

            // check for paperdolling with directional draw order rules
            // Rule: draw weapon first when facing up (behind), draw weapon last when facing down (in front)
            var dirVal = (Direction) GetPlayerDir(index);
            Equipment[] eqOrder = new[] { Equipment.Weapon, Equipment.Armor, Equipment.Helmet, Equipment.Shield };

            // Treat diagonals as their vertical tendency
            bool isUp = dirVal == Direction.Up || dirVal == Direction.UpLeft || dirVal == Direction.UpRight;
            bool isDown = dirVal == Direction.Down || dirVal == Direction.DownLeft || dirVal == Direction.DownRight;

            if (isDown)
            {
                // Move weapon to the end so it draws on top
                eqOrder = new[] { Equipment.Armor, Equipment.Helmet, Equipment.Shield, Equipment.Weapon };
            }
            else if (isUp)
            {
                // Ensure weapon is first so it draws behind
                eqOrder = new[] { Equipment.Weapon, Equipment.Armor, Equipment.Helmet, Equipment.Shield };
            }

            foreach (var eq in eqOrder)
            {
                if (GetPlayerEquipment(index, eq) >= 0)
                {
                    var itemIndex = GetPlayerEquipment(index, eq);
                    var paperId = Data.Item[itemIndex].Paperdoll;
                    if (paperId > 0)
                    {
                        DrawPaperdoll(x, y, paperId, anim, spriteleft);
                    }
                }
            }

            // Check to see if we want to stop showing emote
            {
                ref var withBlock1 = ref Data.Player[index];
                if (withBlock1.EmoteTimer < General.GetTickCount())
                {
                    withBlock1.Emote = 0;
                    withBlock1.EmoteTimer = 0;
                }
            }

            // check for emotes
            if (Data.Player[GameState.MyIndex].Emote > 0)
            {
                DrawEmote(x, y, Data.Player[GameState.MyIndex].Emote);
            }
        }

        public static void DrawEvents()
        {
            if (Data.MyMap.Event == null)
                return;

            for (int i = 0, loopTo = Information.UBound(Data.MyMap.Event); i < loopTo; i++)
            {
                int x = GameLogic.ConvertMapX(Data.MyMap.Event[i].X);
                int y = GameLogic.ConvertMapY(Data.MyMap.Event[i].Y);

                // Skip event if there are no pages
                if (Data.MyMap.Event[i].PageCount <= 0)
                {
                    DrawOutlineRectangle(x, y, GameState.SizeX, GameState.SizeY, Color.Blue, 0.6f);
                    continue;
                }

                // Render event based on its graphic type
                switch (Data.MyMap.Event[i].Pages[0].GraphicType)
                {
                    case 0: // Text Event
                    {
                        int tX = x * GameState.SizeX;
                        int tY = y * GameState.SizeY;
                        TextRenderer.RenderText("E", tX, tY, Color.Green, Color.Black);
                        break;
                    }

                    case 1: // Character Graphic
                    {
                        RenderCharacterGraphic(Data.MyMap.Event[i], x * GameState.SizeX, y * GameState.SizeY);
                        break;
                    }

                    case 2: // Tileset Graphic
                    {
                        RenderTilesetGraphic(Data.MyMap.Event[i], x, y);
                        break;
                    }

                    default:
                    {
                        // Draw fallback outline rectangle if graphic type is unknown
                        DrawOutlineRectangle(x, y, GameState.SizeX, GameState.SizeY, Color.Blue, 0.6f);
                        break;
                    }
                }
            }
        }

        public static void RenderCharacterGraphic(Type.Event eventData, int x, int y)
        {
            // Get the graphic index from the event's first page
            int gfxIndex = eventData.Pages[0].Graphic;

            // Validate the graphic index to ensure it�s within range
            if (gfxIndex <= 0 || gfxIndex > GameState.NumCharacters)
                return;

            // Get animation details (frame index and columns) from the event
            int frameIndex = eventData.Pages[0].GraphicX; // Example frame index
            int columns = 4;
            var gfxInfo = GetGfxInfo(Path.Combine(DataPath.Characters, gfxIndex.ToString()));
            if (gfxInfo == null)
            {
                // Handle the case where the graphic information is not found
                return;
            }

            // Calculate the frame size (assuming square frames for simplicity)
            int frameWidth = gfxInfo.Width / columns;
            int frameHeight = frameWidth; // Adjust if non-square frames

            // Calculate the source rectangle for the current frame
            int column = frameIndex % columns;
            int row = frameIndex / columns;
            var sourceRect = new Rectangle(column * frameWidth, row * frameHeight, frameWidth, frameHeight);

            // Define the position on the map where the graphic will be drawn
            var position = new Vector2(x, y);

            string argPath = Path.Combine(DataPath.Characters, gfxIndex.ToString());
            RenderTexture(ref argPath, (int) Math.Round(position.X), (int) Math.Round(position.Y), sourceRect.X,
                sourceRect.Y,
                frameWidth, frameHeight, sourceRect.Width, sourceRect.Height);
        }

        private static void RenderTilesetGraphic(Type.Event eventData, int x, int y)
        {
            int gfxIndex = eventData.Pages[0].Graphic;

            if (gfxIndex > 0 && gfxIndex <= GameState.NumTileSets)
            {
                // Define source rectangle from tileset graphics
                var srcRect = new Rectangle(eventData.Pages[0].GraphicX * 32, eventData.Pages[0].GraphicY * 32,
                    eventData.Pages[0].GraphicX2 * 32, eventData.Pages[0].GraphicY2 * 32);

                // Adjust position if the tile is larger than 32x32
                if (srcRect.Height > 32)
                    y -= GameState.SizeY;

                // Define destination rectangle
                var destRect = new Rectangle(x, y, srcRect.Width, srcRect.Height);

                string argPath = Path.Combine(DataPath.Tilesets, gfxIndex.ToString());
                RenderTexture(ref argPath, destRect.X, destRect.Y, srcRect.X, srcRect.Y, destRect.Width,
                    destRect.Height,
                    srcRect.Width, srcRect.Height);
            }
            else
            {
                // Draw fallback outline if the tileset graphic is invalid
                DrawOutlineRectangle(x, y, GameState.SizeX, GameState.SizeY, Color.Blue, 0.6f);
            }
        }

        public static void DrawEvent(int id) // draw on map, outside the editor
        {
            int x;
            int y;
            int width;
            int height;
            var sRect = default(Rectangle);
            var anim = default(int);
            var spritetop = default(int);

            try
            {
                if (Data.MapEvents[id].Visible == false)
                {
                    return;
                }

                switch (Data.MapEvents[id].GraphicType)
                {
                    case 0:
                    {
                        return;
                    }
                    case 1:
                    {
                        if (Data.MapEvents[id].Graphic <= 0 |
                            Data.MapEvents[id].Graphic > GameState.NumCharacters)
                            return;

                        anim = Data.MapEvents[id].Steps;

                        // Set the left
                        switch (Data.MapEvents[id].ShowDir)
                        {
                            case (int) Direction.Up:
                            {
                                spritetop = 3;
                                break;
                            }
                            case (int) Direction.Right:
                            {
                                spritetop = 2;
                                break;
                            }
                            case (int) Direction.Down:
                            {
                                spritetop = 0;
                                break;
                            }
                            case (int) Direction.Left:
                            {
                                spritetop = 1;
                                break;
                            }
                        }

                        var gfxInfo = GetGfxInfo(Path.Combine(DataPath.Characters,
                            Data.MapEvents[id].Graphic.ToString()));

                        if (gfxInfo == null)
                        {
                            // Handle the case where gfxInfo is null
                            return;
                        }

                        height = (int) Math.Round((double) gfxInfo.Height / 4d);
                        width = (int) Math.Round((double) gfxInfo.Width / 4d);
                        sRect = new Rectangle((int) Math.Round((double) anim * width),
                            (int) Math.Round((double) spritetop * height), width, height);

                        // Calculate the X
                        x = (int) Math.Round(Data.MapEvents[id].X -
                                             (width - 32d) / 2d);

                        // Is the player's height more than 32..?
                        if ((gfxInfo.Height / 4) > 32)
                        {
                            // Create a 32 pixel offset for larger sprites
                            y = (int) Math.Round(Data.MapEvents[id].Y - (height - 32d));
                        }
                        else
                        {
                            // Proceed as normal
                            y = Data.MapEvents[id].Y;
                        }

                        // render the actual sprite
                        DrawCharacterSprite(Data.MapEvents[id].Graphic, x, y, sRect);
                        break;
                    }
                    case 2:
                    {
                        if (Data.MapEvents[id].Graphic < 1 |
                            Data.MapEvents[id].Graphic > GameState.NumTileSets)
                            return;

                        if (Data.MapEvents[id].GraphicY2 > 0 | Data.MapEvents[id].GraphicX2 > 0)
                        {
                            sRect.X = Data.MapEvents[id].GraphicX * 32;
                            sRect.Y = Data.MapEvents[id].GraphicY * 32;
                            sRect.Width = Data.MapEvents[id].GraphicX2 * 32;
                            sRect.Height = Data.MapEvents[id].GraphicY2 * 32;
                        }
                        else
                        {
                            sRect.X = Data.MapEvents[id].GraphicY * 32;
                            sRect.Height = sRect.Top + 32;
                            sRect.Y = Data.MapEvents[id].GraphicX * 32;
                            sRect.Width = sRect.Left + 32;
                        }

                        x = Data.MapEvents[id].X * 32;
                        y = Data.MapEvents[id].Y * 32;
                        x = (int) Math.Round(x - (sRect.Right - sRect.Left) / 2d);
                        y = y - (sRect.Bottom - sRect.Top) + 32;

                        if (Data.MapEvents[id].GraphicY2 > 1)
                        {
                            string argPath = Path.Combine(DataPath.Tilesets,
                                Data.MapEvents[id].Graphic.ToString());
                            RenderTexture(ref argPath,
                                GameLogic.ConvertMapX(Data.MapEvents[id].X),
                                GameLogic.ConvertMapY(Data.MapEvents[id].Y) - GameState.SizeY,
                                sRect.Left, sRect.Top, sRect.Width, sRect.Height);
                        }
                        else
                        {
                            string argPath1 = Path.Combine(DataPath.Tilesets,
                                Data.MapEvents[id].Graphic.ToString());
                            RenderTexture(ref argPath1,
                                GameLogic.ConvertMapX(Data.MapEvents[id].X),
                                GameLogic.ConvertMapY(Data.MapEvents[id].Y), sRect.Left,
                                sRect.Top,
                                sRect.Width, sRect.Height);
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void Render_Game()
        {
            int x;
            int y;
            int i;

            if (GameState.GettingMap)
                return;

            GameLogic.UpdateCamera();
            // Auto-cancel target if player is off the current camera viewport (native world rect)
            CancelTargetIfOffCamera();

            if (GameState.NumPanoramas > 0 & Data.MyMap.Panorama > 0)
            {
                Map.DrawPanorama(Data.MyMap.Panorama);
            }

            if (GameState.NumParallax > 0 & Data.MyMap.Parallax > 0)
            {
                Map.DrawParallax(Data.MyMap.Parallax);
            }

            // Draw lower tiles
            if (GameState.NumTileSets > 0)
            {
                var loopTo = (int) Math.Round(GameState.TileView.Right + 1d);
                for (x = (int) Math.Round(GameState.TileView.Left - 1d); x < loopTo; x++)
                {
                    var loopTo1 = (int) Math.Round(GameState.TileView.Bottom + 1d);
                    for (y = (int) Math.Round(GameState.TileView.Top - 1d); y < loopTo1; y++)
                    {
                        if (GameLogic.IsValidMapPoint(x, y))
                        {
                            Map.DrawMapGroundTile(x, y);
                        }
                    }
                }
            }

            // events
            if (GameState.MyEditorType != EditorType.Map)
            {
                if (GameState.CurrentEvents > 0 & GameState.CurrentEvents <= Data.MyMap.EventCount)
                {
                    var loopTo2 = Information.UBound(Data.MapEvents);
                    for (i = 0; i <= loopTo2; i++)
                    {
                        if (Data.MapEvents[i].Position == 0)
                        {
                            DrawEvent(i);
                        }
                    }
                }
            }

            // blood
            for (i = 0; i < byte.MaxValue; i++)
                DrawBlood(i);

            // Draw out the items
            if (GameState.NumItems > 0)
            {
                for (i = 0; i < Constant.MaxMapItems; i++)
                {
                    DrawMapItem(i);
                }
            }

            // draw animations
            if (GameState.NumAnimations > 0)
            {
                for (i = 0; i < byte.MaxValue; i++)
                {
                    if (Animation.AnimInstance[i].Used[0])
                    {
                        Animation.Draw(i, 0);
                    }                
                }
            }

            // Y-based render. Renders Players, Npcs and Resources based on Y-axis.
            var loopTo3 = (int) Data.MyMap.MaxY;
            for (y = 0; y < loopTo3; y++)
            {
                if (GameState.NumCharacters > 0)
                {
                    // Npcs
                    for (i = 0; i < Constant.MaxMapNpcs; i++)
                    {
                        if (Math.Floor((decimal) Data.MyMapNpc[i].Y / 32) == y)
                        {
                            DrawNpc(i);
                        }
                    }

                    // Players
                    for (i = 0; i < Constant.MaxPlayers; i++)
                    {
                        if (IsPlaying(i) & GetPlayerMap(i) == GetPlayerMap(GameState.MyIndex))
                        {
                            if (GetPlayerY(i) == y)
                            {
                                DrawPlayer(i);
                            }
                        }
                    }

                    if (GameState.MyEditorType != EditorType.Map)
                    {
                        if (GameState.CurrentEvents > 0 & GameState.CurrentEvents <= Data.MyMap.EventCount)
                        {
                            var loopTo4 = Information.UBound(Data.MapEvents);
                            for (i = 0; i <= loopTo4; i++)
                            {
                                if (Data.MapEvents?[i].Position == 1)
                                {
                                    if (Math.Floor((decimal) Data.MapEvents[i].Y / 32) == y)
                                    {
                                        DrawEvent(i);
                                    }
                                }
                            }
                        }
                    }

                    // Draw the target icon
                    if (GameState.MyTarget >= 0)
                    {
                        switch (GameState.MyTargetType)
                        {
                            case (int) TargetType.Player:
                                if (IsPlaying(GameState.MyTarget))
                                {
                                    if (Data.Player[GameState.MyTarget].Map ==
                                        Data.Player[GameState.MyIndex].Map)
                                    {
                                        if (Data.Player[GameState.MyTarget].Sprite > 0)
                                        {
                                            // Draw the target icon for the player
                                            DrawTarget(
                                                Data.Player[GameState.MyTarget].X - 16,
                                                Data.Player[GameState.MyTarget].Y);
                                        }
                                    }
                                }

                                break;

                            case (int) TargetType.Npc:
                                DrawTarget(
                                    Data.MyMapNpc[GameState.MyTarget].X - 16,
                                    Data.MyMapNpc[GameState.MyTarget].Y);
                                break;
                        }
                    }

                    for (i = 0; i < Constant.MaxPlayers; i++)
                    {
                        if (IsPlaying(i))
                        {
                            if (Data.Player[i].Map == Data.Player[GameState.MyIndex].Map)
                            {
                                if (Data.Player[i].Sprite == 0)
                                    continue;

                                if (GameState.CurXGame == Data.Player[i].X & GameState.CurYGame == Data.Player[i].Y)
                                {
                                    if (GameState.MyTargetType == (int) TargetType.Player & GameState.MyTarget == i)
                                    {
                                    }

                                    else
                                    {
                                        DrawHover(Data.Player[i].X * 32 - 16,
                                            Data.Player[i].Y * 32 + Data.Player[i].Y);
                                    }
                                }
                            }
                        }
                    }
                }

                // Resources
                if (GameState.NumResources > 0)
                {
                    if (GameState.ResourcesInit)
                    {
                        if (GameState.ResourceIndex > 0)
                        {
                            var loopTo5 = GameState.ResourceIndex;
                            for (i = 0; i < loopTo5; i++)
                            {
                                if (Data.MyMapResource[i].Y == y)
                                {
                                    MapResource.DrawMapResource(i);
                                }
                            }
                        }
                    }
                }
            }

            // animations
            if (GameState.NumAnimations > 0)
            {
                for (i = 0; i < byte.MaxValue; i++)
                {
                    if (Animation.AnimInstance[i].Used[1])
                        {
                            Animation.Draw(i, 1);
                        }
                    }              
            }

            if (GameState.NumProjectiles > 0)
            {
                for (i = 0; i < Constant.MaxProjectiles; i++)
                {
                    if (Data.MapProjectile[Data.Player[GameState.MyIndex].Map, i].ProjectileNum >= 0)
                    {
                        Projectile.DrawProjectile(i);
                    }
                }
            }

            if (GameState.CurrentEvents > 0 & GameState.CurrentEvents <= Data.MyMap.EventCount)
            {
                var loopTo6 = GameState.CurrentEvents;
                for (i = 0; i < loopTo6; i++)
                {
                    if (Data.MapEvents[i].Position == 2)
                    {
                        DrawEvent(i);
                    }
                }
            }

            if (GameState.NumTileSets > 0)
            {
                var loopTo7 = (int) Math.Round(GameState.TileView.Right + 1d);
                for (x = (int) Math.Round(GameState.TileView.Left - 1d); x < loopTo7; x++)
                {
                    var loopTo8 = (int) Math.Round(GameState.TileView.Bottom + 1d);
                    for (y = (int) Math.Round(GameState.TileView.Top - 1d); y < loopTo8; y++)
                    {
                        if (GameLogic.IsValidMapPoint(x, y))
                        {
                            Map.DrawMapRoofTile(x, y);
                        }
                    }
                }
            }

            Map.DrawWeather();
            Map.DrawThunderEffect();
            Map.DrawMapTint();

            // Draw out a square at mouse cursor
            if (Conversions.ToInteger(GameState.MapGrid) == 1 & GameState.MyEditorType == EditorType.Map)
            {
                DrawGrid();
            }

            for (i = 0; i < Constant.MaxPlayers; i++)
            {
                if (IsPlaying(i) & GetPlayerMap(i) == GetPlayerMap(GameState.MyIndex))
                {
                    TextRenderer.DrawPlayerName(i);
                }
            }

            if (GameState.MyEditorType != EditorType.Map)
            {
                if (GameState.CurrentEvents > 0 && Data.MyMap.EventCount >= GameState.CurrentEvents)
                {
                    var loopTo9 = GameState.CurrentEvents;
                    for (i = 0; i < loopTo9; i++)
                    {
                        if (Data.MapEvents?[i].Visible == true)
                        {
                            if (Data.MapEvents[i].ShowName == 1)
                            {
                                TextRenderer.DrawEventName(i);
                            }
                        }
                    }
                }
            }

            for (i = 0; i < Constant.MaxMapNpcs; i++)
            {
                TextRenderer.DrawNpcName(i);
            }

            Map.DrawFog();
            Map.DrawPicture();

            for (i = 0; i < byte.MaxValue; i++)
                TextRenderer.DrawActionMsg(i);

            if (GameState.MyEditorType == EditorType.Map)
            {
                UpdateDirBlock();
                UpdateMapAttributes();
            }

            for (i = 0; i < byte.MaxValue; i++)
            {
                if (Data.ChatBubble[i].Active)
                {
                    DrawChatBubble(i);
                }
            }

            if (GameState.Bfps)
            {
                string fps = "FPS: " + GetFps();
                TextRenderer.RenderText(fps, (int) Math.Round(GameState.Camera.Left - 24d),
                    (int) Math.Round(GameState.Camera.Top + 60d), Color.Yellow, Color.Black);
            }

            // draw cursor, player X and Y locations
            if (GameState.BLoc)
            {
                string cur = "Cur X: " + GameState.CurXGame + " Y: " + GameState.CurYGame;
                string loc = "loc X: " + GetPlayerX(GameState.MyIndex) + " Y: " + GetPlayerY(GameState.MyIndex);
                string map = " (Map #" + GetPlayerMap(GameState.MyIndex) + ")";

                TextRenderer.RenderText(cur, (int) Math.Round(GameState.DrawLocX), (int) Math.Round(GameState.DrawLocY + 105f),
                    Color.Yellow, Color.Black);
                TextRenderer.RenderText(loc, (int) Math.Round(GameState.DrawLocX), (int) Math.Round(GameState.DrawLocY + 120f),
                    Color.Yellow, Color.Black);
                TextRenderer.RenderText(map, (int) Math.Round(GameState.DrawLocX), (int) Math.Round(GameState.DrawLocY + 135f),
                    Color.Yellow, Color.Black);
            }
            
            if (GameState.MyEditorType == EditorType.Map)
            {
                if (GameState.MapEditorTab == (int)MapEditorTab.Events)
                {
                    DrawEvents();
                }
            }

            DrawBars();
            Map.DrawMapFade();
        }

    // Cancels the current target if the distance between PLAYER and TARGET exceeds the visible camera view.
        private static void CancelTargetIfOffCamera()
        {
            // Only handle Player and NPC targets
            if (GameState.MyTargetType == (int)TargetType.None)
                return;

            int t = GameState.MyTarget;
            if (t < 0)
                return;

            // Compute the actually visible world rect, factoring in zoom.
            // Camera rectangle is in world pixels for the full render target size.
            // When zoomed in (>1), the visible world area is smaller by 1/zoom.
            int camLeftBase = (int)Math.Floor(GameState.Camera.Left);
            int camTopBase = (int)Math.Floor(GameState.Camera.Top);
            int camWidthBase = GameState.ResolutionWidth;
            int camHeightBase = GameState.ResolutionHeight;

            float zoom = GameState.CameraZoom <= 0 ? 1.0f : GameState.CameraZoom;
            int visWidth = (int)Math.Round(camWidthBase / zoom);
            int visHeight = (int)Math.Round(camHeightBase / zoom);

            // Center the visible rect around the camera center
            int camCenterX = camLeftBase + camWidthBase / 2;
            int camCenterY = camTopBase + camHeightBase / 2;
            int camLeft = camCenterX - visWidth / 2;
            int camTop = camCenterY - visHeight / 2;
            int camRight = camLeft + visWidth;
            int camBottom = camTop + visHeight;

            // Compute max allowed deltas based on visible size
            int maxDx = visWidth / 2;
            int maxDy = visHeight / 2;

            bool shouldClear = false;
            int tileX = -1;
            int tileY = -1;

            if (GameState.MyTargetType == (int)TargetType.Player)
            {
                if (!IsPlaying(t) || GetPlayerMap(t) != GetPlayerMap(GameState.MyIndex))
                {
                    shouldClear = true;
                }
                else
                {
                    // Compare distance between player and target against the view half-size (zoom-aware)
                    int px = GetPlayerRawX(GameState.MyIndex) + GameState.SizeX / 2;
                    int py = GetPlayerRawY(GameState.MyIndex) + GameState.SizeY / 2;
                    int tx = GetPlayerRawX(t) + GameState.SizeX / 2;
                    int ty = GetPlayerRawY(t) + GameState.SizeY / 2;
                    if (Math.Abs(tx - px) >= maxDx || Math.Abs(ty - py) >= maxDy)
                    {
                        shouldClear = true;
                        tileX = GetPlayerX(t);
                        tileY = GetPlayerY(t);
                    }
                }
            }
            else if (GameState.MyTargetType == (int)TargetType.Npc)
            {
                int n = t;
                if (n < 0 || n >= Data.MyMapNpc.Length || Data.MyMapNpc[n].Num < 0)
                {
                    shouldClear = true;
                }
                else
                {
                    int px = GetPlayerRawX(GameState.MyIndex) + GameState.SizeX / 2;
                    int py = GetPlayerRawY(GameState.MyIndex) + GameState.SizeY / 2;
                    int tx = Data.MyMapNpc[n].X + GameState.SizeX / 2;
                    int ty = Data.MyMapNpc[n].Y + GameState.SizeY / 2;
                    if (Math.Abs(tx - px) >= maxDx || Math.Abs(ty - py) >= maxDy)
                    {
                        shouldClear = true;
                        tileX = (int)Math.Floor(Data.MyMapNpc[n].X / 32d);
                        tileY = (int)Math.Floor(Data.MyMapNpc[n].Y / 32d);
                    }
                }
            }
            else
            {
                // Unsupported target types: ignore
                return;
            }

            if (!shouldClear)
                return;

            // Clear locally first
            GameState.MyTarget = -1;
            GameState.MyTargetType = 0;

            // Notify server if we have the tile
            if (tileX >= 0 && tileY >= 0)
            {
                Sender.PlayerSearch(tileX, tileY, 0);
            }
        }

        public static void UpdateMapAttributes()
        {
            if (GameState.MapEditorTab == (int) MapEditorTab.Attributes)
            {
                TextRenderer.DrawMapAttributes();
            }
        }

        public static void UpdateDirBlock()
        {
            int x;
            int y;

            if (GameState.MapEditorTab == (int) MapEditorTab.Directions)
            {
                var loopTo10 = (int) Math.Round(GameState.TileView.Right + 1d);
                for (x = (int) Math.Round(GameState.TileView.Left - 1d); x < loopTo10; x++)
                {
                    var loopTo11 = (int) Math.Round(GameState.TileView.Bottom + 1d);
                    for (y = (int) Math.Round(GameState.TileView.Top - 1d); y < loopTo11; y++)
                    {
                        if (GameLogic.IsValidMapPoint(x, y))
                        {
                            DrawDirections(x, y);
                        }
                    }
                }
            }
        }
    }
}