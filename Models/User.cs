using discordBot.util;

namespace DiscordBot.Models;

public sealed class User : Entity
{
    public double Tigers { get; set; }
    public int Tab { get; set; }
    public double Trust { get; set; }

    public User(ulong id, double tigers = 100, int tab = 0, double trust = -1)
    {
        (Id, Tigers, Tab, Trust) = (id, tigers, tab, trust);
    }
   public static void UpdateDbTest()
   {
       var user = new User(123);

        try
        {
            const string str = "Trust";
            
            // Console.WriteLine(context.Users.Find(user) == null);
            // var u = context.Users.Update(user);
            // Console.WriteLine(context.Users.ToString());
            // Console.WriteLine("executed");
            Console.WriteLine(user.Id);
            // context.Users.Add(user);
            // context.SaveChanges();
            // Console.WriteLine($"There are now {context.Users.Count()}");

            // Console.WriteLine(mod.Pull(user));
            // mod.Pull<int>(123, "Chips");
            try
            {
                // DiscordBotDbModifier.Increment(123, str, 99999);
                // DiscordBotDbModifier.Set(123, str, "999");
                // Console.WriteLine($"{str}: {DiscordBotDbModifier.Pull<int>(123, str)}");
                DiscordBotDbModifier.Increment(6969, str, 3);
                Console.WriteLine($"{str}: {DiscordBotDbModifier.Pull<int>(123, str)}");
            } catch(Exception e) {Console.WriteLine(e.Message);}

            // DiscordBotDbModifier.Update(123, "Chips", "99999999999999");
            // var u = context.Users.Find(new User(124341351624998912));
            // Console.WriteLine(u.Chips);
    
        } catch (Exception e) { Console.WriteLine(e.InnerException?.Message);}

    }

   public override string TableEntityString() { return "Users(Id, Chips, Tab, Trust)"; }
   public override string ToEntityString() { return $"{Id}, {Tigers}, {Tab}, {Trust}"; }
   
   public override string Table() { return "Users"; }

}