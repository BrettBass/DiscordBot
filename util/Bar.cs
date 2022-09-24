using DiscordBot;
using DSharpPlus.Entities;

namespace discordBot.util;



public static class Bar
{
    public static readonly string Logo = Environment.GetEnvironmentVariable("BARLOGO") ??
                                            "https://melmagazine.com/wp-content/uploads/2022/04/Drunken_Monkey_Hypothesis-1024x427.jpg";

    public const int ShotToBeerRatio = 8;
    private const string Table = "Tabs";
    
    public static void AddDrinks(DiscordUser user, int amount)
    {
        if (!DiscordBotDbModifier.Exists(Table, user.Id)) DiscordBotDbModifier.AddEntity(Table, user.Id);
        DiscordBotDbModifier.Increment(Table, user.Id, "Drinks", amount);
    }

    public static int Tab(DiscordUser user)
    {
        if (!DiscordBotDbModifier.Exists(Table, user.Id)) DiscordBotDbModifier.AddEntity(Table, user.Id);
        return DiscordBotDbModifier.Pull<int>(Table, user.Id, "Drinks");
    }

    public static void TakeDrinks(DiscordUser user, int amount)
    {
        if (!DiscordBotDbModifier.Exists(Table, user.Id)) DiscordBotDbModifier.AddEntity(Table, user.Id);
        DiscordBotDbModifier.Decrement(Table, user.Id, "Drinks", amount);
    }
}