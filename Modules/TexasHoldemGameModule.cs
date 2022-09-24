using DiscordBot.games;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace DiscordBot.Modules;

public sealed class TexasHoldemGameModule : BaseCommandModule
{

    [Command("texasholdem")]
    [Aliases("texas")]
    [Description("texas <users>")]
    public async Task TexasHoldem(CommandContext ctx, params DiscordMember[] addedUsers)
    {
        var users = new[] { ctx.Member }.Concat(addedUsers).ToArray();
        var poker = new TexasHoldem();
        var dealtCards = poker.Deal(users.Length);

        
        
        // ctx.Member?.SendMessageAsync("GO FUCK YOURSELF");

        // ReSharper disable once HeapView.ObjectAllocation.Possible
        for(var i = 0; i < users.Length; i++)
        {
            // for (int j = 0; j < 100; j++)
            // {
            var msg = new DiscordMessageBuilder();
            msg.Content += dealtCards[i].Item1.GetEmoji(ctx);
            msg.Content += dealtCards[i].Item2.GetEmoji(ctx);
            await users[i]!.SendMessageAsync( msg );    
            // }
            
            // Console.WriteLine(users[i]?.Username);
        }
        Console.WriteLine("done");
    }

}