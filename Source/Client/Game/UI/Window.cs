using Core.Globals;

namespace Client.Game.UI;

public class Window
{
    public string Name { get; set; } = string.Empty;
    public ControlType Type { get; set; }
    public int Left { get; set; }
    public int Top { get; set; }
    public int OrigLeft { get; set; }
    public int OrigTop { get; set; }
    public int MovedX { get; set; }
    public int MovedY { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool Visible { get; set; }
    public bool CanDrag { get; set; }
    public Font Font { get; set; }
    public string Text { get; set; } = string.Empty;
    public int XOffset { get; set; }
    public int YOffset { get; set; }
    public int Icon { get; set; }
    public bool Enabled { get; set; }
    public long Value { get; set; }
    public int Group { get; set; }
    public byte ZChange { get; set; }
    public int ZOrder { get; set; }
    public Action? OnDraw { get; set; }
    public bool Censor { get; set; }
    public bool ClickThrough { get; set; }
    public int LinkedToWin { get; set; }
    public int LinkedToCon { get; set; }

    public ControlState State { get; set; }
    public List<string> List { get; set; } = []; // Drop down items?

    // Arrays for states
    public List<Design> Design { get; set; } = [];
    public List<int>? Image { get; set; }
    public List<Action?> CallBack { get; set; } = [];

    // Controls in this window
    public List<Control> Controls { get; } = [];
    public Control? LastControl { get; set; }
    public Control? ActiveControl { get; set; }

    public Control GetChild(string controlName)
    {
        foreach (var control in Controls)
        {
            if (string.Equals(control.Name, controlName, StringComparison.CurrentCultureIgnoreCase))
            {
                return control;
            }
        }

        throw new InvalidOperationException("Control not found: " + controlName);
    }

    public bool SetActiveControl(Control control)
    {
        switch (Type)
        {
            case ControlType.TextBox:
                LastControl = ActiveControl;
                ActiveControl = control;
                return true;
        }

        return false;
    }
}