using Battles;
using Battles.UI;
using Cards;
using Map.UI;
using System.Collections.Generic;
using System.Linq;
using UI.Meta.Laboratory;
using UnityEngine.Events;

public class MetaCardUIFilterScreen : UIFilterScreen<MetaCardUIHandler, Card>
{

    public UnityEvent<CardUI> OnCardUse;
    public UnityEvent<CardUI> OnCardInfo;
    public UnityEvent<CardUI> OnCardDismental;
    protected override void CreatePool()
    {
        var deck = BattleData.Player.CharacterData.CharacterDeck;
        while (deck.Length > _collection.Count)
        {
            var card = Instantiate(_cardUIPrefab, this.transform).GetComponent<MetaCardUIHandler>();
            _collection.Add(card);
        }
    }

    protected override void OnActivate(IEnumerable<Card> sortedDeck, int i)
    {
        _collection[i].CardUI.GFX.SetCardReference(
            sortedDeck.ElementAt(i),
            Factory.GameFactory.Instance.ArtBlackBoard
            );

    }

    private void OnCardSelected(CardUI card)
    {

    }

 
    private void Start()
    {
            
    }
}
