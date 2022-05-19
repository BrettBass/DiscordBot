public class Rubies : ICurrency{
    public string name { get; } = "Rubies";
    public string ToString(){ return name; }

    public double startingBalance { get; } = 5;
    public bool decrement(ref double balance, double amount){
        if(balance < amount)
            return false;

        balance -= amount;
        return true;
    }

    public bool increment(ref double balance, double amount){
        balance += amount;
        return true;
    }
}