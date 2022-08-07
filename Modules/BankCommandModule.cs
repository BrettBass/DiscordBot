using System.Globalization;
using discordBot.util;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace DiscordBot.Modules;

public class BankCommandModule : BaseCommandModule
{
            
        //two commands for same action, balance and bal
        [Command("accountstatement")]
        [Aliases("as")]
        public async Task AccountStatement(CommandContext ctx)
        {
            //get the id of user who sent the command
            var user = ctx.User;
            var userdata = DrinkExchange.GetUserData(user);

            var embed = new DiscordEmbedBuilder()
                .WithTitle(DrinkExchange.Name)
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
            DrinkExchange.Deposit(ctx.User, amount);
            await ctx.Channel.SendMessageAsync("Deposited " + amount).ConfigureAwait(false);
        }
        
        //case insensitive command withdraw
        [Command("withdraw")]
        
        public async Task Withdraw(CommandContext ctx, double amount)
        {
            if (DrinkExchange.Withdraw(ctx.User, amount))
                await ctx.Channel.SendMessageAsync("Withdrew " + amount ).ConfigureAwait(false);
            else 
                await ctx.Channel.SendMessageAsync("Invalid amount").ConfigureAwait(false);
        }
        
        [Command("addDrink"), Description("addDrink <user> <amount>"), Aliases("ad")]
        public async Task AddDrink(CommandContext ctx, DiscordUser user, int amount)
        {
            DrinkExchange.AddDrink(user, amount);
            await ctx.Channel.SendMessageAsync("Added " + amount + " drinks to " + user.Username).ConfigureAwait(false);
        }

        [Command("drink"), Description("drink <amount>"), Aliases("d")]
        public async Task Drink(CommandContext ctx, int amount = 1)
        {
            DrinkExchange.TakeDrink(ctx.User, amount);
            
        }

        [Command("tab"), Description("Tab <user?>"), Aliases("t")]
        public async Task Tab(CommandContext ctx, DiscordUser? user = null)
        {
            if (user == null) user = ctx.User;
            var embed = new DiscordEmbedBuilder()
            .WithTitle("Tab")
            .WithThumbnail(DrinkExchange.Logo)
            .AddField("User", user.Username)
            .AddField("Drinks",DrinkExchange.Tab(user).ToString(CultureInfo.InvariantCulture));

            await ctx.Channel.SendMessageAsync(embed.Build()).ConfigureAwait(false);
        }
}