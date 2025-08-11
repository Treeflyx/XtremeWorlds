using System.Diagnostics;
using System.Xml.Serialization;
using Core.Globals;

namespace Core.Configurations;

public static class LocalesManager
{
    public const string MissingKeyFormat = "[MISSING_LOC_KEY: {0}]";
    
    private static readonly Dictionary<string, string> LocalizedStrings = new(StringComparer.OrdinalIgnoreCase);
    private static LocaleData? _localeData;

    public static string DefaultLanguageCode => "en";
    public static string CurrentLanguageCode { get; private set; } = string.Empty;

    public static void Initialize(string? languageCode = null)
    {
        CurrentLanguageCode = languageCode ?? DefaultLanguageCode;
        
        var path = GetLocalePath(CurrentLanguageCode);
        
        if (!File.Exists(path))
        {
            Debug.WriteLine($"Localization file not found: {path}. Creating and using default English localization.");

            _localeData = LocaleData.CreateDefaultEnglish();

            Save(_localeData, path);
        }
        else
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(LocaleData));

                using var reader = new StreamReader(path);

                _localeData = xmlSerializer.Deserialize(reader) as LocaleData ?? LocaleData.CreateDefaultEnglish();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading localization file '{path}': {ex.Message}. Falling back to defaults.");

                _localeData = LocaleData.CreateDefaultEnglish();
            }
        }
        
        PopulateDictionary(_localeData);
    }

    private static string GetLocalePath(string languageCode)
    {
        return Path.Combine(DataPath.Config, $"Locales_{languageCode}.xml");
    }

    private static void PopulateDictionary(LocaleData localeData)
    {
        LocalizedStrings.Clear();
        
        foreach (var item in localeData.AllItems)
        {
            if (!LocalizedStrings.TryAdd(item.Key, item.Value))
            {
                Debug.WriteLine(
                    $"Warning: Duplicate localization key '{item.Key}' found. " +
                    $"Value '{LocalizedStrings[item.Key]}' will be used. " +
                    $"New value '{item.Value}' ignored.");
            }
        }
    }

    private static void Save(LocaleData data, string fileName)
    {
        try
        {
            var path = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            var xmlSerializer = new XmlSerializer(typeof(LocaleData));
            
            using var streamWriter = new StreamWriter(fileName);
            
            xmlSerializer.Serialize(streamWriter, data);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error saving localization file '{fileName}': {ex.Message}");
        }
    }

    public static string Get(string key)
    {
        if (_localeData is null)
        {
            return string.Format(MissingKeyFormat, key);
        }

        var value = LocalizedStrings.GetValueOrDefault(key);
        if (value is not null)
        {
            return value;
        }
        
        return string.Format(MissingKeyFormat, key);
    }
}