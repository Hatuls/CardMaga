using Battle.Combo;
using CardMaga.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

public class MetaComboUIFilterScreen : UIFilterScreen<ComboRecipeUI, Combo>
{
    public event Func<IReadOnlyCollection<Combo>> OnCollectionNeeded;

    [SerializeField]
    Transform _container;
    [SerializeField] float _comboSize = 1f;

    protected override void CreatePool()
    {
        int deckCount = OnCollectionNeeded.Invoke().Count;

        while (deckCount > _collection.Count)
        {
            var card = Instantiate(_cardUIPrefab, _container).GetComponentInChildren<ComboRecipeUI>();
            _collection.Add(card);
        }
    }

    protected override void OnActivate(IEnumerable<Combo> sortedCombo, int i)
    {
        _collection[i].InitRecipe(sortedCombo.ElementAt(i));
        _collection[i].transform.localScale = Vector3.one * _comboSize;
    }

    public void ShowAll()
    {
        CreatePool();
        int length = _collection.Count;
        var combos = OnCollectionNeeded?.Invoke();
            
        for (int i = 0; i < length; i++)
        {
            if (i < combos.Count)
            {
                _collection[i].InitRecipe(combos.ElementAt(i));

                if (_collection[i].gameObject.activeSelf == false)
                    _collection[i].gameObject.SetActive(true);

            }
            else
            {
                if (_collection[i].gameObject.activeSelf == true)
                    _collection[i].gameObject.SetActive(false);
            }
        }
    }
}
