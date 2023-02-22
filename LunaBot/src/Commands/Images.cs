using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;


namespace LunaBot.Commands;

public class Images : BaseCommandModule
{
    [Command("avatar")]
    [Description("Отображает аватар пользователя.")]
    public async Task Avatar(CommandContext ctx, [Description("Участник, аватар которого нужно показать.")] DiscordMember? member = null)
    {
        if (member == null)
        {
            member = ctx.Member;
        }
        
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
        
        await ctx.RespondAsync(embed: embed);
    }
}