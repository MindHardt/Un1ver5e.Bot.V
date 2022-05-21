using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Services
{
    /// <summary>
    /// Represents a throw of a single <see cref="Core.Dice"/> object.
    /// </summary>
    public record ThrowResult
    {
        public Dice Dice { get; }
        public IReadOnlyCollection<int> Throws { get; }
        public int Modifyer { get; }

        public int GetThrowsSum() => Throws.Sum();
        public int GetCompleteSum() => Throws.Sum() + Modifyer;
        internal ThrowResult(Dice dice, IEnumerable<int> throws, int modifyer = 0)
        {
            Dice = dice;
            Throws = throws.ToArray();
            Modifyer = modifyer;
        }

        public static explicit operator int(ThrowResult result) => result.GetCompleteSum();

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

        public int CompareTo(ThrowResult? other)
        {
            if (other is null) throw new NullReferenceException();
            return GetCompleteSum().CompareTo(other.GetCompleteSum());
        }
    }
}
