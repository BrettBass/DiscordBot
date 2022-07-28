using System.Globalization;
using System.Text.RegularExpressions;
using discordBot;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DiscordBot.games;
using discordBot.util;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace DiscordBot.Modules
{
    public class PrefixModule : BaseCommandModule
    {
        DrinkExchange bank = new DrinkExchange();

        [Command("ping")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("go fuck yourself");
        }

        [Command("tyler")]
        public async Task Tyler(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Tyler loves vagina").ConfigureAwait(false);
        }
        
        //two commands for same action, balance and bal
        [Command("accountstatement")]
        [Aliases("as")]
        public async Task AccountStatement(CommandContext ctx)
        {
            //get the id of user who sent the command
            var user = ctx.User;
            var userdata = bank.GetUserData(user);

            var embed = new DiscordEmbedBuilder()
                .WithTitle("Account Statement")
                .WithColor(new DiscordColor(0, 255, 0))
                .WithThumbnail("https://melmagazine.com/wp-content/uploads/2022/04/Drunken_Monkey_Hypothesis-1024x427.jpg")
                .AddField("User", user.Username)
                .AddField("Chips", userdata._chipBalance.ToString(CultureInfo.InvariantCulture), true)
                .AddField("Drinks owed", userdata._barTab.ToString(CultureInfo.InvariantCulture), true)
                .AddField("Trustworthiness", userdata._trustworthy.ToString(CultureInfo.CurrentCulture), true);
        
            await ctx.Channel.SendMessageAsync(embed: embed.Build());
        }

        [Command("deposit")]
        public async Task Deposit(CommandContext ctx, double amount)
        {
            bank.Deposit(ctx.User, amount);
            await ctx.Channel.SendMessageAsync("Deposited " + amount).ConfigureAwait(false);
        }
        
        //case insensitive command withdraw
        [Command("withdraw")]
        
        public async Task Withdraw(CommandContext ctx, double amount)
        {
            if (bank.Withdraw(ctx.User, amount))
                await ctx.Channel.SendMessageAsync("Withdrew " + amount ).ConfigureAwait(false);
            else 
                await ctx.Channel.SendMessageAsync("Invalid amount").ConfigureAwait(false);
        }
        
        [Command("addDrink"), Description("addDrink <user> <amount>"), Aliases("ad")]
        public async Task AddDrink(CommandContext ctx, DiscordUser user, int amount)
        {
            bank.AddDrink(user, amount);
            await ctx.Channel.SendMessageAsync("Added " + amount + " drinks to " + user.Username).ConfigureAwait(false);
        }
        
        [Command("coinflip"), Description("coinflip <heads/tails>"), Aliases(("cf"))]
        // if no numFlips then default to 1
        public async Task CoinFlip(CommandContext ctx, int bet = 1)
        {
            var interactivity = ctx.Client.GetInteractivity();
            await ctx.Channel.SendMessageAsync(ctx.User.Username + " heads or tails: ");
            var response =  await interactivity.WaitForMessageAsync(x => x.Author == ctx.User).ConfigureAwait(false);
            var guess = response.Result.Content;
            
            guess = guess.ToLower();
            if (!Regex.IsMatch(response.Result.Content, "(h+e+a+d+|t+a+i+l+)s*", RegexOptions.IgnoreCase)) {
                
                await ctx.Channel.SendMessageAsync("invalid guess").ConfigureAwait(false);
                return;
            }

            var result = games.BasicDrinking.CoinFlip() ? "heads" : "tails";
            var msg = result + "\n";
            if (result != guess.ToLower()) {
                msg += "drink " + bet + " bitch";
                AddDrink(ctx, ctx.User, bet);
            } else {
                msg += "clean";
                bank.Deposit(ctx.User, bet * 10);
            }
            msg += result + result == guess.ToLower() ? "drink " + bet + " bitch" : "";
            
            await ctx.Channel.SendMessageAsync(msg).ConfigureAwait(false);  
        }

        [Command("coinflip")]
        public async Task CoinFlip(CommandContext ctx, int bet = 1, params DiscordUser[] users)
        {
            users.Append(ctx.User);
            (var result, var resultPattern) = games.BasicDrinking.CoinFlip() ? ("heads", "(h+e+a+d+)s*") : ("tails","(t+a+i+l+)s*");

            var losers = "";
            var interactivity = ctx.Client.GetInteractivity();

            foreach (var user in users)
            {
                InteractivityResult<DiscordMessage> guess;
                do
                {
                    await ctx.Channel.SendMessageAsync(user.Username + " heads or tails: ");
                    guess = await interactivity.WaitForMessageAsync(x => x.Author == user).ConfigureAwait(false);
                } while (!Regex.IsMatch(guess.Result.Content, "(h+e+a+d+|t+a+i+l+)s*", RegexOptions.IgnoreCase));
                
                if (!Regex.IsMatch(guess.Result.Content, resultPattern, RegexOptions.IgnoreCase))
                {
                    losers += user.Username + " ";
                }
            }

            if (losers == "")
            {
                await ctx.Channel.SendMessageAsync("no losers");
                return;
            }

            var embed = new DiscordEmbedBuilder()
                .WithTitle("Balance")
                .WithColor(new DiscordColor(0, 255, 0))
                .AddField("Results", result)
                .AddField("Losers", losers)
                .AddField("Drink Amount", bet.ToString());
            await ctx.Channel.SendMessageAsync(embed: embed.Build());
        }
        
    }
}
