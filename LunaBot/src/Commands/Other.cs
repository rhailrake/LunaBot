using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace LunaBot.Commands;
public class Other : BaseCommandModule
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
                .GroupBy(c => c.Module.ModuleType.Name);
            
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
                .Where(c => !c.IsHidden && c.Module.ModuleType.Name.ToLowerInvariant() == category.ToLowerInvariant())
                .GroupBy(c => c.Module.ModuleType.Name)
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

                embed.AddField($"{command.Name}", command.Description, inline: false);
            }
            
            await ctx.RespondAsync(embed: embed.Build());
        }
        
    }
    
    [Command("ping")]
    [Description("Отправляет пинг к Discord API и выводит задержку в ответ.")]
    public async Task Ping(CommandContext ctx)
    {
        var response = await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Blurple)
            .WithTitle("Ping")
            .WithDescription("Пожалуйста, подождите...")
            .Build());

        var latency = ctx.Client.Ping;
        var editResponse = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Green)
            .WithTitle("Ping")
            .WithDescription($"Pong! Задержка: {latency} мс.")
            .Build();

        await response.ModifyAsync(embed: editResponse);
    }
    
    [Command("avatar")]
    [Description("Отображает аватар пользователя в красивом эмбеде.")]
    public async Task Avatar(CommandContext ctx, [Description("Участник, аватар которого нужно показать.")] DiscordMember member = null)
    {
        // Если не передан участник, берем аватар автора команды
        if (member == null)
        {
            member = ctx.Member;
        }

        // Создаем эмбед
        var embed = new DiscordEmbedBuilder
        {
            Title = "Аватар пользователя",
            Color = DiscordColor.Blurple,
            Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
            {
                Url = member.AvatarUrl
            },
            Description = $"Аватар для пользователя {member.Mention}"
        };

        // Отправляем сообщение с эмбедом
        await ctx.RespondAsync(embed: embed);
    }

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