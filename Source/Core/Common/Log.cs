using System.IO;

namespace Core;

public static class Log
{
    public static void Add(string message, string logFileName)
    {
        if (!Directory.Exists(Path.Logs))
        {
            Directory.CreateDirectory(Path.Logs);
        }

        var path = System.IO.Path.Combine(Path.Logs, logFileName);

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