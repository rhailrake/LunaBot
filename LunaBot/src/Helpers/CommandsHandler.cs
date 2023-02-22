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
                    Title = "ERROR 404",
                    Description = "Команда не найдена, напишите !help для списка доступных команд.",
                    Color = new DiscordColor(0xFF0000)
                };

                await e.Context.RespondAsync(embed: embed);
            }
        };
    }

    public async Task InstallCommandsAsync()
    {
        // Register commands here:
        _commands.RegisterCommands<Info>();
        _commands.RegisterCommands<Tools>();
        _commands.RegisterCommands<Images>();
        _commands.RegisterCommands<Fun>();

        await Task.CompletedTask;
    }

    public async Task LogAboutAllCommands()
    {
        var commandModules = _commands.RegisteredCommands.Values
            .GroupBy(c => c.Module.ModuleType.Name);
        
        Console.WriteLine("[commands] info start:");
        
        foreach (var module in commandModules)
        {
            Console.WriteLine($"[{module.Key}] Registered {module.Count()} command(s).");
        }
        
        Console.WriteLine("[commands] info end. ");
        
        await Task.CompletedTask;
    }
}