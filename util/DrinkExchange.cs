using System.Diagnostics;
using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace discordBot.util;

public sealed class DrinkExchange {
    
    public string name { get; }
    private static string File { get; } = "/home/brett/projects/DiscordBot/storage/DrinkExchange.json";
    public static Dictionary<ulong, UserData>? UserData;

    public DrinkExchange()
    {
        if(!System.IO.File.Exists(File)) System.IO.File.Create(File);
        UserData = JsonConvert.DeserializeObject<Dictionary<ulong, UserData>>(System.IO.File.ReadAllText(File));
        Debug.Assert(UserData != null, nameof(util.UserData) + " != null");
    }

    public bool Withdraw(DiscordUser user, double amount)
    {
        Debug.Assert(UserData != null, nameof(UserData) + " != null");
        if (UserData[user.Id]._chipBalance < amount) return false;
        
        UserData[user.Id].UpdateBalance(-amount);
        UpdateJson();

        return true;
    }

    public double Deposit(DiscordUser user, double amount)
    {
        if(!checkAccount(user)) AddAccount(user);
        UserData[user.Id].UpdateBalance(amount);
        UpdateJson();

        return amount;
    }

    private bool checkAccount(DiscordUser user){

        return(UserData.ContainsKey(user.Id));
    }

    public void AddAccount(DiscordUser user)
    {
        Debug.Assert(!checkAccount(user), user.Username + " already found in json file\n ID:" + user.Id);
        UserData.Add(user.Id, new UserData());
        
    }

    public UserData GetUserData(DiscordUser user)
    {
        return UserData[user.Id];
    }

    public void AddDrink(DiscordUser user, int amount)
    {
        if(!checkAccount(user)) AddAccount(user);
        UserData[user.Id].UpdateTab(amount);
        UpdateJson();
    }

    private void UpdateJson()
    {
        System.IO.File.WriteAllText(File, JsonConvert.SerializeObject(UserData));
    }

    ~DrinkExchange()
    {
        UpdateJson();
    }
}