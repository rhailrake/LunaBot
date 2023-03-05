using DSharpPlus;
using Newtonsoft.Json;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace LunaBot.Helpers
{
    public class Logger
    {
        private readonly string _logFilePath;
        
        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
            Fatal,
            Commands
        }
        
        private LogLevel _level = LogLevel.Info;

        public LogLevel Level
        {
            get => _level;
            set => _level = value;
        }


        public Logger(string logFilePath)
        {
            var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFilePath);
            _logFilePath = absolutePath;
        }

        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Level = level,
                Message = message
            };

            var logJson = JsonConvert.SerializeObject(logEntry);

            try
            {
                var logs = new List<string>();

                // Читаем предыдущие записи
                if (File.Exists(_logFilePath))
                {
                    using (var reader = new StreamReader(_logFilePath))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            logs.Add(line);
                        }
                    }
                }

                // Добавляем новую запись
                logs.Add(logJson);

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
                        writer.WriteLine(log);
                    }
                }
                
                if (level == LogLevel.Fatal)
                {
                    Console.WriteLine("FATAL: Check logs.json");
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
            public LogLevel Level { get; set; }
            public DateTime Timestamp { get; set; }
            public string Message { get; set; }
        }
    }
}
