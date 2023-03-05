using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace LunaBot.Commands;

public class Control : BaseCommandModule
{
    [Command("shutdown")]
    [RequirePermissions(Permissions.Administrator)]
    [Description("Отключает бота и удаляет логи, если добавить true.")]
    public async Task Shutdown(CommandContext ctx, bool deleteLogs = false)
    {
        var embed = new DiscordEmbedBuilder()
            .WithTitle("Отключение бота")
            .WithColor(new DiscordColor("#FF0000"));
        
        if (deleteLogs)
        {
            File.Delete("logs/logs.json");
        
            embed.AddField("Удаление логов", "Логи успешно удалены.");
        }
        
        await ctx.Client.DisconnectAsync();
    
        embed.AddField("Lunabot", "Бот успешно отключен.");
    
        await ctx.RespondAsync(embed: embed);
        
        Environment.Exit(0);
    }

}