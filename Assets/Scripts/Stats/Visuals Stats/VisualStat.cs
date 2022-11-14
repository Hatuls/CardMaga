using CardMaga.Keywords;
using System;
using System.Collections.Generic;
namespace CardMaga.Battle.Visual
{
    public class VisualStat
    {
        public event Action<int> OnValueReduced;
        public event Action<int> OnValueAdded;
        public event Action<int> OnValueChanged;
        public event Action<KeywordType,int> OnKeywordValueChanged;
        private int _amount;
        private KeywordType _keywordType;
        public int Amount
        {
            get => _amount; 
            set
            {
                if (_amount < value)
                    OnValueReduced?.Invoke(value);
                else if (_amount > value)
                    OnValueAdded?.Invoke(value);
                else
                    return;

                _amount = value;
                OnKeywordValueChanged?.Invoke(_keywordType,_amount);
                OnValueChanged?.Invoke(_amount);
            }
        }
        public KeywordType KeywordType => _keywordType;
        public VisualStat(KeywordType keywprdType,int amount)
        {
            _keywordType = keywprdType;
            Amount = amount;
        }

    }


    public class VisualStatHandler
    {
        private Dictionary<KeywordType, VisualStat> _visualStatsDictionary;

        public IReadOnlyDictionary<KeywordType, VisualStat> VisualStatsDictionary => _visualStatsDictionary;

        public VisualStatHandler(IVisualPlayer player)
        {
            var dictionary = player.PlayerData.StatsHandler.StatDictionary;

            _visualStatsDictionary = new Dictionary<KeywordType, VisualStat>(dictionary.Count);
            foreach (var stat in dictionary)
            {
                _visualStatsDictionary.Add(stat.Key, new VisualStat(stat.Key,stat.Value.Amount));

            }
        }

        public VisualStat GetStat(KeywordType keywordType)
        {
            if (VisualStatsDictionary.TryGetValue(keywordType, out VisualStat visualStat))
                return visualStat;
            return null;
        }


        public void Dispose(IVisualPlayer Character)
        {
            //   var dictionary = Character.PlayerData.StatsHandler.StatDictionary;
        }
    }


}