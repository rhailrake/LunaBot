using DSharpPlus;
using DSharpPlus.Entities;
using LunaBot.Helpers;

public class LunaStatus
{
    private readonly DiscordClient _client;
    private readonly DiscordActivity[] _activities = new DiscordActivity[]
    {
        new DiscordActivity("🌕", ActivityType.Watching),
        new DiscordActivity("🌖", ActivityType.Watching),
        new DiscordActivity("🌗", ActivityType.Watching),
        new DiscordActivity("🌘", ActivityType.Watching),
        new DiscordActivity("🌑", ActivityType.Watching),
        new DiscordActivity("🌒", ActivityType.Watching),
        new DiscordActivity("🌓", ActivityType.Watching),
        new DiscordActivity("🌔", ActivityType.Watching)
    };
    private int _activityIndex = 0;

    public LunaStatus(DiscordClient client)
    {
        _client = client;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            try
            {
                await _client.UpdateStatusAsync(_activities[_activityIndex]);
                _activityIndex = (_activityIndex + 1) % _activities.Length;
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                //var logger = new Logger("logs/logs.json");
                // logger.Log($"Error updating Luna status: {ex.Message}");
            }
        }
    }
}