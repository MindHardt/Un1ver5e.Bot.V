using Disqord;
using Disqord.Extensions.Interactivity.Menus;
using Un1ver5e.Bot.Services.Database;
using Un1ver5e.Bot.Services.Database.Entities;
using Un1ver5e.Bot.Utilities;

namespace Un1ver5e.Bot.BoardGames.TicTacToe
{
    public class TttView : ViewBase
    {
        private readonly ApplicationContext dbctx;
        private readonly IMember cross;
        private readonly IMember nil;
        private readonly bool?[] field = new bool?[9];
        private bool crossTurn = true;

        public TttView(ApplicationContext dbctx, IMember cross, IMember nil) : base(default)
        {
            this.dbctx = dbctx;
            this.cross = cross;
            this.nil = nil;

            string crossName = cross.Nick ?? cross.Name;
            string nilName = nil.Nick ?? nil.Name;

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
                                Name = "Крестики",
                                Value = $":x:{crossName}",
                                IsInline = true
                            },
                            new LocalEmbedField()
                            {
                                Name = "Нолики",
                                Value = $":o:{nilName}",
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
            IMember expected = crossTurn ? cross : nil;

            if (e.AuthorId != expected.Id) return;

            int buttonIndex =
                e.Button.Position!.Value +
                e.Button.Row!.Value * 3;

            field[buttonIndex] = crossTurn;

            RestoreButtons();

            IMember? winner;
            if (CheckWin(buttonIndex, out winner) || CheckDraw())
            {
                await EndGame(winner);
            }

            crossTurn = !crossTurn;
        }

        private async ValueTask EndGame(IMember? winner)
        {
            TicTacToeData nilData = dbctx.GetTicTacToe(nil.Id);
            TicTacToeData crossData = dbctx.GetTicTacToe(cross.Id);

            LocalEmbed embed;

            if (winner == null)
            {
                string crossName = cross.Nick ?? cross.Name;
                string nilName = nil.Nick ?? nil.Name;

                embed = new()
                {
                    Title = $"Ничья!",
                    Description = $"Игра между {crossName} и {nilName}",
                    ThumbnailUrl = Menu.Client.CurrentUser.GetAvatarUrl()
                };

                crossData.Draw++;
                nilData.Draw++;
            }
            else
            {
                bool crossWin = cross == winner;
                IMember loser = crossWin ? nil : cross;

                embed = new()
                {
                    Title = $"Победил {winner.Nick ?? winner.Name}",
                    Description = $"Противник {loser.Nick ?? loser.Name}",
                    ThumbnailUrl = winner.GetGuildAvatarUrl()
                };

                if (crossWin)
                {
                    crossData.Win++;
                    nilData.Lose++;
                }
                else
                {
                    crossData.Lose++;
                    nilData.Win++;
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
            dbctx.Update(crossData);
            dbctx.Update(nilData);
            await dbctx.SaveChangesAsync();
        }

        private void RestoreButtons()
        {
            IEnumerable<ButtonViewComponent> newButtons = field
                .Select((cell, index) =>
                {
                    return new ButtonViewComponent(Play)
                    {
                        Style = LocalButtonComponentStyle.Secondary,
                        IsDisabled = cell.HasValue,
                        Label = cell.HasValue ? null : ".",
                        Emoji = cell switch
                        {
                            null => null,
                            true => LocalEmoji.Unicode("❌"),
                            false => LocalEmoji.Unicode("⭕")
                        },
                        Row = index / 3,
                        Position = index % 3
                    };
                });

            this.ReplaceComponents(newButtons);
        }

        private bool CheckDraw()
        {
            return field.All(c => c.HasValue);
        }
        private bool CheckWin(int index, out IMember? winner)
        {
            if (CheckRow(index / 3, out winner)) return true;

            if (CheckColumn(index % 3, out winner)) return true;

            if (index % 4 == 0 && CheckDescLine(out winner)) return true;

            if (index % 2 == 0 && index < 8 && CheckAscLine(out winner)) return true;

            return false;
        }
        private bool CheckRow(int index, out IMember? winner)
        {
            winner = null;
            if ((field[index].HasValue
                && field[index + 1].HasValue
                && field[index + 2].HasValue) == false)
                return false;

            if (field[index]!.Value != field[index + 1]!.Value ||
                field[index + 1]!.Value != field[index + 2]!.Value)
                return false;

            winner = field[index]!.Value ? cross : nil;

            return true;
        }
        private bool CheckColumn(int number, out IMember? winner)
        {
            winner = null;
            if ((field[number].HasValue
                && field[number + 3].HasValue
                && field[number + 6].HasValue) == false)
                return false;

            if (field[number]!.Value != field[number + 3]!.Value ||
                field[number + 3]!.Value != field[number + 6]!.Value)
                return false;

            winner = field[number]!.Value ? cross : nil;

            return true;
        }
        private bool CheckDescLine(out IMember? winner)
        {
            winner = null;
            if ((field[0].HasValue
                && field[4].HasValue
                && field[8].HasValue) == false)
                return false;

            if (field[0]!.Value != field[4]!.Value ||
                field[4]!.Value != field[8]!.Value)
                return false;

            winner = field[0]!.Value ? cross : nil;

            return true;
        }
        private bool CheckAscLine(out IMember? winner)
        {
            winner = null;
            if ((field[6].HasValue
                && field[4].HasValue
                && field[2].HasValue) == false)
                return false;

            if (field[6]!.Value != field[4]!.Value ||
                field[4]!.Value != field[2]!.Value)
                return false;

            winner = field[6]!.Value ? cross : nil;

            return true;
        }
    }
}
