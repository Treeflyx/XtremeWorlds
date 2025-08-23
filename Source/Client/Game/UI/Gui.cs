using System.Collections.Concurrent;
using System.Diagnostics;
using Client.Game.UI.Controls;
using Client.Game.UI.Windows;
using Core.Configurations;
using Core.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static Core.Globals.Command;
using Type = Core.Globals.Type;

namespace Client.Game.UI;

public class Gui
{
    // GUI
    public static ConcurrentDictionary<long, Window> Windows { get; private set; } = new();
    public static Window? ActiveWindow { get; set; }

    // GUi parts
    public static Type.ControlPart DragBox;

    // Used for automatically the zOrder
    public static int ZOrderWin;
    public static int ZOrderCon;

    // Declare a timer to control when dragging can begin
    private static readonly Stopwatch DragTimer = new();
    private const double DragInterval = 100d; // Set the interval in milliseconds to start dragging
    private static bool _canDrag; // Flag to control when dragging is allowed
    private static bool _isDragging;

    public static void UpdateZOrder(long windowIndex, bool forced = false)
    {
        var window = Windows[windowIndex];

        if (!forced)
        {
            if (window.ZChange == 0)
            {
                return;
            }
        }

        if (window.ZOrder == Windows.Count - 1)
        {
            return;
        }

        var oldZOrder = window.ZOrder;

        for (var i = 1; i <= Windows.Count; i++)
        {
            if (Windows[i].ZOrder > oldZOrder)
            {
                Windows[i].ZOrder--;
            }
        }

        window.ZOrder = Windows.Count - 1;
    }

    public static void Combobox_AddItem(int windowIndex, int controlIndex, string text)
    {
        if (Windows[windowIndex].Controls[controlIndex] is ComboBox comboBox)
        {
            comboBox.Items.Add(text);
        }
    }

    // Safe helpers to avoid null/KeyNotFound when UI isn't fully initialized
    public static bool TryGetWindow(string windowName, out Window? window)
    {
        window = GetWindowByName(windowName);
        return window is not null;
    }

    public static bool TryGetControl(string windowName, string controlName, out Control? control)
    {
        control = null;
        var window = GetWindowByName(windowName);
        if (window is null)
        {
            return false;
        }

        try
        {
            control = window.GetChild(controlName);
            return control is not null;
        }
        catch
        {
            return false;
        }
    }

    public static int CreateWindow(string name, string caption, Font font, int zOrder, int left, int top, int width, int height, int icon, bool visible = true, int xOffset = 0, int yOffset = 0, Design designNorm = Design.None, Design designHover = Design.None, Design designMousedown = Design.None, int imageNorm = 0, int imageHover = 0, int imageMousedown = 0, Action? callbackNorm = null, Action? callbackHover = null, Action? callbackMousemove = null, Action? callbackMousedown = null, Action? callbackDblclick = null, Action? onDraw = null, bool canDrag = true, byte zChange = 1, bool clickThrough = false)
    {
        var stateCount = Enum.GetValues<ControlState>().Length;
        var design = new List<Design>(Enumerable.Repeat((Design) 0, stateCount));
        var image = new List<int>(Enumerable.Repeat(0, stateCount));
        var callback = new List<Action?>(Enumerable.Repeat((Action) null, stateCount));

        // Assign specific values for each state
        design[(int) ControlState.Normal] = designNorm;
        design[(int) ControlState.Hover] = designHover;
        design[(int) ControlState.MouseDown] = designMousedown;

        image[(int) ControlState.Normal] = imageNorm;
        image[(int) ControlState.Hover] = imageHover;
        image[(int) ControlState.MouseDown] = imageMousedown;

        callback[(int) ControlState.Normal] = callbackNorm;
        callback[(int) ControlState.Hover] = callbackHover;
        callback[(int) ControlState.MouseDown] = callbackMousedown;
        callback[(int) ControlState.MouseMove] = callbackMousemove;
        callback[(int) ControlState.DoubleClick] = callbackDblclick;

        // Create a new instance of Window and populate it
        var window = new Window
        {
            Name = name,
            X = left,
            Y = top,
            InitialX = left,
            InitialY = top,
            Width = width,
            Height = height,
            Visible = visible,
            CanDrag = canDrag,
            Font = font,
            Text = caption,
            XOffset = xOffset,
            YOffset = yOffset,
            Icon = icon,
            ZChange = zChange,
            ZOrder = zOrder,
            OnDraw = onDraw,
            ClickThrough = clickThrough,
            Design = design,
            Image = image,
            CallBack = callback
        };

        Windows.TryAdd(Windows.Count + 1, window);

        if (visible)
        {
            ActiveWindow = window;
        }

        return Windows.Count;
    }

