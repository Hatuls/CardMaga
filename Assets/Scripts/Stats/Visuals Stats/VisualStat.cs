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
        public event Action<KeywordType, int> OnKeywordValueChanged;


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
                InvokeValueChanges();
            }
        }

        public void InvokeValueChanges()
        {
            OnValueChanged?.Invoke(_amount);
            OnKeywordValueChanged?.Invoke(_keywordType, _amount);
        }

        public KeywordType KeywordType => _keywordType;
        public VisualStat(KeywordType keywprdType, int amount)
        {
            _keywordType = keywprdType;
            Amount = amount;
        }

    }


    public class VisualStatHandler
    {
        public event Action<bool, KeywordType> OnKeywordChanged;
        public event Action<bool, KeywordType,int> OnKeywordStatChanged;

        private bool _isLeft;
        private Dictionary<KeywordType, VisualStat> _visualStatsDictionary;
        public IReadOnlyDictionary<KeywordType, VisualStat> VisualStatsDictionary => _visualStatsDictionary;
        public VisualStatHandler(IVisualPlayer player)
        {
            _isLeft = player.PlayerData.IsLeft;

            var dictionary = player.PlayerData.StatsHandler.StatDictionary;
            _visualStatsDictionary = new Dictionary<KeywordType, VisualStat>(dictionary.Count);
            foreach (var stat in dictionary)
            {
                KeywordType keywordType = stat.Key;

                VisualStat visualStat = new VisualStat(keywordType, stat.Value.Amount);
                _visualStatsDictionary.Add(keywordType, visualStat);
                visualStat.OnKeywordValueChanged -= StatChanged;
            }
        }
        public VisualStat GetStat(KeywordType keywordType)
        {
            if (VisualStatsDictionary.TryGetValue(keywordType, out VisualStat visualStat))
                return visualStat;
            return null;
        }

        internal void UpdateAllStats()
        {
            foreach (var visualStat in VisualStatsDictionary)
                visualStat.Value.InvokeValueChanges();
        }

        private void StatChanged(KeywordType keywordType, int amount)
        {
            if (OnKeywordChanged != null)
                OnKeywordChanged.Invoke(_isLeft, keywordType);
            if (OnKeywordStatChanged != null)
                OnKeywordStatChanged.Invoke(_isLeft, keywordType, amount);
        }

        public void Dispose(VisualCharacter visualCharacter)
        {

            foreach (var stat in VisualStatsDictionary)
            {
                stat.Value.OnKeywordValueChanged -= StatChanged;
            }
        }
    }


}