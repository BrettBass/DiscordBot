using DiscordBot.Models;
using DSharpPlus.Entities;


namespace discordBot.util;

public sealed class Bank
{
    public static readonly string BankName = Environment.GetEnvironmentVariable("BANKNAME") ?? "Drink Exchange";
    
    public static readonly string BankLogo = Environment.GetEnvironmentVariable("BANKLOGO") ?? "https://i.ytimg.com/vi/Xn1lpsAN06I/maxresdefault.jpg";

    public static readonly string[] Currencies = {"Whores", "Guns", "Horses", "Tigers"};

    private const string Table = "BankAccount";

    public static bool Withdraw(DiscordUser user, int amount, String currency)
    {
        if (!DiscordBotDbModifier.Exists(Table, user.Id)) DiscordBotDbModifier.AddEntity(Table, user.Id);
        
        if (!Array.Exists(Currencies, x => String.Equals(x, currency, StringComparison.CurrentCultureIgnoreCase)) 
            || DiscordBotDbModifier.Pull<double>(Table, user.Id, currency) < amount) return false;
        
        DiscordBotDbModifier.Decrement(Table, user.Id, currency, amount);
        return true;
    }

    public static int Deposit(DiscordUser user, int amount, string currency)
    {
        if (!DiscordBotDbModifier.Exists(Table, user.Id)) DiscordBotDbModifier.AddEntity(Table, user.Id);

        if (!Array.Exists(Currencies,
                x => String.Equals(x, currency, StringComparison.CurrentCultureIgnoreCase))) return 0;
        
        DiscordBotDbModifier.Increment(Table, user.Id, currency, amount);

        return amount;
    }

    public static IEnumerable<(string, string?)> BankingInfo(DiscordUser user)
    {
        if (!DiscordBotDbModifier.Exists(Table, user.Id)) DiscordBotDbModifier.AddEntity(Table, user.Id);

        // ReSharper disable once HeapView.ObjectAllocation.Evident
        foreach (var currency in Currencies)
            yield return (currency, DiscordBotDbModifier.Pull<string>(Table, user.Id, currency));
    }
}