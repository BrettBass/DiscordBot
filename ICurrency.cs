
public interface ICurrency{
    string name { get; }

    string ToString();

    double startingBalance { get; }

    bool decrement(ref double balance, double amount);

    bool increment(ref double balance, double amount);
}