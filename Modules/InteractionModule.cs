using Discord;
using Discord.Interactions;
namespace DiscordBot.Modules
{
    
    public class InteractionModule : InteractionModuleBase<SocketInteractionContext>{

        DrinkExchange bank = new DrinkExchange();

        [SlashCommand("ping", "responds with pong")]
        public async Task HandlePingCommand()
        {
            await ReplyAsync("pong");
        }


        [SlashCommand("ad", "adds Drink <user> <amount>")]
        //[Alias("ad"), Summary("increase the amount of drinks"), Remarks("addDrink <user> <amount>")]
        public async Task HandleAdCommand(string user, int amount)
        {
            //given the username pull the id
            var userId = user;

            Console.WriteLine(userId);

            //if string starts with <@ then remove it and string ends with > then remove it
            if (userId.StartsWith("<@!") && userId.EndsWith(">"))
            {
                userId = userId.Substring(3, userId.Length - 4);
            }
            //check if user exists in guild
            if (Context.Guild.Users.FirstOrDefault(x => x.Id == ulong.Parse(userId)) == null)
            {
                await ReplyAsync("User not found");
                return;
            }

            var cur = bank.GetCurrency("Drinks");

            bank.Withdraw(userId, cur, amount);
            await ReplyAsync("Added " + amount + " drinks to " + user);
        }
    }
}