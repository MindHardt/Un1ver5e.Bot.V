using Disqord;
using Disqord.Bot;
using Qmmands;
using Un1ver5e.Bot.Services.Graphics;

namespace Un1ver5e.Bot.Commands
{
    [Name("🎨 Графика"), Description("Бот рисует, вау!")]
    public partial class GraphicCommands : DiscordModuleBase
    {
        public GraphicCommands(IGraphics service)
        {
            this.service = service;
        }

        private readonly IGraphics service;

        [Command("color", "hex"), Description("Смари какой цвет.")]
        public async ValueTask<DiscordCommandResult> ColorCommand(Color color)
        {
            Stream imageStream = await service.GetColorImage(color);
            LocalAttachment imageAttachment = new LocalAttachment(imageStream, "color.png");

            string colorName = color.ToString();
            LocalMessage msg = new LocalMessage()
                .WithContent(colorName)
                .AddAttachment(imageAttachment);

            return Reply(msg);
        }

        [Command("color", "hex"), Description("Смари какой цвет.")]
        public ValueTask<DiscordCommandResult> ColorCommand() => ColorCommand(Color.Random);
    }
}