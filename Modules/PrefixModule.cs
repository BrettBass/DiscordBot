using System.Globalization;
using System.Text.RegularExpressions;
using discordBot;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DiscordBot.games;
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
        [Command("balance")]
        [Aliases("bal")]
        public async Task Balance(CommandContext ctx)
        {
            //get the id of user who sent the command
            var user = ctx.User;
        
            // convert id to string
            var userId = user.Id;
        
            var embed = new DiscordEmbedBuilder()
                .WithTitle("Balance")
                .WithColor(new DiscordColor(0, 255, 0))
                .AddField("User", user.Username);
            Dictionary<string, double> balances = bank.AllBalances(userId);
            foreach (KeyValuePair<string, double> entry in balances)
                embed.AddField(entry.Key, entry.Value.ToString(CultureInfo.InvariantCulture), true);
        
            await ctx.Channel.SendMessageAsync(embed: embed.Build());

        }

        [Command("deposit")]
        public async Task Deposit(CommandContext ctx, string currency, double amount)
        {
            //string currency, double amount
            var user = ctx.User;
            var userId = user.Id;
        
            ICurrency cur = bank.GetCurrency(currency);
            if (cur == null)
            {
                await ctx.Channel.SendMessageAsync("Invalid Currency").ConfigureAwait(false);
                return;
            }
            bank.Deposit(userId, cur, amount);
            await ctx.Channel.SendMessageAsync("Deposited " + amount + " " + currency).ConfigureAwait(false);
        }
        
        //case insensitive command withdraw
        [Command("withdraw")]
        
        public async Task Withdraw(CommandContext ctx, String currency, double amount)
        {
            var user = ctx.User;
            var userId = user.Id;
        
            ICurrency cur = bank.GetCurrency(currency);
            if (cur == null)
            {
                await ctx.Channel.SendMessageAsync("Invalid Currency").ConfigureAwait(false);
                return;
            }
        
            bank.Withdraw(userId, cur, amount);
            await ctx.Channel.SendMessageAsync("Withdrew " + amount + " " + currency).ConfigureAwait(false);
        }
        
        [Command("addDrink"), Description("addDrink <user> <amount>"), Aliases("ad")]
        public async Task AddDrink(CommandContext ctx, DiscordUser user, int amount)
        {
            //given the username pull the id

            Console.WriteLine(user.Username);
        
            //check if user exists in guild
            if (ctx.Guild.GetMemberAsync(user.Id) == null)
            {
                await ctx.Channel.SendMessageAsync("User not found.").ConfigureAwait(false);
                return;
            }

            var cur = bank.GetCurrency("Drinks");
        
            bank.Withdraw(user.Id, cur, amount);
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
                Withdraw(ctx,"Drinks", bet);
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