    public static void CreateTextbox(int windowIndex, string name, int left, int top, int width, int height, string text = "", Font font = Font.Georgia, Alignment align = Alignment.Left, bool visible = true, int alpha = 255, bool isActive = true, int xOffset = 0, int yOffset = 0, int? imageNorm = null, int? imageHover = null, int? imageMousedown = null, Design designNorm = Design.None, Design designHover = Design.None, Design designMousedown = Design.None, bool censor = false, int icon = 0, Action? callbackNorm = null, Action? callbackHover = null, Action? callbackMousedown = null, Action? callbackMousemove = null, Action? callbackDblclick = null, Action? callbackEnter = null)
    {
        var stateCount = Enum.GetValues<ControlState>().Length;

        var callbacks = new List<Action?>(Enumerable.Repeat((Action) null, stateCount).ToList());

        callbacks[(int) ControlState.Normal] = callbackNorm;
        callbacks[(int) ControlState.Hover] = callbackHover;
        callbacks[(int) ControlState.MouseDown] = callbackMousedown;
        callbacks[(int) ControlState.MouseMove] = callbackMousemove;
        callbacks[(int) ControlState.DoubleClick] = callbackDblclick;
        callbacks[(int) ControlState.FocusEnter] = callbackEnter;

        if (!Windows.TryGetValue(windowIndex, out var window))
        {
            throw new UIException($"{windowIndex} is not a valid window index.");
        }

        var textBox = new TextBox
        {
            Name = name,
            X = left,
            Y = top,
            Width = width,
            Height = height,
            Visible = visible,
            Text = text,
            Align = align,
            Font = font,
            Color = Color.White,
            Alpha = alpha,
            XOffset = xOffset,
            YOffset = yOffset,
            ZOrder = window.Controls.Count,
            Censor = censor,
            Icon = icon,
            Design = designNorm,
            DesignHover = designHover,
            DesignMouseDown = designMousedown,
            Image = imageNorm,
            ImageHover = imageHover,
            ImageMouseDown = imageMousedown,
            CallBack = callbacks
        };

        window.Controls.Add(textBox);

        if (isActive)
        {
            window.ActiveControl = textBox;
        }

        ZOrderCon++;
    }

    public static void CreatePictureBox(int windowIndex, string name, int left, int top, int width, int height, bool visible = true, int alpha = 255, int? imageNorm = null, int? imageHover = null, int? imageMousedown = null, Design designNorm = Design.None, Design? designHover = null, Design? designMousedown = null, string texturePath = "", Action? callbackNorm = null, Action? callbackHover = null, Action? callbackMousedown = null, Action? callbackMousemove = null, Action? callbackDblclick = null, Action? onDraw = null)
    {
        if (!Windows.TryGetValue(windowIndex, out var window))
        {
            throw new UIException($"{windowIndex} is not a valid window index.");
        }

        var stateCount = Enum.GetValues<ControlState>().Length;
        var texture = new List<string>(Enumerable.Repeat(string.Empty, stateCount));
        var callback = new List<Action?>(Enumerable.Repeat((Action) null, stateCount));

        if (string.IsNullOrEmpty(texturePath))
        {
            texturePath = DataPath.Gui;
        }

        texture[(int) ControlState.Normal] = texturePath;
        texture[(int) ControlState.Hover] = texturePath;
        texture[(int) ControlState.MouseDown] = texturePath;

        callback[(int) ControlState.Normal] = callbackNorm;
        callback[(int) ControlState.Hover] = callbackHover;
        callback[(int) ControlState.MouseDown] = callbackMousedown;
        callback[(int) ControlState.MouseMove] = callbackMousemove;
        callback[(int) ControlState.DoubleClick] = callbackDblclick;

        if (imageNorm == 0) imageNorm = null;
        if (imageHover == 0) imageHover = null;
        if (imageMousedown == 0) imageMousedown = null;

        var pictureBox = new PictureBox
        {
            Name = name,
            X = left,
            Y = top,
            Width = width,
            Height = height,
            Visible = visible,
            Color = Color.White,
            Alpha = alpha,
            ZOrder = ZOrderCon,
            OnDraw = onDraw,
            Design = designNorm,
            DesignHover = designHover ?? designNorm,
            DesignMouseDown = designMousedown ?? designNorm,
            Image = imageNorm,
            ImageHover = imageHover,
            ImageMouseDown = imageMousedown,
            Texture = texture,
            CallBack = callback
        };

        window.Controls.Add(pictureBox);

        ZOrderCon++;
    }

