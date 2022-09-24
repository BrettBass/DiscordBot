using System.Diagnostics;
using DiscordBot.Modules;
using discordBot.util;
using DotNetEnv;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Executors;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.VoiceNext;
using DSharpPlus.Net;
using DSharpPlus.Lavalink;

namespace DiscordBot;

public class Bot
{
    public InteractivityExtension Interactivity { get; private set; }
    public DiscordClient Client { get; private set; }
    public CommandsNextExtension Commands { get; private set; }
    
    public VoiceNextExtension Voice { get; set; }


    public async Task RunAsync()
    {
        Env.TraversePath().Load();
        Debug.Assert(Environment.GetEnvironmentVariable("TOKEN") != null, "Null TOKEN");
        var config = new DiscordConfiguration
        {
            Token = Environment.GetEnvironmentVariable("TOKEN"),
            TokenType = TokenType.Bot,
            AutoReconnect = true
        };
        Client = new DiscordClient(config);
        Client.Ready += OnClientReady;

        Client.UseInteractivity(new InteractivityConfiguration
        {
            Timeout = TimeSpan.FromMinutes(2)
        });
        

        // Client.UseSlashCommands(new SlashCommandsConfiguration
        // {
        //     
        // });

        var commandsConfig = new CommandsNextConfiguration
        {
            StringPrefixes = new[] { Environment.GetEnvironmentVariable("PREFIX") }!,
            EnableDms = true,
            EnableMentionPrefix = true,
            DmHelp = true,
            IgnoreExtraArguments = true,
            CommandExecutor = new ParallelQueuedCommandExecutor(Int32.Parse(Environment.GetEnvironmentVariable("WORKERS") ?? "1"))
        };
        Commands = Client.UseCommandsNext(commandsConfig);
        
        Commands.RegisterCommands<AdminCommandModule>();
        Commands.RegisterCommands<MusicModule>();
        Commands.RegisterCommands<BankCommandModule>();
        Commands.RegisterCommands<BarCommandModule>();
        Commands.RegisterCommands<PrefixModule>();
        Commands.RegisterCommands<TexasHoldemGameModule>();
        Commands.RegisterCommands<SmokeOrFireGameModule>();
        
        Commands.SetHelpFormatter<CustomHelpFormatter>();
        
        var endpoint = new ConnectionEndpoint
        {
            Hostname = "127.0.0.1", // From your server configuration.
            Port = 2333 // From your server configuration
        };

        var lavalinkConfig = new LavalinkConfiguration
        {
            Password = "youshallnotpass", // From your server configuration.
            RestEndpoint = endpoint,
            SocketEndpoint = endpoint
        };
        
        var lavalink = Client.UseLavalink();

        this.Voice = this.Client.UseVoiceNext();
        await Client.ConnectAsync();
        await lavalink.ConnectAsync(lavalinkConfig);
        await Task.Delay(-1);
    }

    private Task OnClientReady(DiscordClient sender, ReadyEventArgs readyEventArgs)
    {
        return Task.CompletedTask;
    }
}