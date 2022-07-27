using System.Text.RegularExpressions;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DiscordBot.games;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace DiscordBot.Modules;

public class DrinkingGameModule : BaseCommandModule
{
    private enum SofGuess
    {
        Smoke = 0,
        Fire,
        Higher,
        Lower
    }
    // "(s)|(s+m+o+k+e+)"
    // "(f)|(f+i+r+e+)"
    // "(h)|(h+i+g+h+e+r+)"
    // "(l)|(l+o+w+e+r+)"
    
    
    [Command("smokeorfire"), Aliases("sof")]
    public async Task SmokeOrFire(CommandContext ctx, params DiscordUser[] users)
    {
        SmokeOrFire sof = new SmokeOrFire();
        var interactivity = ctx.Client.GetInteractivity();
        
        while (true)
        {
            foreach (var user in users)
            {
                var pass = false;
                while (!pass)
                {
                    await ctx.RespondAsync("Q to quit/nSmoke or Fire").ConfigureAwait(false);
                    var guess = await interactivity.WaitForMessageAsync(x => x.Author == user).ConfigureAwait(false);
                    if (guess.Result.Content.ToLower() == "q") return;
                    if (!Regex.IsMatch(guess.Result.Content, "(s)|(s+m+o+k+e+)|(f)|(f+i+r+e+)"))
                    {
                        await ctx.RespondAsync("invalid guess").ConfigureAwait(false);
                        continue;
                    }

                    if (Sof(ctx, user, sof).Result)
                    {
                        guess = InPlayHand(ctx, user, sof).Result;
                    }
                    
                } 
            }
        }
    }

    private async Task<InteractivityResult<DiscordMessage>> InPlayHand(CommandContext ctx, DiscordUser user, SmokeOrFire sof)
    {
        return new InteractivityResult<DiscordMessage>();
    }    
    private async Task<bool> Sof(CommandContext ctx, DiscordUser user, SmokeOrFire sof)
    {
        return false;
    }
    private async Task<bool> Hol(CommandContext ctx, DiscordUser user, SmokeOrFire sof)
    {
        return false;
    }

    private int CheckGuess(string guess)
    {
        string[] regex = { "(s)|(s+m+o+k+e+)", "(f)|(f+i+r+e+)", "(h)|(h+i+g+h+e+r+)", "(l)|(l+o+w+e+r+)" };

        for (int i = 0; i < regex.Length; i++)
        {
            if (Regex.IsMatch(guess, regex[i], RegexOptions.IgnoreCase))
                return i;
        }

        return -1;
    }
}