    public static void CreateButton(int windowIndex, string name, int left, int top, int width, int height, string text = "", Font font = Font.Georgia, int icon = 0, int? imageNorm = null, int? imageHover = null, int? imageMousedown = null, bool visible = true, Design designNorm = Design.None, Design? designHover = null, Design? designMousedown = null, Action? callbackNorm = null, Action? callbackHover = null, Action? callbackMousedown = null, Action? callbackMousemove = null, Action? callbackDblclick = null, int xOffset = 0, int yOffset = 0, string tooltip = "")
    {
        if (!Windows.TryGetValue(windowIndex, out var window))
        {
            throw new UIException($"{windowIndex} is not a valid window index.");
        }

        var stateCount = Enum.GetValues<ControlState>().Length;
        var texture = new List<string>(Enumerable.Repeat(DataPath.Designs, stateCount).ToList());
        var callback = new List<Action?>(Enumerable.Repeat((Action) null, stateCount).ToList());

        texture[(int) ControlState.Normal] = DataPath.Gui;
        texture[(int) ControlState.Hover] = DataPath.Gui;
        texture[(int) ControlState.MouseDown] = DataPath.Gui;

        callback[(int) ControlState.Normal] = callbackNorm;
        callback[(int) ControlState.Hover] = callbackHover;
        callback[(int) ControlState.MouseDown] = callbackMousedown;
        callback[(int) ControlState.MouseMove] = callbackMousemove;
        callback[(int) ControlState.DoubleClick] = callbackDblclick;

        if (imageNorm == 0) imageNorm = null;
        if (imageHover == 0) imageHover = null;
        if (imageMousedown == 0) imageMousedown = null;

        var button = new Button
        {
            Name = name,
            X = left,
            Y = top,
            Width = width,
            Height = height,
            Visible = visible,
            Text = text,
            Font = font,
            XOffset = xOffset,
            YOffset = yOffset,
            ZOrder = ZOrderCon,
            Tooltip = tooltip,
            Icon = icon,
            Design = designNorm,
            DesignHover = designHover ?? designNorm,
            DesignMouseDown = designMousedown ?? designNorm,
            Image = imageNorm,
            ImageHover = imageHover,
            ImageMouseDown = imageMousedown,
            Texture = texture,
            CallBack = callback
        };

        window.Controls.Add(button);

        ZOrderCon++;
    }

    public static void CreateLabel(int windowIndex, string name, int left, int top, int width, int height, string text, Font font, Alignment align = Alignment.Left, bool visible = true, bool clickThrough = false, bool censor = false, Action? callbackNorm = null, Action? callbackHover = null, Action? callbackMousedown = null, Action? callbackMousemove = null, Action? callbackDblclick = null, bool enabled = false)
    {
        if (!Windows.TryGetValue(windowIndex, out var window))
        {
            throw new UIException($"{windowIndex} is not a valid window index.");
        }

        var controlStateCount = Enum.GetValues<ControlState>().Length;
        var callbackLabel = new List<Action?>(Enumerable.Repeat((Action) null, controlStateCount).ToList());

        callbackLabel[(int) ControlState.Normal] = callbackNorm;
        callbackLabel[(int) ControlState.Hover] = callbackHover;
        callbackLabel[(int) ControlState.MouseDown] = callbackMousedown;
        callbackLabel[(int) ControlState.MouseMove] = callbackMousemove;
        callbackLabel[(int) ControlState.DoubleClick] = callbackDblclick;

        var label = new Label
        {
            Name = name,
            X = left,
            Y = top,
            Width = width,
            Height = height,
            Visible = visible,
            Text = text,
            Align = align,
            Font = font,
            ZOrder = ZOrderCon,
            Enabled = enabled,
            CallBack = callbackLabel
        };

        window.Controls.Add(label);

        ZOrderCon++;
    }

    public static void CreateCheckBox(int windowIndex, string name, int left, int top, int width, int height = 15, int value = 0, string text = "", Font font = Font.Georgia, bool visible = true, Design theDesign = Design.None, int group = 0, Action? callbackNorm = null, Action? callbackHover = null, Action? callbackMousedown = null, Action? callbackMousemove = null, Action? callbackDblclick = null)
    {
        if (!Windows.TryGetValue(windowIndex, out var window))
        {
            throw new UIException($"{windowIndex} is not a valid window index.");
        }

        var stateCount = Enum.GetValues<ControlState>().Length;
        var texture = new List<string>(Enumerable.Repeat(DataPath.Designs, stateCount).ToList());
        var callback = new List<Action?>(Enumerable.Repeat((Action) null, stateCount).ToList());

        texture[0] = DataPath.Gui;

        callback[(int) ControlState.Normal] = callbackNorm;
        callback[(int) ControlState.Hover] = callbackHover;
        callback[(int) ControlState.MouseDown] = callbackMousedown;
        callback[(int) ControlState.MouseMove] = callbackMousemove;
        callback[(int) ControlState.DoubleClick] = callbackDblclick;

        var checkBox = new CheckBox
        {
            Name = name,
            X = left,
            Y = top,
            Width = width,
            Height = height,
            Visible = visible,
            Value = value,
            Text = text,
            Font = font,
            ZOrder = ZOrderCon,
            Group = group,
            Design = theDesign,
            Texture = texture,
            CallBack = callback
        };

        window.Controls.Add(checkBox);

        ZOrderCon++;
    }

    public static void CreateComboBox(int windowIndex, string name, int left, int top, int width, int height, Design design)
    {
        var controlStateCount = Enum.GetValues<ControlState>().Length;
        var texture = new List<string>(Enumerable.Repeat(DataPath.Gui, controlStateCount).ToList());
        var callback = new List<Action?>(Enumerable.Repeat((Action) null, controlStateCount).ToList());

        texture[0] = DataPath.Gui;

        if (!Windows.TryGetValue(windowIndex, out var window))
        {
            return;
        }

        var comboBox = new ComboBox
        {
            Name = name,
            X = left,
            Y = top,
            Width = width,
            Height = height,
            Design = design,
            Texture = texture,
            CallBack = callback
        };

        window.Controls.Add(comboBox);

        ZOrderCon++;
    }

