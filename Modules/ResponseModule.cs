using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;

namespace DiscordBot.Modules;

public class ResponseModule : BaseCommandModule
{
    [Command("echo")]
    public async Task Echo(CommandContext ctx, params string[] msgs)
    {
        var interactivity = ctx.Client.GetInteractivity();
        var msg = string.Join(" ", msgs);
        if (msg == "")
        {
            var msgResult = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel)
                .ConfigureAwait(false);
            msg += msgResult.Result.Content;
            await msgResult.Result.DeleteAsync().ConfigureAwait(false);
        }

        await ctx.Channel.SendMessageAsync(msg).ConfigureAwait(false);
        await ctx.Message.DeleteAsync().ConfigureAwait(false);
    }
}