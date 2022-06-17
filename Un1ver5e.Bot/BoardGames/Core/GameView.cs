using Disqord;
using Disqord.Extensions.Interactivity.Menus;
using Un1ver5e.Bot.Utilities;

namespace Un1ver5e.Bot.BoardGames.Core
{
    public abstract class GameView : ViewBase
    {
        /// <summary>
        /// Creates a new <see cref="GameView"/> which appears in a "loading" state. Once it successfully calls <see cref="CreateMessageAsync()"/> and <see cref="CreateComponentsAsync()"/>, it starts to appear the usual way.
        /// </summary>
        public GameView() : base(default)
        {
            TemplateMessage = new LocalMessage()
                .AddEmbed(new LocalEmbed()
                .WithDescription("Загрузка... 🕑".AsCodeBlock()));

            Task.Run(async () =>
            {
                await Task.WhenAll(
                    UpdateMessageAsync().AsTask(), 
                    UpdateComponentsAsync().AsTask());

                await UpdateAsync();
            });
        }

        /// <summary>
        /// Asyncronously updates the appearance of the message. This is called once in ctor.
        /// </summary>
        /// <returns></returns>
        public async ValueTask UpdateMessageAsync() => TemplateMessage = await CreateMessageAsync();

        /// <summary>
        /// Asyncronously updates the components of the message. This is called once in ctor.
        /// </summary>
        /// <returns></returns>
        public async ValueTask UpdateComponentsAsync() => this.ReplaceComponents(await CreateComponentsAsync());

        /// <summary>
        /// Creates a <see cref="LocalMessage"/> object for this <see cref="GameView"/>.
        /// </summary>
        /// <returns></returns>
        public abstract ValueTask<LocalMessage> CreateMessageAsync();

        /// <summary>
        /// Creates a list of <see cref="ViewComponent"/>s for this <see cref="GameView"/>.
        /// </summary>
        /// <returns></returns>
        public abstract ValueTask<IEnumerable<ViewComponent>> CreateComponentsAsync();

        /// <summary>
        /// Disposes this view. By default this disables all the components.
        /// </summary>
        /// <returns></returns>
        public override ValueTask DisposeAsync()
        {
            this.ReplaceComponents(Array.Empty<ViewComponent>());

            return ValueTask.CompletedTask;
        }
    }
}
