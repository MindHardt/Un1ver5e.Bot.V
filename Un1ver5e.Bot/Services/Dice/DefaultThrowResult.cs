using System.Text;

namespace Un1ver5e.Bot.Services.Dice
{
    /// <summary>
    /// Represents a throw of a single <see cref="Core.Dice"/> object.
    /// </summary>
    public record DefaultThrowResult : IThrowResult
    {
        public DefaultDice Dice { get; }
        public IReadOnlyCollection<int> Throws { get; }
        public int Modifyer { get; }

        public int GetThrowsSum() => Throws.Sum();
        public int GetCompleteSum() => Throws.Sum() + Modifyer;
        internal DefaultThrowResult(DefaultDice dice, IEnumerable<int> throws, int modifyer = 0)
        {
            Dice = dice;
            Throws = throws.ToArray();
            Modifyer = modifyer;
        }

        public override string ToString()
        {
            bool modifyerPositive = Modifyer >= 0;
            string modifyerSign = modifyerPositive ? "+" : "";

            StringBuilder steps = new();
            foreach (int result in Throws)
            {
                steps.Append(result.ToString() + "+");
            }
            if (modifyerPositive == false)
            {
                steps.Remove(steps.Length - 1, 1);
            }

            steps.Append(Modifyer);

            return $"{Dice}{modifyerSign}{Modifyer} => {steps} => {GetThrowsSum()}{modifyerSign}{Modifyer} => {GetCompleteSum()}";
        }
    }
}
