namespace DiscordBot.Models;

public sealed class Tab : Entity
{
    public int Drinks { get; set; }
    public int Shots { get; set; }

    public Tab(ulong id, int drinks = 0, int shots = 0)
    {
        (Id, Drinks, Shots) = (id, drinks, shots);
    }
    
   public override string TableEntityString() { return "Tab(Id, Drinks, Shots)"; }
   public override string ToEntityString() { return $"{Id}, {Drinks}, {Shots}"; }
   
   public override string Table() { return "Tab"; }
   
}