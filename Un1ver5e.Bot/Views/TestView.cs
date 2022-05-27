using Disqord;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Rest;
using System.Text;
using Un1ver5e.Bot.Utilities;

namespace Un1ver5e.Bot.Views
{
    public class TestView : ViewBase
    {
        private int xPos;
        private int yPos;

        public TestView(int x, int y) : base(default)
        {
            xPos = x;
            yPos = y;
            TemplateMessage = GetMessage();
        }

        public TestView() : base(default)
        {
            xPos = Random.Shared.Next(0, 11);
            yPos = Random.Shared.Next(0, 11);
            TemplateMessage = GetMessage();
        }

        [Button(Emoji = "⬇️")]
        public async ValueTask ClickMeDown(ButtonEventArgs e)
        {
            yPos++;
            Menu.View = new TestView(xPos, yPos);
        }

        [Button(Emoji = "⬅️")]
        public async ValueTask ClickMeLeft(ButtonEventArgs e)
        {
            xPos--;
            Menu.View = new TestView(xPos, yPos);
        }

        [Button(Emoji = "⬆️")]
        public async ValueTask ClickMeUp(ButtonEventArgs e)
        {
            yPos--;
            Menu.View = new TestView(xPos, yPos);
        }

        [Button(Emoji = "➡️")]
        public async ValueTask ClickMeRight(ButtonEventArgs e)
        {
            xPos++;
            Menu.View = new TestView(xPos, yPos);
        }

        private LocalMessage GetMessage()
        {
            StringBuilder sb = new();
            
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    sb.Append(x == xPos && y == yPos ? '⬜' : '⬛');
                }
                sb.Append('\n');
            }

            return new LocalMessage().WithContent(sb.ToString().AsCodeBlock());
        }
    }
}
