using CardMaga.UI;
using System.Collections.Generic;
using System.Linq;
using UI.Meta.Laboratory;
using UnityEngine;
using System;
using CardMaga.Card;

public class MetaCardUIFilterScreen : UIFilterScreen<MetaCardUIHandler, CardData>
{
    public event Func<IReadOnlyCollection<CardData>> OnCollectionNeeded;
    [SerializeField]
    Transform _container;
    [SerializeField] float _metaCardSize = 1f;

    public MetaCardUIHandler GetCardFromInstanceID(int id)
        => _collection.First(x => x.CardUI.CardData.CardInstanceID == id);
    protected override void CreatePool()
    {
        int deckCount = OnCollectionNeeded.Invoke().Count;


        while (deckCount > _collection.Count)
        {
            var card = Instantiate(_cardUIPrefab, _container).GetComponent<MetaCardUIHandler>();
            //  card.MetaCardUIOpenerAbst = _metaCardUI;

            _collection.Add(card);
        }
    }



    protected override void OnActivate(IEnumerable<CardData> sortedDeck, int i)
    {
        _collection[i].CardUI.AssignCard(sortedDeck.ElementAt(i));
        _collection[i].transform.localScale = Vector3.one * _metaCardSize;

    }
}
public class DojoUpgradeScrollHandler : MonoBehaviour
{
    [SerializeField] MetaCardUIFilterScreen _cardFilter;
    [SerializeField] MetaComboUIFilterScreen _comboFilter;

}
