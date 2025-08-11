using System.Xml.Serialization;

namespace Core.Configurations;

public sealed class LocaleItem
{
    [XmlAttribute("Key")]
    public string Key { get; set; } = string.Empty;

    [XmlAttribute("Value")]
    public string Value { get; set; } = string.Empty;

    public LocaleItem()
    {
    }

    public LocaleItem(string key, string value)
    {
        Key = key;
        Value = value;
    }
}