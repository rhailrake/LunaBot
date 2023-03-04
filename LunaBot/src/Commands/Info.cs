using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace LunaBot.Commands;

public class Info : BaseCommandModule
{
    [Command("help")]
    [Description("Показывает список доступных команд или список команд в определенной категории.")]
    public async Task Help(CommandContext ctx, [Description("Название категории команд.")] string category = "")
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            var embed = new DiscordEmbedBuilder()
                .WithTitle("Категории команд")
                .WithDescription("Ниже перечислены доступные категории команд. " +
                    "Чтобы узнать, какие команды есть в определенной категории, " +
                    "наберите команду **!help [название категории]**.")
                .WithColor(new DiscordColor("#2ECC71"))
                .WithFooter("Luna Bot");
            
            var modules = ctx.CommandsNext.RegisteredCommands.Values
                .Where(c => !c.IsHidden)
                .GroupBy(c => c.Module!.ModuleType.Name);
            
            foreach (var module in modules)
            {
                var description = string.Join("\n", module
                    .Select(c => $"`{c.Name}` - {c.Description}"));

                embed.AddField(module.Key, description, inline: false);
            }
            
            await ctx.RespondAsync(embed: embed.Build());
        }
        else
        {
            var module = ctx.CommandsNext.RegisteredCommands.Values
                .Where(c => !c.IsHidden && c.Module!.ModuleType.Name.ToLowerInvariant() == category.ToLowerInvariant())
                .GroupBy(c => c.Module!.ModuleType.Name)
                .FirstOrDefault();
            
            if (module == null)
            {
                await ctx.RespondAsync($"Категория `{category}` не найдена.");
                return;
            }
            
            var embed = new DiscordEmbedBuilder()
                .WithTitle($"Команды в категории \"{module.Key}\"")
                .WithColor(new DiscordColor("#2ECC71"))
                .WithFooter("Luna Bot");
            foreach (var command in module)
            {

                embed.AddField($"{command.Name}", command.Description!, inline: false);
            }
            
            await ctx.RespondAsync(embed: embed.Build());
        }
        
    }
}