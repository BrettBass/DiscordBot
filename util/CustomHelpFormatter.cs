using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace discordBot.util;

public class CustomHelpFormatter : DefaultHelpFormatter
{
    protected DiscordEmbedBuilder _embed;

    public CustomHelpFormatter(CommandContext ctx) : base(ctx)
    {
        _embed = new DiscordEmbedBuilder();

        // Help formatters do support dependency injection.
        // Any required services can be specified by declaring constructor parameters. 

        // Other required initialization here ...
    }

    public override BaseHelpFormatter WithCommand(Command command)
    {
        _embed.AddField(command.Name, command.Description);            

        return this;
    }

    public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> cmds)
    {
        foreach (var cmd in cmds)
        {
            _embed.AddField(cmd.Name, cmd.Description);
        }

        return this;
    }
    //TODO Sort commands by Module
    public IEnumerable<Command> Sort(IEnumerable<Command> cmds)
    {

        return cmds;
    }
    public override CommandHelpMessage Build()
    {
        return new CommandHelpMessage(embed: _embed);
    }
}