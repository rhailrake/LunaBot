namespace LunaBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bot = new LunaBot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}