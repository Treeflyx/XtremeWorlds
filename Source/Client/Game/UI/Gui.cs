using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using Client.Game.Objects;
using Client.Net;
using Core.Configurations;
using Core.Globals;
using Microsoft.VisualBasic;
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
    public static long ActiveWindow;

    // GUi parts
    public static Type.ControlPart DragBox;

    // Used for automatically the zOrder
    public static int ZOrderWin;
    public static int ZOrderCon;

    // Declare a timer to control when dragging can begin
    private static readonly Stopwatch _dragTimer = new();
    private const double DragInterval = 100d; // Set the interval in milliseconds to start dragging
    private static bool _canDrag; // Flag to control when dragging is allowed
    private static bool _isDragging;

    public static void UpdateControl(
        long windowIndex, int zOrder, string name,
        Color color, ControlType tType,
        List<Design> design,
        List<long> image,
        List<string> texture,
        List<Action> callback,
        int left = 0, int top = 0, int width = 0, int height = 0,
        bool visible = true,
        bool canDrag = false,
        int max = 0,
        int min = 0,
        int value = 0,
        string text = "",
        Alignment align = 0,
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
        long group = 0L,
        byte length = Constant.NameLength,
        bool enabled = true)
    {
        // Ensure the window exists in the Windows collection
        if (!Windows.TryGetValue(windowIndex, out var w))
        {
            return;
        }

        // Create a new instance of Control with specified properties
        var newControl = new Control
        {
            Name = name,
            Type = tType,
            Left = left,
            Top = top,
            OrigLeft = left,
            OrigTop = top,
            Width = (int) (width * SettingsManager.Instance.Scale),
            Height = (int) (height * SettingsManager.Instance.Scale),
            Visible = visible,
            CanDrag = canDrag,
            Max = max,
            Min = min,
            Value = value,
            Text = text,
            Length = length,
            Align = align,
            Font = font,
            Color = color,
            Alpha = alpha,
            ClickThrough = clickThrough,
            XOffset = xOffset,
            YOffset = yOffset,
            ZChange = zChange,
            ZOrder = zOrder,
            Enabled = enabled,
            OnDraw = onDraw,
            Tooltip = tooltip,
            Group = @group,
            Censor = censor,
            Icon = icon,
            Design = design,
            Image = image,
            Texture = texture,
            CallBack = callback
        };

        // Add the new control to the specified window's controls list
        w.Controls ??=
        [
            new Control()
        ];

        w.Controls.Add(newControl);

        // Update active control if necessary
        if (isActive)
            w.ActiveControl = w.Controls.Count - 1;

        // set the zOrder
        ZOrderCon++;
    }

    public static void UpdateZOrder(long winNum, bool forced = false)
    {
        var window = Windows[winNum];

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
                Windows[i].ZOrder -= 1L;
            }
        }

        window.ZOrder = Windows.Count - 1;
    }

    public static void Combobox_AddItem(string windowName, int controlIndex, string text)
    {
        // Ensure the List property is initialized as a List(Of String) in Control class
        if (Windows[Conversions.ToLong(windowName)].Controls[controlIndex].List is null)
        {
            Windows[Conversions.ToLong(windowName)].Controls[controlIndex].List = new List<string>();
        }

        Windows[Conversions.ToLong(windowName)].Controls[controlIndex].List.Add(text);
    }

    public static int UpdateWindow(
        string name, string caption, Font font, long zOrder,
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
        bool isActive = true,
        bool clickThrough = false)
    {
        var stateCount = Enum.GetValues(typeof(ControlState)).Length;
        var design = new List<Design>(Enumerable.Repeat((Design) 0, stateCount));
        var image = new List<long>(Enumerable.Repeat(0L, stateCount));
        var callback = new List<Action?>(Enumerable.Repeat((Action?) null, stateCount));

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
            Type = ControlType.Window,
            Left = left,
            Top = top,
            OrigLeft = left,
            OrigTop = top,
            Width = (int) (width * SettingsManager.Instance.Scale),
            Height = (int) (height * SettingsManager.Instance.Scale),
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

        // Set the active window if visible
        if (visible)
        {
            ActiveWindow = Windows.Count;
        }

        return Windows.Count;
    }

    public static void UpdateTextbox(
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
        var design = new List<Design>(Enumerable.Repeat(Design.None, stateCount).ToList());
        var image = new List<long>(Enumerable.Repeat(0L, stateCount).ToList());
        var texture = new List<string>(Enumerable.Repeat(DataPath.Designs, stateCount).ToList());
        var callback = new List<Action>(Enumerable.Repeat((Action) null, stateCount).ToList());

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
        callback[(int) ControlState.FocusEnter] = callbackEnter;

        // Control the textbox
        UpdateControl(windowIndex, ZOrderCon, name, Color.White, ControlType.TextBox, design, image, texture, callback, left, top, width, height, visible, text: text, align: align, font: font, alpha: alpha, xOffset: xOffset, yOffset: yOffset, censor: censor, icon: icon, isActive: isActive, length: length);
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
        var image = new List<long>(Enumerable.Repeat(0L, stateCount));
        var texture = new List<string>(Enumerable.Repeat(string.Empty, stateCount));
        var callback = new List<Action>(Enumerable.Repeat((Action) null, stateCount));

        if (string.IsNullOrEmpty(texturePath))
        {
            texturePath = DataPath.Gui;
        }

        // fill temp arrays
        design[(int) ControlState.Normal] = designNorm;
        design[(int) ControlState.Hover] = designHover;
        design[(int) ControlState.MouseDown] = designMousedown;
        image[(int) ControlState.Normal] = imageNorm;
        image[(int) ControlState.Hover] = imageHover;
        image[(int) ControlState.MouseDown] = imageMousedown;
        texture[(int) ControlState.Normal] = texturePath;
        texture[(int) ControlState.Hover] = texturePath;
        texture[(int) ControlState.MouseDown] = texturePath;

        callback[(int) ControlState.Normal] = callbackNorm;
        callback[(int) ControlState.Hover] = callbackHover;
        callback[(int) ControlState.MouseDown] = callbackMousedown;
        callback[(int) ControlState.MouseMove] = callbackMousemove;
        callback[(int) ControlState.DoubleClick] = callbackDblclick;

        // Control the box
        UpdateControl(windowIndex, ZOrderCon, name, Color.White, ControlType.PictureBox, design, image, texture, callback, left, top, width, height, visible, canDrag, alpha: alpha, clickThrough: clickThrough, xOffset: 0, yOffset: 0, onDraw: onDraw);
    }

    public static void UpdateButton(
        int winNum, string name,
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
        var image = new List<long>(Enumerable.Repeat(0L, stateCount).ToList());
        var texture = new List<string>(Enumerable.Repeat(DataPath.Designs, stateCount).ToList());
        var callback = new List<Action>(Enumerable.Repeat((Action) null, stateCount).ToList());

        // fill temp arrays
        design[(int) ControlState.Normal] = designNorm;
        design[(int) ControlState.Hover] = designHover;
        design[(int) ControlState.MouseDown] = designMousedown;
        image[(int) ControlState.Normal] = imageNorm;
        image[(int) ControlState.Hover] = imageHover;
        image[(int) ControlState.MouseDown] = imageMousedown;
        texture[(int) ControlState.Normal] = DataPath.Gui;
        texture[(int) ControlState.Hover] = DataPath.Gui;
        texture[(int) ControlState.MouseDown] = DataPath.Gui;
        callback[(int) ControlState.Normal] = callbackNorm;
        callback[(int) ControlState.Hover] = callbackHover;
        callback[(int) ControlState.MouseDown] = callbackMousedown;
        callback[(int) ControlState.MouseMove] = callbackMousemove;
        callback[(int) ControlState.DoubleClick] = callbackDblclick;

        // Control the button 
        UpdateControl(winNum, ZOrderCon, name, Color.White, ControlType.Button, design, image, texture, callback, left, top, width, height, visible, text: text, font: font, clickThrough: Conversions.ToBoolean(alpha), xOffset: xOffset, yOffset: yOffset, censor: censor, icon: icon, tooltip: tooltip);
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
        var controlStateCount = Enum.GetValues(typeof(ControlState)).Length;
        var designLabel = new List<Design>(Enumerable.Repeat(Design.None, controlStateCount).ToList());
        var imageLabel = new List<long>(Enumerable.Repeat(0L, controlStateCount).ToList());
        var textureLabel = new List<string>(Enumerable.Repeat(DataPath.Designs, controlStateCount).ToList());
        var callbackLabel = new List<Action>(Enumerable.Repeat((Action) null, controlStateCount).ToList());

        // fill temp arrays
        callbackLabel[(int) ControlState.Normal] = callbackNorm;
        callbackLabel[(int) ControlState.Hover] = callbackHover;
        callbackLabel[(int) ControlState.MouseDown] = callbackMousedown;
        callbackLabel[(int) ControlState.MouseMove] = callbackMousemove;
        callbackLabel[(int) ControlState.DoubleClick] = callbackDblclick;

        // Control the label
        UpdateControl(windowIndex, ZOrderCon, name, Color.White, ControlType.Label, designLabel, imageLabel, textureLabel, callbackLabel,
            left, top, width, height, visible, text: text, align: align, font: font, clickThrough: Conversions.ToBoolean(alpha),
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
        var image = new List<long>(Enumerable.Repeat(0L, stateCount).ToList());
        var texture = new List<string>(Enumerable.Repeat(DataPath.Designs, stateCount).ToList());
        var callback = new List<Action>(Enumerable.Repeat((Action) null, stateCount).ToList());

        design[0] = theDesign;
        texture[0] = DataPath.Gui;

        // fill temp arrays
        callback[(int) ControlState.Normal] = callbackNorm;
        callback[(int) ControlState.Hover] = callbackHover;
        callback[(int) ControlState.MouseDown] = callbackMousedown;
        callback[(int) ControlState.MouseMove] = callbackMousemove;
        callback[(int) ControlState.DoubleClick] = callbackDblclick;

        // Control the box
        UpdateControl(windowIndex, ZOrderCon, name, Color.White, ControlType.Checkbox, design, image,
            texture, callback, left, top, width, height, visible, value: value,
            text: text, align: align, font: font, clickThrough: Conversions.ToBoolean(alpha), censor: censor, @group: group);
    }

    public static void UpdateComboBox(int windowIndex, string name, int left, int top, int width, int height, Design design)
    {
        // Get the number of ControlState enum values
        var controlStateCount = Enum.GetValues(typeof(ControlState)).Length;

        // Initialize lists for the control states
        var theDesign = new List<Design>(Enumerable.Repeat(Design.None, controlStateCount).ToList());
        var image = new List<long>(Enumerable.Repeat(0L, controlStateCount).ToList());
        var texture = new List<string>(Enumerable.Repeat(DataPath.Gui, controlStateCount).ToList());
        var callback = new List<Action>(Enumerable.Repeat((Action) null, controlStateCount).ToList());

        // Set the design for the normal state
        theDesign[0] = design;
        texture[0] = DataPath.Gui;

        // Update the control in the window using the updated lists
        UpdateControl(windowIndex, ZOrderCon, name, Color.White, ControlType.ComboMenu, theDesign, image, texture, callback, left, top, width, height);
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
                Windows[windowIndex].ActiveControl = controlIndex;
                return true;
        }

        return false;
    }

    public static int ActivateControl(int startIndex = 0, bool skipLast = true)
    {
        while (true)
        {
            var currentActive = Windows[ActiveWindow].ActiveControl;
            var lastControl = Windows[ActiveWindow].LastControl;

            // Ensure the starting index is correct
            if (startIndex <= currentActive)
            {
                startIndex = currentActive + 1;
            }

            // Attempt to activate the next available control, starting from the given index
            for (int i = startIndex, loopTo = Windows[ActiveWindow].Controls.Count - 1; i <= loopTo; i++)
            {
                if (i != currentActive && (!skipLast || i != lastControl))
                {
                    if (SetActiveControl(ActiveWindow, i))
                    {
                        return i; // Return the index of the control that was activated
                    }
                }
            }

            // If we reached the end, wrap around and start from the beginning
            for (int i = 0, loopTo1 = startIndex - 1; i <= loopTo1; i++)
            {
                if (i != currentActive && (!skipLast || i != lastControl))
                {
                    if (SetActiveControl(ActiveWindow, i))
                    {
                        return i; // Return the index of the control that was activated
                    }
                }
            }

            // No control was activated, return 0 or handle as needed
            if (skipLast)
            {
                startIndex = 0;
                skipLast = false;
                continue;
            }

            return 0;
        }
    }

    public static void CentralizeWindow(int windowIndex)
    {
        var window = Windows[windowIndex];

        window.Left = (int) Math.Round(GameState.ResolutionWidth / 2d - window.Width / 2d);
        window.Top = (int) Math.Round(GameState.ResolutionHeight / 2d - window.Height / 2d);
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

        ActiveWindow = windowIndex;
        if (!resetPosition)
        {
            return;
        }

        var window = Windows[windowIndex];

        window.Left = window.OrigLeft;
        window.Top = window.OrigTop;
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
            if (Windows[i].Visible & Windows[i].ZChange != 1)
            {
                continue;
            }

            ActiveWindow = i;
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
        var ui = Ui.Instance;
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
        int i;
        var curWindow = 0;
        var curControl = 0;

        // Check for MouseDown to start the drag timer
        if (GameClient.IsMouseButtonDown(MouseButton.Left) && GameClient.PreviousMouseState.LeftButton == ButtonState.Released)
        {
            _dragTimer.Restart(); // Start the timer on initial mouse down
            _canDrag = false; // Reset drag flag to ensure it doesn't drag immediately
        }

        // Check for MouseUp to reset dragging
        if (GameClient.IsMouseButtonUp(MouseButton.Left))
        {
            _isDragging = false;
            _dragTimer.Reset(); // Stop the timer on mouse up
        }

        // Enable dragging if the mouse has been held down for the specified interval
        if (_dragTimer.ElapsedMilliseconds >= DragInterval)
        {
            _canDrag = true;
        }
        else
        {
            _canDrag = false;
        }

        lock (GameClient.InputLock)
        {
            // Find the container
            var loopTo = Windows.Count;
            for (i = 1; i < loopTo; i++)
            {
                var withBlock = Windows[i];
                if (withBlock.Enabled && withBlock.Visible)
                {
                    if (withBlock.State != ControlState.MouseDown)
                        withBlock.State = ControlState.Normal;

                    if (GameState.CurMouseX >= withBlock.Left && GameState.CurMouseX <= withBlock.Width + withBlock.Left && GameState.CurMouseY >= withBlock.Top && GameState.CurMouseY <= withBlock.Height + withBlock.Top)
                    {
                        // Handle combo menu logic
                        if (withBlock.Design[0] == Design.ComboMenuNormal)
                        {
                            if (entState == ControlState.MouseMove || entState == ControlState.Hover)
                            {
                                ComboMenu_MouseMove(i);
                            }
                            else if (entState == ControlState.MouseDown)
                            {
                                ComboMenu_MouseDown(i);
                            }
                        }

                        // Track the top-most window
                        if (curWindow == 0L || withBlock.ZOrder > Windows[curWindow].ZOrder)
                        {
                            curWindow = i;
                            _isDragging = true;
                        }

                        if (ActiveWindow > 0)
                        {
                            if (!Windows[ActiveWindow].Visible || !Windows[ActiveWindow].Enabled || !Windows[ActiveWindow].CanDrag)
                                ActiveWindow = curWindow;
                        }
                        else
                        {
                            ActiveWindow = curWindow;
                        }
                    }

                    // Handle window dragging only if dragging is enabled
                    if (entState == ControlState.MouseMove && GameClient.IsMouseButtonDown(MouseButton.Left))
                    {
                        if (ActiveWindow > 0 && _isDragging)
                        {
                            withBlock = Windows[ActiveWindow];
                            if (_canDrag && withBlock.CanDrag && withBlock.Enabled && withBlock.Visible)
                            {
                                withBlock.Left = GameLogic.Clamp(withBlock.Left + (GameState.CurMouseX - withBlock.Left - withBlock.MovedX), 0, GameState.ResolutionWidth - withBlock.Width);
                                withBlock.Top = GameLogic.Clamp(withBlock.Top + (GameState.CurMouseY - withBlock.Top - withBlock.MovedY), 0, GameState.ResolutionHeight - withBlock.Height);
                                break;
                            }
                        }
                    }
                }
            }

            if (curWindow > 0L)
            {
                // Handle the active window's callback
                var callBack = Windows[curWindow].CallBack[(int) entState];

                // Execute the callback if it exists
                callBack?.Invoke();

                if (Windows[curWindow].Controls is not null)
                {
                    // Handle controls in the active window
                    var loopTo1 = (long) (Windows[curWindow].Controls?.Count - 1);
                    for (i = 0; i <= loopTo1; i++)
                    {
                        var withBlock1 = Windows[curWindow].Controls[i];

                        if (withBlock1.Enabled && withBlock1.Visible)
                        {
                            if (GameState.CurMouseX >= withBlock1.Left + Windows[curWindow].Left && GameState.CurMouseX <= withBlock1.Left + withBlock1.Width + Windows[curWindow].Left && GameState.CurMouseY >= withBlock1.Top + Windows[curWindow].Top && GameState.CurMouseY <= withBlock1.Top + withBlock1.Height + Windows[curWindow].Top)
                            {
                                if (curControl == 0L || withBlock1.ZOrder > Windows[curWindow].Controls[curControl].ZOrder)
                                {
                                    curControl = i;
                                }
                            }

                            if (_isDragging)
                            {
                                // Handle control dragging only if dragging is enabled
                                if (entState == ControlState.MouseMove && withBlock1.CanDrag && _canDrag && GameClient.IsMouseButtonDown(MouseButton.Left))
                                {
                                    withBlock1.Left = GameLogic.Clamp(withBlock1.Left + (GameState.CurMouseX - withBlock1.Left - withBlock1.MovedX), 0, Windows[curWindow].Width - withBlock1.Width);
                                    withBlock1.Top = GameLogic.Clamp(withBlock1.Top + (GameState.CurMouseY - withBlock1.Top - withBlock1.MovedY), 0, Windows[curWindow].Height - withBlock1.Height);
                                }
                            }
                        }
                    }
                }

                if (curControl > 0L)
                {
                    // Reset all control states
                    for (var j = 0; j < Windows[curWindow].Controls.Count; j++)
                    {
                        if (curControl != j)
                            Windows[curWindow].Controls[j].State = ControlState.Normal;
                    }

                    var withBlock2 = Windows[curWindow].Controls[curControl];

                    // Handle hover state separately
                    if (entState == ControlState.MouseMove)
                    {
                        withBlock2.State = ControlState.Hover;
                    }
                    else if (entState == ControlState.MouseDown)
                    {
                        withBlock2.State = ControlState.MouseDown;
                    }

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
                                for (i = 0; i < Windows[curWindow].Controls.Count; i++)
                                {
                                    if (Windows[curWindow].Controls[i].Type == ControlType.Checkbox &&
                                        Windows[curWindow].Controls[i].Group == withBlock2.Group)
                                    {
                                        Windows[curWindow].Controls[i].Value = 0;
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

                    callBack = withBlock2.CallBack[(int) entState];

                    // Execute the callback if it exists
                    callBack?.Invoke();
                }
            }

            if (curWindow == 0)
            {
                ResetInterface();
            }

            // Reset mouse state on MouseUp
            if (entState == ControlState.MouseUp)
                ResetMouseDown();
        }

        return true;
    }

    public static void ResetInterface()
    {
        var loopTo = Windows.Count;
        for (var i = 1L; i < loopTo; i++)
        {
            if (Windows[i].State != ControlState.MouseDown)
                Windows[i].State = ControlState.Normal;

            if (Windows[i].Controls is null || Windows[i].Controls.Count == 0)
                continue;

            var loopTo1 = (long) Windows[i].Controls.Count;
            for (var x = 0L; x < loopTo1; x++)
            {
                if (Windows[i].Controls[(int) x].State != ControlState.MouseDown)
                    Windows[i].Controls[(int) x].State = ControlState.Normal;
            }
        }
    }

    public static void ResetMouseDown()
    {
        Action callBack;

        lock (GameClient.InputLock)
        {
            var loopTo = Windows.Count;
            for (var i = 1L; i < loopTo; i++)
            {
                var withBlock = Windows[i];
                // Only reset the state if it was in MouseDown
                if (withBlock.State == ControlState.MouseDown)
                {
                    withBlock.State = ControlState.Normal;
                    callBack = withBlock.CallBack[(int) ControlState.Normal];
                    if (callBack is not null)
                        callBack?.Invoke();
                }

                // Check if Controls is not Nothing and has at least one element
                if (withBlock.Controls is not null && withBlock.Controls.Count > 0)
                {
                    var loopTo1 = (long) (withBlock.Controls.Count - 1);
                    for (var x = 0L; x <= loopTo1; x++)
                    {
                        var control = withBlock.Controls[(int) x];

                        // Only reset the state if it was in MouseDown
                        if (control.State == ControlState.MouseDown)
                        {
                            control.State = ControlState.Normal;

                            callBack = control.CallBack[(int) control.State];
                            if (callBack is not null)
                                callBack?.Invoke();
                        }
                    }
                }
            }
        }
    }

    public static void Render()
    {
        // Exit if no windows are present
        if (Windows.Count == 0)
            return;

        // Reset Z-order

        // Loop through each window based on Z-order
        var loopTo = Windows.Count - 1;
        for (var curZOrder = 0L; curZOrder <= loopTo; curZOrder++)
        {
            for (int i = 1, loopTo1 = Windows.Count; i <= loopTo1; i++)
            {
                if (curZOrder == Windows[i].ZOrder && Windows[i].Visible)
                {
                    // Render the window
                    WindowRenderer.Render(i);

                    // Render visible controls within the window
                    for (int? x = 0, loopTo2 = Windows[i].Controls?.Count - 1; x <= loopTo2; x++)
                    {
                        if (Windows[i].Controls[(int) x].Visible)
                        {
                            RenderControl(i, (long) x);
                        }
                    }
                }
            }
        }
    }

    public static void RenderControl(long winNum, long entNum)
    {
        double height;
        double width;
        var textArray = default(string[]);
        long count;
        long i;
        var taddText = default(string);
        var yOffset = 0L;
        long sprite;
        var left = 0L;

        // Check if the window and Control exist
        if ((winNum < 0L | winNum >= Windows.Count || entNum < 0L) | entNum > Windows[winNum].Controls.Count - 1)
        {
            return;
        }

        // Get the window's position offsets
        var xO = Windows[winNum].Left;
        var yO = Windows[winNum].Top;

        {
            var withBlock = Windows[winNum].Controls[(int) entNum];
            switch (withBlock.Type)
            {
                case ControlType.PictureBox:
                {
                    if (withBlock.Design[(int) withBlock.State] > 0L)
                    {
                        DesignRenderer.Render(withBlock.Design[(int) withBlock.State], withBlock.Left + xO, withBlock.Top + yO, withBlock.Width, withBlock.Height, withBlock.Alpha);
                    }

                    if (withBlock.Image[(int) withBlock.State] > 0L)
                    {
                        var argpath = Path.Combine(withBlock.Texture[(int) withBlock.State], withBlock.Image[(int) withBlock.State].ToString());
                        GameClient.RenderTexture(ref argpath, withBlock.Left + xO, withBlock.Top + yO, 0, 0, withBlock.Width, withBlock.Height, withBlock.Width, withBlock.Height, (byte) withBlock.Alpha);
                    }

                    break;
                }

                case ControlType.TextBox:
                {
                    // Render the design if available
                    if (withBlock.Design[(int) withBlock.State] > 0L)
                    {
                        DesignRenderer.Render(withBlock.Design[(int) withBlock.State], withBlock.Left + xO, withBlock.Top + yO, withBlock.Width, withBlock.Height, withBlock.Alpha);
                    }

                    // Render the image if present
                    if (withBlock.Image[(int) withBlock.State] > 0L)
                    {
                        var argpath1 = Path.Combine(withBlock.Texture[(int) withBlock.State], withBlock.Image[(int) withBlock.State].ToString());
                        GameClient.RenderTexture(ref argpath1, withBlock.Left + xO, withBlock.Top + yO, 0, 0, withBlock.Width, withBlock.Height, withBlock.Width, withBlock.Height, (byte) withBlock.Alpha);
                    }

                    // Handle active window text input
                    if (ActiveWindow == winNum & Windows[winNum].ActiveControl == entNum)
                    {
                        taddText = GameState.ChatShowLine;
                    }

                    // Final text with potential censoring and additional input
                    var finalText = (withBlock.Censor ? Text.CensorText(withBlock.Text) : withBlock.Text) + taddText;

                    // Remove vbNullChar from the finalText
                    finalText = finalText.Replace("\0", string.Empty);

                    // Measure the text size
                    var actualSize = Text.Fonts[withBlock.Font].MeasureString(FilterUnsupportedCharacters(finalText, withBlock.Font));
                    var actualWidth = actualSize.X;
                    var actualHeight = actualSize.Y;

                    // Apply padding and calculate position
                    left = withBlock.Left + xO + withBlock.XOffset;
                    var top = withBlock.Top + yO + withBlock.YOffset + (withBlock.Height - actualHeight) / 2.0d;

                    // Render the final text
                    Text.RenderText(finalText, (int) left, (int) Math.Round(top), withBlock.Color, Color.Black, withBlock.Font);
                    break;
                }

                case ControlType.Button:
                {
                    // Render the button design if defined
                    if (withBlock.Design[(int) withBlock.State] > 0L)
                    {
                        DesignRenderer.Render(withBlock.Design[(int) withBlock.State], withBlock.Left + xO, withBlock.Top + yO, withBlock.Width, withBlock.Height);
                    }

                    // Enqueue the button image if present
                    if (withBlock.Image[(int) withBlock.State] > 0L)
                    {
                        var argpath2 = Path.Combine(withBlock.Texture[(int) withBlock.State], withBlock.Image[(int) withBlock.State].ToString());
                        GameClient.RenderTexture(ref argpath2, withBlock.Left + xO, withBlock.Top + yO, 0, 0, withBlock.Width, withBlock.Height, withBlock.Width, withBlock.Height);
                    }

                    // Render the icon if available
                    if (withBlock.Icon > 0L)
                    {
                        var gfxInfo = GameClient.GetGfxInfo(Path.Combine(DataPath.Items, withBlock.Icon.ToString()));
                        if (gfxInfo == null)
                            break;
                        var iconWidth = gfxInfo.Width;
                        var iconHeight = gfxInfo.Height;

                        var argpath3 = Path.Combine(DataPath.Items, withBlock.Icon.ToString());
                        GameClient.RenderTexture(ref argpath3, withBlock.Left + xO + withBlock.XOffset, withBlock.Top + yO + withBlock.YOffset, 0, 0, iconWidth, iconHeight, iconWidth, iconHeight);
                    }

                    // Measure button text size and apply padding
                    var textSize = Text.Fonts[withBlock.Font].MeasureString(FilterUnsupportedCharacters(withBlock.Text, withBlock.Font));
                    var actualWidth = textSize.X;
                    var actualHeight = textSize.Y;

                    // Calculate horizontal and vertical centers with padding
                    var padding = actualWidth / 6.0d;
                    var horCentre = withBlock.Left + xO + withBlock.XOffset + (withBlock.Width - actualWidth) / 2.0d + padding - 4d;
                    padding = actualHeight / 6.0d;
                    var verCentre = withBlock.Top + yO + withBlock.YOffset + (withBlock.Height - actualHeight) / 2.0d + padding;

                    // Render the button's text
                    Text.RenderText(withBlock.Text, (int) Math.Round(horCentre), (int) Math.Round(verCentre), withBlock.Color, Color.Black, withBlock.Font);
                    break;
                }

                case ControlType.Label:
                {
                    if (Strings.Len(withBlock.Text) > 0 & withBlock.Font > 0)
                    {
                        switch (withBlock.Align)
                        {
                            case Alignment.Left:
                            {
                                if (Text.GetTextWidth(withBlock.Text, withBlock.Font) > withBlock.Width)
                                {
                                    Text.WordWrap(withBlock.Text, withBlock.Font, withBlock.Width, ref textArray);
                                    count = Information.UBound(textArray);
                                    var loopTo = count;
                                    for (i = 0L; i < loopTo; i++)
                                    {
                                        var actualSize = Text.Fonts[withBlock.Font].MeasureString(FilterUnsupportedCharacters(textArray[(int) i], withBlock.Font));
                                        var actualWidth = actualSize.X;
                                        var padding = actualWidth / 6.0d;
                                        left = (long) Math.Round(withBlock.Left + xO + withBlock.XOffset + padding);

                                        Text.RenderText(textArray[(int) i], (int) left, (int) (withBlock.Top + yO + withBlock.YOffset + yOffset), withBlock.Color, Color.Black, withBlock.Font);
                                        yOffset += 14L;
                                    }
                                }
                                else
                                {
                                    var actualSize = Text.Fonts[withBlock.Font].MeasureString(withBlock.Text);
                                    var actualWidth = actualSize.X;
                                    left = withBlock.Left + xO + withBlock.XOffset;

                                    Text.RenderText(withBlock.Text, (int) left, withBlock.Top + yO + withBlock.YOffset, withBlock.Color, Color.Black, withBlock.Font);
                                }

                                break;
                            }

                            case Alignment.Right:
                            {
                                if (Text.GetTextWidth(withBlock.Text, withBlock.Font) > withBlock.Width)
                                {
                                    Text.WordWrap(withBlock.Text, withBlock.Font, withBlock.Width, ref textArray);
                                    count = Information.UBound(textArray);
                                    var loopTo1 = count;
                                    for (i = 0L; i < loopTo1; i++)
                                    {
                                        var actualSize = Text.Fonts[withBlock.Font].MeasureString(textArray[(int) i]);
                                        var actualWidth = actualSize.X;
                                        var padding = actualWidth / 6.0d;
                                        left = (long) Math.Round(withBlock.Left + withBlock.Width - actualWidth + xO + withBlock.XOffset + padding);

                                        Text.RenderText(textArray[(int) i], (int) left, (int) (withBlock.Top + yO + withBlock.YOffset + yOffset), withBlock.Color, Color.Black, withBlock.Font);
                                        yOffset += 14L;
                                    }
                                }
                                else
                                {
                                    var actualSize = Text.Fonts[withBlock.Font].MeasureString(FilterUnsupportedCharacters(withBlock.Text, withBlock.Font));
                                    var actualWidth = actualSize.X;
                                    left = (long) Math.Round(withBlock.Left + withBlock.Width - actualSize.X + xO + withBlock.XOffset);

                                    Text.RenderText(withBlock.Text, (int) left, withBlock.Top + yO + withBlock.YOffset, withBlock.Color, Color.Black, withBlock.Font);
                                }

                                break;
                            }

                            case Alignment.Center:
                            {
                                if (Text.GetTextWidth(withBlock.Text, withBlock.Font) > withBlock.Width)
                                {
                                    Text.WordWrap(withBlock.Text, withBlock.Font, withBlock.Width, ref textArray);
                                    count = Information.UBound(textArray);

                                    var loopTo2 = count;
                                    for (i = 0L; i < loopTo2; i++)
                                    {
                                        var actualSize = Text.Fonts[withBlock.Font].MeasureString(FilterUnsupportedCharacters(textArray[(int) i], withBlock.Font));
                                        var actualWidth = actualSize.X;
                                        var actualHeight = actualSize.Y;
                                        var padding = actualWidth / 8.0d;
                                        left = (long) Math.Round(withBlock.Left + (withBlock.Width - actualWidth) / 2.0d + xO + withBlock.XOffset + padding - 4d);
                                        var top = withBlock.Top + yO + withBlock.YOffset + yOffset + (withBlock.Height - actualHeight) / 2.0d;

                                        Text.RenderText(textArray[(int) i], (int) left, (int) Math.Round(top), withBlock.Color, Color.Black, withBlock.Font);
                                        yOffset += 14L;
                                    }
                                }
                                else
                                {
                                    var actualSize = Text.Fonts[withBlock.Font].MeasureString(FilterUnsupportedCharacters(withBlock.Text, withBlock.Font));
                                    var actualWidth = actualSize.X;
                                    var actualHeight = actualSize.Y;
                                    var padding = actualWidth / 8.0d;
                                    left = (long) Math.Round(withBlock.Left + (withBlock.Width - actualWidth) / 2.0d + xO + withBlock.XOffset + padding - 4d);
                                    var top = withBlock.Top + yO + withBlock.YOffset + (withBlock.Height - actualHeight) / 2.0d;

                                    Text.RenderText(withBlock.Text, (int) left, (int) Math.Round(top), withBlock.Color, Color.Black, withBlock.Font);
                                }

                                break;
                            }
                        }
                    }

                    break;
                }
                // Checkboxes
                case ControlType.Checkbox:
                {
                    switch (withBlock.Design[0])
                    {
                        case Design.CheckboxNormal:
                        {
                            // empty?
                            if (withBlock.Value == 0L)
                                sprite = 2L;
                            else
                                sprite = 3L;

                            // render box
                            var argpath4 = Path.Combine(withBlock.Texture[0], sprite.ToString());
                            GameClient.RenderTexture(ref argpath4, withBlock.Left + xO, withBlock.Top + yO, 0, 0, 16, 16, 16, 16);

                            // find text position
                            switch (withBlock.Align)
                            {
                                case Alignment.Left:
                                {
                                    left = withBlock.Left + 18L + xO;
                                    break;
                                }
                                case Alignment.Right:
                                {
                                    left = withBlock.Left + 18L + (withBlock.Width - 18L) - Text.GetTextWidth(withBlock.Text, withBlock.Font) + xO;
                                    break;
                                }
                                case Alignment.Center:
                                {
                                    left = (long) Math.Round(withBlock.Left + 18L + (withBlock.Width - 18L) / 2d - Text.GetTextWidth(withBlock.Text, withBlock.Font) / 2d + xO);
                                    break;
                                }
                            }

                            // render text
                            Text.RenderText(withBlock.Text, (int) left, withBlock.Top + yO, withBlock.Color, Color.Black);
                            break;
                        }

                        case Design.CheckboxChat:
                        {
                            withBlock.Alpha = withBlock.Value == 0L ? 150 : 255;

                            // render box
                            var argpath5 = Path.Combine(DataPath.Gui, 51.ToString());
                            GameClient.RenderTexture(ref argpath5, withBlock.Left + xO, withBlock.Top + yO, 0, 0, 49, 23, 49, 23);

                            // render text
                            left = (long) Math.Round(withBlock.Left + 22L - Text.GetTextWidth(withBlock.Text, withBlock.Font) / 2d + xO);
                            Text.RenderText(withBlock.Text, (int) left + 8, (int) (withBlock.Top + yO + 4L), withBlock.Color, Color.Black);
                            break;
                        }

                        case Design.CheckboxBuying:
                        {
                            if (withBlock.Value == 0L)
                                sprite = 58L;
                            else
                                sprite = 56L;
                            var argpath6 = Path.Combine(withBlock.Texture[0], sprite.ToString());
                            GameClient.RenderTexture(ref argpath6, withBlock.Left + xO, withBlock.Top + yO, 0, 0, 49, 20, 49, 20);
                            break;
                        }

                        case Design.CheckboxSelling:
                        {
                            if (withBlock.Value == 0L)
                                sprite = 59L;
                            else
                                sprite = 57L;
                            var argpath7 = Path.Combine(withBlock.Texture[0], sprite.ToString());
                            GameClient.RenderTexture(ref argpath7, withBlock.Left + xO, withBlock.Top + yO, 0, 0, 49, 20, 49, 20);
                            break;
                        }
                    }

                    break;
                }

                // comboboxes
                case ControlType.ComboMenu:
                {
                    switch (withBlock.Design[0])
                    {
                        case Design.ComboBoxNormal:
                        {
                            // draw the background
                            DesignRenderer.Render(Design.TextBlack, withBlock.Left + xO, withBlock.Top + yO, withBlock.Width, withBlock.Height);

                            // render the text
                            if (withBlock.Value > 0L)
                            {
                                if (withBlock.Value <= withBlock.List.Count - 1)
                                {
                                    Text.RenderText(withBlock.List[withBlock.Value], withBlock.Left + xO, withBlock.Top + yO, withBlock.Color, Color.Black);
                                }
                            }

                            // draw the little arrow
                            var argpath8 = Path.Combine(withBlock.Texture[0], "66");
                            GameClient.RenderTexture(ref argpath8, withBlock.Left + xO + withBlock.Width, withBlock.Top + yO, 0, 0, 5, 4, 5, 4);
                            break;
                        }
                    }

                    break;
                }
            }

            if (withBlock.OnDraw is not null)
                withBlock.OnDraw.Invoke();
        }
    }

    public static void RenderControl_Square(int sprite, long x, long y, long width, long height, long borderSize, long alpha = 255L, int windowId = 0)
    {
        var bs =
            // Set the border size
            borderSize;

        // Draw center
        var argpath = Path.Combine(DataPath.Designs, sprite.ToString());
        GameClient.RenderTexture(ref argpath, (int) (x + bs), (int) (y + bs), (int) (bs + 1L), (int) (bs + 1L), (int) (width - bs * 2L), (int) (height - bs * 2L), alpha: (byte) alpha);

        // Draw top side
        var argpath1 = Path.Combine(DataPath.Designs, sprite.ToString());
        GameClient.RenderTexture(ref argpath1, (int) (x + bs), (int) y, (int) bs, 0, (int) (width - bs * 2L), (int) bs, 1, (int) bs, (byte) alpha);

        // Draw left side
        var argpath2 = Path.Combine(DataPath.Designs, sprite.ToString());
        GameClient.RenderTexture(ref argpath2, (int) x, (int) (y + bs), 0, (int) bs, (int) bs, (int) (height - bs * 2L), (int) bs, alpha: (byte) alpha);

        // Draw right side
        var argpath3 = Path.Combine(DataPath.Designs, sprite.ToString());
        GameClient.RenderTexture(ref argpath3, (int) (x + width - bs), (int) (y + bs), (int) (bs + 3L), (int) bs, (int) bs, (int) (height - bs * 2L), (int) bs, alpha: (byte) alpha);

        // Draw bottom side
        var argpath4 = Path.Combine(DataPath.Designs, sprite.ToString());
        GameClient.RenderTexture(ref argpath4, (int) (x + bs), (int) (y + height - bs), (int) bs, (int) (bs + 3L), (int) (width - bs * 2L), (int) bs, 1, (int) bs, (byte) alpha);

        // Draw top left corner
        var argpath5 = Path.Combine(DataPath.Designs, sprite.ToString());
        GameClient.RenderTexture(ref argpath5, (int) x, (int) y, 0, 0, (int) bs, (int) bs, (int) bs, (int) bs, (byte) alpha);

        // Draw top right corner
        var argpath6 = Path.Combine(DataPath.Designs, sprite.ToString());
        GameClient.RenderTexture(ref argpath6, (int) (x + width - bs), (int) y, (int) (bs + 3L), 0, (int) bs, (int) bs, (int) bs, (int) bs, (byte) alpha);

        // Draw bottom left corner
        var argpath7 = Path.Combine(DataPath.Designs, sprite.ToString());
        GameClient.RenderTexture(ref argpath7, (int) x, (int) (y + height - bs), 0, (int) (bs + 3L), (int) bs, (int) bs, (int) bs, (int) bs, (byte) alpha);

        // Draw bottom right corner
        var argpath8 = Path.Combine(DataPath.Designs, sprite.ToString());
        GameClient.RenderTexture(ref argpath8, (int) (x + width - bs), (int) (y + height - bs), (int) (bs + 3L), (int) (bs + 3L), (int) bs, (int) bs, (int) bs, (int) bs, (byte) alpha);
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
        Windows[comboMenuIndex].Group = 0L;

        // Load the menu
        Windows[comboMenuIndex].Visible = true;
        Windows[GetWindowIndex("winComboMenuBG")].Visible = true;
        ShowWindow(GetWindowIndex("winComboMenuBG"), true, false);
        ShowWindow(GetWindowIndex("winComboMenu"), true, false);
    }

    public static void ComboMenu_MouseMove(long curWindow)
    {
        {
            var withBlock = Windows[curWindow];
            var y = GameState.CurMouseY - withBlock.Top;

            // Find the option we're hovering over
            if (withBlock.List.Count > 0)
            {
                var loopTo = (long) (withBlock.List.Count - 1);
                for (var i = 0L; i < loopTo; i++)
                {
                    if (y >= 16L * i & y <= 16L * i)
                    {
                        withBlock.Group = i;
                    }
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
            var loopTo = (long) withBlock.List.Count;
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

    // ##########
    // ## Bars ##
    // ##########
    public static void Bars_OnDraw()
    {
        long width;

        var xO = Windows[GetWindowIndex("winBars")].Left;
        var yO = Windows[GetWindowIndex("winBars")].Top;

        // Bars
        var argpath = Path.Combine(DataPath.Gui, 27.ToString());
        GameClient.RenderTexture(ref argpath, (int) (xO + 15L), (int) (yO + 15L), 0, 0, (int) GameState.BarWidthGuiHp, 13, (int) GameState.BarWidthGuiHp, 13);
        var argpath1 = Path.Combine(DataPath.Gui, 28.ToString());
        GameClient.RenderTexture(ref argpath1, (int) (xO + 15L), (int) (yO + 32L), 0, 0, (int) GameState.BarWidthGuiSp, 13, (int) GameState.BarWidthGuiSp, 13);
        var argpath2 = Path.Combine(DataPath.Gui, 29.ToString());
        GameClient.RenderTexture(ref argpath2, (int) (xO + 15L), (int) (yO + 49L), 0, 0, (int) GameState.BarWidthGuiExp, 13, (int) GameState.BarWidthGuiExp, 13);
    }
    
    // ##############
    // ## Drag Box ##
    // ##############
    public static void DragBox_OnDraw()
    {
        long texNum;

        long winIndex = GetWindowIndex("winDragBox");
        var xO = Windows[winIndex].Left;
        var yO = Windows[winIndex].Top;

        if (DragBox.Type == DraggablePartType.None)
            return;

        // get texture num
        {
            ref var withBlock = ref DragBox;
            switch (withBlock.Type)
            {
                case DraggablePartType.Item:
                {
                    if (withBlock.Value >= 0)
                    {
                        texNum = Data.Item[(int) withBlock.Value].Icon;
                        var argpath = Path.Combine(DataPath.Items, texNum.ToString());
                        GameClient.RenderTexture(ref argpath, xO, yO, 0, 0, 32, 32, 32, 32);
                    }

                    break;
                }

                case DraggablePartType.Skill:
                {
                    if (withBlock.Value >= 0)
                    {
                        texNum = Data.Skill[(int) withBlock.Value].Icon;
                        var argpath1 = Path.Combine(DataPath.Skills, texNum.ToString());
                        GameClient.RenderTexture(ref argpath1, xO, yO, 0, 0, 32, 32, 32, 32);
                    }

                    break;
                }
            }
        }
    }

    public static void DragBox_Check()
    {
        long i;
        var curWindow = 0L;
        long curControl;
        Type.Rect tmpRec;

        long winIndex = GetWindowIndex("winDragBox");

        if (DragBox.Type == DraggablePartType.None)
            return;

        // check for other windows
        var loopTo = Windows.Count;
        for (i = 1L; i < loopTo; i++)
        {
            {
                var withBlock = Windows[i];
                if (withBlock.Visible)
                {
                    // can't drag to self
                    if (withBlock.Name != "winDragBox")
                    {
                        if (GameState.CurMouseX >= withBlock.Left & GameState.CurMouseX <= withBlock.Left + withBlock.Width)
                        {
                            if (GameState.CurMouseY >= withBlock.Top & GameState.CurMouseY <= withBlock.Top + withBlock.Height)
                            {
                                if (curWindow == 0L)
                                    curWindow = i;

                                if (withBlock.ZOrder > Windows[curWindow].ZOrder)
                                    curWindow = i;
                            }
                        }
                    }
                }
            }
        }

        // we have a window - check if we can drop
        if (curWindow > 0)
        {
            switch (Windows[curWindow].Name ?? "")
            {
                case "winBank":
                {
                    if (DragBox.Origin == PartOrigin.Bank)
                    {
                        if (DragBox.Type == DraggablePartType.Item)
                        {
                            // find the slot to switch with
                            for (i = 0L; i <= Constant.MaxBank; i++)
                            {
                                tmpRec.Top = Windows[curWindow].Top + GameState.BankTop + (GameState.BankOffsetY + 32L) * (i / GameState.BankColumns);
                                tmpRec.Bottom = tmpRec.Top + 32d;
                                tmpRec.Left = Windows[curWindow].Left + GameState.BankLeft + (GameState.BankOffsetX + 32L) * (i % GameState.BankColumns);
                                tmpRec.Right = tmpRec.Left + 32d;

                                if (GameState.CurMouseX >= tmpRec.Left & GameState.CurMouseX <= tmpRec.Right)
                                {
                                    if (GameState.CurMouseY >= tmpRec.Top & GameState.CurMouseY <= tmpRec.Bottom)
                                    {
                                        // switch the slots
                                        if (DragBox.Slot != i)
                                        {
                                            Bank.ChangeBankSlots((int) DragBox.Slot, (int) i);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (DragBox.Origin == PartOrigin.Inventory)
                    {
                        if (DragBox.Type == DraggablePartType.Item)
                        {
                            if (Data.Item[GetPlayerInv(GameState.MyIndex, (int) DragBox.Slot)].Type != (byte) ItemCategory.Currency)
                            {
                                Bank.DepositItem((int) DragBox.Slot, 1);
                            }
                            else
                            {
                                GameLogic.Dialogue("Deposit Item", "Enter the deposit quantity.", "", DialogueType.DepositItem, DialogueStyle.Input, DragBox.Slot);
                            }
                        }
                    }

                    break;
                }

                case "winInventory":
                {
                    if (DragBox.Origin == PartOrigin.Inventory)
                    {
                        // it's from the inventory!
                        if (DragBox.Type == DraggablePartType.Item)
                        {
                            // find the slot to switch with
                            for (i = 0L; i < Constant.MaxInv; i++)
                            {
                                tmpRec.Top = Windows[curWindow].Top + GameState.InvTop + (GameState.InvOffsetY + 32L) * (i / GameState.InvColumns);
                                tmpRec.Bottom = tmpRec.Top + 32d;
                                tmpRec.Left = Windows[curWindow].Left + GameState.InvLeft + (GameState.InvOffsetX + 32L) * (i % GameState.InvColumns);
                                tmpRec.Right = tmpRec.Left + 32d;

                                if (GameState.CurMouseX >= tmpRec.Left & GameState.CurMouseX <= tmpRec.Right)
                                {
                                    if (GameState.CurMouseY >= tmpRec.Top & GameState.CurMouseY <= tmpRec.Bottom)
                                    {
                                        // switch the slots
                                        if (DragBox.Slot != i)
                                            Sender.SendChangeInvSlots((int) DragBox.Slot, (int) i);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (DragBox.Origin == PartOrigin.Bank)
                    {
                        if (DragBox.Type == DraggablePartType.Item)
                        {
                            if (Data.Item[GetBank(GameState.MyIndex, (byte) DragBox.Slot)].Type != (byte) ItemCategory.Currency)
                            {
                                Bank.WithdrawItem((byte) DragBox.Slot, 0);
                            }
                            else
                            {
                                GameLogic.Dialogue("Withdraw Item", "Enter the amount you wish to withdraw.", "", DialogueType.WithdrawItem, DialogueStyle.Input, DragBox.Slot);
                            }
                        }
                    }

                    break;
                }

                case "winSkills":
                {
                    if (DragBox.Origin == PartOrigin.SkillTree)
                    {
                        if (DragBox.Type == DraggablePartType.Skill)
                        {
                            // find the slot to switch with
                            for (i = 0L; i < Constant.MaxPlayerSkills; i++)
                            {
                                tmpRec.Top = Windows[curWindow].Top + GameState.SkillTop + (GameState.SkillOffsetY + 32L) * (i / GameState.SkillColumns);
                                tmpRec.Bottom = tmpRec.Top + 32d;
                                tmpRec.Left = Windows[curWindow].Left + GameState.SkillLeft + (GameState.SkillOffsetX + 32L) * (i % GameState.SkillColumns);
                                tmpRec.Right = tmpRec.Left + 32d;

                                if (GameState.CurMouseX >= tmpRec.Left & GameState.CurMouseX <= tmpRec.Right)
                                {
                                    if (GameState.CurMouseY >= tmpRec.Top & GameState.CurMouseY <= tmpRec.Bottom)
                                    {
                                        // switch the slots
                                        if (DragBox.Slot != i)
                                            Sender.SendChangeSkillSlots((int) DragBox.Slot, (int) i);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    break;
                }

                case "winHotbar":
                {
                    if (DragBox.Origin != PartOrigin.None)
                    {
                        if (DragBox.Type != DraggablePartType.None)
                        {
                            // find the slot
                            for (i = 0L; i < Constant.MaxHotbar; i++)
                            {
                                tmpRec.Top = Windows[curWindow].Top + GameState.HotbarTop;
                                tmpRec.Bottom = tmpRec.Top + 32d;
                                tmpRec.Left = Windows[curWindow].Left + GameState.HotbarLeft + i * GameState.HotbarOffsetX;
                                tmpRec.Right = tmpRec.Left + 32d;

                                if (GameState.CurMouseX >= tmpRec.Left & GameState.CurMouseX <= tmpRec.Right)
                                {
                                    if (GameState.CurMouseY >= tmpRec.Top & GameState.CurMouseY <= tmpRec.Bottom)
                                    {
                                        // set the Hotbar slot
                                        if (DragBox.Origin != PartOrigin.Hotbar)
                                        {
                                            if (DragBox.Type == DraggablePartType.Item)
                                            {
                                                Sender.SendSetHotbarSlot((int) PartOrigin.Inventory, (int) i, (int) DragBox.Slot, (int) DragBox.Value);
                                            }
                                            else if (DragBox.Type == DraggablePartType.Skill)
                                            {
                                                Sender.SendSetHotbarSlot((int) PartOrigin.SkillTree, (int) i, (int) DragBox.Slot, (int) DragBox.Value);
                                            }
                                        }
                                        else if (DragBox.Slot != i)
                                            Sender.SendSetHotbarSlot((int) PartOrigin.Hotbar, (int) i, (int) DragBox.Slot, (int) DragBox.Value);

                                        break;
                                    }
                                }
                            }
                        }
                    }

                    break;
                }
            }
        }
        else
        {
            // no windows found - dropping on bare map
            switch (DragBox.Origin)
            {
                case PartOrigin.Inventory:
                {
                    if (Data.Item[GetPlayerInv(GameState.MyIndex, (int) DragBox.Slot)].Type != (byte) ItemCategory.Currency)
                    {
                        Sender.SendDropItem((int) DragBox.Slot, GetPlayerInv(GameState.MyIndex, (int) DragBox.Slot));
                    }
                    else
                    {
                        GameLogic.Dialogue("Drop Item", "Please choose how many to drop.", "", DialogueType.DropItem, DialogueStyle.Input, DragBox.Slot);
                    }

                    break;
                }

                case PartOrigin.SkillTree:
                {
                    Sender.ForgetSkill((int) DragBox.Slot);
                    break;
                }

                case PartOrigin.Hotbar:
                {
                    Sender.SendSetHotbarSlot((int) DragBox.Origin, (int) DragBox.Slot, (int) DragBox.Slot, 0);
                    break;
                }
            }
        }

        // close window
        HideWindow(winIndex);

        {
            ref var withBlock1 = ref DragBox;
            withBlock1.Type = DraggablePartType.None;
            withBlock1.Slot = 0L;
            withBlock1.Origin = PartOrigin.None;
            withBlock1.Value = 0L;
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

        // loop through
        long top = -80;

        // re-size right-click background
        Windows[GetWindowIndex("winRightClickBG")].Width = GameState.ResolutionWidth;
        Windows[GetWindowIndex("winRightClickBG")].Height = GameState.ResolutionHeight;

        // re-size combo background
        Windows[GetWindowIndex("winComboMenuBG")].Width = GameState.ResolutionWidth;
        Windows[GetWindowIndex("winComboMenuBG")].Height = GameState.ResolutionHeight;
    }

    public static void DrawSkills()
    {
        int i;
        long x;

        if (GameState.MyIndex < 0 | GameState.MyIndex > Constant.MaxPlayers)
            return;

        var xO = Windows[GetWindowIndex("winSkills")].Left;
        var yO = Windows[GetWindowIndex("winSkills")].Top;

        var width = Windows[GetWindowIndex("winSkills")].Width;
        var height = Windows[GetWindowIndex("winSkills")].Height;

        // render green
        var argpath = Path.Combine(DataPath.Gui, 34.ToString());
        GameClient.RenderTexture(ref argpath, (int) (xO + 4L), (int) (yO + 23L), 0, 0, (int) (width - 8L), (int) (height - 27L), 4, 4);

        width = 76;
        height = 76;

        var y = yO + 23;
        // render grid - row
        for (i = 0; i <= 3; i++)
        {
            if (i == 3)
                height = 42;
            var argpath1 = Path.Combine(DataPath.Gui, 35.ToString());
            GameClient.RenderTexture(ref argpath1, (int) (xO + 4L), y, 0, 0, width, height, width, height);
            var argpath2 = Path.Combine(DataPath.Gui, 35.ToString());
            GameClient.RenderTexture(ref argpath2, (int) (xO + 80L), y, 0, 0, width, height, width, height);
            var argpath3 = Path.Combine(DataPath.Gui, 35.ToString());
            GameClient.RenderTexture(ref argpath3, (int) (xO + 156L), y, 0, 0, 42, height, 42, height);
            y += 76;
        }

        // actually draw the icons
        for (i = 0; i < Constant.MaxPlayerSkills; i++)
        {
            var skillNum = Data.Player[GameState.MyIndex].Skill[i].Num;
            if (skillNum >= 0L & skillNum < Constant.MaxSkills)
            {
                Database.StreamSkill(skillNum);

                // not dragging?
                if (!(DragBox.Origin == PartOrigin.SkillTree & DragBox.Slot == i))
                {
                    long skillPic = Data.Skill[skillNum].Icon;

                    if (skillPic > 0L & skillPic <= GameState.NumSkills)
                    {
                        var top = yO + GameState.SkillTop + (GameState.SkillOffsetY + 32L) * (i / GameState.SkillColumns);
                        var left = xO + GameState.SkillLeft + (GameState.SkillOffsetX + 32L) * (i % GameState.SkillColumns);

                        var argpath4 = Path.Combine(DataPath.Skills, skillPic.ToString());
                        GameClient.RenderTexture(ref argpath4, (int) left, (int) top, 0, 0, 32, 32, 32, 32);
                    }
                }
            }
        }
    }
    
    // Right Click Menu
    public static void RightClick_Close()
    {
        // close all menus
        HideWindow(GetWindowIndex("winRightClickBG"));
        HideWindow(GetWindowIndex("winPlayerMenu"));
    }

    // Player Menu
    public static void PlayerMenu_Party()
    {
        RightClick_Close();
        Party.SendPartyRequest(GetPlayerName((int) GameState.PlayerMenuIndex));
    }

    public static void PlayerMenu_Trade()
    {
        RightClick_Close();
        Trade.SendTradeRequest(GetPlayerName((int) GameState.PlayerMenuIndex));
    }

    public static void PlayerMenu_Guild()
    {
        RightClick_Close();
        Text.AddText("System not yet in place.", (int) ColorName.BrightRed);
    }

    public static void PlayerMenu_Player()
    {
        RightClick_Close();
        Text.AddText("System not yet in place.", (int) ColorName.BrightRed);
    }

    public static void UpdatePartyInterface()
    {
        long i;
        var image = new long[6];
        var height = 0;

        // unload it if we're not in a party
        if (Data.MyParty.Leader == 0)
        {
            HideWindow(GetWindowIndex("winParty"));
            return;
        }

        // load the window
        ShowWindow(GetWindowIndex("winParty"));

        // fill the controls
        {
            var withBlock = Windows[GetWindowIndex("winParty")];
            // clear controls first
            for (i = 0L; i <= 3L; i++)
            {
                withBlock.Controls[GetControlIndex("winParty", "lblName" + i)].Text = "";
                withBlock.Controls[GetControlIndex("winParty", "picEmptyBar_HP" + i)].Visible = false;
                withBlock.Controls[GetControlIndex("winParty", "picEmptyBar_SP" + i)].Visible = false;
                withBlock.Controls[GetControlIndex("winParty", "picBar_HP" + i)].Visible = false;
                withBlock.Controls[GetControlIndex("winParty", "picBar_SP" + i)].Visible = false;
                withBlock.Controls[GetControlIndex("winParty", "picShadow" + i)].Visible = false;
                withBlock.Controls[GetControlIndex("winParty", "picChar" + i)].Visible = false;
                withBlock.Controls[GetControlIndex("winParty", "picChar" + i)].Value = 0;
            }

            // labels
            var cIn = 0L;

            var loopTo = (long) Data.MyParty.MemberCount;
            for (i = 0L; i < loopTo; i++)
            {
                // cache the index
                var pIndex = Data.MyParty.Member[(int) i];
                if (pIndex > 0L)
                {
                    if (pIndex != GameState.MyIndex)
                    {
                        if (IsPlaying(pIndex))
                        {
                            // name and level
                            withBlock.Controls[GetControlIndex("winParty", "lblName" + cIn)].Visible = true;
                            withBlock.Controls[GetControlIndex("winParty", "lblName" + cIn)].Text = GetPlayerName(pIndex);
                            // picture
                            withBlock.Controls[GetControlIndex("winParty", "picShadow" + cIn)].Visible = true;
                            withBlock.Controls[GetControlIndex("winParty", "picChar" + cIn)].Visible = true;
                            // store the player's index as a value for later use
                            withBlock.Controls[GetControlIndex("winParty", "picChar" + cIn)].Value = pIndex;
                            for (var x = 0L; x <= 4L; x++)
                            {
                                withBlock.Controls[GetControlIndex("winParty", "picChar" + cIn)].Image[(int) x] = GetPlayerSprite(pIndex);
                                withBlock.Controls[GetControlIndex("winParty", "picChar" + cIn)].Texture[(int) x] = DataPath.Characters;
                            }

                            // bars
                            withBlock.Controls[GetControlIndex("winParty", "picEmptyBar_HP" + cIn)].Visible = true;
                            withBlock.Controls[GetControlIndex("winParty", "picEmptyBar_SP" + cIn)].Visible = true;
                            withBlock.Controls[GetControlIndex("winParty", "picBar_HP" + cIn)].Visible = true;
                            withBlock.Controls[GetControlIndex("winParty", "picBar_SP" + cIn)].Visible = true;
                            // increment control usage
                            cIn += 1L;
                        }
                    }
                }
            }

            // update the bars
            GameLogic.UpdatePartyBars();

            // set the window size
            height = Data.MyParty.MemberCount switch
            {
                2 => 78,
                3 => 118,
                4 => 158,
                _ => height
            };

            withBlock.Height = height;
        }
    }

    public static void DrawMenuBg()
    {
        // row 1
        var argpath = Path.Combine(DataPath.Pictures, "1");
        GameClient.RenderTexture(ref argpath, GameState.ResolutionWidth - 512, GameState.ResolutionHeight - 512, 0, 0, 512, 512, 512, 512);
        var argpath1 = Path.Combine(DataPath.Pictures, "2");
        GameClient.RenderTexture(ref argpath1, GameState.ResolutionWidth - 1024, GameState.ResolutionHeight - 512, 0, 0, 512, 512, 512, 512);
        var argpath2 = Path.Combine(DataPath.Pictures, "3");
        GameClient.RenderTexture(ref argpath2, GameState.ResolutionWidth - 1536, GameState.ResolutionHeight - 512, 0, 0, 512, 512, 512, 512);
        var argpath3 = Path.Combine(DataPath.Pictures, "4");
        GameClient.RenderTexture(ref argpath3, GameState.ResolutionWidth - 2048, GameState.ResolutionHeight - 512, 0, 0, 512, 512, 512, 512);

        // row 2
        var argpath4 = Path.Combine(DataPath.Pictures, "5");
        GameClient.RenderTexture(ref argpath4, GameState.ResolutionWidth - 512, GameState.ResolutionHeight - 1024, 0, 0, 512, 512, 512, 512);
        var argpath5 = Path.Combine(DataPath.Pictures, "6");
        GameClient.RenderTexture(ref argpath5, GameState.ResolutionWidth - 1024, GameState.ResolutionHeight - 1024, 0, 0, 512, 512, 512, 512);
        var argpath6 = Path.Combine(DataPath.Pictures, "7");
        GameClient.RenderTexture(ref argpath6, GameState.ResolutionWidth - 1536, GameState.ResolutionHeight - 1024, 0, 0, 512, 512, 512, 512);
        var argpath7 = Path.Combine(DataPath.Pictures, "8");
        GameClient.RenderTexture(ref argpath7, GameState.ResolutionWidth - 2048, GameState.ResolutionHeight - 1024, 0, 0, 512, 512, 512, 512);

        // row 3
        var argpath8 = Path.Combine(DataPath.Pictures, "9");
        GameClient.RenderTexture(ref argpath8, GameState.ResolutionWidth - 512, GameState.ResolutionHeight - 1088, 0, 0, 512, 64, 512, 64);
        var argpath9 = Path.Combine(DataPath.Pictures, "10");
        GameClient.RenderTexture(ref argpath9, GameState.ResolutionWidth - 1024, GameState.ResolutionHeight - 1088, 0, 0, 512, 64, 512, 64);
        var argpath10 = Path.Combine(DataPath.Pictures, "11");
        GameClient.RenderTexture(ref argpath10, GameState.ResolutionWidth - 1536, GameState.ResolutionHeight - 1088, 0, 0, 512, 64, 512, 64);
        var argpath11 = Path.Combine(DataPath.Pictures, "12");
        GameClient.RenderTexture(ref argpath11, GameState.ResolutionWidth - 2048, GameState.ResolutionHeight - 1088, 0, 0, 512, 64, 512, 64);
    }
    
    public static void DrawYourTrade()
    {
        var color = 0L;

        var xo = Windows[GetWindowIndex("winTrade")].Left + Windows[GetWindowIndex("winTrade")].Controls[GetControlIndex("winTrade", "picYour")].Left;
        var yo = Windows[GetWindowIndex("winTrade")].Top + Windows[GetWindowIndex("winTrade")].Controls[GetControlIndex("winTrade", "picYour")].Top;

        // your items
        for (var i = 0L; i < Constant.MaxInv; i++)
        {
            if (Data.TradeYourOffer[(int) i].Num >= 0)
            {
                long itemNum = GetPlayerInv(GameState.MyIndex, Data.TradeYourOffer[(int) i].Num);
                if (itemNum >= 0L & itemNum < Constant.MaxItems)
                {
                    Item.StreamItem((int) itemNum);
                    long itemPic = Data.Item[(int) itemNum].Icon;

                    if (itemPic > 0L & itemPic <= GameState.NumItems)
                    {
                        var top = yo + GameState.TradeTop + (GameState.TradeOffsetY + 32L) * (i / GameState.TradeColumns);
                        var left = xo + GameState.TradeLeft + (GameState.TradeOffsetX + 32L) * (i % GameState.TradeColumns);

                        // draw icon
                        var argpath = Path.Combine(DataPath.Items, itemPic.ToString());
                        GameClient.RenderTexture(ref argpath, (int) left, (int) top, 0, 0, 32, 32, 32, 32);

                        // If item is a stack - draw the amount you have
                        if (Data.TradeYourOffer[(int) i].Value > 1)
                        {
                            var y = top + 20L;
                            var x = left + 1L;
                            var amount = Data.TradeYourOffer[(int) i].Value.ToString();

                            // Draw currency but with k, m, b etc. using a convertion function
                            if (Conversions.ToLong(amount) < 1000000L)
                            {
                                color = (long) ColorName.White;
                            }
                            else if (Conversions.ToLong(amount) > 1000000L & Conversions.ToLong(amount) < 10000000L)
                            {
                                color = (long) ColorName.Yellow;
                            }
                            else if (Conversions.ToLong(amount) > 10000000L)
                            {
                                color = (long) ColorName.BrightGreen;
                            }

                            Text.RenderText(GameLogic.ConvertCurrency(Conversions.ToInteger(amount)), (int) x, (int) y, GameClient.QbColorToXnaColor((int) color), GameClient.QbColorToXnaColor((int) color));
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
            long itemNum = Data.TradeTheirOffer[(int) i].Num;
            if (itemNum >= 0L & itemNum < Constant.MaxItems)
            {
                Item.StreamItem((int) itemNum);
                long itemPic = Data.Item[(int) itemNum].Icon;

                if (itemPic > 0L & itemPic <= GameState.NumItems)
                {
                    var top = yo + GameState.TradeTop + (GameState.TradeOffsetY + 32L) * (i / GameState.TradeColumns);
                    var left = xo + GameState.TradeLeft + (GameState.TradeOffsetX + 32L) * (i % GameState.TradeColumns);

                    // draw icon
                    var argpath = Path.Combine(DataPath.Items, itemPic.ToString());
                    GameClient.RenderTexture(ref argpath, (int) left, (int) top, 0, 0, 32, 32, 32, 32);

                    // If item is a stack - draw the amount you have
                    if (Data.TradeTheirOffer[(int) i].Value > 1)
                    {
                        var y = top + 20L;
                        var x = left + 1L;
                        var amount = Data.TradeTheirOffer[(int) i].Value.ToString();

                        // Draw currency but with k, m, b etc. using a convertion function
                        if (Conversions.ToLong(amount) < 1000000L)
                        {
                            color = (long) ColorName.White;
                        }
                        else if (Conversions.ToLong(amount) > 1000000L & Conversions.ToLong(amount) < 10000000L)
                        {
                            color = (long) ColorName.Yellow;
                        }
                        else if (Conversions.ToLong(amount) > 10000000L)
                        {
                            color = (long) ColorName.BrightGreen;
                        }

                        Text.RenderText(GameLogic.ConvertCurrency(Conversions.ToInteger(amount)), (int) x, (int) y, GameClient.QbColorToXnaColor((int) color), GameClient.QbColorToXnaColor((int) color));
                    }
                }
            }
        }
    }

    public static void UpdateActiveControl(Control modifiedControl)
    {
        // Ensure there is an active window and an active control to update
        if (ActiveWindow > 0L && Windows[ActiveWindow].ActiveControl > 0)
        {
            // Update the control within the active window's Controls array
            Windows[ActiveWindow].Controls[Windows[ActiveWindow].ActiveControl] = modifiedControl;
        }
    }

    public static Control GetActiveControl()
    {
        // Ensure there is an active window and an active control within that window
        if (ActiveWindow > 0L && Windows.ContainsKey(ActiveWindow) && Windows[ActiveWindow].ActiveControl > 0)
        {
            // Return the active control from the active window
            return Windows[ActiveWindow].Controls[Windows[ActiveWindow].ActiveControl];
        }

        // No active control found, return Nothing
        return null;
    }
    
    public static string FilterUnsupportedCharacters(string text, Font fontType)
    {
        if (text == null)
        {
            return string.Empty; // or handle it as appropriate
        }

        var supportedText = new StringBuilder();
        foreach (var ch in text)
        {
            if (Text.Fonts[fontType].Characters.Contains(ch))
            {
                supportedText.Append(ch);
            }
        }

        return supportedText.ToString();
    }
}