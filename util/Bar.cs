using DiscordBot;
using DSharpPlus.Entities;

namespace discordBot.util;



public class Bar
{
    public static readonly string Logo = Environment.GetEnvironmentVariable("BARLOGO") ??
                                            "https://melmagazine.com/wp-content/uploads/2022/04/Drunken_Monkey_Hypothesis-1024x427.jpg";

    public static string Column = "Tab";
    
    public static void AddDrinks(DiscordUser user, int amount)
    {
        if (!DiscordBotDbModifier.Exists(user.Id)) DiscordBotDbModifier.AddEntity(user.Id);
        DiscordBotDbModifier.Increment(user.Id, Column, amount);
    }

    public static int Tab(DiscordUser user)
    {
        if (!DiscordBotDbModifier.Exists(user.Id)) DiscordBotDbModifier.AddEntity(user.Id);
        return DiscordBotDbModifier.Pull<int>(user.Id, Column);
    }

    public static void TakeDrinks(DiscordUser user, int amount)
    {
        if (!DiscordBotDbModifier.Exists(user.Id)) DiscordBotDbModifier.AddEntity(user.Id);
        DiscordBotDbModifier.Decrement(user.Id, Column, amount);
    }
}