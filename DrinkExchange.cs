using System.Diagnostics;
using Newtonsoft.Json;

namespace discordBot;

public sealed class DrinkExchange : IBank{
    
    public string name { get; }
    private static string File { get; } = "/home/brett/projects/DiscordBot/DrinkExchange.json";
    public static Dictionary<ulong, Dictionary<string, double>>? UserData;

    public List<ICurrency> currencies { get; } = new List<ICurrency>(){
        new Drink(),
        new Rubies()
    };

    public DrinkExchange()
    {
        if(!System.IO.File.Exists(File)) System.IO.File.Create(File);
        UserData = JsonConvert.DeserializeObject<Dictionary<ulong, Dictionary<string, double>>>(System.IO.File.ReadAllText(File));
        Debug.Assert(UserData != null, nameof(UserData) + " != null");
    }

    public double Balance(ulong userId, ICurrency currency)
    {
        checkAccount(userId);
        return UserData[userId][currency.name];
    }

    public Dictionary<string, double> AllBalances(ulong userId)
    {
        Dictionary<string, double> balances = new Dictionary<string, double>();
        foreach(var currency in currencies)
        {
            var balance = Balance(userId, currency);
            balances.Add(currency.name, balance);
        }
        return balances;
    }

    public double Withdraw(ulong userId, ICurrency currency, double amount){
        checkAccount(userId);

        // pass data[account.ID][currency.name] as ref 
        double balance = UserData[userId][currency.name];
        double ret = currency.decrement(ref balance, amount) ? amount : -1.0;

        UserData[userId][currency.name] = balance;
        UpdateJson();

        return ret;
    }

    public double Deposit(ulong userId, ICurrency currency, double amount){
        checkAccount(userId);
        double balance  = UserData[userId][currency.name];
        currency.increment(ref balance, amount);
        UserData[userId][currency.name] = balance;
        UpdateJson();

        return amount;
    }

    private bool checkAccount(ulong userId){
        //new account

        if(!UserData.ContainsKey(userId)){
            var currDict = new Dictionary<string, double>();

            for (int i = 0; i < currencies.Count; i++){
                ICurrency currency = currencies[i];
                currDict[currency.name] = currency.startingBalance;
            }

            UserData[userId] = currDict;
            UpdateJson();
            return false;
        }
        //missing a currency
        else{
            foreach(ICurrency currency in currencies)
                if(!UserData[userId].ContainsKey(currency.name))
                    UserData[userId][currency.name] = currency.startingBalance;

            return false;
        }
    }
    
    public void addCurrency(ICurrency currency){
        currencies.Add(currency);
    }

    public ICurrency GetCurrency(string name){
        foreach(var currency in currencies){
            if(currency.name == name)
                return currency;
        }
        return null;
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