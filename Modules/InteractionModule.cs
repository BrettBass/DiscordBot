using Discord.Interactions;
using discordBot;
using discordBot.util;

namespace DiscordBot.Modules
{
    
    public class InteractionModule : InteractionModuleBase<SocketInteractionContext>{
        
        [SlashCommand("ping", "responds with pong")]
        public async Task HandlePingCommand()
        {
            await ReplyAsync("pong");
        }


        // [SlashCommand("ad", "adds Drink <user> <amount>")]
        // //[Alias("ad"), Summary("increase the amount of drinks"), Remarks("addDrink <user> <amount>")]
        // public async Task HandleAdCommand(UserCommandAttribute user, int amount)
        // {
        //     //given the username pull the id
        //     var userId = user;
        //
        //     Console.WriteLine(userId);
        //
        //     //if string starts with <@ then remove it and string ends with > then remove it
        //
        //     //check if user exists in guild
        //     if (Context.Guild.Users.FirstOrDefault(x => x.Id == ulong.Parse(userId)) == null)
        //     {
        //         await ReplyAsync("User not found");
        //         return;
        //     }
        //
        //     var cur = bank.GetCurrency("Drinks");
        //
        //     bank.Withdraw(userId, cur, amount);
        //     await ReplyAsync("Added " + amount + " drinks to " + user);
        // }
        
        // check if user wants to participate
    }
}