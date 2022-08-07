using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Yaml;
using DiscordBot.Modules;
using discordBot.util;
using DSharpPlus.CommandsNext.Executors;
using DSharpPlus.Interactivity.Extensions;

namespace DiscordBot;

public class Bot
{
    public  InteractivityExtension Interactivity { get; private set; }
    public DiscordClient Client { get; private set; }
    public CommandsNextExtension Commands { get; private set; }

    public async Task RunAsync()
    {
        var configBuilder = new ConfigurationBuilder()
            //.SetBasePath(AppContext.BaseDirectory)
            .AddYamlFile(Global.ConfigYml)
            .Build();

        var config = new DiscordConfiguration()
        {
            Token = configBuilder["tokens:discord"],
            TokenType = DSharpPlus.TokenType.Bot,
            AutoReconnect = true,
        };
        Client = new DiscordClient(config);
        Client.Ready += OnClientReady;

        Client.UseInteractivity(new InteractivityConfiguration
        {
            Timeout = TimeSpan.FromMinutes(2),
        });

        // Client.UseSlashCommands(new SlashCommandsConfiguration
        // {
        //     
        // });
    
        var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { configBuilder["prefix"] },
                EnableDms = true,
                EnableMentionPrefix = true,
                DmHelp = true,
                IgnoreExtraArguments = true,
                CommandExecutor = new ParallelQueuedCommandExecutor()
                
            };
        Commands = Client.UseCommandsNext(commandsConfig);
        Commands.RegisterCommands<BankCommandModule>();
        Commands.RegisterCommands<PrefixModule>();
        Commands.RegisterCommands<ResponseModule>();
        Commands.RegisterCommands<DrinkingGameModule>();
        Commands.SetHelpFormatter<CustomHelpFormatter>();

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }
    private Task OnClientReady(DiscordClient sender, ReadyEventArgs readyEventArgs) { return Task.CompletedTask; }
    
}