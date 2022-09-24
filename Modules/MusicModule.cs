using DSharpPlus.CommandsNext;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.VoiceNext;

namespace DiscordBot.Modules;

public class MusicModule : BaseCommandModule
{
    
    
        [Command("join"), Description("Joins a voice channel.")]
        public async Task Join(CommandContext ctx, DiscordChannel channel)
        {
            Console.WriteLine("join");
            var lava = ctx.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                await ctx.RespondAsync("The Lavalink connection is not established");
                return;
            }

            var node = lava.ConnectedNodes.Values.First();

            if (channel.Type != ChannelType.Voice)
            {
                await ctx.RespondAsync("Not a valid voice channel.");
                return;
            }

            await node.ConnectAsync(channel);
            await ctx.RespondAsync($"Joined {channel.Name}!");
        }
        //     [Command("join"), Description("Joins a voice channel.")]
        // public async Task Join(CommandContext ctx, DiscordChannel chn = null)
        // {
        //     // check whether VNext is enabled
        //     var vnext = ctx.Client.GetVoiceNext();
        //     if (vnext == null)
        //     {
        //         // not enabled
        //         await ctx.RespondAsync("VNext is not enabled or configured.");
        //         return;
        //     }
        //
        //     // check whether we aren't already connected
        //     var vnc = vnext.GetConnection(ctx.Guild);
        //     if (vnc != null)
        //     {
        //         // already connected
        //         await ctx.RespondAsync("Already connected in this guild.");
        //         return;
        //     }
        //
        //     // get member's voice state
        //     var vstat = ctx.Member?.VoiceState;
        //     if (vstat?.Channel == null && chn == null)
        //     {
        //         // they did not specify a channel and are not in one
        //         await ctx.RespondAsync("You are not in a voice channel.");
        //         return;
        //     }
        //
        //     // channel not specified, use user's
        //     if (chn == null)
        //         chn = vstat.Channel;
        //
        //     // connect
        //     vnc = await vnext.ConnectAsync(chn);
        //     await ctx.RespondAsync($"Connected to `{chn.Name}`");
        // }

        [Command("leave"), Description("Leaves a voice channel.")]
        public async Task Leave(CommandContext ctx)
        {
            // check whether VNext is enabled
            var vnext = ctx.Client.GetVoiceNext();
            if (vnext == null)
            {
                // not enabled
                await ctx.RespondAsync("VNext is not enabled or configured.");
                return;
            }

            // check whether we are connected
            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc == null)
            {
                // not connected
                await ctx.RespondAsync("Not connected in this guild.");
                return;
            }

            // disconnect
            vnc.Disconnect();
            await ctx.RespondAsync("Disconnected");
        }

        [Command("play"), Description("Joins a voice channel.")]
        public async Task Play(CommandContext ctx, [RemainingText] string search)
        {
            Console.WriteLine("play");
            if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
            {
                await ctx.RespondAsync("You are not in a voice channel.");
                return;
            }

            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
            Console.WriteLine("1");
            if (conn == null)
            {
                await ctx.RespondAsync("Lavalink is not connected.");
                return;
            }
            Console.WriteLine("1.5");
            var loadResult = await node.Rest.GetTracksAsync(search);
            Console.WriteLine("2");
            if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed 
                || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
            {
                await ctx.RespondAsync($"Track search failed for {search}.");
                return;
            }

            var track = loadResult.Tracks.First();
            Console.WriteLine("3");

            await conn.PlayAsync(track);

            await ctx.RespondAsync($"Now playing {track.Title}!");
        }
}