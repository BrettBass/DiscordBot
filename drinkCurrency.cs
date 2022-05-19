public class Drink : ICurrency{
    public string name { get; } = "Drinks";

    public string ToString(){ return name; }
    public double startingBalance { get; } = 0;
    public bool decrement(ref double balance, double amount){
        balance += amount;
        return true;
    }

    public bool increment(ref double balance, double amount){
        if(balance < amount)
            return false;

        balance -= amount;
        return true;
    }
}