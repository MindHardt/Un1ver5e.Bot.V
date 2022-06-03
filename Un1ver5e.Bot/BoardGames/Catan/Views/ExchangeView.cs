using Disqord;
using Disqord.Extensions.Interactivity.Menus;
using Un1ver5e.Bot.BoardGames.Catan.Entities;
using Un1ver5e.Bot.Utilities;

namespace Un1ver5e.Bot.BoardGames.Catan.Views
{
    public class ExchangeView : ViewBase
    {
        private bool offererAgree;
        private bool recipientAgree;

        private readonly CatanPlayer offerer;
        private readonly CatanPlayer recipient;
        //These indicate how much of resources is given from offerer to recipient
        //Negative values mean that resources if given from recipient to offerer
        //Positive values mean that resources if given from offerer to recipient
        private int[] exchanges = new int[5];


        public ExchangeView(CatanPlayer offerer, CatanPlayer recipient) : base(default)
        {
            this.offerer = offerer;
            this.recipient = recipient;

            TemplateMessage = new LocalMessage().AddEmbed(CreateEmbed());
            this.ReplaceComponents(CreateButtons());
        }

        private LocalEmbed CreateEmbed()
        {

            return new LocalEmbed()
            {
                Fields =
                {
                    GetPlayerTab(offerer, true),
                    GetExchangeTab(),
                    GetPlayerTab(recipient, false)
                }
            };
        }
        private LocalEmbedField GetPlayerTab(CatanPlayer player, bool invertDeltas)
        {
            int[] values =
            {
                player.Resources[Resource.Clay],
                player.Resources[Resource.Lumber],
                player.Resources[Resource.Ore],
                player.Resources[Resource.Sheep],
                player.Resources[Resource.Wheat]
            };

            int[] deltas = invertDeltas ? exchanges.Select(ex => -ex).ToArray() : exchanges;

            string[] resources = Enumerable.Range(0, 5)
                .Select(index => $"{s_resourceEmojis[index]} {values[index]} {(deltas[index] != 0 ? $"({deltas[index].AsModifyer()})" : "")}")
                .ToArray();

            bool isReady = player == offerer ? offererAgree : recipientAgree;

            return new()
            {
                Name = $"{(isReady ? "✅" : "🚫")} {player.User.GetDisplayName()}",
                Value = string.Join('\n', resources).AsCodeBlock(),
                IsInline = true
            };
        }
        private LocalEmbedField GetExchangeTab()
        {
            int[] actualExchanges = Enumerable.Range(0, 5)
                .Where(index => exchanges[index] != 0)
                .OrderBy(index => exchanges[index])
                .ToArray();

            (int[] to, int[] from) splitted = new(
                actualExchanges.Where(i => exchanges[i] < 0).ToArray(),
                actualExchanges.Where(i => exchanges[i] > 0).ToArray());

            string to = string.Join('\n',
                splitted.to.Select(index => $"<- {s_resourceEmojis[index]} {-exchanges[index]}"));

            string from = string.Join('\n',
                splitted.from.Select(index => $"{s_resourceEmojis[index]} {-exchanges[index]} ->"));

            string value = string.Join("\n\n", to, from);

            return new()
            {
                IsInline = true,
                Name = "Обмен",
                Value = (string.IsNullOrWhiteSpace(value) ? "-" : value).AsCodeBlock()
            };
        }



        private ButtonViewComponent[] CreateButtons()
        {
            ButtonViewComponent[] buttons = new ButtonViewComponent[12];

            for (int index = 0; index < 5; index++)
            {
                ButtonViewComponent increase = new(ChangeValue)
                {
                    Emoji = LocalEmoji.Unicode(s_resourceEmojis[index]),
                    IsDisabled = offerer.Resources.Has((Resource)index, Math.Abs(exchanges[index] + 1)) == false,
                    Label = "▶",
                    Row = 0,
                    Position = index,
                    Style = LocalButtonComponentStyle.Primary
                };
                buttons[index] = increase;

                ButtonViewComponent decrease = new(ChangeValue)
                {
                    Emoji = LocalEmoji.Unicode(s_resourceEmojis[index]),
                    IsDisabled = recipient.Resources.Has((Resource)index, Math.Abs(exchanges[index] - 1)) == false,
                    Label = "◀",
                    Row = 1,
                    Position = index,
                    Style = LocalButtonComponentStyle.Primary
                };
                buttons[index + 5] = decrease;
            }

            ButtonViewComponent agree = new(Agree)
            {
                Emoji = LocalEmoji.Unicode("🤝"),
                IsDisabled = false,
                Row = 2,
                Position = 0,
                Style = LocalButtonComponentStyle.Success
            };
            buttons[10] = agree;

            ButtonViewComponent cancel = new(CancelExchange)
            {
                IsDisabled = false,
                Label = "Отмена",
                Row = 2,
                Position = 1,
                Style = LocalButtonComponentStyle.Danger
            };
            buttons[11] = cancel;

            return buttons;
        }



        private async ValueTask ChangeValue(ButtonEventArgs e)
        {
            int index = e.Button.Position!.Value;

            bool increase = e.Button.Row == 0;

            if (increase)
                exchanges[index]++;
            else
                exchanges[index]--;

            offererAgree = false;
            recipientAgree = false;
            TemplateMessage = new LocalMessage().AddEmbed(CreateEmbed());
        }
        private async ValueTask CancelExchange(ButtonEventArgs e)
        {
            IMember canceller = e.AuthorId == offerer.User.Id ? offerer.User : recipient.User;

            TemplateMessage = new LocalMessage()
                .WithContent($"**{canceller.GetDisplayName()} отменил сделку.**")
                .AddEmbed(CreateEmbed());

            ClearComponents();

            await Menu.ApplyChangesAsync();
            Menu.Stop();
            await Menu.DisposeAsync();
        }
        private async ValueTask Agree(ButtonEventArgs e)
        {
            if (e.AuthorId == offerer.User.Id) 
                offererAgree = true;
            else if (e.AuthorId == recipient.User.Id) 
                recipientAgree = true;

            TemplateMessage = new LocalMessage().AddEmbed(CreateEmbed());
        }


        private static string[] s_resourceEmojis = new string[]
        {
            "🧱", "🌳", "⛏️", "🐑", "🍞"
        };
    }
}
