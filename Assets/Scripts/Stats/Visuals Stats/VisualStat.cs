using Battle;
using CardMaga.Commands;
using Characters.Stats;
using Keywords;
using Managers;
using System;
using System.Collections.Generic;
namespace CardMaga.Battle.Visual
{
    public class VisualStat
    {
        public event Action<int> OnValueReduced;
        public event Action<int> OnValueAdded;
        public event Action<int> OnValueChanged;
        private int _amount;
        public int Amount
        {
            get => _amount; set
            {
                if (_amount < value)
                    OnValueReduced?.Invoke(value);
                else if (_amount > value)
                    OnValueAdded?.Invoke(value);
                else
                    return;

                _amount = value;
                OnValueChanged?.Invoke(_amount);
            }
        }

        public VisualStat(int amount)
        {
            Amount = amount;
        }

    }


    public class VisualStatHandler 
    {
        private Dictionary<KeywordTypeEnum, VisualStat> _visualStatsDictionary;
 
        public IReadOnlyDictionary<KeywordTypeEnum, VisualStat> VisualStatsDictionary => _visualStatsDictionary;

        public VisualStatHandler(IPlayer player)
        {
            var dictionary = player.StatsHandler.StatDictionary;

            _visualStatsDictionary = new Dictionary<KeywordTypeEnum, VisualStat>(dictionary.Count);
            foreach (var stat in dictionary)
            {
                _visualStatsDictionary.Add(stat.Key, new VisualStat(stat.Value.Amount));
           
            }
        }


     

        public void Dispose(IPlayer Character)
        {
            var dictionary = Character.StatsHandler.StatDictionary;
        }
    }


}