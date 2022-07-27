namespace DiscordBot
{
    public class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
            Console.WriteLine("Bot Ready!");
        }
    }
}