    public static int GetWindowIndex(string windowName)
    {
        foreach (var kvp in Windows)
        {
            if (string.Equals(kvp.Value.Name, windowName, StringComparison.CurrentCultureIgnoreCase))
            {
                return (int)kvp.Key;
            }
        }

        return 0;
    }

    public static Window? GetWindowByName(string windowName)
    {
        var windowIndex = GetWindowIndex(windowName);
        if (windowIndex == 0)
        {
            return null;
        }

        return Windows[windowIndex];
    }

    public static int GetControlIndex(string window, string controlName)
    {
        var index = GetWindowIndex(window);

        for (var i = 0; i <= Windows[index].Controls.Count - 1; i++)
        {
            if (string.Equals(Windows[index].Controls[i].Name, controlName, StringComparison.CurrentCultureIgnoreCase))
            {
                return i;
            }
        }

        return 0;
    }

    public static bool SetActiveControl(int windowIndex, int controlIndex)
    {
        var window = Windows[windowIndex];

        return SetActiveControl(window, controlIndex);
    }

    public static bool SetActiveControl(Window window, string controlName)
    {
        var controlIndex = GetControlIndex(window.Name, controlName);

        switch (window.Controls[controlIndex])
        {
            case TextBox:
                window.LastControl = window.ActiveControl;
                window.ActiveControl = window.Controls[controlIndex];
                return true;
        }

        return false;
    }

    public static bool SetActiveControl(Window window, int controlIndex)
    {
        switch (window.Controls[controlIndex])
        {
            case TextBox:
                window.LastControl = window.ActiveControl;
                window.ActiveControl = window.Controls[controlIndex];
                return true;
        }

        return false;
    }

    public static void CentralizeWindow(int windowIndex)
    {
        var window = Windows[windowIndex];

        window.X = (int) Math.Round(GameState.ResolutionWidth / 2d - window.Width / 2d);
        window.Y = (int) Math.Round(GameState.ResolutionHeight / 2d - window.Height / 2d);
        window.InitialX = window.X;
        window.InitialY = window.Y;
    }

    public static void HideWindows()
    {
        for (var i = 1; i <= Windows.Count - 1; i++)
        {
            HideWindow(i);
        }
    }

    public static void ShowWindow(string windowName, bool forced = false, bool resetPosition = true)
    {
        var index = GetWindowIndex(windowName);
        if (index == 0)
        {
            try
            {
                // Try to lazily load the layout if it's not already loaded via the skin script
                WindowLoader.FromLayout(windowName);
                index = GetWindowIndex(windowName);
            }
            catch
            {
                // ignore; will no-op below
            }
        }

        ShowWindow(index, forced, resetPosition);
    }

    public static void ShowWindow(int windowIndex, bool forced = false, bool resetPosition = true)
    {
        if (windowIndex == 0)
        {
            return;
        }

        Windows[windowIndex].Visible = true;

        if (forced)
        {
            UpdateZOrder(windowIndex, forced);
        }
        else if (Windows[windowIndex].ZChange != 0)
        {
            UpdateZOrder(windowIndex);
        }

        ActiveWindow = Windows[windowIndex];
        if (!resetPosition)
        {
            return;
        }

        // If the window was initialized before resolution was known, its initial position
        // may be off-screen (e.g., negative). Recenter it using current resolution.
        var needsRecentering =
            ActiveWindow.InitialX < 0 || ActiveWindow.InitialY < 0 ||
            ActiveWindow.InitialX + ActiveWindow.Width > GameState.ResolutionWidth ||
            ActiveWindow.InitialY + ActiveWindow.Height > GameState.ResolutionHeight;

        if (needsRecentering)
        {
            CentralizeWindow(windowIndex);
            ActiveWindow.InitialX = ActiveWindow.X;
            ActiveWindow.InitialY = ActiveWindow.Y;
        }
        else
        {
            ActiveWindow.X = ActiveWindow.InitialX;
            ActiveWindow.Y = ActiveWindow.InitialY;
        }
    }

    public static void HideWindow(string windowName)
    {
        HideWindow(GetWindowIndex(windowName));
    }

    public static void HideWindow(long windowIndex)
    {
        Windows[windowIndex].Visible = false;

        for (var i = Windows.Count - 1; i >= 1; i += -1)
        {
            if (Windows[i].Visible && Windows[i].ZChange != 1)
            {
                continue;
            }

            ActiveWindow = Windows[i];
            break;
        }
    }

