using System.Text.RegularExpressions;
using DiscordBot.Context;
using DiscordBot.games;
using DiscordBot.Models;
using discordBot.util;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Modules;

public class PrefixModule : BaseCommandModule
{
    [Command("dbadd")]
    public async Task DbAdd(CommandContext ctx)
    {
        await ctx.Channel.SendMessageAsync("adding");
        User.UpdateDbTest();
    }

    [Command("ping")]
    public async Task Ping(CommandContext ctx)
    {
        await ctx.Channel.SendMessageAsync("go fuck yourself");
    }

    [Command("cake")]
    public async Task Cake(CommandContext ctx)
    {
        using (var fs = new FileStream("./storage/images/cake.mp4", FileMode.Open, FileAccess.Read))
        {
            var msg = await new DiscordMessageBuilder()
                .WithContent("Here is a really dumb file that I am testing with.")
                .WithFiles(new Dictionary<string, Stream> { { "ADumbFile1.txt", fs } })
                .SendAsync(ctx.Channel);
        }
        //
        // var msg = new DiscordMessageBuilder().WithFile(new FileStream("./storage/images/cake.mp4"));
        //     "https://images-ext-1.discordapp.net/external/P2ZgfWiikaj0o8EzSoYYtBtC5KYk1SjzxRO3Uiy3xNg/https/media.tenor.com/gZsSfgthvnEAAAPo/gay-butt.mp4";
        //
        //
        //
        // await ctx.Channel.SendMessageAsync(attachment);
    }

    [Command("tyler")]
    public async Task Tyler(CommandContext ctx)
    {
        await ctx.Channel.SendMessageAsync("Tyler loves vagina").ConfigureAwait(false);
    }
    
    [Command("josh")]
    public async Task Josh(CommandContext ctx)
    {
        await ctx.Channel.SendMessageAsync("josh loves brett ass").ConfigureAwait(false);
    }

    [Command("coinflip")]
    [Description("coinflip <heads/tails>")]
    [Aliases("cf")]
    // if no numFlips then default to 1
    public async Task CoinFlip(CommandContext ctx, int bet = 1)
    {
        var interactivity = ctx.Client.GetInteractivity();
        await ctx.Channel.SendMessageAsync(ctx.User.Username + " heads or tails: ");
        var response = await interactivity.WaitForMessageAsync(x => x.Author == ctx.User).ConfigureAwait(false);
        var guess = response.Result.Content;

        guess = guess.ToLower();
        if (!Regex.IsMatch(response.Result.Content, "(h+e+a+d+|t+a+i+l+)s*", RegexOptions.IgnoreCase))
        {
            await ctx.Channel.SendMessageAsync("invalid guess").ConfigureAwait(false);
            return;
        }

        var result = BasicDrinking.CoinFlip() ? "heads" : "tails";
        var msg = result + "\n";
        if (result != guess.ToLower())
        {
            msg += "drink " + bet + " bitch";
            Bar.AddDrinks(ctx.User, bet);
        }
        else
        {
            msg += "clean";
            Bank.Deposit(ctx.User, bet * 10);
        }

        msg += result + result == guess.ToLower() ? "drink " + bet + " bitch" : "";

        await ctx.Channel.SendMessageAsync(msg).ConfigureAwait(false);
    }
    //
    // [Command("coinflip"), Aliases("cf")]
    // [Description("coinflip <bet amount> <players>")]
    // public async Task CoinFlip(CommandContext ctx, int bet = 1, params DiscordUser[] addedUsers)
    // {
    //     var users = new[] {ctx.User}.Concat(addedUsers);
    //     (var result, var resultPattern) = games.BasicDrinking.CoinFlip() ? ("heads", "(h+e+a+d+)s*") : ("tails","(t+a+i+l+)s*");
    //
    //     var losers = "";
    //     var interactivity = ctx.Client.GetInteractivity();
    //
    //     foreach (var user in users)
    //     {
    //         InteractivityResult<DiscordMessage> guess;
    //         do
    //         {
    //             await ctx.Channel.SendMessageAsync(user.Username + " heads or tails: ");
    //             guess = await interactivity.WaitForMessageAsync(x => x.Author == user).ConfigureAwait(false);
    //         } while (!Regex.IsMatch(guess.Result.Content, "(h+e+a+d+|t+a+i+l+)s*", RegexOptions.IgnoreCase));
    //         
    //         if (!Regex.IsMatch(guess.Result.Content, resultPattern, RegexOptions.IgnoreCase))
    //         {
    //             losers += user.Username + " ";
    //         }
    //     }
    //
    //     if (losers == "")
    //     {
    //         await ctx.Channel.SendMessageAsync("no losers");
    //         return;
    //     }
    //
    //     var embed = new DiscordEmbedBuilder()
    //         .WithTitle("Balance")
    //         .WithColor(new DiscordColor(0, 255, 0))
    //         .AddField("Results", result)
    //         .AddField("Losers", losers)
    //         .AddField("Drink Amount", bet.ToString());
    //     await ctx.Channel.SendMessageAsync(embed: embed.Build());
    // }
    
    
}