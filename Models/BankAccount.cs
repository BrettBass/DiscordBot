using discordBot.util;

namespace DiscordBot.Models;

public sealed class BankAccount : Entity
{
    public double Whores { get; set; }
    public int Guns { get; set; }
    public int Horses { get; set; }
    public int Tigers { get; set; }
    

    public BankAccount(ulong id, double whores = 5, int guns = 0, int horses = 0, int tigers = 0)
    {
        (Id, Whores, Guns, Horses, Tigers) = (id, whores, guns, horses, tigers);
    }
   public static void UpdateDbTest()
   {
       var user = new BankAccount(123);

        try
        {
            const string str = "Whores";
            
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
                DiscordBotDbModifier.Increment("BankAccount", 6969, str, 3);
                Console.WriteLine($"{str}: {DiscordBotDbModifier.Pull<int>("BankAccount",123, str)}");
            } catch(Exception e) {Console.WriteLine(e.Message);}

            // DiscordBotDbModifier.Update(123, "Chips", "99999999999999");
            // var u = context.Users.Find(new User(124341351624998912));
            // Console.WriteLine(u.Chips);
    
        } catch (Exception e) { Console.WriteLine(e.InnerException?.Message);}

    }

   public override string TableEntityString() { return "Users(Id, Whores, Guns, Horses, Tigers)"; }
   public override string ToEntityString() { return $"{Id}, {Whores}, {Guns}, {Horses},{Tigers}"; }
   
   public override string Table() { return "BankAccount"; }

}