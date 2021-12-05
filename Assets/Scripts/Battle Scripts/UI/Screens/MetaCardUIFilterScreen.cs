using Cards;
using Map.UI;
using System.Collections.Generic;
using System.Linq;
using UI.Meta.Laboratory;
using UnityEngine;

public class MetaCardUIFilterScreen : UIFilterScreen<MetaCardUIHandler, Card>
{
//[SerializeField]
    [SerializeField]
    CollectionLabOpen _metaCardUI;
    [SerializeField]
    Transform _container;
    [SerializeField] float _metaCardSize = 1f;



    protected override void CreatePool()
    {
        var deck = Account.AccountManager.Instance.AccountCards.CardList;
        while (deck.Count > _collection.Count)
        {
            var card = Instantiate(_cardUIPrefab, _container).GetComponent<MetaCardUIHandler>();
            //  card.MetaCardUIOpenerAbst = _metaCardUI;

            _collection.Add(card);
        }
    }

    protected override void OnActivate(IEnumerable<Card> sortedDeck, int i)
    {
        _collection[i].CardUI.GFX.SetCardReference(
            sortedDeck.ElementAt(i)
            );
        _collection[i].transform.localScale = Vector3.one * _metaCardSize;

    }




}
