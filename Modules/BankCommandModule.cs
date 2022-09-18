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
        var userdata = Bank.GetUserData(user);

        var embed = new DiscordEmbedBuilder()
            .WithTitle(Bank.BankName)
            .WithColor(new DiscordColor(0, 255, 0))
            .WithThumbnail(Bank.BankLogo)
            .AddField("User", user.Username)
            .AddField(Bank.Currency, userdata.Tigers.ToString(CultureInfo.InvariantCulture), true)
            .AddField("Drinks", userdata.Tab.ToString(CultureInfo.InvariantCulture), true)
            .AddField("Trustworthiness", userdata.Trust.ToString(CultureInfo.CurrentCulture), true);

        await ctx.Channel.SendMessageAsync(embed.Build());
    }

    [Command("deposit")]
    public async Task Deposit(CommandContext ctx, int amount)
    {
        Bank.Deposit(ctx.User, amount);
        await ctx.Channel.SendMessageAsync("Deposited " + amount).ConfigureAwait(false);
    }

    //case insensitive command withdraw
    [Command("withdraw")]
    public async Task Withdraw(CommandContext ctx, int amount)
    {
        if (Bank.Withdraw(ctx.User, amount))
            await ctx.Channel.SendMessageAsync("Withdrew " + amount).ConfigureAwait(false);
        else
            await ctx.Channel.SendMessageAsync("Invalid amount").ConfigureAwait(false);
    }
    
}