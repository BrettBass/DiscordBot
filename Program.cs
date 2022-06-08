using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using Discord.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Yaml;
using Microsoft.Extensions.Hosting;
namespace DiscordBot
{
    public class Program
    {
        //static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        private DiscordSocketClient _client;

        public async Task MainAsync(){

            var config = new ConfigurationBuilder()
                //.SetBasePath(AppContext.BaseDirectory)
                .AddYamlFile("C:\\Users\\brett\\Code\\c#\\discordBot\\config.yml")
                .Build();

            Console.WriteLine(config["testGuild"]);

            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) => 
                services
                .AddSingleton(config)
                .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig
                {
                    GatewayIntents = Discord.GatewayIntents.AllUnprivileged,
                    LogGatewayIntentWarnings = false,
                    AlwaysDownloadUsers = true,
                    LogLevel = LogSeverity.Debug
                }))
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionHandler>()
                .AddSingleton(x => new CommandService(new CommandServiceConfig
                {
                    CaseSensitiveCommands = false,
                    DefaultRunMode = Discord.Commands.RunMode.Async,
                    LogLevel = LogSeverity.Debug
                }))
                .AddSingleton<PrefixHandler>()
                )
                .Build();
                await RunAsync(host);
        }

        public async Task RunAsync(IHost host){
            
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            _client = provider.GetRequiredService<DiscordSocketClient>();

            var sCommands = provider.GetRequiredService<InteractionService>();

            var config = provider.GetRequiredService<IConfigurationRoot>();

            await provider.GetRequiredService<InteractionHandler>().InitializeAsync();


            var pCommands = provider.GetRequiredService<PrefixHandler>();
            pCommands.AddModule<DiscordBot.Modules.PrefixModule>();
            await pCommands.InitializeAsync();
            
            _client.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };
            sCommands.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };
            
            _client.Ready += async () => {
                Console.WriteLine("Bot Ready!");
                try{
                await sCommands.RegisterCommandsToGuildAsync(UInt64.Parse(config["testGuild"]), true);
                } catch(Exception e){
                    Console.WriteLine(e.Message);
                }
            };


            await _client.LoginAsync(TokenType.Bot, config["tokens:discord"]);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
       
    }
}