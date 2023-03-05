using Newtonsoft.Json;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace LunaBot.Helpers
{
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
                Timestamp = DateTime.Now,
                Message = message
            };

            try
            {
                var logs = new List<LogEntry>();

                // Читаем предыдущие записи
                if (File.Exists(_logFilePath))
                {
                    using (var reader = new StreamReader(_logFilePath))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            var log = JsonConvert.DeserializeObject<LogEntry>(line);
                            logs.Add(log);
                        }
                    }
                }

                // Добавляем новую запись
                logs.Add(logEntry);

                // Ограничиваем количество записей
                if (logs.Count > 100)
                {
                    logs.RemoveRange(0, logs.Count - 100);
                }

                // Записываем все записи в файл
                using (var writer = new StreamWriter(_logFilePath, false))
                {
                    foreach (var log in logs)
                    {
                        var logJsonString = JsonConvert.SerializeObject(log);
                        writer.WriteLine(logJsonString);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing log entry to file: {ex.Message}");
            }
        }

        public List<LogEntry> GetLatestLogs(int maxEntries = 20)
        {
            var logs = new List<LogEntry>();

            try
            {
                if (File.Exists(_logFilePath))
                {
                    using (var reader = new StreamReader(_logFilePath))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            var log = JsonConvert.DeserializeObject<LogEntry>(line);
                            logs.Add(log);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading log file: {ex.Message}");
            }

            // Ограничиваем количество записей
            if (logs.Count > maxEntries)
            {
                logs.RemoveRange(0, logs.Count - maxEntries);
            }

            return logs;
        }

        public class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Message { get; set; }
        }
    }

    public class Logs : BaseCommandModule
    {
        private readonly Logger _logger;

        public Logs()
        {
            _logger = new Logger("logs/logs.json");
        }

        [Command("logs")]
        [Description("Отображает последние логи")]
        public async Task ShowLogs(CommandContext ctx)
        {
            var logs = _logger.GetLatestLogs();
            
            var embed = new DiscordEmbedBuilder
            {
                Title = "Последние логи:",
                Description = string.Join("\n", logs.Select(log => $"{log.Timestamp}: {log.Message}")),
                Color = new DiscordColor(0xFF0000) // красный цвет
            };

            await ctx.RespondAsync(embed: embed.Build());

        }
    }
}
