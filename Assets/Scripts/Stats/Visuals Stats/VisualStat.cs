﻿using CardMaga.Keywords;
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
        private Dictionary<KeywordType, VisualStat> _visualStatsDictionary;

        public IReadOnlyDictionary<KeywordType, VisualStat> VisualStatsDictionary => _visualStatsDictionary;

        public VisualStatHandler(IVisualPlayer player)
        {
            var dictionary = player.PlayerData.StatsHandler.StatDictionary;

            _visualStatsDictionary = new Dictionary<KeywordType, VisualStat>(dictionary.Count);
            foreach (var stat in dictionary)
            {
                _visualStatsDictionary.Add(stat.Key, new VisualStat(stat.Value.Amount));

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