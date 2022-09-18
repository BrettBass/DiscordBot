using System.ComponentModel.DataAnnotations;

namespace DiscordBot.Models;

public abstract class Entity
{
    [Key] public ulong Id { get; set; }

    public abstract string TableEntityString();
    public abstract string ToEntityString();

    public abstract string Table();
}