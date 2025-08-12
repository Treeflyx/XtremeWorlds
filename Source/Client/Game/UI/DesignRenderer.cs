using Core.Globals;

namespace Client.Game.UI;

public static class DesignRenderer
{
    private static class Textures
    {
        public const string Wood = "1.png";
        public const string Green = "2.png";
        public const string GreenClick = "3.png";
        public const string Red = "4.png";
        public const string RedHover = "5.png";
        public const string RedClick = "6.png";
        public const string Blue = "8.png";
        public const string BlueHover = "9.png";
        public const string BlueClick = "10.png";
        public const string Orange = "11.png";
        public const string OrangeHover = "12.png";
        public const string OrangeClick = "13.png";
        public const string Grey = "14.png";
        public const string Menu = "61.png";
    }

    public static void Render(Design design, int left, int top, int width, int height, int alpha = 255)
    {
        switch (design)
        {
            case Design.MenuHeader:
                RenderMenuHeader(left, top, width, height);
                break;

            case Design.MenuOption:
                RenderMenuOption(left, top, width, height);
                break;

            case Design.Wood:
                RenderWood(left, top, width, height, alpha);
                break;

            case Design.WoodSmall:
                RenderWoodSmall(left, top, width, height, alpha);
                break;

            case Design.WoodEmpty:
                RenderWoodEmpty(left, top, width, height, alpha);
                break;

            case Design.Green:
                RenderGreen(left, top, width, height, alpha);
                break;

            case Design.GreenHover:
                RenderGreenHover(left, top, width, height, alpha);
                break;

            case Design.GreenClick:
                RenderGreenClick(left, top, width, height, alpha);
                break;

            case Design.Red:
                RenderRed(left, top, width, height, alpha);
                break;

            case Design.RedHover:
                RenderRedHover(left, top, width, height, alpha);
                break;

            case Design.RedClick:
                RenderRedClick(left, top, width, height, alpha);
                break;

            case Design.Blue:
                RenderBlue(left, top, width, height, alpha);
                break;

            case Design.BlueHover:
                RenderBlueHover(left, top, width, height, alpha);
                break;

            case Design.BlueClick:
                RenderBlueClick(left, top, width, height, alpha);
                break;

            case Design.Orange:
                RenderOrange(left, top, width, height, alpha);
                break;

            case Design.OrangeHover:
                RenderOrangeHover(left, top, width, height, alpha);
                break;

            case Design.OrangeClick:
                RenderOrangeClick(left, top, width, height, alpha);
                break;

            case Design.Grey:
                RenderGrey(left, top, width, height, alpha);
                break;

            case Design.Parchment:
                RenderParchment(left, top, width, height, alpha);
                break;

            case Design.BlackOval:
                RenderBlackOval(left, top, width, height, alpha);
                break;

            case Design.TextBlack:
                RenderTextBlack(left, top, width, height, alpha);
                break;

            case Design.TextWhite:
                RenderTextWhite(left, top, width, height, alpha);
                break;

            case Design.TextBlackSquare:
                RenderTextBlackSquare(left, top, width, height, alpha);
                break;

            case Design.WindowDescription:
                RenderWindowDescription(left, top, width, height, alpha);
                break;

            case Design.DescriptionPicture:
                RenderDescriptionPicture(left, top, width, height, alpha);
                break;

            case Design.WindowWithShadow:
                RenderWindowWithShadow(left, top, width, height, alpha);
                break;

            case Design.WindowParty:
                RenderWindowParty(left, top, width, height, alpha);
                break;

            case Design.TileSelectionBox:
                RenderTileSelectionBox(left, top, width, height, alpha);
                break;
        }
    }

    private static void RenderMenuHeader(int left, int top, int width, int height)
    {
        var path = Path.Combine(DataPath.Designs, Textures.Menu);

        GameClient.RenderTexture(ref path, left, top, 0, 0, width, height, width, height, 200, 47, 77, 29);
    }

    private static void RenderMenuOption(int left, int top, int width, int height)
    {
        var path = Path.Combine(DataPath.Designs, Textures.Menu);

        GameClient.RenderTexture(ref path, left, top, 0, 0, width, height, width, height, 200, 98, 98, 98);
    }

    private static void RenderWood(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 4;

        RenderSquare(1, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gui, Textures.Wood);

        GameClient.RenderTexture(ref path,
            left + borderSize, top + borderSize, 100, 100,
            width - borderSize * 2, height - borderSize * 2,
            width - borderSize * 2, height - borderSize * 2,
            (byte) alpha);
    }

    private static void RenderWoodSmall(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(8, left + borderSize, top + borderSize, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gui, Textures.Wood);

        GameClient.RenderTexture(ref path,
            left + borderSize, top + borderSize, 100, 100,
            width - borderSize * 2, height - borderSize * 2,
            width - borderSize * 2, height - borderSize * 2,
            (byte) alpha);
    }

    private static void RenderWoodEmpty(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 4;

        RenderSquare(9, left, top, width, height, borderSize, alpha);
    }