    // Rendering & Initialisation
    public static void Init()
    {
        // Erase windows
        Windows = new ConcurrentDictionary<long, Window>();

        // Starter values
        ZOrderWin = 0;
        ZOrderCon = 0;

        // Dynamic UI initialization via Script.Instance (robust: keep going on errors)
        var ui = UIScript.Instance;
        if (ui is not null)
        {
            void Safe(string name, Action call)
            {
                try { call(); }
                catch (Exception ex) { Console.WriteLine($"UI script error in {name}: {ex.Message}"); }
            }

            Safe("UpdateWindow_Menu", () => ui.UpdateWindow_Menu());
            Safe("UpdateWindow_Register", () => ui.UpdateWindow_Register());
            Safe("UpdateWindow_Login", () => ui.UpdateWindow_Login());
            Safe("UpdateWindow_NewChar", () => ui.UpdateWindow_NewChar());
            Safe("UpdateWindow_Jobs", () => ui.UpdateWindow_Jobs());
            Safe("UpdateWindow_Chars", () => ui.UpdateWindow_Chars());
            Safe("UpdateWindow_ChatSmall", () => ui.UpdateWindow_ChatSmall());
            Safe("UpdateWindow_Chat", () => ui.UpdateWindow_Chat());
            Safe("UpdateWindow_Menu", () => ui.UpdateWindow_Menu());
            Safe("UpdateWindow_Description", () => ui.UpdateWindow_Description());
            Safe("UpdateWindow_Inventory", () => ui.UpdateWindow_Inventory());
            Safe("UpdateWindow_Skills", () => ui.UpdateWindow_Skills());
            Safe("UpdateWindow_Character", () => ui.UpdateWindow_Character());
            Safe("UpdateWindow_Hotbar", () => ui.UpdateWindow_Hotbar());
            Safe("UpdateWindow_Bank", () => ui.UpdateWindow_Bank());
            Safe("UpdateWindow_Shop", () => ui.UpdateWindow_Shop());
            Safe("UpdateWindow_EscMenu", () => ui.UpdateWindow_EscMenu());
            Safe("UpdateWindow_Bars", () => ui.UpdateWindow_Bars());
            Safe("UpdateWindow_Dialogue", () => ui.UpdateWindow_Dialogue());
            Safe("UpdateWindow_DragBox", () => ui.UpdateWindow_DragBox());
            Safe("UpdateWindow_Options", () => ui.UpdateWindow_Options());
            Safe("UpdateWindow_Trade", () => ui.UpdateWindow_Trade());
            Safe("UpdateWindow_Party", () => ui.UpdateWindow_Party());
            Safe("UpdateWindow_PlayerMenu", () => ui.UpdateWindow_PlayerMenu());
            Safe("UpdateWindow_RightClick", () => ui.UpdateWindow_RightClick());
            Safe("UpdateWindow_Combobox", () => ui.UpdateWindow_Combobox());
        }
        else
        {
            Console.WriteLine("UI script not loaded; windows will be created on demand from layouts.");
        }
    }

