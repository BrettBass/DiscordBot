using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace discordBot.util;

public class CustomHelpFormatter : DefaultHelpFormatter
{
    protected DiscordEmbedBuilder _embed;
    // protected StringBuilder _strBuilder;

    public CustomHelpFormatter(CommandContext ctx) : base(ctx)
    {
        _embed = new DiscordEmbedBuilder();
        _embed.Color = new Optional<DiscordColor>(DiscordColor.SapGreen);
        _embed.WithThumbnail(
            "https://akns-images.eonline.com/eol_images/Entire_Site/2019911/rs_600x600-191011143636-600x600-nancy-gj-10-11-19.jpg?fit=around%7C1200:1200&output-quality=90&crop=1200:1200;center,top");

        // _strBuilder = new StringBuilder();

        // Help formatters do support dependency injection.
        // Any required services can be specified by declaring constructor parameters. 

        // Other required initialization here ...
    }

    public override BaseHelpFormatter WithCommand(Command command)
    {
        _embed.Title = "**" + command.Name + "**";
        _embed.WithThumbnail(
            "https://arc-anglerfish-arc2-prod-bostonglobe.s3.amazonaws.com/public/6VTZYSW4PAI6PA6TAHWDXIMC6E.jpg");
        _embed.AddField("Aliases", string.Join(" ", command.Aliases), true);
        _embed.AddField("Description", command.Description);

        return this;
    }

    public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> cmds)
    {
        _embed.Title = "**Help**";
        var commands = "Commands";
        var output = "";
        foreach (var cmd in cmds)
        {
            if (cmd.IsHidden || cmd.Name is "help") continue;

            output += "`" + cmd.Name + "`\n";
        }

        if (output is not "") _embed.AddField(commands, output);
        return this;
    }

    public override CommandHelpMessage Build()
    {
        return new CommandHelpMessage(embed: _embed);
        // return new CommandHelpMessage(content: _strBuilder.ToString());
    }
}