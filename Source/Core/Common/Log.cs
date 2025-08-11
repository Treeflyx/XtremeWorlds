using Core.Globals;

namespace Core.Common;

public static class Log
{
    public static void Add(string message, string logFileName)
    {
        if (!Directory.Exists(DataPath.Logs))
        {
            Directory.CreateDirectory(DataPath.Logs);
        }

        var path = Path.Combine(DataPath.Logs, logFileName);

        try
        {
            using var stream = File.Open(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            using var streamWriter = new StreamWriter(stream);
            
            streamWriter.WriteLine(message);
        }
        catch
        {
            // ignored
        }
    }
}