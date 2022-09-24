using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Context;

public class SqliteContext : DbContext
//"Data Source=/src/storage/Sqlite.db"
{
    private readonly string _connectionString = Environment.GetEnvironmentVariable("DISCORDBOTDB_CONNECTIONSTRING") ?? "Data Source=/src/storage/Sqlite.db";

    public DbSet<BankAccount?> BankAccounts { get; set; }
    public DbSet<Tab> Tabs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite(_connectionString);
}