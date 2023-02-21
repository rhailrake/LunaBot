using Newtonsoft.Json;

namespace LunaBot;

internal struct Config
{
    [JsonProperty("token")]
    public string Token { get; private set; }
        
    [JsonProperty("prefix")]
    public string Prefix { get; private set; }
}