using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Context;

public class SqliteContext : DbContext
{
    public readonly string ConnectionString = Environment.GetEnvironmentVariable("DISCORDBOTDB_CONNECTIONSTRING") ?? "Data Source=/src/storage/Sqlite.db";

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite(ConnectionString);
}