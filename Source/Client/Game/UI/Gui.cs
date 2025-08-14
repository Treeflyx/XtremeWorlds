using System.Collections.Concurrent;
using System.Diagnostics;
using Client.Game.UI.Controls;
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
    public static ConcurrentDictionary<long, Window> Windows = new();
    public static Window? ActiveWindow;

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

    public static void CreateControl(
        int windowIndex,
        int layer,
        string name,
        Color color, ControlType type,
        List<Design> designs,
        List<int> images,
        List<string> textures,
        List<Action?> callbacks,
        int left = 0, int top = 0, int width = 0, int height = 0,
        bool visible = true,
        bool canDrag = false,
        int max = 0,
        int min = 0,
        int value = 0,
        string text = "",
        Alignment alignment = 0,
        Font font = Font.Georgia,
        int alpha = 255,
        bool clickThrough = false,
        int xOffset = 0,
        int yOffset = 0,
        byte zChange = 0,
        bool censor = false,
        int icon = 0,
        Action? onDraw = null, bool isActive = true,
        string tooltip = "",
        int group = 0,
        byte length = Constant.NameLength,
        bool enabled = true)
    {
        if (!Windows.TryGetValue(windowIndex, out var window))
        {
            return;
        }

        var newControl = new Control
        {
            Name = name,
            Type = type,
            Left = left,
            Top = top,
            OrigLeft = left,
            OrigTop = top,
            Width = (int)(width * SettingsManager.Instance.Scale),
            Height = (int)(height * SettingsManager.Instance.Scale),
            Visible = visible,
            CanDrag = canDrag,
            Max = max,
            Min = min,
            Value = value,
            Text = text,
            Length = length,
            Align = alignment,
            Font = font,
            Color = color,
            Alpha = alpha,
            ClickThrough = clickThrough,
            XOffset = xOffset,
            YOffset = yOffset,
            ZChange = zChange,
            ZOrder = layer,
            Enabled = enabled,
            OnDraw = onDraw,
            Tooltip = tooltip,
            Group = group,
            Censor = censor,
            Icon = icon,
            Design = designs,
            Image = images,
            Texture = textures,
            CallBack = callbacks
        };

        window.Controls.Add(newControl);

        if (isActive)
        {
            window.ActiveControl = newControl;
        }

        // set the zOrder
        ZOrderCon++;
    }

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
        Windows[windowIndex].Controls[controlIndex].List.Add(text);
    }

    public static int UpdateWindow(
        string name, string caption, Font font, int zOrder,
        int left, int top, int width, int height,
        int icon,
        bool visible = true,
        int xOffset = 0,
        int yOffset = 0,
        Design designNorm = Design.None,
        Design designHover = Design.None,
        Design designMousedown = Design.None,
        int imageNorm = 0,
        int imageHover = 0,
        int imageMousedown = 0,
        Action? callbackNorm = null,
        Action? callbackHover = null,
        Action? callbackMousemove = null,
        Action? callbackMousedown = null,
        Action? callbackDblclick = null,
        Action? onDraw = null,
        bool canDrag = true,
        byte zChange = 1,
        bool clickThrough = false)
    {
        var stateCount = Enum.GetValues<ControlState>().Length;
        var design = new List<Design>(Enumerable.Repeat((Design)0, stateCount));
        var image = new List<int>(Enumerable.Repeat(0, stateCount));
        var callback = new List<Action?>(Enumerable.Repeat((Action)null, stateCount));

        // Assign specific values for each state
        design[(int)ControlState.Normal] = designNorm;
        design[(int)ControlState.Hover] = designHover;
        design[(int)ControlState.MouseDown] = designMousedown;

        image[(int)ControlState.Normal] = imageNorm;
        image[(int)ControlState.Hover] = imageHover;
        image[(int)ControlState.MouseDown] = imageMousedown;

        callback[(int)ControlState.Normal] = callbackNorm;
        callback[(int)ControlState.Hover] = callbackHover;
        callback[(int)ControlState.MouseDown] = callbackMousedown;
        callback[(int)ControlState.MouseMove] = callbackMousemove;
        callback[(int)ControlState.DoubleClick] = callbackDblclick;

        // Create a new instance of Window and populate it
        var window = new Window
        {
            Name = name,
            Type = ControlType.Window,
            Left = left,
            Top = top,
            OrigLeft = left,
            OrigTop = top,
            Width = (int)(width * SettingsManager.Instance.Scale),
            Height = (int)(height * SettingsManager.Instance.Scale),
            Visible = visible,
            CanDrag = canDrag,
            Font = font,
            Text = caption,
            XOffset = xOffset,
            YOffset = yOffset,
            Icon = icon,
            Enabled = true,
            ZChange = zChange,
            ZOrder = zOrder,
            OnDraw = onDraw,
            ClickThrough = clickThrough,
            Design = design,
            Image = image,
            CallBack = callback
        };

        // Add the new control to the specified window's controls list
        Windows.TryAdd(Windows.Count + 1, window);

        if (visible)
        {
            ActiveWindow = window;
        }

        return Windows.Count;
    }

    public static int UpdateTextbox(
        int windowIndex, string name,
        int left, int top, int width, int height,
        string text = "",
        Font font = Font.Georgia,
        Alignment align = Alignment.Left,
        bool visible = true,
        int alpha = 255,
        bool isActive = true,
        int xOffset = 0,
        int yOffset = 0,
        int imageNorm = 0,
        int imageHover = 0,
        int imageMousedown = 0,
        Design designNorm = Design.None,
        Design designHover = Design.None,
        Design designMousedown = Design.None,
        bool censor = false,
        int icon = 0,
        byte length = Constant.NameLength,
        Action? callbackNorm = null,
        Action? callbackHover = null,
        Action? callbackMousedown = null,
        Action? callbackMousemove = null,
        Action? callbackDblclick = null,
        Action? callbackEnter = null)
    {
        var stateCount = Enum.GetValues(typeof(ControlState)).Length;
        var designs = new List<Design>(Enumerable.Repeat(Design.None, stateCount).ToList());
        var images = new List<int>(Enumerable.Repeat(0, stateCount).ToList());
        var textures = new List<string>(Enumerable.Repeat(DataPath.Designs, stateCount).ToList());
        var callbacks = new List<Action?>(Enumerable.Repeat((Action)null, stateCount).ToList());

        designs[(int)ControlState.Normal] = designNorm;
        designs[(int)ControlState.Hover] = designHover;
        designs[(int)ControlState.MouseDown] = designMousedown;

        images[(int)ControlState.Normal] = imageNorm;
        images[(int)ControlState.Hover] = imageHover;
        images[(int)ControlState.MouseDown] = imageMousedown;

        callbacks[(int)ControlState.Normal] = callbackNorm;
        callbacks[(int)ControlState.Hover] = callbackHover;
        callbacks[(int)ControlState.MouseDown] = callbackMousedown;
        callbacks[(int)ControlState.MouseMove] = callbackMousemove;
        callbacks[(int)ControlState.DoubleClick] = callbackDblclick;
        callbacks[(int)ControlState.FocusEnter] = callbackEnter;

        if (!Windows.TryGetValue(windowIndex, out var window))
        {
            throw new UIException($"{windowIndex} is not a valid window index.");
        }

        var textBox = new TextBox
        {
            Name = name,
            Type = ControlType.TextBox,
            Left = left,
            Top = top,
            OrigLeft = left,
            OrigTop = top,
            Width = (int)(width * SettingsManager.Instance.Scale),
            Height = (int)(height * SettingsManager.Instance.Scale),
            Visible = visible,
            CanDrag = false,
            Text = text,
            Length = length,
            Align = align,
            Font = font,
            Color = Color.White,
            Alpha = alpha,
            ClickThrough = false,
            XOffset = xOffset,
            YOffset = yOffset,
            ZOrder = window.Controls.Count,
            Enabled = true,
            Censor = censor,
            Icon = icon,
            Design = designs,
            Image = images,
            Texture = textures,
            CallBack = callbacks
        };

        window.Controls.Add(textBox);

        var controlIndex = window.Controls.Count - 1;

        if (isActive)
        {
            window.ActiveControl = textBox;
        }

        ZOrderCon++;

        return controlIndex;
    }

    public static void UpdatePictureBox(
        int windowIndex, string name,
        int left, int top, int width, int height,
        bool visible = true,
        bool canDrag = false,
        int alpha = 255,
        bool clickThrough = true,
        int imageNorm = 0,
        int imageHover = 0,
        int imageMousedown = 0,
        Design designNorm = Design.None,
        Design designHover = Design.None,
        Design designMousedown = Design.None,
        string texturePath = "",
        Action? callbackNorm = null,
        Action? callbackHover = null,
        Action? callbackMousedown = null,
        Action? callbackMousemove = null,
        Action? callbackDblclick = null,
        Action? onDraw = null)
    {
        var stateCount = Enum.GetValues(typeof(ControlState)).Length;
        var design = new List<Design>(Enumerable.Repeat(Design.None, stateCount));
        var image = new List<int>(Enumerable.Repeat(0, stateCount));
        var texture = new List<string>(Enumerable.Repeat(string.Empty, stateCount));
        var callback = new List<Action?>(Enumerable.Repeat((Action)null, stateCount));

        if (string.IsNullOrEmpty(texturePath))
        {
            texturePath = DataPath.Gui;
        }

        // fill temp arrays
        design[(int)ControlState.Normal] = designNorm;
        design[(int)ControlState.Hover] = designHover;
        design[(int)ControlState.MouseDown] = designMousedown;
        image[(int)ControlState.Normal] = imageNorm;
        image[(int)ControlState.Hover] = imageHover;
        image[(int)ControlState.MouseDown] = imageMousedown;
        texture[(int)ControlState.Normal] = texturePath;
        texture[(int)ControlState.Hover] = texturePath;
        texture[(int)ControlState.MouseDown] = texturePath;

        callback[(int)ControlState.Normal] = callbackNorm;
        callback[(int)ControlState.Hover] = callbackHover;
        callback[(int)ControlState.MouseDown] = callbackMousedown;
        callback[(int)ControlState.MouseMove] = callbackMousemove;
        callback[(int)ControlState.DoubleClick] = callbackDblclick;

        // Control the box
        CreateControl(windowIndex, ZOrderCon, name, Color.White, ControlType.PictureBox, design, image, texture, callback, left, top, width, height, visible, canDrag, alpha: alpha, clickThrough: clickThrough, xOffset: 0, yOffset: 0, onDraw: onDraw);
    }

    public static void UpdateButton(
        int windowIndex, string name,
        int left, int top, int width, int height,
        string text = "",
        Font font = Font.Georgia,
        int icon = 0,
        int imageNorm = 0,
        int imageHover = 0,
        int imageMousedown = 0,
        bool visible = true,
        int alpha = 255,
        Design designNorm = Design.None,
        Design designHover = Design.None,
        Design designMousedown = Design.None,
        Action? callbackNorm = null,
        Action? callbackHover = null,
        Action? callbackMousedown = null,
        Action? callbackMousemove = null,
        Action? callbackDblclick = null,
        int xOffset = 0,
        int yOffset = 0,
        string tooltip = "",
        bool censor = false)
    {
        var stateCount = Enum.GetValues(typeof(ControlState)).Length;
        var design = new List<Design>(Enumerable.Repeat(Design.None, stateCount).ToList());
        var image = new List<int>(Enumerable.Repeat(0, stateCount).ToList());
        var texture = new List<string>(Enumerable.Repeat(DataPath.Designs, stateCount).ToList());
        var callback = new List<Action?>(Enumerable.Repeat((Action)null, stateCount).ToList());

        // fill temp arrays
        design[(int)ControlState.Normal] = designNorm;
        design[(int)ControlState.Hover] = designHover;
        design[(int)ControlState.MouseDown] = designMousedown;
        image[(int)ControlState.Normal] = imageNorm;
        image[(int)ControlState.Hover] = imageHover;
        image[(int)ControlState.MouseDown] = imageMousedown;
        texture[(int)ControlState.Normal] = DataPath.Gui;
        texture[(int)ControlState.Hover] = DataPath.Gui;
        texture[(int)ControlState.MouseDown] = DataPath.Gui;
        callback[(int)ControlState.Normal] = callbackNorm;
        callback[(int)ControlState.Hover] = callbackHover;
        callback[(int)ControlState.MouseDown] = callbackMousedown;
        callback[(int)ControlState.MouseMove] = callbackMousemove;
        callback[(int)ControlState.DoubleClick] = callbackDblclick;

        // Control the button 
        CreateControl(windowIndex, ZOrderCon, name, Color.White, ControlType.Button, design, image, texture, callback, left, top, width, height, visible, text: text, font: font, clickThrough: Conversions.ToBoolean(alpha), xOffset: xOffset, yOffset: yOffset, censor: censor, icon: icon, tooltip: tooltip);
    }

    public static void UpdateLabel(
        int windowIndex, string name,
        int left, int top, int width, int height,
        string text, Font font, Color color,
        Alignment align = Alignment.Left,
        bool visible = true,
        int alpha = 255,
        bool clickThrough = false,
        bool censor = false,
        Action? callbackNorm = null,
        Action? callbackHover = null,
        Action? callbackMousedown = null,
        Action? callbackMousemove = null,
        Action? callbackDblclick = null,
        bool enabled = false)
    {
        var controlStateCount = Enum.GetValues<ControlState>().Length;
        var designLabel = new List<Design>(Enumerable.Repeat(Design.None, controlStateCount).ToList());
        var imageLabel = new List<int>(Enumerable.Repeat(0, controlStateCount).ToList());
        var textureLabel = new List<string>(Enumerable.Repeat(DataPath.Designs, controlStateCount).ToList());
        var callbackLabel = new List<Action?>(Enumerable.Repeat((Action)null, controlStateCount).ToList());

        // fill temp arrays
        callbackLabel[(int)ControlState.Normal] = callbackNorm;
        callbackLabel[(int)ControlState.Hover] = callbackHover;
        callbackLabel[(int)ControlState.MouseDown] = callbackMousedown;
        callbackLabel[(int)ControlState.MouseMove] = callbackMousemove;
        callbackLabel[(int)ControlState.DoubleClick] = callbackDblclick;

        // Control the label
        CreateControl(windowIndex, ZOrderCon, name, Color.White, ControlType.Label, designLabel, imageLabel, textureLabel, callbackLabel,
            left, top, width, height, visible, text: text, alignment: align, font: font, clickThrough: Conversions.ToBoolean(alpha),
            xOffset: Conversions.ToInteger(clickThrough), censor: censor, enabled: enabled);
    }

    public static void UpdateCheckBox(
        int windowIndex, string name,
        int left, int top, int width, int height = 15,
        int value = 0,
        string text = "",
        Font font = Font.Georgia,
        Alignment align = Alignment.Left,
        bool visible = true,
        int alpha = 255,
        Design theDesign = Design.None,
        int group = 0,
        bool censor = false,
        Action? callbackNorm = null,
        Action? callbackHover = null,
        Action? callbackMousedown = null,
        Action? callbackMousemove = null,
        Action? callbackDblclick = null)
    {
        var stateCount = Enum.GetValues(typeof(ControlState)).Length;
        var design = new List<Design>(Enumerable.Repeat(Design.None, stateCount).ToList());
        var image = new List<int>(Enumerable.Repeat(0, stateCount).ToList());
        var texture = new List<string>(Enumerable.Repeat(DataPath.Designs, stateCount).ToList());
        var callback = new List<Action?>(Enumerable.Repeat((Action)null, stateCount).ToList());

        design[0] = theDesign;
        texture[0] = DataPath.Gui;

        // fill temp arrays
        callback[(int)ControlState.Normal] = callbackNorm;
        callback[(int)ControlState.Hover] = callbackHover;
        callback[(int)ControlState.MouseDown] = callbackMousedown;
        callback[(int)ControlState.MouseMove] = callbackMousemove;
        callback[(int)ControlState.DoubleClick] = callbackDblclick;

        // Control the box
        CreateControl(windowIndex, ZOrderCon, name, Color.White, ControlType.Checkbox, design, image,
            texture, callback, left, top, width, height, visible, value: value,
            text: text, alignment: align, font: font, clickThrough: Conversions.ToBoolean(alpha), censor: censor, @group: group);
    }

    public static void UpdateComboBox(int windowIndex, string name, int left, int top, int width, int height, Design design)
    {
        // Get the number of ControlState enum values
        var controlStateCount = Enum.GetValues(typeof(ControlState)).Length;

        // Initialize lists for the control states
        var theDesign = new List<Design>(Enumerable.Repeat(Design.None, controlStateCount).ToList());
        var image = new List<int>(Enumerable.Repeat(0, controlStateCount).ToList());
        var texture = new List<string>(Enumerable.Repeat(DataPath.Gui, controlStateCount).ToList());
        var callback = new List<Action?>(Enumerable.Repeat((Action)null, controlStateCount).ToList());

        // Set the design for the normal state
        theDesign[0] = design;
        texture[0] = DataPath.Gui;

        // Update the control in the window using the updated lists
        CreateControl(windowIndex, ZOrderCon, name, Color.White, ControlType.ComboMenu, theDesign, image, texture, callback, left, top, width, height);
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

    public static bool SetActiveControl(long windowIndex, int controlIndex)
    {
        switch (Windows[windowIndex].Controls[controlIndex].Type)
        {
            case ControlType.TextBox:
                Windows[windowIndex].LastControl = Windows[windowIndex].ActiveControl;
                Windows[windowIndex].ActiveControl = Windows[windowIndex].Controls[controlIndex];
                return true;
        }

        return false;
    }

    public static Control? ActivateControl(int startIndex = 0, bool skipLast = true)
    {
        if (ActiveWindow is null)
        {
            return null;
        }

        var activeControl = ActiveWindow.ActiveControl;
        if (activeControl is not null)
        {
            var index = ActiveWindow.Controls.IndexOf(activeControl);

            startIndex = Math.Min(startIndex, index + 1);
        }

        var controls = ActiveWindow.Controls.Skip(startIndex);
        if (startIndex > 0)
        {
            controls = controls.Concat(ActiveWindow.Controls.Take(startIndex - 1));
        }

        foreach (var control in controls)
        {
            if (skipLast && control == ActiveWindow.LastControl)
            {
                continue;
            }

            if (ActiveWindow.SetActiveControl(control))
            {
                return control;
            }
        }

        return null;
    }

    public static void CentralizeWindow(int windowIndex)
    {
        var window = Windows[windowIndex];

        window.Left = (int)Math.Round(GameState.ResolutionWidth / 2d - window.Width / 2d);
        window.Top = (int)Math.Round(GameState.ResolutionHeight / 2d - window.Height / 2d);
        window.OrigLeft = window.Left;
        window.OrigTop = window.Top;
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

        ActiveWindow.Left = ActiveWindow.OrigLeft;
        ActiveWindow.Top = ActiveWindow.OrigTop;
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
        var curWindow = 0;
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
            for (var i = 1; i < Windows.Count; i++)
            {
                var window = Windows[i];
                if (window is not { Enabled: true, Visible: true })
                {
                    continue;
                }

                if (window.State != ControlState.MouseDown)
                {
                    window.State = ControlState.Normal;
                }

                if (GameState.CurMouseX >= window.Left && GameState.CurMouseX <= window.Width + window.Left && GameState.CurMouseY >= window.Top && GameState.CurMouseY <= window.Height + window.Top)
                {
                    // Handle combo menu logic
                    if (window.Design[0] == Design.ComboMenuNormal)
                    {
                        switch (entState)
                        {
                            case ControlState.MouseMove or ControlState.Hover:
                                ComboMenu_MouseMove(i);
                                break;

                            case ControlState.MouseDown:
                                ComboMenu_MouseDown(i);
                                break;
                        }
                    }

                    // Track the top-most window
                    if (curWindow == 0 || window.ZOrder > Windows[curWindow].ZOrder)
                    {
                        curWindow = i;

                        _isDragging = true;
                    }

                    if (ActiveWindow is not null)
                    {
                        if (!ActiveWindow.Visible || !ActiveWindow.Enabled || !ActiveWindow.CanDrag)
                        {
                            ActiveWindow = Windows[curWindow];
                        }
                    }
                    else
                    {
                        ActiveWindow = Windows[curWindow];
                    }
                }

                if (entState != ControlState.MouseMove || !GameClient.IsMouseButtonDown(MouseButton.Left))
                {
                    continue;
                }

                if (ActiveWindow is not null && _isDragging)
                {
                    window = ActiveWindow;
                    if (_canDrag && window is { CanDrag: true, Enabled: true, Visible: true })
                    {
                        window.Left = GameLogic.Clamp(window.Left + (GameState.CurMouseX - window.Left - window.MovedX), 0, GameState.ResolutionWidth - window.Width);
                        window.Top = GameLogic.Clamp(window.Top + (GameState.CurMouseY - window.Top - window.MovedY), 0, GameState.ResolutionHeight - window.Height);
                        break;
                    }
                }
            }

            if (curWindow > 0L)
            {
                // Handle the active window's callback
                var callBack = Windows[curWindow].CallBack[(int)entState];

                // Execute the callback if it exists
                callBack?.Invoke();

                // Handle controls in the active window
                for (var i = 0; i < Windows[curWindow].Controls.Count; i++)
                {
                    var control = Windows[curWindow].Controls[i];

                    if (control is { Enabled: true, Visible: true })
                    {
                        if (GameState.CurMouseX >= control.Left + Windows[curWindow].Left && GameState.CurMouseX <= control.Left + control.Width + Windows[curWindow].Left && GameState.CurMouseY >= control.Top + Windows[curWindow].Top && GameState.CurMouseY <= control.Top + control.Height + Windows[curWindow].Top)
                        {
                            if (curControl == 0L || control.ZOrder > Windows[curWindow].Controls[curControl].ZOrder)
                            {
                                curControl = i;
                            }
                        }

                        if (_isDragging)
                        {
                            // Handle control dragging only if dragging is enabled
                            if (entState == ControlState.MouseMove && control.CanDrag && _canDrag && GameClient.IsMouseButtonDown(MouseButton.Left))
                            {
                                control.Left = GameLogic.Clamp(control.Left + (GameState.CurMouseX - control.Left - control.MovedX), 0, Windows[curWindow].Width - control.Width);
                                control.Top = GameLogic.Clamp(control.Top + (GameState.CurMouseY - control.Top - control.MovedY), 0, Windows[curWindow].Height - control.Height);
                            }
                        }
                    }
                }

                if (curControl > 0)
                {
                    // Reset all control states
                    for (var j = 0; j < Windows[curWindow].Controls.Count; j++)
                    {
                        if (curControl != j)
                        {
                            Windows[curWindow].Controls[j].State = ControlState.Normal;
                        }
                    }

                    var withBlock2 = Windows[curWindow].Controls[curControl];

                    withBlock2.State = entState switch
                    {
                        ControlState.MouseMove => ControlState.Hover,
                        ControlState.MouseDown => ControlState.MouseDown,
                        _ => withBlock2.State
                    };

                    if (GameClient.IsMouseButtonDown(MouseButton.Left) && withBlock2.CanDrag)
                    {
                        withBlock2.MovedX = GameState.CurMouseX - withBlock2.Left;
                        withBlock2.MovedY = GameState.CurMouseY - withBlock2.Top;
                    }

                    // Handle specific control types
                    switch (withBlock2.Type)
                    {
                        case ControlType.Checkbox:
                            {
                                if (withBlock2.Group > 0L && withBlock2.Value == 0L)
                                {
                                    foreach (var control in Windows[curWindow].Controls)
                                    {
                                        if (control.Type == ControlType.Checkbox &&
                                            control.Group == withBlock2.Group)
                                        {
                                            control.Value = 0;
                                        }
                                    }

                                    withBlock2.Value = 0;
                                }

                                break;
                            }

                        case ControlType.ComboMenu:
                            {
                                ShowComboMenu(curWindow, curControl);
                                break;
                            }
                    }

                    if (GameClient.IsMouseButtonDown(MouseButton.Left))
                    {
                        SetActiveControl(curWindow, curControl);
                    }

                    callBack = withBlock2.CallBack[(int)entState];

                    // Execute the callback if it exists
                    callBack?.Invoke();
                }
            }

            if (curWindow == 0)
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
                    window.CallBack[(int)ControlState.Normal]?.Invoke();
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
                    control.CallBack[(int)control.State]?.Invoke();
                }
            }
        }
    }

    public static void Render()
    {
        if (Windows.Count == 0)
        {
            return;
        }

        foreach (var window in Windows.Values.OrderBy(x => x.ZOrder).Where(x => x.Visible))
        {
            WindowRenderer.Render(window);

            foreach (var control in window.Controls.Where(x => x.Visible))
            {
                ControlRenderer.RenderControl(window, control);
            }
        }
    }

    public static void CloseComboMenu()
    {
        HideWindow(GetWindowIndex("winComboMenuBG"));
        HideWindow(GetWindowIndex("winComboMenu"));
    }

    public static void ShowComboMenu(int curWindow, int curControl)
    {
        var withBlock = Windows[curWindow].Controls[curControl];
        // Linked to
        long comboMenuIndex = GetWindowIndex("winComboMenu");
        Windows[comboMenuIndex].LinkedToWin = curWindow;
        Windows[comboMenuIndex].LinkedToCon = curControl;

        // Set the size
        Windows[comboMenuIndex].Height = 2 + withBlock.List.Count * 16; // Assumes .List is a collection
        Windows[comboMenuIndex].Left = Windows[curWindow].Left + withBlock.Left + 2;
        var top = Windows[curWindow].Top + withBlock.Top + withBlock.Height;
        if (top + Windows[comboMenuIndex].Height > GameState.ResolutionHeight)
        {
            top = GameState.ResolutionHeight - Windows[comboMenuIndex].Height;
        }

        Windows[comboMenuIndex].Top = top;
        Windows[comboMenuIndex].Width = withBlock.Width - 4;

        // Set the values
        Windows[comboMenuIndex].List = withBlock.List;
        Windows[comboMenuIndex].Value = withBlock.Value;
        Windows[comboMenuIndex].Group = 0;

        // Load the menu
        Windows[comboMenuIndex].Visible = true;
        Windows[GetWindowIndex("winComboMenuBG")].Visible = true;
        ShowWindow(GetWindowIndex("winComboMenuBG"), true, false);
        ShowWindow(GetWindowIndex("winComboMenu"), true, false);
    }

    public static void ComboMenu_MouseMove(long curWindow)
    {
        var withBlock = Windows[curWindow];
        var y = GameState.CurMouseY - withBlock.Top;

        // Find the option we're hovering over
        if (withBlock.List.Count > 0)
        {
            for (var i = 0; i < withBlock.List.Count - 1; i++)
            {
                if (y >= 16 * i & y <= 16 * i)
                {
                    withBlock.Group = i;
                }
            }
        }
    }

    public static void ComboMenu_MouseDown(long windowIndex)
    {
        var withBlock = Windows[windowIndex];
        var y = GameState.CurMouseY - withBlock.Top;

        // Find the option we're hovering over
        if (withBlock.List.Count > 0)
        {
            var loopTo = (long)withBlock.List.Count;
            for (var i = 0; i < loopTo; i++)
            {
                if (y >= 16L * i & y <= 16L * i)
                {
                    Windows[withBlock.LinkedToWin].Controls[withBlock.LinkedToCon].Value = i;
                    CloseComboMenu();
                    break;
                }
            }
        }
    }

    public static void UpdateStats_UI()
    {
        // set the bar labels
        var winBars = Windows[GetWindowIndex("winBars")];
        winBars.Controls[GetControlIndex("winBars", "lblHP")].Text = GetPlayerVital(GameState.MyIndex, Vital.Health) + "/" + GetPlayerMaxVital(GameState.MyIndex, Vital.Health);
        winBars.Controls[GetControlIndex("winBars", "lblMP")].Text = GetPlayerVital(GameState.MyIndex, Vital.Stamina) + "/" + GetPlayerMaxVital(GameState.MyIndex, Vital.Stamina);
        winBars.Controls[GetControlIndex("winBars", "lblEXP")].Text = GetPlayerExp(GameState.MyIndex) + "/" + GameState.NextlevelExp;

        // update character screen
        var winCharacter = Windows[GetWindowIndex("winCharacter")];
        winCharacter.Controls[GetControlIndex("winCharacter", "lblHealth")].Text = "Health";
        winCharacter.Controls[GetControlIndex("winCharacter", "lblSpirit")].Text = "Spirit";
        winCharacter.Controls[GetControlIndex("winCharacter", "lblExperience")].Text = "Exp";
        winCharacter.Controls[GetControlIndex("winCharacter", "lblHealth2")].Text = GetPlayerVital(GameState.MyIndex, Vital.Health) + "/" + GetPlayerMaxVital(GameState.MyIndex, Vital.Health);
        winCharacter.Controls[GetControlIndex("winCharacter", "lblSpirit2")].Text = GetPlayerVital(GameState.MyIndex, Vital.Stamina) + "/" + GetPlayerMaxVital(GameState.MyIndex, Vital.Stamina);
        winCharacter.Controls[GetControlIndex("winCharacter", "lblExperience2")].Text = Data.Player[GameState.MyIndex].Exp + "/" + GameState.NextlevelExp;
    }

    public static void ResizeGui()
    {
        // move Hotbar
        Windows[GetWindowIndex("winHotbar")].Left = GameState.ResolutionWidth - 432;

        // move chat
        Windows[GetWindowIndex("winChat")].Top = GameState.ResolutionHeight - 178;
        Windows[GetWindowIndex("winChatSmall")].Top = GameState.ResolutionHeight - 162;

        // move menu
        Windows[GetWindowIndex("winMenu")].Left = GameState.ResolutionWidth - 238;
        Windows[GetWindowIndex("winMenu")].Top = GameState.ResolutionHeight - 42;

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
        var color = 0L;

        var xo = Windows[GetWindowIndex("winTrade")].Left + Windows[GetWindowIndex("winTrade")].Controls[GetControlIndex("winTrade", "picYour")].Left;
        var yo = Windows[GetWindowIndex("winTrade")].Top + Windows[GetWindowIndex("winTrade")].Controls[GetControlIndex("winTrade", "picYour")].Top;

        // your items
        for (var i = 0L; i < Constant.MaxInv; i++)
        {
            if (Data.TradeYourOffer[(int)i].Num >= 0)
            {
                long itemNum = GetPlayerInv(GameState.MyIndex, Data.TradeYourOffer[(int)i].Num);
                if (itemNum >= 0L & itemNum < Constant.MaxItems)
                {
                    Item.StreamItem((int)itemNum);
                    long itemPic = Data.Item[(int)itemNum].Icon;

                    if (itemPic > 0L & itemPic <= GameState.NumItems)
                    {
                        var top = yo + GameState.TradeTop + (GameState.TradeOffsetY + 32L) * (i / GameState.TradeColumns);
                        var left = xo + GameState.TradeLeft + (GameState.TradeOffsetX + 32L) * (i % GameState.TradeColumns);

                        // draw icon
                        var argpath = Path.Combine(DataPath.Items, itemPic.ToString());
                        GameClient.RenderTexture(ref argpath, (int)left, (int)top, 0, 0, 32, 32, 32, 32);

                        // If item is a stack - draw the amount you have
                        if (Data.TradeYourOffer[(int)i].Value > 1)
                        {
                            var y = top + 20L;
                            var x = left + 1L;
                            var amount = Data.TradeYourOffer[(int)i].Value.ToString();

                            // Draw currency but with k, m, b etc. using a convertion function
                            if (Conversions.ToLong(amount) < 1000000L)
                            {
                                color = (long)ColorName.White;
                            }
                            else if (Conversions.ToLong(amount) > 1000000L & Conversions.ToLong(amount) < 10000000L)
                            {
                                color = (long)ColorName.Yellow;
                            }
                            else if (Conversions.ToLong(amount) > 10000000L)
                            {
                                color = (long)ColorName.BrightGreen;
                            }

                            Text.RenderText(GameLogic.ConvertCurrency(Conversions.ToInteger(amount)), (int)x, (int)y, GameClient.QbColorToXnaColor((int)color), GameClient.QbColorToXnaColor((int)color));
                        }
                    }
                }
            }
        }
    }

    public static void DrawTheirTrade()
    {
        var color = 0L;

        var xo = Windows[GetWindowIndex("winTrade")].Left + Windows[GetWindowIndex("winTrade")].Controls[GetControlIndex("winTrade", "picTheir")].Left;
        var yo = Windows[GetWindowIndex("winTrade")].Top + Windows[GetWindowIndex("winTrade")].Controls[GetControlIndex("winTrade", "picTheir")].Top;

        // their items
        for (var i = 0L; i < Constant.MaxInv; i++)
        {
            long itemNum = Data.TradeTheirOffer[(int)i].Num;
            if (itemNum >= 0L & itemNum < Constant.MaxItems)
            {
                Item.StreamItem((int)itemNum);
                long itemPic = Data.Item[(int)itemNum].Icon;

                if (itemPic > 0L & itemPic <= GameState.NumItems)
                {
                    var top = yo + GameState.TradeTop + (GameState.TradeOffsetY + 32L) * (i / GameState.TradeColumns);
                    var left = xo + GameState.TradeLeft + (GameState.TradeOffsetX + 32L) * (i % GameState.TradeColumns);

                    // draw icon
                    var argpath = Path.Combine(DataPath.Items, itemPic.ToString());
                    GameClient.RenderTexture(ref argpath, (int)left, (int)top, 0, 0, 32, 32, 32, 32);

                    // If item is a stack - draw the amount you have
                    if (Data.TradeTheirOffer[(int)i].Value > 1)
                    {
                        var y = top + 20L;
                        var x = left + 1L;
                        var amount = Data.TradeTheirOffer[(int)i].Value.ToString();

                        // Draw currency but with k, m, b etc. using a convertion function
                        if (Conversions.ToLong(amount) < 1000000L)
                        {
                            color = (long)ColorName.White;
                        }
                        else if (Conversions.ToLong(amount) > 1000000L & Conversions.ToLong(amount) < 10000000L)
                        {
                            color = (long)ColorName.Yellow;
                        }
                        else if (Conversions.ToLong(amount) > 10000000L)
                        {
                            color = (long)ColorName.BrightGreen;
                        }

                        Text.RenderText(GameLogic.ConvertCurrency(Conversions.ToInteger(amount)), (int)x, (int)y, GameClient.QbColorToXnaColor((int)color), GameClient.QbColorToXnaColor((int)color));
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
        if (ActiveWindow == null || ActiveWindow.Controls.Count == 0)
            return;

        var controls = ActiveWindow.Controls;
        int currentIndex = controls.IndexOf(ActiveWindow.ActiveControl);
        int count = controls.Count;
        int nextIndex = (currentIndex + 1) % count;

        // Try to find the next enabled and visible control
        for (int i = 0; i < count; i++)
        {
            var ctrl = controls[nextIndex];
            if (ctrl.Enabled && ctrl.Visible && ctrl.Type == ControlType.TextBox)
            {
                ActiveWindow.ActiveControl = ctrl;
                // Optionally, trigger focus event/callback here
                return;
            }
            nextIndex = (nextIndex + 1) % count;
        }
    }
}