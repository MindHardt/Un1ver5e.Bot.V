using Disqord;
using Disqord.Bot;
using Qmmands;
using Un1ver5e.Bot.Services.Graphics;

namespace Un1ver5e.Bot.Commands
{
    [Name("🎨 Графика"), Description("Бот рисует, вау!")]
    public partial class GraphicCommands : DiscordModuleBase
    {
        public GraphicCommands(IGraphics service, Random random)
        {
            this.service = service;
            this.random = random;
        }

        private readonly Random random;
        private readonly IGraphics service;

        [Command("color", "hex"), Description("Смари какой цвет.")]
        public async ValueTask<DiscordCommandResult> ColorCommand(Color color)
        {
            Stream imageStream = await service.GetColorImage(color);
            LocalAttachment imageAttachment = new LocalAttachment(imageStream, "color.png");

            LocalMessage msg = new LocalMessage().AddAttachment(imageAttachment);

            return Reply(msg);
        }
    }
}