public interface IBank{

    string name { get; }
    string file { get; }

    List<ICurrency> currencies { get; }
    
    double CheckBalance(string userId, ICurrency currency);

    Dictionary<string, double> AllBalances(string userID);

    double Withdraw(string userId, ICurrency currency, double amount);

    double Deposit(string userId, ICurrency currency, double amount);

    ICurrency GetCurrency(string name);

}