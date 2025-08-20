using Core.Globals;
using Microsoft.Xna.Framework;

namespace Client.Game.UI;

public abstract class Control : Component
{
    public int Value { get; set; }
    public string Text { get; set; } = string.Empty;
    public Alignment Align { get; set; } = Alignment.Left;
    public Font Font { get; set; } = Font.Georgia;
    public Color Color { get; set; } = Color.White;
    public int Alpha { get; set; } = 255;
    public int XOffset { get; set; }
    public int YOffset { get; set; }
    public int ZOrder { get; set; }
    public bool Enabled { get; set; } = true;
    public string Tooltip { get; set; } = string.Empty;
    public int Icon { get; set; }
    public ControlState State { get; set; }
    public Design Design { get; set; } = Design.None;
    public Design? DesignHover { get; set; }
    public Design? DesignMouseDown { get; set; }
    public int? Image { get; set; }
    public int? ImageHover { get; set; }
    public int? ImageMouseDown { get; set; }
    public Action? OnDraw { get; set; }

    // Arrays for states
    public List<string> Texture { get; set; } = [];
    public List<Action?> CallBack { get; set; } = [];

    public abstract void Render(int x, int y);

    protected Design GetActiveDesign()
    {
        return State switch
        {
            ControlState.Normal => Design,
            ControlState.Hover => DesignHover ?? Design,
            ControlState.MouseDown => DesignMouseDown ?? Design,
            _ => Design
        };
    }

    protected int? GetActiveImage()
    {
        return State switch
        {
            ControlState.Normal => Image,
            ControlState.Hover => ImageHover ?? Image,
            ControlState.MouseDown => ImageMouseDown ?? Image,
            _ => Image
        };
    }
}