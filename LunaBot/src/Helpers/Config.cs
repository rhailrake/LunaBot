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
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<Config>(json);
    }
}