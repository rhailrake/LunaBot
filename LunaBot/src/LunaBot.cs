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
        try
        {
            // Logs
            var logger = new Logger("logs/logs.json");
            logger.Log("[Logs] attached!");
            // Logs
        
            // Config
            logger.Log($"[info] Started loading LunaBot at {DateTime.Now}");
            logger.Log("[info] Loading config file..");
            var config = Config.LoadFromFile("secrets/cfg.json");
            logger.Log("[info] Loaded config file!");
            // Config

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
        
            logger.Log("[status] Setting up status..");
            Task.Run(() => new LunaStatus(Client).RunAsync());
            logger.Log("[status] Loaded!");
        
            await Client.ConnectAsync();

            await commandHandler.LogAboutAllCommands();
        
            logger.Log($"Finished loading at {DateTime.Now} | LunaBot has started..");

            await Task.Delay(-1);
        }
        catch (Exception e)
        {
            var logger = new Logger("logs/logs.json");
            logger.Log($"Something wrong with LunaBot: {e.Message}");
        }
    }
    
    private Task OnClientReady(ReadyEventArgs e) 
    {
        return Task.CompletedTask;
    }
}