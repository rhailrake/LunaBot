using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using LunaBot.Commands;
using Newtonsoft.Json;

namespace LunaBot;

public class LunaBot
{
    private DiscordClient? Client { get; set; }
    public InteractivityExtension Interactivity { get; private set; }
    public CommandsNextExtension? Commands { get; set; }

    public async Task RunAsync() 
    {
        string json;
        await using (var fs = File.OpenRead("secrets/cfg.json"))
        using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
            json = await sr.ReadToEndAsync();

        var configJson = JsonConvert.DeserializeObject<Config>(json);

        var config = new DiscordConfiguration()
        {
            Intents = DiscordIntents.All,
            Token = configJson.Token,
            TokenType = TokenType.Bot,
            AutoReconnect = true,
        };

        Client = new DiscordClient(config);
        Client.UseInteractivity(new InteractivityConfiguration()
        {
            Timeout = TimeSpan.FromMinutes(2)
        });

        var commandsConfig = new CommandsNextConfiguration()
        {
            StringPrefixes = new string[] { configJson.Prefix },
            EnableMentionPrefix = true,
            EnableDms = false,
            EnableDefaultHelp = false
        };
        
        // Commands register
        Commands = Client.UseCommandsNext(commandsConfig);
        Commands.RegisterCommands<Other>();
        
        Commands.CommandErrored += async (s, e) =>
        {
            if (e.Exception is CommandNotFoundException)
            {
                // создаем красивый эмбед, сообщающий, что команда не найдена
                var embed = new DiscordEmbedBuilder
                {
                    Title = "ERROR 404",
                    Description = "Команда не найдена, напишите !help для доступных команд.",
                    Color = new DiscordColor(0xFF0000)
                };

                // отправляем эмбед в канал, откуда пришла команда
                await e.Context.RespondAsync(embed: embed);
            }
        };

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }
    
    private Task OnClientReady(ReadyEventArgs e) 
    {
        return Task.CompletedTask;
    }
}