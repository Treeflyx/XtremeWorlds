using Core.Globals;
using Microsoft.Xna.Framework;

namespace Client.Game.UI;

public class Control
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
    public int Max { get; set; }
    public int Min { get; set; }
    public int Value { get; set; }
    public string Text { get; set; } = string.Empty;
    public byte Length { get; set; }
    public Alignment Align { get; set; }
    public Font Font { get; set; }
    public Color Color { get; set; }
    public int Alpha { get; set; }
    public bool ClickThrough { get; set; }
    public int XOffset { get; set; }
    public int YOffset { get; set; }
    public byte ZChange { get; set; }
    public int ZOrder { get; set; }
    public bool Enabled { get; set; }
    public Action? OnDraw { get; set; }
    public string Tooltip { get; set; } = string.Empty;
    public long Group { get; set; }
    public bool Censor { get; set; }
    public long Icon { get; set; }
    public ControlState State { get; set; }
    public List<string> List { get; set; } = [];

    // Arrays for states
    public List<Design> Design { get; set; } = [];
    public List<long> Image { get; set; } = [];
    public List<string> Texture { get; set; } = [];
    public List<Action> CallBack { get; set; } = [];
}