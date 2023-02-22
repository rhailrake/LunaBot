using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using LunaBot.Helpers;
using Microsoft.Extensions.Logging;

namespace LunaBot;

public class LunaBot
{
    private DiscordClient? Client { get; set; }
    private CommandsNextExtension? Commands { get; set; }

    public async Task RunAsync() 
    {
        Console.WriteLine($"[info] Started loading LunaBot at {DateTime.Now}");
        Console.WriteLine("[info] Loading config file..");
        var config = Config.LoadFromFile("secrets/cfg.json");
        Console.WriteLine("[info] Loaded config file!");
        var discordConfig = new DiscordConfiguration()
        {
            Intents = DiscordIntents.All,
            Token = config.Token,
            TokenType = TokenType.Bot,
            AutoReconnect = true,
            MinimumLogLevel = LogLevel.Critical
        };

        Client = new DiscordClient(discordConfig);
        Client.UseInteractivity(new InteractivityConfiguration()
        {
            Timeout = TimeSpan.FromMinutes(2)
        });

        Commands = Client.UseCommandsNext(new CommandsNextConfiguration
        {
            StringPrefixes = new string[] { config.Prefix },
            EnableMentionPrefix = true,
            EnableDms = false,
            EnableDefaultHelp = false
        });
        
        var commandHandler = new CommandsHandler(Commands);
        await commandHandler.InstallCommandsAsync();
        
        Console.WriteLine("[status] Setting up status..");
        Task.Run(() => new LunaStatus(Client).RunAsync());
        Console.WriteLine("[status] Loaded!");
        
        await Client.ConnectAsync();

        await commandHandler.LogAboutAllCommands();
        
        Console.WriteLine($"Finished loading at {DateTime.Now} | LunaBot has started..");

        await Task.Delay(-1);
    }
    
    private Task OnClientReady(ReadyEventArgs e) 
    {
        return Task.CompletedTask;
    }
}