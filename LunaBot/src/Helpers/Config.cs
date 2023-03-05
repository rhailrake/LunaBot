using Newtonsoft.Json;

namespace LunaBot.Helpers;

public class Config
{
    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("prefix")]
    public string Prefix { get; set; }

    public static Config LoadFromFile(string path)
    {
        try
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Config>(json);
        }
        catch (Exception e)
        {
            var logger = new Logger("logs/logs.json");
            logger.Log($"Something wrong in Config helper: {e.Message}");
        }
        throw new InvalidOperationException();
    }
}