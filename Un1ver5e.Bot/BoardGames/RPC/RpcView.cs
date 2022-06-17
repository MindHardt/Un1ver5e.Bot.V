using Disqord;
using Disqord.Extensions.Interactivity.Menus;
using Un1ver5e.Bot.Services.Database;
using Un1ver5e.Bot.Services.Database.Entities;
using Un1ver5e.Bot.Utilities;

namespace Un1ver5e.Bot.BoardGames.TicTacToe
{
    public class RpcView : ViewBase
    {
        private readonly ApplicationContext dbctx;
        private readonly IMember offerer;
        private readonly IMember opponent;

        private int? offererOption;
        private int? opponentOption;
        // 0 => rock
        // 1 => paper
        // 2 => scissors

        public RpcView(ApplicationContext dbctx, IMember offerer, IMember opponent) : base(default)
        {
            this.dbctx = dbctx;
            this.offerer = offerer;
            this.opponent = opponent;

            string offererName = offerer.GetDisplayName();
            string opponentName = offerer.GetDisplayName();

            TemplateMessage = new LocalMessage()
            {
                Embeds = new List<LocalEmbed>()
                {
                    new LocalEmbed()
                    {
                        Fields = new List<LocalEmbedField>()
                        {
                            new LocalEmbedField()
                            {
                                Name = offererName,
                                Value = ":x:",
                                IsInline = true
                            },
                            new LocalEmbedField()
                            {
                                Name = opponentName,
                                Value = ":x:",
                                IsInline = true
                            }
                        }
                    }
                }
            };

            RestoreButtons();
        }

        private async ValueTask Play(ButtonEventArgs e)
        {
            if (e.AuthorId != offerer.Id && e.AuthorId != opponent.Id) return;

            if (e.AuthorId == offerer.Id)   offererOption = e.Button.Position!.Value;
            else                            opponentOption = e.Button.Position!.Value;

            TemplateMessage = new LocalMessage()
            {
                Embeds = new List<LocalEmbed>()
                {
                    new LocalEmbed()
                    {
                        Fields = new List<LocalEmbedField>()
                        {
                            new LocalEmbedField()
                            {
                                Name = offerer.GetDisplayName(),
                                Value = offererOption is null ? ":x:" : ":white_check_mark:",
                                IsInline = true
                            },
                            new LocalEmbedField()
                            {
                                Name = opponent.GetDisplayName(),
                                Value = opponentOption is null ? ":x:" : ":white_check_mark:",
                                IsInline = true
                            }
                        }
                    }
                }
            };
            RestoreButtons();

            if (offererOption.HasValue && opponentOption.HasValue)
            {
                IMember? winner = offererOption switch
                {
                    0 => opponentOption switch
                    {
                        0 => null,      // rock-rock
                        1 => opponent,  // rock-paper
                        2 => offerer,   // rock-scissors
                        _ => throw new ArgumentException()
                    },
                    1 => opponentOption switch
                    {
                        0 => offerer,   // paper-rock
                        1 => null,      // paper-paper
                        2 => opponent,  // paper-scissors
                        _ => throw new ArgumentException()
                    },
                    2 => opponentOption switch
                    {
                        0 => opponent,  // scissors-rock
                        1 => offerer,   // scissors-paper
                        2 => null,      // scissors-scissors
                        _ => throw new ArgumentException()
                    },
                    _ => throw new ArgumentException()
                };

                await EndGame(winner);
            }
        }

        private async ValueTask EndGame(IMember? winner)
        {
            RpcData offererData = dbctx.GetRpc(offerer.Id);
            RpcData opponentData = dbctx.GetRpc(opponent.Id);

            LocalEmbed embed;

            if (winner == null)
            {
                string offererName = offerer.GetDisplayName();
                string opponentName = opponent.GetDisplayName();

                embed = new()
                {
                    Title = $"Ничья!",
                    Description = $"Игра между {offererName} и {opponentName}",
                    ThumbnailUrl = Menu.Client.CurrentUser.GetAvatarUrl()
                };

                offererData.Draw++;
                opponentData.Draw++;
            }
            else
            {
                bool offererWin = offerer == winner;
                IMember loser = offererWin ? opponent : offerer;

                embed = new()
                {
                    Title = $"Победил {winner.GetDisplayName()}",
                    Description = $"Противник {loser.GetDisplayName()}",
                    ThumbnailUrl = winner.GetGuildAvatarUrl()
                };

                if (offererWin)
                {
                    offererData.Win++;
                    opponentData.Lose++;
                }
                else
                {
                    offererData.Lose++;
                    opponentData.Win++;
                }
            }

            //Updating message
            TemplateMessage = new LocalMessage().AddEmbed(embed);
            //Disabling buttons
            foreach (ButtonViewComponent button in EnumerateComponents())
            {
                button.IsDisabled = true;
            }
            //Saving
            await Menu.ApplyChangesAsync();

            //Updating data in database
            dbctx.Update(offererData);
            dbctx.Update(opponentData);
            await dbctx.SaveChangesAsync();
        }

        private void RestoreButtons()
        {
            IEnumerable<ButtonViewComponent> newButtons = new ButtonViewComponent[]
            {
                new ButtonViewComponent(Play)
                {
                    Style = LocalButtonComponentStyle.Secondary,
                    IsDisabled = false,
                    Label = ".",
                    Emoji = LocalEmoji.Unicode("⛰️"),
                    Position = 0
                },
                new ButtonViewComponent(Play)
                {
                    Style = LocalButtonComponentStyle.Secondary,
                    IsDisabled = false,
                    Label = ".",
                    Emoji = LocalEmoji.Unicode("🧻"),
                    Position = 1
                },
                new ButtonViewComponent(Play)
                {
                    Style = LocalButtonComponentStyle.Secondary,
                    IsDisabled = false,
                    Label = ".",
                    Emoji = LocalEmoji.Unicode("✂️"),
                    Position = 2
                },
            };

            this.ReplaceComponents(newButtons);
        }


    }
}
