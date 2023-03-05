using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using LunaBot.Helpers;

namespace LunaBot.Commands;

    public class Logs : BaseCommandModule
    {
        private readonly Logger _logger;

        public Logs()
        {
            _logger = new Logger("logs/logs.json");
        }

        [Command("logs")]
        [Description("Отображает последние логи")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task ShowLogs(CommandContext ctx)
        {
            var logs = _logger.GetLatestLogs();
            var logEntries = logs.Select(log => $"{log.Timestamp}: [{log.Level}] {log.Message}");

            var embed = new DiscordEmbedBuilder
            {
                Title = "Последние логи:",
                Description = string.Join("\n", logEntries),
                Color = new DiscordColor(0xFF0000) // красный цвет
            };

            await ctx.RespondAsync(embed: embed.Build());

        }

        [Command("clearlogs")]
        [Description("Полностью очищает логи")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task ClearLogs(CommandContext ctx)
        {
            try
            {
                var LogFilePath = "logs/logs.json";
                File.Delete(LogFilePath);
                var successEmbed = new DiscordEmbedBuilder
                {
                    Title = "Успешно",
                    Description = "Логи были полностью очищены",
                    Color = new DiscordColor(0x00FF00) // зеленый цвет
                };
                await ctx.RespondAsync(embed: successEmbed.Build());
                _logger.Log($"Succsesfully cleared logs!", Logger.LogLevel.Debug);
            }
            catch (Exception ex)
            {
                var errorEmbed = new DiscordEmbedBuilder
                {
                    Title = "Ошибка",
                    Description = $"Не удалось очистить логи: {ex.Message}",
                    Color = new DiscordColor(0xFF0000) // красный цвет
                };
                await ctx.RespondAsync(embed: errorEmbed.Build());
                _logger.Log($"Something when executing clear logs command: {ex.Message}", Logger.LogLevel.Fatal);
            }
        }
    }