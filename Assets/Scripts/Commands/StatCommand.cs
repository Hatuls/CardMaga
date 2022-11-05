using Characters.Stats;

namespace CardMaga.Commands
{
    public class StatCommand : ICommand
    {
        private readonly int _amount;
        private readonly BaseStat _baseStat;

        public StatCommand(BaseStat baseStat, int amount)
        {
            _baseStat = baseStat;
            _amount = amount;
        }
        public void Execute()
        {
            if (_amount > 0)
                _baseStat.Add(_amount);
            else if (_amount < 0)
                _baseStat.Reduce(_amount);

        }

        public void Undo()
        {
            if (_amount < 0)
                _baseStat.Add(_amount);
            else if (_amount > 0)
                _baseStat.Reduce(_amount);

        }
    }

}