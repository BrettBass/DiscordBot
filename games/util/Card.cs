using System.Runtime.CompilerServices;

namespace DiscordBot.games.util;

public class Card
{
    public enum Suites
    {
        Hearts = 0,
        Diamonds,
        Clubs,
        Spades
    }

    public int Value { get; set; }

    public Suites Suite { get; set; }
    
    //Used to get full name, also useful 
    //if you want to just get the named value
    public string NamedValue
    {
        get
        {
            string name = string.Empty;
            switch (Value)
            {
                case (1):
                    name = "Ace";
                    break;
                case (13):
                    name = "King";
                    break;
                case (12):
                    name = "Queen";
                    break;
                case (11):
                    name = "Jack";
                    break;
                default:
                    name = Value.ToString();
                    break;
            }

            return name;
        }
    }

    public string Name
    {
        get
        {
            return NamedValue + " of  " + Suite.ToString();
        }
    }
    
    public int GetSuite() { return (int) Suite; }

    public Card(int Value, Suites Suite)
    {
        this.Value = Value;
        this.Suite = Suite;
    }
}