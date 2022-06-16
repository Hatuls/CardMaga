using System;
using System.Collections.Generic;
using Battles.UI;
using UnityEngine;
using Object = System.Object;

[Serializable]
public class CardUIPool : MonoBehaviour
{
    [SerializeField]
    protected GameObject _prefabOfType;

    [SerializeField] private RectTransform _parent;

    private Stack<CardUI> _poolToType = new Stack<CardUI>();

    [SerializeField] private List<CardUI> _totalPoolType = new List<CardUI>();
    public CardUI Pull()
    {
        CardUI cache = null;

        if (_poolToType.Count > 0)
            cache = _poolToType.Pop();
        else
            cache = GenerateNewOfType();

        return cache;
    }

    private CardUI GenerateNewOfType()
    {
        CardUI cache = MonoBehaviour.Instantiate(_prefabOfType,_parent).GetComponent<CardUI>();
        cache.OnDisposed += AddToQueue;
        _totalPoolType.Add(cache);
        return cache;
    }

    private void AddToQueue(CardUI type)
    {
        _poolToType.Push(type);
        type.gameObject.SetActive(false);
    }
    public void ResetPool()
    {
        for (int i = 0; i < _totalPoolType.Count; i++)
            _totalPoolType[i].Dispose();
    }

    private void OnDestroy()
    {
        {
            for (int i = 0; i < _totalPoolType.Count; i++)
                _totalPoolType[i].OnDisposed -= AddToQueue;
        }
    }
}
