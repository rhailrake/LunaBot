using Newtonsoft.Json;

namespace LunaBot.Helpers;

public class Logger
{
    private readonly string _logFilePath;

    public Logger(string logFilePath)
    {
        var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFilePath);
        _logFilePath = absolutePath;
    }

    public void Log(string message)
    {
        var logEntry = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            Message = message
        };

        var logJson = JsonConvert.SerializeObject(logEntry);

        try
        {
            using (var writer = new StreamWriter(_logFilePath, true))
            {
                writer.WriteLine(logJson);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing log entry to file: {ex.Message}");
        }
    }

    public string GetLatestLogs(int maxEntries = 10)
    {
        var logs = new List<string>();

        try
        {
            using (var reader = new StreamReader(_logFilePath))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    logs.Add(line);

                    if (logs.Count >= maxEntries)
                    {
                        logs.RemoveAt(0);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading log file: {ex.Message}");
        }

        var logJson = JsonConvert.SerializeObject(logs, Formatting.Indented);

        return logJson;
    }

    private class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
    }
}