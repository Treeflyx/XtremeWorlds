using System.Collections.Concurrent;
using System.Diagnostics;
using Client.Game.UI.Controls;
using Client.Game.UI.Windows;
using Core.Configurations;
using Core.Globals;
using Microsoft.VisualBasic.CompilerServices;
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
            Width = (int) (width * SettingsManager.Instance.Scale),
            Height = (int) (height * SettingsManager.Instance.Scale),
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
            Width = (int) (width * SettingsManager.Instance.Scale),
            Height = (int) (height * SettingsManager.Instance.Scale),
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
            Width = (int) (width * SettingsManager.Instance.Scale),
            Height = (int) (height * SettingsManager.Instance.Scale),
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
            Width = (int) (width * SettingsManager.Instance.Scale),
            Height = (int) (height * SettingsManager.Instance.Scale),
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
            Width = (int) (width * SettingsManager.Instance.Scale),
            Height = (int) (height * SettingsManager.Instance.Scale),
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
            Width = (int) (width * SettingsManager.Instance.Scale),
            Height = (int) (height * SettingsManager.Instance.Scale),
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
            Width = (int) (width * SettingsManager.Instance.Scale),
            Height = (int) (height * SettingsManager.Instance.Scale),
            Design = design,
            Texture = texture,
            CallBack = callback
        };

        window.Controls.Add(comboBox);

        ZOrderCon++;
    }

    public static int GetWindowIndex(string windowName)
    {
        for (var i = 0; i <= Windows.Count - 1; i++)
        {
            if (string.Equals(Windows[i + 1].Name, windowName, StringComparison.CurrentCultureIgnoreCase))
            {
                return i + 1;
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
        ShowWindow(GetWindowIndex(windowName), forced, resetPosition);
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

        ActiveWindow.X = ActiveWindow.InitialX;
        ActiveWindow.Y = ActiveWindow.InitialY;
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

        // Menu (dynamic UI initialization via Script.Instance)
        var ui = UIScript.Instance;
        ui?.UpdateWindow_Menu();
        ui?.UpdateWindow_Register();
        ui?.UpdateWindow_Login();
        ui?.UpdateWindow_NewChar();
        ui?.UpdateWindow_Jobs();
        ui?.UpdateWindow_Chars();
        ui?.UpdateWindow_ChatSmall();
        ui?.UpdateWindow_Chat();
        ui?.UpdateWindow_Menu();
        ui?.UpdateWindow_Description();
        ui?.UpdateWindow_Inventory();
        ui?.UpdateWindow_Skills();
        ui?.UpdateWindow_Character();
        ui?.UpdateWindow_Hotbar();
        ui?.UpdateWindow_Bank();
        ui?.UpdateWindow_Shop();
        ui?.UpdateWindow_EscMenu();
        ui?.UpdateWindow_Bars();
        ui?.UpdateWindow_Dialogue();
        ui?.UpdateWindow_DragBox();
        ui?.UpdateWindow_Options();
        ui?.UpdateWindow_Trade();
        ui?.UpdateWindow_Party();
        ui?.UpdateWindow_PlayerMenu();
        ui?.UpdateWindow_RightClick();
        ui?.UpdateWindow_Combobox();
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
                        if (GameState.CurMouseX >= control.X + curWindow.X &&
                            GameState.CurMouseX <= control.X + control.Width + curWindow.X &&
                            GameState.CurMouseY >= control.Y + curWindow.Y &&
                            GameState.CurMouseY <= control.Y + control.Height + curWindow.Y)
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

                        case ComboBox:
                            WinComboMenu.Show(curWindow, curControl);
                            break;
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
            if (y < 16 * i || y > 16 * i)
            {
                continue;
            }

            if (window.ParentControl is not null)
            {
                window.ParentControl.Value = i;
            }

            WinComboMenu.Close();
            break;
        }
    }

    public static void ResizeGui()
    {
        // move Hotbar
        Windows[GetWindowIndex("winHotbar")].X = GameState.ResolutionWidth - 432;

        // move chat
        Windows[GetWindowIndex("winChat")].Y = GameState.ResolutionHeight - 178;
        Windows[GetWindowIndex("winChatSmall")].Y = GameState.ResolutionHeight - 162;

        // move menu
        Windows[GetWindowIndex("winMenu")].X = GameState.ResolutionWidth - 238;
        Windows[GetWindowIndex("winMenu")].Y = GameState.ResolutionHeight - 42;

        // re-size right-click background
        Windows[GetWindowIndex("winRightClickBG")].Width = GameState.ResolutionWidth;
        Windows[GetWindowIndex("winRightClickBG")].Height = GameState.ResolutionHeight;

        // re-size combo background
        Windows[GetWindowIndex("winComboMenuBG")].Width = GameState.ResolutionWidth;
        Windows[GetWindowIndex("winComboMenuBG")].Height = GameState.ResolutionHeight;
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

        var xo = Windows[GetWindowIndex("winTrade")].X + Windows[GetWindowIndex("winTrade")].Controls[GetControlIndex("winTrade", "picYour")].X;
        var yo = Windows[GetWindowIndex("winTrade")].Y + Windows[GetWindowIndex("winTrade")].Controls[GetControlIndex("winTrade", "picYour")].Y;

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
                        var argpath = Path.Combine(DataPath.Items, itemPic.ToString());
                        GameClient.RenderTexture(ref argpath, (int) left, (int) top, 0, 0, 32, 32, 32, 32);

                        // If item is a stack - draw the amount you have
                        if (Data.TradeYourOffer[i].Value > 1)
                        {
                            var y = top + 20L;
                            var x = left + 1L;
                            var amount = Data.TradeYourOffer[i].Value.ToString();

                            // Draw currency but with k, m, b etc. using a convertion function
                            if (Conversions.ToLong(amount) < 1000000L)
                            {
                                color = (int) ColorName.White;
                            }
                            else if (Conversions.ToLong(amount) > 1000000L & Conversions.ToLong(amount) < 10000000L)
                            {
                                color = (int) ColorName.Yellow;
                            }
                            else if (Conversions.ToLong(amount) > 10000000L)
                            {
                                color = (int) ColorName.BrightGreen;
                            }

                            TextRenderer.RenderText(GameLogic.ConvertCurrency(Conversions.ToInteger(amount)), (int) x, (int) y, GameClient.QbColorToXnaColor(color), GameClient.QbColorToXnaColor(color));
                        }
                    }
                }
            }
        }
    }

    public static void DrawTheirTrade()
    {
        var color = 0;

        var xo = Windows[GetWindowIndex("winTrade")].X + Windows[GetWindowIndex("winTrade")].Controls[GetControlIndex("winTrade", "picTheir")].X;
        var yo = Windows[GetWindowIndex("winTrade")].Y + Windows[GetWindowIndex("winTrade")].Controls[GetControlIndex("winTrade", "picTheir")].Y;

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
                    var argpath = Path.Combine(DataPath.Items, itemPic.ToString());
                    GameClient.RenderTexture(ref argpath, (int) left, (int) top, 0, 0, 32, 32, 32, 32);

                    // If item is a stack - draw the amount you have
                    if (Data.TradeTheirOffer[i].Value > 1)
                    {
                        var y = top + 20L;
                        var x = left + 1L;
                        var amount = Data.TradeTheirOffer[i].Value.ToString();

                        // Draw currency but with k, m, b etc. using a convertion function
                        if (Conversions.ToLong(amount) < 1000000L)
                        {
                            color = (int) ColorName.White;
                        }
                        else if (Conversions.ToLong(amount) > 1000000L & Conversions.ToLong(amount) < 10000000L)
                        {
                            color = (int) ColorName.Yellow;
                        }
                        else if (Conversions.ToLong(amount) > 10000000L)
                        {
                            color = (int) ColorName.BrightGreen;
                        }

                        TextRenderer.RenderText(GameLogic.ConvertCurrency(Conversions.ToInteger(amount)), (int) x, (int) y, GameClient.QbColorToXnaColor(color), GameClient.QbColorToXnaColor(color));
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