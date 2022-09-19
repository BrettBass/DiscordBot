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

        var embed = new DiscordEmbedBuilder()
            .WithTitle(Bank.BankName)
            .WithColor(new DiscordColor(0, 255, 0))
            .WithThumbnail(Bank.BankLogo)
            .AddField("User", user.Username);
        foreach (var (name, value) in Bank.BankingInfo(user))
            embed.AddField(name, value);
        

        await ctx.Channel.SendMessageAsync(embed.Build());
    }

    [Command("deposit")]
    public async Task Deposit(CommandContext ctx, int amount, string currency)
    {
        await ctx.Channel.SendMessageAsync("Deposited " + Bank.Deposit(ctx.User, amount, currency)).ConfigureAwait(false);
    }

    //case insensitive command withdraw
    [Command("withdraw")]
    public async Task Withdraw(CommandContext ctx, int amount, string currency)
    {
        if (Bank.Withdraw(ctx.User, amount, currency))
            await ctx.Channel.SendMessageAsync("Withdrew " + amount).ConfigureAwait(false);
        else
            await ctx.Channel.SendMessageAsync("Invalid amount").ConfigureAwait(false);
    }
    
}