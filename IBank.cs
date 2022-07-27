public interface IBank{

    string name { get; }
    static string file { get; }

    List<ICurrency> currencies { get; }
    
    double Balance(ulong userId, ICurrency currency);

    Dictionary<string, double> AllBalances(ulong userID);

    double Withdraw(ulong userId, ICurrency currency, double amount);

    double Deposit(ulong userId, ICurrency currency, double amount);

    ICurrency GetCurrency(string name);

}