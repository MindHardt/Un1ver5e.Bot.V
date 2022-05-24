namespace Un1ver5e.Bot.Services.Dice
{
    public interface IDice
    {
        public IEnumerable<int> GetResults(Random random);
        public IThrowResult Throw(Random random, int modifyer = 0);
    }
}
