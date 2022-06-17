using Disqord;
using Disqord.Bot;
using Disqord.Extensions.Interactivity;
using Disqord.Gateway;
using Microsoft.Extensions.Hosting;
using Qmmands;
using Serilog.Core;
using System.Text.RegularExpressions;
using Un1ver5e.Bot.Services;
using Un1ver5e.Bot.Services.Database;
using Un1ver5e.Bot.Services.Database.Entities;
using Un1ver5e.Bot.Utilities;
using Un1ver5e.Commands.Attributes;

namespace Un1ver5e.Bot.Commands
{
    [RequireDebug]
    [RequireBotOwner]
    [Name("🗒 Теги"), Description("Страшные вещи")]
    public class TagCommands : DiscordModuleBase
    {
        private readonly ApplicationContext dbctx;

        public TagCommands(ApplicationContext dbctx)
        {
            this.dbctx = dbctx;
        }

        [RequireReferencedMessage]
        [Command("createtag")]
        public async ValueTask<DiscordCommandResult> CreateTag(string name)
        {
            IUserMessage referenced = Context.Message.ReferencedMessage.Value;
            name = name.ToLower();

            if (dbctx.Tags.Where(tag => tag.Name == name).Any()) throw new ArgumentException($"Tag {name} already exists");

            MessageTag tag = MessageTag.FromMessage(referenced, name);
            dbctx.Tags.Add(tag);
            await dbctx.SaveChangesAsync();

            return Reply($"Тег {tag.Name} успешно создан!");
        }
    }
}