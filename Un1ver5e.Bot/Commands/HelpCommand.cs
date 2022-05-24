using Disqord;
using Disqord.Bot;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un1ver5e.Bot.Commands.Attributes;
using Un1ver5e.Bot.Utilities;

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
                LocalEmbedField field = new()
                {
                    Name = $"**{module.Name}** ({string.Join(", ", module.Aliases.Select(a => $"`{a}`"))})",
                    Value = string.Join(", ", module.Commands.Select(c => $"`{c.Name[commandNameOffset..]}`"))
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
    }
}
