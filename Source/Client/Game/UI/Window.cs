using Core.Globals;

namespace Client.Game.UI;

public sealed class Window : Component
{
    public int InitialX { get; set; }
    public int InitialY { get; set; }
    public int MovedX { get; set; }
    public int MovedY { get; set; }
    public bool CanDrag { get; set; } = true;
    public Font Font { get; set; }
    public string Text { get; set; } = string.Empty;
    public int XOffset { get; set; }
    public int YOffset { get; set; }
    public int Icon { get; set; }
    public long Value { get; set; }
    public int Group { get; set; }
    public byte ZChange { get; set; }
    public int ZOrder { get; set; }
    public Action? OnDraw { get; set; }
    public bool ClickThrough { get; set; }
    
    public Control? ParentControl { get; set; }

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
    
    public bool Contains(int x, int y)
    {
        return x >= X && x <= X + Width && y >= Y && y <= Y + Height;
    }
}