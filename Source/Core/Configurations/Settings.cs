using System.Text.Json;

namespace Core.Configurations;

public class SettingsManager
{
    private const string FileName = "Settings.json";
    
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true
    };
    
    public static SettingsManager Instance { get; } = Load();
    
    public string Language { get; set; } = "English";
    public string Username { get; set; } = "";
    public bool SaveUsername { get; set; } = true;
    public string MenuMusic { get; set; } = "menu.mid";
    public bool Music { get; set; } = true;
    public bool Sound { get; set; } = true;
    public float MusicVolume { get; set; } = 100.0f;
    public float SoundVolume { get; set; } = 100.0f;
    public string MusicExt { get; set; } = ".mid";
    public string SoundExt { get; set; } = ".ogg";
    public byte Resolution { get; set; } = 1;
    public double Scale { get; set; } = 1.0;
    public bool Vsync { get; set; } = true;
    public bool Fullscreen { get; set; }
    public byte CameraWidth { get; set; } = 32;
    public byte CameraHeight { get; set; } = 24;
    public bool OpenAdminPanelOnLogin { get; set; } = true;
    public byte[] ChannelState { get; set; } = [1, 1, 1, 1, 1, 1, 1];
    public string Ip { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 7001;
    public string GameName { get; set; } = "XtremeWorlds";
    public string Website { get; set; } = "https://xtremeworlds.com/";
    public string Welcome { get; set; } = "Welcome to XtremeWorlds, enjoy your stay!";
    public double TimeSpeed { get; set; }
    public bool Autotile { get; set; } = true;
    public int MaxBackups { get; set; } = 5;
    public int ServerShutdown { get; set; } = 60;
    public int SaveInterval { get; set; } = 5;
    public int MaxSqlClients { get; set; } = 10;
    public string Skin { get; set; } = "Crystalshire";
    
    private static SettingsManager Load()
    {
        try
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XtremeWorlds");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            path = Path.Combine(path, FileName);
            if (!File.Exists(path))
            {
                return CreateDefaults();
            }

            var settingsJson = File.ReadAllText(path);
            var settings = JsonSerializer.Deserialize<SettingsManager>(settingsJson);

            return settings ?? new SettingsManager();
        }
        catch
        {
            return CreateDefaults();
        }
    }
    
    private static void Save(SettingsManager settings)
    {
        try
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XtremeWorlds");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, FileName);

            var settingsJson = JsonSerializer.Serialize(settings, JsonSerializerOptions);

            File.WriteAllText(path, settingsJson);
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to save settings");
            Console.WriteLine(e);
        }
    }

    public static void Save() => Save(Instance);
    
    private static SettingsManager CreateDefaults()
    {
        var settings = new SettingsManager();

        Save(settings);
        
        return settings;
    }
}