    public static bool HandleInterfaceEvents(ControlState entState)
    {
        Window? curWindow = null;
        var curControl = 0;

        // Check for MouseDown to start the drag timer
        if (GameClient.IsMouseButtonDown(MouseButton.Left) && GameClient.PreviousMouseState.LeftButton == ButtonState.Released)
        {
            DragTimer.Restart(); // Start the timer on initial mouse down
            _canDrag = false; // Reset drag flag to ensure it doesn't drag immediately
        }

        // Check for MouseUp to reset dragging
        if (GameClient.IsMouseButtonUp(MouseButton.Left))
        {
            _isDragging = false;

            DragTimer.Reset(); // Stop the timer on mouse up
        }

        // Enable dragging if the mouse has been held down for the specified interval
        _canDrag = DragTimer.ElapsedMilliseconds >= DragInterval;

        lock (GameClient.InputLock)
        {
            foreach (var window in Windows.Values)
            {
                if (!window.Visible)
                {
                    continue;
                }

                if (window.State != ControlState.MouseDown)
                {
                    window.State = ControlState.Normal;
                }

                if (GameState.CurMouseX >= window.X &&
                    GameState.CurMouseX <= window.Width + window.X &&
                    GameState.CurMouseY >= window.Y &&
                    GameState.CurMouseY <= window.Height + window.Y)
                {
                    // Handle combo menu logic
                    if (window.Design[0] == Design.ComboMenuNormal)
                    {
                        switch (entState)
                        {
                            case ControlState.MouseMove or ControlState.Hover:
                                ComboMenu_MouseMove(window);
                                break;

                            case ControlState.MouseDown:
                                ComboMenu_MouseDown(window);
                                break;
                        }
                    }

                    // Track the top-most window
                    if (curWindow is null || window.ZOrder > curWindow.ZOrder)
                    {
                        curWindow = window;

                        _isDragging = true;
                    }

                    if (ActiveWindow is not null)
                    {
                        if (!ActiveWindow.Visible || !ActiveWindow.CanDrag)
                        {
                            ActiveWindow = curWindow;
                        }
                    }
                    else
                    {
                        ActiveWindow = curWindow;
                    }
                }

                if (entState != ControlState.MouseMove || !GameClient.IsMouseButtonDown(MouseButton.Left))
                {
                    continue;
                }

                if (ActiveWindow is not null && _isDragging)
                {
                    if (_canDrag && ActiveWindow is {CanDrag: true, Visible: true})
                    {
                        ActiveWindow.X = GameLogic.Clamp(
                            ActiveWindow.X +
                            (GameState.CurMouseX - ActiveWindow.X - ActiveWindow.MovedX), 0,
                            GameState.ResolutionWidth - ActiveWindow.Width);
                        ActiveWindow.Y = GameLogic.Clamp(
                            ActiveWindow.Y +
                            (GameState.CurMouseY - ActiveWindow.Y - ActiveWindow.MovedY), 0,
                            GameState.ResolutionHeight - ActiveWindow.Height);
                        break;
                    }
                }
            }

            if (curWindow is not null)
            {
                // Handle the active window's callback
                var callBack = curWindow.CallBack[(int) entState];

                // Execute the callback if it exists
                callBack?.Invoke();

                // Handle controls in the active window
                for (var i = 0; i < curWindow.Controls.Count; i++)
                {
                    var control = curWindow.Controls[i];

                    if (control is {Enabled: true, Visible: true})
                    {
                        bool isComboMenuHit = false;
                        if (control is ComboBox comboBox)
                        {                          
                            int itemHeight = 10;
                            int menuPadding = 5;
                            int menuX = curWindow.X + comboBox.X - menuPadding;
                            int menuY = curWindow.Y + comboBox.Y + comboBox.Height;
                            int menuWidth = comboBox.Width + menuPadding * 2;
                            int menuHeight = comboBox.Items.Count * itemHeight + menuPadding * 2;
                            if (GameState.CurMouseX >= menuX &&
                                GameState.CurMouseX <= menuX + menuWidth &&
                                GameState.CurMouseY >= menuY &&
                                GameState.CurMouseY <= menuY + menuHeight)
                            {
                                isComboMenuHit = true;
                            }
                            // Optionally, you can add a border or background drawing here if you have a custom render method
                        }

                        if ((GameState.CurMouseX >= control.X + curWindow.X &&
                             GameState.CurMouseX <= control.X + control.Width + curWindow.X &&
                             GameState.CurMouseY >= control.Y + curWindow.Y &&
                             GameState.CurMouseY <= control.Y + control.Height + curWindow.Y)
                            || isComboMenuHit)
                        {
                            if (curControl == 0L || control.ZOrder > curWindow.Controls[curControl].ZOrder)
                            {
                                curControl = i;
                            }
                        }
                    }
                }

                if (curControl > 0)
                {
                    // Reset all control states
                    for (var j = 0; j < curWindow.Controls.Count; j++)
                    {
                        if (curControl != j)
                        {
                            curWindow.Controls[j].State = ControlState.Normal;
                        }
                    }

                    var withBlock2 = curWindow.Controls[curControl];

                    withBlock2.State = entState switch
                    {
                        ControlState.MouseMove => ControlState.Hover,
                        ControlState.MouseDown => ControlState.MouseDown,
                        _ => withBlock2.State
                    };

                    // Handle specific control types
                    switch (withBlock2)
                    {
                        case CheckBox checkBox:
                        {
                            if (checkBox.Group > 0 && withBlock2.Value == 0)
                            {
                                foreach (var control in curWindow.Controls.OfType<CheckBox>())
                                {
                                    if (control != checkBox && control.Group == checkBox.Group)
                                    {
                                        control.Value = 0;
                                    }
                                }

                                withBlock2.Value = 0;
                            }

                            break;
                        }
                        case ComboBox comboBox:
                        {
                            int itemHeight = 10;
                            int menuPadding = 5;
                            bool menuIsOpen = WinComboMenu.IsOpen(curWindow, curControl); // You may need to implement this check if not present
                            if (entState == ControlState.MouseDown && GameClient.IsMouseButtonDown(MouseButton.Left))
                            {
                                if (menuIsOpen)
                                {
                                    int menuX = curWindow.X + comboBox.X - menuPadding;
                                    int menuY = curWindow.Y + comboBox.Y + comboBox.Height;
                                    int menuWidth = comboBox.Width + menuPadding * 2;
                                    int menuHeight = comboBox.Items.Count * itemHeight + menuPadding * 2;
                                    bool inMenu = GameState.CurMouseX >= menuX && GameState.CurMouseX <= menuX + menuWidth &&
                                                  GameState.CurMouseY >= menuY && GameState.CurMouseY <= menuY + menuHeight;
                                    int relY = GameState.CurMouseY - (curWindow.Y + comboBox.Y + comboBox.Height + menuPadding);
                                    int idx = relY / itemHeight;
                                    if (inMenu && idx >= 0 && idx < comboBox.Items.Count)
                                    {
                                        comboBox.Value = idx;
                                        WinComboMenu.Close(); // Hide menu after selection
                                    }
                                    else if (!inMenu)
                                    {
                                        // Clicked outside menu, close it
                                        WinComboMenu.Close();
                                    }
                                }
                                else
                                {
                                    // Menu not open yet, open it
                                    WinComboMenu.Show(curWindow, curControl);
                                }
                            }
                            break;
                        }
                    }

                    if (GameClient.IsMouseButtonDown(MouseButton.Left))
                    {
                        SetActiveControl(curWindow, curControl);
                    }

                    callBack = withBlock2.CallBack[(int) entState];

                    // Execute the callback if it exists
                    callBack?.Invoke();
                }
            }

            if (curWindow is null)
            {
                ResetInterface();
            }

            if (entState == ControlState.MouseUp)
            {
                ResetMouseDown();
            }
        }

        return true;
    }

