using System.Xml;
using Core.Globals;

namespace Client.Game.UI;

public static class WindowLoader
{
    private const Font DefaultWindowFont = Font.Georgia;
    private const Font DefaultControlFont = Font.Arial;

    public static Window FromLayout(string layoutName)
    {
        var path = Path.Combine("Content", "Skins", "Layouts", layoutName + ".xml");
        if (!File.Exists(path))
        {
            throw new UIException(
                $"Unable to load window layout '{layoutName}'. " +
                $"Layout file '{path}' does not exist.");
        }

        using var stream = File.OpenRead(path);

        using var xmlReader = XmlReader.Create(stream, new XmlReaderSettings
        {
            IgnoreWhitespace = true,
            IgnoreComments = true
        });

        xmlReader.MoveToContent();
        if (xmlReader.NodeType != XmlNodeType.Element ||
            xmlReader.Name != "Window")
        {
            throw new UIException("Window layout file is missing root 'Window' element.");
        }

        var windowIndex = ReadWindow(xmlReader);

        return Gui.Windows[windowIndex];
    }

    private static int ReadWindow(XmlReader xmlReader)
    {
        var name = xmlReader.GetAttribute("Name");
        var caption = xmlReader.GetAttribute("Caption");
        var fontName = xmlReader.GetAttribute("Font");
        var font = GetFontByName(fontName, DefaultWindowFont);
        var size = xmlReader.GetAttribute("Size");
        var sizeVec = GetVector(size);
        var position = xmlReader.GetAttribute("Position");
        var positionVec = GetVector(position);
        var icon = xmlReader.GetAttribute("Icon");
        var iconOffset = xmlReader.GetAttribute("IconOffset");
        var iconOffsetVec = GetVector(iconOffset);
        var designName = xmlReader.GetAttribute("Design");
        var design = GetDesignByName(designName, Design.None);
        var designHoverName = xmlReader.GetAttribute("DesignHover");
        var designHover = GetDesignByName(designHoverName, design);
        var designMousedownName = xmlReader.GetAttribute("DesignMouseDown");
        var designMousedown = GetDesignByName(designMousedownName, design);
        var startPosition = xmlReader.GetAttribute("StartPosition");
        var visible = GetBoolean(xmlReader.GetAttribute("Visible"), true);

        var windowIndex = Gui.CreateWindow(
            name: name ?? string.Empty,
            caption: caption ?? string.Empty,
            font: font,
            zOrder: Gui.ZOrderWin,
            left: positionVec.X,
            top: positionVec.Y,
            width: sizeVec.X,
            height: sizeVec.Y,
            icon: GetIcon(icon),
            visible: visible,
            xOffset: iconOffsetVec.X,
            yOffset: iconOffsetVec.Y,
            designNorm: design,
            designHover: designHover,
            designMousedown: designMousedown);

        if (!string.IsNullOrEmpty(startPosition))
        {
            if (startPosition.Equals("Center", StringComparison.OrdinalIgnoreCase) ||
                startPosition.Equals("CenterScreen", StringComparison.OrdinalIgnoreCase))
            {
                Gui.CentralizeWindow(windowIndex);
            }
        }

        Gui.ZOrderCon = 0;

        while (xmlReader.Read())
        {
            if (xmlReader.NodeType == XmlNodeType.Element)
            {
                ReadControl(xmlReader, windowIndex);
            }
            else if (xmlReader.NodeType == XmlNodeType.EndElement)
            {
                break;
            }
        }

        return windowIndex;
    }

    private static void ReadControl(XmlReader xmlReader, int windowIndex)
    {
        switch (xmlReader.Name)
        {
            case "Button":
                ReadButton(xmlReader, windowIndex);
                break;

            case "CheckBox":
                ReadCheckBox(xmlReader, windowIndex);
                break;

            case "Label":
                ReadLabel(xmlReader, windowIndex);
                break;

            case "PictureBox":
                ReadPictureBox(xmlReader, windowIndex);
                break;

            case "TextBox":
                ReadTextBox(xmlReader, windowIndex);
                break;
        }

        if (!xmlReader.IsEmptyElement)
        {
            xmlReader.Skip();
        }
    }

