using CardMaga.Keywords;
using CardMaga.Tools.Pools;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class BuffVisualData : IPoolable<BuffVisualData>
    {

        public event Action<BuffVisualData> OnDisposed;

        [SerializeField] KeywordType _keywordType;
        [SerializeField] int _buffCurrentAmount;
        public KeywordType KeywordType { get => _keywordType; }
        public int BuffCurrentAmount { get => _buffCurrentAmount; }


        public void AssignValues(KeywordType keywordType, int buffAmount)
        {
            _keywordType = keywordType;
            _buffCurrentAmount = buffAmount;
        }

        public void Dispose()
        {
            OnDisposed?.Invoke(this);
        }

        public bool IsEmpty()
        {
            return _buffCurrentAmount == 0;
        }
    }
}