namespace Core.Globals;

public static class DataPath
{
    public static string Local
    {
        get
        {
            if (OperatingSystem.IsMacOS())
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "XtremeWorlds");
            }

            return Environment.CurrentDirectory;
        }
    }
    
    // Use the application base directory so running from bin/Build works and finds Content next to the executable
    public static string Asset => Path.Combine(AppContext.BaseDirectory ?? Environment.CurrentDirectory, "Content");
    public static string Config => Path.Combine(Local, "Config");
    public static string Skins => Path.Combine(Asset, "Skins");
    public static string Graphics => Path.Combine(Asset, "Graphics");
    public static string Fonts => Path.Combine("", "Fonts");
    public static string Gui => Path.Combine(Graphics, "Gui");
    public static string Gradients => Path.Combine(Gui, "Gradients");
    public static string Designs => Path.Combine(Gui, "Designs");
    public static string Tilesets => Path.Combine(Graphics, "Tilesets");
    public static string Characters => Path.Combine(Graphics, "Characters");
    public static string Emotes => Path.Combine(Graphics, "Emotes");
    public static string Paperdolls => Path.Combine(Graphics, "Paperdolls");
    public static string Fogs => Path.Combine(Graphics, "Fogs");
    public static string Parallax => Path.Combine(Graphics, "Parallax");
    public static string Panoramas => Path.Combine(Graphics, "Panoramas");
    public static string Pictures => Path.Combine(Graphics, "Pictures");
    public static string Logs => Path.Combine(Local, "Logs");
    public static string Database => Path.Combine(Local, "Database");
    public static string Music => Path.Combine(Asset, "Music");
    public static string Sounds => Path.Combine(Asset, "Sounds");
    public static string Items => Path.Combine(Graphics, "Items");
    public static string Maps => Path.Combine(Graphics, "Maps");
    public static string Animations => Path.Combine(Graphics, "Animations");
    public static string Skills => Path.Combine(Graphics, "Skills");
    public static string Projectiles => Path.Combine(Graphics, "Projectiles");
    public static string Resources => Path.Combine(Graphics, "Resources");
    public static string Misc => Path.Combine(Graphics, "Misc");

    public static string EnsureFileExtension(string path, string defaultExtension = ".png")
    {
        if (string.IsNullOrWhiteSpace(Path.GetExtension(path)))
        {
            return path + defaultExtension;
        }
        
        return path;
    }
}