    private static void ReadLabel(XmlReader xmlReader, int windowIndex)
    {
        var name = xmlReader.GetAttribute("Name");
        var text = xmlReader.GetAttribute("Text");
        var position = xmlReader.GetAttribute("Position");
        var positionVec = GetVector(position);
        var size = xmlReader.GetAttribute("Size");
        var sizeVec = GetVector(size);
        var fontName = xmlReader.GetAttribute("Font");
        var font = GetFontByName(fontName, DefaultControlFont);
        var alignmentName = xmlReader.GetAttribute("Align");
        var alignment = GetAlignmentByName(alignmentName, Alignment.Left);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: name ?? string.Empty,
            text: text ?? string.Empty,
            left: positionVec.X,
            top: positionVec.Y,
            width: sizeVec.X,
            height: sizeVec.Y,
            font: font,
            align: alignment);
    }

    private static void ReadPictureBox(XmlReader xmlReader, int windowIndex)
    {
        var name = xmlReader.GetAttribute("Name");
        var position = xmlReader.GetAttribute("Position");
        var positionVec = GetVector(position);
        var size = xmlReader.GetAttribute("Size");
        var sizeVec = GetVector(size);
        var image = GetInt32(xmlReader.GetAttribute("Image"));
        var imageHover = GetInt32(xmlReader.GetAttribute("ImageHover"), image);
        var imageMousedown = GetInt32(xmlReader.GetAttribute("ImageMouseDown"), image);
        var designName = xmlReader.GetAttribute("Design");
        var design = GetDesignByName(designName, Design.None);
        var designHoverName = xmlReader.GetAttribute("DesignHover");
        var designHover = GetDesignByName(designHoverName, design);
        var designMousedownName = xmlReader.GetAttribute("DesignMouseDown");
        var designMousedown = GetDesignByName(designMousedownName, design);

        Gui.CreatePictureBox(
            windowIndex,
            name ?? string.Empty,
            positionVec.X,
            positionVec.Y,
            sizeVec.X,
            sizeVec.Y,
            imageNorm: image,
            imageHover: imageHover,
            imageMousedown: imageMousedown,
            designNorm: design,
            designHover: designHover,
            designMousedown: designMousedown);
    }

    private static void ReadButton(XmlReader xmlReader, int windowIndex)
    {
        var name = xmlReader.GetAttribute("Name");
        var text = xmlReader.GetAttribute("Text");
        var position = xmlReader.GetAttribute("Position");
        var positionVec = GetVector(position);
        var size = xmlReader.GetAttribute("Size");
        var sizeVec = GetVector(size);
        var fontName = xmlReader.GetAttribute("Font");
        var font = GetFontByName(fontName, DefaultControlFont);
        var image = GetInt32(xmlReader.GetAttribute("Image"));
        var imageHover = GetInt32(xmlReader.GetAttribute("ImageHover"), image);
        var imageMousedown = GetInt32(xmlReader.GetAttribute("ImageMouseDown"), image);
        var designName = xmlReader.GetAttribute("Design");
        var design = GetDesignByName(designName, Design.None);
        var designHoverName = xmlReader.GetAttribute("DesignHover");
        var designHover = GetDesignByName(designHoverName, design);
        var designMousedownName = xmlReader.GetAttribute("DesignMouseDown");
        var designMousedown = GetDesignByName(designMousedownName, design);

        var x = positionVec.X;
        if (x < 0)
        {
            x = Gui.Windows[windowIndex].Width + x;
        }

        var y = positionVec.Y;
        if (y < 0)
        {
            y = Gui.Windows[windowIndex].Height + y;
        }

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: name ?? string.Empty,
            text: text ?? string.Empty,
            left: x, top: y,
            width: sizeVec.X,
            height: sizeVec.Y,
            font: font,
            imageNorm: image,
            imageHover: imageHover,
            imageMousedown: imageMousedown,
            designNorm: design,
            designHover: designHover,
            designMousedown: designMousedown);
    }

    private static void ReadTextBox(XmlReader xmlReader, int windowIndex)
    {
        var name = xmlReader.GetAttribute("Name");
        var text = xmlReader.GetAttribute("Text");
        var position = xmlReader.GetAttribute("Position");
        var positionVec = GetVector(position);
        var size = xmlReader.GetAttribute("Size");
        var sizeVec = GetVector(size);
        var fontName = xmlReader.GetAttribute("Font");
        var font = GetFontByName(fontName, DefaultControlFont);
        var designName = xmlReader.GetAttribute("Design");
        var design = GetDesignByName(designName, Design.TextWhite);
        var designHoverName = xmlReader.GetAttribute("DesignHover");
        var designHover = GetDesignByName(designHoverName, design);
        var designMousedownName = xmlReader.GetAttribute("DesignMouseDown");
        var designMousedown = GetDesignByName(designMousedownName, design);
        var censor = GetBoolean(xmlReader.GetAttribute("Censor"));

        Gui.CreateTextbox(
            windowIndex: windowIndex,
            name: name ?? string.Empty,
            left: positionVec.X,
            top: positionVec.Y,
            width: sizeVec.X,
            height: sizeVec.Y,
            text: text ?? string.Empty,
            font: font,
            xOffset: 5,
            yOffset: 3,
            designNorm: design,
            designHover: designHover,
            designMousedown: designMousedown, censor: censor);
    }

    private static void ReadCheckBox(XmlReader xmlReader, int windowIndex)
    {
        var name = xmlReader.GetAttribute("Name");
        var text = xmlReader.GetAttribute("Text");
        var position = xmlReader.GetAttribute("Position");
        var positionVec = GetVector(position);
        var size = xmlReader.GetAttribute("Size");
        var sizeVec = GetVector(size);
        var fontName = xmlReader.GetAttribute("Font");
        var font = GetFontByName(fontName, DefaultControlFont);
        var designName = xmlReader.GetAttribute("Design");
        var design = GetDesignByName(designName, Design.None);

        Gui.CreateCheckBox(
            windowIndex: windowIndex,
            name: name ?? string.Empty,
            text: text ?? string.Empty,
            left: positionVec.X,
            top: positionVec.Y,
            width: sizeVec.X,
            font: font,
            theDesign: design);
    }

    private static Font GetFontByName(string? fontName, Font defaultValue)
    {
        if (string.IsNullOrEmpty(fontName))
        {
            return defaultValue;
        }

        if (Enum.TryParse<Font>(fontName, true, out var result))
        {
            return result;
        }

        return defaultValue;
    }

    private static Alignment GetAlignmentByName(string? alignmentName, Alignment defaultValue)
    {
        if (string.IsNullOrEmpty(alignmentName))
        {
            return defaultValue;
        }

        if (Enum.TryParse<Alignment>(alignmentName, true, out var result))
        {
            return result;
        }

        return defaultValue;
    }

    private static (int X, int Y) GetVector(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return (0, 0);
        }

        var comma = value.IndexOf(',');
        if (comma == -1)
        {
            return (0, 0);
        }

        if (!int.TryParse(value.AsSpan(0, comma), out var x)) x = 0;
        if (!int.TryParse(value.AsSpan(comma + 1), out var y)) y = 0;

        return (x, y);
    }

    private static int GetIcon(string? icon)
    {
        if (string.IsNullOrEmpty(icon))
        {
            return 0;
        }

        if (int.TryParse(icon, out var result))
        {
            return result;
        }

        return 0;
    }

    private static Design GetDesignByName(string? designName, Design defaultValue)
    {
        if (string.IsNullOrEmpty(designName))
        {
            return defaultValue;
        }

        if (Enum.TryParse<Design>(designName, true, out var result))
        {
            return result;
        }

        return defaultValue;
    }

    private static bool GetBoolean(string? value, bool defaultValue = false)
    {
        if (string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        if (bool.TryParse(value, out var result))
        {
            return result;
        }

        return defaultValue;
    }

    private static int GetInt32(string? value, int defaultValue = 0)
    {
        if (string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        if (int.TryParse(value, out var result))
        {
            return result;
        }

        return defaultValue;
    }
}