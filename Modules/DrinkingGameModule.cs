using System.Text.RegularExpressions;
using Discord;
using discordBot;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DiscordBot.games;
using DSharpPlus.Interactivity;
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
    public async Task SmokeOrFire(CommandContext ctx, params DiscordUser[] users)
    {
        users.Append(ctx.User);
        SmokeOrFire sof = new SmokeOrFire();
        var interactivity = ctx.Client.GetInteractivity();
        var pass = 0;
        while (true)
        {
            foreach (var user in users)
            {
                var contMultiplier = 2;
                while (pass < 4)
                {
                    int options;
                    if (sof.GetCardsInPlay() == 0)
                    {
                        await ctx.RespondAsync("Smoke or Fire").ConfigureAwait(false);
                        options = (int)Options.Minimum;
                    }
                    else
                    {
                        await ctx.RespondAsync("Smoke or Fire, Higher or Lower").ConfigureAwait(false);
                        options = (int)Options.Full;
                    }

                    var guess = await interactivity.WaitForMessageAsync(x => x.Author == user).ConfigureAwait(false);
                    if (guess.Result.Content.ToLower() == "q")
                    {
                        await ctx.RespondAsync("Game Over").ConfigureAwait(false);
                        return;
                    }
                    
                    int choice = CheckGuess(guess.Result.Content, options);
                    if (choice < 0)
                    {
                        await ctx.RespondAsync("invalid guess").ConfigureAwait(false);
                        continue;
                    }

                    bool result = choice < 2 ? sof.Color(choice) : sof.Value(choice);
                    var embed = new DiscordEmbedBuilder()
                        .WithTitle(result ? "Winner" : "Loser")
                        .WithColor(new DiscordColor(0, 255, 0));
                    sof.CardsToEmbed(ref embed);
                    
                    await ctx.Channel.SendMessageAsync(embed: embed.Build()).ConfigureAwait(false);
                    if (result)
                    {
                        pass++;
                    }
                    else
                    {
                        var drinksOwed = sof.GetCardsInPlay();
                        DrinkExchange.AddDrink(user, drinksOwed);
                        await ctx.RespondAsync(drinksOwed + " Drinks Added to Tab").ConfigureAwait(false);
                        pass = 0;
                        sof.ClearInPlayCards();
                    }

                    if (pass >= 4)
                    {
                        await ctx.RespondAsync("Would you like to (C)ontinue?").ConfigureAwait(false);
                        var answer = await interactivity.WaitForMessageAsync(x => x.Author == user).ConfigureAwait(false);
                        if (answer.Result.Content.ToLower() == "c")
                        {
                            await ctx.RespondAsync("Continued").ConfigureAwait(false);
                            contMultiplier <<= 1;
                            pass = 3;
                        }
                        else
                        {
                            var chipsRewarded = sof.GetCardsInPlay() * contMultiplier;
                            DrinkExchange.Deposit(user, chipsRewarded);
                            await ctx.RespondAsync("rewarded: " + chipsRewarded + " Chips").ConfigureAwait(false);
                        }
                    }
                    
                }

                pass = 1;
            }
            await ctx.RespondAsync("If anyone wants to (Q)uit:").ConfigureAwait(false);
            var quit = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);
            if (quit.Result.Content.ToLower() == "q")
            {
                await ctx.RespondAsync("Game Over").ConfigureAwait(false);
                return;
            }
        }
    }

    private int CheckGuess(string guess, int options)
    {
        string[] regex = { "(s)|(s+m+o+k+e+)", "(f)|(f+i+r+e+)", "(h)|(h+i+g+h+e+r+)", "(l)|(l+o+w+e+r+)" };
        
        for (int i = 0; i < options; i++)
        {
            if (Regex.IsMatch(guess, regex[i], RegexOptions.IgnoreCase))
                return i;
        }

        return -1;
    }
}