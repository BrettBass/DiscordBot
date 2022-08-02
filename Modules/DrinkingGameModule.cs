using System.Globalization;
using System.Text.RegularExpressions;
using discordBot;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DiscordBot.games;
using DSharpPlus.Interactivity.Extensions;

namespace DiscordBot.Modules;

public class DrinkingGameModule : BaseCommandModule
{
    private enum Options
    {
        Minimum = 2,
        Full = 4,
    }


    [Command("smokeorfire"), Aliases("sof")]
    public async Task SmokeOrFire(CommandContext ctx, params DiscordUser[] addedUsers)
    {
        var users = new[] {ctx.User}.Concat(addedUsers);
        var interactivity = ctx.Client.GetInteractivity();
        SmokeOrFire sof = new SmokeOrFire();
        var pass = 0;
        while (true)
        {
            foreach (var user in users)
            {
                var contMultiplier = 2;
                while (pass < 4)
                {
                    Options options = sof.GetCardsInPlay() == 0 ? Options.Minimum : Options.Full;
                    var guess = PromptGuess(ctx, user, options).Result;

                    if (guess.ToLower() == "q")
                    {
                        await ctx.RespondAsync("Game Over").ConfigureAwait(false);
                        return;
                    }
                    
                    int choice = CheckGuess(guess, (int)options);
                    if (choice < 0)
                    {
                        await ctx.RespondAsync("invalid guess").ConfigureAwait(false);
                        continue;
                    }

                    bool result = choice < 2 ? sof.Color(choice) : sof.Value(choice);

                    var msg = new DiscordMessageBuilder();
                    sof.CardsToEmojis(ctx, ref msg);
                    await ctx.Channel.SendMessageAsync(msg);

                    pass++;
                    
                    if (!result)
                    {
                        var drinksOwed = sof.GetCardsInPlay();
                        DrinkExchange.AddDrink(user, drinksOwed);
                        var embed = new DiscordEmbedBuilder()
                            .WithTitle("Loser")
                            .WithColor(new DiscordColor(0, 255, 0))
                            .WithThumbnail("https://pm1.narvii.com/6450/2f24804e66bd3d4449a2619f2d422ade180ce78c_hq.jpg")
                            .AddField("User", user.Username)
                            .AddField("Tab", DrinkExchange.Tab(user).ToString(CultureInfo.InvariantCulture));
                        
                        await ctx.Channel.SendMessageAsync(embed: embed.Build()).ConfigureAwait(false);
                        
                        pass = 0;
                        sof.ClearInPlayCards();
                    }

                    if (pass >= 4)
                    {
                        if (Pass(ctx, user).Result)
                        {
                            var chipsRewarded = sof.GetCardsInPlay() * contMultiplier;
                            DrinkExchange.Deposit(user, chipsRewarded);
                            
                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("Passed")
                                .WithColor(new DiscordColor(0, 255, 0))
                                .WithThumbnail("https://i.pinimg.com/736x/60/4f/0b/604f0b8b93f5c46bfbe5939e39411b13.jpg")
                                .AddField("User", user.Username)
                                .AddField("Chips Rewarded", chipsRewarded.ToString());
                            
                            await ctx.Channel.SendMessageAsync(embed: embed.Build()).ConfigureAwait(false);
                            
                            continue;
                        }
                        
                        await ctx.RespondAsync("Continued").ConfigureAwait(false);
                        contMultiplier <<= 1;
                        pass = 3;
                    }
                    
                }

                pass = 1;
            }
            await ctx.RespondAsync("If anyone wants to (Q)uit:").ConfigureAwait(false);
            var quit = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && users.Contains(x.Author)).ConfigureAwait(false);
            if (quit.Result.Content.ToLower() == "q")
            {
                await ctx.RespondAsync("Game Over").ConfigureAwait(false);
                return;
            }
        }
    }

    private async Task<string> PromptGuess(CommandContext ctx, DiscordUser user, Options options)
    {
        var interactivity = ctx.Client.GetInteractivity();
        var msg = user.Username + ": Smoke or Fire" + (options == Options.Full ? ", Higher or Lower" : "");

        await ctx.RespondAsync(msg).ConfigureAwait(false);

        var guess = await interactivity.WaitForMessageAsync(x => x.Author == user).ConfigureAwait(false);

        return guess.Result.Content;
    }

    private async Task<bool> Pass(CommandContext ctx, DiscordUser user)
    {
        var interactivity = ctx.Client.GetInteractivity();
        await ctx.RespondAsync("Would you like to (P)ass or (C)ontinue?").ConfigureAwait(false);
        var answer = await interactivity.WaitForMessageAsync(x => x.Author == user).ConfigureAwait(false);
        return Regex.IsMatch(answer.Result.Content, "^((p+)|(p+a+s+s+))$", RegexOptions.IgnoreCase);
    }
    
    

    private int CheckGuess(string guess, int options)
    {
        string[] regex = { "^((s+)|(s+m+o+k+e+))$", "^((f+)|(f+i+r+e+))$", "^((h+)|(h+i+g+h+e+r+))$", "^((l+)|(l+o+w+e+r+))$" };
        
        for (int i = 0; i < options; i++)
        {
            if (Regex.IsMatch(guess, regex[i], RegexOptions.IgnoreCase))
                return i;
        }

        return -1;
    }
}