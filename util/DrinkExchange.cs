using System.Diagnostics;
using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace discordBot.util;

public sealed class DrinkExchange {
    
    public static string Name = "Drink Exchange";
    private static string File = "/home/brett/projects/DiscordBot/storage/DrinkExchange.json";
    public static Dictionary<ulong, UserData>? UserData = JsonConvert.DeserializeObject<Dictionary<ulong, UserData>>(System.IO.File.ReadAllText(File));


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

    private static void UpdateJson()
    {
        System.IO.File.WriteAllText(File, JsonConvert.SerializeObject(UserData));
    }

    ~DrinkExchange()
    {
        UpdateJson();
    }
}