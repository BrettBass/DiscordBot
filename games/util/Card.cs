using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace DiscordBot.games.util;

public class Card
{
    public enum Suites
    {
        // Hearts = 0,
        // Diamonds,
        // Clubs,
        // Spades
        H = 0,
        D,
        C,
        S
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
                case (-1):
                    name = "Blank";
                    break;
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

    public string Name =>
        NamedValue + " of  " + Suite;

        public string Emoji =>
        // NamedValue + " of  " + Suite.ToString();
        ":" + NamedValue + Suite + ":";
    
    public DiscordEmoji GetEmoji(CommandContext ctx) { return DiscordEmoji.FromName(ctx.Client, Emoji);}
    

    public int GetSuite() { return (int) Suite; }

    public Card(int Value, Suites Suite)
    {
        this.Value = Value;
        this.Suite = Suite;
    }

    public Card()
    {
        this.Value = -1;
        this.Suite = Suites.H;
    }
}