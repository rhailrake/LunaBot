using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using LunaBot.Commands;

namespace LunaBot.Helpers;

public class CommandsHandler
{
    private readonly CommandsNextExtension _commands;

    public CommandsHandler(CommandsNextExtension commands)
    {
        _commands = commands;

        _commands.CommandErrored += async (s, e) =>
        {
            if (e.Exception is CommandNotFoundException)
            {
                var embed = new DiscordEmbedBuilder
                {
                    Title = "ERR0R 404",
                    Description = "Команда не найдена, напишите !help для списка доступных команд.",
                    Color = new DiscordColor(0xFF0000)
                };

                await e.Context.RespondAsync(embed: embed);
            }
        };
    }

    public async Task InstallCommandsAsync()
    {
        try
        {
            // Register commands here:
            _commands.RegisterCommands<Logs>();
            _commands.RegisterCommands<Info>();
            _commands.RegisterCommands<Tools>();
            _commands.RegisterCommands<Images>();
            _commands.RegisterCommands<Fun>();

            await Task.CompletedTask;
        }
        catch (Exception e)
        {
            var logger = new Logger("logs/logs.json");
            logger.Log($"Something wrong in CommandHandler: {e.Message}");
        }
    }

    public async Task LogAboutAllCommands()
    {
        var logger = new Logger("logs/logs.json");
        var commandModules = _commands.RegisteredCommands.Values
            .GroupBy(c => c.Module!.ModuleType.Name);
        
        logger.Log("[commands] info start:");
        
        foreach (var module in commandModules)
        {
            logger.Log($"[{module.Key}] Registered {module.Count()} command(s).");
        }
        
        logger.Log("[commands] info end. ");
        
        await Task.CompletedTask;
    }
}