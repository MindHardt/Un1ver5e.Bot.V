using Disqord;
using Disqord.Bot;
using Qmmands;
using Un1ver5e.Bot.Commands.Attributes;

namespace Un1ver5e.Bot.Commands
{
    [DisableHelp]
    public class HelpCommandModule : DiscordModuleBase
    {
        [Command("help")]
        public DiscordCommandResult HelpCommand()
        {
            CommandService commandService = (CommandService)Context.Services.GetService(typeof(CommandService))!;

            IEnumerable<Module> modules = commandService.GetAllModules()
                .Where(m => m.Attributes.All(a => a is not DisableHelpAttribute))
                .Where(m => m.RunChecksAsync(Context).GetAwaiter().GetResult().IsSuccessful);
            List<LocalEmbedField> moduleDescriptions = new();

            foreach (Module module in modules)
            {
                int commandNameOffset = module.Aliases.Any() ? module.Aliases.First().Length + 1 : 0;

                string[] aliases = module.Aliases
                    .Select(a => $"`{a}`")
                    .ToArray();
                string[] commands = module.Commands
                    .OrderBy(c => c.Name)
                    .Select(c => $"`{c.Name[commandNameOffset..]}`")
                    .ToArray();

                LocalEmbedField field = new()
                {
                    Name = $"**{module.Name}** ({string.Join(", ", aliases)})",
                    Value = string.Join(", ", commands)
                };

                moduleDescriptions.Add(field);
            }

            LocalMessage message = new()
            {
                Embeds = new List<LocalEmbed>()
                {
                    new LocalEmbed()
                    {
                        Fields = moduleDescriptions
                    }
                }
            };

            return Reply(message);
        }

        [Command("help")]
        public DiscordCommandResult HelpCommand(string command)
        {
            CommandService commandService = (CommandService)Context.Services.GetService(typeof(CommandService))!;

            IEnumerable<Command> commands = commandService.GetAllModules()
                .Where(m => m.Attributes.All(a => a is not DisableHelpAttribute))
                .SelectMany(m => m.Commands)
                .Where(c => c.RunChecksAsync(Context).GetAwaiter().GetResult().IsSuccessful)
                .Where(c => c.Aliases.Append(c.Name).Contains(command));

            Command match = commands.Single();

            LocalMessage message = new()
            {
                Embeds = new List<LocalEmbed>()
                {
                    new LocalEmbed()
                    {
                        Fields = new List<LocalEmbedField>()
                        {
                            new LocalEmbedField()
                            {
                                Name = match.Name,
                                Value = match.Description
                            }
                        }
                    }
                }
            };

            return Reply(message);
        }
    }
}