    public static void ResetInterface()
    {
        foreach (var window in Windows.Values)
        {
            if (window.State != ControlState.MouseDown)
            {
                window.State = ControlState.Normal;
            }

            if (window.Controls.Count == 0)
            {
                continue;
            }

            foreach (var control in window.Controls)
            {
                if (control.State != ControlState.MouseDown)
                {
                    control.State = ControlState.Normal;
                }
            }
        }
    }

    public static void ResetMouseDown()
    {
        lock (GameClient.InputLock)
        {
            foreach (var window in Windows.Values)
            {
                if (window.State == ControlState.MouseDown)
                {
                    window.State = ControlState.Normal;
                    window.CallBack[(int) ControlState.Normal]?.Invoke();
                }

                if (window.Controls.Count == 0)
                {
                    continue;
                }

                foreach (var control in window.Controls)
                {
                    if (control.State != ControlState.MouseDown)
                    {
                        continue;
                    }

                    control.State = ControlState.Normal;
                    control.CallBack[(int) control.State]?.Invoke();
                }
            }
        }
    }

    public static void Render()
    {
        if (Windows.IsEmpty)
        {
            return;
        }

        foreach (var window in Windows.Values.OrderBy(x => x.ZOrder).Where(x => x.Visible))
        {
            WindowRenderer.Render(window);

            foreach (var control in window.Controls.Where(x => x.Visible))
            {
                control.Render(window.X, window.Y);
                control.OnDraw?.Invoke();
            }
        }
    }
    
    private static void ComboMenu_MouseMove(Window window)
    {
        var y = GameState.CurMouseY - window.Y;

        for (var i = 0; i < window.List.Count - 1; i++)
        {
            if (y >= 16 * i && y <= 16 * i)
            {
                window.Group = i;
            }
        }
    }

    private static void ComboMenu_MouseDown(Window window)
    {
        if (window.List.Count == 0)
        {
            return;
        }

        var y = GameState.CurMouseY - window.Y;
        for (var i = 0; i < window.List.Count; i++)
        {
            if (y >= 16 * i && y < 16 * (i + 1))
            {
                if (window.ParentControl is not null)
                {
                    // Set the ComboBox.Value property if possible
                    if (window.ParentControl is Client.Game.UI.Controls.ComboBox comboBox)
                        comboBox.Value = i;
                    else
                        window.ParentControl.Value = i;
                }
                break;
            }
        }
        WinComboMenu.Close();
    }

    public static void ResizeGui()
    {
        // If UI hasn't been initialized yet, bail out safely
        if (Windows.IsEmpty)
        {
            return;
        }

        // Helper to safely apply changes when a window exists
        static void TryApply(string name, Action<Window> apply)
        {
            var idx = GetWindowIndex(name);
            if (idx == 0)
            {
                return;
            }

            if (Windows.TryGetValue(idx, out var w))
            {
                apply(w);
            }
        }

        // move Hotbar
        TryApply("winHotbar", w => w.X = GameState.ResolutionWidth - 432);

        // move chat
        TryApply("winChat", w => w.Y = GameState.ResolutionHeight - 178);
        TryApply("winChatSmall", w => w.Y = GameState.ResolutionHeight - 162);

        // move menu
        TryApply("winMenu", w =>
        {
            w.X = GameState.ResolutionWidth - 238;
            w.Y = GameState.ResolutionHeight - 42;
        });

        // re-size right-click background
        TryApply("winRightClickBG", w =>
        {
            w.Width = GameState.ResolutionWidth;
            w.Height = GameState.ResolutionHeight;
        });

        // re-size combo background
        TryApply("winComboMenuBG", w =>
        {
            w.Width = GameState.ResolutionWidth;
            w.Height = GameState.ResolutionHeight;
        });
    }

    public static void DrawMenuBackground()
    {
        var path = Path.Combine(DataPath.Pictures, "1");

        GameClient.RenderTexture(
            path: ref path,
            dX: 0, dY: 0, sX: 0, sY: 0,
            dW: 1920, dH: 1080,
            sW: 1920, sH: 1080);
    }

