using System.Globalization;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using discordBot.util;

namespace DiscordBot.Modules;

public class BarCommandModule : BaseCommandModule
{
    [Command("addDrink")]
    [Description("addDrink <user> <amount>")]
    [Aliases("ad")]
    public async Task AddDrink(CommandContext ctx, DiscordUser user, int amount)
    {
        Bar.AddDrinks(user, amount);
        await ctx.Channel.SendMessageAsync("Added " + amount + " drinks to " + user.Username).ConfigureAwait(false);
    }

    [Command("drink")]
    [Description("drink <amount>")]
    [Aliases("d")]
    public async Task Drink(CommandContext ctx, int amount = 1)
    {
        await ctx.Channel.SendMessageAsync($"{ctx.User.Username}\n{amount} drink/s removed from tab").ConfigureAwait(false);
        Bar.TakeDrinks(ctx.User, amount);
    }

    [Command("tab")]
    [Description("Tab <user?>")]
    [Aliases("t")]
    public async Task Tab(CommandContext ctx, DiscordUser? user = null)
    {
        if (user == null) user = ctx.User;
        var embed = new DiscordEmbedBuilder()
            .WithTitle("Tab")
            .WithThumbnail(Bar.Logo)
            .AddField("User", user.Username)
            .AddField("Drinks", Bar.Tab(user).ToString(CultureInfo.InvariantCulture));

        await ctx.Channel.SendMessageAsync(embed.Build()).ConfigureAwait(false);
    }
}