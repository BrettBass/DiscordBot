using Newtonsoft.Json;
public sealed class DrinkExchange : IBank{
    
    public string name { get; }
    public string file{ get; } = "drinkExchange.json";

    public List<ICurrency> currencies { get; } = new List<ICurrency>(){
        new Drink(),
        new Rubies()
    };

    public DrinkExchange(){
        
        if(!File.Exists(file))
            File.Create(file);
    }

    public double CheckBalance(string userId, ICurrency currency){

        var json = File.ReadAllText(file);
        
        var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(json);

        //check if data is null
        if(data == null){
            data = new Dictionary<string, Dictionary<string, double>>();
        }

        if(!checkAccount(userId, ref data))
            //checkAccount creates the account if it doesn't exist
            File.WriteAllText(file, JsonConvert.SerializeObject(data));
        
        return data[userId][currency.name];
    }

    public Dictionary<string, double> AllBalances(string userID){
        Dictionary<string, double> balances = new Dictionary<string, double>();
        foreach(var currency in currencies){
            var balance = CheckBalance(userID, currency);
            balances.Add(currency.name, balance);
        }
        return balances;
    }

    public double Withdraw(string userId, ICurrency currency, double amount){
        
        var json = File.ReadAllText(file);
        
        var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(json);

        checkAccount(userId, ref data);

        // pass data[account.ID][currency.name] as ref 
        double balance = data[userId][currency.name];
        double ret = currency.decrement(ref balance, amount) ? amount : -1.0;

        data[userId][currency.name] = balance;

        File.WriteAllText(file, JsonConvert.SerializeObject(data));
        return ret;
    }

    public double Deposit(string userId, ICurrency currency, double amount){

        var json = File.ReadAllText(file);

        var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(json);

        checkAccount(userId, ref data);
        double balance  = data[userId][currency.name];
        currency.increment(ref balance, amount);
        data[userId][currency.name] = balance;

        File.WriteAllText(file, JsonConvert.SerializeObject(data));

        return amount;
    }

    private bool checkAccount(string userId, ref Dictionary<string, Dictionary<string, double>> jsondata){
        //new account

        if(!jsondata.ContainsKey(userId)){
            var currDict = new Dictionary<string, double>();

            for (int i = 0; i < currencies.Count; i++){
                ICurrency currency = currencies[i];
                currDict[currency.name] = currency.startingBalance;
            }

            jsondata[userId] = currDict;
            return false;
        }
        //missing a currency
        else{
            foreach(ICurrency currency in currencies)
                if(!jsondata[userId].ContainsKey(currency.name))
                    jsondata[userId][currency.name] = currency.startingBalance;
            

            return false;
        }
        return true;
    }

    public bool User(string userId){
        var json = File.ReadAllText(file);
        var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(json);
        return data.ContainsKey(userId);
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

}