    public static void DrawYourTrade()
    {
        var color = 0;
        if (!TryGetWindow("winTrade", out var winTrade) ||
            !TryGetControl("winTrade", "picYour", out var picYour))
        {
            return;
        }

        var xo = winTrade!.X + picYour!.X;
        var yo = winTrade!.Y + picYour!.Y;

        // your items
        for (var i = 0; i < Constant.MaxInv; i++)
        {
            if (Data.TradeYourOffer[i].Num >= 0)
            {
                long itemNum = GetPlayerInv(GameState.MyIndex, Data.TradeYourOffer[i].Num);
                if (itemNum >= 0 & itemNum < Constant.MaxItems)
                {
                    Item.StreamItem((int) itemNum);
                    long itemPic = Data.Item[(int) itemNum].Icon;

                    if (itemPic > 0 & itemPic <= GameState.NumItems)
                    {
                        var top = yo + GameState.TradeTop + (GameState.TradeOffsetY + 32L) * (i / GameState.TradeColumns);
                        var left = xo + GameState.TradeLeft + (GameState.TradeOffsetX + 32L) * (i % GameState.TradeColumns);

                        // draw icon
                        var argPath = Path.Combine(DataPath.Items, itemPic.ToString());
                        GameClient.RenderTexture(ref argPath, (int) left, (int) top, 0, 0, 32, 32, 32, 32);

                        // If item is a stack - draw the amount you have
                        if (Data.TradeYourOffer[i].Value > 1)
                        {
                            var y = top + 20L;
                            var x = left + 1L;
                            var amountValue = Data.TradeYourOffer[i].Value;

                            // Color thresholds: <1M white, 1M-10M yellow, >10M bright green
                            if (amountValue < 1_000_000L)
                            {
                                color = (int) ColorName.White;
                            }
                            else if (amountValue > 1_000_000L && amountValue < 10_000_000L)
                            {
                                color = (int) ColorName.Yellow;
                            }
                            else if (amountValue > 10_000_000L)
                            {
                                color = (int) ColorName.BrightGreen;
                            }

                            TextRenderer.RenderText(
                                GameLogic.ConvertCurrency((int) amountValue),
                                (int) x,
                                (int) y,
                                GameClient.QbColorToXnaColor(color),
                                GameClient.QbColorToXnaColor(color));
                        }
                    }
                }
            }
        }
    }

    public static void DrawTheirTrade()
    {
        var color = 0;
        if (!TryGetWindow("winTrade", out var winTrade) ||
            !TryGetControl("winTrade", "picTheir", out var picTheir))
        {
            return;
        }

        var xo = winTrade!.X + picTheir!.X;
        var yo = winTrade!.Y + picTheir!.Y;

        // their items
        for (var i = 0; i < Constant.MaxInv; i++)
        {
            long itemNum = Data.TradeTheirOffer[i].Num;
            if (itemNum >= 0 & itemNum < Constant.MaxItems)
            {
                Item.StreamItem((int) itemNum);
                long itemPic = Data.Item[(int) itemNum].Icon;

                if (itemPic > 0 & itemPic <= GameState.NumItems)
                {
                    var top = yo + GameState.TradeTop + (GameState.TradeOffsetY + 32L) * (i / GameState.TradeColumns);
                    var left = xo + GameState.TradeLeft + (GameState.TradeOffsetX + 32L) * (i % GameState.TradeColumns);

                    // draw icon
                    var argPath = Path.Combine(DataPath.Items, itemPic.ToString());
                    GameClient.RenderTexture(ref argPath, (int) left, (int) top, 0, 0, 32, 32, 32, 32);

                    // If item is a stack - draw the amount you have
                    if (Data.TradeTheirOffer[i].Value > 1)
                    {
                        var y = top + 20L;
                        var x = left + 1L;
                        var amountValue = Data.TradeTheirOffer[i].Value;

                        // Color thresholds: <1M white, 1M-10M yellow, >10M bright green
                        if (amountValue < 1_000_000L)
                        {
                            color = (int) ColorName.White;
                        }
                        else if (amountValue > 1_000_000L && amountValue < 10_000_000L)
                        {
                            color = (int) ColorName.Yellow;
                        }
                        else if (amountValue > 10_000_000L)
                        {
                            color = (int) ColorName.BrightGreen;
                        }

                        TextRenderer.RenderText(
                            GameLogic.ConvertCurrency((int) amountValue),
                            (int) x,
                            (int) y,
                            GameClient.QbColorToXnaColor(color),
                            GameClient.QbColorToXnaColor(color));
                    }
                }
            }
        }
    }

    public static void UpdateActiveControl(Control modifiedControl)
    {
        if (ActiveWindow?.ActiveControl is not null)
        {
            var index = ActiveWindow.Controls.IndexOf(ActiveWindow.ActiveControl);

            // Update the control within the active window's Controls array
            ActiveWindow.Controls[index] = modifiedControl;
        }
    }

    public static Control? GetActiveControl()
    {
        return ActiveWindow?.ActiveControl;
    }

    /// <summary>
    /// Moves focus to the next enabled, visible, and focusable control in the active window.
    /// </summary>
    public static void FocusNextControl()
    {
        if (ActiveWindow?.Controls is not {Count: > 0})
        {
            return;
        }

        var controls = ActiveWindow.Controls;
        var currentIndex = ActiveWindow.ActiveControl is null ? -1 : controls.IndexOf(ActiveWindow.ActiveControl);
        var nextIndex = (currentIndex + 1) % controls.Count;

        while (nextIndex != currentIndex)
        {
            var control = controls[nextIndex];
            if (control is {Enabled: true, Visible: true} and TextBox)
            {
                ActiveWindow.ActiveControl = control;
                return;
            }

            nextIndex = (nextIndex + 1) % controls.Count;
        }
    }
}