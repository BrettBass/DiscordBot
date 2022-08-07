using System.Diagnostics;
using DiscordBot;
using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace discordBot.util;

public sealed class DrinkExchange {
    
    public static readonly string Name = "Drink Exchange";
    public static Dictionary<ulong, UserData>? UserData = JsonConvert.DeserializeObject<Dictionary<ulong, UserData>>(File.ReadAllText(Global.JsonDatabase));
    public static readonly string Logo = "https://i.ytimg.com/vi/Xn1lpsAN06I/maxresdefault.jpg";

    public static bool Withdraw(DiscordUser user, double amount)
    {
        Debug.Assert(UserData != null, nameof(UserData) + " != null");
        if (UserData[user.Id]._chipBalance < amount) return false;
        
        UserData[user.Id].UpdateBalance(-amount);
        UpdateJson();

        return true;
    }

    public static double Deposit(DiscordUser user, double amount)
    {
        if(!checkAccount(user)) AddAccount(user);
        UserData[user.Id].UpdateBalance(amount);
        UpdateJson();

        return amount;
    }

    private static bool checkAccount(DiscordUser user){

        return(UserData.ContainsKey(user.Id));
    }

    public static void AddAccount(DiscordUser user)
    {
        Debug.Assert(!checkAccount(user), user.Username + " already found in json file\n ID:" + user.Id);
        UserData.Add(user.Id, new UserData());
        
    }

    public static UserData GetUserData(DiscordUser user)
    {
        return UserData[user.Id];
    }

    public static void AddDrink(DiscordUser user, int amount)
    {
        if(!checkAccount(user)) AddAccount(user);
        UserData[user.Id].UpdateTab(amount);
        UpdateJson();
    }

    public static double Tab(DiscordUser user)
    {
        if(!checkAccount(user)) AddAccount(user);
        return UserData[user.Id]._barTab;
    }

    public static void TakeDrink(DiscordUser user, double amount)
    {
        if(!checkAccount(user)) AddAccount(user);
        UserData[user.Id].UpdateTab(-amount);
    }

    private static void UpdateJson()
    {
        File.WriteAllText(Global.JsonDatabase, JsonConvert.SerializeObject(UserData));
    }

    ~DrinkExchange()
    {
        UpdateJson();
    }
}