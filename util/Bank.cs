using DiscordBot.Models;
using DSharpPlus.Entities;


namespace discordBot.util;

public sealed class Bank
{
    public static readonly string BankName = Environment.GetEnvironmentVariable("BANKNAME") ?? "Drink Exchange";
    
    public static readonly string BankLogo = Environment.GetEnvironmentVariable("BANKLOGO") ?? "https://i.ytimg.com/vi/Xn1lpsAN06I/maxresdefault.jpg";

    public static readonly string Currency = "Tigers";

    public static bool Withdraw(DiscordUser user, int amount)
    {
        if (!DiscordBotDbModifier.Exists(user.Id)) DiscordBotDbModifier.AddEntity(user.Id);
        
        if (DiscordBotDbModifier.Pull<double>(user.Id, Currency) < amount) return false;
        
        DiscordBotDbModifier.Decrement(user.Id, Currency, amount);
        return true;
    }

    public static int Deposit(DiscordUser user, int amount)
    {
        if (!DiscordBotDbModifier.Exists(user.Id)) DiscordBotDbModifier.AddEntity(user.Id);
        
        DiscordBotDbModifier.Increment(user.Id, Currency, amount);

        return amount;
    }

    public static User GetUserData(DiscordUser user)
    {
        if (!DiscordBotDbModifier.Exists(user.Id)) DiscordBotDbModifier.AddEntity(user.Id);
        var bal  = DiscordBotDbModifier.Pull<int>(user.Id, Currency);
        var tab = DiscordBotDbModifier.Pull<int>(user.Id, "Tab");
        var trust = DiscordBotDbModifier.Pull<int>(user.Id, "Trust");
        
        return new User(user.Id, bal, tab, trust);
    }
}