    private static void RenderGreen(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(2, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.Wood);

        GameClient.RenderTexture(ref path,
            left + borderSize, top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderGreenHover(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(2, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.Green);

        GameClient.RenderTexture(ref path,
            left + borderSize, top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderGreenClick(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(2, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.GreenClick);

        GameClient.RenderTexture(ref path,
            left + borderSize, top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderRed(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(3, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.Red);

        GameClient.RenderTexture(ref path,
            left + borderSize, top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderRedHover(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(3, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.RedHover);

        GameClient.RenderTexture(ref path,
            left + borderSize, top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderRedClick(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(3, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.RedClick);

        GameClient.RenderTexture(ref path,
            left + borderSize, top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderBlue(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(14, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.Blue);

        GameClient.RenderTexture(ref path,
            left + borderSize,
            top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderBlueHover(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(14, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.BlueHover);

        GameClient.RenderTexture(ref path,
            left + borderSize,
            top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderBlueClick(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(14, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.BlueClick);

        GameClient.RenderTexture(ref path,
            left + borderSize,
            top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderOrange(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(15, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.Orange);

        GameClient.RenderTexture(ref path,
            left + borderSize,
            top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderOrangeHover(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(15, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.OrangeHover);

        GameClient.RenderTexture(ref path, left + borderSize, top + borderSize, 0, 0, width - borderSize * 2, height - borderSize * 2, 128, 128, (byte) alpha);
    }

    private static void RenderOrangeClick(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(15, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.OrangeClick);

        GameClient.RenderTexture(ref path,
            left + borderSize,
            top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderGrey(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 2;

        RenderSquare(17, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, Textures.Grey);

        GameClient.RenderTexture(ref path,
            left + borderSize,
            top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderParchment(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 20;

        RenderSquare(4, left, top, width, height, borderSize, alpha);
    }

    private static void RenderBlackOval(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 4;

        RenderSquare(5, left, top, width, height, borderSize, alpha);
    }

    private static void RenderTextBlack(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 5;

        RenderSquare(6, left, top, width, height, borderSize, alpha);
    }

    private static void RenderTextWhite(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 5;

        RenderSquare(7, left, top, width, height, borderSize, alpha);
    }

    private static void RenderTextBlackSquare(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 4;

        RenderSquare(10, left, top, width, height, borderSize, alpha);
    }

    private static void RenderWindowDescription(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 8;

        RenderSquare(11, left, top, width, height, borderSize, alpha);
    }

    private static void RenderDescriptionPicture(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 3;

        RenderSquare(12, left, top, width, height, borderSize, alpha);

        var path = Path.Combine(DataPath.Gradients, "7");

        GameClient.RenderTexture(ref path,
            left + borderSize,
            top + borderSize, 0, 0,
            width - borderSize * 2,
            height - borderSize * 2,
            128, 128, (byte) alpha);
    }

    private static void RenderWindowWithShadow(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 35;

        RenderSquare(13,
            left - borderSize,
            top - borderSize,
            width + borderSize * 2,
            height + borderSize * 2,
            borderSize, alpha);
    }

    private static void RenderWindowParty(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 12;

        RenderSquare(16, left, top, width, height, borderSize, alpha);
    }

    private static void RenderTileSelectionBox(int left, int top, int width, int height, int alpha)
    {
        const int borderSize = 4;

        RenderSquare(18, left, top, width, height, borderSize, alpha);
    }
    
    private static void RenderSquare(int sprite, int x, int y, int width, int height, int borderSize, int alpha = 255)
    {
        var path = Path.Combine(DataPath.Designs, sprite.ToString());
        
        // Draw center
        GameClient.RenderTexture(ref path, x + borderSize, y + borderSize, borderSize + 1, borderSize + 1, width - borderSize * 2, height - borderSize * 2, alpha: (byte) alpha);

        // Draw top side
        GameClient.RenderTexture(ref path, x + borderSize, y, borderSize, 0, width - borderSize * 2, borderSize, 1, borderSize, (byte) alpha);

        // Draw left side
        GameClient.RenderTexture(ref path, x, y + borderSize, 0, borderSize, borderSize, height - borderSize * 2, borderSize, alpha: (byte) alpha);

        // Draw right side
        GameClient.RenderTexture(ref path, x + width - borderSize, y + borderSize, borderSize + 3, borderSize, borderSize, height - borderSize * 2, borderSize, alpha: (byte) alpha);

        // Draw bottom side
        GameClient.RenderTexture(ref path, x + borderSize, y + height - borderSize, borderSize, borderSize + 3, width - borderSize * 2, borderSize, 1, borderSize, (byte) alpha);

        // Draw top left corner
        GameClient.RenderTexture(ref path, x, y, 0, 0, borderSize, borderSize, borderSize, borderSize, (byte) alpha);

        // Draw top right corner
        GameClient.RenderTexture(ref path, x + width - borderSize, y, borderSize + 3, 0, borderSize, borderSize, borderSize, borderSize, (byte) alpha);

        // Draw bottom left corner
        GameClient.RenderTexture(ref path, x, y + height - borderSize, 0, borderSize + 3, borderSize, borderSize, borderSize, borderSize, (byte) alpha);

        // Draw bottom right corner
        GameClient.RenderTexture(ref path, x + width - borderSize, y + height - borderSize, borderSize + 3, borderSize + 3, borderSize, borderSize, borderSize, borderSize, (byte) alpha);
    }
}