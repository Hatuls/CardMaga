using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Card
{

    [Serializable]
    public class CardUIPool : MonoBehaviour
    {
        [SerializeField]
        protected GameObject _prefabOfType;

        [SerializeField] private RectTransform _parent;

        private Stack<BattleCardUI> _poolToType = new Stack<BattleCardUI>();

        [SerializeField] private List<BattleCardUI> _totalPoolType = new List<BattleCardUI>();
        
        public BattleCardUI Pull()
        {
            BattleCardUI cache = null;

            if (_poolToType.Count > 0)
                cache = _poolToType.Pop();
            else
                cache = GenerateNewOfType();

            return cache;
        }

        private BattleCardUI GenerateNewOfType()
        {
            BattleCardUI cache = MonoBehaviour.Instantiate(_prefabOfType, _parent).GetComponent<BattleCardUI>();
            cache.OnDisposed += AddToQueue;
            _totalPoolType.Add(cache);
            return cache;
        }

        private void AddToQueue(BattleCardUI type)
        {
            _poolToType.Push(type);
            type.gameObject.SetActive(false);
        }
        public void ResetPool()
        {
            for (int i = 0; i < _totalPoolType.Count; i++)
                _totalPoolType[i].Dispose();
        }

        public void Init()
        {
            for (int i = 0; i < _totalPoolType.Count; i++)
                _totalPoolType[i].OnDisposed += AddToQueue;

            ResetPool();
        }

        private void OnDestroy()
        {
            {
                for (int i = 0; i < _totalPoolType.Count; i++)
                    _totalPoolType[i].OnDisposed -= AddToQueue;
            }
        }
    }

}