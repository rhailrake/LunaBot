using System.Text.RegularExpressions;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json.Linq;

namespace LunaBot.Commands;

public class Tools : BaseCommandModule
{
    [Command("wiki")]
    [Description("Ищет статью в википедии.")]
    private async Task SearchWikipedia(CommandContext ctx, string query)
    {
        // API endpoint для поиска статей в Википедии
        string apiEndpoint =
            $"https://en.wikipedia.org/w/api.php?action=query&format=json&list=search&srsearch={query}";

        // Создаем HTTP клиент и запрос
        HttpClient client = new HttpClient();
        HttpResponseMessage response;

        try
        {
            // Отправляем GET запрос на API endpoint
            response = await client.GetAsync(apiEndpoint);
        }
        catch (Exception ex)
        {
            await ctx.RespondAsync($"Произошла ошибка при запросе статей: {ex.Message}");
            return;
        }

        // Если запрос успешен, выводим первые 5 статей
        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(responseBody);
            JArray searchResults = (JArray)json["query"]["search"];

            if (searchResults.Count > 0)
            {
                // Создаем новый эмбед и добавляем информацию о статьях
                var embed = new DiscordEmbedBuilder()
                    .WithTitle($"Результаты по запросу: {query}")
                    .WithColor(new DiscordColor("#ff6600"));

                for (int i = 0; i < 5 && i < searchResults.Count; i++)
                {
                    JObject result = (JObject)searchResults[i];
                    string title = result["title"].ToString();
                    string snippet = result["snippet"].ToString();

                    // Удаляем теги HTML из сниппета
                    snippet = Regex.Replace(snippet, "<.*?>", "");

                    // Ограничиваем длину сниппета до 250 символов
                    if (snippet.Length > 250)
                    {
                        snippet = snippet.Substring(0, 250) + "...";
                    }

                    // Добавляем информацию о статье в эмбед
                    embed.AddField($"{i + 1}. {title}", snippet);
                }

                await ctx.RespondAsync(embed: embed);
            }
            else
            {
                await ctx.RespondAsync($"По запросу \"{query}\" ничего не найдено.");
            }
        }
        else
        {
            await ctx.RespondAsync($"Произошла ошибка при запросе статей: {response.StatusCode}");
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
}