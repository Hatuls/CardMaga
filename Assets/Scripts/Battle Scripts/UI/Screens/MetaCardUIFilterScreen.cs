using Battles;
using Battles.UI;
using Cards;
using Map.UI;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using UI.Meta.Laboratory;
using UnityEngine;

public class MetaCardUIFilterScreen : UIFilterScreen<MetaCardUIHandler, Card>
{

    public CardUIEvent OnCardUse;
    public CardUIEvent OnCardRemove;
    public CardUIEvent OnCardInfo;
    public CardUIEvent OnCardDismental;

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
            card.MetaCardUIOpenerAbst = _metaCardUI;
            card.MetaCardUIFilterScreen = this;
            _collection.Add(card);
        }
    }

    protected override void OnActivate(IEnumerable<Card> sortedDeck, int i)
    {
        _collection[i].CardUI.GFX.SetCardReference(
            sortedDeck.ElementAt(i),
            Factory.GameFactory.Instance.ArtBlackBoard
            );
        _collection[i].transform.localScale = Vector3.one * _metaCardSize;

    }
 
    public void OnCardRemoveSelected(CardUI card) => OnCardRemove?.Invoke(card);
    public void OnCardUseSelected(CardUI card) => OnCardUse?.Invoke(card);
    public void OnCardDismentalSelected(CardUI card) => OnCardDismental?.Invoke(card);
    public void OnCardInfoSelected (CardUI card) => OnCardInfo?.Invoke(card);


}
