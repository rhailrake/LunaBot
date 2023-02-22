using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace LunaBot.Commands;
public class Fun : BaseCommandModule
{
    [Command("coinflip")]
    [Description("Подбросить монетку.")]
    public async Task Coinflip(CommandContext ctx)
    {
        var rnd = new Random();
        string result = rnd.Next(2) == 0 ? "`орел`" : "`решка`";
        var embed = new DiscordEmbedBuilder
        {
            Title = "Coinflip:",
            Color = DiscordColor.Blurple,
            Description = $"Результат: {result}"
        };
        await ctx.RespondAsync(embed: embed);
    }
    
    [Command("roll")]
    [Description("Бросает кости с заданным количеством сторон.")]
    public async Task Roll(CommandContext ctx, [Description("Число сторон.")] int sides = 6)
    {
        var rnd = new Random();
        var roll = rnd.Next(1, sides + 1);
        var embed = new DiscordEmbedBuilder
        {
            Title = "Roll:",
            Color = DiscordColor.Blurple,
            Description = $"Результат: **{roll}** (1-{sides})."
        };
        await ctx.RespondAsync(embed: embed);
    }
}