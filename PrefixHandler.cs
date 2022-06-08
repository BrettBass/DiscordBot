using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot{
    public class PrefixHandler{

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        public PrefixHandler(DiscordSocketClient client, CommandService commands, IConfigurationRoot config){
            _client = client;
            _commands = commands;
            _config = config;
        }

        public async Task InitializeAsync(){ _client.MessageReceived += HandleCommandAsync; }

        public void AddModule<T>(){ _commands.AddModuleAsync<T>(null); }

        private async Task HandleCommandAsync(SocketMessage msgParam) { 
            var msg = msgParam as SocketUserMessage;
            if(msg == null) return;

            int argPos = 0;

            if(!(msg.HasCharPrefix(_config["prefix"][0], ref argPos) || !msg.HasMentionPrefix(_client.CurrentUser, ref argPos) || msg.Author.IsBot)) return;

            var context = new SocketCommandContext(_client, msg);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null
            );

        }
    }
}