using DiscordBot.Context;
using DiscordBot.Models;
using DSharpPlus.Entities;


namespace discordBot.util;

public sealed class Bank
{
    public static readonly string BankName = Environment.GetEnvironmentVariable("BANKNAME") ?? "Drink Exchange";
    
    public static readonly string BankLogo = Environment.GetEnvironmentVariable("BANKLOGO") ?? "https://i.ytimg.com/vi/Xn1lpsAN06I/maxresdefault.jpg";

    public static readonly string[] Currencies = {"Whores", "Guns", "Horses", "Tigers"};

    private const string Table = "BankAccounts";

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

    public static BankAccount BankingInfo(DiscordUser user)
    {
        // if (!DiscordBotDbModifier.Exists(Table, user.Id)) DiscordBotDbModifier.AddEntity(Table, user.Id);

        using SqliteContext lite = new SqliteContext();
        var acc = lite.BankAccounts.FirstOrDefault(account => account!.Id == user.Id);
        if (acc != null) return acc;
        
        acc = new BankAccount(user.Id);
        lite.BankAccounts.Add(acc);
        lite.SaveChanges();
        return acc;
    }

    private static void AddAccount(ref BankAccount acc)
    {
        
    }
}