using DiscordBot.Context;
using DiscordBot.Models;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Modules;

[RequireUserPermissions(Permissions.Administrator)]
public class AdminCommandModule: BaseCommandModule
{
    [Command("migratelite")]
    public async Task MigrateLite(CommandContext ctx)
    {
        Console.WriteLine("*** migrating... ***");
    
        await using var lite = new SqliteContext();
    
        if (lite.Database.GetPendingMigrationsAsync().Result.Any())
        {
            try{await lite.Database.MigrateAsync();}
            catch(Exception e) { Console.WriteLine(e.InnerException?.Message);}
            
        }
    
        await ctx.Channel.SendMessageAsync("Sqlite Migration complete.");
    }

    [Command("pub")]
    public async Task Pub(CommandContext ctx)
    {
        using (SqliteContext lite = new SqliteContext())
        {
            var userTab = lite.Tabs.FirstOrDefault(tab => tab.Id == ctx.User.Id);
            await ctx.Channel.SendMessageAsync(userTab?.Drinks.ToString());
        }
    }
}